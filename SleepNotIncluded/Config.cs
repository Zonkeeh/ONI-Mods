using System;
using Newtonsoft.Json;

namespace SleepNotIncluded
{
    public class Config
    {
        [JsonProperty]
        public bool ForceLoad { get; set; } = false;
    }
}
