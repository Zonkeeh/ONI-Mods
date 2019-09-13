// Decompiled with JetBrains decompiler
// Type: SpaceHeater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : StateMachineComponent<SpaceHeater.StatesInstance>, IEffectDescriptor
{
  public float targetTemperature = 308.15f;
  public int radius = 2;
  private List<int> monitorCells = new List<int>();
  public float minimumCellMass;
  [SerializeField]
  private bool heatLiquid;
  [MyCmpReq]
  private Operational operational;

  public float TargetTemperature
  {
    get
    {
      return this.targetTemperature;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void SetLiquidHeater()
  {
    this.heatLiquid = true;
  }

  private SpaceHeater.MonitorState MonitorHeating(float dt)
  {
    this.monitorCells.Clear();
    GameUtil.GetNonSolidCells(Grid.PosToCell(this.transform.GetPosition()), this.radius, this.monitorCells);
    int num1 = 0;
    float num2 = 0.0f;
    for (int index = 0; index < this.monitorCells.Count; ++index)
    {
      if ((double) Grid.Mass[this.monitorCells[index]] > (double) this.minimumCellMass && (Grid.Element[this.monitorCells[index]].IsGas && !this.heatLiquid || Grid.Element[this.monitorCells[index]].IsLiquid && this.heatLiquid))
      {
        ++num1;
        num2 += Grid.Temperature[this.monitorCells[index]];
      }
    }
    if (num1 == 0)
      return this.heatLiquid ? SpaceHeater.MonitorState.NotEnoughLiquid : SpaceHeater.MonitorState.NotEnoughGas;
    return (double) num2 / (double) num1 >= (double) this.targetTemperature ? SpaceHeater.MonitorState.TooHot : SpaceHeater.MonitorState.ReadyToHeat;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance
  {
    public StatesInstance(SpaceHeater master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
  {
    public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;
    public SpaceHeater.States.OnlineStates online;
    private StatusItem statusItemUnderMassLiquid;
    private StatusItem statusItemUnderMassGas;
    private StatusItem statusItemOverTemp;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.offline;
      this.serializable = false;
      this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.statusItemOverTemp = new StatusItem("statusItemOverTemp", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.statusItemOverTemp.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
      {
        SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance) obj;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
      });
      this.offline.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State) this.online, (StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).DefaultState(this.online.heating).Update("spaceheater_online", (System.Action<SpaceHeater.StatesInstance, float>) ((smi, dt) =>
      {
        switch (smi.master.MonitorHeating(dt))
        {
          case SpaceHeater.MonitorState.ReadyToHeat:
            smi.GoTo((StateMachine.BaseState) this.online.heating);
            break;
          case SpaceHeater.MonitorState.TooHot:
            smi.GoTo((StateMachine.BaseState) this.online.overtemp);
            break;
          case SpaceHeater.MonitorState.NotEnoughLiquid:
            smi.GoTo((StateMachine.BaseState) this.online.undermassliquid);
            break;
          case SpaceHeater.MonitorState.NotEnoughGas:
            smi.GoTo((StateMachine.BaseState) this.online.undermassgas);
            break;
        }
      }), UpdateRate.SIM_4000ms, false);
      this.online.heating.Enter((StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Exit((StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false)));
      this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, (object) null);
      this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, (object) null);
      this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, (object) null);
    }

    public class OnlineStates : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
    {
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
    }
  }

  private enum MonitorState
  {
    ReadyToHeat,
    TooHot,
    NotEnoughLiquid,
    NotEnoughGas,
  }
}
