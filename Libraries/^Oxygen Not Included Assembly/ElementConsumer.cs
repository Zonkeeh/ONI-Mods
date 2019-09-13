// Decompiled with JetBrains decompiler
// Type: ElementConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IEffectDescriptor
{
  private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();
  private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>((System.Action<ElementConsumer, object>) ((component, data) => component.OnActiveChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>((System.Action<ElementConsumer, object>) ((component, data) => component.OnStorageChange(data)));
  [HashedEnum]
  [SerializeField]
  public SimHashes elementToConsume = SimHashes.Vacuum;
  [SerializeField]
  public byte consumptionRadius = 1;
  [SerializeField]
  public bool showInStatusPanel = true;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
  public bool showDescriptor = true;
  public bool isRequired = true;
  private bool hasAvailableCapacity = true;
  [SerializeField]
  public float consumptionRate;
  [SerializeField]
  public float minimumMass;
  [SerializeField]
  public Vector3 sampleCellOffset;
  [SerializeField]
  public ElementConsumer.Configuration configuration;
  [Serialize]
  [NonSerialized]
  public float consumedMass;
  [Serialize]
  [NonSerialized]
  public float consumedTemperature;
  [SerializeField]
  public bool storeOnConsume;
  [MyCmpGet]
  public Storage storage;
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private KSelectable selectable;
  private Guid statusHandle;
  private bool consumptionEnabled;

  public float AverageConsumeRate
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.accumulator);
    }
  }

  public static void ClearInstanceMap()
  {
    ElementConsumer.handleInstanceMap.Clear();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.accumulator = Game.Instance.accumulators.Add("Element", (KMonoBehaviour) this);
    if (this.elementToConsume == SimHashes.Void)
      throw new ArgumentException("No consumable elements specified");
    this.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
    if ((double) this.capacityKG == double.PositiveInfinity)
      return;
    this.hasAvailableCapacity = !this.IsStorageFull();
    this.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.accumulator);
    base.OnCleanUp();
  }

  protected virtual bool IsActive()
  {
    if (!((UnityEngine.Object) this.operational == (UnityEngine.Object) null))
      return this.operational.IsActive;
    return true;
  }

  public void EnableConsumption(bool enabled)
  {
    bool consumptionEnabled = this.consumptionEnabled;
    this.consumptionEnabled = enabled;
    if (!Sim.IsValidHandle(this.simHandle) || enabled == consumptionEnabled)
      return;
    this.UpdateSimData();
  }

  private bool IsStorageFull()
  {
    PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      return (double) primaryElement.Mass >= (double) this.capacityKG;
    return false;
  }

  public void RefreshConsumptionRate()
  {
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    this.UpdateSimData();
  }

  private void UpdateSimData()
  {
    Debug.Assert(Sim.IsValidHandle(this.simHandle));
    SimMessages.SetElementConsumerData(this.simHandle, this.GetSampleCell(), !this.consumptionEnabled || !this.hasAvailableCapacity ? 0.0f : this.consumptionRate);
    this.UpdateStatusItem();
  }

  public static void AddMass(Sim.ConsumedMassInfo consumed_info)
  {
    ElementConsumer elementConsumer;
    if (!Sim.IsValidHandle(consumed_info.simHandle) || !ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
      return;
    elementConsumer.AddMassInternal(consumed_info);
  }

  private int GetSampleCell()
  {
    return Grid.PosToCell(this.transform.GetPosition() + this.sampleCellOffset);
  }

  private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
  {
    if ((double) consumed_info.mass > 0.0)
    {
      if (this.storeOnConsume)
      {
        Element element = ElementLoader.elements[(int) consumed_info.removedElemIdx];
        if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
        {
          if (element.IsLiquid)
            this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
          else if (element.IsGas)
            this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
        }
      }
      else
      {
        this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
        this.consumedMass += consumed_info.mass;
      }
    }
    Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
  }

  public bool IsElementAvailable
  {
    get
    {
      int sampleCell = this.GetSampleCell();
      if (this.elementToConsume == Grid.Element[sampleCell].id)
        return (double) Grid.Mass[sampleCell] >= (double) this.minimumMass;
      return false;
    }
  }

  private void UpdateStatusItem()
  {
    if (!this.showInStatusPanel)
      return;
    if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
    {
      this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, (object) this);
    }
    else
    {
      if (!(this.statusHandle != Guid.Empty))
        return;
      this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
    }
  }

  private void OnStorageChange(object data)
  {
    bool flag = !this.IsStorageFull();
    if (flag == this.hasAvailableCapacity)
      return;
    this.hasAvailableCapacity = flag;
    this.RefreshConsumptionRate();
  }

  protected override void OnCmpEnable()
  {
    if (!this.isSpawned || !this.IsActive())
      return;
    this.UpdateStatusItem();
  }

  protected override void OnCmpDisable()
  {
    this.UpdateStatusItem();
  }

  public List<Descriptor> RequirementDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.isRequired && this.showDescriptor)
    {
      Element elementByHash = ElementLoader.FindElementByHash(this.elementToConsume);
      string str = elementByHash.tag.ProperName();
      if (elementByHash.IsVacuum)
        str = this.configuration != ElementConsumer.Configuration.AllGas ? (this.configuration != ElementConsumer.Configuration.AllLiquid ? (string) UI.BUILDINGEFFECTS.CONSUMESANYELEMENT : (string) ELEMENTS.STATE.LIQUID) : (string) ELEMENTS.STATE.GAS;
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.REQUIRESELEMENT, (object) str), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, (object) str), Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public List<Descriptor> EffectDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.showDescriptor)
    {
      Element elementByHash = ElementLoader.FindElementByHash(this.elementToConsume);
      string str = elementByHash.tag.ProperName();
      if (elementByHash.IsVacuum)
        str = this.configuration != ElementConsumer.Configuration.AllGas ? (this.configuration != ElementConsumer.Configuration.AllLiquid ? (string) UI.BUILDINGEFFECTS.CONSUMESANYELEMENT : (string) ELEMENTS.STATE.LIQUID) : (string) ELEMENTS.STATE.GAS;
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass((float) ((double) this.consumptionRate / 100.0 * 100.0), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass((float) ((double) this.consumptionRate / 100.0 * 100.0), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (Descriptor requirementDescriptor in this.RequirementDescriptors(def))
      descriptorList.Add(requirementDescriptor);
    foreach (Descriptor effectDescriptor in this.EffectDescriptors(def))
      descriptorList.Add(effectDescriptor);
    return descriptorList;
  }

  private void OnActiveChanged(object data)
  {
    this.EnableConsumption(this.operational.IsActive);
  }

  protected override void OnSimUnregister()
  {
    Debug.Assert(Sim.IsValidHandle(this.simHandle));
    ElementConsumer.handleInstanceMap.Remove(this.simHandle);
    ElementConsumer.StaticUnregister(this.simHandle);
  }

  protected override void OnSimRegister(
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
  {
    SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
  }

  protected override System.Action<int> GetStaticUnregister()
  {
    // ISSUE: reference to a compiler-generated field
    if (ElementConsumer.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ElementConsumer.\u003C\u003Ef__mg\u0024cache0 = new System.Action<int>(ElementConsumer.StaticUnregister);
    }
    // ISSUE: reference to a compiler-generated field
    return ElementConsumer.\u003C\u003Ef__mg\u0024cache0;
  }

  private static void StaticUnregister(int sim_handle)
  {
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    SimMessages.RemoveElementConsumer(-1, sim_handle);
  }

  protected override void OnSimRegistered()
  {
    if (this.consumptionEnabled)
      this.UpdateSimData();
    ElementConsumer.handleInstanceMap[this.simHandle] = this;
  }

  public enum Configuration
  {
    Element,
    AllLiquid,
    AllGas,
  }
}
