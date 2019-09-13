// Decompiled with JetBrains decompiler
// Type: EntombVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
  [MyCmpReq]
  private KSelectable selectable;
  private OccupyArea _occupyArea;
  [Serialize]
  private bool isEntombed;
  private HandleVector<int>.Handle partitionerEntry;

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public bool GetEntombed
  {
    get
    {
      return this.isEntombed;
    }
  }

  public string WiltStateString
  {
    get
    {
      return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback((string) CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, (object) this.gameObject);
    }
  }

  public WiltCondition.Condition[] Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.Entombed
      };
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntombVulnerable), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.CheckEntombed();
    if (!this.isEntombed)
      return;
    this.Trigger(-1089732772, (object) true);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnSolidChanged(object data)
  {
    this.CheckEntombed();
  }

  private void CheckEntombed()
  {
    int cell = Grid.PosToCell(this.gameObject.transform.GetPosition());
    if (!Grid.IsValidCell(cell))
      return;
    if (!this.IsCellSafe(cell))
    {
      if (this.isEntombed)
        return;
      this.isEntombed = true;
      this.selectable.AddStatusItem(Db.Get().CreatureStatusItems.Entombed, (object) this.gameObject);
      this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
      this.Trigger(-1089732772, (object) true);
    }
    else
    {
      if (!this.isEntombed)
        return;
      this.isEntombed = false;
      this.selectable.RemoveStatusItem(Db.Get().CreatureStatusItems.Entombed, false);
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) false);
    }
  }

  public bool IsCellSafe(int cell)
  {
    OccupyArea occupyArea = this.occupyArea;
    int rootCell = cell;
    // ISSUE: reference to a compiler-generated field
    if (EntombVulnerable.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EntombVulnerable.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(EntombVulnerable.IsCellSafeCB);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = EntombVulnerable.\u003C\u003Ef__mg\u0024cache0;
    return occupyArea.TestArea(rootCell, (object) null, fMgCache0);
  }

  private static bool IsCellSafeCB(int cell, object data)
  {
    if (Grid.IsValidCell(cell))
      return !Grid.Solid[cell];
    return false;
  }
}
