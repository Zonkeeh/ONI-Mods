// Decompiled with JetBrains decompiler
// Type: AirConditioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IEffectDescriptor, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>((System.Action<AirConditioner, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>((System.Action<AirConditioner, object>) ((component, data) => component.OnActiveChanged(data)));
  public float temperatureDelta = -14f;
  public float maxEnvironmentDelta = -50f;
  private int cooledAirOutputCell = -1;
  private float lastSampleTime = -1f;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  protected Storage storage;
  [MyCmpReq]
  protected Operational operational;
  [MyCmpReq]
  private ConduitConsumer consumer;
  [MyCmpReq]
  private BuildingComplete building;
  [MyCmpGet]
  private OccupyArea occupyArea;
  private HandleVector<int>.Handle structureTemperature;
  private float lowTempLag;
  private bool showingLowTemp;
  public bool isLiquidConditioner;
  private bool showingHotEnv;
  private Guid statusHandle;
  [Serialize]
  private float targetTemperature;
  private float envTemp;
  private int cellCount;

  public float lastEnvTemp { get; private set; }

  public float lastGasTemp { get; private set; }

  public float TargetTemperature
  {
    get
    {
      return this.targetTemperature;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
    this.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.structureTemperature = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
  }

  public void Sim200ms(float dt)
  {
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null && !this.operational.IsOperational)
      this.operational.SetActive(false, false);
    else
      this.UpdateState(dt);
  }

  private static bool UpdateStateCb(int cell, object data)
  {
    AirConditioner airConditioner = data as AirConditioner;
    ++airConditioner.cellCount;
    airConditioner.envTemp += Grid.Temperature[cell];
    return true;
  }

  private void UpdateState(float dt)
  {
    bool flag = this.consumer.IsSatisfied;
    this.envTemp = 0.0f;
    this.cellCount = 0;
    if ((UnityEngine.Object) this.occupyArea != (UnityEngine.Object) null && (UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
    {
      OccupyArea occupyArea = this.occupyArea;
      int cell = Grid.PosToCell(this.gameObject);
      // ISSUE: reference to a compiler-generated field
      if (AirConditioner.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AirConditioner.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(AirConditioner.UpdateStateCb);
      }
      // ISSUE: reference to a compiler-generated field
      Func<int, object, bool> fMgCache0 = AirConditioner.\u003C\u003Ef__mg\u0024cache0;
      occupyArea.TestArea(cell, (object) this, fMgCache0);
      this.envTemp /= (float) this.cellCount;
    }
    this.lastEnvTemp = this.envTemp;
    List<GameObject> items = this.storage.items;
    for (int index = 0; index < items.Count; ++index)
    {
      PrimaryElement component = items[index].GetComponent<PrimaryElement>();
      if ((double) component.Mass > 0.0 && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
      {
        flag = true;
        this.lastGasTemp = component.Temperature;
        float temperature = component.Temperature + this.temperatureDelta;
        if ((double) temperature < 1.0)
        {
          temperature = 1f;
          this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
        }
        else
          this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0.0f);
        float num1 = (!this.isLiquidConditioner ? Game.Instance.gasConduitFlow : Game.Instance.liquidConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, temperature, component.DiseaseIdx, component.DiseaseCount);
        component.KeepZeroMassObject = true;
        float num2 = num1 / component.Mass;
        int num3 = (int) ((double) component.DiseaseCount * (double) num2);
        component.Mass -= num1;
        component.ModifyDiseaseCount(-num3, "AirConditioner.UpdateState");
        float num4 = (temperature - component.Temperature) * component.Element.specificHeatCapacity * num1;
        float display_dt = (double) this.lastSampleTime <= 0.0 ? 1f : Time.time - this.lastSampleTime;
        this.lastSampleTime = Time.time;
        GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num4, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
        break;
      }
    }
    if ((double) Time.time - (double) this.lastSampleTime > 2.0)
    {
      GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0.0f, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
      this.lastSampleTime = Time.time;
    }
    this.operational.SetActive(flag, false);
    this.UpdateStatus();
  }

  private void OnOperationalChanged(object data)
  {
    if (!this.operational.IsOperational)
      return;
    this.UpdateState(0.0f);
  }

  private void OnActiveChanged(object data)
  {
    this.UpdateStatus();
  }

  private void UpdateStatus()
  {
    if (this.operational.IsActive)
    {
      if ((double) this.lowTempLag >= 1.0 && !this.showingLowTemp)
      {
        this.statusHandle = !this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, (object) this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, (object) this);
        this.showingLowTemp = true;
        this.showingHotEnv = false;
      }
      else if ((double) this.lowTempLag <= 0.0 && (this.showingHotEnv || this.showingLowTemp))
      {
        this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, (object) null);
        this.showingLowTemp = false;
        this.showingHotEnv = false;
      }
      else
      {
        if (!(this.statusHandle == Guid.Empty))
          return;
        this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, (object) null);
        this.showingLowTemp = false;
        this.showingHotEnv = false;
      }
    }
    else
      this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null, (object) null);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
    float dtu = Mathf.Abs(this.temperatureDelta * ElementLoader.FindElementByName(!this.isLiquidConditioner ? "Oxygen" : "Water").specificHeatCapacity);
    float dtu_s = dtu * 1f;
    Descriptor descriptor1 = new Descriptor();
    string txt = string.Format((string) (!this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER), (object) GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
    string tooltip = string.Format((string) (!this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER), (object) GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic));
    descriptor1.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor1);
    Descriptor descriptor2 = new Descriptor();
    descriptor2.SetupDescriptor(string.Format((string) (!this.isLiquidConditioner ? UI.BUILDINGEFFECTS.GASCOOLING : UI.BUILDINGEFFECTS.LIQUIDCOOLING), (object) formattedTemperature), string.Format((string) (!this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING), (object) formattedTemperature), Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor2);
    return descriptorList;
  }
}
