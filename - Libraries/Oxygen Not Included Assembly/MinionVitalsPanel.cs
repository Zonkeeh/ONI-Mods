// Decompiled with JetBrains decompiler
// Type: MinionVitalsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MinionVitalsPanel : KMonoBehaviour
{
  public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();
  public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();
  public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();
  public GameObject LineItemPrefab;
  public GameObject CheckboxLinePrefab;
  public GameObject selectedEntity;
  public Transform conditionsContainerNormal;
  public Transform conditionsContainerAdditional;

  public void Init()
  {
    this.AddAmountLine(Db.Get().Amounts.HitPoints, (Func<AmountInstance, string>) null);
    this.AddAttributeLine(Db.Get().CritterAttributes.Happiness, (Func<AttributeInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Wildness, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Incubation, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Viability, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Fertility, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Age, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Stress, (Func<AmountInstance, string>) null);
    this.AddAttributeLine(Db.Get().Attributes.QualityOfLife, (Func<AttributeInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Bladder, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Breath, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Stamina, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Calories, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.ScaleGrowth, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Temperature, (Func<AmountInstance, string>) null);
    this.AddAmountLine(Db.Get().Amounts.Decor, (Func<AmountInstance, string>) null);
    this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetAirPressureLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<PressureVulnerable>() != (UnityEngine.Object) null && go.GetComponent<PressureVulnerable>().pressure_sensitive ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_pressure(go)), (Func<GameObject, string>) (go => this.GetAirPressureTooltip(go)));
    this.AddCheckboxLine((Amount) null, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetAtmosphereLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<PressureVulnerable>() != (UnityEngine.Object) null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0 ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_atmosphere(go)), (Func<GameObject, string>) (go => this.GetAtmosphereTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetInternalTemperatureLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<TemperatureVulnerable>() != (UnityEngine.Object) null ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_temperature(go)), (Func<GameObject, string>) (go => this.GetInternalTemperatureTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (Func<GameObject, string>) (go => this.GetFertilizationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go =>
    {
      if ((UnityEngine.Object) go.GetComponent<ReceptacleMonitor>() == (UnityEngine.Object) null)
        return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
      return go.GetComponent<ReceptacleMonitor>().Replanted ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
    }), (Func<GameObject, bool>) (go => this.check_fertilizer(go)), (Func<GameObject, string>) (go => this.GetFertilizationTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (Func<GameObject, string>) (go => this.GetIrrigationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go =>
    {
      ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.Replanted ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
    }), (Func<GameObject, bool>) (go => this.check_irrigation(go)), (Func<GameObject, string>) (go => this.GetIrrigationTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetIlluminationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => MinionVitalsPanel.CheckboxLineDisplayType.Normal), (Func<GameObject, bool>) (go => this.check_illumination(go)), (Func<GameObject, string>) (go => this.GetIlluminationTooltip(go)));
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    SimAndRenderScheduler.instance.Add((object) this, false);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    SimAndRenderScheduler.instance.Remove((object) this);
  }

  private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.gameObject, false);
    gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite((HashedString) amount.uiSprite);
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    this.amountsLines.Add(new MinionVitalsPanel.AmountLine()
    {
      amount = amount,
      go = gameObject,
      locText = gameObject.GetComponentInChildren<LocText>(),
      toolTip = gameObject.GetComponentInChildren<ToolTip>(),
      imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>(),
      toolTipFunc = tooltip_func == null ? new Func<AmountInstance, string>(amount.GetTooltip) : tooltip_func
    });
  }

  private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.gameObject, false);
    gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite((HashedString) attribute.uiSprite);
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    MinionVitalsPanel.AttributeLine attributeLine = new MinionVitalsPanel.AttributeLine();
    attributeLine.attribute = attribute;
    attributeLine.go = gameObject;
    attributeLine.locText = gameObject.GetComponentInChildren<LocText>();
    attributeLine.toolTip = gameObject.GetComponentInChildren<ToolTip>();
    gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
    attributeLine.toolTipFunc = tooltip_func == null ? new Func<AttributeInstance, string>(attribute.GetTooltip) : tooltip_func;
    this.attributesLines.Add(attributeLine);
  }

  private void AddCheckboxLine(
    Amount amount,
    Transform parentContainer,
    Func<GameObject, string> label_text_func,
    Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition,
    Func<GameObject, bool> checkbox_value_func,
    Func<GameObject, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, this.gameObject, false);
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    MinionVitalsPanel.CheckboxLine checkboxLine = new MinionVitalsPanel.CheckboxLine();
    checkboxLine.go = gameObject;
    checkboxLine.parentContainer = parentContainer;
    checkboxLine.amount = amount;
    checkboxLine.locText = component.GetReference("Label") as LocText;
    checkboxLine.get_value = checkbox_value_func;
    checkboxLine.display_condition = display_condition;
    checkboxLine.label_text_func = label_text_func;
    checkboxLine.go.name = "Checkbox_";
    if (amount != null)
      checkboxLine.go.name += amount.Name;
    else
      checkboxLine.go.name += "Unnamed";
    if (tooltip_func != null)
    {
      checkboxLine.tooltip = tooltip_func;
      ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
      tt.refreshWhileHovering = true;
      tt.OnToolTip = (Func<string>) (() =>
      {
        tt.ClearMultiStringTooltip();
        tt.AddMultiStringTooltip(tooltip_func(this.selectedEntity), (ScriptableObject) null);
        return string.Empty;
      });
    }
    this.checkboxLines.Add(checkboxLine);
  }

  public void Refresh()
  {
    if ((UnityEngine.Object) this.selectedEntity == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedEntity.gameObject == (UnityEngine.Object) null)
      return;
    Amounts amounts = this.selectedEntity.GetAmounts();
    Attributes attributes = this.selectedEntity.GetAttributes();
    if (amounts == null || attributes == null)
      return;
    WiltCondition component1 = this.selectedEntity.GetComponent<WiltCondition>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      this.conditionsContainerNormal.gameObject.SetActive(false);
      this.conditionsContainerAdditional.gameObject.SetActive(false);
      foreach (MinionVitalsPanel.AmountLine amountsLine in this.amountsLines)
      {
        bool flag = amountsLine.TryUpdate(amounts);
        if (amountsLine.go.activeSelf != flag)
          amountsLine.go.SetActive(flag);
      }
      foreach (MinionVitalsPanel.AttributeLine attributesLine in this.attributesLines)
      {
        bool flag = attributesLine.TryUpdate(attributes);
        if (attributesLine.go.activeSelf != flag)
          attributesLine.go.SetActive(flag);
      }
    }
    bool flag1 = false;
    for (int index1 = 0; index1 < this.checkboxLines.Count; ++index1)
    {
      MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[index1];
      MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
      if (this.checkboxLines[index1].amount != null)
      {
        for (int index2 = 0; index2 < amounts.Count; ++index2)
        {
          AmountInstance amountInstance = amounts[index2];
          if (checkboxLine.amount == amountInstance.amount)
          {
            checkboxLineDisplayType = checkboxLine.display_condition(this.selectedEntity.gameObject);
            break;
          }
        }
      }
      else
        checkboxLineDisplayType = checkboxLine.display_condition(this.selectedEntity.gameObject);
      if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
      {
        checkboxLine.locText.SetText(checkboxLine.label_text_func(this.selectedEntity.gameObject));
        if (!checkboxLine.go.activeSelf)
          checkboxLine.go.SetActive(true);
        GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
        gameObject.SetActive(checkboxLine.get_value(this.selectedEntity.gameObject));
        if ((UnityEngine.Object) checkboxLine.go.transform.parent != (UnityEngine.Object) checkboxLine.parentContainer)
        {
          checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
          checkboxLine.go.transform.localScale = Vector3.one;
        }
        if ((UnityEngine.Object) checkboxLine.parentContainer == (UnityEngine.Object) this.conditionsContainerAdditional)
          flag1 = true;
        if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
        {
          if (checkboxLine.get_value(this.selectedEntity.gameObject))
          {
            checkboxLine.locText.color = Color.black;
            gameObject.transform.parent.GetComponent<Image>().color = Color.black;
          }
          else
          {
            Color color = new Color(0.9921569f, 0.0f, 0.1019608f);
            checkboxLine.locText.color = color;
            gameObject.transform.parent.GetComponent<Image>().color = color;
          }
        }
        else
        {
          checkboxLine.locText.color = Color.grey;
          gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
        }
      }
      else if (checkboxLine.go.activeSelf)
        checkboxLine.go.SetActive(false);
    }
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    Growing component2 = component1.GetComponent<Growing>();
    bool flag2 = component1.HasTag(GameTags.Decoration);
    this.conditionsContainerNormal.gameObject.SetActive(true);
    this.conditionsContainerAdditional.gameObject.SetActive(!flag2);
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
    {
      float num = 1f;
      LocText reference1 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
      reference1.text = string.Empty;
      reference1.text = !flag2 ? string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, (object) Util.FormatTwoDecimalPlace((float) ((double) num * 0.25 * 100.0))) : string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE);
      reference1.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP));
      LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
      reference2.color = !this.selectedEntity.GetComponent<ReceptacleMonitor>().Replanted ? Color.grey : Color.black;
      reference2.text = string.Empty;
      reference2.text = string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, (object) Util.FormatTwoDecimalPlace(num * 100f));
      reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP));
    }
    else
    {
      LocText reference1 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
      reference1.text = string.Empty;
      reference1.text = string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, (object) GameUtil.GetFormattedCycles(component1.GetComponent<Growing>().WildGrowthTime(), "F1"));
      reference1.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, (object) GameUtil.GetFormattedCycles(component1.GetComponent<Growing>().WildGrowthTime(), "F1")));
      LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
      reference2.color = !this.selectedEntity.GetComponent<ReceptacleMonitor>().Replanted ? Color.grey : Color.black;
      reference2.text = string.Empty;
      reference2.text = !flag1 ? string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, (object) GameUtil.GetFormattedCycles(component1.GetComponent<Growing>().DomesticGrowthTime(), "F1")) : string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, (object) GameUtil.GetFormattedCycles(component1.GetComponent<Growing>().DomesticGrowthTime(), "F1"));
      reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, (object) GameUtil.GetFormattedCycles(component1.GetComponent<Growing>().DomesticGrowthTime(), "F1")));
    }
    foreach (MinionVitalsPanel.AmountLine amountsLine in this.amountsLines)
      amountsLine.go.SetActive(false);
    foreach (MinionVitalsPanel.AttributeLine attributesLine in this.attributesLines)
      attributesLine.go.SetActive(false);
  }

  private string GetAirPressureTooltip(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return string.Empty;
    return STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
  }

  private string GetInternalTemperatureTooltip(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return string.Empty;
    return STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
  }

  private string GetFertilizationTooltip(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    if (smi == null)
      return string.Empty;
    return STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
  }

  private string GetIrrigationTooltip(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    if (smi == null)
      return string.Empty;
    return STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
  }

  private string GetIlluminationTooltip(GameObject go)
  {
    IlluminationVulnerable component = go.GetComponent<IlluminationVulnerable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return string.Empty;
    if (component.prefersDarkness && component.IsComfortable() || !component.prefersDarkness && !component.IsComfortable())
      return (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK;
    return (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
  }

  private string GetReceptacleTooltip(GameObject go)
  {
    ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return string.Empty;
    if (component.HasOperationalReceptacle())
      return (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL;
    return (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
  }

  private string GetAtmosphereTooltip(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.ExternalElement.name);
    return (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
  }

  private string GetAirPressureLabel(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return Db.Get().Amounts.AirPressure.Name + "\n    • " + GameUtil.GetFormattedMass(component.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, false, "{0:0.#}") + " - " + GameUtil.GetFormattedMass(component.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
  }

  private string GetInternalTemperatureLabel(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    return Db.Get().Amounts.Temperature.Name + "\n    • " + GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false) + " - " + GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
  }

  private string GetFertilizationLabel(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    string str = Db.Get().Amounts.Fertilization.Name;
    foreach (PlantElementAbsorber.ConsumeInfo consumedElement in smi.def.consumedElements)
      str = str + "\n    • " + ElementLoader.GetElement(consumedElement.tag).name + " " + GameUtil.GetFormattedMass(consumedElement.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
    return str;
  }

  private string GetIrrigationLabel(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    return Db.Get().Amounts.Irrigation.Name + "\n    • " + ElementLoader.GetElement(smi.def.consumedElements[0].tag).name + ": " + GameUtil.GetFormattedMass(smi.def.consumedElements[0].massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
  }

  private string GetIlluminationLabel(GameObject go)
  {
    return Db.Get().Amounts.Illumination.Name + "\n    • " + (string) (!go.GetComponent<IlluminationVulnerable>().prefersDarkness ? STRINGS.UI.GAMEOBJECTEFFECTS.LIGHT : STRINGS.UI.GAMEOBJECTEFFECTS.DARKNESS);
  }

  private string GetAtmosphereLabel(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    string str = (string) STRINGS.UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
    foreach (Element safeAtmosphere in component.safe_atmospheres)
      str = str + "\n    • " + safeAtmosphere.name;
    return str;
  }

  private bool check_pressure(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
    return true;
  }

  private bool check_temperature(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
    return true;
  }

  private bool check_irrigation(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    if (smi == null)
      return true;
    if (!smi.IsInsideState((StateMachine.BaseState) smi.sm.replanted.starved))
      return !smi.IsInsideState((StateMachine.BaseState) smi.sm.wild);
    return false;
  }

  private bool check_illumination(GameObject go)
  {
    IlluminationVulnerable component = go.GetComponent<IlluminationVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.IsComfortable();
    return true;
  }

  private bool check_receptacle(GameObject go)
  {
    ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    return component.HasOperationalReceptacle();
  }

  private bool check_fertilizer(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    if (smi != null)
      return smi.sm.hasCorrectFertilizer.Get(smi);
    return true;
  }

  private bool check_atmosphere(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.IsSafeElement(Grid.Element[Grid.PosToCell(go)]);
    return true;
  }

  [DebuggerDisplay("{amount.Name}")]
  public struct AmountLine
  {
    public Amount amount;
    public GameObject go;
    public ValueTrendImageToggle imageToggle;
    public LocText locText;
    public ToolTip toolTip;
    public Func<AmountInstance, string> toolTipFunc;

    public bool TryUpdate(Amounts amounts)
    {
      foreach (AmountInstance amount in (Modifications<Amount, AmountInstance>) amounts)
      {
        if (this.amount == amount.amount && !amount.hide)
        {
          this.locText.SetText(this.amount.GetDescription(amount));
          this.toolTip.toolTip = this.toolTipFunc(amount);
          this.imageToggle.SetValue(amount);
          return true;
        }
      }
      return false;
    }
  }

  [DebuggerDisplay("{attribute.Name}")]
  public struct AttributeLine
  {
    public Klei.AI.Attribute attribute;
    public GameObject go;
    public LocText locText;
    public ToolTip toolTip;
    public Func<AttributeInstance, string> toolTipFunc;

    public bool TryUpdate(Attributes attributes)
    {
      foreach (AttributeInstance attribute in attributes)
      {
        if (this.attribute == attribute.modifier && !attribute.hide)
        {
          this.locText.SetText(this.attribute.GetDescription(attribute));
          this.toolTip.toolTip = this.toolTipFunc(attribute);
          return true;
        }
      }
      return false;
    }
  }

  public struct CheckboxLine
  {
    public Amount amount;
    public GameObject go;
    public LocText locText;
    public Func<GameObject, string> tooltip;
    public Func<GameObject, bool> get_value;
    public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;
    public Func<GameObject, string> label_text_func;
    public Transform parentContainer;
  }

  public enum CheckboxLineDisplayType
  {
    Normal,
    Diminished,
    Hidden,
  }
}
