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
                fileType.ToString()+".nsdl");
            FileStream fileStream = new FileStream(fileName,
                                       FileMode.OpenOrCreate,
                                       FileAccess.Write,
                                       FileShare.ReadWrite);
            var sm = new StreamWriter(fileStream);
            await Task.Run(() =>
            sm.Write(JsonConvert.SerializeObject(data)));
            sm.Close();
            fileStream.Close();
        }
        public async static Task<T> ReadDataAsync<T>(FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                fileType.ToString() + ".nsdl");
            
            if (File.Exists(fileName))
            {
                FileStream fileStream = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read,
                                       FileShare.ReadWrite);
                StreamReader sm = new StreamReader(fileStream);
                string result = null;
                await Task.Run(() => result = sm.ReadToEnd());
                sm.Close();
                fileStream.Close();
                return JsonConvert.DeserializeObject<T>(result);

            }
            else
            {
                throw new Exception("File Not Found");
            }
        }
        public async static void ClearData(FileType fileType)
        {
            fileName = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                fileType.ToString() + ".nsdl");
            if (File.Exists(fileName))
                await Task.Run(()=>File.Delete(fileName));
            else return;
        }
    }
    enum FileType
    {
        AuthFile,
        Schedule,
        Teacher,
        Student,
        Events,
        Shared,
        Admin
    }
}
