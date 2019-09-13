using System;
using Newtonsoft.Json;

namespace CritterProofDoors
{
    public class Config
    {
        [JsonProperty]
        // If enabled will treat all normal in game doors as 'critter proof'.
        public bool TreatDefaultDoorsAsCritterProof { get; set; } = false;

        [JsonProperty]
        // If enabled will change the textures of the added 'critter proof' doors to custom textures.
        public bool UseCustomTextures { get; set; } = true;

        [JsonProperty]
        public bool EnablePneumaticCritterProofDoor { get; set; } = true;

        [JsonProperty]
        public bool EnableCritterProofManualAirlock { get; set; } = true;

        [JsonProperty]
        public bool EnableCritterProofMechanisedAirlock { get; set; } = true;
    }
}
