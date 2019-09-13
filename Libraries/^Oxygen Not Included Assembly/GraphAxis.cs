// Decompiled with JetBrains decompiler
// Type: GraphAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct GraphAxis
{
  public string name;
  public float min_value;
  public float max_value;
  public float guide_frequency;

  public float range
  {
    get
    {
      return this.max_value - this.min_value;
    }
  }
}
