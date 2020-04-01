using System;
using Newtonsoft.Json;

namespace OxygenNotNeeded
{
    public class Config
    {
        [JsonProperty]
        public bool ForceLoad { get; set; } = false;
    }
}
