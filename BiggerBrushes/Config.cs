using System;
using Newtonsoft.Json;

namespace BiggerBrushes
{
    public class Config
    {
        [JsonProperty]
        public int MinSize { get; set; } = 1;

        [JsonProperty]
        public int MaxSize { get; set; } = 25;

    }
}
