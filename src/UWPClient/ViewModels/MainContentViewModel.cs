//using BulkTest.Models;
//using Prism.Commands;
//using Prism.Windows.Navigation;
//using System.Collections.Generic;

//namespace UWPClient.ViewModels
//{
//    public class MainContentViewModel : BaseViewModel
//    {
//        API _selectedApi;
//        List<API> _apis;
//        List<Parameter> _parameters;
//        Parameter _selectedParameter;
//        int _selectedIndex;
//        readonly DelegateCommand _submitCommand;
//        public Parameter SelectedParameter { get;  }
//        public List<API> Apis { get { return _apis; } set { SetProperty(ref _apis, value); } }
//        public List<Parameter> Parameters { get { return _parameters; } set { SetProperty(ref _parameters, value); } }
//        public int SelectedIndex { get { return _selectedIndex; } set { SetProperty(ref _selectedIndex, value); UpdateDetailView(); } }
//        public DelegateCommand SubmitCommand => _submitCommand;
//        public Response Response => _selectedParameter?.Response ?? null;

//        public MainContentViewModel(INavigationService navigationService)
//        {
//            this.NavigationService = navigationService;

//            LoadApis();
//            _submitCommand = new DelegateCommand(async () =>
//            {
//                Parameter tmpParameter = SelectedParameter;
//                Request request = new Request
//                {
//                    Api = _selectedApi
//                };

//                request.Parameters = this.Parameters;
//                HttpGeneralRequest http = new HttpGeneralRequest(request);
//                Response response = await http.SendRequest();
//                tmpParameter.Response = response;
//                RaisePropertyChanged(nameof(Response));
//            });
//        }


//        private async void LoadApis()
//        {
//            Response json = await HttpGeneralRequest.GetJson(); //Utils.ReadFileRelativePath("s:\\swagger.json");

//            Path path = Newtonsoft.Json.JsonConvert.DeserializeObject<Path>(json.Body);
//            Apis = path.Paths;
//        }

//        private void UpdateDetailView()
//        {
//            if (SelectedIndex < 0 || SelectedIndex > Apis.Count) return;

//            _selectedApi = Apis[SelectedIndex];
//            Parameters = _selectedApi.Parameters;
//            RaisePropertyChanged(nameof(Response));
//        }
//    }
//}
