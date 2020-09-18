using Firebase.Database;
using System;
using System.Collections.Generic;
using Firebase.Database.Query;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;
using ClassAid.Models.Users;
using System.Security.Cryptography;

namespace ClassAid.DataContex
{
    public class FireSharpDB
    {
        private FirebaseClient client;
        public FireSharpDB(string server, string password)
        {
            client = new FirebaseClient(
              server,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(password)
              });
            //jevexor737@cyberper.net
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Accepts Generic data type</typeparam>
        /// <param name="table">Name of the table</param>
        /// <param name="data">Generic type of data</param>
        /// <returns>Returns the unique key as a <c>string</c></returns>
        public async Task<string> InsertData<T>(string table, T data)
        {
            var res = await client.Child(table).PostAsync(data);
            //client.Child(table).PostAsync(data, false);
            return res.Key;
        }
        public IDisposable StreamChat<T>(string table)
        {
            IDisposable observable = client
              .Child(table)
              .AsObservable<T>()
              .Subscribe();
            return observable;
        }
        public FirebaseClient GetClient()
        {
            return client;
        }
    }
}
