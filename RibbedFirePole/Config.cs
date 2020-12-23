using Newtonsoft.Json;

namespace RibbedFirePole
{
    public class Config
    {
        [JsonProperty]
        public float ClimbSpeed { get; set; } = 1.2f;

        [JsonProperty]
        public float FallSpeed { get; set; } = 4.5f;
    }
}
