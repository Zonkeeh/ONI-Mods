// Decompiled with JetBrains decompiler
// Type: SimulatedTemperatureAdjuster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedTemperatureAdjuster
{
  private float temperature;
  private float heatCapacity;
  private float thermalConductivity;
  private bool operational;
  private Storage storage;

  public SimulatedTemperatureAdjuster(
    float simulated_temperature,
    float heat_capacity,
    float thermal_conductivity,
    Storage storage)
  {
    this.temperature = simulated_temperature;
    this.heatCapacity = heat_capacity;
    this.thermalConductivity = thermal_conductivity;
    this.storage = storage;
    storage.gameObject.Subscribe(-592767678, new System.Action<object>(this.OnOperationalChanged));
    storage.gameObject.Subscribe(-1697596308, new System.Action<object>(this.OnStorageChanged));
    this.operational = true;
    Operational component = storage.gameObject.GetComponent<Operational>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.operational = component.IsOperational;
    this.OnOperationalChanged((object) this.operational);
  }

  public List<Descriptor> GetDescriptors()
  {
    return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
  }

  public static List<Descriptor> GetDescriptors(float temperature)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string formattedTemperature = GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, (object) formattedTemperature), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, (object) formattedTemperature), Descriptor.DescriptorType.Effect, false);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  private void Register(SimTemperatureTransfer stt)
  {
    stt.onSimRegistered -= new System.Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    stt.onSimRegistered += new System.Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    if (!Sim.IsValidHandle(stt.SimHandle))
      return;
    this.OnItemSimRegistered(stt);
  }

  private void Unregister(SimTemperatureTransfer stt)
  {
    stt.onSimRegistered -= new System.Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    if (!Sim.IsValidHandle(stt.SimHandle))
      return;
    SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0.0f, 0.0f, 0.0f);
  }

  private void OnItemSimRegistered(SimTemperatureTransfer stt)
  {
    if ((UnityEngine.Object) stt == (UnityEngine.Object) null || !Sim.IsValidHandle(stt.SimHandle))
      return;
    float temperature = this.temperature;
    float heat_capacity = this.heatCapacity;
    float thermal_conductivity = this.thermalConductivity;
    if (!this.operational)
    {
      temperature = 0.0f;
      heat_capacity = 0.0f;
      thermal_conductivity = 0.0f;
    }
    SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, temperature, heat_capacity, thermal_conductivity);
  }

  private void OnOperationalChanged(object data)
  {
    this.operational = (bool) data;
    if (this.operational)
    {
      foreach (GameObject gameObject in this.storage.items)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          this.OnItemSimRegistered(gameObject.GetComponent<SimTemperatureTransfer>());
      }
    }
    else
    {
      foreach (GameObject gameObject in this.storage.items)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          this.Unregister(gameObject.GetComponent<SimTemperatureTransfer>());
      }
    }
  }

  public void CleanUp()
  {
    this.storage.gameObject.Unsubscribe(-1697596308, new System.Action<object>(this.OnStorageChanged));
    foreach (GameObject gameObject in this.storage.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        this.Unregister(gameObject.GetComponent<SimTemperatureTransfer>());
    }
  }

  private void OnStorageChanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    SimTemperatureTransfer component1 = gameObject.GetComponent<SimTemperatureTransfer>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    Pickupable component2 = gameObject.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    if (this.operational && (UnityEngine.Object) component2.storage == (UnityEngine.Object) this.storage)
      this.Register(component1);
    else
      this.Unregister(component1);
  }
}
