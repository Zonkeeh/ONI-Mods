// Decompiled with JetBrains decompiler
// Type: LogicOperationalController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class LogicOperationalController : KMonoBehaviour
{
  public static readonly HashedString PORT_ID = (HashedString) "LogicOperational";
  private static readonly Operational.Flag logicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);
  public static readonly LogicPorts.Port[] INPUT_PORTS_0_0 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
  };
  public static readonly LogicPorts.Port[] INPUT_PORTS_0_1 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 1), (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
  };
  public static readonly LogicPorts.Port[] INPUT_PORTS_1_0 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(1, 0), (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
  };
  public static readonly LogicPorts.Port[] INPUT_PORTS_1_1 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(1, 1), (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
  };
  public static readonly LogicPorts.Port[] INPUT_PORTS_N1_0 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 0), (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, (string) UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
  };
  private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>((System.Action<LogicOperationalController, object>) ((component, data) => component.OnLogicValueChanged(data)));
  public int unNetworkedValue = 1;
  private static StatusItem infoStatusItem;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
    if (LogicOperationalController.infoStatusItem == null)
    {
      LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem infoStatusItem = LogicOperationalController.infoStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (LogicOperationalController.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LogicOperationalController.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = LogicOperationalController.\u003C\u003Ef__mg\u0024cache0;
      infoStatusItem.resolveStringCallback = fMgCache0;
    }
    this.CheckWireState();
  }

  private LogicCircuitNetwork GetNetwork()
  {
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID));
  }

  private LogicCircuitNetwork CheckWireState()
  {
    LogicCircuitNetwork network = this.GetNetwork();
    int num = network == null ? this.unNetworkedValue : network.OutputValue;
    this.GetComponent<Operational>().SetFlag(LogicOperationalController.logicOperationalFlag, num > 0);
    return network;
  }

  private static string ResolveInfoStatusItemString(string format_str, object data)
  {
    return (string) (!((Component) data).GetComponent<Operational>().GetFlag(LogicOperationalController.logicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED);
  }

  private void OnLogicValueChanged(object data)
  {
    if (!(((LogicValueChanged) data).portID == LogicOperationalController.PORT_ID))
      return;
    LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
    this.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, (object) this);
  }
}
