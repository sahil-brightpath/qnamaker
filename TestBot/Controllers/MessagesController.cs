﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;

namespace TestBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        //static void Main()
        //{
        //    MakeRequest();
        //    Console.WriteLine("Hit ENTER to exit...");
        //    Console.ReadLine();
        //}

        static async void MakeRequest()
        {
         //   String result = MakePublishRequest();
            var client = new HttpClient();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "16c5a1e70abf426aa3b3f97604b292da");

            var uri = "https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/create";// + queryString;

            HttpResponseMessage response;
            //string Body = @'"{"name" : "My Knowledgebase", "urls": [ "http://www.seattle.gov/hala/faq" ]}"';
            //var serialized = JsonConvert.SerializeObject(Body);
            // Request body
            //string json = "{ \"name\": \"sendKnowledgebase\", " +
            //    " \"qnaPairs\": [" +
            //   "  \"answer\": \"ticket online, via your mobile device\"," +
            //" \"question\": \"How do I buy a timed  \" " +
            //   "]" +
            //        ",  \"urls\":  \"http://www.seattle.gov/hala/faq\"}";


           // string testjson = "{\"name\": \"sendKnowledgebase\"}";
            //var dict = new Dictionary<string, string>
            //{
            //     {"name", "Knowledgebase"},
            //    {"urls","http://www.seattle.gov/hala/faq"}

            //};

            //var serializer = new JavaScriptSerializer();
            //serializer.Serialize(dict);
            byte[] byteData = Encoding.UTF8.GetBytes(MakeQuestionBank());

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }

        }
        //public static async Task<string> MakePublishRequest()
        //{
        //    string ret = string.Empty;
        //    StreamWriter requestWriter;
        //    var webRequest = System.Net.WebRequest.Create("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/create") as HttpWebRequest;
        //    if (webRequest != null)
        //    {
        //        webRequest.Method = "POST";
        //        webRequest.ServicePoint.Expect100Continue = false;
        //       // webRequest.Timeout = 20000;
        //        webRequest.Headers.Add("Ocp-Apim-Subscription-Key", "16c5a1e70abf426aa3b3f97604b292da");
        //        webRequest.ContentType = "application/json";
        //        //POST the data.
        //        using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
        //        {
        //           // string testjson = "{\"name\": \"sendKnowledgebase\"}";
        //            requestWriter.Write(MakeQuestionBank());
        //        }
        //    }

        //    HttpWebResponse resp = (HttpWebResponse) await webRequest.GetResponseAsync();
          
        //    Stream resStream = resp.GetResponseStream();
        //    StreamReader reader = new StreamReader(resStream);
        //    ret = reader.ReadToEnd();
        //    return ret;
        //}
        //public static async Task<string> MakePublishRequest()
        //{
        //    string url = "http://www.seattle.gov/hala/faq";
        //    var client = new HttpClient();

        //    HttpResponseMessage response;

        //    HttpContent httpContent = new StringContent("hi", Encoding.UTF8, "application/json");
        //    httpContent.Headers.Add("Ocp-Apim-Subscription-Key", "16c5a1e70abf426aa3b3f97604b292da");
        //    Console.WriteLine("Going to call: " + url);
        //    response = await client.PutAsync(new Uri(url), httpContent);
        //    response.StatusCode.ToString();
        //    Console.WriteLine("QNAMaker MakePublishRequest statuscode: " + response.StatusCode.ToString());
        //    Console.WriteLine("QNAMaker MakePublishRequest response: " + response.ToString());
        //    return response.StatusCode.ToString();

        //}

        public static async Task<string> RequestAsync<T>(string input)
        {
            //var strEscaped = Uri.EscapeDataString(input);
            //var url = $"https://api.projectoxford.ai/luis/v1/application?id={id}&subscription-key={key}&q={strEscaped}";


            string responseString = string.Empty;

            string query = input; //User Query
            string knowledgebaseId = "6446ae5d-2685-462a-8163-03bb27dd650c"; // Use knowledge base id created.
            string qnamakerSubscriptionKey = "16c5a1e70abf426aa3b3f97604b292da"; //Use subscription key assigned to you.

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "text/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }
            return responseString;
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
          
          //  String result = await MakePublishRequest();
            if (activity.Type == ActivityTypes.Message)
            {
                MakeRequest();
                string getanswer=  await RequestAsync<string>(activity.Text);
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                Activity reply = activity.CreateReply(getanswer);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        public static string MakeQuestionBank()
        {
            QnaPair qna = new QnaPair() { question = "What is your name ?", answer = "My name is Raj" };

            AddToKB questionbank = new AddToKB() { name = "Kbtest8883",qnaPairs=new List<QnaPair>(),urls=new List<string>() };
            questionbank.qnaPairs.Add(qna);
            questionbank.urls.Add("http://www.seattle.gov/hala/faq");


            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(questionbank);


        }


    }
    public class QnaPair
    {

        [JsonProperty("answer")]
        public string answer { get; set; }

        [JsonProperty("question")]
        public string question { get; set; }
    }

    public class AddToKB
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("qnaPairs")]
        public IList<QnaPair> qnaPairs { get; set; }

        [JsonProperty("urls")]
        public IList<string> urls { get; set; }
    }
}