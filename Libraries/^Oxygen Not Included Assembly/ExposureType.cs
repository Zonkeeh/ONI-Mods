// Decompiled with JetBrains decompiler
// Type: ExposureType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class ExposureType
{
  public string germ_id;
  public string sickness_id;
  public string infection_effect;
  public int exposure_threshold;
  public bool infect_immediately;
  public List<string> required_traits;
  public List<string> excluded_traits;
  public List<string> excluded_effects;
  public int base_resistance;
}
