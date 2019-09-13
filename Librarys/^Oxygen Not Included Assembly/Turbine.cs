// Decompiled with JetBrains decompiler
// Type: Turbine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using System;
using UnityEngine;

public class Turbine : KMonoBehaviour
{
  private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");
  public float requiredMassFlowDifferential = 3f;
  public float activePercent = 0.75f;
  public float minActiveTemperature = 400f;
  public float emitTemperature = 300f;
  [Serialize]
  private byte diseaseIdx = byte.MaxValue;
  private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;
  public SimHashes srcElem;
  public float minEmitMass;
  public float maxRPM;
  public float rpmAcceleration;
  public float rpmDeceleration;
  public float minGenerationRPM;
  public float pumpKGRate;
  [Serialize]
  private float storedMass;
  [Serialize]
  private float storedTemperature;
  [Serialize]
  private int diseaseCount;
  [MyCmpGet]
  private Generator generator;
  [Serialize]
  private float currentRPM;
  private int[] srcCells;
  private int[] destCells;
  private Turbine.Instance smi;
  private static StatusItem inputBlockedStatusItem;
  private static StatusItem outputBlockedStatusItem;
  private static StatusItem insufficientMassStatusItem;
  private static StatusItem insufficientTemperatureStatusItem;
  private static StatusItem activeStatusItem;
  private static StatusItem spinningUpStatusItem;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;
  private MeterController meter;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> emitCallbackManager = Game.Instance.massEmitCallbackManager;
    // ISSUE: reference to a compiler-generated field
    if (Turbine.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Turbine.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<Sim.MassEmittedCallback, object> fMgCache0 = Turbine.\u003C\u003Ef__mg\u0024cache0;
    this.simEmitCBHandle = emitCallbackManager.Add(fMgCache0, (object) this, "TurbineEmit");
    BuildingDef def = this.GetComponent<BuildingComplete>().Def;
    this.srcCells = new int[def.WidthInCells];
    this.destCells = new int[def.WidthInCells];
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    for (int index1 = 0; index1 < def.WidthInCells; ++index1)
    {
      int x = index1 - (def.WidthInCells - 1) / 2;
      this.srcCells[index1] = Grid.OffsetCell(cell, new CellOffset(x, -1));
      this.destCells[index1] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
      int index2 = Grid.OffsetCell(cell, new CellOffset(x, 0));
      SimMessages.SetCellProperties(index2, (byte) 39);
      Grid.Foundation[index2] = true;
      Grid.SetSolid(index2, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[index2] = false;
      World.Instance.OnSolidChanged(index2);
      GameScenePartitioner.Instance.TriggerEvent(index2, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    this.smi = new Turbine.Instance(this);
    this.smi.StartSM();
    this.CreateMeter();
  }

  private void CreateMeter()
  {
    this.meter = new MeterController((KAnimControllerBase) this.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_OL",
      "meter_frame",
      "meter_fill"
    });
    this.smi.UpdateMeter();
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM("cleanup");
    BuildingDef def = this.GetComponent<BuildingComplete>().Def;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    for (int index1 = 0; index1 < def.WidthInCells; ++index1)
    {
      int x = index1 - (def.WidthInCells - 1) / 2;
      int index2 = Grid.OffsetCell(cell, new CellOffset(x, 0));
      SimMessages.ClearCellProperties(index2, (byte) 39);
      Grid.Foundation[index2] = false;
      Grid.SetSolid(index2, false, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[index2] = true;
      World.Instance.OnSolidChanged(index2);
      GameScenePartitioner.Instance.TriggerEvent(index2, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, nameof (Turbine));
    this.simEmitCBHandle.Clear();
    base.OnCleanUp();
  }

  private void Pump(float dt)
  {
    float mass = this.pumpKGRate * dt / (float) this.srcCells.Length;
    foreach (int srcCell in this.srcCells)
    {
      Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> consumedCallbackManager = Game.Instance.massConsumedCallbackManager;
      // ISSUE: reference to a compiler-generated field
      if (Turbine.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Turbine.\u003C\u003Ef__mg\u0024cache1 = new System.Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<Sim.MassConsumedCallback, object> fMgCache1 = Turbine.\u003C\u003Ef__mg\u0024cache1;
      HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = consumedCallbackManager.Add(fMgCache1, (object) this, "TurbineConsume");
      SimMessages.ConsumeMass(srcCell, this.srcElem, mass, (byte) 1, handle.index);
    }
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
  {
    ((Turbine) data).OnSimConsume(mass_cb_info);
  }

  private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
  {
    if ((double) mass_cb_info.mass <= 0.0)
      return;
    this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
    this.storedMass += mass_cb_info.mass;
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
    this.diseaseIdx = finalDiseaseInfo.idx;
    this.diseaseCount = finalDiseaseInfo.count;
    if ((double) this.storedMass <= (double) this.minEmitMass || !this.simEmitCBHandle.IsValid())
      return;
    float mass = this.storedMass / (float) this.destCells.Length;
    int disease_count = this.diseaseCount / this.destCells.Length;
    Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
    foreach (int destCell in this.destCells)
      SimMessages.EmitMass(destCell, mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
    this.storedMass = 0.0f;
    this.storedTemperature = 0.0f;
    this.diseaseIdx = byte.MaxValue;
    this.diseaseCount = 0;
  }

  private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
  {
    ((Turbine) data).OnSimEmitted(info);
  }

  private void OnSimEmitted(Sim.MassEmittedCallback info)
  {
    if (info.suceeded == (byte) 1)
      return;
    this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
    this.storedMass += info.mass;
    if (info.diseaseIdx == byte.MaxValue)
      return;
    SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo();
    diseaseInfo.idx = this.diseaseIdx;
    diseaseInfo.count = this.diseaseCount;
    SimUtil.DiseaseInfo a = diseaseInfo;
    diseaseInfo = new SimUtil.DiseaseInfo();
    diseaseInfo.idx = info.diseaseIdx;
    diseaseInfo.count = info.diseaseCount;
    SimUtil.DiseaseInfo b = diseaseInfo;
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
    this.diseaseIdx = finalDiseaseInfo.idx;
    this.diseaseCount = finalDiseaseInfo.count;
  }

  public static void InitializeStatusItems()
  {
    Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
    Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
    Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022);
    Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022);
    Turbine.activeStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Turbine turbine = (Turbine) data;
      str = string.Format(str, (object) (int) turbine.currentRPM);
      return str;
    });
    Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
    Turbine.insufficientMassStatusItem.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Turbine turbine = (Turbine) data;
      str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
      str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
      return str;
    });
    Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
    StatusItem temperatureStatusItem1 = Turbine.insufficientTemperatureStatusItem;
    // ISSUE: reference to a compiler-generated field
    if (Turbine.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Turbine.\u003C\u003Ef__mg\u0024cache2 = new Func<string, object, string>(Turbine.ResolveStrings);
    }
    // ISSUE: reference to a compiler-generated field
    Func<string, object, string> fMgCache2 = Turbine.\u003C\u003Ef__mg\u0024cache2;
    temperatureStatusItem1.resolveStringCallback = fMgCache2;
    StatusItem temperatureStatusItem2 = Turbine.insufficientTemperatureStatusItem;
    // ISSUE: reference to a compiler-generated field
    if (Turbine.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Turbine.\u003C\u003Ef__mg\u0024cache3 = new Func<string, object, string>(Turbine.ResolveStrings);
    }
    // ISSUE: reference to a compiler-generated field
    Func<string, object, string> fMgCache3 = Turbine.\u003C\u003Ef__mg\u0024cache3;
    temperatureStatusItem2.resolveTooltipCallback = fMgCache3;
  }

  private static string ResolveStrings(string str, object data)
  {
    Turbine turbine = (Turbine) data;
    str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
    str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
    return str;
  }

  public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
  {
    private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[2]
    {
      (HashedString) "working_pre",
      (HashedString) "working_loop"
    };
    public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;
    public Turbine.States.OperationalStates operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      Turbine.InitializeStatusItems();
      default_state = (StateMachine.BaseState) this.operational;
      this.serializable = true;
      this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Operational>().IsOperational)).QueueAnim("off", false, (Func<Turbine.Instance, string>) null).Enter((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.master.currentRPM = 0.0f;
        smi.UpdateMeter();
      }));
      this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Operational>().IsOperational)).Update("UpdateOperational", (System.Action<Turbine.Instance, float>) ((smi, dt) => smi.UpdateState(dt)), UpdateRate.SIM_200ms, false).Exit((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi => smi.DisableStatusItems()));
      this.operational.idle.QueueAnim("on", false, (Func<Turbine.Instance, string>) null);
      this.operational.spinningUp.ToggleStatusItem((Func<Turbine.Instance, StatusItem>) (smi => Turbine.spinningUpStatusItem), (Func<Turbine.Instance, object>) (smi => (object) smi.master)).QueueAnim("buildup", true, (Func<Turbine.Instance, string>) null);
      this.operational.active.Update("UpdateActive", (System.Action<Turbine.Instance, float>) ((smi, dt) => smi.master.Pump(dt)), UpdateRate.SIM_200ms, false).ToggleStatusItem((Func<Turbine.Instance, StatusItem>) (smi => Turbine.activeStatusItem), (Func<Turbine.Instance, object>) (smi => (object) smi.master)).Enter((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
        smi.GetComponent<Operational>().SetActive(true, false);
      })).Exit((StateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<Generator>().ResetJoules();
        smi.GetComponent<Operational>().SetActive(false, false);
      }));
    }

    public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
    {
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;
      public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
    }
  }

  public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
  {
    private Guid inputBlockedHandle = Guid.Empty;
    private Guid outputBlockedHandle = Guid.Empty;
    private Guid insufficientMassHandle = Guid.Empty;
    private Guid insufficientTemperatureHandle = Guid.Empty;
    public bool isInputBlocked;
    public bool isOutputBlocked;
    public bool insufficientMass;
    public bool insufficientTemperature;

    public Instance(Turbine master)
      : base(master)
    {
    }

    public void UpdateState(float dt)
    {
      float num = !this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? -this.master.rpmDeceleration : this.master.rpmAcceleration;
      this.master.currentRPM = Mathf.Clamp(this.master.currentRPM + dt * num, 0.0f, this.master.maxRPM);
      this.UpdateMeter();
      this.UpdateStatusItems();
      StateMachine.BaseState currentState = this.smi.GetCurrentState();
      if ((double) this.master.currentRPM >= (double) this.master.minGenerationRPM)
      {
        if (currentState != this.sm.operational.active)
          this.smi.GoTo((StateMachine.BaseState) this.sm.operational.active);
        this.smi.master.generator.GenerateJoules(this.smi.master.generator.WattageRating * dt, false);
      }
      else if ((double) this.master.currentRPM > 0.0)
      {
        if (currentState == this.sm.operational.spinningUp)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.sm.operational.spinningUp);
      }
      else
      {
        if (currentState == this.sm.operational.idle)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.sm.operational.idle);
      }
    }

    public void UpdateMeter()
    {
      if (this.master.meter == null)
        return;
      float percent_full = Mathf.Clamp01(this.master.currentRPM / this.master.maxRPM);
      this.master.meter.SetPositionPercent(percent_full);
      this.master.meter.SetSymbolTint((KAnimHashedString) Turbine.TINT_SYMBOL, (Color32) ((double) percent_full < (double) this.master.activePercent ? Color.red : Color.green));
    }

    private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
    {
      float a1 = 0.0f;
      float a2 = 0.0f;
      float a3 = float.PositiveInfinity;
      this.isInputBlocked = false;
      for (int index = 0; index < this.master.srcCells.Length; ++index)
      {
        int srcCell = this.master.srcCells[index];
        float b1 = Grid.Mass[srcCell];
        if (Grid.Element[srcCell].id == this.master.srcElem)
          a1 = Mathf.Max(a1, b1);
        float b2 = Grid.Temperature[srcCell];
        a2 = Mathf.Max(a2, b2);
        byte num = Grid.ElementIdx[srcCell];
        Element element = ElementLoader.elements[(int) num];
        if (element.IsLiquid || element.IsSolid)
          this.isInputBlocked = true;
      }
      this.isOutputBlocked = false;
      for (int index = 0; index < this.master.destCells.Length; ++index)
      {
        int destCell = this.master.destCells[index];
        float b = Grid.Mass[destCell];
        a3 = Mathf.Min(a3, b);
        byte num = Grid.ElementIdx[destCell];
        Element element = ElementLoader.elements[(int) num];
        if (element.IsLiquid || element.IsSolid)
          this.isOutputBlocked = true;
      }
      insufficient_mass = (double) a1 - (double) a3 < (double) this.master.requiredMassFlowDifferential;
      insufficient_temperature = (double) a2 < (double) this.master.minActiveTemperature;
      if (!insufficient_mass)
        return !insufficient_temperature;
      return false;
    }

    public void UpdateStatusItems()
    {
      KSelectable component = this.GetComponent<KSelectable>();
      this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
      this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
      this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
      this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
    }

    private Guid UpdateStatusItem(
      StatusItem item,
      bool show,
      Guid current_handle,
      KSelectable ksel)
    {
      Guid guid = current_handle;
      if (show != (current_handle != Guid.Empty))
        guid = !show ? ksel.RemoveStatusItem(current_handle, false) : ksel.AddStatusItem(item, (object) this.master);
      return guid;
    }

    public void DisableStatusItems()
    {
      KSelectable component = this.GetComponent<KSelectable>();
      component.RemoveStatusItem(this.inputBlockedHandle, false);
      component.RemoveStatusItem(this.outputBlockedHandle, false);
      component.RemoveStatusItem(this.insufficientMassHandle, false);
      component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
    }
  }
}
