// Decompiled with JetBrains decompiler
// Type: Database.TechTreeTitles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Database
{
  public class TechTreeTitles : ResourceSet<TechTreeTitle>
  {
    public TechTreeTitles(ResourceSet parent)
      : base("TreeTitles", parent)
    {
    }

    public void Load(TextAsset tree_file)
    {
      foreach (ResourceTreeNode node in (ResourceLoader<ResourceTreeNode>) new ResourceTreeLoader<ResourceTreeNode>(tree_file))
      {
        if (string.Equals(node.Id.Substring(0, 1), "_"))
        {
          TechTreeTitle techTreeTitle = new TechTreeTitle(node.Id, (ResourceSet) this, (string) Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + node.Id.ToUpper()), node);
        }
      }
    }
  }
}
