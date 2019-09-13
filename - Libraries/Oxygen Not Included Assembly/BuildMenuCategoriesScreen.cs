// Decompiled with JetBrains decompiler
// Type: BuildMenuCategoriesScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuCategoriesScreen : KIconToggleMenu
{
  private HashedString selectedCategory = HashedString.Invalid;
  public System.Action<HashedString, int> onCategoryClicked;
  [SerializeField]
  public bool modalKeyInputBehaviour;
  [SerializeField]
  private Image focusIndicator;
  [SerializeField]
  private Color32 focusedColour;
  [SerializeField]
  private Color32 unfocusedColour;
  private IList<HashedString> subcategories;
  private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;
  private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;
  private BuildMenuBuildingsScreen buildingsScreen;
  private HashedString category;
  private IList<BuildMenu.BuildingInfo> buildingInfos;

  public override float GetSortKey()
  {
    return 7f;
  }

  public HashedString Category
  {
    get
    {
      return this.category;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.onSelect += new KIconToggleMenu.OnSelect(this.OnClickCategory);
  }

  public void Configure(
    HashedString category,
    int depth,
    object data,
    Dictionary<HashedString, List<BuildingDef>> categorized_building_map,
    Dictionary<HashedString, List<HashedString>> categorized_category_map,
    BuildMenuBuildingsScreen buildings_screen)
  {
    this.category = category;
    this.categorizedBuildingMap = categorized_building_map;
    this.categorizedCategoryMap = categorized_category_map;
    this.buildingsScreen = buildings_screen;
    List<KIconToggleMenu.ToggleInfo> toggleInfoList = new List<KIconToggleMenu.ToggleInfo>();
    if (typeof (IList<BuildMenu.BuildingInfo>).IsAssignableFrom(data.GetType()))
      this.buildingInfos = (IList<BuildMenu.BuildingInfo>) data;
    else if (typeof (IList<BuildMenu.DisplayInfo>).IsAssignableFrom(data.GetType()))
    {
      this.subcategories = (IList<HashedString>) new List<HashedString>();
      foreach (BuildMenu.DisplayInfo displayInfo in (IEnumerable<BuildMenu.DisplayInfo>) data)
      {
        string iconName = displayInfo.iconName;
        string str = HashCache.Get().Get(displayInfo.category).ToUpper().Replace(" ", string.Empty);
        KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo((string) Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + str + ".NAME"), iconName, (object) new BuildMenuCategoriesScreen.UserData()
        {
          category = displayInfo.category,
          depth = depth,
          requirementsState = PlanScreen.RequirementsState.Tech
        }, displayInfo.hotkey, (string) Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + str + ".TOOLTIP"), string.Empty);
        toggleInfoList.Add(toggleInfo);
        this.subcategories.Add(displayInfo.category);
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
    }
    this.UpdateBuildableStates(true);
  }

  private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
  {
    BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData) toggle_info.userData;
    switch (userData.requirementsState)
    {
      case PlanScreen.RequirementsState.Materials:
      case PlanScreen.RequirementsState.Complete:
        if (this.selectedCategory != userData.category)
        {
          this.selectedCategory = userData.category;
          KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
          break;
        }
        this.selectedCategory = HashedString.Invalid;
        this.ClearSelection();
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
        break;
      default:
        this.selectedCategory = HashedString.Invalid;
        this.ClearSelection();
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
        break;
    }
    toggle_info.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
    if (this.onCategoryClicked == null)
      return;
    this.onCategoryClicked(this.selectedCategory, userData.depth);
  }

  private void UpdateButtonStates()
  {
    if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
      return;
    foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
    {
      BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData) toggleInfo.userData;
      HashedString category = userData.category;
      PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(category);
      bool flag = categoryRequirements == PlanScreen.RequirementsState.Tech;
      toggleInfo.toggle.gameObject.SetActive(!flag);
      switch (categoryRequirements)
      {
        case PlanScreen.RequirementsState.Materials:
          toggleInfo.toggle.fgImage.SetAlpha(!flag ? 1f : 0.2509804f);
          ImageToggleState.State state1 = !this.selectedCategory.IsValid || !(category == this.selectedCategory) ? ImageToggleState.State.Disabled : ImageToggleState.State.DisabledActive;
          if (!userData.currentToggleState.HasValue || userData.currentToggleState.GetValueOrDefault() != state1)
          {
            userData.currentToggleState = new ImageToggleState.State?(state1);
            this.SetImageToggleState(toggleInfo.toggle.gameObject, state1);
            break;
          }
          break;
        case PlanScreen.RequirementsState.Complete:
          ImageToggleState.State state2 = !this.selectedCategory.IsValid || category != this.selectedCategory ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
          if (!userData.currentToggleState.HasValue || userData.currentToggleState.GetValueOrDefault() != state2)
          {
            userData.currentToggleState = new ImageToggleState.State?(state2);
            this.SetImageToggleState(toggleInfo.toggle.gameObject, state2);
            break;
          }
          break;
      }
      toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject.gameObject.SetActive(flag);
    }
  }

  private void SetImageToggleState(GameObject target, ImageToggleState.State state)
  {
    foreach (ImageToggleState component in target.GetComponents<ImageToggleState>())
      component.SetState(state);
  }

  private PlanScreen.RequirementsState GetCategoryRequirements(HashedString category)
  {
    bool flag1 = true;
    bool flag2 = true;
    List<BuildingDef> buildingDefList;
    if (this.categorizedBuildingMap.TryGetValue(category, out buildingDefList))
    {
      if (buildingDefList.Count > 0)
      {
        foreach (BuildingDef def in buildingDefList)
        {
          if (def.ShowInBuildMenu && !def.Deprecated)
          {
            PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
            flag1 = flag1 && requirementsState == PlanScreen.RequirementsState.Tech;
            flag2 = flag2 && (requirementsState == PlanScreen.RequirementsState.Materials || requirementsState == PlanScreen.RequirementsState.Tech);
          }
        }
      }
    }
    else
    {
      List<HashedString> hashedStringList;
      if (this.categorizedCategoryMap.TryGetValue(category, out hashedStringList))
      {
        foreach (HashedString category1 in hashedStringList)
        {
          PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(category1);
          flag1 = flag1 && categoryRequirements == PlanScreen.RequirementsState.Tech;
          flag2 = flag2 && (categoryRequirements == PlanScreen.RequirementsState.Materials || categoryRequirements == PlanScreen.RequirementsState.Tech);
        }
      }
    }
    PlanScreen.RequirementsState requirementsState1 = !flag1 ? (!flag2 ? PlanScreen.RequirementsState.Complete : PlanScreen.RequirementsState.Materials) : PlanScreen.RequirementsState.Tech;
    if (DebugHandler.InstantBuildMode)
      requirementsState1 = PlanScreen.RequirementsState.Complete;
    return requirementsState1;
  }

  public void UpdateNotifications(ICollection<HashedString> updated_categories)
  {
    if (this.toggleInfo == null)
      return;
    this.UpdateBuildableStates(false);
    foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
    {
      HashedString category = ((BuildMenuCategoriesScreen.UserData) toggleInfo.userData).category;
      if (updated_categories.Contains(category))
        toggleInfo.toggle.gameObject.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
    }
  }

  public override void Close()
  {
    base.Close();
    this.selectedCategory = HashedString.Invalid;
    this.SetHasFocus(false);
    if (this.buildingInfos == null)
      return;
    this.buildingsScreen.Close();
  }

  [ContextMenu("ForceUpdateBuildableStates")]
  private void ForceUpdateBuildableStates()
  {
    this.UpdateBuildableStates(true);
  }

  public void UpdateBuildableStates(bool skip_flourish)
  {
    if (this.subcategories != null && this.subcategories.Count > 0)
    {
      this.UpdateButtonStates();
      foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
      {
        BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData) toggleInfo.userData;
        PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(userData.category);
        if (userData.requirementsState != categoryRequirements)
        {
          userData.requirementsState = categoryRequirements;
          toggleInfo.userData = (object) userData;
          if (!skip_flourish)
          {
            toggleInfo.toggle.ActivateFlourish(false);
            string str = "NotificationPing";
            if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(str))
            {
              toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(str);
              BuildMenu.Instance.PlayNewBuildingSounds();
            }
          }
        }
      }
    }
    else
      this.buildingsScreen.UpdateBuildableStates();
  }

  protected override void OnShow(bool show)
  {
    if (this.buildingInfos != null)
    {
      if (show)
      {
        this.buildingsScreen.Configure(this.category, this.buildingInfos);
        this.buildingsScreen.Show(true);
      }
      else
        this.buildingsScreen.Close();
    }
    base.OnShow(show);
  }

  public override void ClearSelection()
  {
    this.selectedCategory = HashedString.Invalid;
    base.ClearSelection();
    foreach (KToggle toggle in this.toggles)
      toggle.isOn = false;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.modalKeyInputBehaviour)
    {
      if (!this.HasFocus)
        return;
      if (e.TryConsume(Action.Escape))
      {
        Game.Instance.Trigger(288942073, (object) null);
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
    else
    {
      base.OnKeyDown(e);
      if (!e.Consumed)
        return;
      this.UpdateButtonStates();
    }
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.modalKeyInputBehaviour)
    {
      if (!this.HasFocus)
        return;
      if (e.TryConsume(Action.Escape))
      {
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
    else
      base.OnKeyUp(e);
  }

  public override void SetHasFocus(bool has_focus)
  {
    base.SetHasFocus(has_focus);
    if (!((UnityEngine.Object) this.focusIndicator != (UnityEngine.Object) null))
      return;
    this.focusIndicator.color = (Color) (!has_focus ? this.unfocusedColour : this.focusedColour);
  }

  private class UserData
  {
    public HashedString category;
    public int depth;
    public PlanScreen.RequirementsState requirementsState;
    public ImageToggleState.State? currentToggleState;
  }
}
