// Decompiled with JetBrains decompiler
// Type: LogicCircuitNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class LogicCircuitNetwork : UtilityNetwork
{
  private List<LogicWire> wires = new List<LogicWire>();
  private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();
  private List<ILogicEventSender> senders = new List<ILogicEventSender>();
  private int previousValue = -1;
  private int outputValue;
  private bool resetting;

  public override void AddItem(int cell, object item)
  {
    if (item is LogicWire)
      this.wires.Add((LogicWire) item);
    else if (item is ILogicEventReceiver)
    {
      this.receivers.Add((ILogicEventReceiver) item);
    }
    else
    {
      if (!(item is ILogicEventSender))
        return;
      this.senders.Add((ILogicEventSender) item);
    }
  }

  public override void RemoveItem(int cell, object item)
  {
    if (item is LogicWire)
      this.wires.Remove((LogicWire) item);
    else if (item is ILogicEventReceiver)
    {
      this.receivers.Remove(item as ILogicEventReceiver);
    }
    else
    {
      if (!(item is ILogicEventSender))
        return;
      this.senders.Remove((ILogicEventSender) item);
    }
  }

  public override void ConnectItem(int cell, object item)
  {
    if (item is ILogicEventReceiver)
    {
      ((ILogicNetworkConnection) item).OnLogicNetworkConnectionChanged(true);
    }
    else
    {
      if (!(item is ILogicEventSender))
        return;
      ((ILogicNetworkConnection) item).OnLogicNetworkConnectionChanged(true);
    }
  }

  public override void DisconnectItem(int cell, object item)
  {
    if (item is ILogicEventReceiver)
    {
      ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
      logicEventReceiver.ReceiveLogicEvent(0);
      logicEventReceiver.OnLogicNetworkConnectionChanged(false);
    }
    else
    {
      if (!(item is ILogicEventSender))
        return;
      (item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
    }
  }

  public override void Reset(UtilityNetworkGridNode[] grid)
  {
    this.resetting = true;
    this.previousValue = -1;
    this.outputValue = 0;
    for (int index = 0; index < this.wires.Count; ++index)
    {
      LogicWire wire = this.wires[index];
      if ((Object) wire != (Object) null)
      {
        int cell = Grid.PosToCell(wire.transform.GetPosition());
        UtilityNetworkGridNode utilityNetworkGridNode = grid[cell];
        utilityNetworkGridNode.networkIdx = -1;
        grid[cell] = utilityNetworkGridNode;
      }
    }
    this.wires.Clear();
    this.senders.Clear();
    this.receivers.Clear();
    this.resetting = false;
  }

  public void UpdateLogicValue()
  {
    if (this.resetting)
      return;
    this.previousValue = this.outputValue;
    this.outputValue = 0;
    foreach (ILogicEventSender sender in this.senders)
      sender.LogicTick();
    foreach (ILogicEventSender sender in this.senders)
      this.outputValue |= sender.GetLogicValue();
  }

  public void SendLogicEvents(bool force_send)
  {
    if (this.resetting || this.outputValue == this.previousValue && !force_send)
      return;
    foreach (ILogicEventReceiver receiver in this.receivers)
      receiver.ReceiveLogicEvent(this.outputValue);
    if (force_send)
      return;
    this.TriggerAudio(this.previousValue < 0 ? 0 : this.previousValue);
  }

  private void TriggerAudio(int old_value)
  {
    SpeedControlScreen instance1 = SpeedControlScreen.Instance;
    if (old_value == this.outputValue || !((Object) instance1 != (Object) null) || instance1.IsPaused)
      return;
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    List<LogicWire> logicWireList = new List<LogicWire>();
    for (int index = 0; index < this.wires.Count; ++index)
    {
      if (visibleArea.Min <= (Vector2) this.wires[index].transform.GetPosition() && (Vector2) this.wires[index].transform.GetPosition() <= visibleArea.Max)
        logicWireList.Add(this.wires[index]);
    }
    if (logicWireList.Count <= 0)
      return;
    int index1 = Mathf.CeilToInt((float) (logicWireList.Count / 2));
    if (!((Object) logicWireList[index1] != (Object) null))
      return;
    EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound("Logic_Circuit_Toggle", false), logicWireList[index1].transform.GetPosition());
    int num1 = (int) instance2.setParameterValue("wireCount", (float) (this.wires.Count % 24));
    int num2 = (int) instance2.setParameterValue("enabled", (float) this.outputValue);
    KFMOD.EndOneShot(instance2);
  }

  public int OutputValue
  {
    get
    {
      return this.outputValue;
    }
  }

  public List<LogicWire> Wires
  {
    get
    {
      return this.wires;
    }
  }

  public ReadOnlyCollection<ILogicEventSender> Senders
  {
    get
    {
      return this.senders.AsReadOnly();
    }
  }

  public ReadOnlyCollection<ILogicEventReceiver> Receivers
  {
    get
    {
      return this.receivers.AsReadOnly();
    }
  }
}
