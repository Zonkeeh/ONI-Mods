// Decompiled with JetBrains decompiler
// Type: EnergyGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class EnergyGenerator : Generator, IEffectDescriptor, ISingleSliderControl, ISliderControl
{
  private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>((System.Action<EnergyGenerator, object>) ((component, data) => component.OnActiveChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>((System.Action<EnergyGenerator, object>) ((component, data) => component.OnCopySettings(data)));
  [SerializeField]
  [Serialize]
  private float batteryRefillPercent = 0.5f;
  public bool hasMeter = true;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private ManualDeliveryKG delivery;
  public bool ignoreBatteryRefillPercent;
  private static StatusItem batteriesSufficientlyFull;
  public Meter.Offset meterOffset;
  [SerializeField]
  public EnergyGenerator.Formula formula;
  private MeterController meter;

  public string SliderTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";
    }
  }

  public string SliderUnits
  {
    get
    {
      return (string) UI.UNITSUFFIXES.PERCENT;
    }
  }

  public int SliderDecimalPlaces(int index)
  {
    return 0;
  }

  public float GetSliderMin(int index)
  {
    return 0.0f;
  }

  public float GetSliderMax(int index)
  {
    return 100f;
  }

  public float GetSliderValue(int index)
  {
    return this.batteryRefillPercent * 100f;
  }

  public void SetSliderValue(float value, int index)
  {
    this.batteryRefillPercent = value / 100f;
  }

  string ISliderControl.GetSliderTooltip()
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), (object) this.GetComponent<ManualDeliveryKG>().requestedItemTag.ProperName(), (object) (float) ((double) this.batteryRefillPercent * 100.0));
  }

  public string GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    EnergyGenerator.EnsureStatusItemAvailable();
    this.Subscribe<EnergyGenerator>(824508782, EnergyGenerator.OnActiveChangedDelegate);
    if (this.ignoreBatteryRefillPercent)
      return;
    this.gameObject.AddOrGet<CopyBuildingSettings>();
    this.Subscribe<EnergyGenerator>(-905833192, EnergyGenerator.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    EnergyGenerator component = ((GameObject) data).GetComponent<EnergyGenerator>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.batteryRefillPercent = component.batteryRefillPercent;
  }

  protected void OnActiveChanged(object data)
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !((Operational) data).IsActive ? Db.Get().BuildingStatusItems.GeneratorOffline : Db.Get().BuildingStatusItems.Wattage, (object) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.hasMeter)
      return;
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
  }

  private bool IsConvertible(float dt)
  {
    bool flag = true;
    foreach (EnergyGenerator.InputItem input in this.formula.inputs)
    {
      GameObject first = this.storage.FindFirst(input.tag);
      if ((UnityEngine.Object) first != (UnityEngine.Object) null)
      {
        PrimaryElement component = first.GetComponent<PrimaryElement>();
        float num = input.consumptionRate * dt;
        flag = flag && (double) component.Mass >= (double) num;
      }
      else
        flag = false;
      if (!flag)
        break;
    }
    return flag;
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    if (this.hasMeter)
    {
      EnergyGenerator.InputItem input = this.formula.inputs[0];
      float percent_full = 0.0f;
      GameObject first = this.storage.FindFirst(input.tag);
      if ((UnityEngine.Object) first != (UnityEngine.Object) null)
        percent_full = first.GetComponent<PrimaryElement>().Mass / input.maxStoredMass;
      this.meter.SetPositionPercent(percent_full);
    }
    ushort circuitId = this.CircuitID;
    this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
    bool flag1 = false;
    if (this.operational.IsOperational)
    {
      bool flag2 = false;
      List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId);
      if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
      {
        foreach (Battery battery in batteriesOnCircuit)
        {
          if ((double) this.batteryRefillPercent <= 0.0 && (double) battery.PercentFull <= 0.0)
          {
            flag2 = true;
            break;
          }
          if ((double) battery.PercentFull < (double) this.batteryRefillPercent)
          {
            flag2 = true;
            break;
          }
        }
      }
      else
        flag2 = true;
      if (!this.ignoreBatteryRefillPercent)
        this.selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !flag2, (object) null);
      if ((UnityEngine.Object) this.delivery != (UnityEngine.Object) null)
        this.delivery.Pause(!flag2, "Circuit has sufficient energy");
      if (this.formula.inputs != null)
      {
        bool flag3 = this.IsConvertible(dt);
        this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag3, (object) this.formula);
        if (flag3)
        {
          foreach (EnergyGenerator.InputItem input in this.formula.inputs)
          {
            float amount = input.consumptionRate * dt;
            this.storage.ConsumeIgnoringDisease(input.tag, amount);
          }
          PrimaryElement component = this.GetComponent<PrimaryElement>();
          foreach (EnergyGenerator.OutputItem output in this.formula.outputs)
            this.Emit(output, dt, component);
          this.GenerateJoules(this.WattageRating * dt, false);
          this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, (object) this);
          flag1 = true;
        }
      }
    }
    this.operational.SetActive(flag1, false);
  }

  public List<Descriptor> RequirementDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.formula.inputs == null || this.formula.inputs.Length == 0)
      return descriptorList;
    for (int index = 0; index < this.formula.inputs.Length; ++index)
    {
      EnergyGenerator.InputItem input = this.formula.inputs[index];
      string str = input.tag.ProperName();
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass(input.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass(input.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public List<Descriptor> EffectDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.formula.outputs == null || this.formula.outputs.Length == 0)
      return descriptorList;
    for (int index = 0; index < this.formula.outputs.Length; ++index)
    {
      EnergyGenerator.OutputItem output = this.formula.outputs[index];
      string str = ElementLoader.FindElementByHash(output.element).tag.ProperName();
      Descriptor descriptor = new Descriptor();
      if ((double) output.minTemperature > 0.0)
        descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, (object) str, (object) GameUtil.GetFormattedMass(output.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(output.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, (object) str, (object) GameUtil.GetFormattedMass(output.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(output.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
      else
        descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, (object) str, (object) GameUtil.GetFormattedMass(output.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, (object) str, (object) GameUtil.GetFormattedMass(output.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
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

  public static StatusItem BatteriesSufficientlyFull
  {
    get
    {
      return EnergyGenerator.batteriesSufficientlyFull;
    }
  }

  public static void EnsureStatusItemAvailable()
  {
    if (EnergyGenerator.batteriesSufficientlyFull != null)
      return;
    EnergyGenerator.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
  }

  public static EnergyGenerator.Formula CreateSimpleFormula(
    Tag input_element,
    float input_mass_rate,
    float max_stored_input_mass,
    SimHashes output_element = SimHashes.Void,
    float output_mass_rate = 0.0f,
    bool store_output_mass = true,
    CellOffset output_offset = default (CellOffset),
    float min_output_temperature = 0.0f)
  {
    EnergyGenerator.Formula formula = new EnergyGenerator.Formula();
    formula.inputs = new EnergyGenerator.InputItem[1]
    {
      new EnergyGenerator.InputItem(input_element, input_mass_rate, max_stored_input_mass)
    };
    if (output_element != SimHashes.Void)
      formula.outputs = new EnergyGenerator.OutputItem[1]
      {
        new EnergyGenerator.OutputItem(output_element, output_mass_rate, store_output_mass, output_offset, min_output_temperature)
      };
    else
      formula.outputs = (EnergyGenerator.OutputItem[]) null;
    return formula;
  }

  private void Emit(EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
  {
    Element elementByHash = ElementLoader.FindElementByHash(output.element);
    float num1 = output.creationRate * dt;
    if (output.store)
    {
      if (elementByHash.IsGas)
        this.storage.AddGasChunk(output.element, num1, root_pe.Temperature, byte.MaxValue, 0, true, true);
      else if (elementByHash.IsLiquid)
        this.storage.AddLiquid(output.element, num1, root_pe.Temperature, byte.MaxValue, 0, true, true);
      else
        this.storage.Store(elementByHash.substance.SpawnResource(this.transform.GetPosition(), num1, root_pe.Temperature, byte.MaxValue, 0, false, false, false), true, false, true, false);
    }
    else
    {
      int num2 = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), output.emitOffset);
      float temperature = Mathf.Max(root_pe.Temperature, output.minTemperature);
      if (elementByHash.IsGas)
        SimMessages.ModifyMass(num2, num1, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temperature, output.element);
      else if (elementByHash.IsLiquid)
      {
        int elementIndex = ElementLoader.GetElementIndex(output.element);
        FallingWater.instance.AddParticle(num2, (byte) elementIndex, num1, temperature, byte.MaxValue, 0, true, false, false, false);
      }
      else
        elementByHash.substance.SpawnResource(Grid.CellToPosCCC(num2, Grid.SceneLayer.Front), num1, temperature, byte.MaxValue, 0, true, false, false);
    }
  }

  [DebuggerDisplay("{tag} -{consumptionRate} kg/s")]
  [Serializable]
  public struct InputItem
  {
    public Tag tag;
    public float consumptionRate;
    public float maxStoredMass;

    public InputItem(Tag tag, float consumption_rate, float max_stored_mass)
    {
      this.tag = tag;
      this.consumptionRate = consumption_rate;
      this.maxStoredMass = max_stored_mass;
    }
  }

  [DebuggerDisplay("{element} {creationRate} kg/s")]
  [Serializable]
  public struct OutputItem
  {
    public SimHashes element;
    public float creationRate;
    public bool store;
    public CellOffset emitOffset;
    public float minTemperature;

    public OutputItem(SimHashes element, float creation_rate, bool store, float min_temperature = 0.0f)
    {
      this = new EnergyGenerator.OutputItem(element, creation_rate, store, CellOffset.none, min_temperature);
    }

    public OutputItem(
      SimHashes element,
      float creation_rate,
      bool store,
      CellOffset emit_offset,
      float min_temperature = 0.0f)
    {
      this.element = element;
      this.creationRate = creation_rate;
      this.store = store;
      this.emitOffset = emit_offset;
      this.minTemperature = min_temperature;
    }
  }

  [Serializable]
  public struct Formula
  {
    public EnergyGenerator.InputItem[] inputs;
    public EnergyGenerator.OutputItem[] outputs;
    public Tag meterTag;
  }
}
