// Decompiled with JetBrains decompiler
// Type: Tech
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Tech : Resource
{
  public List<Tech> requiredTech = new List<Tech>();
  public List<Tech> unlockedTech = new List<Tech>();
  public List<TechItem> unlockedItems = new List<TechItem>();
  public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();
  public int tier;
  public string desc;
  private ResourceTreeNode node;

  public Tech(string id, ResourceSet parent, string name, string desc, ResourceTreeNode node)
    : base(id, parent, name)
  {
    this.desc = desc;
    this.node = node;
  }

  public Vector2 center
  {
    get
    {
      return this.node.center;
    }
  }

  public float width
  {
    get
    {
      return this.node.width;
    }
  }

  public float height
  {
    get
    {
      return this.node.height;
    }
  }

  public List<ResourceTreeNode.Edge> edges
  {
    get
    {
      return this.node.edges;
    }
  }

  public bool CanAfford(ResearchPointInventory pointInventory)
  {
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      if ((double) pointInventory.PointsByTypeID[keyValuePair.Key] < (double) keyValuePair.Value)
        return false;
    }
    return true;
  }

  public string CostString(ResearchTypes types)
  {
    string empty = string.Empty;
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      empty += string.Format("{0}:{1}", (object) types.GetResearchType(keyValuePair.Key).name.ToString(), (object) keyValuePair.Value.ToString());
      empty += "\n";
    }
    return empty;
  }

  public bool IsComplete()
  {
    if (!((Object) Research.Instance != (Object) null))
      return false;
    TechInstance techInstance = Research.Instance.Get(this);
    if (techInstance != null)
      return techInstance.IsComplete();
    return false;
  }

  public bool ArePrerequisitesComplete()
  {
    foreach (Tech tech in this.requiredTech)
    {
      if (!tech.IsComplete())
        return false;
    }
    return true;
  }
}
