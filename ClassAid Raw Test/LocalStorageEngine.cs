using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClassAid_Raw_Test
{
    class LocalStorageEngine
    {
        private static string fileName = "";
        public async static void SaveDataAsync<T>(T data, FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), 
                ((int)fileType).ToString()+ ".nsdl");
            //await Task.Run(() =>
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
            Console.WriteLine("Writing Done. URI - " + fileName);
        }
        public static T ReadDataAsync<T>(FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                ((int)fileType).ToString()+ ".nsdl");
            if (File.Exists(fileName))
            {
                string result;
                result = File.ReadAllText(fileName);
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
