using Newtonsoft.Json;

namespace WirelessPower.Configuration
{
    public class WirelessPowerConfig
    {
        [JsonProperty]
        public int MaxNumberOfChannels { get; set; } = 10;
        [JsonProperty]
        public bool UseColourInStatusItems { get; set; } = false;
        [JsonProperty]
        public bool UseEnergyFalloff { get; set; } = true;
        [JsonProperty]
        public float CustomEnergyFalloffPercentage { get; set; } = 0.05f;
        [JsonProperty]
        public float BatteryCapacity { get; set; } = 40000f;
        [JsonProperty]
        public float BatteryJoulesLostPerSecond { get; set; } = 0.75f;
        [JsonProperty]
        public float DefaultTransfer { get; set; } = 200f;
        [JsonProperty]
        public float MinTransfer { get; set; } = 10f;
        [JsonProperty]
        public float MaxTransfer { get; set; } = 5000f;
        [JsonProperty]
        public bool BuildUsesOnlySteel { get; set; } = true;
        [JsonProperty]
        public int BatteryBuildTime { get; set; } = 60;
        [JsonProperty]
        public float BatteryMaterialCost { get; set; } = 400f;
        [JsonProperty]
        public int SenderReceiverBuildTime { get; set; } = 30;
        [JsonProperty]
        public float SenderReceiverMaterialCost { get; set; } = 200f;
    }
}
