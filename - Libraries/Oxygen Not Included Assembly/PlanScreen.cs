// Decompiled with JetBrains decompiler
// Type: PlanScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlanScreen : KIconToggleMenu
{
  private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>()
  {
    {
      PlanScreen.CacheHashedString("Base"),
      "icon_category_base"
    },
    {
      PlanScreen.CacheHashedString("Oxygen"),
      "icon_category_oxygen"
    },
    {
      PlanScreen.CacheHashedString("Power"),
      "icon_category_electrical"
    },
    {
      PlanScreen.CacheHashedString("Food"),
      "icon_category_food"
    },
    {
      PlanScreen.CacheHashedString("Plumbing"),
      "icon_category_plumbing"
    },
    {
      PlanScreen.CacheHashedString("HVAC"),
      "icon_category_ventilation"
    },
    {
      PlanScreen.CacheHashedString("Refining"),
      "icon_category_refinery"
    },
    {
      PlanScreen.CacheHashedString("Medical"),
      "icon_category_medical"
    },
    {
      PlanScreen.CacheHashedString("Furniture"),
      "icon_category_furniture"
    },
    {
      PlanScreen.CacheHashedString("Equipment"),
      "icon_category_misc"
    },
    {
      PlanScreen.CacheHashedString("Utilities"),
      "icon_category_utilities"
    },
    {
      PlanScreen.CacheHashedString("Automation"),
      "icon_category_automation"
    },
    {
      PlanScreen.CacheHashedString("Conveyance"),
      "icon_category_shipping"
    },
    {
      PlanScreen.CacheHashedString("Rocketry"),
      "icon_category_rocketry"
    }
  };
  private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();
  public Dictionary<BuildingDef, KToggle> ActiveToggles = new Dictionary<BuildingDef, KToggle>();
  private float notificationPingExpire = 0.5f;
  private float specialNotificationEmbellishDelay = 8f;
  private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();
  private Dictionary<Def, PlanScreen.RequirementsState> buildableDefs = new Dictionary<Def, PlanScreen.RequirementsState>();
  private float buildGrid_bg_width = 274f;
  private float buildGrid_bg_borderHeight = 32f;
  private int buildGrid_maxRowsBeforeScroll = 5;
  [SerializeField]
  private GameObject planButtonPrefab;
  [SerializeField]
  private GameObject recipeInfoScreenParent;
  [SerializeField]
  private GameObject productInfoScreenPrefab;
  [SerializeField]
  private GameObject copyBuildingButton;
  private ProductInfoScreen productInfoScreen;
  [SerializeField]
  public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;
  public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;
  private KIconToggleMenu.ToggleInfo activeCategoryInfo;
  private float timeSinceNotificationPing;
  private int notificationPingCount;
  private GameObject selectedBuildingGameObject;
  public Transform GroupsTransform;
  public Sprite Overlay_NeedTech;
  public RectTransform buildingGroupsRoot;
  public RectTransform BuildButtonBGPanel;
  public RectTransform BuildingGroupContentsRect;
  public Sprite defaultBuildingIconSprite;
  public Material defaultUIMaterial;
  public Material desaturatedUIMaterial;
  public LocText PlanCategoryLabel;
  private int ignoreToolChangeMessages;
  [SerializeField]
  private TextStyleSetting[] CategoryLabelTextStyles;
  private float initTime;
  private Dictionary<Tag, HashedString> tagCategoryMap;
  private Dictionary<Tag, int> tagOrderMap;
  private int buildable_state_update_idx;
  private int building_button_refresh_idx;
  private float buildGrid_bg_rowHeight;

  public static PlanScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    PlanScreen.Instance = (PlanScreen) null;
  }

  public static Dictionary<HashedString, string> IconNameMap
  {
    get
    {
      return PlanScreen.iconNameMap;
    }
  }

  private static HashedString CacheHashedString(string str)
  {
    return HashCache.Get().Add(str);
  }

  public override float GetSortKey()
  {
    return 2f;
  }

  public PlanScreen.RequirementsState BuildableState(BuildingDef def)
  {
    PlanScreen.RequirementsState requirementsState;
    if ((UnityEngine.Object) def == (UnityEngine.Object) null || !this.buildableDefs.TryGetValue((Def) def, out requirementsState))
      requirementsState = PlanScreen.RequirementsState.Materials;
    return requirementsState;
  }

  protected override void OnPrefabInit()
  {
    if (BuildMenu.UseHotkeyBuildMenu())
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      base.OnPrefabInit();
      this.productInfoScreen = Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, true);
      this.productInfoScreen.rectTransform().pivot = new Vector2(0.0f, 0.0f);
      this.productInfoScreen.rectTransform().SetLocalPosition(new Vector3(280f, 0.0f, 0.0f));
      this.productInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
      Game.Instance.Subscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
      Game.Instance.Subscribe(1174281782, new System.Action<object>(this.OnActiveToolChanged));
    }
    this.buildingGroupsRoot.gameObject.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.initTime = KTime.Instance.UnscaledGameTime;
    if (BuildMenu.UseHotkeyBuildMenu())
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      PlanScreen.Instance = this;
      this.onSelect += new KIconToggleMenu.OnSelect(this.OnClickCategory);
      this.Refresh();
      foreach (Toggle toggle in this.toggles)
        toggle.group = this.GetComponent<ToggleGroup>();
      this.GetBuildableStates(true);
      Game.Instance.Subscribe(288942073, new System.Action<object>(this.OnUIClear));
    }
    this.copyBuildingButton.GetComponent<MultiToggle>().onClick = (System.Action) (() => this.OnClickCopyBuilding());
    this.RefreshCopyBuildingButton((object) null);
    Game.Instance.Subscribe(-1503271301, new System.Action<object>(this.RefreshCopyBuildingButton));
    this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.COPY_BUILDING_TOOLTIP, Action.CopyBuilding));
  }

  private void OnClickCopyBuilding()
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
      return;
    Building component = SelectTool.Instance.selected.GetComponent<Building>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Def.ShowInBuildMenu || component.Def.Deprecated)
      return;
    PlanScreen.Instance.CopyBuildingOrder(component);
    this.copyBuildingButton.SetActive(false);
  }

  public void RefreshCopyBuildingButton(object data = null)
  {
    MultiToggle component1 = this.copyBuildingButton.GetComponent<MultiToggle>();
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
    {
      component1.gameObject.SetActive(false);
      component1.ChangeState(0);
    }
    else
    {
      Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.Def.ShowInBuildMenu && !component2.Def.Deprecated)
      {
        Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) component2.gameObject, "ui", false);
        component1.gameObject.SetActive(true);
        component1.transform.Find("FG").GetComponent<Image>().sprite = uiSprite.first;
        component1.transform.Find("FG").GetComponent<Image>().color = Color.white;
        component1.ChangeState(1);
      }
      else
      {
        component1.gameObject.SetActive(false);
        component1.ChangeState(0);
      }
    }
  }

  public void Refresh()
  {
    List<KIconToggleMenu.ToggleInfo> toggleInfoList = new List<KIconToggleMenu.ToggleInfo>();
    if (this.tagCategoryMap != null)
      return;
    int building_index = 0;
    this.tagCategoryMap = new Dictionary<Tag, HashedString>();
    this.tagOrderMap = new Dictionary<Tag, int>();
    if (TUNING.BUILDINGS.PLANORDER.Count > 12)
      DebugUtil.LogWarningArgs((object) "Insufficient keys to cover root plan menu", (object) ("Max of 12 keys supported but TUNING.BUILDINGS.PLANORDER has " + (object) TUNING.BUILDINGS.PLANORDER.Count));
    this.toggleEntries.Clear();
    for (int index = 0; index < TUNING.BUILDINGS.PLANORDER.Count; ++index)
    {
      PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[index];
      Action hotkey = index >= 12 ? Action.NumActions : (Action) (36 + index);
      string iconName = PlanScreen.iconNameMap[planInfo.category];
      string upper = HashCache.Get().Get(planInfo.category).ToUpper();
      KIconToggleMenu.ToggleInfo toggle_info = new KIconToggleMenu.ToggleInfo(STRINGS.UI.StripLinkFormatting((string) Strings.Get("STRINGS.UI.BUILDCATEGORIES." + upper + ".NAME")), iconName, (object) planInfo.category, hotkey, (string) Strings.Get("STRINGS.UI.BUILDCATEGORIES." + upper + ".TOOLTIP"), string.Empty);
      toggleInfoList.Add(toggle_info);
      PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.data, this.tagCategoryMap, this.tagOrderMap, ref building_index);
      List<BuildingDef> building_defs = new List<BuildingDef>();
      foreach (BuildingDef buildingDef in Assets.BuildingDefs)
      {
        HashedString hashedString;
        if (!buildingDef.Deprecated && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && !(hashedString != planInfo.category))
          building_defs.Add(buildingDef);
      }
      this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggle_info, planInfo.category, building_defs, planInfo.hideIfNotResearched));
    }
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) toggleInfoList);
    this.toggles.ForEach((System.Action<KToggle>) (to =>
    {
      foreach (ImageToggleState component in to.GetComponents<ImageToggleState>())
      {
        if ((UnityEngine.Object) component.TargetImage.sprite != (UnityEngine.Object) null && component.TargetImage.name == "FG" && !component.useSprites)
          component.SetSprites(Assets.GetSprite((HashedString) (component.TargetImage.sprite.name + "_disabled")), component.TargetImage.sprite, component.TargetImage.sprite, Assets.GetSprite((HashedString) (component.TargetImage.sprite.name + "_disabled")));
      }
      to.GetComponent<KToggle>().soundPlayer.Enabled = false;
    }));
    for (int index = 0; index < this.toggleEntries.Count; ++index)
    {
      PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[index];
      toggleEntry.CollectToggleImages();
      this.toggleEntries[index] = toggleEntry;
    }
  }

  public void CopyBuildingOrder(Building building)
  {
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (string str in (List<string>) planInfo.data)
      {
        if (building.Def.PrefabID == str)
        {
          this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
          this.OnSelectBuilding(this.ActiveToggles[building.Def].gameObject, building.Def);
          this.productInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
          break;
        }
      }
    }
  }

  private static void PopulateOrderInfo(
    HashedString category,
    object data,
    Dictionary<Tag, HashedString> category_map,
    Dictionary<Tag, int> order_map,
    ref int building_index)
  {
    if (data.GetType() == typeof (PlanScreen.PlanInfo))
    {
      PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo) data;
      PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.data, category_map, order_map, ref building_index);
    }
    else
    {
      foreach (string name in (IEnumerable<string>) data)
      {
        Tag index = new Tag(name);
        category_map[index] = category;
        order_map[index] = building_index;
        ++building_index;
      }
    }
  }

  protected override void OnCmpEnable()
  {
    this.Refresh();
    this.productInfoScreen.Show(false);
  }

  protected override void OnCmpDisable()
  {
    this.ClearButtons();
  }

  private void ClearButtons()
  {
    foreach (KeyValuePair<BuildingDef, KToggle> activeToggle in this.ActiveToggles)
    {
      activeToggle.Value.gameObject.SetActive(false);
      activeToggle.Value.transform.SetParent((Transform) null);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) activeToggle.Value.gameObject);
    }
    this.ActiveToggles.Clear();
  }

  public void OnSelectBuilding(GameObject button_go, BuildingDef def)
  {
    if ((UnityEngine.Object) button_go == (UnityEngine.Object) null)
      Debug.Log((object) "Button gameObject is null", (UnityEngine.Object) this.gameObject);
    else if ((UnityEngine.Object) button_go == (UnityEngine.Object) this.selectedBuildingGameObject)
    {
      this.CloseRecipe(true);
    }
    else
    {
      ++this.ignoreToolChangeMessages;
      this.selectedBuildingGameObject = button_go;
      this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
      PlanScreen.ToggleEntry toggleEntry;
      if (this.GetToggleEntryForCategory(this.tagCategoryMap[def.Tag], out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
      {
        toggleEntry.pendingResearchAttentions.Remove(def.Tag);
        button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        if (toggleEntry.pendingResearchAttentions.Count == 0)
          toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
      }
      this.productInfoScreen.ClearProduct(false);
      ToolMenu.Instance.ClearSelection();
      PrebuildTool.Instance.Activate(def, this.BuildableState(def));
      this.productInfoScreen.Show(true);
      this.productInfoScreen.ConfigureScreen(def);
      --this.ignoreToolChangeMessages;
    }
  }

  private void GetBuildableStates(bool force_update)
  {
    if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
      return;
    if ((double) this.timeSinceNotificationPing < (double) this.specialNotificationEmbellishDelay)
      this.timeSinceNotificationPing += Time.unscaledDeltaTime;
    if ((double) this.timeSinceNotificationPing >= (double) this.notificationPingExpire)
      this.notificationPingCount = 0;
    int num1 = 10;
    if (force_update)
    {
      num1 = Assets.BuildingDefs.Count;
      this.buildable_state_update_idx = 0;
    }
    ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
    for (int index = 0; index < num1; ++index)
    {
      this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
      BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
      HashedString hashedString;
      if (!buildingDef.Deprecated && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString))
      {
        PlanScreen.RequirementsState requirementsState = PlanScreen.RequirementsState.Complete;
        if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
        {
          if (!Db.Get().TechItems.IsTechItemComplete(buildingDef.PrefabID))
            requirementsState = PlanScreen.RequirementsState.Tech;
          else if (!ProductInfoScreen.MaterialsMet(buildingDef.CraftRecipe))
            requirementsState = PlanScreen.RequirementsState.Materials;
        }
        if (!this.buildableDefs.ContainsKey((Def) buildingDef))
          this.buildableDefs.Add((Def) buildingDef, requirementsState);
        else if (this.buildableDefs[(Def) buildingDef] != requirementsState)
        {
          this.buildableDefs[(Def) buildingDef] = requirementsState;
          if ((UnityEngine.Object) this.productInfoScreen.currentDef == (UnityEngine.Object) buildingDef)
          {
            ++this.ignoreToolChangeMessages;
            this.productInfoScreen.ClearProduct(false);
            this.productInfoScreen.Show(true);
            this.productInfoScreen.ConfigureScreen(buildingDef);
            --this.ignoreToolChangeMessages;
          }
          if (requirementsState == PlanScreen.RequirementsState.Complete)
          {
            foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
            {
              if ((HashedString) toggleInfo.userData == hashedString)
              {
                string str = "NotificationPing";
                if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(str) && !pooledList.Contains(hashedString))
                {
                  pooledList.Add(hashedString);
                  toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(str);
                  if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 1.5)
                  {
                    if ((double) this.timeSinceNotificationPing >= (double) this.specialNotificationEmbellishDelay)
                    {
                      string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
                      if (sound != null)
                        SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition()));
                    }
                    string sound1 = GlobalAssets.GetSound("NewBuildable", false);
                    if (sound1 != null)
                    {
                      EventInstance instance = SoundEvent.BeginOneShot(sound1, SoundListenerController.Instance.transform.GetPosition());
                      int num2 = (int) instance.setParameterValue("playCount", (float) this.notificationPingCount);
                      SoundEvent.EndOneShot(instance);
                    }
                  }
                  this.timeSinceNotificationPing = 0.0f;
                  ++this.notificationPingCount;
                }
              }
            }
          }
        }
      }
    }
    pooledList.Recycle();
  }

  private void SetCategoryButtonState()
  {
    foreach (PlanScreen.ToggleEntry toggleEntry in this.toggleEntries)
    {
      KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
      toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
      bool flag1 = false;
      bool flag2 = true;
      if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
      {
        flag1 = true;
        flag2 = false;
      }
      else
      {
        foreach (BuildingDef buildingDef in toggleEntry.buildingDefs)
        {
          if (this.BuildableState(buildingDef) == PlanScreen.RequirementsState.Complete)
          {
            flag1 = true;
            flag2 = false;
            break;
          }
        }
        if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
          flag2 = false;
      }
      this.CategoryInteractive[toggleInfo] = !flag2;
      GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
      if (!flag1)
      {
        if (flag2 && toggleEntry.hideIfNotResearched)
          toggleInfo.toggle.gameObject.SetActive(false);
        else if (flag2)
        {
          toggleInfo.toggle.gameObject.SetActive(true);
          toggleInfo.toggle.fgImage.SetAlpha(0.2509804f);
          gameObject.gameObject.SetActive(true);
        }
        else
        {
          toggleInfo.toggle.gameObject.SetActive(true);
          toggleInfo.toggle.fgImage.SetAlpha(1f);
          gameObject.gameObject.SetActive(false);
        }
        ImageToggleState.State newState = this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData ? ImageToggleState.State.Disabled : ImageToggleState.State.DisabledActive;
        foreach (ImageToggleState toggleImage in toggleEntry.toggleImages)
          toggleImage.SetState(newState);
      }
      else
      {
        toggleInfo.toggle.gameObject.SetActive(true);
        toggleInfo.toggle.fgImage.SetAlpha(1f);
        gameObject.gameObject.SetActive(false);
        ImageToggleState.State newState = this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
        foreach (ImageToggleState toggleImage in toggleEntry.toggleImages)
          toggleImage.SetState(newState);
      }
    }
  }

  private void DeactivateBuildTools()
  {
    InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
    if (!((UnityEngine.Object) activeTool != (UnityEngine.Object) null))
      return;
    System.Type type = activeTool.GetType();
    if (type != typeof (BuildTool) && !typeof (BaseUtilityBuildTool).IsAssignableFrom(type))
      return;
    activeTool.DeactivateTool((InterfaceTool) null);
  }

  public void CloseRecipe(bool playSound = false)
  {
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
    if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
      ToolMenu.Instance.ClearSelection();
    this.DeactivateBuildTools();
    if ((UnityEngine.Object) this.productInfoScreen != (UnityEngine.Object) null)
      this.productInfoScreen.ClearProduct(true);
    if (this.activeCategoryInfo != null)
      this.UpdateBuildingButtonList(this.activeCategoryInfo);
    this.selectedBuildingGameObject = (GameObject) null;
  }

  private void CloseCategoryPanel(bool playSound = true)
  {
    this.activeCategoryInfo = (KIconToggleMenu.ToggleInfo) null;
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
    this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse((System.Action<object>) (s =>
    {
      this.ClearButtons();
      this.buildingGroupsRoot.gameObject.SetActive(false);
    }));
    this.PlanCategoryLabel.text = string.Empty;
  }

  private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
  {
    this.CloseRecipe(false);
    if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
    {
      this.CloseCategoryPanel(false);
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
    }
    else
    {
      if (this.activeCategoryInfo == toggle_info)
        this.CloseCategoryPanel(true);
      else
        this.OpenCategoryPanel(toggle_info, true);
      this.ConfigurePanelSize();
      this.SetScrollPoint(0.0f);
    }
  }

  private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
  {
    HashedString userData = (HashedString) toggle_info.userData;
    this.ClearButtons();
    this.buildingGroupsRoot.gameObject.SetActive(true);
    this.activeCategoryInfo = toggle_info;
    if (play_sound)
      UISounds.PlaySound(UISounds.Sound.ClickObject);
    this.BuildButtonList(userData, this.GroupsTransform.gameObject);
    this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
    this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand((System.Action<object>) null);
  }

  public void OpenCategoryByName(string category)
  {
    PlanScreen.ToggleEntry toggleEntry;
    if (!this.GetToggleEntryForCategory((HashedString) category, out toggleEntry))
      return;
    this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
  }

  private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
  {
    KToggle toggle = toggle_info.toggle;
    if ((UnityEngine.Object) toggle == (UnityEngine.Object) null)
    {
      foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
      {
        if (toggleInfo.userData == toggle_info.userData)
          toggle = toggleInfo.toggle;
      }
    }
    int num = 2;
    if ((UnityEngine.Object) toggle != (UnityEngine.Object) null && this.ActiveToggles.Count != 0)
    {
      for (int index = 0; index < num; ++index)
      {
        if (this.building_button_refresh_idx >= this.ActiveToggles.Count)
          this.building_button_refresh_idx = 0;
        this.RefreshBuildingButton(this.ActiveToggles.ElementAt<KeyValuePair<BuildingDef, KToggle>>(this.building_button_refresh_idx).Key, this.ActiveToggles.ElementAt<KeyValuePair<BuildingDef, KToggle>>(this.building_button_refresh_idx).Value, (HashedString) toggle_info.userData);
        ++this.building_button_refresh_idx;
      }
    }
    if (!this.productInfoScreen.gameObject.activeSelf)
      return;
    this.productInfoScreen.materialSelectionPanel.UpdateResourceToggleValues();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    this.GetBuildableStates(false);
    this.SetCategoryButtonState();
    if (this.activeCategoryInfo == null)
      return;
    this.UpdateBuildingButtonList(this.activeCategoryInfo);
  }

  private void BuildButtonList(HashedString plan_category, GameObject parent)
  {
    IOrderedEnumerable<BuildingDef> orderedEnumerable = Assets.BuildingDefs.Where<BuildingDef>((Func<BuildingDef, bool>) (def =>
    {
      if (this.tagCategoryMap.ContainsKey(def.Tag) && this.tagCategoryMap[def.Tag] == plan_category)
        return !def.Deprecated;
      return false;
    })).OrderBy<BuildingDef, int>((Func<BuildingDef, int>) (def => this.tagOrderMap[def.Tag]));
    this.ActiveToggles.Clear();
    int btnIndex = 0;
    string plan_category1 = plan_category.ToString();
    foreach (BuildingDef def in (IEnumerable<BuildingDef>) orderedEnumerable)
    {
      if (def.ShowInBuildMenu)
      {
        this.CreateButton(def, parent, plan_category1, btnIndex);
        ++btnIndex;
      }
    }
  }

  private void ConfigurePanelSize()
  {
    GridLayoutGroup component = this.GroupsTransform.GetComponent<GridLayoutGroup>();
    this.buildGrid_bg_rowHeight = component.cellSize.y + component.spacing.y;
    int childCount = this.GroupsTransform.childCount;
    for (int index = 0; index < this.GroupsTransform.childCount; ++index)
    {
      if (!this.GroupsTransform.GetChild(index).gameObject.activeSelf)
        --childCount;
    }
    int num = Mathf.CeilToInt((float) childCount / 3f);
    this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num >= 4);
    this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, this.buildGrid_bg_borderHeight + (float) Mathf.Clamp(num, 0, this.buildGrid_maxRowsBeforeScroll) * this.buildGrid_bg_rowHeight);
  }

  private void SetScrollPoint(float targetY)
  {
    this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
  }

  private GameObject CreateButton(
    BuildingDef def,
    GameObject parent,
    string plan_category,
    int btnIndex)
  {
    GameObject button_go = Util.KInstantiateUI(this.planButtonPrefab, parent, true);
    button_go.name = STRINGS.UI.StripLinkFormatting(def.name) + " Group:" + plan_category;
    KToggle componentInChildren = button_go.GetComponentInChildren<KToggle>();
    componentInChildren.soundPlayer.Enabled = false;
    this.ActiveToggles.Add(def, componentInChildren);
    this.RefreshBuildingButton(def, componentInChildren, (HashedString) plan_category);
    componentInChildren.onClick += (System.Action) (() => this.OnSelectBuilding(button_go, def));
    return button_go;
  }

  private static bool TechRequirementsMet(TechItem techItem)
  {
    if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && techItem != null)
      return techItem.IsComplete();
    return true;
  }

  private static bool TechRequirementsUpcoming(TechItem techItem)
  {
    return PlanScreen.TechRequirementsMet(techItem);
  }

  private bool GetToggleEntryForCategory(
    HashedString category,
    out PlanScreen.ToggleEntry toggleEntry)
  {
    foreach (PlanScreen.ToggleEntry toggleEntry1 in this.toggleEntries)
    {
      if (toggleEntry1.planCategory == category)
      {
        toggleEntry = toggleEntry1;
        return true;
      }
    }
    toggleEntry = new PlanScreen.ToggleEntry();
    return false;
  }

  public void RefreshBuildingButton(BuildingDef def, KToggle toggle, HashedString buildingCategory)
  {
    if ((UnityEngine.Object) toggle == (UnityEngine.Object) null)
      return;
    PlanScreen.ToggleEntry toggleEntry;
    if (this.GetToggleEntryForCategory(buildingCategory, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
      toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
    TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
    bool flag1 = PlanScreen.TechRequirementsMet(techItem);
    bool flag2 = PlanScreen.TechRequirementsUpcoming(techItem);
    if (toggle.gameObject.activeSelf != flag2)
    {
      toggle.gameObject.SetActive(flag2);
      this.ConfigurePanelSize();
      this.SetScrollPoint(0.0f);
    }
    if (!toggle.gameObject.activeInHierarchy || (UnityEngine.Object) toggle.bgImage == (UnityEngine.Object) null)
      return;
    Image componentsInChild = toggle.bgImage.GetComponentsInChildren<Image>()[1];
    Sprite sprite = def.GetUISprite("ui", false);
    if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
      sprite = this.defaultBuildingIconSprite;
    componentsInChild.sprite = sprite;
    componentsInChild.SetNativeSize();
    componentsInChild.rectTransform().sizeDelta /= 4f;
    ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
    this.PositionTooltip(toggle, component);
    component.ClearMultiStringTooltip();
    string name = def.Name;
    string effect = def.Effect;
    component.AddMultiStringTooltip(name, (ScriptableObject) this.buildingToolTipSettings.BuildButtonName);
    component.AddMultiStringTooltip(effect, (ScriptableObject) this.buildingToolTipSettings.BuildButtonDescription);
    LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.text = def.Name;
    ImageToggleState.State state = this.BuildableState(def) != PlanScreen.RequirementsState.Complete ? ImageToggleState.State.Disabled : ImageToggleState.State.Inactive;
    ImageToggleState.State newState = !((UnityEngine.Object) toggle.gameObject == (UnityEngine.Object) this.selectedBuildingGameObject) || this.BuildableState(def) != PlanScreen.RequirementsState.Complete && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive ? (this.BuildableState(def) == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled) : ImageToggleState.State.Active;
    if ((UnityEngine.Object) toggle.gameObject == (UnityEngine.Object) this.selectedBuildingGameObject && newState == ImageToggleState.State.Disabled)
      newState = ImageToggleState.State.DisabledActive;
    else if (newState == ImageToggleState.State.Disabled)
      newState = ImageToggleState.State.Disabled;
    toggle.GetComponent<ImageToggleState>().SetState(newState);
    Material material = this.BuildableState(def) == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive ? this.defaultUIMaterial : this.desaturatedUIMaterial;
    if ((UnityEngine.Object) componentsInChild.material != (UnityEngine.Object) material)
    {
      componentsInChild.material = material;
      if ((UnityEngine.Object) material == (UnityEngine.Object) this.desaturatedUIMaterial)
      {
        if (flag1)
          componentsInChild.color = new Color(1f, 1f, 1f, 0.6f);
        else
          componentsInChild.color = new Color(1f, 1f, 1f, 0.15f);
      }
      else
        componentsInChild.color = Color.white;
    }
    Image fgImage = toggle.gameObject.GetComponent<KToggle>().fgImage;
    fgImage.gameObject.SetActive(false);
    if (!flag1)
    {
      fgImage.sprite = this.Overlay_NeedTech;
      fgImage.gameObject.SetActive(true);
      string newString = string.Format((string) STRINGS.UI.PRODUCTINFO_REQUIRESRESEARCHDESC, (object) techItem.parentTech.Name);
      component.AddMultiStringTooltip("\n", (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
      component.AddMultiStringTooltip(newString, (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
    }
    else
    {
      if (this.BuildableState(def) == PlanScreen.RequirementsState.Complete)
        return;
      fgImage.gameObject.SetActive(false);
      component.AddMultiStringTooltip("\n", (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
      string missingresourcesHover = (string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
      component.AddMultiStringTooltip(missingresourcesHover, (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
      foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
      {
        string newString = string.Format("{0}{1}: {2}", (object) "• ", (object) ingredient.tag.ProperName(), (object) GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        component.AddMultiStringTooltip(newString, (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
      }
      component.AddMultiStringTooltip(string.Empty, (ScriptableObject) this.buildingToolTipSettings.ResearchRequirement);
    }
  }

  private void PositionTooltip(KToggle toggle, ToolTip tip)
  {
    tip.overrideParentObject = !this.productInfoScreen.gameObject.activeSelf ? this.buildingGroupsRoot : this.productInfoScreen.rectTransform();
  }

  private void SetMaterialTint(KToggle toggle, bool disabled)
  {
    SwapUIAnimationController component = toggle.GetComponent<SwapUIAnimationController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetState(!disabled);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (!this.mouseOver || !this.ConsumeMouseScroll || (e.TryConsume(Action.ZoomIn) || !e.TryConsume(Action.ZoomOut)))
      ;
    if (e.IsAction(Action.CopyBuilding) && e.TryConsume(Action.CopyBuilding))
      this.OnClickCopyBuilding();
    if (this.toggles == null)
      return;
    if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(Action.Escape))
    {
      this.OnClickCategory(this.activeCategoryInfo);
      SelectTool.Instance.Activate();
      this.ClearSelection();
    }
    else
    {
      if (e.Consumed)
        return;
      base.OnKeyDown(e);
    }
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if ((UnityEngine.Object) this.selectedBuildingGameObject != (UnityEngine.Object) null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
    {
      this.CloseRecipe(false);
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
    }
    else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.OnUIClear((object) null);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void OnRecipeElementsFullySelected()
  {
    BuildingDef def = (BuildingDef) null;
    foreach (KeyValuePair<BuildingDef, KToggle> activeToggle in this.ActiveToggles)
    {
      if ((UnityEngine.Object) activeToggle.Value == (UnityEngine.Object) this.currentlySelectedToggle)
      {
        def = activeToggle.Key;
        break;
      }
    }
    DebugUtil.DevAssert((bool) ((UnityEngine.Object) def), "def is null");
    if (!(bool) ((UnityEngine.Object) def))
      return;
    if (def.isKAnimTile && def.isUtility)
    {
      IList<Tag> selectedElementAsList = this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
      (!((UnityEngine.Object) def.BuildingComplete.GetComponent<Wire>() != (UnityEngine.Object) null) ? (BaseUtilityBuildTool) UtilityBuildTool.Instance : (BaseUtilityBuildTool) WireBuildTool.Instance).Activate(def, selectedElementAsList);
    }
    else
      BuildTool.Instance.Activate(def, this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList, (GameObject) null);
  }

  public void OnResearchComplete(object tech)
  {
    foreach (Resource unlockedItem in ((Tech) tech).unlockedItems)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(unlockedItem.Id);
      PlanScreen.ToggleEntry toggleEntry;
      if ((UnityEngine.Object) buildingDef != (UnityEngine.Object) null && this.GetToggleEntryForCategory(this.tagCategoryMap[buildingDef.Tag], out toggleEntry))
      {
        toggleEntry.pendingResearchAttentions.Add(buildingDef.Tag);
        toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
      }
    }
  }

  private void OnUIClear(object data)
  {
    if (this.activeCategoryInfo == null)
      return;
    this.selected = -1;
    this.OnClickCategory(this.activeCategoryInfo);
    SelectTool.Instance.Activate();
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  private void OnActiveToolChanged(object data)
  {
    if (data == null || this.ignoreToolChangeMessages > 0)
      return;
    System.Type type = data.GetType();
    if (typeof (BuildTool).IsAssignableFrom(type) || typeof (PrebuildTool).IsAssignableFrom(type) || typeof (BaseUtilityBuildTool).IsAssignableFrom(type))
      return;
    this.CloseRecipe(false);
    this.CloseCategoryPanel(false);
  }

  public PrioritySetting GetBuildingPriority()
  {
    return this.productInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
  }

  public struct PlanInfo
  {
    public HashedString category;
    public bool hideIfNotResearched;
    public object data;

    public PlanInfo(HashedString category, bool hideIfNotResearched, object data)
    {
      this.category = category;
      this.hideIfNotResearched = hideIfNotResearched;
      this.data = data;
    }
  }

  [Serializable]
  public struct BuildingToolTipSettings
  {
    public TextStyleSetting BuildButtonName;
    public TextStyleSetting BuildButtonDescription;
    public TextStyleSetting MaterialRequirement;
    public TextStyleSetting ResearchRequirement;
  }

  [Serializable]
  public struct BuildingNameTextSetting
  {
    public TextStyleSetting ActiveSelected;
    public TextStyleSetting ActiveDeselected;
    public TextStyleSetting InactiveSelected;
    public TextStyleSetting InactiveDeselected;
  }

  private struct ToggleEntry
  {
    public KIconToggleMenu.ToggleInfo toggleInfo;
    public HashedString planCategory;
    public List<BuildingDef> buildingDefs;
    public List<Tag> pendingResearchAttentions;
    private List<TechItem> requiredTechItems;
    public ImageToggleState[] toggleImages;
    public bool hideIfNotResearched;

    public ToggleEntry(
      KIconToggleMenu.ToggleInfo toggle_info,
      HashedString plan_category,
      List<BuildingDef> building_defs,
      bool hideIfNotResearched)
    {
      this.toggleInfo = toggle_info;
      this.planCategory = plan_category;
      this.buildingDefs = building_defs;
      this.hideIfNotResearched = hideIfNotResearched;
      this.pendingResearchAttentions = new List<Tag>();
      this.requiredTechItems = new List<TechItem>();
      this.toggleImages = (ImageToggleState[]) null;
      foreach (Def buildingDef in building_defs)
      {
        TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
        if (techItem == null)
        {
          this.requiredTechItems.Clear();
          break;
        }
        if (!this.requiredTechItems.Contains(techItem))
          this.requiredTechItems.Add(techItem);
      }
    }

    public bool AreAnyRequiredTechItemsAvailable()
    {
      if (this.requiredTechItems.Count == 0)
        return true;
      foreach (TechItem requiredTechItem in this.requiredTechItems)
      {
        if (PlanScreen.TechRequirementsUpcoming(requiredTechItem))
          return true;
      }
      return false;
    }

    public void CollectToggleImages()
    {
      this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
    }
  }

  public enum RequirementsState
  {
    Tech,
    Materials,
    Complete,
  }
}
