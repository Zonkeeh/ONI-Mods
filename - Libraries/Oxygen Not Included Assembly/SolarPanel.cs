// Decompiled with JetBrains decompiler
// Type: SolarPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolarPanel : Generator
{
  private static readonly EventSystem.IntraObjectHandler<SolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<SolarPanel>((System.Action<SolarPanel, object>) ((component, data) => component.OnActiveChanged(data)));
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
  private CellOffset[] solarCellOffsets = new CellOffset[14]
  {
    new CellOffset(-3, 2),
    new CellOffset(-2, 2),
    new CellOffset(-1, 2),
    new CellOffset(0, 2),
    new CellOffset(1, 2),
    new CellOffset(2, 2),
    new CellOffset(3, 2),
    new CellOffset(-3, 1),
    new CellOffset(-2, 1),
    new CellOffset(-1, 1),
    new CellOffset(0, 1),
    new CellOffset(1, 1),
    new CellOffset(2, 1),
    new CellOffset(3, 1)
  };
  private MeterController meter;
  private SolarPanel.StatesInstance smi;
  private Guid statusHandle;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SolarPanel>(824508782, SolarPanel.OnActiveChangedDelegate);
    this.smi = new SolarPanel.StatesInstance(this);
    this.smi.StartSM();
    this.accumulator = Game.Instance.accumulators.Add("Element", (KMonoBehaviour) this);
    BuildingDef def = this.GetComponent<BuildingComplete>().Def;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    for (int index1 = 0; index1 < def.WidthInCells; ++index1)
    {
      int x = index1 - (def.WidthInCells - 1) / 2;
      int index2 = Grid.OffsetCell(cell, new CellOffset(x, 0));
      SimMessages.SetCellProperties(index2, (byte) 39);
      Grid.Foundation[index2] = true;
      Grid.SetSolid(index2, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
      World.Instance.OnSolidChanged(index2);
      GameScenePartitioner.Instance.TriggerEvent(index2, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
      Grid.RenderedByWorld[index2] = false;
    }
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
  }

  protected override void OnCleanUp()
  {
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
      World.Instance.OnSolidChanged(index2);
      GameScenePartitioner.Instance.TriggerEvent(index2, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
      Grid.RenderedByWorld[index2] = true;
    }
    Game.Instance.accumulators.Remove(this.accumulator);
    base.OnCleanUp();
  }

  protected void OnActiveChanged(object data)
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !((Operational) data).IsActive ? Db.Get().BuildingStatusItems.GeneratorOffline : Db.Get().BuildingStatusItems.Wattage, (object) this);
  }

  private void UpdateStatusItem()
  {
    this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
    if (this.statusHandle == Guid.Empty)
    {
      this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.SolarPanelWattage, (object) this);
    }
    else
    {
      if (!(this.statusHandle != Guid.Empty))
        return;
      this.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.SolarPanelWattage, (object) this);
    }
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    ushort circuitId = this.CircuitID;
    this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
    if (!this.operational.IsOperational)
      return;
    float num1 = 0.0f;
    foreach (CellOffset solarCellOffset in this.solarCellOffsets)
    {
      int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), solarCellOffset)];
      num1 += (float) num2 * 0.00053f;
    }
    this.operational.SetActive((double) num1 > 0.0, false);
    float num3 = Mathf.Clamp(num1, 0.0f, 380f);
    Game.Instance.accumulators.Accumulate(this.accumulator, num3 * dt);
    if ((double) num3 > 0.0)
      this.GenerateJoules(Mathf.Max(num3 * dt, 1f * dt), false);
    this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 380f);
    this.UpdateStatusItem();
  }

  public float CurrentWattage
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.accumulator);
    }
  }

  public class StatesInstance : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.GameInstance
  {
    public StatesInstance(SolarPanel master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel>
  {
    public GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.State idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.DoNothing();
    }
  }
}
