// Decompiled with JetBrains decompiler
// Type: Gantry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class Gantry : Switch
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (Gantry);
  public static CellOffset[] TileOffsets = new CellOffset[2]
  {
    new CellOffset(-2, 1),
    new CellOffset(-1, 1)
  };
  public static CellOffset[] RetractableOffsets = new CellOffset[4]
  {
    new CellOffset(0, 1),
    new CellOffset(1, 1),
    new CellOffset(2, 1),
    new CellOffset(3, 1)
  };
  [MyCmpReq]
  private Building building;
  private Gantry.Instance smi;
  private static StatusItem infoStatusItem;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (Gantry.infoStatusItem == null)
    {
      Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem infoStatusItem = Gantry.infoStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (Gantry.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Gantry.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = Gantry.\u003C\u003Ef__mg\u0024cache0;
      infoStatusItem.resolveStringCallback = fMgCache0;
    }
    this.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    for (int index1 = 0; index1 < Gantry.TileOffsets.Length; ++index1)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(Gantry.TileOffsets[index1]);
      int index2 = Grid.OffsetCell(cell, rotatedOffset);
      SimMessages.ReplaceAndDisplaceElement(index2, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature, byte.MaxValue, 0, -1);
      Grid.Objects[index2, 1] = this.gameObject;
      Grid.Foundation[index2] = true;
      Grid.Objects[index2, 9] = this.gameObject;
      Grid.SetSolid(index2, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
      Grid.RenderedByWorld[index2] = false;
      World.Instance.OnSolidChanged(index2);
      GameScenePartitioner.Instance.TriggerEvent(index2, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    this.smi = new Gantry.Instance(this, this.IsSwitchedOn);
    this.smi.StartSM();
    this.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, (object) this.smi);
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM("cleanup");
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset tileOffset in Gantry.TileOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(tileOffset);
      int index = Grid.OffsetCell(cell1, rotatedOffset);
      SimMessages.ReplaceAndDisplaceElement(index, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0.0f, -1f, byte.MaxValue, 0, -1);
      Grid.Objects[index, 1] = (GameObject) null;
      Grid.Objects[index, 9] = (GameObject) null;
      Grid.Foundation[index] = false;
      Grid.SetSolid(index, false, CellEventLogger.Instance.SimCellOccupierDestroy);
      Grid.RenderedByWorld[index] = true;
      World.Instance.OnSolidChanged(index);
      GameScenePartitioner.Instance.TriggerEvent(index, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    }
    foreach (CellOffset retractableOffset in Gantry.RetractableOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(retractableOffset);
      int cell2 = Grid.OffsetCell(cell1, rotatedOffset);
      Grid.FakeFloor[cell2] = false;
      Pathfinding.Instance.AddDirtyNavGridCell(cell2);
    }
    base.OnCleanUp();
  }

  public void SetWalkable(bool active)
  {
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset retractableOffset in Gantry.RetractableOffsets)
    {
      CellOffset rotatedOffset = this.building.GetRotatedOffset(retractableOffset);
      int cell2 = Grid.OffsetCell(cell1, rotatedOffset);
      Grid.FakeFloor[cell2] = active;
      Pathfinding.Instance.AddDirtyNavGridCell(cell2);
    }
  }

  protected override void Toggle()
  {
    base.Toggle();
    this.smi.SetSwitchState(this.switchedOn);
  }

  protected override void OnRefreshUserMenu(object data)
  {
    if (this.smi.IsAutomated())
      return;
    base.OnRefreshUserMenu(data);
  }

  protected override void UpdateSwitchStatus()
  {
  }

  private static string ResolveInfoStatusItemString(string format_str, object data)
  {
    Gantry.Instance instance = (Gantry.Instance) data;
    return string.Format((string) (!instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL : BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL), (object) (string) (!instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.RETRACTED : BUILDING.STATUSITEMS.GANTRY.EXTENDED));
  }

  public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
  {
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;
    public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;
    public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.extended;
      this.serializable = true;
      this.retracted_pre.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(false))).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
      this.retracted.PlayAnim("off").ParamTransition<bool>((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.Parameter<bool>) this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
      this.extended_pre.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.SetActive(false))).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
      this.extended.Enter((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.master.SetWalkable(true))).Exit((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State.Callback) (smi => smi.master.SetWalkable(false))).PlayAnim("on").ParamTransition<bool>((StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.Parameter<bool>) this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse);
    }
  }

  public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
  {
    public bool logic_on = true;
    private Operational operational;
    public LogicPorts logic;
    private bool manual_on;

    public Instance(Gantry master, bool manual_start_state)
      : base(master)
    {
      this.manual_on = manual_start_state;
      this.operational = this.GetComponent<Operational>();
      this.logic = this.GetComponent<LogicPorts>();
      this.Subscribe(-592767678, new System.Action<object>(this.OnOperationalChanged));
      this.Subscribe(-801688580, new System.Action<object>(this.OnLogicValueChanged));
      this.smi.sm.should_extend.Set(true, this.smi);
    }

    public bool IsAutomated()
    {
      return this.logic.IsPortConnected(Gantry.PORT_ID);
    }

    public bool IsExtended()
    {
      if (this.IsAutomated())
        return this.logic_on;
      return this.manual_on;
    }

    public void SetSwitchState(bool on)
    {
      this.manual_on = on;
      this.UpdateShouldExtend();
    }

    public void SetActive(bool active)
    {
      this.operational.SetActive(this.operational.IsOperational && active, false);
    }

    private void OnOperationalChanged(object data)
    {
      this.UpdateShouldExtend();
    }

    private void OnLogicValueChanged(object data)
    {
      LogicValueChanged logicValueChanged = (LogicValueChanged) data;
      if (logicValueChanged.portID != Gantry.PORT_ID)
        return;
      this.logic_on = logicValueChanged.newValue != 0;
      this.UpdateShouldExtend();
    }

    private void UpdateShouldExtend()
    {
      if (!this.operational.IsOperational)
        return;
      if (this.IsAutomated())
        this.smi.sm.should_extend.Set(this.logic_on, this.smi);
      else
        this.smi.sm.should_extend.Set(this.manual_on, this.smi);
    }
  }
}
