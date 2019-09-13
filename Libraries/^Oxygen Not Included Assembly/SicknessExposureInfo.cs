// Decompiled with JetBrains decompiler
// Type: SicknessExposureInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct SicknessExposureInfo
{
  public string sicknessID;
  public string sourceInfo;

  public SicknessExposureInfo(string id, string infection_source_info)
  {
    this.sicknessID = id;
    this.sourceInfo = infection_source_info;
  }
}
