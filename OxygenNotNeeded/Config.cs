using System;
using Newtonsoft.Json;

namespace RevealWholeMap
{
    public class Config
    {
        [JsonProperty]
        public bool ForceLoad { get; set; } = false;
    }
}
