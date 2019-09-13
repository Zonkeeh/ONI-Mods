// Decompiled with JetBrains decompiler
// Type: EmptyConduitWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class EmptyConduitWorkable : Workable
{
  private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>((System.Action<EmptyConduitWorkable, object>) ((component, data) => component.OnEmptyConduitCancelled(data)));
  [Serialize]
  private float elapsedTime = -1f;
  private bool emptiedPipe = true;
  [MyCmpReq]
  private Conduit conduit;
  private static StatusItem emptyLiquidConduitStatusItem;
  private static StatusItem emptyGasConduitStatusItem;
  private Chore chore;
  private const float RECHECK_PIPE_INTERVAL = 2f;
  private const float TIME_TO_EMPTY_PIPE = 4f;
  private const float NO_EMPTY_SCHEDULED = -1f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.SetWorkTime(float.PositiveInfinity);
    this.faceTargetWhenWorking = true;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.Subscribe<EmptyConduitWorkable>(2127324410, EmptyConduitWorkable.OnEmptyConduitCancelledDelegate);
    if (EmptyConduitWorkable.emptyLiquidConduitStatusItem == null)
    {
      EmptyConduitWorkable.emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit", (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, 66);
      EmptyConduitWorkable.emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit", (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, 130);
    }
    this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
    this.shouldShowSkillPerkStatusItem = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((double) this.elapsedTime == -1.0)
      return;
    this.MarkForEmptying();
  }

  public void MarkForEmptying()
  {
    if (this.chore != null)
      return;
    this.GetComponent<KSelectable>().ToggleStatusItem(this.GetStatusItem(), true, (object) null);
    this.CreateWorkChore();
  }

  private void CancelEmptying()
  {
    this.CleanUpVisualization();
    if (this.chore == null)
      return;
    this.chore.Cancel("Cancel");
    this.chore = (Chore) null;
    this.shouldShowSkillPerkStatusItem = false;
    this.UpdateStatusItem((object) null);
  }

  private void CleanUpVisualization()
  {
    StatusItem statusItem = this.GetStatusItem();
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.ToggleStatusItem(statusItem, false, (object) null);
    this.elapsedTime = -1f;
    if (this.chore == null)
      return;
    this.GetComponent<Prioritizable>().RemoveRef();
  }

  protected override void OnCleanUp()
  {
    this.CancelEmptying();
    base.OnCleanUp();
  }

  private ConduitFlow GetFlowManager()
  {
    return this.conduit.type != ConduitType.Gas ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
  }

  private void OnEmptyConduitCancelled(object data)
  {
    this.CancelEmptying();
  }

  private StatusItem GetStatusItem()
  {
    switch (this.conduit.type)
    {
      case ConduitType.Gas:
        return EmptyConduitWorkable.emptyGasConduitStatusItem;
      case ConduitType.Liquid:
        return EmptyConduitWorkable.emptyLiquidConduitStatusItem;
      default:
        throw new ArgumentException();
    }
  }

  private void CreateWorkChore()
  {
    this.GetComponent<Prioritizable>().AddRef();
    this.chore = (Chore) new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDoPlumbing.Id);
    this.elapsedTime = 0.0f;
    this.emptiedPipe = false;
    this.shouldShowSkillPerkStatusItem = true;
    this.UpdateStatusItem((object) null);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if ((double) this.elapsedTime == -1.0)
      return true;
    bool flag = false;
    this.elapsedTime += dt;
    if (!this.emptiedPipe)
    {
      if ((double) this.elapsedTime > 4.0)
      {
        this.EmptyPipeContents();
        this.emptiedPipe = true;
        this.elapsedTime = 0.0f;
      }
    }
    else if ((double) this.elapsedTime > 2.0)
    {
      if ((double) this.GetFlowManager().GetContents(Grid.PosToCell(this.transform.GetPosition())).mass > 0.0)
      {
        this.elapsedTime = 0.0f;
        this.emptiedPipe = false;
      }
      else
      {
        this.CleanUpVisualization();
        this.chore = (Chore) null;
        flag = true;
        this.shouldShowSkillPerkStatusItem = false;
        this.UpdateStatusItem((object) null);
      }
    }
    return flag;
  }

  public void EmptyPipeContents()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    ConduitFlow.ConduitContents conduitContents = this.GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
    this.elapsedTime = 0.0f;
    if ((double) conduitContents.mass <= 0.0 || conduitContents.element == SimHashes.Vacuum)
      return;
    IChunkManager instance;
    switch (this.conduit.type)
    {
      case ConduitType.Gas:
        instance = (IChunkManager) GasSourceManager.Instance;
        break;
      case ConduitType.Liquid:
        instance = (IChunkManager) LiquidSourceManager.Instance;
        break;
      default:
        throw new ArgumentException();
    }
    instance.CreateChunk(conduitContents.element, conduitContents.mass, conduitContents.temperature, conduitContents.diseaseIdx, conduitContents.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
  }

  public override float GetPercentComplete()
  {
    return Mathf.Clamp01(this.elapsedTime / 4f);
  }
}
