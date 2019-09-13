// Decompiled with JetBrains decompiler
// Type: DrowningMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISim1000ms
{
  public bool canDrownToDeath = true;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Effects effects;
  private OccupyArea _occupyArea;
  [Serialize]
  [SerializeField]
  private float timeToDrown;
  [Serialize]
  private bool drowned;
  private bool drowning;
  protected const float MaxDrownTime = 75f;
  protected const float RegenRate = 5f;
  protected const float CellLiquidThreshold = 0.95f;
  public bool livesUnderWater;
  private Extents extents;
  private HandleVector<int>.Handle partitionerEntry;
  public static Effect drowningEffect;
  public static Effect saturatedEffect;

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public bool Drowning
  {
    get
    {
      return this.drowning;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.timeToDrown = 75f;
    if (DrowningMonitor.drowningEffect == null)
    {
      DrowningMonitor.drowningEffect = new Effect("Drowning", (string) CREATURES.STATUSITEMS.DROWNING.NAME, (string) CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0.0f, false, false, true, (string) null, 0.0f, (string) null);
      DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, (string) CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
    }
    if (DrowningMonitor.saturatedEffect != null)
      return;
    DrowningMonitor.saturatedEffect = new Effect("Saturated", (string) CREATURES.STATUSITEMS.SATURATED.NAME, (string) CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0.0f, false, false, true, (string) null, 0.0f, (string) null);
    DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, (string) CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnMove();
    this.CheckDrowning((object) null);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnMove), "DrowningMonitor.OnSpawn");
  }

  private void OnMove()
  {
    if (this.partitionerEntry.IsValid())
    {
      Extents extents = this.occupyArea.GetExtents();
      GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, extents.x, extents.y);
    }
    else
      this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.OnLiquidChanged));
    this.CheckDrowning((object) null);
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnMove));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void CheckDrowning(object data = null)
  {
    if (this.drowned)
      return;
    if (!this.IsCellSafe(Grid.PosToCell(this.gameObject.transform.GetPosition())))
    {
      if (!this.drowning)
      {
        this.drowning = true;
        this.Trigger(1949704522, (object) null);
        this.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
      }
      if ((double) this.timeToDrown <= 0.0 && this.canDrownToDeath)
      {
        this.GetSMI<DeathMonitor.Instance>()?.Kill(Db.Get().Deaths.Drowned);
        this.Trigger(-750750377, (object) null);
        this.drowned = true;
      }
    }
    else if (this.drowning)
    {
      this.drowning = false;
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
      this.Trigger(99949694, (object) null);
    }
    if (this.livesUnderWater)
      this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.drowning, (object) this);
    else
      this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowning, (object) this);
    if (!((UnityEngine.Object) this.effects != (UnityEngine.Object) null))
      return;
    if (this.drowning)
    {
      if (this.livesUnderWater)
        this.effects.Add(DrowningMonitor.saturatedEffect, false);
      else
        this.effects.Add(DrowningMonitor.drowningEffect, false);
    }
    else if (this.livesUnderWater)
      this.effects.Remove(DrowningMonitor.saturatedEffect);
    else
      this.effects.Remove(DrowningMonitor.drowningEffect);
  }

  private static bool CellSafeTest(int testCell, object data)
  {
    int cell = Grid.CellAbove(testCell);
    return Grid.IsValidCell(testCell) && Grid.IsValidCell(cell) && !Grid.IsSubstantialLiquid(testCell, 0.95f) && (!Grid.IsLiquid(testCell) || !Grid.Element[cell].IsLiquid && !Grid.Element[cell].IsSolid);
  }

  public bool IsCellSafe(int cell)
  {
    OccupyArea occupyArea = this.occupyArea;
    int rootCell = cell;
    // ISSUE: reference to a compiler-generated field
    if (DrowningMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DrowningMonitor.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(DrowningMonitor.CellSafeTest);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = DrowningMonitor.\u003C\u003Ef__mg\u0024cache0;
    return occupyArea.TestArea(rootCell, (object) this, fMgCache0);
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.Drowning
      };
    }
  }

  public string WiltStateString
  {
    get
    {
      if (this.livesUnderWater)
        return (string) CREATURES.STATUSITEMS.SATURATED.NAME;
      return (string) CREATURES.STATUSITEMS.DROWNING.NAME;
    }
  }

  private void OnLiquidChanged(object data)
  {
    this.CheckDrowning((object) null);
  }

  public void Sim1000ms(float dt)
  {
    this.CheckDrowning((object) null);
    if (this.drowning)
    {
      if (this.drowned)
        return;
      this.timeToDrown -= dt;
      if ((double) this.timeToDrown > 0.0)
        return;
      this.CheckDrowning((object) null);
    }
    else
    {
      this.timeToDrown += dt * 5f;
      this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0.0f, 75f);
    }
  }
}
