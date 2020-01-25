using BulkTest.Models;
using BulkTest.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UWPClient.Models;
using Windows.Storage;


namespace UWPClient
{
    public static class WorkspaceManager
    {
        static readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        static ApplicationDataContainer dataContainer = ApplicationData.Current.LocalSettings;

        public async static Task<Tuple<List<ApiItem>, string>> LoadWorkspaceItem(string name)
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync(name);

                string content = await FileIO.ReadTextAsync(sampleFile);
                List<ApiItem> Apis = Utils.ConvertXmlStringtoObject<List<ApiItem>>(content);
                string token = dataContainer.Values[$"{name}_token"]?.ToString() ?? "";
                return new Tuple<List<ApiItem>, string>(Apis, token);
            }
            catch (Exception)
            { }

            return null;

        }


        public async static Task SaveWorkspaceItem(List<ApiItem> workspace, string name, string token)
        {
            string content = Utils.ConvertObjectToXMLString(workspace);
            if (System.IO.File.Exists(localFolder.Path + $"\\{name}"))
                System.IO.File.Delete(localFolder.Path + $"\\{name}");

            StorageFile file = await localFolder.CreateFileAsync(name);
            if (token == null)
                token = "";
            dataContainer.Values[$"{name}_token"] = token;


            await FileIO.WriteTextAsync(file, content);
        }

        public async static Task<Tuple<List<ApiItem>, string>> LoadFromUrl(string url)
        {
            Response json = await HttpGeneralRequest.GetJson(url);

            Path path = Newtonsoft.Json.JsonConvert.DeserializeObject<Path>(json.Body);
            List<ApiItem> Apis = ApiItem.Convert(path);
            return new Tuple<List<ApiItem>, string>(Apis, path.Name);
        }

        public static void SaveWorkspace(Workspace workspace)
        {
            dataContainer.Values["workspace"] = Utils.Serialize(workspace);
        }

        public static void DeleteWorkspace(Workspace workspace, string name)
        {
            if (System.IO.File.Exists(localFolder.Path + $"\\{name}"))
                System.IO.File.Delete(localFolder.Path + $"\\{name}");

        }

        public static Workspace LoadWorkspace()
        {
            ApplicationDataContainer dataContainer = ApplicationData.Current.LocalSettings;
            Workspace workspace = Utils.Deserialize<Workspace>(dataContainer.Values["workspace"]?.ToString());
            return workspace;
        }

        public async static Task<string> SaveReport(string report)
        {
            string fileName = DateTime.Now.ToUniversalTime().ToFileTimeUtc().ToString();
            StorageFile file = await localFolder.CreateFileAsync(fileName + ".txt");

            await FileIO.WriteTextAsync(file, report);
            return localFolder.Path + $"\\{fileName}.txt";
        }

        public static async void OpenFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return;

            Windows.System.Launcher.LaunchFileAsync(await StorageFile.GetFileFromPathAsync(filePath));
        }
    }
}
