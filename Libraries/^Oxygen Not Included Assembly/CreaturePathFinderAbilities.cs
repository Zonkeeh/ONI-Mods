// Decompiled with JetBrains decompiler
// Type: CreaturePathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class CreaturePathFinderAbilities : PathFinderAbilities
{
  public int maxUnderwaterCost;

  public CreaturePathFinderAbilities(Navigator navigator)
    : base(navigator)
  {
  }

  protected override void Refresh(Navigator navigator)
  {
    if (PathFinder.IsSubmerged(Grid.PosToCell((KMonoBehaviour) navigator)))
    {
      this.maxUnderwaterCost = int.MaxValue;
    }
    else
    {
      AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup((Component) navigator);
      this.maxUnderwaterCost = attributeInstance == null ? int.MaxValue : (int) attributeInstance.GetTotalValue();
    }
  }

  public override bool TraversePath(
    ref PathFinder.PotentialPath path,
    int from_cell,
    NavType from_nav_type,
    int cost,
    int transition_id,
    int underwater_cost)
  {
    return underwater_cost <= this.maxUnderwaterCost;
  }
}
