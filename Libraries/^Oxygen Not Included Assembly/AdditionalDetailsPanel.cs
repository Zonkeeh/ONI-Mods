// Decompiled with JetBrains decompiler
// Type: AdditionalDetailsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalDetailsPanel : TargetScreen
{
  public GameObject attributesLabelTemplate;
  private GameObject detailsPanel;
  private DetailsPanelDrawer drawer;

  public override bool IsValidForTarget(GameObject target)
  {
    return true;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.detailsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.drawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.detailsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
  }

  private void Update()
  {
    this.Refresh();
  }

  public override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  public override void OnDeselectTarget(GameObject target)
  {
    base.OnDeselectTarget(target);
  }

  private void Refresh()
  {
    this.drawer.BeginDrawing();
    this.RefreshDetails();
    this.drawer.EndDrawing();
  }

  private GameObject AddOrGetLabel(
    Dictionary<string, GameObject> labels,
    GameObject panel,
    string id)
  {
    GameObject gameObject;
    if (labels.ContainsKey(id))
    {
      gameObject = labels[id];
    }
    else
    {
      gameObject = Util.KInstantiate(this.attributesLabelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, (string) null);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      labels[id] = gameObject;
    }
    gameObject.SetActive(true);
    return gameObject;
  }

  private void RefreshDetails()
  {
    this.detailsPanel.SetActive(true);
    this.detailsPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.DETAILS.GROUPNAME_DETAILS;
    PrimaryElement component1 = this.selectedTarget.GetComponent<PrimaryElement>();
    CellSelectionObject component2 = this.selectedTarget.GetComponent<CellSelectionObject>();
    float mass;
    float temperature;
    Element element;
    byte diseaseIdx;
    int diseaseCount;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      mass = component1.Mass;
      temperature = component1.Temperature;
      element = component1.Element;
      diseaseIdx = component1.DiseaseIdx;
      diseaseCount = component1.DiseaseCount;
    }
    else
    {
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      mass = component2.Mass;
      temperature = component2.temperature;
      element = component2.element;
      diseaseIdx = component2.diseaseIdx;
      diseaseCount = component2.diseaseCount;
    }
    bool flag1 = element.id == SimHashes.Vacuum || element.id == SimHashes.Void;
    float specificHeatCapacity = element.specificHeatCapacity;
    float highTemp = element.highTemp;
    float lowTemp = element.lowTemp;
    BuildingComplete component3 = this.selectedTarget.GetComponent<BuildingComplete>();
    float num1 = !((UnityEngine.Object) component3 != (UnityEngine.Object) null) ? -1f : component3.creationTime;
    LogicPorts component4 = this.selectedTarget.GetComponent<LogicPorts>();
    EnergyConsumer component5 = this.selectedTarget.GetComponent<EnergyConsumer>();
    Operational component6 = this.selectedTarget.GetComponent<Operational>();
    Battery component7 = this.selectedTarget.GetComponent<Battery>();
    float num2;
    float num3;
    float num4;
    if ((UnityEngine.Object) component6 != (UnityEngine.Object) null && ((UnityEngine.Object) component4 != (UnityEngine.Object) null || (UnityEngine.Object) component5 != (UnityEngine.Object) null || (UnityEngine.Object) component7 != (UnityEngine.Object) null))
    {
      num2 = component6.GetUptimeForTimeSpan(10f);
      num3 = component6.GetUptimeForTimeSpan(600f);
      num4 = component6.GetUptimeForTimeSpan(6000f);
    }
    else
    {
      num2 = -1f;
      num3 = -1f;
      num4 = -1f;
    }
    this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.PRIMARYELEMENT.NAME, element.name)).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.PRIMARYELEMENT.TOOLTIP, element.name)).NewLabel(this.drawer.Format((string) UI.ELEMENTAL.MASS.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.MASS.TOOLTIP, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
    if ((double) num1 > 0.0)
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.AGE.NAME, Util.FormatTwoDecimalPlace((float) (((double) GameClock.Instance.GetTime() - (double) num1) / 600.0)))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.AGE.TOOLTIP, Util.FormatTwoDecimalPlace((float) (((double) GameClock.Instance.GetTime() - (double) num1) / 600.0))));
    else
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.AGE.NAME, (string) UI.ELEMENTAL.AGE.UNKNOWN)).Tooltip((string) UI.ELEMENTAL.AGE.UNKNOWN_TOOLTIP);
    if ((double) num2 >= 0.0)
      this.drawer.NewLabel((string) UI.ELEMENTAL.UPTIME.NAME.Replace("{0}", GameUtil.GetFormattedTime(10f)).Replace("{1}", GameUtil.GetFormattedTime(600f)).Replace("{2}", GameUtil.GetFormattedTime(6000f)).Replace("{3}", GameUtil.GetFormattedPercent(num2 * 100f, GameUtil.TimeSlice.None)).Replace("{4}", GameUtil.GetFormattedPercent(num3 * 100f, GameUtil.TimeSlice.None)).Replace("{5}", GameUtil.GetFormattedPercent(num4 * 100f, GameUtil.TimeSlice.None)));
    if (!flag1)
    {
      bool flag2 = false;
      float thermalConductivity = element.thermalConductivity;
      Building component8 = this.selectedTarget.GetComponent<Building>();
      if ((UnityEngine.Object) component8 != (UnityEngine.Object) null)
      {
        thermalConductivity *= component8.Def.ThermalConductivity;
        flag2 = (double) component8.Def.ThermalConductivity < 1.0;
      }
      string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
      float shc = specificHeatCapacity * 1f;
      string text1 = string.Format((string) UI.ELEMENTAL.SHC.NAME, (object) GameUtil.GetDisplaySHC(shc).ToString("0.000"));
      string tooltip_text1 = (string) UI.ELEMENTAL.SHC.TOOLTIP.Replace("{SPECIFIC_HEAT_CAPACITY}", text1 + GameUtil.GetSHCSuffix()).Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
      string text2 = string.Format((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.NAME, (object) GameUtil.GetDisplayThermalConductivity(thermalConductivity).ToString("0.000"));
      string tooltip_text2 = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.TOOLTIP.Replace("{THERMAL_CONDUCTIVITY}", text2 + GameUtil.GetThermalConductivitySuffix()).Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.TEMPERATURE.NAME, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.TEMPERATURE.TOOLTIP, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).NewLabel(this.drawer.Format((string) UI.ELEMENTAL.DISEASE.NAME, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.DISEASE.TOOLTIP, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, true))).NewLabel(text1).Tooltip(tooltip_text1).NewLabel(text2).Tooltip(tooltip_text2);
      if (flag2)
        this.drawer.NewLabel((string) UI.GAMEOBJECTEFFECTS.INSULATED.NAME).Tooltip((string) UI.GAMEOBJECTEFFECTS.INSULATED.TOOLTIP);
    }
    if (element.IsSolid)
    {
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.MELTINGPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
      if ((UnityEngine.Object) this.selectedTarget.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
      {
        AttributeModifier attributeModifier = component1.Element.attributeModifiers.Find((Predicate<AttributeModifier>) (m => m.AttributeId == Db.Get().BuildingAttributes.OverheatTemperature.Id));
        if (attributeModifier != null)
          this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.OVERHEATPOINT.NAME, attributeModifier.GetFormattedString(this.selectedTarget.gameObject))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.OVERHEATPOINT.TOOLTIP, attributeModifier.GetFormattedString(this.selectedTarget.gameObject)));
      }
    }
    else if (element.IsLiquid)
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.FREEZEPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.FREEZEPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).NewLabel(this.drawer.Format((string) UI.ELEMENTAL.VAPOURIZATIONPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.VAPOURIZATIONPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
    else if (!flag1)
      this.drawer.NewLabel(this.drawer.Format((string) UI.ELEMENTAL.DEWPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))).Tooltip(this.drawer.Format((string) UI.ELEMENTAL.DEWPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
    Attributes attributes = this.selectedTarget.GetAttributes();
    if (attributes != null)
    {
      for (int index = 0; index < attributes.Count; ++index)
      {
        AttributeInstance attributeInstance = attributes.AttributeTable[index];
        if (attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Details || attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Expectation)
          this.drawer.NewLabel(attributeInstance.modifier.Name + ": " + attributeInstance.GetFormattedValue()).Tooltip(attributeInstance.GetAttributeValueTooltip());
      }
    }
    List<Descriptor> detailDescriptors = GameUtil.GetDetailDescriptors(GameUtil.GetAllDescriptors(this.selectedTarget, false));
    for (int index = 0; index < detailDescriptors.Count; ++index)
    {
      Descriptor descriptor = detailDescriptors[index];
      this.drawer.NewLabel(descriptor.text).Tooltip(descriptor.tooltipText);
    }
  }
}
