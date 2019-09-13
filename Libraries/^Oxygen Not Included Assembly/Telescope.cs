// Decompiled with JetBrains decompiler
// Type: Telescope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Telescope : Workable, OxygenBreather.IGasProvider, IEffectDescriptor, ISim200ms
{
  private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);
  public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition()
  {
    id = nameof (ContainsOxygen),
    sortOrder = 1,
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen) != (UnityEngine.Object) null)
  };
  public int clearScanCellRadius = 15;
  private Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);
  private OxygenBreather.IGasProvider workerGasProvider;
  private Operational operational;
  private float percentClear;
  private static StatusItem reducedVisibilityStatusItem;
  private static StatusItem noVisibilityStatusItem;
  private Storage storage;
  private Chore chore;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SpacecraftManager.instance.Subscribe(532901469, new System.Action<object>(this.UpdateWorkingState));
    Components.Telescopes.Add(this);
    if (Telescope.reducedVisibilityStatusItem == null)
    {
      Telescope.reducedVisibilityStatusItem = new StatusItem("SPACE_VISIBILITY_REDUCED", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      StatusItem visibilityStatusItem1 = Telescope.reducedVisibilityStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (Telescope.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Telescope.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(Telescope.GetStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = Telescope.\u003C\u003Ef__mg\u0024cache0;
      visibilityStatusItem1.resolveStringCallback = fMgCache0;
      Telescope.noVisibilityStatusItem = new StatusItem("SPACE_VISIBILITY_NONE", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      StatusItem visibilityStatusItem2 = Telescope.noVisibilityStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (Telescope.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Telescope.\u003C\u003Ef__mg\u0024cache1 = new Func<string, object, string>(Telescope.GetStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache1 = Telescope.\u003C\u003Ef__mg\u0024cache1;
      visibilityStatusItem2.resolveStringCallback = fMgCache1;
    }
    Telescope telescope = this;
    telescope.OnWorkableEventCB = telescope.OnWorkableEventCB + new System.Action<Workable.WorkableEvent>(this.OnWorkableEvent);
    this.operational = this.GetComponent<Operational>();
    this.storage = this.GetComponent<Storage>();
  }

  protected override void OnCleanUp()
  {
    Components.Telescopes.Remove(this);
    SpacecraftManager.instance.Unsubscribe(532901469, new System.Action<object>(this.UpdateWorkingState));
    base.OnCleanUp();
  }

  public void Sim200ms(float dt)
  {
    Extents extents = this.GetComponent<Building>().GetExtents();
    int x1 = Mathf.Max(0, extents.x - this.clearScanCellRadius);
    int x2 = Mathf.Min(extents.x + this.clearScanCellRadius);
    int y = extents.y + extents.height - 3;
    int num1 = x2 - x1 + 1;
    int cell1 = Grid.XYToCell(x1, y);
    int cell2 = Grid.XYToCell(x2, y);
    int num2 = 0;
    for (int index = cell1; index <= cell2; ++index)
    {
      if (Grid.ExposedToSunlight[index] >= (byte) 253)
        ++num2;
    }
    Operational component1 = this.GetComponent<Operational>();
    component1.SetFlag(Telescope.visibleSkyFlag, num2 > 0);
    bool on = num2 < num1;
    KSelectable component2 = this.GetComponent<KSelectable>();
    if (num2 > 0)
    {
      component2.ToggleStatusItem(Telescope.noVisibilityStatusItem, false, (object) null);
      component2.ToggleStatusItem(Telescope.reducedVisibilityStatusItem, on, (object) this);
    }
    else
    {
      component2.ToggleStatusItem(Telescope.noVisibilityStatusItem, true, (object) this);
      component2.ToggleStatusItem(Telescope.reducedVisibilityStatusItem, false, (object) null);
    }
    this.percentClear = (float) num2 / (float) num1;
    if (component1.IsActive || !component1.IsOperational || this.chore != null)
      return;
    this.chore = this.CreateChore();
    this.SetWorkTime(float.PositiveInfinity);
  }

  private static string GetStatusItemString(string src_str, object data)
  {
    Telescope telescope = (Telescope) data;
    return src_str.Replace("{VISIBILITY}", GameUtil.GetFormattedPercent(telescope.percentClear * 100f, GameUtil.TimeSlice.None)).Replace("{RADIUS}", telescope.clearScanCellRadius.ToString());
  }

  private void OnWorkableEvent(Workable.WorkableEvent ev)
  {
    Worker worker = this.worker;
    if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    OxygenBreather component1 = worker.GetComponent<OxygenBreather>();
    KPrefabID component2 = worker.GetComponent<KPrefabID>();
    switch (ev)
    {
      case Workable.WorkableEvent.WorkStarted:
        this.ShowProgressBar(true);
        this.progressBar.SetUpdateFunc((Func<float>) (() =>
        {
          if (SpacecraftManager.instance.HasAnalysisTarget())
            return SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE;
          return 0.0f;
        }));
        this.workerGasProvider = component1.GetGasProvider();
        component1.SetGasProvider((OxygenBreather.IGasProvider) this);
        component1.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
        component2.AddTag(GameTags.Shaded, false);
        break;
      case Workable.WorkableEvent.WorkStopped:
        component1.SetGasProvider(this.workerGasProvider);
        component1.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
        this.ShowProgressBar(false);
        component2.RemoveTag(GameTags.Shaded);
        break;
    }
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (SpacecraftManager.instance.HasAnalysisTarget())
    {
      float num1 = 1f + Db.Get().AttributeConverters.ResearchSpeed.Lookup((Component) worker).Evaluate();
      int analysisDestinationId = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
      float num2 = 1f / (float) SpacecraftManager.instance.GetDestination(analysisDestinationId).OneBasedDistance;
      float num3 = (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED / TUNING.ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY / 600f;
      float points = dt * num1 * num2 * num3;
      SpacecraftManager.instance.EarnDestinationAnalysisPoints(analysisDestinationId, points);
    }
    return base.OnWorkTick(worker, dt);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Oxygen);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(elementByHash.tag.ProperName(), string.Format((string) STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, (object) elementByHash.tag.ProperName()), Descriptor.DescriptorType.Requirement);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  protected Chore CreateChore()
  {
    WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    workChore.AddPrecondition(Telescope.ContainsOxygen, (object) null);
    return (Chore) workChore;
  }

  protected void UpdateWorkingState(object data)
  {
    bool flag = false;
    if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
      flag = true;
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected, !flag, (object) null);
    this.operational.SetFlag(this.flag, flag);
    if (flag || !(bool) ((UnityEngine.Object) this.worker))
      return;
    this.StopWork(this.worker, true);
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ShouldEmitCO2()
  {
    return false;
  }

  public bool ShouldStoreCO2()
  {
    return false;
  }

  public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
  {
    if (this.storage.items.Count <= 0)
      return false;
    GameObject gameObject = this.storage.items[0];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    bool flag = (double) component.Mass >= (double) amount;
    component.Mass = Mathf.Max(0.0f, component.Mass - amount);
    return flag;
  }
}
