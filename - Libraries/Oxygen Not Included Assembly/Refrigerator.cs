// Decompiled with JetBrains decompiler
// Type: Refrigerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : KMonoBehaviour, IUserControlledCapacity, IEffectDescriptor, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.UpdateLogicCircuitCB(data)));
  [SerializeField]
  public float simulatedInternalTemperature = 277.15f;
  [SerializeField]
  public float simulatedInternalHeatCapacity = 400f;
  [SerializeField]
  public float simulatedThermalConductivity = 1000f;
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private LogicPorts ports;
  private FilteredStorage filteredStorage;
  private SimulatedTemperatureAdjuster temperatureAdjuster;

  protected override void OnPrefabInit()
  {
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, new Tag[1]
    {
      GameTags.Compostable
    }, (IUserControlledCapacity) this, true, Db.Get().ChoreTypes.FoodFetch);
  }

  protected override void OnSpawn()
  {
    this.operational.SetActive(this.operational.IsOperational, false);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
    this.filteredStorage.FilterChanged();
    this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, this.GetComponent<Storage>());
    this.UpdateLogicCircuit();
    this.Subscribe<Refrigerator>(-592767678, Refrigerator.OnOperationalChangedDelegate);
    this.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
    this.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
    this.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
  }

  protected override void OnCleanUp()
  {
    this.filteredStorage.CleanUp();
    this.temperatureAdjuster.CleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    this.operational.SetActive(this.operational.IsOperational, false);
  }

  public bool IsActive()
  {
    return this.operational.IsActive;
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    Refrigerator component = gameObject.GetComponent<Refrigerator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetDescriptors(def.BuildingComplete);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
  }

  public float UserMaxCapacity
  {
    get
    {
      return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
    }
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
      this.UpdateLogicCircuit();
    }
  }

  public float AmountStored
  {
    get
    {
      return this.storage.MassStored();
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
      return this.storage.capacityKg;
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

  private void UpdateLogicCircuitCB(object data)
  {
    this.UpdateLogicCircuit();
  }

  private void UpdateLogicCircuit()
  {
    bool flag = this.filteredStorage.IsFull();
    bool isOperational = this.operational.IsOperational;
    bool on = flag && isOperational;
    this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, !on ? 0 : 1);
    this.filteredStorage.SetLogicMeter(on);
  }
}
