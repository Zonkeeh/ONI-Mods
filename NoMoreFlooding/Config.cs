using System;
using Newtonsoft.Json;

namespace NoMoreFlooding
{
    public class Config
    {
        [JsonProperty]
        public bool GenerateFeritilzer { get; set; } = true;
    }
}
