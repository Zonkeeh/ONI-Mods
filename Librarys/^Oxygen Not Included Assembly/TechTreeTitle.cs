// Decompiled with JetBrains decompiler
// Type: TechTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TechTreeTitle : Resource
{
  public string desc;
  private ResourceTreeNode node;

  public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node)
    : base(id, parent, name)
  {
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
}
