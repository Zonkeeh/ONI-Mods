using System;
using Newtonsoft.Json;

namespace NoMoreFlooding
{
    public class Config
    {
        [JsonProperty]
        public bool GenerateFeritilzer { get; set; } = true;

        [JsonProperty]
        public float GenerationMultiplier { get; set; } = 1;

        [JsonProperty]
        public float ComposterEmitMass { get; set; } = 10f;
    }
}
