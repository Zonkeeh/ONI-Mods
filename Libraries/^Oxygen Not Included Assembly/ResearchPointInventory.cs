// Decompiled with JetBrains decompiler
// Type: ResearchPointInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

public class ResearchPointInventory
{
  public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();

  public ResearchPointInventory()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
      this.PointsByTypeID.Add(type.id, 0.0f);
  }

  public void AddResearchPoints(string researchTypeID, float points)
  {
    if (!this.PointsByTypeID.ContainsKey(researchTypeID))
    {
      Debug.LogWarning((object) ("Research inventory is missing research point key " + researchTypeID));
    }
    else
    {
      Dictionary<string, float> pointsByTypeId;
      string index;
      (pointsByTypeId = this.PointsByTypeID)[index = researchTypeID] = pointsByTypeId[index] + points;
    }
  }

  public void RemoveResearchPoints(string researchTypeID, float points)
  {
    this.AddResearchPoints(researchTypeID, -points);
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
    {
      if (!this.PointsByTypeID.ContainsKey(type.id))
        this.PointsByTypeID.Add(type.id, 0.0f);
    }
  }
}
