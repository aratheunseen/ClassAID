using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;

namespace ClassAid.Models.Engines
{
    public class PushNotification
    {
        public static void Send(string title, string message, string key)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic ZDhmNDI4NWEtNWQ2My00OTQwLWJiOTMtNzU3NzU0MGI0ZmQ2");

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"7ab7ae00-d9e6-47cb-a4ed-f5045215fc9f\","
                                                    + "\"contents\": {\"en\": \"" + message + "\"},"
                                                    + "\"headings\": {\"en\": \"" + title + "\"}," 
                                                    +"\"included_segments\": [\"All\"]}");
            //byte[] byteArray = Encoding.UTF8.GetBytes("{"
            //                                        + "\"app_id\": \"7ab7ae00-d9e6-47cb-a4ed-f5045215fc9f\","
            //                                        + "\"contents\": {\"en\": \"" + message + "\"},"
            //                                        + "\"headings\": {\"en\": \"" + title + "\"},"
            //                                        + "\"filters\": [{\"field\": \"tag\", \"key\": \"AdminKey\"," +
            //                                        " \"relation\": \"=\", \"value\": \"" + key + "\"}]}");
            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException)
            {
                DependencyService.Get<Toast>().Show("Couldn't send Notifications to the end users.");
            }

        }
    }
}
