using Newtonsoft.Json;

namespace Trashcans
{
    public class Config
    {
        [JsonProperty]
        public bool RequiresPower { get; set; } = true;
        [JsonProperty]
        public bool CanOverheat { get; set; } = true;
        [JsonProperty]
        public bool CanFlood { get; set; } = false;
        [JsonProperty]
        public float EnergyConsumptionWhenActive { get; set; } = 60f;

        [JsonProperty]
        public bool EnableAutoTrash { get; set; } = true;
        [JsonProperty]
        public float MaxAutoTrashInterval { get; set; } = 3600f;

        [JsonProperty]
        public float GasTrashCapicityKG { get; set; } = 100f;
        [JsonProperty]
        public float LiquidTrashCapicityKG { get; set; } = 1000f;
        [JsonProperty]
        public float SolidTrashCapicityKG { get; set; } = 1000f;
    }
}
