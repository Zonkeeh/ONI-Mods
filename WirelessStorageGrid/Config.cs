using System;
using Newtonsoft.Json;

namespace WirelessStorageGrid
{
    public class Config
{
    [JsonProperty]
    public bool ExampleConfigBool { get; set; } = false;
}
}
