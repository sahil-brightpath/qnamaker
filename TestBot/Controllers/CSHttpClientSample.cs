using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;


namespace TestBot.Controllers
{
    public class CSHttpClientSample
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{16c5a1e70abf426aa3b3f97604b292da}");

            var uri = "https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/create?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
                response = await client.PostAsync(uri, content);
            }

        }
    }
}


//namespace CSHttpClientSample
//{
//    static class Program
//    {
//        static void Main()
//        {
//            MakeRequest();
//            Console.WriteLine("Hit ENTER to exit...");
//            Console.ReadLine();
//        }

//        static async void MakeRequest()
//        {
//            var client = new HttpClient();
//            var queryString = HttpUtility.ParseQueryString(string.Empty);

//            // Request headers
//            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscription key}");

//            var uri = "https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/create?" + queryString;

//            HttpResponseMessage response;

//            // Request body
//            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

//            using (var content = new ByteArrayContent(byteData))
//            {
//                content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
//                response = await client.PostAsync(uri, content);
//            }

//        }
//    }
//}