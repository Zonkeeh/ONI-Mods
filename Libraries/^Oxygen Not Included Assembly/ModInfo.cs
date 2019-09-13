// Decompiled with JetBrains decompiler
// Type: ModInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

[Serializable]
public struct ModInfo
{
  [JsonConverter(typeof (StringEnumConverter))]
  public ModInfo.Source source;
  [JsonConverter(typeof (StringEnumConverter))]
  public ModInfo.ModType type;
  public string assetID;
  public string assetPath;
  public bool enabled;
  public bool markedForDelete;
  public bool markedForUpdate;
  public string description;
  public ulong lastModifiedTime;

  public enum Source
  {
    Local,
    Steam,
    Rail,
  }

  public enum ModType
  {
    WorldGen,
    Scenario,
    Mod,
  }
}
