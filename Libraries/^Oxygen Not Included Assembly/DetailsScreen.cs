// Decompiled with JetBrains decompiler
// Type: DetailsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetailsScreen : KTabMenu
{
  private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>((System.Action<DetailsScreen, object>) ((component, data) => component.OnRefreshData(data)));
  public static DetailsScreen Instance;
  [SerializeField]
  private KButton CodexEntryButton;
  [Header("Panels")]
  public Transform UserMenuPanel;
  [Header("Name Editing (disabled)")]
  [SerializeField]
  private KButton CloseButton;
  [Header("Tabs")]
  [SerializeField]
  private EditableTitleBar TabTitle;
  [SerializeField]
  private DetailsScreen.Screens[] screens;
  [SerializeField]
  private GameObject tabHeaderContainer;
  [Header("Side Screens")]
  [SerializeField]
  private GameObject sideScreenContentBody;
  [SerializeField]
  private GameObject sideScreen;
  [SerializeField]
  private LocText sideScreenTitle;
  [SerializeField]
  private List<DetailsScreen.SideScreenRef> sideScreens;
  [Header("Secondary Side Screens")]
  [SerializeField]
  private GameObject sideScreen2ContentBody;
  [SerializeField]
  private GameObject sideScreen2;
  [SerializeField]
  private LocText sideScreen2Title;
  private KScreen activeSideScreen2;
  private bool HasActivated;
  private bool isEditing;
  private SideScreenContent currentSideScreen;

  public static void DestroyInstance()
  {
    DetailsScreen.Instance = (DetailsScreen) null;
  }

  public GameObject target { get; private set; }

  public override float GetSortKey()
  {
    if (this.isEditing)
      return 10f;
    return base.GetSortKey();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SortScreenOrder();
    this.ConsumeMouseScroll = true;
    Debug.Assert((UnityEngine.Object) DetailsScreen.Instance == (UnityEngine.Object) null);
    DetailsScreen.Instance = this;
    UIRegistry.detailsScreen = this;
    this.DeactivateSideContent();
    this.Show(false);
    this.Subscribe(Game.Instance.gameObject, -1503271301, new System.Action<object>(this.OnSelectObject));
  }

  private void OnSelectObject(object data)
  {
    if (data != null)
      return;
    this.previouslyActiveTab = -1;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.CodexEntryButton.onClick += new System.Action(this.OpenCodexEntry);
    this.CloseButton.onClick += new System.Action(this.DeselectAndClose);
    this.TabTitle.OnNameChanged += new System.Action<string>(this.OnNameChanged);
    this.TabTitle.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this.sideScreen2.SetActive(false);
    this.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
  }

  private void OnStartedEditing()
  {
    this.isEditing = true;
    KScreenManager.Instance.RefreshStack();
  }

  private void OnNameChanged(string newName)
  {
    this.isEditing = false;
    if (string.IsNullOrEmpty(newName))
      return;
    MinionIdentity component1 = this.target.GetComponent<MinionIdentity>();
    StorageLocker component2 = this.target.GetComponent<StorageLocker>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.SetName(newName);
    }
    else
    {
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.SetName(newName);
    }
  }

  protected override void OnDeactivate()
  {
    this.DeactivateSideContent();
    base.OnDeactivate();
  }

  protected override void OnShow(bool show)
  {
    if (!show)
    {
      this.DeactivateSideContent();
    }
    else
    {
      this.MaskSideContent(false);
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
    }
    base.OnShow(show);
  }

  protected override void OnCmpDisable()
  {
    this.DeactivateSideContent();
    base.OnCmpDisable();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.isEditing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.isEditing || !((UnityEngine.Object) this.target != (UnityEngine.Object) null) || !PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      return;
    this.DeselectAndClose();
  }

  private static Component GetComponent(GameObject go, string name)
  {
    System.Type type = System.Type.GetType(name);
    return type == null ? go.GetComponent(name) : go.GetComponent(type);
  }

  private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
  {
    if (excluded_tags == null || excluded_tags.Length == 0)
      return false;
    bool flag = false;
    KPrefabID component = go.GetComponent<KPrefabID>();
    for (int index = 0; index < excluded_tags.Length; ++index)
    {
      Tag excludedTag = excluded_tags[index];
      if (component.PrefabTag == excludedTag)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private void UpdateCodexButton()
  {
    this.CodexEntryButton.isInteractable = this.GetSelectedObjectCodexID() != string.Empty;
    this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (!this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.NO_CODEX_ENTRY : UI.TOOLTIPS.OPEN_CODEX_ENTRY));
  }

  public void OnRefreshData(object obj)
  {
    this.SetTitle(this.PreviousActiveTab);
    for (int index = 0; index < this.tabs.Count; ++index)
    {
      if (this.tabs[index].gameObject.activeInHierarchy)
        this.tabs[index].Trigger(-1514841199, obj);
    }
  }

  public void Refresh(GameObject go)
  {
    if (this.screens == null)
      return;
    this.target = go;
    CellSelectionObject component = this.target.GetComponent<CellSelectionObject>();
    if ((bool) ((UnityEngine.Object) component))
      component.OnObjectSelected((object) null);
    if (!this.HasActivated)
    {
      if (this.screens != null)
      {
        for (int index = 0; index < this.screens.Length; ++index)
        {
          GameObject gameObject = KScreenManager.Instance.InstantiateScreen(this.screens[index].screen.gameObject, this.body.gameObject).gameObject;
          this.screens[index].screen = gameObject.GetComponent<TargetScreen>();
          this.screens[index].tabIdx = this.AddTab(this.screens[index].icon, (string) Strings.Get(this.screens[index].displayName), (KScreen) this.screens[index].screen, (string) Strings.Get(this.screens[index].tooltip));
        }
      }
      this.onTabActivated += new KTabMenu.TabActivated(this.OnTabActivated);
      this.HasActivated = true;
    }
    int tabIdx = -1;
    int num = 0;
    for (int index = 0; index < this.screens.Length; ++index)
    {
      bool flag1 = this.screens[index].screen.IsValidForTarget(go);
      bool flag2 = this.screens[index].hideWhenDead && this.gameObject.HasTag(GameTags.Dead);
      bool enabled = flag1 && !flag2;
      this.SetTabEnabled(this.screens[index].tabIdx, enabled);
      if (enabled)
      {
        ++num;
        if (tabIdx == -1)
        {
          if (SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
          {
            if (SimDebugView.Instance.GetMode() == this.screens[index].focusInViewMode)
              tabIdx = index;
          }
          else if (enabled && this.previouslyActiveTab >= 0 && (this.previouslyActiveTab < this.screens.Length && this.screens[index].name == this.screens[this.previouslyActiveTab].name))
            tabIdx = this.screens[index].tabIdx;
        }
      }
    }
    if (tabIdx != -1)
      this.ActivateTab(tabIdx);
    else
      this.ActivateTab(0);
    this.tabHeaderContainer.gameObject.SetActive(this.CountTabs() > 1);
    if (this.sideScreens == null || this.sideScreens.Count <= 0)
      return;
    this.sideScreens.ForEach((System.Action<DetailsScreen.SideScreenRef>) (scn =>
    {
      if (!scn.screenPrefab.IsValidForTarget(this.target))
        return;
      if ((UnityEngine.Object) scn.screenInstance == (UnityEngine.Object) null)
        scn.screenInstance = Util.KInstantiateUI<SideScreenContent>(scn.screenPrefab.gameObject, this.sideScreenContentBody, false);
      if (!this.sideScreen.activeInHierarchy)
        this.sideScreen.SetActive(true);
      scn.screenInstance.transform.SetAsFirstSibling();
      scn.screenInstance.SetTarget(this.target);
      scn.screenInstance.Show(true);
      this.currentSideScreen = scn.screenInstance;
      this.RefreshTitle();
    }));
  }

  public void RefreshTitle()
  {
    if (!(bool) ((UnityEngine.Object) this.currentSideScreen))
      return;
    this.sideScreenTitle.SetText(this.currentSideScreen.GetTitle());
  }

  private void OnTabActivated(int newTab, int oldTab)
  {
    this.SetTitle(newTab);
    if (oldTab != -1)
      this.screens[oldTab].screen.SetTarget((GameObject) null);
    if (newTab == -1)
      return;
    this.screens[newTab].screen.SetTarget(this.target);
  }

  public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
  {
    this.ClearSecondarySideScreen();
    this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
    this.activeSideScreen2.Activate();
    this.sideScreen2Title.text = title;
    this.sideScreen2.SetActive(true);
    return this.activeSideScreen2;
  }

  public void ClearSecondarySideScreen()
  {
    if ((UnityEngine.Object) this.activeSideScreen2 != (UnityEngine.Object) null)
    {
      this.activeSideScreen2.Deactivate();
      this.activeSideScreen2 = (KScreen) null;
    }
    this.sideScreen2.SetActive(false);
  }

  public void DeactivateSideContent()
  {
    if ((UnityEngine.Object) SideDetailsScreen.Instance != (UnityEngine.Object) null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
      SideDetailsScreen.Instance.Show(false);
    if (this.sideScreens != null && this.sideScreens.Count > 0)
      this.sideScreens.ForEach((System.Action<DetailsScreen.SideScreenRef>) (scn =>
      {
        if (!((UnityEngine.Object) scn.screenInstance != (UnityEngine.Object) null))
          return;
        scn.screenInstance.ClearTarget();
        scn.screenInstance.Show(false);
      }));
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
    this.sideScreen.SetActive(false);
  }

  public void MaskSideContent(bool hide)
  {
    if (hide)
      this.sideScreen.transform.localScale = Vector3.zero;
    else
      this.sideScreen.transform.localScale = Vector3.one;
  }

  private string GetSelectedObjectCodexID()
  {
    string str = string.Empty;
    CellSelectionObject component1 = SelectTool.Instance.selected.GetComponent<CellSelectionObject>();
    BuildingUnderConstruction component2 = SelectTool.Instance.selected.GetComponent<BuildingUnderConstruction>();
    CreatureBrain component3 = SelectTool.Instance.selected.GetComponent<CreatureBrain>();
    PlantableSeed component4 = SelectTool.Instance.selected.GetComponent<PlantableSeed>();
    BudUprootedMonitor component5 = SelectTool.Instance.selected.GetComponent<BudUprootedMonitor>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(component1.element.id.ToString());
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(component2.Def.PrefabID);
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(SelectTool.Instance.selected.PrefabID().ToString()).Replace("BABY", string.Empty);
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(SelectTool.Instance.selected.PrefabID().ToString()).Replace("SEED", string.Empty);
    else if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) component5.parentObject.Get() != (UnityEngine.Object) null)
        str = CodexCache.FormatLinkID(component5.parentObject.Get().PrefabID().ToString());
      else if ((UnityEngine.Object) component5.GetComponent<TreeBud>() != (UnityEngine.Object) null)
        str = CodexCache.FormatLinkID(component5.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
    }
    else
      str = CodexCache.FormatLinkID(SelectTool.Instance.selected.PrefabID().ToString());
    if (CodexCache.entries.ContainsKey(str) || CodexCache.FindSubEntry(str) != null)
      return str;
    return string.Empty;
  }

  public void OpenCodexEntry()
  {
    string selectedObjectCodexId = this.GetSelectedObjectCodexID();
    if (!(selectedObjectCodexId != string.Empty))
      return;
    ManagementMenu.Instance.OpenCodexToEntry(selectedObjectCodexId);
  }

  public void DeselectAndClose()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
    SelectTool.Instance.Select((KSelectable) null, false);
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.target = (GameObject) null;
    this.DeactivateSideContent();
    this.Show(false);
  }

  private void SortScreenOrder()
  {
    Array.Sort<DetailsScreen.Screens>(this.screens, (Comparison<DetailsScreen.Screens>) ((x, y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority)));
  }

  public void UpdatePortrait(GameObject target)
  {
    KSelectable component1 = target.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    this.TabTitle.portrait.ClearPortrait();
    Building component2 = component1.GetComponent<Building>();
    if ((bool) ((UnityEngine.Object) component2))
    {
      Sprite uiSprite = component2.Def.GetUISprite("ui", false);
      if ((UnityEngine.Object) uiSprite != (UnityEngine.Object) null)
      {
        this.TabTitle.portrait.SetPortrait(uiSprite);
        return;
      }
    }
    if ((bool) ((UnityEngine.Object) target.GetComponent<MinionIdentity>()))
    {
      this.TabTitle.SetPortrait(component1.gameObject);
    }
    else
    {
      Edible component3 = target.GetComponent<Edible>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, string.Empty));
      }
      else
      {
        PrimaryElement component4 = target.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
        {
          this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, string.Empty));
        }
        else
        {
          CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
          if (!((UnityEngine.Object) component5 != (UnityEngine.Object) null))
            return;
          string animName = !component5.element.IsSolid ? component5.element.substance.name : "ui";
          this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, string.Empty));
        }
      }
    }
  }

  public bool CompareTargetWith(GameObject compare)
  {
    return (UnityEngine.Object) this.target == (UnityEngine.Object) compare;
  }

  public void SetTitle(int selectedTabIndex)
  {
    this.UpdateCodexButton();
    if (!((UnityEngine.Object) this.TabTitle != (UnityEngine.Object) null))
      return;
    this.TabTitle.SetTitle(this.target.GetProperName());
    MinionIdentity minionIdentity = (MinionIdentity) null;
    StorageLocker storageLocker = (StorageLocker) null;
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
      storageLocker = this.target.gameObject.GetComponent<StorageLocker>();
    }
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), string.Empty);
      this.TabTitle.SetUserEditable(true);
    }
    else if ((UnityEngine.Object) storageLocker != (UnityEngine.Object) null)
    {
      this.TabTitle.SetSubText(string.Empty, string.Empty);
      this.TabTitle.SetUserEditable(true);
    }
    else
    {
      this.TabTitle.SetSubText(string.Empty, string.Empty);
      this.TabTitle.SetUserEditable(false);
    }
  }

  public void SetTitle(string title)
  {
    this.TabTitle.SetTitle(title);
  }

  public TargetScreen GetActiveTab()
  {
    if (this.previouslyActiveTab >= 0 && this.previouslyActiveTab < this.screens.Length)
      return this.screens[this.previouslyActiveTab].screen;
    return (TargetScreen) null;
  }

  [Serializable]
  private struct Screens
  {
    public string name;
    public string displayName;
    public string tooltip;
    public Sprite icon;
    public TargetScreen screen;
    public int displayOrderPriority;
    public bool hideWhenDead;
    public HashedString focusInViewMode;
    [HideInInspector]
    public int tabIdx;
  }

  [Serializable]
  public class SideScreenRef
  {
    public string name;
    public SideScreenContent screenPrefab;
    public Vector2 offset;
    [HideInInspector]
    public SideScreenContent screenInstance;
  }
}
