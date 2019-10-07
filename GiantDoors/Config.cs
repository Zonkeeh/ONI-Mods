using System;
using Newtonsoft.Json;

namespace GiantsDoor
{
    public class Config
    {
        [JsonProperty]
        public float NormalSpeed { get; set; } = 1f;

        [JsonProperty]
        public float PoweredDoorUnpoweredSpeed { get; set; } = 0.9f;

        [JsonProperty]
        public float PoweredDoorPoweredSpeed { get; set; } = 1.2f;
    }
}
