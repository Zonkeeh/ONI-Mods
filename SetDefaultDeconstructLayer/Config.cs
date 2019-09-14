using System;
using Newtonsoft.Json;

namespace SetDefaultDeconstructionLayer
{
    public class Config
    {
        [JsonProperty]
        public bool DefaultEveryTime { get; set; } = false;

        [JsonProperty]
        public string SelectedLayer { get; set; } = "Building";
    }
}
