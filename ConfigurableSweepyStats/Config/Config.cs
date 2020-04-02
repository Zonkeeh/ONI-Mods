using System;
using Newtonsoft.Json;

namespace ConfigurableSweepyStats
{
    public class Config
    {
        [JsonProperty]
        public bool DebugMode { get; set; } = false;


        [JsonProperty]
        public float StorageCapacity { get; set; } = 1000f;
        [JsonProperty]
        public bool SweepyUsesPower { get; set; } = true;
        [JsonProperty]
        public float BatteryDepletionRate { get; set; } = 40f;
        [JsonProperty]
        public float BatteryCapacity { get; set; } = 21000f;
        [JsonProperty]
        public float BaseMovementSpeed { get; set; } = 1f;
        [JsonProperty]
        public int BaseProbingRadius { get; set; } = 32;
        [JsonProperty]
        public bool BatteryDrainBasedOnSpeed { get; set; } = true;
        [JsonProperty]
        public float DrainSpeedMultiplier { get; set; } = 1f;


        [JsonProperty]
        public bool UseCustomSliders { get; set; } = true;
        [JsonProperty]
        public float MinSpeedSliderValue { get; set; } = 1f;
        [JsonProperty]
        public float MaxSpeedSliderValue { get; set; } = 5f;
        [JsonProperty]
        public float MinProbingSliderValue { get; set; } = 1f;
        [JsonProperty]
        public float MaxProbingSliderValue { get; set; } = 64f;


        [JsonProperty]
        public bool StationUsesPower { get; set; } = true;
        [JsonProperty]
        public bool StationCanOverheat { get; set; } = false;
        [JsonProperty]
        public bool StationCanFlood { get; set; } = false;
        [JsonProperty]
        public float StationEnergyConsumption { get; set; } = 240f;
        [JsonProperty]
        public float StationStorageCapacity { get; set; } = 3000f;
        [JsonProperty]
        public bool StationHasConveyorOutput { get; set; } = false;

    }
}
