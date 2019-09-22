using System;
using Newtonsoft.Json;

namespace DuplicantLifecycle
{
    public class Config
{
    [JsonProperty]
    public bool ExampleConfigBool { get; set; } = false;
}
}
