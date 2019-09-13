// Decompiled with JetBrains decompiler
// Type: LogicCircuitManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class LogicCircuitManager
{
  public static float ClockTickInterval = 0.1f;
  private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();
  private float elapsedTime;
  private UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduitSystem;
  public System.Action<ILogicUIElement> onElemAdded;
  public System.Action<ILogicUIElement> onElemRemoved;

  public LogicCircuitManager(
    UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduit_system)
  {
    this.conduitSystem = conduit_system;
    this.elapsedTime = 0.0f;
  }

  public void Sim200ms(float dt)
  {
    this.Refresh(dt);
  }

  private void Refresh(float dt)
  {
    if (this.conduitSystem.IsDirty)
    {
      this.conduitSystem.Update();
      this.PropagateSignals(true);
      this.elapsedTime = 0.0f;
    }
    else
    {
      if (this.conduitSystem.GetNetworks().Count <= 0 || !((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null) || SpeedControlScreen.Instance.IsPaused)
        return;
      this.elapsedTime += dt;
      while ((double) this.elapsedTime > (double) LogicCircuitManager.ClockTickInterval)
      {
        this.elapsedTime -= LogicCircuitManager.ClockTickInterval;
        this.PropagateSignals(false);
      }
    }
  }

  private void PropagateSignals(bool force_send_events)
  {
    IList<UtilityNetwork> networks = Game.Instance.logicCircuitSystem.GetNetworks();
    foreach (LogicCircuitNetwork logicCircuitNetwork in (IEnumerable<UtilityNetwork>) networks)
      logicCircuitNetwork.UpdateLogicValue();
    foreach (LogicCircuitNetwork logicCircuitNetwork in (IEnumerable<UtilityNetwork>) networks)
      logicCircuitNetwork.SendLogicEvents(force_send_events);
  }

  public LogicCircuitNetwork GetNetworkForCell(int cell)
  {
    return this.conduitSystem.GetNetworkForCell(cell) as LogicCircuitNetwork;
  }

  public void AddVisElem(ILogicUIElement elem)
  {
    this.uiVisElements.Add(elem);
    if (this.onElemAdded == null)
      return;
    this.onElemAdded(elem);
  }

  public void RemoveVisElem(ILogicUIElement elem)
  {
    if (this.onElemRemoved != null)
      this.onElemRemoved(elem);
    this.uiVisElements.Remove(elem);
  }

  public ReadOnlyCollection<ILogicUIElement> GetVisElements()
  {
    return this.uiVisElements.AsReadOnly();
  }

  public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
  {
    go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, (object) null);
  }

  private struct Signal
  {
    public int cell;
    public int value;

    public Signal(int cell, int value)
    {
      this.cell = cell;
      this.value = value;
    }
  }
}
