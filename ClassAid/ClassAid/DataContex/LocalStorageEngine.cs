using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClassAid.DataContex
{
    class LocalStorageEngine
    {
        private static string fileName = "";
        public async static void SaveDataAsync<T>(T data, FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), 
                ((int)fileType).ToString(), ".nsdl");
            await Task.Run(() =>
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data)));
        }
        public async static Task<T> ReadDataAsync<T>(T data, FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                ((int)fileType).ToString(), ".nsdl");
            if (File.Exists(fileName))
            {
                string result = null;
                await Task.Run(() => result = File.ReadAllText(fileName));
                Task.WaitAll();
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception("File Not Found");
            }
        }
    }
    enum FileType
    {
        AuthFile,
        Schedule,
        Teacher,
        Student,
        Events
    }
}
