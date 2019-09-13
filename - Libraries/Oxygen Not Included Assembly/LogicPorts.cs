// Decompiled with JetBrains decompiler
// Type: LogicPorts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPorts : KMonoBehaviour, IEffectDescriptor, IRenderEveryTick
{
  private int cell = -1;
  private Orientation orientation = Orientation.NumRotations;
  [SerializeField]
  public LogicPorts.Port[] outputPortInfo;
  [SerializeField]
  public LogicPorts.Port[] inputPortInfo;
  public List<ILogicUIElement> outputPorts;
  public List<ILogicUIElement> inputPorts;
  [Serialize]
  private int[] serializedOutputValues;
  private bool isPhysical;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.isPhysical = (UnityEngine.Object) this.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    if (!this.isPhysical && (UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() == (UnityEngine.Object) null)
    {
      OverlayScreen.Instance.OnOverlayChanged += new System.Action<HashedString>(this.OnOverlayChanged);
      this.OnOverlayChanged(OverlayScreen.Instance.mode);
      this.CreateVisualizers();
      SimAndRenderScheduler.instance.Add((object) this, false);
    }
    else if (this.isPhysical)
    {
      this.UpdateMissingWireIcon();
      this.CreatePhysicalPorts();
    }
    else
      this.CreateVisualizers();
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new System.Action<HashedString>(this.OnOverlayChanged);
    this.DestroyVisualizers();
    if (this.isPhysical)
      this.DestroyPhysicalPorts();
    base.OnCleanUp();
  }

  public void RenderEveryTick(float dt)
  {
    this.CreateVisualizers();
  }

  public void HackRefreshVisualizers()
  {
    this.CreateVisualizers();
  }

  private void CreateVisualizers()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    bool flag = cell != this.cell;
    this.cell = cell;
    if (!flag)
    {
      Rotatable component = this.GetComponent<Rotatable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Orientation orientation = component.GetOrientation();
        flag = orientation != this.orientation;
        this.orientation = orientation;
      }
    }
    if (!flag)
      return;
    this.DestroyVisualizers();
    if (this.outputPortInfo != null)
    {
      this.outputPorts = new List<ILogicUIElement>();
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.outputPortInfo[index];
        LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
        this.outputPorts.Add((ILogicUIElement) logicPortVisualizer);
        Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) logicPortVisualizer);
      }
    }
    if (this.inputPortInfo == null)
      return;
    this.inputPorts = new List<ILogicUIElement>();
    for (int index = 0; index < this.inputPortInfo.Length; ++index)
    {
      LogicPorts.Port port = this.inputPortInfo[index];
      LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
      this.inputPorts.Add((ILogicUIElement) logicPortVisualizer);
      Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) logicPortVisualizer);
    }
  }

  private void DestroyVisualizers()
  {
    if (this.outputPorts != null)
    {
      foreach (ILogicUIElement outputPort in this.outputPorts)
        Game.Instance.logicCircuitManager.RemoveVisElem(outputPort);
    }
    if (this.inputPorts == null)
      return;
    foreach (ILogicUIElement inputPort in this.inputPorts)
      Game.Instance.logicCircuitManager.RemoveVisElem(inputPort);
  }

  private void CreatePhysicalPorts()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.cell)
      return;
    this.cell = cell;
    this.DestroyVisualizers();
    if (this.outputPortInfo != null)
    {
      this.outputPorts = new List<ILogicUIElement>();
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port info = this.outputPortInfo[index];
        LogicEventSender logicEventSender = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), (System.Action<int>) (new_value =>
        {
          if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
            return;
          this.OnLogicValueChanged(info.id, new_value);
        }), new System.Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
        this.outputPorts.Add((ILogicUIElement) logicEventSender);
        Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) logicEventSender);
        Game.Instance.logicCircuitSystem.AddToNetworks(logicEventSender.GetLogicUICell(), (object) logicEventSender, true);
      }
      if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
      {
        for (int index = 0; index < this.outputPorts.Count; ++index)
          (this.outputPorts[index] as LogicEventSender).SetValue(this.serializedOutputValues[index]);
      }
    }
    this.serializedOutputValues = (int[]) null;
    if (this.inputPortInfo == null)
      return;
    this.inputPorts = new List<ILogicUIElement>();
    for (int index = 0; index < this.inputPortInfo.Length; ++index)
    {
      LogicPorts.Port info = this.inputPortInfo[index];
      LogicEventHandler logicEventHandler = new LogicEventHandler(this.GetActualCell(info.cellOffset), (System.Action<int>) (new_value =>
      {
        if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
          return;
        this.OnLogicValueChanged(info.id, new_value);
      }), new System.Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
      this.inputPorts.Add((ILogicUIElement) logicEventHandler);
      Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) logicEventHandler);
      Game.Instance.logicCircuitSystem.AddToNetworks(logicEventHandler.GetLogicUICell(), (object) logicEventHandler, true);
    }
  }

  private bool ShowMissingWireIcon()
  {
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    if (this.outputPortInfo != null)
    {
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.outputPortInfo[index];
        if (port.requiresConnection)
        {
          int portCell = this.GetPortCell(port.id);
          if (logicCircuitManager.GetNetworkForCell(portCell) == null)
            return true;
        }
      }
    }
    if (this.inputPortInfo != null)
    {
      for (int index = 0; index < this.inputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.inputPortInfo[index];
        if (port.requiresConnection)
        {
          int portCell = this.GetPortCell(port.id);
          if (logicCircuitManager.GetNetworkForCell(portCell) == null)
            return true;
        }
      }
    }
    return false;
  }

  private void OnLogicNetworkConnectionChanged(int cell, bool connected)
  {
    this.UpdateMissingWireIcon();
  }

  private void UpdateMissingWireIcon()
  {
    LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), this.gameObject);
  }

  private void DestroyPhysicalPorts()
  {
    if (this.outputPorts != null)
    {
      foreach (ILogicEventSender outputPort in this.outputPorts)
        Game.Instance.logicCircuitSystem.RemoveFromNetworks(outputPort.GetLogicCell(), (object) outputPort, true);
    }
    if (this.inputPorts == null)
      return;
    for (int index = 0; index < this.inputPorts.Count; ++index)
    {
      LogicEventHandler inputPort = this.inputPorts[index] as LogicEventHandler;
      if (inputPort != null)
        Game.Instance.logicCircuitSystem.RemoveFromNetworks(inputPort.GetLogicCell(), (object) inputPort, true);
    }
  }

  private void OnLogicValueChanged(HashedString port_id, int new_value)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    this.gameObject.Trigger(-801688580, (object) new LogicValueChanged()
    {
      portID = port_id,
      newValue = new_value
    });
  }

  private int GetActualCell(CellOffset offset)
  {
    Rotatable component = this.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      offset = component.GetRotatedCellOffset(offset);
    return Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), offset);
  }

  public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
  {
    foreach (LogicPorts.Port port1 in this.inputPortInfo)
    {
      if (this.GetActualCell(port1.cellOffset) == cell)
      {
        port = port1;
        isInput = true;
        return true;
      }
    }
    foreach (LogicPorts.Port port1 in this.outputPortInfo)
    {
      if (this.GetActualCell(port1.cellOffset) == cell)
      {
        port = port1;
        isInput = false;
        return true;
      }
    }
    port = new LogicPorts.Port();
    isInput = false;
    return false;
  }

  public void SendSignal(HashedString port_id, int new_value)
  {
    foreach (LogicEventSender outputPort in this.outputPorts)
    {
      if (outputPort.ID == port_id)
      {
        outputPort.SetValue(new_value);
        break;
      }
    }
  }

  public int GetPortCell(HashedString port_id)
  {
    foreach (LogicPorts.Port port in this.inputPortInfo)
    {
      if (port.id == port_id)
        return this.GetActualCell(port.cellOffset);
    }
    foreach (LogicPorts.Port port in this.outputPortInfo)
    {
      if (port.id == port_id)
        return this.GetActualCell(port.cellOffset);
    }
    return -1;
  }

  public int GetInputValue(HashedString port_id)
  {
    for (int index = 0; index < this.inputPortInfo.Length; ++index)
    {
      if (this.inputPortInfo[index].id == port_id)
      {
        LogicEventHandler inputPort = this.inputPorts[index] as LogicEventHandler;
        if (inputPort == null)
          return 0;
        return inputPort.Value;
      }
    }
    return 0;
  }

  public int GetOutputValue(HashedString port_id)
  {
    for (int index = 0; index < this.outputPorts.Count; ++index)
    {
      LogicEventSender outputPort = this.outputPorts[index] as LogicEventSender;
      if (outputPort == null)
        return 0;
      if (outputPort.ID == port_id)
        return outputPort.GetLogicValue();
    }
    return 0;
  }

  public bool IsPortConnected(HashedString port_id)
  {
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetPortCell(port_id)) != null;
  }

  private void OnOverlayChanged(HashedString mode)
  {
    if (mode == OverlayModes.Logic.ID)
    {
      this.enabled = true;
      this.CreateVisualizers();
    }
    else
    {
      this.enabled = false;
      this.DestroyVisualizers();
    }
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    LogicPorts component = def.BuildingComplete.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (component.inputPortInfo != null && component.inputPortInfo.Length > 0)
      {
        Descriptor descriptor = new Descriptor((string) UI.LOGIC_PORTS.INPUT_PORTS, (string) UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
        descriptorList.Add(descriptor);
        foreach (LogicPorts.Port port in component.inputPortInfo)
        {
          string tooltip = string.Format((string) UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, (object) port.activeDescription, (object) port.inactiveDescription);
          descriptor = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
          descriptor.IncreaseIndent();
          descriptorList.Add(descriptor);
        }
      }
      if (component.outputPortInfo != null && component.outputPortInfo.Length > 0)
      {
        Descriptor descriptor = new Descriptor((string) UI.LOGIC_PORTS.OUTPUT_PORTS, (string) UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
        descriptorList.Add(descriptor);
        foreach (LogicPorts.Port port in component.outputPortInfo)
        {
          string tooltip = string.Format((string) UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, (object) port.activeDescription, (object) port.inactiveDescription);
          descriptor = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
          descriptor.IncreaseIndent();
          descriptorList.Add(descriptor);
        }
      }
    }
    return descriptorList;
  }

  [OnSerializing]
  private void OnSerializing()
  {
    if (!this.isPhysical || this.outputPorts == null)
      return;
    this.serializedOutputValues = new int[this.outputPorts.Count];
    for (int index = 0; index < this.outputPorts.Count; ++index)
    {
      LogicEventSender outputPort = this.outputPorts[index] as LogicEventSender;
      this.serializedOutputValues[index] = outputPort.GetLogicValue();
    }
  }

  [OnSerialized]
  private void OnSerialized()
  {
    this.serializedOutputValues = (int[]) null;
  }

  [Serializable]
  public struct Port
  {
    public HashedString id;
    public CellOffset cellOffset;
    public string description;
    public string activeDescription;
    public string inactiveDescription;
    public bool requiresConnection;
    public LogicPortSpriteType spriteType;
    public bool displayCustomName;

    public Port(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon,
      LogicPortSpriteType sprite_type,
      bool display_custom_name = false)
    {
      this.id = id;
      this.cellOffset = cell_offset;
      this.description = description;
      this.activeDescription = activeDescription;
      this.inactiveDescription = inactiveDescription;
      this.requiresConnection = show_wire_missing_icon;
      this.spriteType = sprite_type;
      this.displayCustomName = display_custom_name;
    }

    public static LogicPorts.Port InputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
    }

    public static LogicPorts.Port OutputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
    }
  }
}
