// Decompiled with JetBrains decompiler
// Type: EnergyInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class EnergyInfoScreen : TargetScreen
{
  private Dictionary<string, GameObject> overviewLabels = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> generatorsLabels = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> consumersLabels = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> batteriesLabels = new Dictionary<string, GameObject>();
  public GameObject labelTemplate;
  private GameObject overviewPanel;
  private GameObject generatorsPanel;
  private GameObject consumersPanel;
  private GameObject batteriesPanel;

  public override bool IsValidForTarget(GameObject target)
  {
    if (!((Object) target.GetComponent<Generator>() != (Object) null) && !((Object) target.GetComponent<Wire>() != (Object) null) && !((Object) target.GetComponent<Battery>() != (Object) null))
      return (Object) target.GetComponent<EnergyConsumer>() != (Object) null;
    return true;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overviewPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.overviewPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.ENERGYGENERATOR.CIRCUITOVERVIEW;
    this.generatorsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.generatorsPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.ENERGYGENERATOR.GENERATORS;
    this.consumersPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.consumersPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.ENERGYGENERATOR.CONSUMERS;
    this.batteriesPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.batteriesPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.ENERGYGENERATOR.BATTERIES;
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
      gameObject = Util.KInstantiate(this.labelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, (string) null);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      labels[id] = gameObject;
      gameObject.SetActive(true);
    }
    return gameObject;
  }

  private void LateUpdate()
  {
    this.Refresh();
  }

  private void Refresh()
  {
    if ((Object) this.selectedTarget == (Object) null)
      return;
    foreach (KeyValuePair<string, GameObject> overviewLabel in this.overviewLabels)
      overviewLabel.Value.SetActive(false);
    foreach (KeyValuePair<string, GameObject> generatorsLabel in this.generatorsLabels)
      generatorsLabel.Value.SetActive(false);
    foreach (KeyValuePair<string, GameObject> consumersLabel in this.consumersLabels)
      consumersLabel.Value.SetActive(false);
    foreach (KeyValuePair<string, GameObject> batteriesLabel in this.batteriesLabels)
      batteriesLabel.Value.SetActive(false);
    CircuitManager circuitManager = Game.Instance.circuitManager;
    ushort circuitID = ushort.MaxValue;
    EnergyConsumer component1 = this.selectedTarget.GetComponent<EnergyConsumer>();
    if ((Object) component1 != (Object) null)
    {
      circuitID = component1.CircuitID;
    }
    else
    {
      Generator component2 = this.selectedTarget.GetComponent<Generator>();
      if ((Object) component2 != (Object) null)
        circuitID = component2.CircuitID;
    }
    if (circuitID == ushort.MaxValue)
    {
      int cell = Grid.PosToCell(this.selectedTarget.transform.GetPosition());
      circuitID = circuitManager.GetCircuitID(cell);
    }
    if (circuitID != ushort.MaxValue)
    {
      this.overviewPanel.SetActive(true);
      this.generatorsPanel.SetActive(true);
      this.consumersPanel.SetActive(true);
      this.batteriesPanel.SetActive(true);
      float availableOnCircuit = circuitManager.GetJoulesAvailableOnCircuit(circuitID);
      GameObject label1 = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "joulesAvailable");
      label1.GetComponent<LocText>().text = string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES, (object) GameUtil.GetFormattedJoules(availableOnCircuit, "F1", GameUtil.TimeSlice.None));
      label1.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP;
      label1.SetActive(true);
      float generatedByCircuit1 = circuitManager.GetWattsGeneratedByCircuit(circuitID);
      float generatedByCircuit2 = circuitManager.GetPotentialWattsGeneratedByCircuit(circuitID);
      GameObject label2 = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "wattageGenerated");
      string str = (double) generatedByCircuit1 != (double) generatedByCircuit2 ? string.Format("{0} / {1}", (object) GameUtil.GetFormattedWattage(generatedByCircuit1, GameUtil.WattageFormatterUnit.Automatic), (object) GameUtil.GetFormattedWattage(generatedByCircuit2, GameUtil.WattageFormatterUnit.Automatic)) : GameUtil.GetFormattedWattage(generatedByCircuit1, GameUtil.WattageFormatterUnit.Automatic);
      label2.GetComponent<LocText>().text = string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED, (object) str);
      label2.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP;
      label2.SetActive(true);
      GameObject label3 = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "wattageConsumed");
      label3.GetComponent<LocText>().text = string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED, (object) GameUtil.GetFormattedWattage(circuitManager.GetWattsUsedByCircuit(circuitID), GameUtil.WattageFormatterUnit.Automatic));
      label3.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP;
      label3.SetActive(true);
      GameObject label4 = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "potentialWattageConsumed");
      label4.GetComponent<LocText>().text = string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, (object) GameUtil.GetFormattedWattage(circuitManager.GetWattsNeededWhenActive(circuitID), GameUtil.WattageFormatterUnit.Automatic));
      label4.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP;
      label4.SetActive(true);
      GameObject label5 = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "maxSafeWattage");
      label5.GetComponent<LocText>().text = string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE, (object) GameUtil.GetFormattedWattage(circuitManager.GetMaxSafeWattageForCircuit(circuitID), GameUtil.WattageFormatterUnit.Automatic));
      label5.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP;
      label5.SetActive(true);
      ReadOnlyCollection<Generator> generatorsOnCircuit = circuitManager.GetGeneratorsOnCircuit(circuitID);
      ReadOnlyCollection<IEnergyConsumer> consumersOnCircuit = circuitManager.GetConsumersOnCircuit(circuitID);
      List<Battery> batteriesOnCircuit = circuitManager.GetBatteriesOnCircuit(circuitID);
      ReadOnlyCollection<Battery> transformersOnCircuit = circuitManager.GetTransformersOnCircuit(circuitID);
      if (generatorsOnCircuit.Count > 0)
      {
        foreach (Generator generator in generatorsOnCircuit)
        {
          if ((Object) generator != (Object) null && (Object) generator.GetComponent<Battery>() == (Object) null)
          {
            label5 = this.AddOrGetLabel(this.generatorsLabels, this.generatorsPanel, generator.gameObject.GetInstanceID().ToString());
            if (generator.GetComponent<Operational>().IsActive)
              label5.GetComponent<LocText>().text = string.Format("{0}: {1}", (object) generator.GetComponent<KSelectable>().entityName, (object) GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic));
            else
              label5.GetComponent<LocText>().text = string.Format("{0}: {1} / {2}", (object) generator.GetComponent<KSelectable>().entityName, (object) GameUtil.GetFormattedWattage(0.0f, GameUtil.WattageFormatterUnit.Automatic), (object) GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic));
            label5.SetActive(true);
            label5.GetComponent<LocText>().fontStyle = !((Object) generator.gameObject == (Object) this.selectedTarget) ? FontStyles.Normal : FontStyles.Bold;
          }
        }
      }
      else
      {
        label5 = this.AddOrGetLabel(this.generatorsLabels, this.generatorsPanel, "nogenerators");
        label5.GetComponent<LocText>().text = (string) UI.DETAILTABS.ENERGYGENERATOR.NOGENERATORS;
        label5.SetActive(true);
      }
      if (consumersOnCircuit.Count > 0 || transformersOnCircuit.Count > 0)
      {
        foreach (IEnergyConsumer consumer in consumersOnCircuit)
          this.AddConsumerInfo(consumer, label5);
        foreach (IEnergyConsumer consumer in transformersOnCircuit)
          this.AddConsumerInfo(consumer, label5);
      }
      else
      {
        GameObject label6 = this.AddOrGetLabel(this.consumersLabels, this.consumersPanel, "noconsumers");
        label6.GetComponent<LocText>().text = (string) UI.DETAILTABS.ENERGYGENERATOR.NOCONSUMERS;
        label6.SetActive(true);
      }
      if (batteriesOnCircuit.Count > 0)
      {
        foreach (Battery battery in batteriesOnCircuit)
        {
          if ((Object) battery != (Object) null)
          {
            GameObject label6 = this.AddOrGetLabel(this.batteriesLabels, this.batteriesPanel, battery.gameObject.GetInstanceID().ToString());
            label6.GetComponent<LocText>().text = string.Format("{0}: {1}", (object) battery.GetComponent<KSelectable>().entityName, (object) GameUtil.GetFormattedJoules(battery.JoulesAvailable, "F1", GameUtil.TimeSlice.None));
            label6.SetActive(true);
            label6.GetComponent<LocText>().fontStyle = !((Object) battery.gameObject == (Object) this.selectedTarget) ? FontStyles.Normal : FontStyles.Bold;
          }
        }
      }
      else
      {
        GameObject label6 = this.AddOrGetLabel(this.batteriesLabels, this.batteriesPanel, "nobatteries");
        label6.GetComponent<LocText>().text = (string) UI.DETAILTABS.ENERGYGENERATOR.NOBATTERIES;
        label6.SetActive(true);
      }
    }
    else
    {
      this.overviewPanel.SetActive(true);
      this.generatorsPanel.SetActive(false);
      this.consumersPanel.SetActive(false);
      this.batteriesPanel.SetActive(false);
      GameObject label = this.AddOrGetLabel(this.overviewLabels, this.overviewPanel, "nocircuit");
      label.GetComponent<LocText>().text = (string) UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED;
      label.SetActive(true);
    }
  }

  private void AddConsumerInfo(IEnergyConsumer consumer, GameObject label)
  {
    KMonoBehaviour kmonoBehaviour = consumer as KMonoBehaviour;
    if (!((Object) kmonoBehaviour != (Object) null))
      return;
    label = this.AddOrGetLabel(this.consumersLabels, this.consumersPanel, kmonoBehaviour.gameObject.GetInstanceID().ToString());
    float wattsUsed = consumer.WattsUsed;
    float neededWhenActive = consumer.WattsNeededWhenActive;
    string str = (double) wattsUsed != (double) neededWhenActive ? string.Format("{0} / {1}", (object) GameUtil.GetFormattedWattage(wattsUsed, GameUtil.WattageFormatterUnit.Automatic), (object) GameUtil.GetFormattedWattage(neededWhenActive, GameUtil.WattageFormatterUnit.Automatic)) : GameUtil.GetFormattedWattage(wattsUsed, GameUtil.WattageFormatterUnit.Automatic);
    label.GetComponent<LocText>().text = string.Format("{0}: {1}", (object) consumer.Name, (object) str);
    label.SetActive(true);
    label.GetComponent<LocText>().fontStyle = !((Object) kmonoBehaviour.gameObject == (Object) this.selectedTarget) ? FontStyles.Normal : FontStyles.Bold;
  }
}
