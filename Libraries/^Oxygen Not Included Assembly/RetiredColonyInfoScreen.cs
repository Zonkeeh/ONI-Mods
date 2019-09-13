// Decompiled with JetBrains decompiler
// Type: RetiredColonyInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RetiredColonyInfoScreen : KModalScreen
{
  private string[] currentSlideshowFiles = new string[0];
  private Dictionary<string, GameObject> achievementEntries = new Dictionary<string, GameObject>();
  private List<GameObject> activeColonyWidgetContainers = new List<GameObject>();
  private Dictionary<string, GameObject> activeColonyWidgets = new Dictionary<string, GameObject>();
  private Dictionary<string, Color> statColors = new Dictionary<string, Color>()
  {
    {
      RetiredColonyData.DataIDs.OxygenProduced,
      new Color(0.17f, 0.91f, 0.91f, 1f)
    },
    {
      RetiredColonyData.DataIDs.OxygenConsumed,
      new Color(0.17f, 0.91f, 0.91f, 1f)
    },
    {
      RetiredColonyData.DataIDs.CaloriesProduced,
      new Color(0.24f, 0.49f, 0.32f, 1f)
    },
    {
      RetiredColonyData.DataIDs.CaloriesRemoved,
      new Color(0.24f, 0.49f, 0.32f, 1f)
    },
    {
      RetiredColonyData.DataIDs.PowerProduced,
      new Color(0.98f, 0.69f, 0.23f, 1f)
    },
    {
      RetiredColonyData.DataIDs.PowerWasted,
      new Color(0.82f, 0.3f, 0.35f, 1f)
    },
    {
      RetiredColonyData.DataIDs.WorkTime,
      new Color(0.99f, 0.51f, 0.28f, 1f)
    },
    {
      RetiredColonyData.DataIDs.TravelTime,
      new Color(0.55f, 0.55f, 0.75f, 1f)
    },
    {
      RetiredColonyData.DataIDs.AverageWorkTime,
      new Color(0.99f, 0.51f, 0.28f, 1f)
    },
    {
      RetiredColonyData.DataIDs.AverageTravelTime,
      new Color(0.55f, 0.55f, 0.75f, 1f)
    },
    {
      RetiredColonyData.DataIDs.LiveDuplicants,
      new Color(0.98f, 0.69f, 0.23f, 1f)
    },
    {
      RetiredColonyData.DataIDs.RocketsInFlight,
      new Color(0.9f, 0.9f, 0.16f, 1f)
    },
    {
      RetiredColonyData.DataIDs.AverageStressCreated,
      new Color(0.8f, 0.32f, 0.33f, 1f)
    },
    {
      RetiredColonyData.DataIDs.AverageStressRemoved,
      new Color(0.8f, 0.32f, 0.33f, 1f)
    },
    {
      RetiredColonyData.DataIDs.AverageGerms,
      new Color(0.68f, 0.79f, 0.18f, 1f)
    },
    {
      RetiredColonyData.DataIDs.DomesticatedCritters,
      new Color(0.62f, 0.31f, 0.47f, 1f)
    },
    {
      RetiredColonyData.DataIDs.WildCritters,
      new Color(0.62f, 0.31f, 0.47f, 1f)
    }
  };
  private Dictionary<string, GameObject> explorerColonyWidgets = new Dictionary<string, GameObject>();
  public static RetiredColonyInfoScreen Instance;
  private bool wasPixelPerfect;
  [Header("Screen")]
  [SerializeField]
  private KButton closeButton;
  [Header("Header References")]
  [SerializeField]
  private GameObject explorerHeaderContainer;
  [SerializeField]
  private GameObject colonyHeaderContainer;
  [SerializeField]
  private LocText colonyName;
  [SerializeField]
  private LocText cycleCount;
  [Header("Timelapse References")]
  [SerializeField]
  private Slideshow slideshow;
  [Header("Main Layout")]
  [SerializeField]
  private GameObject coloniesSection;
  [SerializeField]
  private GameObject achievementsSection;
  [Header("Achievement References")]
  [SerializeField]
  private GameObject achievementsContainer;
  [SerializeField]
  private GameObject achievementsPrefab;
  [SerializeField]
  private GameObject victoryAchievementsPrefab;
  [SerializeField]
  private TMP_InputField achievementSearch;
  [SerializeField]
  private KButton clearAchievementSearchButton;
  [SerializeField]
  private GameObject[] achievementVeils;
  [Header("Duplicant References")]
  [SerializeField]
  private GameObject duplicantPrefab;
  [Header("Building References")]
  [SerializeField]
  private GameObject buildingPrefab;
  [Header("Colony Stat References")]
  [SerializeField]
  private GameObject statsContainer;
  [SerializeField]
  private GameObject specialMediaBlock;
  [SerializeField]
  private GameObject tallFeatureBlock;
  [SerializeField]
  private GameObject standardStatBlock;
  [SerializeField]
  private GameObject lineGraphPrefab;
  public RetiredColonyData[] retiredColonyData;
  [Header("Explorer References")]
  [SerializeField]
  private GameObject colonyScroll;
  [SerializeField]
  private GameObject explorerRoot;
  [SerializeField]
  private GameObject explorerGrid;
  [SerializeField]
  private GameObject colonyDataRoot;
  [SerializeField]
  private GameObject colonyButtonPrefab;
  [SerializeField]
  private TMP_InputField explorerSearch;
  [SerializeField]
  private KButton clearExplorerSearchButton;
  [Header("Navigation Buttons")]
  [SerializeField]
  private KButton closeScreenButton;
  [SerializeField]
  private KButton viewOtherColoniesButton;
  [SerializeField]
  private KButton quitToMainMenuButton;
  [SerializeField]
  private GameObject disabledPlatformUnlocks;
  private bool explorerGridConfigured;
  private const float maxAchievementWidth = 830f;
  private Canvas canvasRef;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    RetiredColonyInfoScreen.Instance = this;
    this.ConfigButtons();
    this.LoadExplorer();
    this.PopulateAchievements();
    this.ConsumeMouseScroll = true;
    this.explorerSearch.text = string.Empty;
    this.explorerSearch.onValueChanged.AddListener((UnityAction<string>) (value =>
    {
      if (this.colonyDataRoot.activeSelf)
        this.FilterColonyData(this.explorerSearch.text);
      else
        this.FilterExplorer(this.explorerSearch.text);
    }));
    this.clearExplorerSearchButton.onClick += (System.Action) (() => this.explorerSearch.text = string.Empty);
    this.achievementSearch.text = string.Empty;
    this.achievementSearch.onValueChanged.AddListener((UnityAction<string>) (value => this.FilterAchievements(this.achievementSearch.text)));
    this.clearAchievementSearchButton.onClick += (System.Action) (() => this.achievementSearch.text = string.Empty);
    this.RefreshUIScale((object) null);
    this.Subscribe(-810220474, new System.Action<object>(this.RefreshUIScale));
  }

  private void RefreshUIScale(object data = null)
  {
    this.StartCoroutine(this.DelayedRefreshScale());
  }

  [DebuggerHidden]
  private IEnumerator DelayedRefreshScale()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CDelayedRefreshScale\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void ConfigButtons()
  {
    this.closeButton.ClearOnClick();
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    this.viewOtherColoniesButton.ClearOnClick();
    this.viewOtherColoniesButton.onClick += (System.Action) (() => this.ToggleExplorer(true));
    this.quitToMainMenuButton.ClearOnClick();
    this.quitToMainMenuButton.onClick += (System.Action) (() => this.ConfirmDecision((string) STRINGS.UI.FRONTEND.MAINMENU.QUITCONFIRM, new System.Action(this.OnQuitConfirm)));
    this.closeScreenButton.ClearOnClick();
    this.closeScreenButton.onClick += (System.Action) (() => this.Show(false));
    this.viewOtherColoniesButton.gameObject.SetActive(false);
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      this.closeScreenButton.gameObject.SetActive(true);
      this.closeScreenButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.RETURN_TO_GAME);
      this.quitToMainMenuButton.gameObject.SetActive(true);
    }
    else
    {
      this.closeScreenButton.gameObject.SetActive(true);
      this.closeScreenButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.BUTTONS.CLOSE);
      this.quitToMainMenuButton.gameObject.SetActive(false);
    }
  }

  private void ConfirmDecision(string text, System.Action onConfirm)
  {
    this.gameObject.SetActive(false);
    ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(text, onConfirm, new System.Action(this.OnCancelPopup), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
  }

  private void OnCancelPopup()
  {
    this.gameObject.SetActive(true);
  }

  private void OnQuitConfirm()
  {
    LoadingOverlay.Load((System.Action) (() =>
    {
      this.Deactivate();
      PauseScreen.TriggerQuitGame();
    }));
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.GetCanvasRef();
    this.wasPixelPerfect = this.canvasRef.pixelPerfect;
    this.canvasRef.pixelPerfect = false;
  }

  private void GetCanvasRef()
  {
    if ((UnityEngine.Object) this.transform.parent.GetComponent<Canvas>() != (UnityEngine.Object) null)
      this.canvasRef = this.transform.parent.GetComponent<Canvas>();
    else
      this.canvasRef = this.transform.parent.parent.GetComponent<Canvas>();
  }

  protected override void OnCmpDisable()
  {
    this.canvasRef.pixelPerfect = this.wasPixelPerfect;
    base.OnCmpDisable();
  }

  public RetiredColonyData GetColonyDataByBaseName(string name)
  {
    name = RetireColonyUtility.StripInvalidCharacters(name);
    for (int index = 0; index < this.retiredColonyData.Length; ++index)
    {
      if (RetireColonyUtility.StripInvalidCharacters(this.retiredColonyData[index].colonyName) == name)
        return this.retiredColonyData[index];
    }
    return (RetiredColonyData) null;
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      this.RefreshUIScale((object) null);
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      if (!show)
      {
        if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
          MusicManager.instance.StopSong("Music_Victory_03_StoryAndSummary", true, STOP_MODE.ALLOWFADEOUT);
      }
      else
      {
        this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
        if (MusicManager.instance.SongIsPlaying("Music_Victory_03_StoryAndSummary"))
          MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 2f, true);
      }
    }
    else if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      this.ToggleExplorer(true);
    this.disabledPlatformUnlocks.SetActive((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null);
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null))
      return;
    this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("enabled").gameObject.SetActive((DebugHandler.InstantBuildMode || SaveGame.Instance.sandboxEnabled ? 1 : (Game.Instance.debugWasUsed ? 1 : 0)) == 0);
    this.disabledPlatformUnlocks.GetComponent<HierarchyReferences>().GetReference("disabled").gameObject.SetActive(DebugHandler.InstantBuildMode || SaveGame.Instance.sandboxEnabled || Game.Instance.debugWasUsed);
  }

  public void LoadColony(RetiredColonyData data)
  {
    this.colonyName.text = data.colonyName.ToUpper();
    this.cycleCount.text = string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, (object) data.cycleCount.ToString());
    this.ToggleExplorer(false);
    this.RefreshUIScale((object) null);
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      this.viewOtherColoniesButton.gameObject.SetActive(true);
    this.ClearColony();
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      ColonyAchievementTracker component = SaveGame.Instance.GetComponent<ColonyAchievementTracker>();
      this.UpdateAchievementData(data, component.achievementsToDisplay.ToArray());
      component.ClearDisplayAchievements();
      this.PopulateAchievementProgress(component);
    }
    else
      this.UpdateAchievementData(data, (string[]) null);
    this.DisplayStatistics(data);
    this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0.0f, 0.0f));
  }

  private void PopulateAchievementProgress(ColonyAchievementTracker tracker)
  {
    if (!((UnityEngine.Object) tracker != (UnityEngine.Object) null))
      return;
    foreach (KeyValuePair<string, GameObject> achievementEntry in this.achievementEntries)
    {
      ColonyAchievementStatus achievement;
      tracker.achievements.TryGetValue(achievementEntry.Key, out achievement);
      if (achievement != null)
      {
        AchievementWidget component = achievementEntry.Value.GetComponent<AchievementWidget>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.ShowProgress(achievement);
          if (achievement.failed)
            component.SetFailed();
        }
      }
    }
  }

  private bool LoadSlideshow(RetiredColonyData data)
  {
    this.clearCurrentSlideshow();
    this.currentSlideshowFiles = RetireColonyUtility.LoadColonySlideshowFiles(data.colonyName);
    this.slideshow.SetFiles(this.currentSlideshowFiles, -1);
    if (this.currentSlideshowFiles != null)
      return this.currentSlideshowFiles.Length > 0;
    return false;
  }

  private void clearCurrentSlideshow()
  {
    this.currentSlideshowFiles = new string[0];
  }

  private bool LoadScreenshot(RetiredColonyData data)
  {
    this.clearCurrentSlideshow();
    Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(data.colonyName);
    if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
    {
      this.slideshow.setSlide(sprite);
      this.CorrectTimelapseImageSize(sprite);
    }
    return (UnityEngine.Object) sprite != (UnityEngine.Object) null;
  }

  private void ClearColony()
  {
    foreach (UnityEngine.Object colonyWidgetContainer in this.activeColonyWidgetContainers)
      UnityEngine.Object.Destroy(colonyWidgetContainer);
    this.activeColonyWidgetContainers.Clear();
    this.activeColonyWidgets.Clear();
    this.UpdateAchievementData((RetiredColonyData) null, (string[]) null);
  }

  private void PopulateAchievements()
  {
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      GameObject gameObject = Util.KInstantiateUI(!resource.isVictoryCondition ? this.achievementsPrefab : this.victoryAchievementsPrefab, this.achievementsContainer, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("nameLabel").SetText(resource.Name);
      component.GetReference<LocText>("descriptionLabel").SetText(resource.description);
      if (string.IsNullOrEmpty(resource.icon) || (UnityEngine.Object) Assets.GetSprite((HashedString) resource.icon) == (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) Assets.GetSprite((HashedString) resource.Name) != (UnityEngine.Object) null)
          component.GetReference<Image>("icon").sprite = Assets.GetSprite((HashedString) resource.Name);
        else
          component.GetReference<Image>("icon").sprite = Assets.GetSprite((HashedString) "check");
      }
      else
        component.GetReference<Image>("icon").sprite = Assets.GetSprite((HashedString) resource.icon);
      if (resource.isVictoryCondition)
        gameObject.transform.SetAsFirstSibling();
      gameObject.GetComponent<MultiToggle>().ChangeState(2);
      this.achievementEntries.Add(resource.Id, gameObject);
    }
    this.UpdateAchievementData((RetiredColonyData) null, (string[]) null);
  }

  [DebuggerHidden]
  private IEnumerator ClearAchievementVeil(float delay = 0.0f)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CClearAchievementVeil\u003Ec__Iterator1()
    {
      delay = delay,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator ShowAchievementVeil()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CShowAchievementVeil\u003Ec__Iterator2()
    {
      \u0024this = this
    };
  }

  private void UpdateAchievementData(RetiredColonyData data, string[] newlyAchieved = null)
  {
    int num1 = 1;
    float num2 = 1f;
    if (newlyAchieved != null && newlyAchieved.Length > 0)
      this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(true);
    foreach (KeyValuePair<string, GameObject> achievementEntry in this.achievementEntries)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (data != null)
      {
        foreach (string achievement in data.achievements)
        {
          if (achievement == achievementEntry.Key)
          {
            flag1 = true;
            break;
          }
        }
      }
      if (!flag1 && data == null && this.retiredColonyData != null)
      {
        foreach (RetiredColonyData retiredColonyData in this.retiredColonyData)
        {
          foreach (string achievement in retiredColonyData.achievements)
          {
            if (achievement == achievementEntry.Key)
              flag2 = true;
          }
        }
      }
      bool flag3 = false;
      if (newlyAchieved != null)
      {
        for (int index = 0; index < newlyAchieved.Length; ++index)
        {
          if (newlyAchieved[index] == achievementEntry.Key)
            flag3 = true;
        }
      }
      if (flag1 || flag3)
      {
        if (flag3)
        {
          achievementEntry.Value.GetComponent<AchievementWidget>().ActivateNewlyAchievedFlourish(num2 + (float) num1 * 1f);
          ++num1;
        }
        else
          achievementEntry.Value.GetComponent<AchievementWidget>().SetAchievedNow();
      }
      else if (flag2)
        achievementEntry.Value.GetComponent<AchievementWidget>().SetAchievedBefore();
      else if (data == null)
        achievementEntry.Value.GetComponent<AchievementWidget>().SetNeverAchieved();
      else
        achievementEntry.Value.GetComponent<AchievementWidget>().SetNotAchieved();
    }
    if (newlyAchieved == null || newlyAchieved.Length <= 0)
      return;
    this.StartCoroutine(this.ShowAchievementVeil());
    this.StartCoroutine(this.ClearAchievementVeil(num2 + (float) num1 * 1f));
  }

  private void DisplayInfoBlock(RetiredColonyData data, GameObject container)
  {
    container.GetComponent<HierarchyReferences>().GetReference<LocText>("ColonyNameLabel").SetText(data.colonyName);
    container.GetComponent<HierarchyReferences>().GetReference<LocText>("CycleCountLabel").SetText(string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, (object) data.cycleCount.ToString()));
  }

  private void CorrectTimelapseImageSize(Sprite sprite)
  {
    Vector2 sizeDelta = this.slideshow.transform.parent.GetComponent<RectTransform>().sizeDelta;
    Vector2 fittedSize = this.slideshow.GetFittedSize(sprite, sizeDelta.x, sizeDelta.y);
    LayoutElement component = this.slideshow.GetComponent<LayoutElement>();
    LayoutElement layoutElement1 = component;
    float x = fittedSize.x;
    component.preferredWidth = x;
    double num1 = (double) x;
    layoutElement1.minWidth = (float) num1;
    LayoutElement layoutElement2 = component;
    float y = fittedSize.y;
    component.preferredHeight = y;
    double num2 = (double) y;
    layoutElement2.minHeight = (float) num2;
  }

  private void DisplayTimelapse(RetiredColonyData data, GameObject container)
  {
    RectTransform reference = container.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PlayIcon");
    this.slideshow = container.GetComponent<HierarchyReferences>().GetReference<Slideshow>("Slideshow");
    this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
    this.slideshow.SetPaused(true);
    this.slideshow.onBeforePlay = (Slideshow.onBeforeAndEndPlayDelegate) (() => this.LoadSlideshow(data));
    this.slideshow.onEndingPlay = (Slideshow.onBeforeAndEndPlayDelegate) (() => this.LoadScreenshot(data));
    if (!this.LoadScreenshot(data))
    {
      this.slideshow.gameObject.SetActive(false);
      reference.gameObject.SetActive(false);
    }
    else
    {
      this.slideshow.gameObject.SetActive(true);
      reference.gameObject.SetActive(true);
    }
  }

  private void DisplayDuplicants(
    RetiredColonyData data,
    GameObject container,
    int range_min = -1,
    int range_max = -1)
  {
    for (int index = container.transform.childCount - 1; index >= 0; --index)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) container.transform.GetChild(index).gameObject);
    for (int index = 0; index < data.Duplicants.Length; ++index)
    {
      if (index < range_min || index > range_max && range_max != -1)
      {
        new GameObject().transform.SetParent(container.transform);
      }
      else
      {
        RetiredColonyData.RetiredDuplicantData duplicant = data.Duplicants[index];
        GameObject gameObject = Util.KInstantiateUI(this.duplicantPrefab, container, true);
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        component.GetReference<LocText>("NameLabel").SetText(duplicant.name);
        component.GetReference<LocText>("AgeLabel").SetText(string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.DUPLICANT_AGE, (object) duplicant.age.ToString()));
        component.GetReference<LocText>("SkillLabel").SetText(string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.SKILL_LEVEL, (object) duplicant.skillPointsGained.ToString()));
        SymbolOverrideController reference = component.GetReference<SymbolOverrideController>("SymbolOverrideController");
        reference.RemoveAllSymbolOverrides(0);
        KBatchedAnimController componentInChildren = gameObject.GetComponentInChildren<KBatchedAnimController>();
        componentInChildren.SetSymbolVisiblity((KAnimHashedString) "snapTo_neck", false);
        componentInChildren.SetSymbolVisiblity((KAnimHashedString) "snapTo_goggles", false);
        componentInChildren.SetSymbolVisiblity((KAnimHashedString) "snapTo_hat", false);
        componentInChildren.SetSymbolVisiblity((KAnimHashedString) "snapTo_hat_hair", false);
        foreach (KeyValuePair<string, string> accessory in duplicant.accessories)
        {
          KAnim.Build.Symbol symbol = Db.Get().Accessories.Get(accessory.Value).symbol;
          AccessorySlot accessorySlot = Db.Get().AccessorySlots.Get(accessory.Key);
          reference.AddSymbolOverride((HashedString) accessorySlot.targetSymbolId, symbol, 0);
          gameObject.GetComponentInChildren<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) accessory.Key, true);
        }
        reference.ApplyOverrides();
      }
    }
    this.StartCoroutine(this.ActivatePortraitsWhenReady(container));
  }

  [DebuggerHidden]
  private IEnumerator ActivatePortraitsWhenReady(GameObject container)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CActivatePortraitsWhenReady\u003Ec__Iterator3()
    {
      container = container
    };
  }

  private void DisplayBuildings(RetiredColonyData data, GameObject container)
  {
    for (int index = container.transform.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) container.transform.GetChild(index).gameObject);
    data.buildings.Sort((Comparison<Tuple<string, int>>) ((a, b) =>
    {
      if (a.second > b.second)
        return 1;
      return a.second == b.second ? 0 : -1;
    }));
    data.buildings.Reverse();
    foreach (Tuple<string, int> building in data.buildings)
    {
      GameObject prefab = Assets.GetPrefab((Tag) building.first);
      HierarchyReferences component = Util.KInstantiateUI(this.buildingPrefab, container, true).GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("NameLabel").SetText(GameUtil.ApplyBoldString(prefab.GetProperName()));
      component.GetReference<LocText>("CountLabel").SetText(string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.BUILDING_COUNT, (object) building.second.ToString()));
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) prefab, "ui", false);
      component.GetReference<Image>("Portrait").sprite = uiSprite.first;
    }
  }

  [DebuggerHidden]
  private IEnumerator ComputeSizeStatGrid()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CComputeSizeStatGrid\u003Ec__Iterator4()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator ComputeSizeExplorerGrid()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RetiredColonyInfoScreen.\u003CComputeSizeExplorerGrid\u003Ec__Iterator5()
    {
      \u0024this = this
    };
  }

  private void DisplayStatistics(RetiredColonyData data)
  {
    GameObject container = Util.KInstantiateUI(this.specialMediaBlock, this.statsContainer, true);
    this.activeColonyWidgetContainers.Add(container);
    this.activeColonyWidgets.Add("timelapse", container);
    this.DisplayTimelapse(data, container);
    GameObject duplicantBlock = Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
    this.activeColonyWidgetContainers.Add(duplicantBlock);
    this.activeColonyWidgets.Add("duplicants", duplicantBlock);
    duplicantBlock.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.TITLES.DUPLICANTS);
    PageView pageView = duplicantBlock.GetComponentInChildren<PageView>();
    pageView.OnChangePage = (System.Action<int>) (page => this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, page * pageView.ChildrenPerPage, (page + 1) * pageView.ChildrenPerPage));
    this.DisplayDuplicants(data, duplicantBlock.GetComponent<HierarchyReferences>().GetReference("Content").gameObject, -1, -1);
    GameObject gameObject = Util.KInstantiateUI(this.tallFeatureBlock, this.statsContainer, true);
    this.activeColonyWidgetContainers.Add(gameObject);
    this.activeColonyWidgets.Add("buildings", gameObject);
    gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.TITLES.BUILDINGS);
    this.DisplayBuildings(data, gameObject.GetComponent<HierarchyReferences>().GetReference("Content").gameObject);
    int num = 2;
    for (int index1 = 0; index1 < data.Stats.Length; index1 += num)
    {
      GameObject layoutBlockGameObject = Util.KInstantiateUI(this.standardStatBlock, this.statsContainer, true);
      this.activeColonyWidgetContainers.Add(layoutBlockGameObject);
      for (int index2 = 0; index2 < num; ++index2)
      {
        if (index1 + index2 <= data.Stats.Length - 1)
          this.ConfigureGraph(this.GetStatistic(data.Stats[index1 + index2].id, data), layoutBlockGameObject);
      }
    }
    this.StartCoroutine(this.ComputeSizeStatGrid());
  }

  private void ConfigureGraph(
    RetiredColonyData.RetiredColonyStatistic statistic,
    GameObject layoutBlockGameObject)
  {
    GameObject gameObject = Util.KInstantiateUI(this.lineGraphPrefab, layoutBlockGameObject, true);
    this.activeColonyWidgets.Add(statistic.name, gameObject);
    GraphBase componentInChildren1 = gameObject.GetComponentInChildren<GraphBase>();
    componentInChildren1.graphName = statistic.name;
    componentInChildren1.label_title.SetText(componentInChildren1.graphName);
    componentInChildren1.axis_x.name = statistic.nameX;
    componentInChildren1.axis_y.name = statistic.nameY;
    componentInChildren1.label_x.SetText(componentInChildren1.axis_x.name);
    componentInChildren1.label_y.SetText(componentInChildren1.axis_y.name);
    LineLayer componentInChildren2 = gameObject.GetComponentInChildren<LineLayer>();
    componentInChildren1.axis_y.min_value = 0.0f;
    componentInChildren1.axis_y.max_value = statistic.GetByMaxValue().second * 1.2f;
    componentInChildren1.axis_x.min_value = 0.0f;
    componentInChildren1.axis_x.max_value = statistic.GetByMaxKey().first;
    componentInChildren1.axis_x.guide_frequency = (float) (((double) componentInChildren1.axis_x.max_value - (double) componentInChildren1.axis_x.min_value) / 10.0);
    componentInChildren1.axis_y.guide_frequency = (float) (((double) componentInChildren1.axis_y.max_value - (double) componentInChildren1.axis_y.min_value) / 10.0);
    componentInChildren1.RefreshGuides();
    Tuple<float, float>[] points = statistic.value;
    GraphedLine graphedLine = componentInChildren2.NewLine(points, statistic.id);
    if (this.statColors.ContainsKey(statistic.id))
      componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color = this.statColors[statistic.id];
    graphedLine.line_renderer.color = componentInChildren2.line_formatting[componentInChildren2.line_formatting.Length - 1].color;
  }

  private RetiredColonyData.RetiredColonyStatistic GetStatistic(
    string id,
    RetiredColonyData data)
  {
    foreach (RetiredColonyData.RetiredColonyStatistic stat in data.Stats)
    {
      if (stat.id == id)
        return stat;
    }
    return (RetiredColonyData.RetiredColonyStatistic) null;
  }

  private void ToggleExplorer(bool active)
  {
    this.ConfigButtons();
    this.explorerRoot.SetActive(active);
    this.colonyDataRoot.SetActive(!active);
    if (!this.explorerGridConfigured)
    {
      this.explorerGridConfigured = true;
      this.StartCoroutine(this.ComputeSizeExplorerGrid());
    }
    this.explorerHeaderContainer.SetActive(active);
    this.colonyHeaderContainer.SetActive(!active);
    if (active)
      this.colonyDataRoot.transform.parent.rectTransform().SetPosition(new Vector3(this.colonyDataRoot.transform.parent.rectTransform().position.x, 0.0f, 0.0f));
    this.UpdateAchievementData((RetiredColonyData) null, (string[]) null);
    this.explorerSearch.text = string.Empty;
  }

  private void LoadExplorer()
  {
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
      return;
    this.ToggleExplorer(true);
    this.retiredColonyData = RetireColonyUtility.LoadRetiredColonies(false);
    foreach (RetiredColonyData retiredColonyData in this.retiredColonyData)
    {
      RetiredColonyData data = retiredColonyData;
      GameObject gameObject = Util.KInstantiateUI(this.colonyButtonPrefab, this.explorerGrid, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      Sprite sprite = RetireColonyUtility.LoadRetiredColonyPreview(RetireColonyUtility.StripInvalidCharacters(data.colonyName));
      Image reference1 = component.GetReference<Image>("ColonyImage");
      RectTransform reference2 = component.GetReference<RectTransform>("PreviewUnavailableText");
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
      {
        reference1.enabled = true;
        reference1.sprite = sprite;
        reference2.gameObject.SetActive(false);
      }
      else
      {
        reference1.enabled = false;
        reference2.gameObject.SetActive(true);
      }
      component.GetReference<LocText>("ColonyNameLabel").SetText(retiredColonyData.colonyName);
      component.GetReference<LocText>("CycleCountLabel").SetText(string.Format((string) STRINGS.UI.RETIRED_COLONY_INFO_SCREEN.CYCLE_COUNT, (object) retiredColonyData.cycleCount.ToString()));
      component.GetReference<LocText>("DateLabel").SetText(retiredColonyData.date);
      gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.LoadColony(data));
      string key = retiredColonyData.colonyName;
      int num = 0;
      for (; this.explorerColonyWidgets.ContainsKey(key); key = retiredColonyData.colonyName + "_" + (object) num)
        ++num;
      this.explorerColonyWidgets.Add(key, gameObject);
    }
  }

  private void FilterExplorer(string search)
  {
    foreach (KeyValuePair<string, GameObject> explorerColonyWidget in this.explorerColonyWidgets)
    {
      if (string.IsNullOrEmpty(search) || explorerColonyWidget.Key.ToUpper().Contains(search.ToUpper()))
        explorerColonyWidget.Value.SetActive(true);
      else
        explorerColonyWidget.Value.SetActive(false);
    }
  }

  private void FilterColonyData(string search)
  {
    foreach (KeyValuePair<string, GameObject> activeColonyWidget in this.activeColonyWidgets)
    {
      if (string.IsNullOrEmpty(search) || activeColonyWidget.Key.ToUpper().Contains(search.ToUpper()))
        activeColonyWidget.Value.SetActive(true);
      else
        activeColonyWidget.Value.SetActive(false);
    }
  }

  private void FilterAchievements(string search)
  {
    foreach (KeyValuePair<string, GameObject> achievementEntry in this.achievementEntries)
    {
      if (string.IsNullOrEmpty(search) || Db.Get().ColonyAchievements.Get(achievementEntry.Key).Name.ToUpper().Contains(search.ToUpper()))
        achievementEntry.Value.SetActive(true);
      else
        achievementEntry.Value.SetActive(false);
    }
  }
}
