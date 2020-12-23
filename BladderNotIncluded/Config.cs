using System;
using Newtonsoft.Json;

namespace BladderNotIncluded
{
    public class Config
    {
        [JsonProperty]
        public bool ForceLoad { get; set; } = false;
    }
}
