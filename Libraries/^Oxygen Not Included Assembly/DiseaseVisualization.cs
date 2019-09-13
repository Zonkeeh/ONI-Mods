// Decompiled with JetBrains decompiler
// Type: DiseaseVisualization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseVisualization : ScriptableObject
{
  public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();
  public Sprite overlaySprite;

  public DiseaseVisualization.Info GetInfo(HashedString id)
  {
    foreach (DiseaseVisualization.Info info in this.info)
    {
      if (id == (HashedString) info.name)
        return info;
    }
    return new DiseaseVisualization.Info();
  }

  [Serializable]
  public struct Info
  {
    public string name;
    public Color32 overlayColour;

    public Info(string name)
    {
      this.name = name;
      this.overlayColour = (Color32) Color.red;
    }
  }
}
