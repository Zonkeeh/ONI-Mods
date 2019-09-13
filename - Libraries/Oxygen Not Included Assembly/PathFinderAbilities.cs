// Decompiled with JetBrains decompiler
// Type: PathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class PathFinderAbilities
{
  protected Navigator navigator;
  protected int prefabInstanceID;

  public PathFinderAbilities(Navigator navigator)
  {
    this.navigator = navigator;
  }

  public void Refresh()
  {
    this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
    this.Refresh(this.navigator);
  }

  protected abstract void Refresh(Navigator navigator);

  public abstract bool TraversePath(
    ref PathFinder.PotentialPath path,
    int from_cell,
    NavType from_nav_type,
    int cost,
    int transition_id,
    int underwater_cost);

  public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
  {
    return 0;
  }
}
