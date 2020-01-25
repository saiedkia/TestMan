using BulkTest;
using BulkTest.Models;
using BulkTest.Report;
using BulkTest.Utils;
using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPClient.ContentDialogs;
using UWPClient.Models;
using UWPClient.Views;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPClient.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        #region API/Workspace
        Workspace _workspace;
        public Workspace Workspace { get => _workspace; set { SetProperty(ref _workspace, value); } }

        public int _workspaceSelectedIndex = -1;
        public int _workspaceSelectedPreviousIndex = -1;
        public int WorkspaceSelectedIndex
        {
            get => _workspaceSelectedIndex;
            set
            {
                Task.Run(async () =>
                {
                    await MainPage.Invoker.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await SaveWorkspaceChanges();
                        SetProperty(ref _workspaceSelectedIndex, value < -1 ? -1 : value); UpdateWorkSpace();
                    });
                });
            }
        }

        public class ApiCategory : List<ApiItem>
        {
            public ApiCategory() { }
            public ApiCategory(List<ApiItem> items) { AddRange(items); }
            public string Key { get; set; }
        }

        public ObservableCollection<ApiCategory> _categorizedApi;
        CollectionViewSource _apiCollections;
        public CollectionViewSource ApiCollections { get => _apiCollections; }

        void cate()
        {
            _categorizedApi = new ObservableCollection<ApiCategory>();
            _apiCollections = new CollectionViewSource();
            foreach (ApiItem item in Apis)
            {
                ApiCategory aCategory = new ApiCategory()
                {
                    Key = item.Api.Category,
                };

                aCategory.AddRange(Apis.Where(x => x.Api.Category == item.Api.Category).ToList());
                _categorizedApi.Add(aCategory);
            }


            _apiCollections.Source = _categorizedApi.GroupBy(x => x.Key).Select(y => new ApiCategory(y.First().ToList()) { Key = y.Key });

            _apiCollections.IsSourceGrouped = true;
            RaisePropertyChanged(nameof(ApiCollections));
        }

        List<ApiItem> _apis;
        public List<ApiItem> Apis { get { return _apis; } set { SetProperty(ref _apis, value); cate(); } }

        public ApiItem SelectedApiItem { get => SelectedApiIndex >= 0 ? Apis?[SelectedApiIndex] : null; }

        int _selectedApiIndex = -1;
        public int SelectedApiIndex { get { return _selectedApiIndex; } set { SetProperty(ref _selectedApiIndex, value); UpdateDetailView(); } }
        #endregion

        #region Requests
        ObservableCollection<RequestParameterBindable> _requests;
        public ObservableCollection<RequestParameterBindable> Requests { get { return _requests; } set { SetProperty(ref _requests, value); } }


        public RequestParameterBindable SelectedRequest { get => SelectedRequestIndex >= 0 && _requests.Count > 0 ? _requests[SelectedRequestIndex] : null; }

        public List<RequestParameterBindable> SelectedRequests { get; set; }

        int _selectedRequestIndex = -1;
        public int SelectedRequestIndex
        {
            get => _selectedRequestIndex;
            set
            {
                if (value == -1 && Requests.Count > 0) return;
                SetProperty(ref _selectedRequestIndex, value);

                if (!SelectedParameters.Equals(_prevSelectedParameters))
                {
                    RaisePropertyChanged(nameof(SelectedParameters));
                    _prevSelectedParameters = SelectedParameters;
                }
                RaisePropertyChanged(nameof(Response));
                RaisePropertyChanged(nameof(ResponseExpectedStatusCodeIndex));
                RaisePropertyChanged(nameof(RequestEditMode));

                if (value != -1)
                    RaisePropertyChanged(nameof(RawValue));
            }
        }

        bool _prevRequestEditMode;
        public bool RequestEditMode
        {
            get
            {
                if (Requests?.Count > 0 && SelectedRequest == null) return _prevRequestEditMode;

                return SelectedRequest == null || SelectedRequest.EditMode == EditMode.Form ? false : true;
            }
            set
            {
                _prevRequestEditMode = value;
                if (SelectedRequest == null) return; SelectedRequest.EditMode = value ? EditMode.Raw : EditMode.Form; RaisePropertyChanged(nameof(RequestEditMode)); RaisePropertyChanged(nameof(SelectedParameters));
            }
        }

        public string RawValue { get => SelectedRequest?.RawValue; set { if (SelectedRequest != null) SelectedRequest.RawValue = value; } }
        #endregion

        #region Parameters

        List<ParameterToViewDto> _prevSelectedParameters;
        public List<ParameterToViewDto> SelectedParameters
        {
            get
            {
                if (SelectedRequest?.Parameters == null) return null;

                return ParameterToViewDto.ToList(SelectedRequest.EditMode == EditMode.Form ? SelectedRequest.Parameters : SelectedRequest.Parameters.Where(x => x.In != ParameterType.Body).ToList());
            }
        }

        public Response Response => SelectedRequest?.Response ?? null;
        #endregion

        #region Commands
        readonly DelegateCommand _submitCommand;
        readonly DelegateCommand _addCommand;
        readonly DelegateCommand _saveCommand;
        readonly DelegateCommand _settingCommand;
        readonly DelegateCommand _deleteCommand;
        readonly DelegateCommand _runCommand;
        readonly DelegateCommand _reportCommand;
        readonly DelegateCommand<RequestParameterBindable> _deleteRequestCommand;
        public DelegateCommand SubmitCommand => _submitCommand;
        public DelegateCommand AddCommand => _addCommand;
        public DelegateCommand SaveCommand => _saveCommand;
        public DelegateCommand SettingCommand => _settingCommand;
        public DelegateCommand DeleteCommand => _deleteCommand;
        public DelegateCommand RunCommand => _runCommand;
        public DelegateCommand<RequestParameterBindable> DeleteRequestCommand => _deleteRequestCommand;
        public DelegateCommand ReportCommand => _reportCommand;
        #endregion

        #region Request StatusCode
        List<string> _responseExpectedStatusCodes = new List<string>() { "None", "200", "201", "400", "401", "403", "404", "500", "600", "601", "602", "603", "604", "605" };
        public List<string> ResponseExpectedStatusCodes { get => _responseExpectedStatusCodes; set => SetProperty(ref _responseExpectedStatusCodes, value); }

        int _responseExpectedStatusCodeIndex;
        public int ResponseExpectedStatusCodeIndex
        {
            get => _responseExpectedStatusCodes.IndexOf(SelectedRequest?.ResponseExpectedStatusCode?.ToString() ?? "None");
            set
            {
                SetProperty(ref _responseExpectedStatusCodeIndex, value);
                if (SelectedRequest == null) return;

                if (value > 0)
                    SelectedRequest.ResponseExpectedStatusCode = int.Parse(_responseExpectedStatusCodes[value]);
                else
                    SelectedRequest.ResponseExpectedStatusCode = null;
            }
        }

        #endregion

        bool isWorkSpaceChanged = false;

        public MainPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;

            LoadWorkspace();
            _submitCommand = new DelegateCommand(async () =>
            {
                if (SelectedRequest == null) return;

                await SubmitRequest(SelectedRequest);
                isWorkSpaceChanged = true;
            });


            _addCommand = new DelegateCommand(() =>
            {
                if (SelectedApiItem == null) return;

                RequestParameterBindable rparames = new RequestParameterBindable();
                rparames.Parameters = SelectedApiItem.Api.Parameters.Clone();
                SelectedApiItem.Parameters.Add(rparames);
                Requests.Add(rparames);

                isWorkSpaceChanged = true;
            });

            _saveCommand = new DelegateCommand(() =>
            {
                SaveWorkspaceItem();
                isWorkSpaceChanged = false;
            });

            _settingCommand = new DelegateCommand(() =>
            {
                TokenInput input = new TokenInput
                {
                    Value = Workspace.Token,
                    Callback = (_, token) =>
                    {
                        Workspace.Token = token;
                        isWorkSpaceChanged = true;
                    }
                };

                input.ShowAsync();
            });

            _deleteCommand = new DelegateCommand(async () =>
            {
                if (Apis?.Count > 1 && WorkspaceSelectedIndex >= 0)
                {
                    MessageDialog messageDialog = new MessageDialog("are you sure?");
                    messageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((command) =>
                    {

                    })));

                    messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((command) =>
                    {
                        DeleteApi();
                    })));

                    messageDialog.DefaultCommandIndex = 0;
                    await messageDialog.ShowAsync();
                }
                else
                {

                }
            });

            _deleteRequestCommand = new DelegateCommand<RequestParameterBindable>((request) =>
            {
                Requests.Remove(request);
                Apis[SelectedApiIndex].Parameters.Remove(request);
                isWorkSpaceChanged = true;
            });

            _runCommand = new DelegateCommand(() =>
            {
                if (Requests == null || Requests.Count < 1) return;

                if (SelectedRequests.Count > 0)
                    foreach (RequestParameterBindable item in SelectedRequests)
                        SubmitRequest(item);
                else
                    foreach (RequestParameterBindable item in Requests)
                        SubmitRequest(item);

                isWorkSpaceChanged = true;
            });


            _reportCommand = new DelegateCommand(async () =>
            {
                if (Requests == null) return;

                StringBuilder builder = new StringBuilder();
                foreach (RequestParameterBindable item in Requests)
                {
                    if (item.ResponseTime <= 0) continue;

                    string report = GenerateReport(item);
                    if (report.StartsWith("Invalid response/request!!") || report.Count() < 20) continue;

                    builder.AppendLine();
                    builder.AppendLine(report);
                    builder.AppendLine();
                    builder.AppendLine(" ----------------------------------- ");
                    builder.AppendLine();
                }

                string finalReport = builder.ToString();
                if (finalReport.Length > 50)
                {
                    string filePath = await WorkspaceManager.SaveReport(finalReport);
                    WorkspaceManager.OpenFile(filePath);
                }

            });
        }

        private string GenerateReport(RequestParameterBindable request)
        {
            Request _request = new Request
            {
                Api = SelectedApiItem.Api,
                Parameters = new List<Parameter>(request.Parameters.Append(request.ExtraParameters)),
                ContentType = ContentType.ApplicationJson
            };

            if (!string.IsNullOrEmpty(Workspace.Token))
                _request.Parameters.Add(
                    new Parameter()
                    {
                        Name = "Authorization",
                        Value = Workspace.Token.TrimLineFeed(),
                        In = ParameterType.Header
                    }
                );

            return ReportGenerator.Generate(request.Response, _request, request.ResponseTime, request.Name, request.RawValue);
        }

        private async Task SubmitRequest(RequestParameterBindable requestParameters)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (requestParameters.EditMode == EditMode.Raw && Utils.JsonIsValid(requestParameters.RawValue) == false)
            {
                MessageDialog m = new MessageDialog("json is invalid");
                m.ShowAsync();
                return;
            }


            RequestParameterBindable tmpParameter = requestParameters;
            Uri host = new Uri(SelectedApiItem.BaseUrl);
            string baseUrl = $"{host.Scheme}://{host.Host}:{host.Port}";
            tmpParameter.IsRunning = true;
            Request request = new Request
            {
                Api = SelectedApiItem.Api,
                Parameters = new List<Parameter>(tmpParameter.Parameters.Append(tmpParameter.ExtraParameters)),
                ContentType = ContentType.ApplicationJson,
                RawValue = requestParameters.EditMode == EditMode.Raw ? requestParameters.RawValue : null
            };


            if (!string.IsNullOrEmpty(Workspace.Token))
                request.Parameters.Add(
                    new Parameter()
                    {
                        Name = "Authorization",
                        Value = Workspace.Token.TrimLineFeed(),
                        In = ParameterType.Header
                    }
                );

            HttpGeneralRequest http = new HttpGeneralRequest(request);
            Response response = await http.SendRequest(baseUrl);
            tmpParameter.Response = response;
            watch.Stop();
            tmpParameter.ResponseTime = watch.ElapsedMilliseconds;

            RaisePropertyChanged(nameof(Requests));
            try
            {
                response.Body = Utils.Beautity(response.Body);
            }
            catch { }
            finally
            {
                tmpParameter.IsRunning = false;
            }

            RaisePropertyChanged(nameof(Response));
        }

        private void DeleteApi()
        {
            WorkspaceManager.DeleteWorkspace(Workspace, Workspace.Workspaces[WorkspaceSelectedIndex].Name);
            Workspace.Workspaces.RemoveAt(WorkspaceSelectedIndex);
            var v = new List<WorkspaceItem>() { Workspace.Workspaces.Last() };
            Workspace tmpWorkspace = new Workspace
            {
                Workspaces = Workspace.Workspaces.Except(v).ToList()
            };

            WorkspaceManager.SaveWorkspace(tmpWorkspace);
            Workspace = new Workspace(Workspace.Workspaces.ToList());
            WorkspaceSelectedIndex--;
        }

        private void LoadWorkspace()
        {
            Workspace = WorkspaceManager.LoadWorkspace();
            if (Workspace?.Workspaces == null) Workspace = new Workspace();

            Workspace.Workspaces.Add(new WorkspaceItem() { Name = "Browse", Path = null });
        }

        private void AddApiAndSaveWorkspace(string apiItemName)
        {
            if (this.Workspace.Workspaces.Where(x => x.Name == apiItemName).Count() == 0)
                this.Workspace.Workspaces.Insert(this.Workspace.Workspaces.Count - 1, new WorkspaceItem(apiItemName, apiItemName));

            var v = new List<WorkspaceItem>() { Workspace.Workspaces.Last() };
            Workspace tmpWorkspace = new Workspace
            {
                Workspaces = Workspace.Workspaces.Except(v).ToList()
            };

            WorkspaceManager.SaveWorkspace(tmpWorkspace);
            Workspace workspace = new Workspace(this.Workspace.Workspaces.ToList());
            this.Workspace = workspace;
        }

        private void UpdateDetailView()
        {
            if (SelectedApiIndex < 0 || SelectedApiIndex > Apis.Count) return;

            Requests = new ObservableCollection<RequestParameterBindable>(SelectedApiItem.Parameters);
            SelectedRequestIndex = 0;
        }

        #region SAVE/LOAD XML
        private void SaveWorkspaceItem()
        {
            if (WorkspaceSelectedIndex < 0) return;

            WorkspaceManager.SaveWorkspaceItem(Apis, Workspace.Workspaces[WorkspaceSelectedIndex].Name, Workspace.Token);
        }

        private async void UpdateWorkSpace()
        {
            if (_workspaceSelectedPreviousIndex == WorkspaceSelectedIndex) return;
            if (WorkspaceSelectedIndex == -1)
            {
                Apis = null;
                Requests = null;
            }
            else if (Workspace.Workspaces.Count == 1 || WorkspaceSelectedIndex == Workspace.Workspaces.Count - 1)
            {
                TokenInput input = new TokenInput("JsonUrl:")
                {
                    WaitForMyCommand = true,
                    Callback = async (inputDialog, url) =>
                    {
                        try
                        {
                            Tuple<List<ApiItem>, string> tmpApi = await WorkspaceManager.LoadFromUrl(url);
                            foreach (ApiItem item in tmpApi.Item1) item.BaseUrl = url;
                            await WorkspaceManager.SaveWorkspaceItem(tmpApi.Item1, tmpApi.Item2, null);
                            AddApiAndSaveWorkspace(tmpApi.Item2);
                            WorkspaceSelectedIndex = Workspace.Workspaces.Count - 2;
                        }
                        catch
                        {
                            if (inputDialog.WaitForMyCommand)
                                inputDialog.Close();
                            await (new MessageDialog("invlaid json/url")).ShowAsync();
                        }
                        finally
                        {
                            if (inputDialog.WaitForMyCommand)
                                inputDialog.Close();
                        }
                    }
                };

                await input.ShowAsync();
            }

            if (WorkspaceSelectedIndex == Workspace.Workspaces.Count - 1)
            {
                WorkspaceSelectedIndex = _workspaceSelectedPreviousIndex;
            }
            else
            {
                if (WorkspaceSelectedIndex >= 0 && _workspaceSelectedPreviousIndex != WorkspaceSelectedIndex)
                {

                    Tuple<List<ApiItem>, string> api = await WorkspaceManager.LoadWorkspaceItem(Workspace.Workspaces[WorkspaceSelectedIndex].Path);

                    Apis = api?.Item1;
                    Workspace.Token = api?.Item2;
                }

                _workspaceSelectedPreviousIndex = WorkspaceSelectedIndex;
            }
        }
        #endregion


        public async void RequestRename()
        {
            TokenInput input = new TokenInput
            {
                Title = $"Rename '{SelectedRequest.Name}'",
                Callback = (tokenInput, value) =>
                {
                    string tmpValue = value.TrimLineFeed();
                    if (tmpValue.Count() > 0)
                        SelectedRequest.Name = tmpValue;
                }
            };

            await input.ShowAsync();
        }

        public async Task SaveWorkspaceChanges()
        {
            if (!isWorkSpaceChanged) return;

            MessageDialog dialog = new MessageDialog("save changes???");
            dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((command) =>
            {
                isWorkSpaceChanged = false;
            })));

            dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((command) =>
            {
                SaveWorkspaceItem();
                isWorkSpaceChanged = false;
            })));


            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
        }
    }
}

