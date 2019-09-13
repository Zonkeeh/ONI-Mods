// Decompiled with JetBrains decompiler
// Type: Door
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Door : Workable, ISaveLoadable, ISim200ms
{
  private static readonly HashedString SOUND_POWERED_PARAMETER = (HashedString) "doorPowered";
  private static readonly HashedString SOUND_PROGRESS_PARAMETER = (HashedString) "doorProgress";
  private static readonly EventSystem.IntraObjectHandler<Door> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Door>((System.Action<Door, object>) ((component, data) => component.OnCopySettings(data)));
  public static readonly HashedString OPEN_CLOSE_PORT_ID = new HashedString("DoorOpenClose");
  private static readonly KAnimFile[] OVERRIDE_ANIMS = new KAnimFile[1]
  {
    Assets.GetAnim((HashedString) "anim_use_remote_kanim")
  };
  private static readonly EventSystem.IntraObjectHandler<Door> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Door>((System.Action<Door, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Door> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Door>((System.Action<Door, object>) ((component, data) => component.OnLogicValueChanged(data)));
  [SerializeField]
  public float unpoweredAnimSpeed = 0.25f;
  [SerializeField]
  public float poweredAnimSpeed = 1f;
  [SerializeField]
  public bool allowAutoControl = true;
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private Rotatable rotatable;
  [MyCmpReq]
  private KBatchedAnimController animController;
  [MyCmpReq]
  public Building building;
  [MyCmpGet]
  private EnergyConsumer consumer;
  [MyCmpAdd]
  private LoopingSounds loopingSounds;
  public Orientation verticalOrientation;
  [SerializeField]
  public bool hasComplexUserControls;
  [SerializeField]
  public Door.DoorType doorType;
  [SerializeField]
  public string doorClosingSoundEventName;
  [SerializeField]
  public string doorOpeningSoundEventName;
  private string doorClosingSound;
  private string doorOpeningSound;
  [Serialize]
  private bool hasBeenUnsealed;
  [Serialize]
  private Door.ControlState controlState;
  private bool on;
  private bool do_melt_check;
  private int openCount;
  private Door.ControlState requestedState;
  private Chore changeStateChore;
  private Door.Controller.Instance controller;
  private LoggerFSS log;
  private const float REFRESH_HACK_DELAY = 1f;
  private bool doorOpenLiquidRefreshHack;
  private float doorOpenLiquidRefreshTime;
  private bool applyLogicChange;

  public Door()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  private void OnCopySettings(object data)
  {
    Door component = ((GameObject) data).GetComponent<Door>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.QueueStateChange(component.requestedState);
  }

  public Door.ControlState CurrentState
  {
    get
    {
      return this.controlState;
    }
  }

  public Door.ControlState RequestedState
  {
    get
    {
      return this.requestedState;
    }
  }

  public bool ShouldBlockFallingSand
  {
    get
    {
      return this.rotatable.GetOrientation() != this.verticalOrientation;
    }
  }

  public bool isSealed
  {
    get
    {
      return this.controller.sm.isSealed.Get(this.controller);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = Door.OVERRIDE_ANIMS;
    this.synchronizeAnims = false;
    this.SetWorkTime(3f);
    if (!string.IsNullOrEmpty(this.doorClosingSoundEventName))
      this.doorClosingSound = GlobalAssets.GetSound(this.doorClosingSoundEventName, false);
    if (!string.IsNullOrEmpty(this.doorOpeningSoundEventName))
      this.doorOpeningSound = GlobalAssets.GetSound(this.doorOpeningSoundEventName, false);
    this.Subscribe<Door>(-905833192, Door.OnCopySettingsDelegate);
  }

  private Door.ControlState GetNextState(Door.ControlState wantedState)
  {
    return (Door.ControlState) ((int) (wantedState + 1) % 3);
  }

  private static bool DisplacesGas(Door.DoorType type)
  {
    return type != Door.DoorType.Internal;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.GetComponent<KPrefabID>() != (UnityEngine.Object) null)
      this.log = new LoggerFSS(nameof (Door), 35);
    if (!this.allowAutoControl && this.controlState == Door.ControlState.Auto)
      this.controlState = Door.ControlState.Locked;
    StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
    HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
    if (Door.DisplacesGas(this.doorType))
      structureTemperatures.Bypass(handle);
    this.controller = new Door.Controller.Instance(this);
    this.controller.StartSM();
    if (this.doorType == Door.DoorType.Sealed && !this.hasBeenUnsealed)
      this.Seal();
    this.UpdateDoorSpeed(this.operational.IsOperational);
    this.Subscribe<Door>(-592767678, Door.OnOperationalChangedDelegate);
    this.Subscribe<Door>(824508782, Door.OnOperationalChangedDelegate);
    this.Subscribe<Door>(-801688580, Door.OnLogicValueChangedDelegate);
    this.requestedState = this.CurrentState;
    this.ApplyRequestedControlState(true);
    int num1 = this.rotatable.GetOrientation() != Orientation.Neutral ? 0 : this.building.Def.WidthInCells * (this.building.Def.HeightInCells - 1);
    int num2 = this.rotatable.GetOrientation() != Orientation.Neutral ? this.building.Def.HeightInCells : this.building.Def.WidthInCells;
    for (int index = 0; index != num2; ++index)
    {
      int placementCell = this.building.PlacementCells[num1 + index];
      Grid.FakeFloor[placementCell] = true;
      Pathfinding.Instance.AddDirtyNavGridCell(placementCell);
    }
    List<int> intList = new List<int>();
    foreach (int placementCell in this.building.PlacementCells)
    {
      Grid.HasDoor[placementCell] = true;
      Grid.HasAccessDoor[placementCell] = (UnityEngine.Object) this.GetComponent<AccessControl>() != (UnityEngine.Object) null;
      if (this.rotatable.IsRotated)
      {
        intList.Add(Grid.CellAbove(placementCell));
        intList.Add(Grid.CellBelow(placementCell));
      }
      else
      {
        intList.Add(Grid.CellLeft(placementCell));
        intList.Add(Grid.CellRight(placementCell));
      }
      SimMessages.SetCellProperties(placementCell, (byte) 8);
      if (Door.DisplacesGas(this.doorType))
        Grid.RenderedByWorld[placementCell] = false;
    }
  }

  protected override void OnCleanUp()
  {
    this.UpdateDoorState(true);
    List<int> intList = new List<int>();
    foreach (int placementCell in this.building.PlacementCells)
    {
      SimMessages.ClearCellProperties(placementCell, (byte) 12);
      Grid.RenderedByWorld[placementCell] = Grid.Element[placementCell].substance.renderedByWorld;
      Grid.FakeFloor[placementCell] = false;
      if (Grid.Element[placementCell].IsSolid)
        SimMessages.ReplaceAndDisplaceElement(placementCell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0.0f, -1f, byte.MaxValue, 0, -1);
      Pathfinding.Instance.AddDirtyNavGridCell(placementCell);
      if (this.rotatable.IsRotated)
      {
        intList.Add(Grid.CellAbove(placementCell));
        intList.Add(Grid.CellBelow(placementCell));
      }
      else
      {
        intList.Add(Grid.CellLeft(placementCell));
        intList.Add(Grid.CellRight(placementCell));
      }
    }
    foreach (int placementCell in this.building.PlacementCells)
    {
      Grid.HasDoor[placementCell] = false;
      Grid.HasAccessDoor[placementCell] = false;
      Game.Instance.SetDupePassableSolid(placementCell, false, Grid.Solid[placementCell]);
      Grid.CritterImpassable[placementCell] = false;
      Grid.DupeImpassable[placementCell] = false;
      Pathfinding.Instance.AddDirtyNavGridCell(placementCell);
    }
    base.OnCleanUp();
  }

  public void Seal()
  {
    this.controller.sm.isSealed.Set(true, this.controller);
  }

  public void OrderUnseal()
  {
    this.controller.GoTo((StateMachine.BaseState) this.controller.sm.Sealed.awaiting_unlock);
  }

  private void RefreshControlState()
  {
    switch (this.controlState)
    {
      case Door.ControlState.Auto:
        this.controller.sm.isLocked.Set(false, this.controller);
        break;
      case Door.ControlState.Opened:
        this.controller.sm.isLocked.Set(false, this.controller);
        break;
      case Door.ControlState.Locked:
        this.controller.sm.isLocked.Set(true, this.controller);
        break;
    }
    this.Trigger(279163026, (object) this.controlState);
    this.SetWorldState();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CurrentDoorControlState, (object) this);
  }

  private void OnOperationalChanged(object data)
  {
    bool isOperational = this.operational.IsOperational;
    if (isOperational == this.on)
      return;
    this.UpdateDoorSpeed(isOperational);
    if (this.on && this.GetComponent<KPrefabID>().HasTag(GameTags.Transition))
      this.SetActive(true);
    else
      this.SetActive(false);
  }

  private void UpdateDoorSpeed(bool powered)
  {
    this.on = powered;
    this.UpdateAnimAndSoundParams(powered);
    float positionPercent = this.animController.GetPositionPercent();
    this.animController.Play(this.animController.CurrentAnim.hash, this.animController.PlayMode, 1f, 0.0f);
    this.animController.SetPositionPercent(positionPercent);
  }

  private void UpdateAnimAndSoundParams(bool powered)
  {
    if (powered)
    {
      this.animController.PlaySpeedMultiplier = this.poweredAnimSpeed;
      if (this.doorClosingSound != null)
        this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 1f);
      if (this.doorOpeningSound == null)
        return;
      this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 1f);
    }
    else
    {
      this.animController.PlaySpeedMultiplier = this.unpoweredAnimSpeed;
      if (this.doorClosingSound != null)
        this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 0.0f);
      if (this.doorOpeningSound == null)
        return;
      this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 0.0f);
    }
  }

  private void SetActive(bool active)
  {
    if (!this.operational.IsOperational)
      return;
    this.operational.SetActive(active, false);
  }

  private void SetWorldState()
  {
    int[] placementCells = this.building.PlacementCells;
    bool is_door_open = this.IsOpen();
    this.SetPassableState(is_door_open, (IList<int>) placementCells);
    this.SetSimState(is_door_open, (IList<int>) placementCells);
  }

  private void SetPassableState(bool is_door_open, IList<int> cells)
  {
    for (int index = 0; index < cells.Count; ++index)
    {
      int cell = cells[index];
      switch (this.doorType)
      {
        case Door.DoorType.Pressure:
        case Door.DoorType.ManualPressure:
        case Door.DoorType.Sealed:
          Grid.CritterImpassable[cell] = this.controlState != Door.ControlState.Opened;
          bool solid = !is_door_open;
          bool passable = this.controlState != Door.ControlState.Locked;
          Game.Instance.SetDupePassableSolid(cell, passable, solid);
          if (this.controlState == Door.ControlState.Opened)
          {
            this.doorOpenLiquidRefreshHack = true;
            this.doorOpenLiquidRefreshTime = 1f;
            break;
          }
          break;
        case Door.DoorType.Internal:
          Grid.CritterImpassable[cell] = this.controlState != Door.ControlState.Opened;
          Grid.DupeImpassable[cell] = this.controlState == Door.ControlState.Locked;
          break;
      }
      Pathfinding.Instance.AddDirtyNavGridCell(cell);
    }
  }

  private void SetSimState(bool is_door_open, IList<int> cells)
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    float mass = component.Mass / (float) cells.Count;
    for (int index = 0; index < cells.Count; ++index)
    {
      int cell = cells[index];
      switch (this.doorType)
      {
        case Door.DoorType.Pressure:
        case Door.DoorType.ManualPressure:
        case Door.DoorType.Sealed:
          World.Instance.groundRenderer.MarkDirty(cell);
          if (is_door_open)
          {
            HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorOpened), false));
            SimMessages.Dig(cell, handle.index);
            if (this.ShouldBlockFallingSand)
            {
              SimMessages.ClearCellProperties(cell, (byte) 4);
              break;
            }
            SimMessages.SetCellProperties(cell, (byte) 4);
            break;
          }
          HandleVector<Game.CallbackInfo>.Handle handle1 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorClosed), false));
          float temperature = component.Temperature;
          if ((double) temperature <= 0.0)
            temperature = component.Temperature;
          SimMessages.ReplaceAndDisplaceElement(cell, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, byte.MaxValue, 0, handle1.index);
          SimMessages.SetCellProperties(cell, (byte) 4);
          break;
      }
    }
  }

  private void UpdateDoorState(bool cleaningUp)
  {
    foreach (int placementCell in this.building.PlacementCells)
    {
      if (Grid.IsValidCell(placementCell))
        Grid.Foundation[placementCell] = !cleaningUp;
    }
  }

  public void QueueStateChange(Door.ControlState nextState)
  {
    this.requestedState = this.requestedState == nextState ? this.controlState : nextState;
    if (this.requestedState == this.controlState)
    {
      if (this.changeStateChore == null)
        return;
      this.changeStateChore.Cancel("Change state");
      this.changeStateChore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
    }
    else if (DebugHandler.InstantBuildMode)
    {
      this.controlState = this.requestedState;
      this.RefreshControlState();
      this.OnOperationalChanged((object) null);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
      double num = (double) this.Open();
      this.Close();
    }
    else
    {
      if (this.changeStateChore != null)
        this.changeStateChore.Cancel("Change state");
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, (object) this);
      this.changeStateChore = (Chore) new WorkChore<Door>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }
  }

  private void OnSimDoorOpened()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || !Door.DisplacesGas(this.doorType))
      return;
    StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
    HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
    structureTemperatures.UnBypass(handle);
    this.do_melt_check = false;
  }

  private void OnSimDoorClosed()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || !Door.DisplacesGas(this.doorType))
      return;
    StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
    HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
    structureTemperatures.Bypass(handle);
    this.do_melt_check = true;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.changeStateChore = (Chore) null;
    this.ApplyRequestedControlState(false);
  }

  public float Open()
  {
    if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
    {
      StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
      HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
      if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
      {
        int[] placementCells = this.building.PlacementCells;
        float num1 = 0.0f;
        int num2 = 0;
        for (int index1 = 0; index1 < placementCells.Length; ++index1)
        {
          int index2 = placementCells[index1];
          if ((double) Grid.Mass[index2] > 0.0)
          {
            ++num2;
            num1 += Grid.Temperature[index2];
          }
        }
        if (num2 > 0)
        {
          float num3 = num1 / (float) placementCells.Length;
          PrimaryElement component = this.GetComponent<PrimaryElement>();
          KCrashReporter.Assert((double) num3 > 0.0, "Door has calculated an invalid temperature");
          component.Temperature = num3;
        }
      }
    }
    ++this.openCount;
    float num = 1f;
    if ((UnityEngine.Object) this.consumer != (UnityEngine.Object) null)
      num = !this.consumer.IsPowered ? 0.5f : 1f;
    switch (this.controlState)
    {
      case Door.ControlState.Auto:
      case Door.ControlState.Opened:
        this.controller.sm.isOpen.Set(true, this.controller);
        break;
    }
    return num;
  }

  public void Close()
  {
    this.openCount = Mathf.Max(0, this.openCount - 1);
    if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
    {
      StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
      HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      if (handle.IsValid() && !structureTemperatures.IsBypassed(handle))
      {
        float temperature = structureTemperatures.GetPayload(handle).Temperature;
        component.Temperature = temperature;
      }
    }
    switch (this.controlState)
    {
      case Door.ControlState.Auto:
        if (this.openCount != 0)
          break;
        this.controller.sm.isOpen.Set(false, this.controller);
        Game.Instance.userMenu.Refresh(this.gameObject);
        break;
      case Door.ControlState.Locked:
        this.controller.sm.isOpen.Set(false, this.controller);
        break;
    }
  }

  public bool IsOpen()
  {
    if (!this.controller.IsInsideState((StateMachine.BaseState) this.controller.sm.open) && !this.controller.IsInsideState((StateMachine.BaseState) this.controller.sm.closedelay))
      return this.controller.IsInsideState((StateMachine.BaseState) this.controller.sm.closeblocked);
    return true;
  }

  private void ApplyRequestedControlState(bool force = false)
  {
    if (this.requestedState == this.controlState && !force)
      return;
    this.controlState = this.requestedState;
    this.RefreshControlState();
    this.OnOperationalChanged((object) null);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
    this.Trigger(1734268753, (object) this);
    if (force)
      return;
    double num = (double) this.Open();
    this.Close();
  }

  public void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (logicValueChanged.portID != Door.OPEN_CLOSE_PORT_ID)
      return;
    int newValue = logicValueChanged.newValue;
    if (this.changeStateChore != null)
    {
      this.changeStateChore.Cancel("Change state");
      this.changeStateChore = (Chore) null;
    }
    this.requestedState = newValue != 1 ? Door.ControlState.Locked : Door.ControlState.Opened;
    this.applyLogicChange = true;
  }

  public void Sim200ms(float dt)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    if (this.doorOpenLiquidRefreshHack)
    {
      this.doorOpenLiquidRefreshTime -= dt;
      if ((double) this.doorOpenLiquidRefreshTime <= 0.0)
      {
        this.doorOpenLiquidRefreshHack = false;
        foreach (int placementCell in this.building.PlacementCells)
          Pathfinding.Instance.AddDirtyNavGridCell(placementCell);
      }
    }
    if (this.applyLogicChange)
    {
      this.applyLogicChange = false;
      this.ApplyRequestedControlState(false);
    }
    if (!this.do_melt_check)
      return;
    StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
    HandleVector<int>.Handle handle = structureTemperatures.GetHandle(this.gameObject);
    if (!handle.IsValid() || structureTemperatures.GetPayload(handle).enabled)
      return;
    foreach (int placementCell in this.building.PlacementCells)
    {
      if (!Grid.Solid[placementCell])
      {
        StructureTemperatureComponents.DoMelt(this.GetComponent<PrimaryElement>());
        break;
      }
    }
  }

  public enum DoorType
  {
    Pressure,
    ManualPressure,
    Internal,
    Sealed,
  }

  public enum ControlState
  {
    Auto,
    Opened,
    Locked,
    NumStates,
  }

  public class Controller : GameStateMachine<Door.Controller, Door.Controller.Instance, Door>
  {
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State open;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State opening;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closing;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closedelay;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closeblocked;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locking;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locked;
    public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
    public Door.Controller.SealedStates Sealed;
    public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isOpen;
    public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isLocked;
    public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isBlocked;
    public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isSealed;
    public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter sealDirectionRight;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = true;
      default_state = (StateMachine.BaseState) this.closed;
      this.root.Update("RefreshIsBlocked", (System.Action<Door.Controller.Instance, float>) ((smi, dt) => smi.RefreshIsBlocked()), UpdateRate.SIM_200ms, false).ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isSealed, this.Sealed.closed, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
      this.closeblocked.PlayAnim("open").ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isBlocked, this.closedelay, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
      this.closedelay.PlayAnim("open").ScheduleGoTo(0.5f, (StateMachine.BaseState) this.closing).ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
      this.closing.ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Func<Door.Controller.Instance, string>) (smi => smi.master.doorClosingSound), (Func<Door.Controller.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.doorClosingSound))).Enter("SetParams", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.UpdateAnimAndSoundParams(smi.master.on))).Update((System.Action<Door.Controller.Instance, float>) ((smi, dt) =>
      {
        if (smi.master.doorClosingSound == null)
          return;
        smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorClosingSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
      }), UpdateRate.SIM_33ms, false).Enter("SetActive", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetActive(true))).Exit("SetActive", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetActive(false))).PlayAnim("closing").OnAnimQueueComplete(this.closed);
      this.open.PlayAnim("open").ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isOpen, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse).Enter("SetWorldStateOpen", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetWorldState()));
      this.closed.PlayAnim("closed").ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isOpen, this.opening, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isLocked, this.locking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).Enter("SetWorldStateClosed", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetWorldState()));
      this.locking.PlayAnim("locked_pre").OnAnimQueueComplete(this.locked).Enter("SetWorldStateClosed", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetWorldState()));
      this.locked.PlayAnim("locked").ParamTransition<bool>((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.Parameter<bool>) this.isLocked, this.unlocking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
      this.unlocking.PlayAnim("locked_pst").OnAnimQueueComplete(this.closed);
      this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Func<Door.Controller.Instance, string>) (smi => smi.master.doorOpeningSound), (Func<Door.Controller.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.doorOpeningSound))).Enter("SetParams", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.UpdateAnimAndSoundParams(smi.master.on))).Update((System.Action<Door.Controller.Instance, float>) ((smi, dt) =>
      {
        if (smi.master.doorOpeningSound == null)
          return;
        smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorOpeningSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
      }), UpdateRate.SIM_33ms, false).Enter("SetActive", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetActive(true))).Exit("SetActive", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetActive(false))).PlayAnim("opening").OnAnimQueueComplete(this.open);
      this.Sealed.Enter((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi =>
      {
        OccupyArea component = smi.master.GetComponent<OccupyArea>();
        for (int index = 0; index < component.OccupiedCellsOffsets.Length; ++index)
          Grid.PreventFogOfWarReveal[Grid.OffsetCell(Grid.PosToCell(smi.master.gameObject), component.OccupiedCellsOffsets[index])] = false;
        smi.sm.isLocked.Set(true, smi);
        smi.master.controlState = Door.ControlState.Locked;
        smi.master.RefreshControlState();
        if (!smi.master.GetComponent<Unsealable>().facingRight)
          return;
        smi.master.GetComponent<KBatchedAnimController>().FlipX = true;
      })).Enter("SetWorldStateClosed", (StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi => smi.master.SetWorldState())).Exit((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi =>
      {
        smi.sm.isLocked.Set(false, smi);
        smi.master.GetComponent<AccessControl>().controlEnabled = true;
        smi.master.controlState = Door.ControlState.Opened;
        smi.master.RefreshControlState();
        smi.sm.isOpen.Set(true, smi);
        smi.sm.isLocked.Set(false, smi);
        smi.sm.isSealed.Set(false, smi);
      }));
      this.Sealed.closed.PlayAnim("sealed", KAnim.PlayMode.Once);
      this.Sealed.awaiting_unlock.ToggleChore((Func<Door.Controller.Instance, Chore>) (smi => this.CreateUnsealChore(smi, true)), this.Sealed.chore_pst);
      this.Sealed.chore_pst.Enter((StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State.Callback) (smi =>
      {
        smi.master.hasBeenUnsealed = true;
        if (smi.master.GetComponent<Unsealable>().unsealed)
        {
          smi.GoTo((StateMachine.BaseState) this.opening);
          FogOfWarMask.ClearMask(Grid.CellRight(Grid.PosToCell(smi.master.gameObject)));
          FogOfWarMask.ClearMask(Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)));
        }
        else
          smi.GoTo((StateMachine.BaseState) this.Sealed.closed);
      }));
    }

    private Chore CreateUnsealChore(Door.Controller.Instance smi, bool approach_right)
    {
      return (Chore) new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }

    public class SealedStates : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
    {
      public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;
      public Door.Controller.SealedStates.AwaitingUnlock awaiting_unlock;
      public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State chore_pst;

      public class AwaitingUnlock : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
      {
        public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State awaiting_arrival;
        public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
      }
    }

    public class Instance : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.GameInstance
    {
      public Instance(Door door)
        : base(door)
      {
      }

      public void RefreshIsBlocked()
      {
        bool flag = false;
        foreach (int placementCell in this.master.GetComponent<Building>().PlacementCells)
        {
          if ((UnityEngine.Object) Grid.Objects[placementCell, 0] != (UnityEngine.Object) null)
          {
            flag = true;
            break;
          }
        }
        this.sm.isBlocked.Set(flag, this.smi);
      }
    }
  }
}
