// Decompiled with JetBrains decompiler
// Type: SandboxToolParameterMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SandboxToolParameterMenu : KScreen
{
  private List<GameObject> inputFields = new List<GameObject>();
  public SandboxToolParameterMenu.SliderValue brushRadiusSlider = new SandboxToolParameterMenu.SliderValue(1f, 10f, "dash", "circle_hard", string.Empty, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.BrushSize = Mathf.RoundToInt(value)));
  public SandboxToolParameterMenu.SliderValue noiseScaleSlider = new SandboxToolParameterMenu.SliderValue(0.0f, 1f, "little", "lots", string.Empty, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.NoiseScale = value));
  public SandboxToolParameterMenu.SliderValue noiseDensitySlider = new SandboxToolParameterMenu.SliderValue(1f, 20f, "little", "lots", string.Empty, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.NoiseDensity = value));
  public SandboxToolParameterMenu.SliderValue massSlider = new SandboxToolParameterMenu.SliderValue(0.1f, 1000f, "action_pacify", "status_item_plant_solid", (string) STRINGS.UI.UNITSUFFIXES.MASS.KILOGRAM, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.MASS.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.Mass = (float) Mathf.RoundToInt(value * 10000f) / 10000f));
  public SandboxToolParameterMenu.SliderValue temperatureSlider = new SandboxToolParameterMenu.SliderValue(150f, 500f, "cold", "hot", (string) STRINGS.UI.UNITSUFFIXES.TEMPERATURE.KELVIN, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.temperature = Mathf.Clamp((float) Mathf.RoundToInt(value * 100f) / 100f, 1f, 9999f)));
  public SandboxToolParameterMenu.SliderValue temperatureAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-15f, 15f, "cold", "hot", (string) STRINGS.UI.UNITSUFFIXES.TEMPERATURE.KELVIN, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.temperatureAdditive = (float) Mathf.RoundToInt(value * 100f) / 100f));
  public SandboxToolParameterMenu.SliderValue diseaseCountSlider = new SandboxToolParameterMenu.SliderValue(0.0f, 10000f, "status_item_barren", "germ", (string) STRINGS.UI.UNITSUFFIXES.DISEASE.UNITS, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.TOOLTIP, (System.Action<float>) (value => SandboxToolParameterMenu.instance.settings.diseaseCount = Mathf.RoundToInt(value)));
  public static SandboxToolParameterMenu instance;
  public SandboxSettings settings;
  [SerializeField]
  private GameObject sliderPropertyPrefab;
  [SerializeField]
  private GameObject selectorPropertyPrefab;
  public SandboxToolParameterMenu.SelectorValue elementSelector;
  public SandboxToolParameterMenu.SelectorValue diseaseSelector;
  public SandboxToolParameterMenu.SelectorValue entitySelector;

  public static void DestroyInstance()
  {
    SandboxToolParameterMenu.instance = (SandboxToolParameterMenu) null;
  }

  public override float GetSortKey()
  {
    return 100f;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.settings = new SandboxSettings();
    this.settings.OnChangeElement += (System.Action) (() =>
    {
      this.elementSelector.button.GetComponentInChildren<LocText>().text = SandboxToolParameterMenu.instance.settings.Element.name + " (" + SandboxToolParameterMenu.instance.settings.Element.GetStateString() + ")";
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) this.settings.Element, "ui", false);
      this.elementSelector.button.GetComponentsInChildren<Image>()[1].sprite = uiSprite.first;
      this.elementSelector.button.GetComponentsInChildren<Image>()[1].color = uiSprite.second;
      this.temperatureSlider.SetRange(Mathf.Max(SandboxToolParameterMenu.instance.settings.Element.lowTemp - 10f, 1f), Mathf.Min(9999f, SandboxToolParameterMenu.instance.settings.Element.highTemp + 10f));
      this.temperatureSlider.SetValue(SandboxToolParameterMenu.instance.settings.Element.defaultValues.temperature);
      this.massSlider.SetRange(0.1f, SandboxToolParameterMenu.instance.settings.Element.defaultValues.mass * 2f);
    });
    this.settings.OnChangeDisease += (System.Action) (() =>
    {
      this.diseaseSelector.button.GetComponentInChildren<LocText>().text = SandboxToolParameterMenu.instance.settings.Disease.Name;
      this.diseaseSelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite((HashedString) "germ");
      this.diseaseCountSlider.SetRange(0.0f, 1000000f);
    });
    this.settings.OnChangeEntity += (System.Action) (() =>
    {
      this.entitySelector.button.GetComponentInChildren<LocText>().text = SandboxToolParameterMenu.instance.settings.Entity.GetProperName();
      Tuple<Sprite, Color> tuple = !(this.settings.Entity.PrefabTag == (Tag) MinionConfig.ID) ? Def.GetUISprite((object) this.settings.Entity.PrefabTag, "ui", false) : new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_duplicant_portrait_placeholder"), Color.white);
      if (tuple == null)
        return;
      this.entitySelector.button.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
      this.entitySelector.button.GetComponentsInChildren<Image>()[1].color = tuple.second;
    });
    this.settings.OnChangeBrushSize += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is BrushTool))
        return;
      (PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.BrushSize);
    });
    this.settings.OnChangeNoiseScale += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is BrushTool))
        return;
      (PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.BrushSize);
    });
    this.settings.OnChangeNoiseDensity += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is BrushTool))
        return;
      (PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.BrushSize);
    });
    this.settings.InstantBuild = true;
    this.activateOnSpawn = true;
    this.ConsumeMouseScroll = true;
  }

  public void DisableParameters()
  {
    this.elementSelector.row.SetActive(false);
    this.entitySelector.row.SetActive(false);
    this.brushRadiusSlider.row.SetActive(false);
    this.noiseScaleSlider.row.SetActive(false);
    this.noiseDensitySlider.row.SetActive(false);
    this.massSlider.row.SetActive(false);
    this.temperatureAdditiveSlider.row.SetActive(false);
    this.temperatureSlider.row.SetActive(false);
    this.diseaseCountSlider.row.SetActive(false);
    this.diseaseSelector.row.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConfigureElementSelector();
    this.ConfigureDiseaseSelector();
    this.ConfigureEntitySelector();
    this.SpawnSelector(this.entitySelector);
    this.SpawnSelector(this.elementSelector);
    this.SpawnSlider(this.brushRadiusSlider);
    this.SpawnSlider(this.noiseScaleSlider);
    this.SpawnSlider(this.noiseDensitySlider);
    this.SpawnSlider(this.massSlider);
    this.SpawnSlider(this.temperatureSlider);
    this.SpawnSlider(this.temperatureAdditiveSlider);
    this.SpawnSelector(this.diseaseSelector);
    this.SpawnSlider(this.diseaseCountSlider);
    if (!((UnityEngine.Object) SandboxToolParameterMenu.instance == (UnityEngine.Object) null))
      return;
    SandboxToolParameterMenu.instance = this;
    this.gameObject.SetActive(false);
    this.settings.SelectElement(ElementLoader.FindElementByHash(SimHashes.Water));
    this.brushRadiusSlider.SetRange(1f, 10f);
    this.brushRadiusSlider.slider.wholeNumbers = true;
    this.noiseScaleSlider.SetRange(0.0f, 1f);
    this.noiseDensitySlider.SetRange(0.0f, 20f);
    this.temperatureSlider.SetRange(Mathf.Max(SandboxToolParameterMenu.instance.settings.Element.lowTemp - 10f, 1f), SandboxToolParameterMenu.instance.settings.Element.highTemp + 10f);
    this.massSlider.SetRange(0.1f, SandboxToolParameterMenu.instance.settings.Element.defaultValues.mass * 2f);
    this.massSlider.SetValue(this.settings.Mass);
    this.settings.SelectDisease(Db.Get().Diseases.FoodGerms);
    this.settings.SelectEntity(Assets.GetPrefab("MushBar".ToTag()).GetComponent<KPrefabID>());
  }

  private void ConfigureElementSelector()
  {
    Func<object, bool> condition1 = (Func<object, bool>) (element => (element as Element).IsSolid);
    Func<object, bool> condition2 = (Func<object, bool>) (element => (element as Element).IsLiquid);
    Func<object, bool> condition3 = (Func<object, bool>) (element => (element as Element).IsGas);
    List<Element> commonElements = new List<Element>();
    Func<object, bool> condition4 = (Func<object, bool>) (element => commonElements.Contains(element as Element));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Oxygen));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Water));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Vacuum));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Dirt));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SandStone));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Cuprite));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Algae));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CarbonDioxide));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Sand));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SlimeMold));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Granite));
    List<Element> elementList = new List<Element>();
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
        elementList.Add(element);
    }
    elementList.Sort((Comparison<Element>) ((a, b) => a.name.CompareTo(b.name)));
    this.elementSelector = new SandboxToolParameterMenu.SelectorValue((object[]) elementList.ToArray(), (System.Action<object>) (element => this.settings.SelectElement(element as Element)), (Func<object, string>) (element => (element as Element).name + " (" + (element as Element).GetStateString() + ")"), (Func<string, object, bool>) ((filterString, option) => ((option as Element).name.ToUpper() + (option as Element).GetStateString().ToUpper()).Contains(filterString.ToUpper())), (Func<object, Tuple<Sprite, Color>>) (element => Def.GetUISprite((object) (element as Element), "ui", false)), new SandboxToolParameterMenu.SelectorValue.SearchFilter[4]
    {
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.COMMON, condition4, (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, (Tuple<Sprite, Color>) null),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.SOLID, condition1, (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.SandStone), "ui", false)),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.LIQUID, condition2, (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.Water), "ui", false)),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.GAS, condition3, (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.Oxygen), "ui", false))
    });
  }

  private void ConfigureEntitySelector()
  {
    List<SandboxToolParameterMenu.SelectorValue.SearchFilter> searchFilterList = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
    searchFilterList.Add(new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.FOOD, (Func<object, bool>) (entity =>
    {
      string idString = (entity as KPrefabID).PrefabID().ToString();
      if (!(entity as KPrefabID).HasTag(GameTags.Egg))
        return TUNING.FOOD.FOOD_TYPES_LIST.Find((Predicate<EdiblesManager.FoodInfo>) (match => match.Id == idString)) != null;
      return false;
    }), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) Assets.GetPrefab((Tag) "MushBar"), "ui", false)));
    searchFilterList.Add(new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL, (Func<object, bool>) (entity =>
    {
      if (!((entity as KPrefabID).PrefabID().Name == MinionConfig.ID) && !((entity as KPrefabID).PrefabID().Name == DustCometConfig.ID) && !((entity as KPrefabID).PrefabID().Name == RockCometConfig.ID))
        return (entity as KPrefabID).PrefabID().Name == IronCometConfig.ID;
      return true;
    }), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_duplicant_portrait_placeholder"), Color.white)));
    SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter1 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE, (Func<object, bool>) (entity => false), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) Assets.GetPrefab((Tag) "Hatch"), "ui", false));
    searchFilterList.Add(parentFilter1);
    List<Tag> tagList = new List<Tag>();
    foreach (GameObject gameObject in Assets.GetPrefabsWithTag("CreatureBrain".ToTag()))
    {
      CreatureBrain brain = gameObject.GetComponent<CreatureBrain>();
      if (!tagList.Contains(brain.species))
      {
        Tuple<Sprite, Color> icon = new Tuple<Sprite, Color>(CodexCache.entries[brain.species.ToString().ToUpper()].icon, CodexCache.entries[brain.species.ToString().ToUpper()].iconColor);
        tagList.Add(brain.species);
        SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + brain.species.ToString().ToUpper()), (Func<object, bool>) (entity =>
        {
          CreatureBrain component = Assets.GetPrefab((entity as KPrefabID).PrefabID()).GetComponent<CreatureBrain>();
          if ((entity as KPrefabID).HasTag((Tag) "CreatureBrain".ToString()))
            return component.species == brain.species;
          return false;
        }), parentFilter1, icon);
        searchFilterList.Add(searchFilter);
      }
    }
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter1 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE_EGG, (Func<object, bool>) (entity => (entity as KPrefabID).HasTag(GameTags.Egg)), parentFilter1, Def.GetUISprite((object) Assets.GetPrefab((Tag) "HatchEgg"), "ui", false));
    searchFilterList.Add(searchFilter1);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.EQUIPMENT, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        return (UnityEngine.Object) gameObject.GetComponent<Equippable>() != (UnityEngine.Object) null;
      return false;
    }), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) Assets.GetPrefab((Tag) "Funky_Vest"), "ui", false));
    searchFilterList.Add(searchFilter2);
    SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.PLANTS, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return false;
      if (!((UnityEngine.Object) gameObject.GetComponent<Harvestable>() != (UnityEngine.Object) null))
        return (UnityEngine.Object) gameObject.GetComponent<WiltCondition>() != (UnityEngine.Object) null;
      return true;
    }), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) Assets.GetPrefab((Tag) "PrickleFlower"), "ui", false));
    searchFilterList.Add(parentFilter2);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter3 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SEEDS, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        return (UnityEngine.Object) gameObject.GetComponent<PlantableSeed>() != (UnityEngine.Object) null;
      return false;
    }), parentFilter2, Def.GetUISprite((object) Assets.GetPrefab((Tag) "PrickleFlowerSeed"), "ui", false));
    searchFilterList.Add(searchFilter3);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter4 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.INDUSTRIAL_PRODUCTS, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return false;
      if (!gameObject.HasTag(GameTags.IndustrialIngredient) && !gameObject.HasTag(GameTags.IndustrialProduct) && !gameObject.HasTag(GameTags.Medicine))
        return gameObject.HasTag(GameTags.MedicalSupplies);
      return true;
    }), (SandboxToolParameterMenu.SelectorValue.SearchFilter) null, Def.GetUISprite((object) Assets.GetPrefab((Tag) "BasicCure"), "ui", false));
    searchFilterList.Add(searchFilter4);
    List<KPrefabID> kprefabIdList = new List<KPrefabID>();
    foreach (KPrefabID prefab in Assets.Prefabs)
    {
      foreach (SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter5 in searchFilterList)
      {
        if (searchFilter5.condition((object) prefab))
        {
          kprefabIdList.Add(prefab);
          break;
        }
      }
    }
    this.entitySelector = new SandboxToolParameterMenu.SelectorValue((object[]) kprefabIdList.ToArray(), (System.Action<object>) (entity => this.settings.SelectEntity(entity as KPrefabID)), (Func<object, string>) (entity => (entity as KPrefabID).GetProperName()), (Func<string, object, bool>) ((filterString, option) => (option as KPrefabID).GetProperName().ToUpper().Contains(filterString.ToUpper())), (Func<object, Tuple<Sprite, Color>>) (entity =>
    {
      GameObject prefab = Assets.GetPrefab((entity as KPrefabID).PrefabTag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        if (prefab.PrefabID() == (Tag) MinionConfig.ID)
          return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_duplicant_portrait_placeholder"), Color.white);
        KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.AnimFiles.Length > 0 && (UnityEngine.Object) component.AnimFiles[0] != (UnityEngine.Object) null)
          return Def.GetUISprite((object) prefab, "ui", false);
      }
      return (Tuple<Sprite, Color>) null;
    }), searchFilterList.ToArray());
  }

  private void ConfigureDiseaseSelector()
  {
    this.diseaseSelector = new SandboxToolParameterMenu.SelectorValue((object[]) Db.Get().Diseases.resources.ToArray(), (System.Action<object>) (disease => this.settings.SelectDisease(disease as Klei.AI.Disease)), (Func<object, string>) (disease => (disease as Klei.AI.Disease).Name), (Func<string, object, bool>) ((filterText, option) => (option as Klei.AI.Disease).Name.ToUpper().Contains(filterText.ToUpper())), (Func<object, Tuple<Sprite, Color>>) (disease => new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "germ"), (Color) (disease as Klei.AI.Disease).overlayColour)), (SandboxToolParameterMenu.SelectorValue.SearchFilter[]) null);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!((UnityEngine.Object) PlayerController.Instance.ActiveTool != (UnityEngine.Object) null) || !((UnityEngine.Object) SandboxToolParameterMenu.instance != (UnityEngine.Object) null))
      return;
    this.RefreshDisplay();
  }

  public void RefreshDisplay()
  {
    this.brushRadiusSlider.row.SetActive(PlayerController.Instance.ActiveTool is BrushTool);
    if (PlayerController.Instance.ActiveTool is BrushTool)
      this.brushRadiusSlider.SetValue((float) this.settings.BrushSize);
    this.massSlider.SetValue(this.settings.Mass);
    this.temperatureSlider.SetValue(this.settings.temperature);
    this.temperatureAdditiveSlider.SetValue(this.settings.temperatureAdditive);
    this.diseaseCountSlider.SetValue((float) this.settings.diseaseCount);
  }

  private GameObject SpawnSelector(SandboxToolParameterMenu.SelectorValue selector)
  {
    GameObject gameObject1 = Util.KInstantiateUI(this.selectorPropertyPrefab, this.gameObject, true);
    HierarchyReferences component = gameObject1.GetComponent<HierarchyReferences>();
    GameObject panel = component.GetReference("ScrollPanel").gameObject;
    GameObject gameObject2 = component.GetReference("Content").gameObject;
    InputField filterInputField = component.GetReference<InputField>("Filter");
    KButton reference = component.GetReference<KButton>("Button");
    reference.onClick += (System.Action) (() =>
    {
      panel.SetActive(!panel.activeSelf);
      if (!panel.activeSelf)
        return;
      panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
      filterInputField.ActivateInputField();
    });
    GameObject gameObject3 = component.GetReference("optionPrefab").gameObject;
    selector.row = gameObject1;
    selector.optionButtons = new List<KeyValuePair<object, GameObject>>();
    if (selector.filters != null)
    {
      GameObject clearFilterButton = Util.KInstantiateUI(gameObject3, gameObject2, false);
      clearFilterButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.SANDBOXTOOLS.FILTERS.BACK;
      clearFilterButton.GetComponentsInChildren<Image>()[1].enabled = false;
      clearFilterButton.GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        selector.currentFilter = (SandboxToolParameterMenu.SelectorValue.SearchFilter) null;
        selector.optionButtons.ForEach((System.Action<KeyValuePair<object, GameObject>>) (test =>
        {
          if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
            test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
          else
            test.Value.SetActive(false);
        }));
        clearFilterButton.SetActive(false);
        panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
      });
      foreach (SandboxToolParameterMenu.SelectorValue.SearchFilter filter1 in selector.filters)
      {
        SandboxToolParameterMenu.SelectorValue.SearchFilter filter = filter1;
        GameObject gameObject4 = Util.KInstantiateUI(gameObject3, gameObject2, false);
        gameObject4.SetActive(filter.parentFilter == null);
        gameObject4.GetComponentInChildren<LocText>().text = filter.Name;
        if (filter.icon != null)
        {
          gameObject4.GetComponentsInChildren<Image>()[1].sprite = filter.icon.first;
          gameObject4.GetComponentsInChildren<Image>()[1].color = filter.icon.second;
        }
        gameObject4.GetComponent<KButton>().onClick += (System.Action) (() =>
        {
          selector.currentFilter = filter;
          clearFilterButton.SetActive(true);
          selector.optionButtons.ForEach((System.Action<KeyValuePair<object, GameObject>>) (test =>
          {
            if (!(test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter))
              test.Value.SetActive(selector.runCurrentFilter(test.Key));
            else if ((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null)
              test.Value.SetActive(false);
            else
              test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == filter);
          }));
          panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
        });
        selector.optionButtons.Add(new KeyValuePair<object, GameObject>((object) filter, gameObject4));
      }
    }
    foreach (object option1 in selector.options)
    {
      object option = option1;
      GameObject gameObject4 = Util.KInstantiateUI(gameObject3, gameObject2, true);
      gameObject4.GetComponentInChildren<LocText>().text = selector.getOptionName(option);
      gameObject4.GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        selector.onValueChanged(option);
        panel.SetActive(false);
      });
      Tuple<Sprite, Color> tuple = selector.getOptionSprite(option);
      gameObject4.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
      gameObject4.GetComponentsInChildren<Image>()[1].color = tuple.second;
      selector.optionButtons.Add(new KeyValuePair<object, GameObject>(option, gameObject4));
      if (option is SandboxToolParameterMenu.SelectorValue.SearchFilter)
        gameObject4.SetActive((option as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
      else
        gameObject4.SetActive(false);
    }
    selector.button = reference;
    filterInputField.onValueChanged.AddListener((UnityAction<string>) (filterString =>
    {
      List<KeyValuePair<object, GameObject>> keyValuePairList = new List<KeyValuePair<object, GameObject>>();
      selector.optionButtons.ForEach((System.Action<KeyValuePair<object, GameObject>>) (test =>
      {
        if (!(test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter))
          return;
        test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).Name.ToUpper().Contains(filterString.ToUpper()));
      }));
      foreach (object option1 in selector.options)
      {
        object option = option1;
        foreach (KeyValuePair<object, GameObject> keyValuePair in selector.optionButtons.FindAll((Predicate<KeyValuePair<object, GameObject>>) (match => match.Key == option)))
        {
          if (filterString == string.Empty)
            keyValuePair.Value.SetActive(false);
          else
            keyValuePair.Value.SetActive(selector.filterOptionFunction(filterString, option));
        }
      }
      panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
    }));
    this.inputFields.Add(filterInputField.gameObject);
    panel.SetActive(false);
    return gameObject1;
  }

  private GameObject SpawnSlider(SandboxToolParameterMenu.SliderValue value)
  {
    GameObject gameObject = Util.KInstantiateUI(this.sliderPropertyPrefab, this.gameObject, true);
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("BottomIcon").sprite = Assets.GetSprite((HashedString) value.bottomSprite);
    component.GetReference<Image>("TopIcon").sprite = Assets.GetSprite((HashedString) value.topSprite);
    KSlider slider = component.GetReference<KSlider>("Slider");
    KNumberInputField inputField = component.GetReference<KNumberInputField>("InputField");
    gameObject.GetComponent<ToolTip>().SetSimpleTooltip(value.tooltip);
    slider.minValue = value.minValue;
    slider.maxValue = value.maxValue;
    inputField.minValue = 0.0f;
    inputField.maxValue = 99999f;
    this.inputFields.Add(inputField.gameObject);
    value.slider = slider;
    value.inputField = inputField;
    value.row = gameObject;
    slider.onReleaseHandle += (System.Action) (() =>
    {
      slider.value = Mathf.Round(slider.value * 10f) / 10f;
      inputField.currentValue = slider.value;
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    slider.onDrag += (System.Action) (() =>
    {
      slider.value = Mathf.Round(slider.value * 10f) / 10f;
      inputField.currentValue = slider.value;
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    slider.onMove += (System.Action) (() =>
    {
      slider.value = Mathf.Round(slider.value * 10f) / 10f;
      inputField.currentValue = slider.value;
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    inputField.onEndEdit += (System.Action) (() =>
    {
      float f = Mathf.Clamp(Mathf.Round(inputField.currentValue), inputField.minValue, inputField.maxValue);
      inputField.SetDisplayValue(f.ToString());
      slider.value = Mathf.Round(f);
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(f);
    });
    component.GetReference<LocText>("UnitLabel").text = value.unitString;
    return gameObject;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.CheckBlockedInput())
    {
      if (e.Consumed)
        return;
      e.Consumed = true;
    }
    else
      base.OnKeyDown(e);
  }

  private bool CheckBlockedInput()
  {
    bool flag = false;
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
    {
      GameObject selectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject != (UnityEngine.Object) null)
      {
        foreach (GameObject inputField in this.inputFields)
        {
          if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) inputField.gameObject)
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }

  public class SelectorValue
  {
    public List<SandboxToolParameterMenu.SelectorValue.SearchFilter> activeFilters = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
    public GameObject row;
    public List<KeyValuePair<object, GameObject>> optionButtons;
    public KButton button;
    public object[] options;
    public System.Action<object> onValueChanged;
    public Func<object, string> getOptionName;
    public Func<string, object, bool> filterOptionFunction;
    public Func<object, Tuple<Sprite, Color>> getOptionSprite;
    public SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters;
    public SandboxToolParameterMenu.SelectorValue.SearchFilter currentFilter;

    public SelectorValue(
      object[] options,
      System.Action<object> onValueChanged,
      Func<object, string> getOptionName,
      Func<string, object, bool> filterOptionFunction,
      Func<object, Tuple<Sprite, Color>> getOptionSprite,
      SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = null)
    {
      this.options = options;
      this.onValueChanged = onValueChanged;
      this.getOptionName = getOptionName;
      this.filterOptionFunction = filterOptionFunction;
      this.getOptionSprite = getOptionSprite;
      this.filters = filters;
    }

    public bool runCurrentFilter(object obj)
    {
      return this.currentFilter == null || this.currentFilter.condition(obj);
    }

    public class SearchFilter
    {
      public string Name;
      public Func<object, bool> condition;
      public SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter;
      public Tuple<Sprite, Color> icon;

      public SearchFilter(
        string Name,
        Func<object, bool> condition,
        SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter = null,
        Tuple<Sprite, Color> icon = null)
      {
        this.Name = Name;
        this.condition = condition;
        this.parentFilter = parentFilter;
        this.icon = icon;
      }
    }
  }

  public class SliderValue
  {
    public GameObject row;
    public string bottomSprite;
    public string topSprite;
    public float minValue;
    public float maxValue;
    public string unitString;
    public System.Action<float> onValueChanged;
    public string tooltip;
    public KSlider slider;
    public KNumberInputField inputField;

    public SliderValue(
      float minValue,
      float maxValue,
      string bottomSprite,
      string topSprite,
      string unitString,
      string tooltip,
      System.Action<float> onValueChanged)
    {
      this.minValue = minValue;
      this.maxValue = maxValue;
      this.bottomSprite = bottomSprite;
      this.topSprite = topSprite;
      this.unitString = unitString;
      this.onValueChanged = onValueChanged;
      this.tooltip = tooltip;
    }

    public void SetRange(float min, float max)
    {
      this.minValue = min;
      this.maxValue = max;
      this.slider.minValue = this.minValue;
      this.slider.maxValue = this.maxValue;
      this.inputField.currentValue = this.minValue + (float) (((double) this.maxValue - (double) this.minValue) / 2.0);
      this.inputField.SetDisplayValue(this.inputField.currentValue.ToString());
      this.slider.value = this.minValue + (float) (((double) this.maxValue - (double) this.minValue) / 2.0);
      this.onValueChanged(this.minValue + (float) (((double) this.maxValue - (double) this.minValue) / 2.0));
    }

    public void SetValue(float value)
    {
      this.slider.value = value;
      this.inputField.currentValue = value;
      this.onValueChanged(value);
      this.RefreshDisplay();
    }

    public void RefreshDisplay()
    {
      this.inputField.SetDisplayValue(this.inputField.currentValue.ToString());
    }
  }
}
