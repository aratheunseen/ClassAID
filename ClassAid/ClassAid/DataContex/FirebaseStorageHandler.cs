using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ClassAid.DataContex
{
    class FirebaseStorageHandler
    {
        public async static Task<string> SaveFile(Stream path)
        {
            //string documentsPath = 
            //    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //Debug.WriteLine(path);
            //Debug.WriteLine(documentsPath);
            //path = Path.Combine("/data/user/0/", path);
            //Debug.WriteLine(path);
            //var stream = File.Open(path, FileMode.Open);
            //FileStream fileStream = new FileStream(path, FileAccess.Read);
            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage("gs://classaidapp.appspot.com")
                .Child("data")
                .PutAsync(path);

            ////// Track progress of the upload
            ////task.Progress.ProgressChanged += (s, e) => 
            ////Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;
            return downloadUrl;
        }
    }
}
