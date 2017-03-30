using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBot.Models
{
    public class QnaPair
    {
        [JsonProperty("answer")]
        public string answer { get; set; }

        [JsonProperty("question")]
        public string question { get; set; }
    }
}