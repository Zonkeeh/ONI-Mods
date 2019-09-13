// Decompiled with JetBrains decompiler
// Type: BuildMenuBuildingsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuBuildingsScreen : KIconToggleMenu
{
  [SerializeField]
  private Image focusIndicator;
  [SerializeField]
  private Color32 focusedColour;
  [SerializeField]
  private Color32 unfocusedColour;
  public System.Action<BuildingDef> onBuildingSelected;
  [SerializeField]
  private LocText titleLabel;
  [SerializeField]
  private BuildMenuBuildingsScreen.BuildingToolTipSettings buildingToolTipSettings;
  [SerializeField]
  private LayoutElement contentSizeLayout;
  [SerializeField]
  private GridLayoutGroup gridSizer;
  [SerializeField]
  private Sprite Overlay_NeedTech;
  [SerializeField]
  private Material defaultUIMaterial;
  [SerializeField]
  private Material desaturatedUIMaterial;
  private BuildingDef selectedBuilding;

  public override float GetSortKey()
  {
    return 8f;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateBuildableStates();
    Game.Instance.Subscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
    this.onSelect += new KIconToggleMenu.OnSelect(this.OnClickBuilding);
    Game.Instance.Subscribe(-1190690038, new System.Action<object>(this.OnBuildToolDeactivated));
  }

  public void Configure(HashedString category, IList<BuildMenu.BuildingInfo> building_infos)
  {
    this.ClearButtons();
    this.SetHasFocus(true);
    List<KIconToggleMenu.ToggleInfo> toggleInfoList = new List<KIconToggleMenu.ToggleInfo>();
    this.titleLabel.text = (string) Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + HashCache.Get().Get(category).ToUpper().Replace(" ", string.Empty) + ".BUILDMENUTITLE");
    foreach (BuildMenu.BuildingInfo buildingInfo in (IEnumerable<BuildMenu.BuildingInfo>) building_infos)
    {
      BuildingDef def = Assets.GetBuildingDef(buildingInfo.id);
      if (def.ShowInBuildMenu && !def.Deprecated)
      {
        KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(def.Name, (object) new BuildMenuBuildingsScreen.UserData(def, PlanScreen.RequirementsState.Tech), def.HotKey, (Func<Sprite>) (() => def.GetUISprite("ui", false)));
        toggleInfoList.Add(toggleInfo);
      }
    }
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) toggleInfoList);
    for (int index = 0; index < this.toggleInfo.Count; ++index)
      this.RefreshToggle(this.toggleInfo[index]);
    int a = 0;
    IEnumerator enumerator = this.gridSizer.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        if (((Component) enumerator.Current).gameObject.activeSelf)
          ++a;
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.gridSizer.constraintCount = Mathf.Min(a, 3);
    int num1 = Mathf.Min(a, this.gridSizer.constraintCount);
    int num2 = (a + this.gridSizer.constraintCount - 1) / this.gridSizer.constraintCount;
    int num3 = num1 - 1;
    int num4 = num2 - 1;
    Vector2 vector2 = new Vector2((float) ((double) num1 * (double) this.gridSizer.cellSize.x + (double) num3 * (double) this.gridSizer.spacing.x) + (float) this.gridSizer.padding.left + (float) this.gridSizer.padding.right, (float) ((double) num2 * (double) this.gridSizer.cellSize.y + (double) num4 * (double) this.gridSizer.spacing.y) + (float) this.gridSizer.padding.top + (float) this.gridSizer.padding.bottom);
    this.contentSizeLayout.minWidth = vector2.x;
    this.contentSizeLayout.minHeight = vector2.y;
  }

  private void ConfigureToolTip(ToolTip tooltip, BuildingDef def)
  {
    tooltip.ClearMultiStringTooltip();
    tooltip.AddMultiStringTooltip(def.Name, (ScriptableObject) this.buildingToolTipSettings.BuildButtonName);
    tooltip.AddMultiStringTooltip(def.Effect, (ScriptableObject) this.buildingToolTipSettings.BuildButtonDescription);
  }

  public void CloseRecipe(bool playSound = false)
  {
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
    ToolMenu.Instance.ClearSelection();
    this.DeactivateBuildTools();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) PrebuildTool.Instance)
      SelectTool.Instance.Activate();
    this.selectedBuilding = (BuildingDef) null;
    this.onBuildingSelected(this.selectedBuilding);
  }

  private void RefreshToggle(KIconToggleMenu.ToggleInfo info)
  {
    if (info == null || (UnityEngine.Object) info.toggle == (UnityEngine.Object) null)
      return;
    BuildingDef def = (info.userData as BuildMenuBuildingsScreen.UserData).def;
    TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
    bool flag1 = DebugHandler.InstantBuildMode || techItem == null || techItem.IsComplete();
    bool flag2 = flag1 || techItem == null || techItem.parentTech.ArePrerequisitesComplete();
    KToggle toggle = info.toggle;
    if (toggle.gameObject.activeSelf != flag2)
      toggle.gameObject.SetActive(flag2);
    if ((UnityEngine.Object) toggle.bgImage == (UnityEngine.Object) null)
      return;
    Image componentsInChild = toggle.bgImage.GetComponentsInChildren<Image>()[1];
    Sprite uiSprite = def.GetUISprite("ui", false);
    componentsInChild.sprite = uiSprite;
    componentsInChild.SetNativeSize();
    componentsInChild.rectTransform().sizeDelta /= 4f;
    ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    string str = def.Name;
    string effect = def.Effect;
    if (def.HotKey != Action.NumActions)
      str = GameUtil.AppendHotkeyString(str, def.HotKey);
    component.AddMultiStringTooltip(str, (ScriptableObject) this.buildingToolTipSettings.BuildButtonName);
    component.AddMultiStringTooltip(effect, (ScriptableObject) this.buildingToolTipSettings.BuildButtonDescription);
    LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.text = def.Name;
    PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
    ImageToggleState.State state = requirementsState != PlanScreen.RequirementsState.Complete ? ImageToggleState.State.Disabled : ImageToggleState.State.Inactive;
    ImageToggleState.State newState = !((UnityEngine.Object) def == (UnityEngine.Object) this.selectedBuilding) || requirementsState != PlanScreen.RequirementsState.Complete && !DebugHandler.InstantBuildMode ? (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled) : ImageToggleState.State.Active;
    if ((UnityEngine.Object) def == (UnityEngine.Object) this.selectedBuilding && newState == ImageToggleState.State.Disabled)
      newState = ImageToggleState.State.DisabledActive;
    else if (newState == ImageToggleState.State.Disabled)
      newState = ImageToggleState.State.Disabled;
    toggle.GetComponent<ImageToggleState>().SetState(newState);
    Material material;
    Color color1;
    if (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode)
    {
      material = this.defaultUIMaterial;
      color1 = Color.white;
    }
    else
    {
      material = this.desaturatedUIMaterial;
      Color color2;
      if (flag1)
      {
        color2 = new Color(1f, 1f, 1f, 0.6f);
      }
      else
      {
        Color color3 = new Color(1f, 1f, 1f, 0.15f);
        componentsInChild.color = color3;
        color2 = color3;
      }
      color1 = color2;
    }
    if ((UnityEngine.Object) componentsInChild.material != (UnityEngine.Object) material)
    {
      componentsInChild.material = material;
      componentsInChild.color = color1;
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
      if (requirementsState == PlanScreen.RequirementsState.Complete)
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

  public void ClearUI()
  {
    this.Show(false);
    this.ClearButtons();
  }

  private void ClearButtons()
  {
    foreach (KToggle toggle in this.toggles)
    {
      toggle.gameObject.SetActive(false);
      toggle.gameObject.transform.SetParent((Transform) null);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) toggle.gameObject);
    }
    if (this.toggles != null)
      this.toggles.Clear();
    if (this.toggleInfo == null)
      return;
    this.toggleInfo.Clear();
  }

  private void OnClickBuilding(KIconToggleMenu.ToggleInfo toggle_info)
  {
    this.OnSelectBuilding((toggle_info.userData as BuildMenuBuildingsScreen.UserData).def);
  }

  private void OnSelectBuilding(BuildingDef def)
  {
    switch (BuildMenu.Instance.BuildableState(def))
    {
      case PlanScreen.RequirementsState.Materials:
      case PlanScreen.RequirementsState.Complete:
        if ((UnityEngine.Object) def != (UnityEngine.Object) this.selectedBuilding)
        {
          this.selectedBuilding = def;
          KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
          break;
        }
        this.selectedBuilding = (BuildingDef) null;
        this.ClearSelection();
        this.CloseRecipe(true);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
        break;
      default:
        this.selectedBuilding = (BuildingDef) null;
        this.ClearSelection();
        this.CloseRecipe(true);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
        break;
    }
    this.onBuildingSelected(this.selectedBuilding);
  }

  public void UpdateBuildableStates()
  {
    if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
      return;
    BuildingDef def1 = (BuildingDef) null;
    foreach (KIconToggleMenu.ToggleInfo info in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
    {
      this.RefreshToggle(info);
      BuildMenuBuildingsScreen.UserData userData = info.userData as BuildMenuBuildingsScreen.UserData;
      BuildingDef def2 = userData.def;
      if (!def2.Deprecated)
      {
        PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def2);
        if (requirementsState != userData.requirementsState)
        {
          if ((UnityEngine.Object) def2 == (UnityEngine.Object) BuildMenu.Instance.SelectedBuildingDef)
            def1 = def2;
          this.RefreshToggle(info);
          userData.requirementsState = requirementsState;
        }
      }
    }
    if (!((UnityEngine.Object) def1 != (UnityEngine.Object) null))
      return;
    BuildMenu.Instance.RefreshProductInfoScreen(def1);
  }

  private void OnResearchComplete(object data)
  {
    this.UpdateBuildableStates();
  }

  private void DeactivateBuildTools()
  {
    InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
    if (!((UnityEngine.Object) activeTool != (UnityEngine.Object) null))
      return;
    System.Type type = activeTool.GetType();
    if (type != typeof (BuildTool) && !typeof (BaseUtilityBuildTool).IsAssignableFrom(type) && !typeof (PrebuildTool).IsAssignableFrom(type))
      return;
    activeTool.DeactivateTool((InterfaceTool) null);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!this.mouseOver || !this.ConsumeMouseScroll || (e.TryConsume(Action.ZoomIn) || !e.TryConsume(Action.ZoomOut)))
      ;
    if (!this.HasFocus)
      return;
    if (e.TryConsume(Action.Escape))
    {
      Game.Instance.Trigger(288942073, (object) null);
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
    }
    else
    {
      base.OnKeyDown(e);
      if (e.Consumed)
        return;
      Action action = e.GetAction();
      if (action < Action.BUILD_MENU_START_INTERCEPT)
        return;
      e.TryConsume(action);
    }
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!this.HasFocus)
      return;
    if ((UnityEngine.Object) this.selectedBuilding != (UnityEngine.Object) null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
      Game.Instance.Trigger(288942073, (object) null);
    }
    else
    {
      base.OnKeyUp(e);
      if (e.Consumed)
        return;
      Action action = e.GetAction();
      if (action < Action.BUILD_MENU_START_INTERCEPT)
        return;
      e.TryConsume(action);
    }
  }

  public override void Close()
  {
    ToolMenu.Instance.ClearSelection();
    this.DeactivateBuildTools();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) PrebuildTool.Instance)
      SelectTool.Instance.Activate();
    this.selectedBuilding = (BuildingDef) null;
    this.ClearButtons();
    this.gameObject.SetActive(false);
  }

  public override void SetHasFocus(bool has_focus)
  {
    base.SetHasFocus(has_focus);
    if (!((UnityEngine.Object) this.focusIndicator != (UnityEngine.Object) null))
      return;
    this.focusIndicator.color = (Color) (!has_focus ? this.unfocusedColour : this.focusedColour);
  }

  private void OnBuildToolDeactivated(object data)
  {
    this.CloseRecipe(false);
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

  private class UserData
  {
    public BuildingDef def;
    public PlanScreen.RequirementsState requirementsState;

    public UserData(BuildingDef def, PlanScreen.RequirementsState state)
    {
      this.def = def;
      this.requirementsState = state;
    }
  }
}
