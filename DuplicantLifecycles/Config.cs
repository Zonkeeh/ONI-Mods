using System;
using Newtonsoft.Json;

namespace DuplicantLifecycles
{
    public class Config
    {
        [JsonProperty]
        public string Cutoff_Comment { get; set; } = "Age cutoffs represent the age at which seperation occurs between different categories.";
        [JsonProperty]
        public float MaxDuplicantAge { get; set; } = 200f;

        [JsonProperty]
        public float YouthfulAgeCutoff { get; set; } = 30f;
        [JsonProperty]
        public float MiddleAgedAgeCutoff { get; set; } = 140f;
        [JsonProperty]
        public float ElderlyAgeCutoff { get; set; } = 180f;


        [JsonProperty]
        public string Logic_Comment { get; set; } = "Below values represent logic customisation. This includes wether a base value should be used for the multipliers to add/remove from or whether the duplicants total stat should be used.";
        [JsonProperty]
        public bool UsePercentageOfTotalStats { get; set; } = true;
        [JsonProperty]
        public float CustomStatBaseValue { get; set; } = 4f;
        [JsonProperty]
        public float TimeToDieProbabilityDecrease { get; set; } = 0;
        [JsonProperty]
        public bool EnabledDeathFromAge { get; set; } = true;
        [JsonProperty]
        public bool EnableImmortalTrait { get; set; } = true;


        [JsonProperty]
        public string Multiplier_Comment { get; set; } = "These multipliers are either applied to the base value or the attributes value. Eg if the base value is 5 a multiplier of 0.5 would cause an addition to an attribute of 2.5";
        [JsonProperty]
        public float YouthAttributeMultiplier { get; set; } = 1f;
        [JsonProperty]
        public float MiddleAgedAttributeMultiplier { get; set; } = 0.35f;
        [JsonProperty]
        public float ElderlyAttributeMultiplier { get; set; } = -0.35f;
        [JsonProperty]
        public float DyingAttributeMultiplier { get; set; } = -1f;
        [JsonProperty]
        public float ImmortalAttributeMultiplier { get; set; } = 0.1f;
    }
}
