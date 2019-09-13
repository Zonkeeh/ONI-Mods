// Decompiled with JetBrains decompiler
// Type: KMod.Label
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace KMod
{
  [JsonObject(MemberSerialization.Fields)]
  [DebuggerDisplay("{title}")]
  public struct Label
  {
    public Label.DistributionPlatform distribution_platform;
    public string id;
    public long version;
    public string title;

    [JsonIgnore]
    private string distribution_platform_name
    {
      get
      {
        return this.distribution_platform.ToString();
      }
    }

    [JsonIgnore]
    public string install_path
    {
      get
      {
        return FileSystem.Normalize(Path.Combine(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name), this.id));
      }
    }

    [JsonIgnore]
    public DateTime time_stamp
    {
      get
      {
        return DateTime.FromFileTimeUtc(this.version);
      }
    }

    public override string ToString()
    {
      return this.title;
    }

    public bool Match(Label rhs)
    {
      if (this.id == rhs.id)
        return this.distribution_platform == rhs.distribution_platform;
      return false;
    }

    public enum DistributionPlatform
    {
      Local,
      Steam,
      Epic,
      Rail,
      Dev,
    }
  }
}
