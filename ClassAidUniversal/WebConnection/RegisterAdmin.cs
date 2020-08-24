using ClassAidUniversal.Users;
using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
namespace ClassAidUniversal.WebConnection
{
    public class RegisterAdmin
    {
        public static string Register(Admin admin)
        {
            var client = new RestClient("https://localhost:44367/api/adminauthapi");
            var request = new RestRequest();
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(JsonConvert.SerializeObject(admin));
            var response = client.Post(request);
            return response.Content; // Raw content as string
            
        }
    }
}
