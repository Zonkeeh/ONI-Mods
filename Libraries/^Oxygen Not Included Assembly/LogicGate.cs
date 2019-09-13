// Decompiled with JetBrains decompiler
// Type: LogicGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGate : LogicGateBase, ILogicEventSender, ILogicNetworkConnection
{
  private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description()
  {
    name = (string) UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
    active = (string) UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
    inactive = (string) UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE
  };
  private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_DOUBLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description()
  {
    name = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_ONE_NAME,
    active = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_ONE_ACTIVE,
    inactive = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_ONE_INACTIVE
  };
  private static readonly LogicGate.LogicGateDescriptions.Description INPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description()
  {
    name = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_TWO_NAME,
    active = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_TWO_ACTIVE,
    inactive = (string) UI.LOGIC_PORTS.GATE_DOUBLE_INPUT_TWO_INACTIVE
  };
  private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicGate>((System.Action<LogicGate, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicGate>((System.Action<LogicGate, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  private LogicGate.LogicGateDescriptions descriptions;
  private const bool IS_CIRCUIT_ENDPOINT = true;
  private bool connected;
  protected bool cleaningUp;
  [Serialize]
  protected int outputValue;
  private LogicEventHandler inputOne;
  private LogicEventHandler inputTwo;
  private LogicPortVisualizer output;

  protected override void OnSpawn()
  {
    this.inputOne = new LogicEventHandler(this.InputCellOne, new System.Action<int>(this.UpdateState), (System.Action<int, bool>) null, LogicPortSpriteType.Input);
    if (this.RequiresTwoInputs)
      this.inputTwo = new LogicEventHandler(this.InputCellTwo, new System.Action<int>(this.UpdateState), (System.Action<int, bool>) null, LogicPortSpriteType.Input);
    this.Subscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate);
    this.Subscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate);
    BuildingHP component = this.GetComponent<BuildingHP>();
    if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsBroken)
      return;
    this.Connect();
  }

  protected override void OnCleanUp()
  {
    this.cleaningUp = true;
    this.Disconnect();
    this.Unsubscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate, false);
    this.Unsubscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate, false);
    base.OnCleanUp();
  }

  private void OnBuildingBroken(object data)
  {
    this.Disconnect();
  }

  private void OnBuildingFullyRepaired(object data)
  {
    this.Connect();
  }

  private void Connect()
  {
    if (this.connected)
      return;
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
    this.connected = true;
    int outputCell = this.OutputCell;
    logicCircuitSystem.AddToNetworks(outputCell, (object) this, true);
    this.output = new LogicPortVisualizer(outputCell, LogicPortSpriteType.Output);
    logicCircuitManager.AddVisElem((ILogicUIElement) this.output);
    int inputCellOne = this.InputCellOne;
    logicCircuitSystem.AddToNetworks(inputCellOne, (object) this.inputOne, true);
    logicCircuitManager.AddVisElem((ILogicUIElement) this.inputOne);
    if (this.RequiresTwoInputs)
    {
      int inputCellTwo = this.InputCellTwo;
      logicCircuitSystem.AddToNetworks(inputCellTwo, (object) this.inputTwo, true);
      logicCircuitManager.AddVisElem((ILogicUIElement) this.inputTwo);
    }
    this.RefreshAnimation();
  }

  private void Disconnect()
  {
    if (!this.connected)
      return;
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
    this.connected = false;
    int outputCell = this.OutputCell;
    logicCircuitSystem.RemoveFromNetworks(outputCell, (object) this, true);
    logicCircuitManager.RemoveVisElem((ILogicUIElement) this.output);
    this.output = (LogicPortVisualizer) null;
    int inputCellOne = this.InputCellOne;
    logicCircuitSystem.RemoveFromNetworks(inputCellOne, (object) this.inputOne, true);
    logicCircuitManager.RemoveVisElem((ILogicUIElement) this.inputOne);
    this.inputOne = (LogicEventHandler) null;
    if (this.RequiresTwoInputs)
    {
      int inputCellTwo = this.InputCellTwo;
      logicCircuitSystem.RemoveFromNetworks(inputCellTwo, (object) this.inputTwo, true);
      logicCircuitManager.RemoveVisElem((ILogicUIElement) this.inputTwo);
      this.inputTwo = (LogicEventHandler) null;
    }
    this.RefreshAnimation();
  }

  private void UpdateState(int new_value)
  {
    if (this.cleaningUp)
      return;
    int val1 = this.inputOne.Value;
    int val2 = this.inputTwo == null ? 0 : this.inputTwo.Value;
    this.outputValue = 0;
    switch (this.op)
    {
      case LogicGateBase.Op.And:
        this.outputValue = val1 & val2;
        break;
      case LogicGateBase.Op.Or:
        this.outputValue = val1 | val2;
        break;
      case LogicGateBase.Op.Not:
        this.outputValue = val1 != 0 ? 0 : 1;
        break;
      case LogicGateBase.Op.Xor:
        this.outputValue = val1 ^ val2;
        break;
      case LogicGateBase.Op.CustomSingle:
        this.outputValue = this.GetCustomValue(val1, val2);
        break;
    }
    this.RefreshAnimation();
  }

  public virtual void LogicTick()
  {
  }

  protected virtual int GetCustomValue(int val1, int val2)
  {
    return val1;
  }

  public int GetPortValue(LogicGateBase.PortId port)
  {
    switch (port)
    {
      case LogicGateBase.PortId.InputOne:
        return this.inputOne.Value;
      case LogicGateBase.PortId.InputTwo:
        if (this.RequiresTwoInputs)
          return this.inputTwo.Value;
        return 0;
      default:
        return this.outputValue;
    }
  }

  public bool GetPortConnected(LogicGateBase.PortId port)
  {
    if (port == LogicGateBase.PortId.InputTwo && !this.RequiresTwoInputs)
      return false;
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.PortCell(port)) != null;
  }

  public void SetPortDescriptions(LogicGate.LogicGateDescriptions descriptions)
  {
    this.descriptions = descriptions;
  }

  public LogicGate.LogicGateDescriptions.Description GetPortDescription(
    LogicGateBase.PortId port)
  {
    switch (port)
    {
      case LogicGateBase.PortId.InputOne:
        if (this.descriptions.inputOne != null)
          return this.descriptions.inputOne;
        if (this.RequiresTwoInputs)
          return LogicGate.INPUT_ONE_DOUBLE_DESCRIPTION;
        return LogicGate.INPUT_ONE_SINGLE_DESCRIPTION;
      case LogicGateBase.PortId.InputTwo:
        if (this.descriptions.inputTwo != null)
          return this.descriptions.inputTwo;
        return LogicGate.INPUT_TWO_DESCRIPTION;
      default:
        return this.descriptions.output;
    }
  }

  public int GetLogicValue()
  {
    return this.outputValue;
  }

  public int GetLogicCell()
  {
    return this.GetLogicUICell();
  }

  public int GetLogicUICell()
  {
    return this.OutputCell;
  }

  public bool IsLogicInput()
  {
    return false;
  }

  protected void RefreshAnimation()
  {
    if (this.cleaningUp)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(this.OutputCell) is LogicCircuitNetwork))
      component.Play((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
    else if (this.RequiresTwoInputs)
    {
      int num = this.inputOne.Value + this.inputTwo.Value * 2 + this.outputValue * 4;
      component.Play((HashedString) ("on_" + num.ToString()), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    else
    {
      int num = this.inputOne.Value + this.outputValue * 4;
      component.Play((HashedString) ("on_" + num.ToString()), KAnim.PlayMode.Once, 1f, 0.0f);
    }
  }

  public void OnLogicNetworkConnectionChanged(bool connected)
  {
  }

  public class LogicGateDescriptions
  {
    public LogicGate.LogicGateDescriptions.Description inputOne;
    public LogicGate.LogicGateDescriptions.Description inputTwo;
    public LogicGate.LogicGateDescriptions.Description output;

    public class Description
    {
      public string name;
      public string active;
      public string inactive;
    }
  }
}
