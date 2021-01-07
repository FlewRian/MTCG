using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Player
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password {get; set; }
    }
}