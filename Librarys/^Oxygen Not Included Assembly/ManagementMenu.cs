// Decompiled with JetBrains decompiler
// Type: ManagementMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagementMenu : KIconToggleMenu
{
  private Dictionary<KIconToggleMenu.ToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<KIconToggleMenu.ToggleInfo, ManagementMenu.ScreenData>();
  [SerializeField]
  private KToggle smallPrefab;
  private ManagementMenu.ScreenData activeScreen;
  private KButton activeButton;
  public static ManagementMenu Instance;
  public KScreen researchScreen;
  private JobsTableScreen jobsScreen;
  public VitalsTableScreen vitalsScreen;
  public ScheduleScreen scheduleScreen;
  public KScreen reportsScreen;
  private ConsumablesTableScreen consumablesScreen;
  public CodexScreen codexScreen;
  private StarmapScreen starmapScreen;
  private SkillsScreen skillsScreen;
  public string colourSchemeDisabled;
  public InstantiateUIPrefabChild instantiator;
  private KIconToggleMenu.ToggleInfo jobsInfo;
  private KIconToggleMenu.ToggleInfo consumablesInfo;
  private KIconToggleMenu.ToggleInfo scheduleInfo;
  private KIconToggleMenu.ToggleInfo vitalsInfo;
  private KIconToggleMenu.ToggleInfo reportsInfo;
  private KIconToggleMenu.ToggleInfo researchInfo;
  private KIconToggleMenu.ToggleInfo codexInfo;
  private KIconToggleMenu.ToggleInfo starmapInfo;
  private KIconToggleMenu.ToggleInfo skillsInfo;
  public KButton[] CloseButtons;
  public KToggle PauseMenuButton;
  private string skillsTooltip;
  private string skillsTooltipDisabled;
  private string researchTooltip;
  private string researchTooltipDisabled;
  private string starmapTooltip;
  private string starmapTooltipDisabled;

  public static void DestroyInstance()
  {
    ManagementMenu.Instance = (ManagementMenu) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ManagementMenu.Instance = this;
    CodexCache.Init();
    ScheduledUIInstantiation component = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<ScheduledUIInstantiation>();
    this.instantiator.Instantiate();
    this.jobsScreen = this.instantiator.GetComponentInChildren<JobsTableScreen>(true);
    this.consumablesScreen = this.instantiator.GetComponentInChildren<ConsumablesTableScreen>(true);
    this.vitalsScreen = this.instantiator.GetComponentInChildren<VitalsTableScreen>(true);
    this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
    this.codexScreen = this.instantiator.GetComponentInChildren<CodexScreen>(true);
    this.scheduleScreen = this.instantiator.GetComponentInChildren<ScheduleScreen>(true);
    this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
    this.Subscribe(Game.Instance.gameObject, 288942073, new System.Action<object>(this.OnUIClear));
    this.consumablesInfo = new KIconToggleMenu.ToggleInfo((string) UI.CONSUMABLES, "OverviewUI_consumables_icon", (object) null, Action.ManageConsumables, (string) UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, string.Empty);
    this.vitalsInfo = new KIconToggleMenu.ToggleInfo((string) UI.VITALS, "OverviewUI_vitals_icon", (object) null, Action.ManageVitals, (string) UI.TOOLTIPS.MANAGEMENTMENU_VITALS, string.Empty);
    this.reportsInfo = new KIconToggleMenu.ToggleInfo((string) UI.REPORT, "OverviewUI_reports_icon", (object) null, Action.ManageReport, (string) UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, string.Empty);
    this.reportsInfo.prefabOverride = this.smallPrefab;
    this.researchInfo = new KIconToggleMenu.ToggleInfo((string) UI.RESEARCH, "OverviewUI_research_nav_icon", (object) null, Action.ManageResearch, (string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, string.Empty);
    this.jobsInfo = new KIconToggleMenu.ToggleInfo((string) UI.JOBS, "OverviewUI_priority_icon", (object) null, Action.ManagePriorities, (string) UI.TOOLTIPS.MANAGEMENTMENU_JOBS, string.Empty);
    this.skillsInfo = new KIconToggleMenu.ToggleInfo((string) UI.SKILLS, "OverviewUI_jobs_icon", (object) null, Action.ManageSkills, (string) UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, string.Empty);
    this.starmapInfo = new KIconToggleMenu.ToggleInfo((string) UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", (object) null, Action.ManageStarmap, (string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, string.Empty);
    this.codexInfo = new KIconToggleMenu.ToggleInfo((string) UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", (object) null, Action.ManageDatabase, (string) UI.TOOLTIPS.MANAGEMENTMENU_CODEX, string.Empty);
    this.codexInfo.prefabOverride = this.smallPrefab;
    this.scheduleInfo = new KIconToggleMenu.ToggleInfo((string) UI.SCHEDULE, "OverviewUI_schedule2_icon", (object) null, Action.ManageSchedule, (string) UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, string.Empty);
    this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.consumablesScreen,
      tabIdx = 3,
      toggleInfo = this.consumablesInfo
    });
    this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.vitalsScreen,
      tabIdx = 2,
      toggleInfo = this.vitalsInfo
    });
    this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData()
    {
      screen = this.reportsScreen,
      tabIdx = 4,
      toggleInfo = this.reportsInfo
    });
    this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.jobsScreen,
      tabIdx = 1,
      toggleInfo = this.jobsInfo
    });
    this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.skillsScreen,
      tabIdx = 0,
      toggleInfo = this.skillsInfo
    });
    this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.codexScreen,
      tabIdx = 6,
      toggleInfo = this.codexInfo
    });
    this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.scheduleScreen,
      tabIdx = 7,
      toggleInfo = this.scheduleInfo
    });
    this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.starmapScreen,
      tabIdx = 7,
      toggleInfo = this.starmapInfo
    });
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) new List<KIconToggleMenu.ToggleInfo>()
    {
      this.vitalsInfo,
      this.consumablesInfo,
      this.scheduleInfo,
      this.jobsInfo,
      this.skillsInfo,
      this.researchInfo,
      this.starmapInfo,
      this.reportsInfo,
      this.codexInfo
    });
    this.onSelect += new KIconToggleMenu.OnSelect(this.OnButtonClick);
    this.PauseMenuButton.onClick += new System.Action(this.OnPauseMenuClicked);
    this.PauseMenuButton.transform.SetAsLastSibling();
    this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, Action.Escape);
    Components.ResearchCenters.OnAdd += new System.Action<ResearchCenter>(this.CheckResearch);
    Components.ResearchCenters.OnRemove += new System.Action<ResearchCenter>(this.CheckResearch);
    Components.RoleStations.OnAdd += new System.Action<RoleStation>(this.CheckSkills);
    Components.RoleStations.OnRemove += new System.Action<RoleStation>(this.CheckSkills);
    Game.Instance.Subscribe(-809948329, new System.Action<object>(this.CheckResearch));
    Game.Instance.Subscribe(-809948329, new System.Action<object>(this.CheckSkills));
    Components.Telescopes.OnAdd += new System.Action<Telescope>(this.CheckStarmap);
    Components.Telescopes.OnRemove += new System.Action<Telescope>(this.CheckStarmap);
    this.skillsTooltipDisabled = (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION;
    this.skillsTooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, Action.ManageSkills);
    this.researchTooltipDisabled = (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH;
    this.researchTooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, Action.ManageResearch);
    this.starmapTooltipDisabled = (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE;
    this.starmapTooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, Action.ManageStarmap);
    this.CheckResearch((object) null);
    this.CheckSkills((object) null);
    this.CheckStarmap((object) null);
    this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() =>
    {
      if (!this.ResearchAvailable())
        return this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo];
      return true;
    });
    foreach (KButton closeButton in this.CloseButtons)
    {
      closeButton.onClick += new System.Action(this.CloseAll);
      closeButton.soundPlayer.Enabled = false;
    }
    foreach (KToggle toggle in this.toggles)
    {
      toggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
      toggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
    }
  }

  private void OnPauseMenuClicked()
  {
    PauseScreen.Instance.Show(true);
    this.PauseMenuButton.isOn = false;
  }

  public void AddResearchScreen(ResearchScreen researchScreen)
  {
    if ((UnityEngine.Object) this.researchScreen != (UnityEngine.Object) null)
      return;
    this.researchScreen = (KScreen) researchScreen;
    this.researchScreen.gameObject.SetActive(false);
    this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData()
    {
      screen = this.researchScreen,
      tabIdx = 5,
      toggleInfo = this.researchInfo
    });
    this.researchScreen.Show(false);
  }

  public void CheckResearch(object o)
  {
    if ((UnityEngine.Object) this.researchInfo.toggle == (UnityEngine.Object) null)
      return;
    bool disabled = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode;
    bool active = !disabled && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
    string tooltip = !disabled ? this.researchTooltip : this.researchTooltipDisabled;
    this.ConfigureToggle(this.researchInfo.toggle, disabled, active, tooltip, this.ToggleToolTipTextStyleSetting);
  }

  public void CheckSkills(object o = null)
  {
    if (this.skillsInfo == null || (UnityEngine.Object) this.skillsInfo.toggle == (UnityEngine.Object) null)
      return;
    bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode;
    bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
    string tooltip = !disabled ? this.skillsTooltip : this.skillsTooltipDisabled;
    this.ConfigureToggle(this.skillsInfo.toggle, disabled, active, tooltip, this.ToggleToolTipTextStyleSetting);
  }

  public void CheckStarmap(object o = null)
  {
    if ((UnityEngine.Object) this.starmapInfo.toggle == (UnityEngine.Object) null)
      return;
    bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode;
    bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
    string tooltip = !disabled ? this.starmapTooltip : this.starmapTooltipDisabled;
    this.ConfigureToggle(this.starmapInfo.toggle, disabled, active, tooltip, this.ToggleToolTipTextStyleSetting);
  }

  private void ConfigureToggle(
    KToggle toggle,
    bool disabled,
    bool active,
    string tooltip,
    TextStyleSetting tooltip_style)
  {
    toggle.interactable = active;
    toggle.GetComponent<KToggle>().interactable = !disabled;
    if (disabled)
      toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
    else
      toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
    ToolTip component = toggle.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    component.AddMultiStringTooltip(tooltip, (ScriptableObject) tooltip_style);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.activeScreen != null && e.TryConsume(Action.Escape))
      this.ToggleScreen(this.activeScreen);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.ToggleScreen(this.activeScreen);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private bool ResearchAvailable()
  {
    if (Components.ResearchCenters.Count <= 0)
      return DebugHandler.InstantBuildMode;
    return true;
  }

  private bool SkillsAvailable()
  {
    if (Components.RoleStations.Count <= 0)
      return DebugHandler.InstantBuildMode;
    return true;
  }

  private bool StarmapAvailable()
  {
    if (Components.Telescopes.Count <= 0)
      return DebugHandler.InstantBuildMode;
    return true;
  }

  public void CloseAll()
  {
    if (this.activeScreen == null)
      return;
    if (this.activeScreen.toggleInfo != null)
      this.ToggleScreen(this.activeScreen);
    this.CloseActive();
    this.ClearSelection();
  }

  private void OnUIClear(object data)
  {
    this.CloseAll();
  }

  public void ToggleScreen(ManagementMenu.ScreenData screenData)
  {
    if (screenData == null)
      return;
    if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
    {
      this.CheckResearch((object) null);
      this.CloseActive();
    }
    else if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
    {
      this.CheckSkills((object) null);
      this.CloseActive();
    }
    else if (screenData.toggleInfo == this.starmapInfo && !this.StarmapAvailable())
    {
      this.CheckStarmap((object) null);
      this.CloseActive();
    }
    else
    {
      if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
        return;
      if (this.activeScreen != null)
      {
        this.activeScreen.toggleInfo.toggle.isOn = false;
        this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
      }
      if (this.activeScreen != screenData)
      {
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
        if (this.activeScreen != null)
          this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
        screenData.toggleInfo.toggle.ActivateFlourish(true);
        screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
        this.CloseActive();
        this.activeScreen = screenData;
        this.activeScreen.screen.Show(true);
      }
      else
      {
        this.activeScreen.screen.Show(false);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
        AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
        this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
        this.activeScreen = (ManagementMenu.ScreenData) null;
        screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
      }
    }
  }

  public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
  {
    this.ToggleScreen(this.ScreenInfoMatch[toggle_info]);
  }

  private void CloseActive()
  {
    if (this.activeScreen == null)
      return;
    this.activeScreen.toggleInfo.toggle.isOn = false;
    this.activeScreen.screen.Show(false);
    this.activeScreen = (ManagementMenu.ScreenData) null;
  }

  public void ToggleResearch()
  {
    if ((UnityEngine.Object) this.researchScreen == (UnityEngine.Object) null)
      this.AddResearchScreen(UnityEngine.Object.FindObjectOfType<ResearchScreen>());
    if (!this.ResearchAvailable() && this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo] || this.researchInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
  }

  public void ToggleCodex()
  {
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
  }

  public void OpenCodexToEntry(string id)
  {
    if (!this.codexScreen.gameObject.activeInHierarchy)
      this.ToggleCodex();
    this.codexScreen.ChangeArticle(id, false);
  }

  public void ToggleSkills()
  {
    if (!this.SkillsAvailable() && this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo] || this.skillsInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
  }

  public void ToggleStarmap()
  {
    if (this.starmapInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
  }

  public void TogglePriorities()
  {
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
  }

  public void OpenReports(int day)
  {
    if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
      this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
    ReportScreen.Instance.ShowReport(day);
  }

  public void OpenResearch()
  {
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
  }

  public void OpenStarmap()
  {
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
  }

  public void OpenSkills(MinionIdentity minionIdentity)
  {
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
      return;
    this.skillsScreen.CurrentlySelectedMinion = (IAssignableIdentity) minionIdentity;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
  }

  public class ScreenData
  {
    public KScreen screen;
    public KIconToggleMenu.ToggleInfo toggleInfo;
    public int tabIdx;
  }
}
