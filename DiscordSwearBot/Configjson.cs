using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordSwearBot
{
    public struct Configjson
    {
        [JsonProperty("prefixes")]
        public string[] Prefix { get; private set; }    

        [JsonProperty("dbSettings")]
        public DbSettings dbSettings { get; private set; }
    }
}
