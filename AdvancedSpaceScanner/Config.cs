using System;
using Newtonsoft.Json;
using TUNING;

namespace AdvancedSpaceScanner
{
    public class Config
    {
        [JsonProperty]
        public float RefinedMetalCost { get; set; } = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0];
        [JsonProperty]
        public float GlassCost { get; set; } = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0];
        [JsonProperty]
        public bool DoesRegolithSpawn { get; set; } = false;
        [JsonProperty]
        public bool IsOverheatable { get; set; } = false;
        [JsonProperty]
        public int MinimumWarningTime { get; set; } = 40;
        [JsonProperty]
        public int MaximumWarningTime { get; set; } = 300;
        [JsonProperty]
        public int InterferenceRadius { get; set; } = 4;
    }
}
