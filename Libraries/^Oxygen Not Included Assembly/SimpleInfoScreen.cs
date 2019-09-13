// Decompiled with JetBrains decompiler
// Type: SimpleInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleInfoScreen : TargetScreen
{
  private static readonly EventSystem.IntraObjectHandler<SimpleInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<SimpleInfoScreen>((System.Action<SimpleInfoScreen, object>) ((component, data) => component.OnRefreshData(data)));
  private Dictionary<string, GameObject> storageLabels = new Dictionary<string, GameObject>();
  public Color statusItemTextColor_regular = Color.black;
  public Color statusItemTextColor_bad = new Color(0.9568627f, 0.2901961f, 0.2784314f);
  public Color statusItemTextColor_old = new Color(0.8235294f, 0.8235294f, 0.8235294f);
  private List<SimpleInfoScreen.StatusItemEntry> statusItems = new List<SimpleInfoScreen.StatusItemEntry>();
  private List<SimpleInfoScreen.StatusItemEntry> oldStatusItems = new List<SimpleInfoScreen.StatusItemEntry>();
  private List<LocText> attributeLabels = new List<LocText>();
  public GameObject attributesLabelTemplate;
  public GameObject attributesLabelButtonTemplate;
  public GameObject DescriptionContainerTemplate;
  private DescriptionContainer descriptionContainer;
  public GameObject StampContainerTemplate;
  public GameObject StampPrefab;
  public GameObject VitalsPanelTemplate;
  public Sprite DefaultPortraitIcon;
  public Text StatusPanelCurrentActionLabel;
  public GameObject StatusItemPrefab;
  public Sprite statusWarningIcon;
  private CollapsibleDetailContentPanel statusItemPanel;
  private CollapsibleDetailContentPanel vitalsPanel;
  private CollapsibleDetailContentPanel fertilityPanel;
  private GameObject storagePanel;
  private GameObject infoPanel;
  private GameObject stampContainer;
  private MinionVitalsPanel vitalsContainer;
  private GameObject InfoFolder;
  private GameObject statusItemsFolder;
  public GameObject TextContainerPrefab;
  private GameObject stressPanel;
  private DetailsPanelDrawer stressDrawer;
  public TextStyleSetting ToolTipStyle_Property;
  public TextStyleSetting StatusItemStyle_Main;
  public TextStyleSetting StatusItemStyle_Other;
  private GameObject lastTarget;
  private bool TargetIsMinion;
  private System.Action<object> onStorageChangeDelegate;

  public SimpleInfoScreen()
  {
    this.onStorageChangeDelegate = new System.Action<object>(this.OnStorageChange);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return true;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.statusItemPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.statusItemPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
    this.statusItemPanel.HeaderLabel.text = (string) STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_STATUS;
    this.statusItemPanel.scalerMask.hoverLock = true;
    this.statusItemsFolder = this.statusItemPanel.Content.gameObject;
    this.vitalsPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.vitalsPanel.SetTitle((string) STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION);
    this.vitalsContainer = Util.KInstantiateUI(this.VitalsPanelTemplate, this.vitalsPanel.Content.gameObject, false).GetComponent<MinionVitalsPanel>();
    this.fertilityPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.fertilityPanel.SetTitle((string) STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_FERTILITY);
    this.infoPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.infoPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_DESCRIPTION;
    GameObject gameObject = this.infoPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject;
    this.descriptionContainer = Util.KInstantiateUI<DescriptionContainer>(this.DescriptionContainerTemplate, gameObject, false);
    this.storagePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.stressPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.stressDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.stressPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
    this.stampContainer = Util.KInstantiateUI(this.StampContainerTemplate, gameObject, false);
    this.Subscribe<SimpleInfoScreen>(-1514841199, SimpleInfoScreen.OnRefreshDataDelegate);
  }

  public override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Subscribe(target, -1697596308, this.onStorageChangeDelegate);
    this.Subscribe(target, -1197125120, this.onStorageChangeDelegate);
    this.RefreshStorage();
    this.Subscribe(target, 1059811075, new System.Action<object>(this.OnBreedingChanceChanged));
    this.RefreshBreedingChance();
    this.vitalsPanel.SetTitle((string) (!((UnityEngine.Object) target.GetComponent<WiltCondition>() == (UnityEngine.Object) null) ? STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS : STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION));
    KSelectable component = target.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
      if (statusItemGroup != null)
      {
        statusItemGroup.OnAddStatusItem += new System.Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem);
        statusItemGroup.OnRemoveStatusItem += new System.Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem);
        foreach (StatusItemGroup.Entry status_item in statusItemGroup)
        {
          if (status_item.category != null && status_item.category.Id == "Main")
            this.DoAddStatusItem(status_item, status_item.category, false);
        }
        foreach (StatusItemGroup.Entry status_item in statusItemGroup)
        {
          if (status_item.category == null || status_item.category.Id != "Main")
            this.DoAddStatusItem(status_item, status_item.category, false);
        }
      }
    }
    this.statusItemPanel.gameObject.SetActive(true);
    this.statusItemPanel.scalerMask.UpdateSize();
    this.Refresh(true);
  }

  public override void OnDeselectTarget(GameObject target)
  {
    base.OnDeselectTarget(target);
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
    {
      this.Unsubscribe(target, -1697596308, this.onStorageChangeDelegate);
      this.Unsubscribe(target, -1197125120, this.onStorageChangeDelegate);
      this.Unsubscribe(target, 1059811075, new System.Action<object>(this.OnBreedingChanceChanged));
    }
    KSelectable component = target.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
    if (statusItemGroup == null)
      return;
    statusItemGroup.OnAddStatusItem -= new System.Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem);
    statusItemGroup.OnRemoveStatusItem -= new System.Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem);
    foreach (SimpleInfoScreen.StatusItemEntry statusItem in this.statusItems)
      statusItem.Destroy(true);
    this.statusItems.Clear();
    foreach (SimpleInfoScreen.StatusItemEntry oldStatusItem in this.oldStatusItems)
    {
      oldStatusItem.onDestroy = (System.Action<SimpleInfoScreen.StatusItemEntry>) null;
      oldStatusItem.Destroy(true);
    }
    this.oldStatusItems.Clear();
  }

  private void OnStorageChange(object data)
  {
    this.RefreshStorage();
  }

  private void OnBreedingChanceChanged(object data)
  {
    this.RefreshBreedingChance();
  }

  private void OnAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category)
  {
    this.DoAddStatusItem(status_item, category, false);
  }

  private void DoAddStatusItem(
    StatusItemGroup.Entry status_item,
    StatusItemCategory category,
    bool show_immediate = false)
  {
    GameObject statusItemsFolder = this.statusItemsFolder;
    Color color = status_item.item.notificationType == NotificationType.BadMinor || status_item.item.notificationType == NotificationType.Bad || status_item.item.notificationType == NotificationType.DuplicantThreatening ? this.statusItemTextColor_bad : this.statusItemTextColor_regular;
    TextStyleSetting style = category != Db.Get().StatusItemCategories.Main ? this.StatusItemStyle_Other : this.StatusItemStyle_Main;
    SimpleInfoScreen.StatusItemEntry statusItemEntry1 = new SimpleInfoScreen.StatusItemEntry(status_item, category, this.StatusItemPrefab, statusItemsFolder.transform, this.ToolTipStyle_Property, color, style, show_immediate, new System.Action<SimpleInfoScreen.StatusItemEntry>(this.OnStatusItemDestroy));
    statusItemEntry1.SetSprite(status_item.item.sprite);
    if (category != null)
    {
      int index = -1;
      foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems.FindAll((Predicate<SimpleInfoScreen.StatusItemEntry>) (e => e.category == category)))
      {
        index = statusItemEntry2.GetIndex();
        statusItemEntry2.Destroy(true);
        this.oldStatusItems.Remove(statusItemEntry2);
      }
      if (category == Db.Get().StatusItemCategories.Main)
        index = 0;
      if (index != -1)
        statusItemEntry1.SetIndex(index);
    }
    this.statusItems.Add(statusItemEntry1);
  }

  private void OnRemoveStatusItem(StatusItemGroup.Entry status_item, bool immediate = false)
  {
    this.DoRemoveStatusItem(status_item, immediate);
  }

  private void DoRemoveStatusItem(StatusItemGroup.Entry status_item, bool destroy_immediate = false)
  {
    for (int index = 0; index < this.statusItems.Count; ++index)
    {
      if (this.statusItems[index].item.item == status_item.item)
      {
        SimpleInfoScreen.StatusItemEntry statusItem = this.statusItems[index];
        this.statusItems.RemoveAt(index);
        this.oldStatusItems.Add(statusItem);
        statusItem.Destroy(destroy_immediate);
        break;
      }
    }
  }

  private void OnStatusItemDestroy(SimpleInfoScreen.StatusItemEntry item)
  {
    this.oldStatusItems.Remove(item);
  }

  private void Update()
  {
    this.Refresh(false);
  }

  private void OnRefreshData(object obj)
  {
    this.Refresh(false);
  }

  public void Refresh(bool force = false)
  {
    if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) this.lastTarget || force)
    {
      this.lastTarget = this.selectedTarget;
      if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null)
      {
        this.SetPanels(this.selectedTarget);
        this.SetStamps(this.selectedTarget);
      }
    }
    int count = this.statusItems.Count;
    this.statusItemPanel.gameObject.SetActive(count > 0);
    for (int index = 0; index < count; ++index)
      this.statusItems[index].Refresh();
    if (this.vitalsContainer.isActiveAndEnabled)
      this.vitalsContainer.Refresh();
    this.RefreshStress();
    this.RefreshStorage();
  }

  private void SetPanels(GameObject target)
  {
    MinionIdentity component1 = target.GetComponent<MinionIdentity>();
    Amounts amounts = target.GetAmounts();
    PrimaryElement component2 = target.GetComponent<PrimaryElement>();
    BuildingComplete component3 = target.GetComponent<BuildingComplete>();
    BuildingUnderConstruction component4 = target.GetComponent<BuildingUnderConstruction>();
    CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
    InfoDescription component6 = target.GetComponent<InfoDescription>();
    Edible component7 = target.GetComponent<Edible>();
    this.attributeLabels.ForEach((System.Action<LocText>) (x => UnityEngine.Object.Destroy((UnityEngine.Object) x.gameObject)));
    this.attributeLabels.Clear();
    this.vitalsPanel.gameObject.SetActive(amounts != null);
    string str1 = string.Empty;
    string str2 = string.Empty;
    if (amounts != null)
    {
      this.vitalsContainer.selectedEntity = this.selectedTarget;
      Uprootable component8 = this.selectedTarget.gameObject.GetComponent<Uprootable>();
      if ((UnityEngine.Object) component8 != (UnityEngine.Object) null)
        this.vitalsPanel.gameObject.SetActive((UnityEngine.Object) component8.GetPlanterStorage != (UnityEngine.Object) null);
      if ((UnityEngine.Object) this.selectedTarget.gameObject.GetComponent<WiltCondition>() != (UnityEngine.Object) null)
        this.vitalsPanel.gameObject.SetActive(true);
    }
    if ((bool) ((UnityEngine.Object) component1))
      str1 = string.Empty;
    else if ((bool) ((UnityEngine.Object) component6))
      str1 = component6.description;
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      str1 = component3.Def.Effect;
      str2 = component3.Def.Desc;
    }
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
    {
      str1 = component4.Def.Effect;
      str2 = component4.Def.Desc;
    }
    else if ((UnityEngine.Object) component7 != (UnityEngine.Object) null)
    {
      EdiblesManager.FoodInfo foodInfo = component7.FoodInfo;
      str1 += string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.CALORIES, (object) GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true));
    }
    else if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      str1 = component5.element.FullDescription(false);
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      Element elementByHash = ElementLoader.FindElementByHash(component2.ElementID);
      str1 = elementByHash == null ? string.Empty : elementByHash.FullDescription(false);
    }
    List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(target, true);
    bool flag = gameObjectEffects.Count > 0;
    this.descriptionContainer.gameObject.SetActive(flag);
    this.descriptionContainer.descriptors.gameObject.SetActive(flag);
    if (flag)
      this.descriptionContainer.descriptors.SetDescriptors((IList<Descriptor>) gameObjectEffects);
    this.descriptionContainer.description.text = str1;
    this.descriptionContainer.flavour.text = str2;
    this.infoPanel.gameObject.SetActive((UnityEngine.Object) component1 == (UnityEngine.Object) null);
    this.descriptionContainer.gameObject.SetActive(this.infoPanel.activeSelf);
    this.descriptionContainer.flavour.gameObject.SetActive(str2 != string.Empty && str2 != "\n");
    if (!this.vitalsPanel.gameObject.activeSelf || amounts.Count != 0)
      return;
    this.vitalsPanel.gameObject.SetActive(false);
  }

  private void RefreshBreedingChance()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
    {
      this.fertilityPanel.gameObject.SetActive(false);
    }
    else
    {
      FertilityMonitor.Instance smi = this.selectedTarget.GetSMI<FertilityMonitor.Instance>();
      if (smi == null)
      {
        this.fertilityPanel.gameObject.SetActive(false);
      }
      else
      {
        int num = 0;
        foreach (FertilityMonitor.BreedingChance breedingChance in smi.breedingChances)
        {
          List<FertilityModifier> forTag = Db.Get().FertilityModifiers.GetForTag(breedingChance.egg);
          if (forTag.Count > 0)
          {
            string empty = string.Empty;
            foreach (FertilityModifier fertilityModifier in forTag)
              empty += string.Format((string) STRINGS.UI.DETAILTABS.EGG_CHANCES.CHANCE_MOD_FORMAT, (object) fertilityModifier.GetTooltip());
            this.fertilityPanel.SetLabel("breeding_" + (object) num++, string.Format((string) STRINGS.UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, (object) breedingChance.egg.ProperName(), (object) GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format((string) STRINGS.UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP, (object) breedingChance.egg.ProperName(), (object) GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None), (object) empty));
          }
          else
            this.fertilityPanel.SetLabel("breeding_" + (object) num++, string.Format((string) STRINGS.UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, (object) breedingChance.egg.ProperName(), (object) GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format((string) STRINGS.UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD, (object) breedingChance.egg.ProperName(), (object) GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)));
        }
        this.fertilityPanel.Commit();
      }
    }
  }

  private void RefreshStorage()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
    {
      this.storagePanel.gameObject.SetActive(false);
    }
    else
    {
      Storage[] componentsInChildren = this.selectedTarget.GetComponentsInChildren<Storage>();
      if (componentsInChildren == null)
      {
        this.storagePanel.gameObject.SetActive(false);
      }
      else
      {
        Storage[] all = Array.FindAll<Storage>(componentsInChildren, (Predicate<Storage>) (n => n.showInUI));
        if (all.Length == 0)
        {
          this.storagePanel.gameObject.SetActive(false);
        }
        else
        {
          this.storagePanel.gameObject.SetActive(true);
          this.storagePanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) (!((UnityEngine.Object) this.selectedTarget.GetComponent<MinionIdentity>() != (UnityEngine.Object) null) ? STRINGS.UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS : STRINGS.UI.DETAILTABS.DETAILS.GROUPNAME_MINION_CONTENTS);
          foreach (KeyValuePair<string, GameObject> storageLabel in this.storageLabels)
            storageLabel.Value.SetActive(false);
          int num = 0;
          foreach (Storage storage in all)
          {
            foreach (GameObject go in storage.items)
            {
              if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
              {
                GameObject storageLabel = this.AddOrGetStorageLabel(this.storageLabels, this.storagePanel, "storage_" + num.ToString());
                ++num;
                if (storage.allowUIItemRemoval)
                {
                  Transform transform = storageLabel.transform.Find("removeAttributeButton");
                  if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
                  {
                    KButton component = transform.GetComponent<KButton>();
                    component.enabled = true;
                    component.gameObject.SetActive(true);
                    GameObject select_item = go;
                    Storage selected_storage = storage;
                    component.onClick += (System.Action) (() => selected_storage.Drop(select_item, true));
                  }
                }
                PrimaryElement component1 = go.GetComponent<PrimaryElement>();
                Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
                storageLabel.GetComponentInChildren<ToolTip>().ClearMultiStringTooltip();
                string unitFormattedName = GameUtil.GetUnitFormattedName(go, false);
                string str1 = string.Format((string) STRINGS.UI.DETAILTABS.DETAILS.CONTENTS_MASS, (object) unitFormattedName, (object) GameUtil.GetFormattedMass(component1.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
                string str2 = string.Format((string) STRINGS.UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, (object) str1, (object) GameUtil.GetFormattedTemperature(component1.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
                if (smi != null)
                {
                  string str3 = smi.StateString();
                  if (!string.IsNullOrEmpty(str3))
                    str2 += string.Format((string) STRINGS.UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, (object) str3);
                  storageLabel.GetComponentInChildren<ToolTip>().AddMultiStringTooltip(smi.GetToolTip(), (ScriptableObject) PluginAssets.Instance.defaultTextStyleSetting);
                }
                if (component1.DiseaseIdx != byte.MaxValue)
                {
                  str2 += string.Format((string) STRINGS.UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, (object) GameUtil.GetFormattedDisease(component1.DiseaseIdx, component1.DiseaseCount, false));
                  string formattedDisease = GameUtil.GetFormattedDisease(component1.DiseaseIdx, component1.DiseaseCount, true);
                  storageLabel.GetComponentInChildren<ToolTip>().AddMultiStringTooltip(formattedDisease, (ScriptableObject) PluginAssets.Instance.defaultTextStyleSetting);
                }
                storageLabel.GetComponentInChildren<LocText>().text = str2;
                KButton component2 = storageLabel.GetComponent<KButton>();
                GameObject select_target = go;
                component2.onClick += (System.Action) (() => SelectTool.Instance.Select(select_target.GetComponent<KSelectable>(), false));
              }
            }
          }
          if (num != 0)
            return;
          this.AddOrGetStorageLabel(this.storageLabels, this.storagePanel, "empty").GetComponentInChildren<LocText>().text = (string) STRINGS.UI.DETAILTABS.DETAILS.STORAGE_EMPTY;
        }
      }
    }
  }

  private GameObject AddOrGetStorageLabel(
    Dictionary<string, GameObject> labels,
    GameObject panel,
    string id)
  {
    GameObject gameObject;
    if (labels.ContainsKey(id))
    {
      gameObject = labels[id];
      gameObject.GetComponent<KButton>().ClearOnClick();
      Transform c = gameObject.transform.Find("removeAttributeButton");
      if ((UnityEngine.Object) c != (UnityEngine.Object) null)
      {
        KButton component = c.FindComponent<KButton>();
        component.enabled = false;
        component.gameObject.SetActive(false);
        component.ClearOnClick();
      }
    }
    else
    {
      gameObject = Util.KInstantiate(this.attributesLabelButtonTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, (string) null);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      labels[id] = gameObject;
    }
    gameObject.SetActive(true);
    return gameObject;
  }

  private void RefreshStress()
  {
    MinionIdentity identity = !((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null) ? (MinionIdentity) null : this.selectedTarget.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) identity == (UnityEngine.Object) null)
    {
      this.stressPanel.SetActive(false);
    }
    else
    {
      List<ReportManager.ReportEntry.Note> stressNotes = new List<ReportManager.ReportEntry.Note>();
      this.stressPanel.SetActive(true);
      this.stressPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) STRINGS.UI.DETAILTABS.STATS.GROUPNAME_STRESS;
      ReportManager.ReportEntry reportEntry1 = ReportManager.Instance.TodaysReport.reportEntries.Find((Predicate<ReportManager.ReportEntry>) (entry => entry.reportType == ReportManager.ReportType.StressDelta));
      this.stressDrawer.BeginDrawing();
      float num = 0.0f;
      stressNotes.Clear();
      int index1 = reportEntry1.contextEntries.FindIndex((Predicate<ReportManager.ReportEntry>) (entry => entry.context == identity.GetProperName()));
      ReportManager.ReportEntry reportEntry2 = index1 == -1 ? (ReportManager.ReportEntry) null : reportEntry1.contextEntries[index1];
      if (reportEntry2 != null)
      {
        reportEntry2.IterateNotes((System.Action<ReportManager.ReportEntry.Note>) (note => stressNotes.Add(note)));
        stressNotes.Sort((Comparison<ReportManager.ReportEntry.Note>) ((a, b) => a.value.CompareTo(b.value)));
        for (int index2 = 0; index2 < stressNotes.Count; ++index2)
        {
          this.stressDrawer.NewLabel(((double) stressNotes[index2].value <= 0.0 ? string.Empty : UIConstants.ColorPrefixRed) + stressNotes[index2].note + ": " + Util.FormatTwoDecimalPlace(stressNotes[index2].value) + "%" + ((double) stressNotes[index2].value <= 0.0 ? string.Empty : UIConstants.ColorSuffix));
          num += stressNotes[index2].value;
        }
      }
      this.stressDrawer.NewLabel(((double) num <= 0.0 ? string.Empty : UIConstants.ColorPrefixRed) + string.Format((string) STRINGS.UI.DETAILTABS.DETAILS.NET_STRESS, (object) Util.FormatTwoDecimalPlace(num)) + ((double) num <= 0.0 ? string.Empty : UIConstants.ColorSuffix));
      this.stressDrawer.EndDrawing();
    }
  }

  private void ShowAttributes(GameObject target)
  {
    Attributes attributes = target.GetAttributes();
    if (attributes == null)
      return;
    List<AttributeInstance> all = attributes.AttributeTable.FindAll((Predicate<AttributeInstance>) (a => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.General));
    if (all.Count <= 0)
      return;
    this.descriptionContainer.descriptors.gameObject.SetActive(true);
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (AttributeInstance attributeInstance in all)
    {
      Descriptor descriptor = new Descriptor(string.Format("{0}: {1}", (object) attributeInstance.Name, (object) attributeInstance.GetFormattedValue()), attributeInstance.GetAttributeValueTooltip(), Descriptor.DescriptorType.Effect, false);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    this.descriptionContainer.descriptors.SetDescriptors((IList<Descriptor>) descriptorList);
  }

  private void SetStamps(GameObject target)
  {
    for (int index = 0; index < this.stampContainer.transform.childCount; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.stampContainer.transform.GetChild(index).gameObject);
    if (!((UnityEngine.Object) target.GetComponent<BuildingComplete>() != (UnityEngine.Object) null))
      ;
  }

  [DebuggerDisplay("{item.item.Name}")]
  public class StatusItemEntry : IRenderEveryTick
  {
    private float fadeOutTime = 1.8f;
    public StatusItemGroup.Entry item;
    public StatusItemCategory category;
    private LayoutElement spacerLayout;
    private GameObject widget;
    private ToolTip toolTip;
    private TextStyleSetting tooltipStyle;
    public System.Action<SimpleInfoScreen.StatusItemEntry> onDestroy;
    private Image image;
    private LocText text;
    private KButton button;
    public Color color;
    public TextStyleSetting style;
    private SimpleInfoScreen.StatusItemEntry.FadeStage fadeStage;
    private float fade;
    private float fadeInTime;

    public StatusItemEntry(
      StatusItemGroup.Entry item,
      StatusItemCategory category,
      GameObject status_item_prefab,
      Transform parent,
      TextStyleSetting tooltip_style,
      Color color,
      TextStyleSetting style,
      bool skip_fade,
      System.Action<SimpleInfoScreen.StatusItemEntry> onDestroy)
    {
      this.item = item;
      this.category = category;
      this.tooltipStyle = tooltip_style;
      this.onDestroy = onDestroy;
      this.color = color;
      this.style = style;
      this.widget = Util.KInstantiateUI(status_item_prefab, parent.gameObject, false);
      this.text = this.widget.GetComponentInChildren<LocText>(true);
      SetTextStyleSetting.ApplyStyle((TextMeshProUGUI) this.text, style);
      this.toolTip = this.widget.GetComponentInChildren<ToolTip>(true);
      this.image = this.widget.GetComponentInChildren<Image>(true);
      item.SetIcon(this.image);
      this.widget.SetActive(true);
      this.toolTip.OnToolTip = new Func<string>(this.OnToolTip);
      this.button = this.widget.GetComponentInChildren<KButton>();
      if (item.item.statusItemClickCallback != null)
        this.button.onClick += new System.Action(this.OnClick);
      else
        this.button.enabled = false;
      this.fadeStage = !skip_fade ? SimpleInfoScreen.StatusItemEntry.FadeStage.IN : SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT;
      SimAndRenderScheduler.instance.Add((object) this, false);
      this.Refresh();
      this.SetColor(1f);
    }

    public Image GetImage
    {
      get
      {
        return this.image;
      }
    }

    internal void SetSprite(TintedSprite sprite)
    {
      if (sprite == null)
        return;
      this.image.sprite = sprite.sprite;
    }

    public int GetIndex()
    {
      return this.widget.transform.GetSiblingIndex();
    }

    public void SetIndex(int index)
    {
      this.widget.transform.SetSiblingIndex(index);
    }

    public void RenderEveryTick(float dt)
    {
      switch (this.fadeStage)
      {
        case SimpleInfoScreen.StatusItemEntry.FadeStage.IN:
          this.fade = Mathf.Min(this.fade + Time.deltaTime / this.fadeInTime, 1f);
          this.SetColor(this.fade);
          if ((double) this.fade < 1.0)
            break;
          this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT;
          break;
        case SimpleInfoScreen.StatusItemEntry.FadeStage.OUT:
          this.SetColor(this.fade);
          this.fade = Mathf.Max(this.fade - Time.deltaTime / this.fadeOutTime, 0.0f);
          if ((double) this.fade > 0.0)
            break;
          this.Destroy(true);
          break;
      }
    }

    private string OnToolTip()
    {
      this.item.ShowToolTip(this.toolTip, this.tooltipStyle);
      return string.Empty;
    }

    private void OnClick()
    {
      this.item.OnClick();
    }

    public void Refresh()
    {
      string name = this.item.GetName();
      if (!(name != this.text.text))
        return;
      this.text.text = name;
      this.SetColor(1f);
    }

    private void SetColor(float alpha = 1f)
    {
      Color color = new Color(this.color.r, this.color.g, this.color.b, alpha);
      this.image.color = color;
      this.text.color = color;
    }

    public void Destroy(bool immediate)
    {
      if (immediate)
      {
        if (this.onDestroy != null)
          this.onDestroy(this);
        SimAndRenderScheduler.instance.Remove((object) this);
        this.toolTip.OnToolTip = (Func<string>) null;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.widget);
      }
      else
      {
        this.fade = 0.5f;
        this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.OUT;
      }
    }

    private enum FadeStage
    {
      IN,
      WAIT,
      OUT,
    }
  }
}
