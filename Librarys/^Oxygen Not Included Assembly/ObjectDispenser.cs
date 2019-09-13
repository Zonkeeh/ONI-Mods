// Decompiled with JetBrains decompiler
// Type: ObjectDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class ObjectDispenser : Switch, IUserControlledCapacity
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (ObjectDispenser);
  private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>((System.Action<ObjectDispenser, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  private LoggerFS log;
  public CellOffset dropOffset;
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Storage storage;
  [MyCmpGet]
  private Rotatable rotatable;
  private ObjectDispenser.Instance smi;
  private static StatusItem infoStatusItem;
  protected FilteredStorage filteredStorage;

  public virtual float UserMaxCapacity
  {
    get
    {
      return Mathf.Min(this.userMaxCapacity, this.GetComponent<Storage>().capacityKg);
    }
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
    }
  }

  public float AmountStored
  {
    get
    {
      return this.GetComponent<Storage>().MassStored();
    }
  }

  public float MinCapacity
  {
    get
    {
      return 0.0f;
    }
  }

  public float MaxCapacity
  {
    get
    {
      return this.GetComponent<Storage>().capacityKg;
    }
  }

  public bool WholeValues
  {
    get
    {
      return false;
    }
  }

  public LocString CapacityUnits
  {
    get
    {
      return GameUtil.GetCurrentMassUnit(false);
    }
  }

  protected override void OnPrefabInit()
  {
    this.Initialize();
  }

  protected void Initialize()
  {
    base.OnPrefabInit();
    this.log = new LoggerFS(nameof (ObjectDispenser), 35);
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (Tag[]) null, (IUserControlledCapacity) this, false, Db.Get().ChoreTypes.StorageFetch);
    this.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new ObjectDispenser.Instance(this, this.IsSwitchedOn);
    this.smi.StartSM();
    if (ObjectDispenser.infoStatusItem == null)
    {
      ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem infoStatusItem = ObjectDispenser.infoStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (ObjectDispenser.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectDispenser.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = ObjectDispenser.\u003C\u003Ef__mg\u0024cache0;
      infoStatusItem.resolveStringCallback = fMgCache0;
    }
    this.filteredStorage.FilterChanged();
    this.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, (object) this.smi);
  }

  protected override void OnCleanUp()
  {
    this.filteredStorage.CleanUp();
    base.OnCleanUp();
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public void DropHeldItems()
  {
    while (this.storage.Count > 0)
    {
      GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
      if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
        gameObject.transform.SetPosition(this.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
      else
        gameObject.transform.SetPosition(this.transform.GetPosition() + this.dropOffset.ToVector3());
    }
    this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, new Vector3(), true);
  }

  protected override void Toggle()
  {
    base.Toggle();
  }

  protected override void OnRefreshUserMenu(object data)
  {
    if (this.smi.IsAutomated())
      return;
    base.OnRefreshUserMenu(data);
  }

  private static string ResolveInfoStatusItemString(string format_str, object data)
  {
    ObjectDispenser.Instance instance = (ObjectDispenser.Instance) data;
    return string.Format((string) (!instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL), (object) (string) (!instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED : BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED));
  }

  public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
  {
    public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;
    public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;
    public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;
    public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;
    public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = true;
      this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, (StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State.Callback) (smi => smi.UpdateState())).ParamTransition<bool>((StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>) this.should_open, this.drop_item, (StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>.Callback) ((smi, p) =>
      {
        if (p)
          return !smi.master.GetComponent<Storage>().IsEmpty();
        return false;
      }));
      this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
      this.load_item_pst.ParamTransition<bool>((StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>) this.should_open, this.idle, (StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>.Callback) ((smi, p) => !p)).ParamTransition<bool>((StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>) this.should_open, this.drop_item, (StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit((StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State.Callback) (smi => smi.master.DropHeldItems()));
    }
  }

  public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
  {
    public bool logic_on = true;
    private Operational operational;
    public LogicPorts logic;
    private bool manual_on;

    public Instance(ObjectDispenser master, bool manual_start_state)
      : base(master)
    {
      this.manual_on = manual_start_state;
      this.operational = this.GetComponent<Operational>();
      this.logic = this.GetComponent<LogicPorts>();
      this.Subscribe(-592767678, new System.Action<object>(this.OnOperationalChanged));
      this.Subscribe(-801688580, new System.Action<object>(this.OnLogicValueChanged));
      this.smi.sm.should_open.Set(true, this.smi);
    }

    public void UpdateState()
    {
      this.smi.GoTo((StateMachine.BaseState) this.sm.load_item);
    }

    public bool IsAutomated()
    {
      return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
    }

    public bool IsOpened
    {
      get
      {
        if (this.IsAutomated())
          return this.logic_on;
        return this.manual_on;
      }
    }

    public void SetSwitchState(bool on)
    {
      this.manual_on = on;
      this.UpdateShouldOpen();
    }

    public void SetActive(bool active)
    {
      this.operational.SetActive(this.operational.IsOperational && active, false);
    }

    private void OnOperationalChanged(object data)
    {
      this.UpdateShouldOpen();
    }

    private void OnLogicValueChanged(object data)
    {
      LogicValueChanged logicValueChanged = (LogicValueChanged) data;
      if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
        return;
      this.logic_on = logicValueChanged.newValue != 0;
      this.UpdateShouldOpen();
    }

    private void UpdateShouldOpen()
    {
      if (!this.operational.IsOperational)
        return;
      if (this.IsAutomated())
        this.smi.sm.should_open.Set(this.logic_on, this.smi);
      else
        this.smi.sm.should_open.Set(this.manual_on, this.smi);
    }
  }
}
