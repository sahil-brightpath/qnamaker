using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBot.Models
{
    public class AddToKB
    {
        [JsonProperty("name")]
        public string name { get; set; }
          [JsonProperty("qnaPairs")]
          public IList<QnaPair> qnaPairs { get; set; }
        //  public string qnaPairs { get; set; }
        //public Dictionary<string, string> qnapair { get; set; }
        [JsonProperty("urls")]
        public IList<string> urls { get; set; }
    }
}