using System;
using Newtonsoft.Json;

namespace DestructibleFeatures
{
    public class Config
    {
        [JsonProperty]
        public bool RemoveNeutronium { get; set; } = true;

        [JsonProperty]
        public bool ReplaceNeutroniumWithObsidian { get; set; } = true;

        [JsonProperty]
        public int AnaylsisTime { get; set; } = 3600;

        [JsonProperty]
        public int DeconstructTime { get; set; } = 1800;
    }
}
