// Decompiled with JetBrains decompiler
// Type: StarmapScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class StarmapScreen : KModalScreen
{
  private Dictionary<Spacecraft, HierarchyReferences> listRocketRows = new Dictionary<Spacecraft, HierarchyReferences>();
  private int rocketConditionEventHandler = -1;
  private List<GameObject> planetRows = new List<GameObject>();
  private Dictionary<SpaceDestination, StarmapPlanet> planetWidgets = new Dictionary<SpaceDestination, StarmapPlanet>();
  private float planetsMaxDistance = 1f;
  private int distanceOverlayVerticalOffset = 500;
  private int distanceOverlayYOffset = 24;
  private int selectionUpdateHandle = -1;
  private bool forceScrollDown = true;
  public GameObject listPanel;
  public GameObject rocketPanel;
  public LocText listHeaderLabel;
  public LocText listHeaderStatusLabel;
  public HierarchyReferences listRocketTemplate;
  public LocText listNoRocketText;
  public RectTransform rocketListContainer;
  [Header("Shared References")]
  public BreakdownList breakdownListPrefab;
  public GameObject progressBarPrefab;
  [Header("Selected Rocket References")]
  public LocText rocketHeaderLabel;
  public LocText rocketHeaderStatusLabel;
  private BreakdownList rocketDetailsStatus;
  public Sprite rocketDetailsStatusIcon;
  private BreakdownList rocketDetailsChecklist;
  public Sprite rocketDetailsChecklistIcon;
  private BreakdownList rocketDetailsMass;
  public Sprite rocketDetailsMassIcon;
  private BreakdownList rocketDetailsRange;
  public Sprite rocketDetailsRangeIcon;
  public RocketThrustWidget rocketThrustWidget;
  private BreakdownList rocketDetailsStorage;
  public Sprite rocketDetailsStorageIcon;
  private BreakdownList rocketDetailsDupes;
  public Sprite rocketDetailsDupesIcon;
  private BreakdownList rocketDetailsFuel;
  public Sprite rocketDetailsFuelIcon;
  private BreakdownList rocketDetailsOxidizer;
  public Sprite rocketDetailsOxidizerIcon;
  public RectTransform rocketDetailsContainer;
  [Header("Selected Destination References")]
  public LocText destinationHeaderLabel;
  public LocText destinationStatusLabel;
  public LocText destinationNameLabel;
  public LocText destinationTypeNameLabel;
  public LocText destinationTypeValueLabel;
  public LocText destinationDistanceNameLabel;
  public LocText destinationDistanceValueLabel;
  public LocText destinationDescriptionLabel;
  private BreakdownList destinationDetailsAnalysis;
  private GenericUIProgressBar destinationAnalysisProgressBar;
  public Sprite destinationDetailsAnalysisIcon;
  private BreakdownList destinationDetailsResearch;
  public Sprite destinationDetailsResearchIcon;
  private BreakdownList destinationDetailsMass;
  public Sprite destinationDetailsMassIcon;
  private BreakdownList destinationDetailsComposition;
  public Sprite destinationDetailsCompositionIcon;
  private BreakdownList destinationDetailsResources;
  public Sprite destinationDetailsResourcesIcon;
  private BreakdownList destinationDetailsArtifacts;
  public Sprite destinationDetailsArtifactsIcon;
  public RectTransform destinationDetailsContainer;
  public MultiToggle showRocketsButton;
  public MultiToggle launchButton;
  public MultiToggle analyzeButton;
  [Header("Map References")]
  public RectTransform Map;
  public RectTransform rowsContiner;
  public GameObject rowPrefab;
  public StarmapPlanet planetPrefab;
  public GameObject rocketIconPrefab;
  public Image distanceOverlay;
  public Image visualizeRocketImage;
  public Image visualizeRocketTrajectory;
  public LocText visualizeRocketLabel;
  public LocText visualizeRocketProgress;
  public Color[] distanceColors;
  public LocText titleBarLabel;
  public KButton button;
  private const int DESTINATION_ICON_SCALE = 2;
  public static StarmapScreen Instance;
  private SpaceDestination selectedDestination;
  private KSelectable currentSelectable;
  private CommandModule currentCommandModule;
  private LaunchConditionManager currentLaunchConditionManager;
  private bool currentRocketHasGasContainer;
  private bool currentRocketHasLiquidContainer;
  private bool currentRocketHasSolidContainer;
  private bool currentRocketHasEntitiesContainer;
  private Coroutine animateAnalysisRoutine;
  private Coroutine animateSelectedPlanetRoutine;
  private BreakdownListRow rangeRowTotal;

  public static void DestroyInstance()
  {
    StarmapScreen.Instance = (StarmapScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    this.rocketDetailsStatus = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsStatus.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.MISSIONSTATUS);
    this.rocketDetailsStatus.SetIcon(this.rocketDetailsStatusIcon);
    this.rocketDetailsStatus.gameObject.name = "rocketDetailsStatus";
    this.rocketDetailsChecklist = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsChecklist.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.LAUNCHCHECKLIST);
    this.rocketDetailsChecklist.SetIcon(this.rocketDetailsChecklistIcon);
    this.rocketDetailsChecklist.gameObject.name = "rocketDetailsChecklist";
    this.rocketDetailsRange = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsRange.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.MAXRANGE);
    this.rocketDetailsRange.SetIcon(this.rocketDetailsRangeIcon);
    this.rocketDetailsRange.gameObject.name = "rocketDetailsRange";
    this.rocketDetailsMass = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsMass.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.MASS);
    this.rocketDetailsMass.SetIcon(this.rocketDetailsMassIcon);
    this.rocketDetailsMass.gameObject.name = "rocketDetailsMass";
    this.rocketThrustWidget = UnityEngine.Object.Instantiate<RocketThrustWidget>(this.rocketThrustWidget, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsStorage = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsStorage.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.STORAGE);
    this.rocketDetailsStorage.SetIcon(this.rocketDetailsStorageIcon);
    this.rocketDetailsStorage.gameObject.name = "rocketDetailsStorage";
    this.rocketDetailsFuel = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsFuel.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.FUEL);
    this.rocketDetailsFuel.SetIcon(this.rocketDetailsFuelIcon);
    this.rocketDetailsFuel.gameObject.name = "rocketDetailsFuel";
    this.rocketDetailsOxidizer = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsOxidizer.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.OXIDIZER);
    this.rocketDetailsOxidizer.SetIcon(this.rocketDetailsOxidizerIcon);
    this.rocketDetailsOxidizer.gameObject.name = "rocketDetailsOxidizer";
    this.rocketDetailsDupes = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.rocketDetailsContainer);
    this.rocketDetailsDupes.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.PASSENGERS);
    this.rocketDetailsDupes.SetIcon(this.rocketDetailsDupesIcon);
    this.rocketDetailsDupes.gameObject.name = "rocketDetailsDupes";
    this.destinationDetailsAnalysis = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsAnalysis.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.ANALYSIS);
    this.destinationDetailsAnalysis.SetIcon(this.destinationDetailsAnalysisIcon);
    this.destinationDetailsAnalysis.gameObject.name = "destinationDetailsAnalysis";
    this.destinationDetailsAnalysis.SetDescription(string.Format((string) STRINGS.UI.STARMAP.ANALYSIS_DESCRIPTION, (object) 0));
    this.destinationAnalysisProgressBar = UnityEngine.Object.Instantiate<GameObject>(this.progressBarPrefab.gameObject, (Transform) this.destinationDetailsContainer).GetComponent<GenericUIProgressBar>();
    this.destinationAnalysisProgressBar.SetMaxValue((float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
    this.destinationDetailsResearch = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsResearch.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.RESEARCH);
    this.destinationDetailsResearch.SetIcon(this.destinationDetailsResearchIcon);
    this.destinationDetailsResearch.gameObject.name = "destinationDetailsResearch";
    this.destinationDetailsResearch.SetDescription(string.Format((string) STRINGS.UI.STARMAP.RESEARCH_DESCRIPTION, (object) 0));
    this.destinationDetailsMass = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsMass.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.DESTINATION_MASS);
    this.destinationDetailsMass.SetIcon(this.destinationDetailsMassIcon);
    this.destinationDetailsMass.gameObject.name = "destinationDetailsMass";
    this.destinationDetailsComposition = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsComposition.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.WORLDCOMPOSITION);
    this.destinationDetailsComposition.SetIcon(this.destinationDetailsCompositionIcon);
    this.destinationDetailsComposition.gameObject.name = "destinationDetailsComposition";
    this.destinationDetailsResources = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsResources.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.RESOURCES);
    this.destinationDetailsResources.SetIcon(this.destinationDetailsResourcesIcon);
    this.destinationDetailsResources.gameObject.name = "destinationDetailsResources";
    this.destinationDetailsArtifacts = UnityEngine.Object.Instantiate<BreakdownList>(this.breakdownListPrefab, (Transform) this.destinationDetailsContainer);
    this.destinationDetailsArtifacts.SetTitle((string) STRINGS.UI.STARMAP.LISTTITLES.ARTIFACTS);
    this.destinationDetailsArtifacts.SetIcon(this.destinationDetailsArtifactsIcon);
    this.destinationDetailsArtifacts.gameObject.name = "destinationDetailsArtifacts";
    this.LoadPlanets();
    this.selectionUpdateHandle = Game.Instance.Subscribe(-1503271301, new System.Action<object>(this.OnSelectableChanged));
    this.titleBarLabel.text = (string) STRINGS.UI.STARMAP.TITLE;
    this.button.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleStarmap());
    this.launchButton.play_sound_on_click = false;
    this.launchButton.onClick += (System.Action) (() =>
    {
      if ((UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null && this.selectedDestination != null)
      {
        KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
        this.LaunchRocket(this.currentLaunchConditionManager);
      }
      else
        KFMOD.PlayOneShot(GlobalAssets.GetSound("Negative", false));
    });
    this.launchButton.ChangeState(1);
    this.showRocketsButton.onClick += (System.Action) (() => this.OnSelectableChanged((object) null));
    this.SelectDestination((SpaceDestination) null);
    SpacecraftManager.instance.Subscribe(532901469, (System.Action<object>) (data =>
    {
      this.RefreshAnalyzeButton();
      this.UpdateDestinationStates();
    }));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.selectionUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.selectionUpdateHandle);
    this.StopAllCoroutines();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapSnapshot);
      MusicManager.instance.PlaySong("Music_Starmap", false);
      this.UpdateDestinationStates();
      this.Refresh((object) null);
    }
    else
    {
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MENUStarmapSnapshot, STOP_MODE.ALLOWFADEOUT);
      MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
    }
    this.OnSelectableChanged(!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null) ? (object) SelectTool.Instance.selected.gameObject : (object) (GameObject) null);
    this.forceScrollDown = true;
  }

  private void UpdateDestinationStates()
  {
    int a1 = 0;
    int a2 = 0;
    int num = 1;
    foreach (SpaceDestination destination in SpacecraftManager.instance.destinations)
    {
      a1 = Mathf.Max(a1, destination.OneBasedDistance);
      if (destination.AnalysisState() == SpacecraftManager.DestinationAnalysisState.Complete)
        a2 = Mathf.Max(a2, destination.OneBasedDistance);
    }
    for (int index = a2; index < a1; ++index)
    {
      bool flag = false;
      foreach (SpaceDestination destination in SpacecraftManager.instance.destinations)
      {
        if (destination.distance == index)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        ++num;
      else
        break;
    }
    foreach (KeyValuePair<SpaceDestination, StarmapPlanet> planetWidget in this.planetWidgets)
    {
      KeyValuePair<SpaceDestination, StarmapPlanet> KVP = planetWidget;
      StarmapScreen starmapScreen = this;
      SpaceDestination key = KVP.Key;
      StarmapPlanet planet = KVP.Value;
      Color color1 = new Color(0.25f, 0.25f, 0.25f, 0.5f);
      Color color2 = new Color(0.75f, 0.75f, 0.75f, 0.75f);
      if (KVP.Key.distance >= a2 + num)
      {
        planet.SetUnknownBGActive(false, Color.white);
        planet.SetSprite(Assets.GetSprite((HashedString) "unknown_far"), color1);
      }
      else
      {
        planet.SetAnalysisActive(SpacecraftManager.instance.GetStarmapAnalysisDestinationID() == KVP.Key.id);
        bool flag = SpacecraftManager.instance.GetDestinationAnalysisState(key) == SpacecraftManager.DestinationAnalysisState.Complete;
        SpaceDestinationType destinationType = key.GetDestinationType();
        planet.SetLabel(!flag ? (string) STRINGS.UI.STARMAP.UNKNOWN_DESTINATION + "\n" + string.Format(STRINGS.UI.STARMAP.ANALYSIS_AMOUNT.text, (object) GameUtil.GetFormattedPercent((float) (100.0 * ((double) SpacecraftManager.instance.GetDestinationAnalysisScore(KVP.Key) / (double) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE)), GameUtil.TimeSlice.None)) : destinationType.Name + "\n<color=#979798> " + GameUtil.GetFormattedDistance((float) ((double) KVP.Key.OneBasedDistance * 10000.0 * 1000.0)) + "</color>");
        planet.SetSprite(!flag ? Assets.GetSprite((HashedString) "unknown") : Assets.GetSprite((HashedString) destinationType.spriteName), !flag ? color2 : Color.white);
        planet.SetUnknownBGActive(SpacecraftManager.instance.GetDestinationAnalysisState(KVP.Key) != SpacecraftManager.DestinationAnalysisState.Complete, color2);
        planet.SetFillAmount(SpacecraftManager.instance.GetDestinationAnalysisScore(KVP.Key) / (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
        List<int> spacecraftsForDestination = SpacecraftManager.instance.GetSpacecraftsForDestination(key);
        planet.SetRocketIcons(spacecraftsForDestination.Count, this.rocketIconPrefab);
        bool show = (UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null && key == SpacecraftManager.instance.GetSpacecraftDestination(this.currentLaunchConditionManager);
        planet.ShowAsCurrentRocketDestination(show);
        planet.SetOnClick((System.Action) (() =>
        {
          if ((UnityEngine.Object) closure_0.currentLaunchConditionManager == (UnityEngine.Object) null)
          {
            closure_0.SelectDestination(KVP.Key);
          }
          else
          {
            if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(closure_0.currentLaunchConditionManager).state != Spacecraft.MissionState.Grounded)
              return;
            closure_0.SelectDestination(KVP.Key);
          }
        }));
        planet.SetOnEnter((System.Action) (() => planet.ShowLabel(true)));
        planet.SetOnExit((System.Action) (() => planet.ShowLabel(false)));
      }
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    StarmapScreen.Instance = this;
  }

  private string DisplayDistance(float distance)
  {
    return Util.FormatWholeNumber(distance) + " " + (string) STRINGS.UI.UNITSUFFIXES.DISTANCE.KILOMETER;
  }

  private string DisplayDestinationMass(SpaceDestination selectedDestination)
  {
    return GameUtil.GetFormattedMass(selectedDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}");
  }

  private string DisplayTotalStorageCapacity(CommandModule command)
  {
    float mass = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(command.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        mass += component.storage.Capacity();
    }
    return GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}");
  }

  private string StorageCapacityTooltip(CommandModule command, SpaceDestination dest)
  {
    string str = string.Empty;
    bool flag = dest != null && SpacecraftManager.instance.GetDestinationAnalysisState(dest) == SpacecraftManager.DestinationAnalysisState.Complete;
    if (dest != null && flag)
    {
      if ((double) dest.AvailableMass <= (double) ConditionHasMinimumMass.CargoCapacity(dest, command))
        str = str + (string) STRINGS.UI.STARMAP.LAUNCHCHECKLIST.INSUFFICENT_MASS_TOOLTIP + STRINGS.UI.HORIZONTAL_BR_RULE;
      str = str + string.Format((string) STRINGS.UI.STARMAP.LAUNCHCHECKLIST.RESOURCE_MASS_TOOLTIP, (object) dest.GetDestinationType().Name, (object) GameUtil.GetFormattedMass(dest.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(ConditionHasMinimumMass.CargoCapacity(dest, command), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")) + "\n\n";
    }
    float num = dest == null ? 0.0f : dest.AvailableMass;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(command.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (flag)
        {
          float resourcesPercentage = dest.GetAvailableResourcesPercentage(component.storageType);
          float a = Mathf.Min(component.storage.Capacity(), resourcesPercentage * num);
          num -= a;
          str = str + component.gameObject.GetProperName() + " " + string.Format((string) STRINGS.UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(Mathf.Min(a, component.storage.Capacity()), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")) + "\n";
        }
        else
          str = str + component.gameObject.GetProperName() + " " + string.Format((string) STRINGS.UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(0.0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")) + "\n";
      }
    }
    return str;
  }

  private void LoadPlanets()
  {
    foreach (SpaceDestination destination in Game.Instance.spacecraftManager.destinations)
    {
      if ((double) destination.OneBasedDistance * 10000.0 > (double) this.planetsMaxDistance)
        this.planetsMaxDistance = (float) destination.OneBasedDistance * 10000f;
      while (this.planetRows.Count < destination.distance + 1)
      {
        GameObject go = Util.KInstantiateUI(this.rowPrefab, this.rowsContiner.gameObject, true);
        go.rectTransform().SetAsFirstSibling();
        this.planetRows.Add(go);
        go.GetComponentInChildren<Image>().color = this.distanceColors[this.planetRows.Count % this.distanceColors.Length];
        go.GetComponentInChildren<LocText>().text = this.DisplayDistance((float) (this.planetRows.Count + 1) * 10000f);
      }
      GameObject gameObject = Util.KInstantiateUI(this.planetPrefab.gameObject, this.planetRows[destination.distance], true);
      this.planetWidgets.Add(destination, gameObject.GetComponent<StarmapPlanet>());
    }
    this.UpdateDestinationStates();
  }

  private void UnselectAllPlanets()
  {
    if (this.animateSelectedPlanetRoutine != null)
      this.StopCoroutine(this.animateSelectedPlanetRoutine);
    foreach (KeyValuePair<SpaceDestination, StarmapPlanet> planetWidget in this.planetWidgets)
    {
      planetWidget.Value.SetSelectionActive(false);
      planetWidget.Value.ShowAsCurrentRocketDestination(false);
    }
  }

  private void SelectPlanet(StarmapPlanet planet)
  {
    planet.SetSelectionActive(true);
    if (this.animateSelectedPlanetRoutine != null)
      this.StopCoroutine(this.animateSelectedPlanetRoutine);
    this.animateSelectedPlanetRoutine = this.StartCoroutine(this.AnimatePlanetSelection(planet));
  }

  [DebuggerHidden]
  private IEnumerator AnimatePlanetSelection(StarmapPlanet planet)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new StarmapScreen.\u003CAnimatePlanetSelection\u003Ec__Iterator0()
    {
      planet = planet
    };
  }

  private void Update()
  {
    this.PositionPlanetWidgets();
    if (!this.forceScrollDown)
      return;
    this.ScrollToBottom();
    this.forceScrollDown = false;
  }

  private void ScrollToBottom()
  {
    RectTransform transform = this.Map.GetComponentInChildren<VerticalLayoutGroup>().rectTransform();
    transform.SetLocalPosition(new Vector3(transform.localPosition.x, transform.rect.height - this.Map.rect.height, transform.localPosition.z));
  }

  public override float GetSortKey()
  {
    return 100f;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && (e.TryConsume(Action.MouseRight) || e.TryConsume(Action.Escape)))
      ManagementMenu.Instance.CloseAll();
    else if (this.CheckBlockedInput())
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
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
    {
      GameObject selectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject != (UnityEngine.Object) null)
      {
        foreach (KeyValuePair<Spacecraft, HierarchyReferences> listRocketRow in this.listRocketRows)
        {
          EditableTitleBar component = listRocketRow.Value.GetReference<RectTransform>("EditableTitle").GetComponent<EditableTitleBar>();
          if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) component.inputField.gameObject)
            return true;
        }
      }
    }
    return false;
  }

  private void PositionPlanetWidgets()
  {
    float num = this.rowPrefab.GetComponent<RectTransform>().rect.height / 2f;
    foreach (KeyValuePair<SpaceDestination, StarmapPlanet> planetWidget in this.planetWidgets)
      planetWidget.Value.rectTransform().anchoredPosition = new Vector2(planetWidget.Value.transform.parent.rectTransform().sizeDelta.x * planetWidget.Key.startingOrbitPercentage, -num);
  }

  private void OnSelectableChanged(object data)
  {
    if (!this.gameObject.activeSelf)
      return;
    if (this.rocketConditionEventHandler != -1)
      this.Unsubscribe(this.rocketConditionEventHandler);
    if (data != null)
    {
      this.currentSelectable = ((GameObject) data).GetComponent<KSelectable>();
      this.currentCommandModule = this.currentSelectable.GetComponent<CommandModule>();
      this.currentLaunchConditionManager = this.currentSelectable.GetComponent<LaunchConditionManager>();
      if ((UnityEngine.Object) this.currentCommandModule != (UnityEngine.Object) null && (UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null)
      {
        this.SelectDestination(SpacecraftManager.instance.GetSpacecraftDestination(this.currentLaunchConditionManager));
        this.rocketConditionEventHandler = this.currentLaunchConditionManager.Subscribe(1655598572, new System.Action<object>(this.Refresh));
        this.ShowRocketDetailsPanel();
      }
      else
      {
        this.currentSelectable = (KSelectable) null;
        this.currentCommandModule = (CommandModule) null;
        this.currentLaunchConditionManager = (LaunchConditionManager) null;
        this.ShowRocketListPanel();
      }
    }
    else
    {
      this.currentSelectable = (KSelectable) null;
      this.currentCommandModule = (CommandModule) null;
      this.currentLaunchConditionManager = (LaunchConditionManager) null;
      this.ShowRocketListPanel();
    }
    this.Refresh((object) null);
  }

  private void ShowRocketListPanel()
  {
    this.listPanel.SetActive(true);
    this.rocketPanel.SetActive(false);
    this.launchButton.ChangeState(1);
    this.UpdateDistanceOverlay((LaunchConditionManager) null);
    this.UpdateMissionOverlay((LaunchConditionManager) null);
  }

  private void ShowRocketDetailsPanel()
  {
    this.listPanel.SetActive(false);
    this.rocketPanel.SetActive(true);
    this.ValidateTravelAbility();
    this.UpdateDistanceOverlay((LaunchConditionManager) null);
    this.UpdateMissionOverlay((LaunchConditionManager) null);
  }

  private void LaunchRocket(LaunchConditionManager lcm)
  {
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(lcm);
    if (spacecraftDestination == null)
      return;
    lcm.Launch(spacecraftDestination);
    this.ClearRocketListPanel();
    this.FillRocketListPanel();
    this.ShowRocketListPanel();
    this.Refresh((object) null);
  }

  private void FillRocketListPanel()
  {
    this.ClearRocketListPanel();
    List<Spacecraft> spacecraft1 = SpacecraftManager.instance.GetSpacecraft();
    if (spacecraft1.Count == 0)
    {
      this.listHeaderStatusLabel.text = (string) STRINGS.UI.STARMAP.NO_ROCKETS_TITLE;
      this.listNoRocketText.gameObject.SetActive(true);
    }
    else
    {
      this.listHeaderStatusLabel.text = string.Format((string) STRINGS.UI.STARMAP.ROCKET_COUNT, (object) spacecraft1.Count);
      this.listNoRocketText.gameObject.SetActive(false);
    }
    foreach (Spacecraft spacecraft2 in spacecraft1)
    {
      Spacecraft rocket = spacecraft2;
      StarmapScreen starmapScreen = this;
      HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.listRocketTemplate.gameObject, this.rocketListContainer.gameObject, true);
      BreakdownList component1 = hierarchyReferences.GetComponent<BreakdownList>();
      MultiToggle component2 = hierarchyReferences.GetComponent<MultiToggle>();
      EditableTitleBar component3 = hierarchyReferences.GetReference<RectTransform>("EditableTitle").GetComponent<EditableTitleBar>();
      MultiToggle component4 = hierarchyReferences.GetReference<RectTransform>("LaunchRocketButton").GetComponent<MultiToggle>();
      MultiToggle component5 = hierarchyReferences.GetReference<RectTransform>("LandRocketButton").GetComponent<MultiToggle>();
      HierarchyReferences component6 = hierarchyReferences.GetReference<RectTransform>("ProgressBar").GetComponent<HierarchyReferences>();
      LaunchConditionManager launchConditionManager = rocket.launchConditions;
      CommandModule component7 = launchConditionManager.GetComponent<CommandModule>();
      MinionStorage component8 = launchConditionManager.GetComponent<MinionStorage>();
      component3.SetTitle(rocket.rocketName);
      component3.OnNameChanged += (System.Action<string>) (newName => rocket.SetRocketName(newName));
      component2.onEnter += (System.Action) (() =>
      {
        LaunchConditionManager launchConditions = rocket.launchConditions;
        closure_0.UpdateDistanceOverlay(launchConditions);
        closure_0.UpdateMissionOverlay(launchConditions);
      });
      component2.onExit += (System.Action) (() =>
      {
        closure_0.UpdateDistanceOverlay((LaunchConditionManager) null);
        closure_0.UpdateMissionOverlay((LaunchConditionManager) null);
      });
      component2.onClick += (System.Action) (() => closure_0.OnSelectableChanged((object) rocket.launchConditions.gameObject));
      component4.play_sound_on_click = false;
      component4.onClick += (System.Action) (() =>
      {
        if ((UnityEngine.Object) launchConditionManager != (UnityEngine.Object) null)
        {
          KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
          closure_0.LaunchRocket(launchConditionManager);
        }
        else
          KFMOD.PlayOneShot(GlobalAssets.GetSound("Negative", false));
      });
      if ((DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive) && SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(launchConditionManager).state != Spacecraft.MissionState.Grounded)
      {
        component5.gameObject.SetActive(true);
        component5.transform.SetAsLastSibling();
        component5.play_sound_on_click = false;
        component5.onClick += (System.Action) (() =>
        {
          if ((UnityEngine.Object) launchConditionManager != (UnityEngine.Object) null)
          {
            KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
            SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(launchConditionManager).ForceComplete();
            closure_0.ClearRocketListPanel();
            closure_0.FillRocketListPanel();
            closure_0.ShowRocketListPanel();
            closure_0.Refresh((object) null);
          }
          else
            KFMOD.PlayOneShot(GlobalAssets.GetSound("Negative", false));
        });
      }
      else
        component5.gameObject.SetActive(false);
      BreakdownListRow breakdownListRow1 = component1.AddRow();
      string str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.GROUNDED;
      BreakdownListRow.Status dotColor = BreakdownListRow.Status.Green;
      switch (rocket.state)
      {
        case Spacecraft.MissionState.Grounded:
          dotColor = BreakdownListRow.Status.Green;
          str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.GROUNDED;
          break;
        case Spacecraft.MissionState.Launching:
          str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.LAUNCHING;
          dotColor = BreakdownListRow.Status.Yellow;
          break;
        case Spacecraft.MissionState.Underway:
          dotColor = BreakdownListRow.Status.Red;
          str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.UNDERWAY;
          break;
        case Spacecraft.MissionState.WaitingToLand:
          dotColor = BreakdownListRow.Status.Yellow;
          str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.WAITING_TO_LAND;
          break;
        case Spacecraft.MissionState.Landing:
          dotColor = BreakdownListRow.Status.Yellow;
          str = (string) STRINGS.UI.STARMAP.MISSION_STATUS.LANDING;
          break;
      }
      breakdownListRow1.ShowStatusData((string) STRINGS.UI.STARMAP.ROCKETSTATUS.STATUS, str, dotColor);
      breakdownListRow1.SetHighlighted(true);
      if ((UnityEngine.Object) component8 != (UnityEngine.Object) null)
      {
        List<MinionStorage.Info> storedMinionInfo = component8.GetStoredMinionInfo();
        BreakdownListRow breakdownListRow2 = component1.AddRow();
        int count = storedMinionInfo.Count;
        breakdownListRow2.ShowStatusData((string) STRINGS.UI.STARMAP.LISTTITLES.PASSENGERS, count.ToString(), count != 0 ? BreakdownListRow.Status.Green : BreakdownListRow.Status.Red);
      }
      if (rocket.state == Spacecraft.MissionState.Grounded)
      {
        string tooltipText = string.Empty;
        List<GameObject> attachedNetwork = AttachableBuilding.GetAttachedNetwork(launchConditionManager.GetComponent<AttachableBuilding>());
        foreach (GameObject go in attachedNetwork)
          tooltipText = tooltipText + go.GetProperName() + "\n";
        BreakdownListRow breakdownListRow2 = component1.AddRow();
        breakdownListRow2.ShowData((string) STRINGS.UI.STARMAP.LISTTITLES.MODULES, attachedNetwork.Count.ToString());
        breakdownListRow2.AddTooltip(tooltipText);
        component1.AddRow().ShowData((string) STRINGS.UI.STARMAP.LISTTITLES.MAXRANGE, this.DisplayDistance(component7.rocketStats.GetRocketMaxDistance()));
        BreakdownListRow breakdownListRow3 = component1.AddRow();
        breakdownListRow3.ShowData((string) STRINGS.UI.STARMAP.LISTTITLES.STORAGE, this.DisplayTotalStorageCapacity(component7));
        breakdownListRow3.AddTooltip(this.StorageCapacityTooltip(component7, this.selectedDestination));
        BreakdownListRow breakdownListRow4 = component1.AddRow();
        if (this.selectedDestination != null)
        {
          if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
          {
            bool flag = (double) this.selectedDestination.AvailableMass >= (double) ConditionHasMinimumMass.CargoCapacity(this.selectedDestination, component7);
            breakdownListRow4.ShowStatusData((string) STRINGS.UI.STARMAP.LISTTITLES.DESTINATION_MASS, this.DisplayDestinationMass(this.selectedDestination), !flag ? BreakdownListRow.Status.Yellow : BreakdownListRow.Status.Default);
            breakdownListRow4.AddTooltip(this.StorageCapacityTooltip(component7, this.selectedDestination));
          }
          else
            breakdownListRow4.ShowStatusData((string) STRINGS.UI.STARMAP.LISTTITLES.DESTINATION_MASS, (string) STRINGS.UI.STARMAP.COMPOSITION_UNDISCOVERED_AMOUNT, BreakdownListRow.Status.Default);
        }
        else
        {
          breakdownListRow4.ShowStatusData((string) STRINGS.UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED, string.Empty, BreakdownListRow.Status.Red);
          breakdownListRow4.AddTooltip((string) STRINGS.UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED);
        }
        component4.GetComponent<RectTransform>().SetAsLastSibling();
        component4.gameObject.SetActive(true);
        component6.gameObject.SetActive(false);
      }
      else
      {
        float duration = rocket.GetDuration();
        float timeLeft = rocket.GetTimeLeft();
        float x = (double) duration != 0.0 ? (float) (1.0 - (double) timeLeft / (double) duration) : 0.0f;
        component1.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATUS.TIMEREMAINING, Util.FormatOneDecimalPlace(timeLeft / 600f) + " / " + GameUtil.GetFormattedCycles(duration, "F1"));
        component6.gameObject.SetActive(true);
        RectTransform reference = component6.GetReference<RectTransform>("ProgressImage");
        LocText component9 = component6.GetReference<RectTransform>("ProgressText").GetComponent<LocText>();
        reference.transform.localScale = new Vector3(x, 1f, 1f);
        component9.text = GameUtil.GetFormattedPercent(x * 100f, GameUtil.TimeSlice.None);
        component6.GetComponent<RectTransform>().SetAsLastSibling();
        component4.gameObject.SetActive(false);
      }
      this.listRocketRows.Add(rocket, hierarchyReferences);
    }
    this.UpdateRocketRowsTravelAbility();
  }

  private void ClearRocketListPanel()
  {
    this.listHeaderStatusLabel.text = (string) STRINGS.UI.STARMAP.NO_ROCKETS_TITLE;
    foreach (KeyValuePair<Spacecraft, HierarchyReferences> listRocketRow in this.listRocketRows)
      UnityEngine.Object.Destroy((UnityEngine.Object) listRocketRow.Value.gameObject);
    this.listRocketRows.Clear();
  }

  private void FillChecklist(LaunchConditionManager launchConditionManager)
  {
    foreach (RocketLaunchCondition launchCondition1 in launchConditionManager.GetLaunchConditionList())
    {
      BreakdownListRow breakdownListRow = this.rocketDetailsChecklist.AddRow();
      string launchStatusMessage = launchCondition1.GetLaunchStatusMessage(true);
      RocketLaunchCondition.LaunchStatus launchCondition2 = launchCondition1.EvaluateLaunchCondition();
      BreakdownListRow.Status status = BreakdownListRow.Status.Green;
      switch (launchCondition2)
      {
        case RocketLaunchCondition.LaunchStatus.Warning:
          status = BreakdownListRow.Status.Yellow;
          break;
        case RocketLaunchCondition.LaunchStatus.Failure:
          status = BreakdownListRow.Status.Red;
          break;
      }
      breakdownListRow.ShowCheckmarkData(launchStatusMessage, string.Empty, status);
      if (launchCondition2 != RocketLaunchCondition.LaunchStatus.Ready)
        breakdownListRow.SetHighlighted(true);
      breakdownListRow.AddTooltip(launchCondition1.GetLaunchStatusTooltip(launchCondition2 == RocketLaunchCondition.LaunchStatus.Failure));
    }
  }

  private void SelectDestination(SpaceDestination destination)
  {
    this.selectedDestination = destination;
    this.UnselectAllPlanets();
    if (this.selectedDestination != null)
    {
      this.SelectPlanet(this.planetWidgets[this.selectedDestination]);
      if ((UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null)
        SpacecraftManager.instance.SetSpacecraftDestination(this.currentLaunchConditionManager, this.selectedDestination);
      this.ShowDestinationPanel();
      this.UpdateRocketRowsTravelAbility();
    }
    else
      this.ClearDestinationPanel();
    if ((UnityEngine.Object) this.rangeRowTotal != (UnityEngine.Object) null && this.selectedDestination != null && (UnityEngine.Object) this.currentCommandModule != (UnityEngine.Object) null)
      this.rangeRowTotal.SetStatusColor(!this.currentCommandModule.reachable.CanReachDestination(this.selectedDestination) ? BreakdownListRow.Status.Red : BreakdownListRow.Status.Green);
    this.UpdateDestinationStates();
    this.Refresh((object) null);
  }

  private void UpdateRocketRowsTravelAbility()
  {
    foreach (KeyValuePair<Spacecraft, HierarchyReferences> listRocketRow in this.listRocketRows)
    {
      Spacecraft key = listRocketRow.Key;
      LaunchConditionManager launchConditions = key.launchConditions;
      CommandModule component1 = launchConditions.GetComponent<CommandModule>();
      MultiToggle component2 = listRocketRow.Value.GetReference<RectTransform>("LaunchRocketButton").GetComponent<MultiToggle>();
      bool flag1 = key.state == Spacecraft.MissionState.Grounded;
      SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(launchConditions);
      bool flag2 = spacecraftDestination != null && component1.reachable.CanReachDestination(spacecraftDestination);
      bool launch = launchConditions.CheckReadyToLaunch();
      component2.ChangeState(!flag1 || !flag2 || !launch ? 1 : 0);
    }
  }

  private void RefreshAnalyzeButton()
  {
    if (this.selectedDestination == null)
    {
      this.analyzeButton.ChangeState(1);
      this.analyzeButton.onClick = (System.Action) null;
      this.analyzeButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.STARMAP.NO_ANALYZABLE_DESTINATION_SELECTED;
    }
    else if (this.selectedDestination.AnalysisState() == SpacecraftManager.DestinationAnalysisState.Complete)
    {
      if (DebugHandler.InstantBuildMode)
      {
        this.analyzeButton.ChangeState(0);
        this.analyzeButton.onClick = (System.Action) (() =>
        {
          this.selectedDestination.TryCompleteResearchOpportunity();
          this.ShowDestinationPanel();
        });
        this.analyzeButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.STARMAP.ANALYSIS_COMPLETE + " (debug research)";
      }
      else
      {
        this.analyzeButton.ChangeState(1);
        this.analyzeButton.onClick = (System.Action) null;
        this.analyzeButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.STARMAP.ANALYSIS_COMPLETE;
      }
    }
    else
    {
      this.analyzeButton.ChangeState(0);
      if (this.selectedDestination.id == SpacecraftManager.instance.GetStarmapAnalysisDestinationID())
      {
        this.analyzeButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.STARMAP.SUSPEND_DESTINATION_ANALYSIS;
        this.analyzeButton.onClick = (System.Action) (() => SpacecraftManager.instance.SetStarmapAnalysisDestinationID(-1));
      }
      else
      {
        this.analyzeButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.STARMAP.ANALYZE_DESTINATION;
        this.analyzeButton.onClick = (System.Action) (() =>
        {
          if (DebugHandler.InstantBuildMode)
          {
            SpacecraftManager.instance.SetStarmapAnalysisDestinationID(this.selectedDestination.id);
            SpacecraftManager.instance.EarnDestinationAnalysisPoints(this.selectedDestination.id, 99999f);
            this.ShowDestinationPanel();
          }
          else
            SpacecraftManager.instance.SetStarmapAnalysisDestinationID(this.selectedDestination.id);
        });
      }
    }
  }

  private void Refresh(object data = null)
  {
    this.FillRocketListPanel();
    this.RefreshAnalyzeButton();
    this.ShowDestinationPanel();
    if ((UnityEngine.Object) this.currentCommandModule != (UnityEngine.Object) null && (UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null)
    {
      this.FillRocketPanel();
      if (this.selectedDestination == null)
        return;
      this.ValidateTravelAbility();
    }
    else
      this.ClearRocketPanel();
  }

  private void ClearRocketPanel()
  {
    this.rocketHeaderStatusLabel.text = (string) STRINGS.UI.STARMAP.ROCKETSTATUS.NONE;
    this.rocketDetailsChecklist.ClearRows();
    this.rocketDetailsMass.ClearRows();
    this.rocketDetailsRange.ClearRows();
    this.rocketThrustWidget.gameObject.SetActive(false);
    this.rocketDetailsStorage.ClearRows();
    this.rocketDetailsFuel.ClearRows();
    this.rocketDetailsOxidizer.ClearRows();
    this.rocketDetailsDupes.ClearRows();
    this.rocketDetailsStatus.ClearRows();
    this.currentRocketHasLiquidContainer = false;
    this.currentRocketHasGasContainer = false;
    this.currentRocketHasSolidContainer = false;
    this.currentRocketHasEntitiesContainer = false;
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.rocketDetailsContainer);
  }

  private void FillRocketPanel()
  {
    this.ClearRocketPanel();
    this.rocketHeaderStatusLabel.text = (string) STRINGS.UI.STARMAP.STATUS;
    this.UpdateDistanceOverlay((LaunchConditionManager) null);
    this.UpdateMissionOverlay((LaunchConditionManager) null);
    this.FillChecklist(this.currentLaunchConditionManager);
    this.UpdateRangeDisplay();
    this.UpdateMassDisplay();
    this.UpdateOxidizerDisplay();
    this.UpdateStorageDisplay();
    this.UpdateFuelDisplay();
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.rocketDetailsContainer);
  }

  private void UpdateRangeDisplay()
  {
    this.rocketDetailsRange.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_OXIDIZABLE_FUEL, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetTotalOxidizableFuel(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
    this.rocketDetailsRange.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.ENGINE_EFFICIENCY, GameUtil.GetFormattedEngineEfficiency(this.currentCommandModule.rocketStats.GetEngineEfficiency()));
    this.rocketDetailsRange.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.OXIDIZER_EFFICIENCY, GameUtil.GetFormattedPercent(this.currentCommandModule.rocketStats.GetAverageOxidizerEfficiency(), GameUtil.TimeSlice.None));
    float meters = this.currentCommandModule.rocketStats.GetBoosterThrust() * 1000f;
    if ((double) meters != 0.0)
      this.rocketDetailsRange.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.SOLID_BOOSTER, GameUtil.GetFormattedDistance(meters));
    BreakdownListRow breakdownListRow1 = this.rocketDetailsRange.AddRow();
    breakdownListRow1.ShowStatusData((string) STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_THRUST, GameUtil.GetFormattedDistance(this.currentCommandModule.rocketStats.GetTotalThrust() * 1000f), BreakdownListRow.Status.Green);
    breakdownListRow1.SetImportant(true);
    float distance = (float) -((double) this.currentCommandModule.rocketStats.GetTotalThrust() - (double) this.currentCommandModule.rocketStats.GetRocketMaxDistance());
    this.rocketThrustWidget.gameObject.SetActive(true);
    BreakdownListRow breakdownListRow2 = this.rocketDetailsRange.AddRow();
    breakdownListRow2.ShowStatusData((string) STRINGS.UI.STARMAP.ROCKETSTATUS.WEIGHTPENALTY, this.DisplayDistance(distance), BreakdownListRow.Status.Red);
    breakdownListRow2.SetHighlighted(true);
    this.rocketDetailsRange.AddCustomRow(this.rocketThrustWidget.gameObject);
    this.rocketThrustWidget.Draw(this.currentCommandModule);
    BreakdownListRow breakdownListRow3 = this.rocketDetailsRange.AddRow();
    breakdownListRow3.ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_RANGE, GameUtil.GetFormattedDistance(this.currentCommandModule.rocketStats.GetRocketMaxDistance() * 1000f));
    breakdownListRow3.SetImportant(true);
  }

  private void UpdateMassDisplay()
  {
    this.rocketDetailsMass.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.DRY_MASS, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetDryMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    this.rocketDetailsMass.AddRow().ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.WET_MASS, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetWetMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    BreakdownListRow breakdownListRow = this.rocketDetailsMass.AddRow();
    breakdownListRow.ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATUS.TOTAL, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetTotalMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    breakdownListRow.SetImportant(true);
  }

  private void UpdateFuelDisplay()
  {
    Tag engineFuelTag = this.currentCommandModule.rocketStats.GetEngineFuelTag();
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.currentCommandModule.GetComponent<AttachableBuilding>()))
    {
      FuelTank component1 = gameObject.GetComponent<FuelTank>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        BreakdownListRow breakdownListRow = this.rocketDetailsFuel.AddRow();
        if (engineFuelTag.IsValid)
        {
          Element element = ElementLoader.GetElement(engineFuelTag);
          Debug.Assert(element != null, (object) "fuel_element");
          breakdownListRow.ShowData(gameObject.gameObject.GetProperName() + " (" + element.name + ")", GameUtil.GetFormattedMass(component1.GetAmountAvailable(engineFuelTag), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
        }
        else
        {
          breakdownListRow.ShowData(gameObject.gameObject.GetProperName(), (string) STRINGS.UI.STARMAP.ROCKETSTATS.NO_ENGINE);
          breakdownListRow.SetStatusColor(BreakdownListRow.Status.Red);
        }
      }
      SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        BreakdownListRow breakdownListRow = this.rocketDetailsFuel.AddRow();
        Element element = ElementLoader.GetElement(component2.fuelTag);
        Debug.Assert(element != null, (object) "fuel_element");
        breakdownListRow.ShowData(gameObject.gameObject.GetProperName() + " (" + element.name + ")", GameUtil.GetFormattedMass(component2.fuelStorage.GetMassAvailable(component2.fuelTag), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
      }
    }
    BreakdownListRow breakdownListRow1 = this.rocketDetailsFuel.AddRow();
    breakdownListRow1.ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_FUEL, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetTotalFuel(true), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    breakdownListRow1.SetImportant(true);
  }

  private void UpdateOxidizerDisplay()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.currentCommandModule.GetComponent<AttachableBuilding>()))
    {
      OxidizerTank component1 = gameObject.GetComponent<OxidizerTank>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        foreach (KeyValuePair<Tag, float> keyValuePair in component1.GetOxidizersAvailable())
        {
          if ((double) keyValuePair.Value != 0.0)
            this.rocketDetailsOxidizer.AddRow().ShowData(gameObject.gameObject.GetProperName() + " (" + keyValuePair.Key.ProperName() + ")", GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
        }
      }
      SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        this.rocketDetailsOxidizer.AddRow().ShowData(gameObject.gameObject.GetProperName() + " (" + ElementLoader.FindElementByHash(SimHashes.OxyRock).name + ")", GameUtil.GetFormattedMass(component2.fuelStorage.GetMassAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    }
    BreakdownListRow breakdownListRow = this.rocketDetailsOxidizer.AddRow();
    breakdownListRow.ShowData((string) STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_OXIDIZER, GameUtil.GetFormattedMass(this.currentCommandModule.rocketStats.GetTotalOxidizer(true), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    breakdownListRow.SetImportant(true);
  }

  private void UpdateStorageDisplay()
  {
    float num = this.selectedDestination == null ? 0.0f : this.selectedDestination.AvailableMass;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.currentCommandModule.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        BreakdownListRow breakdownListRow = this.rocketDetailsStorage.AddRow();
        if (this.selectedDestination != null)
        {
          float resourcesPercentage = this.selectedDestination.GetAvailableResourcesPercentage(component.storageType);
          float a = Mathf.Min(component.storage.Capacity(), resourcesPercentage * num);
          num -= a;
          breakdownListRow.ShowData(gameObject.gameObject.GetProperName(), string.Format((string) STRINGS.UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(Mathf.Min(a, component.storage.Capacity()), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")));
        }
        else
          breakdownListRow.ShowData(gameObject.gameObject.GetProperName(), string.Format((string) STRINGS.UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(0.0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")));
      }
    }
  }

  private void ClearDestinationPanel()
  {
    this.destinationDetailsContainer.gameObject.SetActive(false);
    this.destinationStatusLabel.text = (string) STRINGS.UI.STARMAP.ROCKETSTATUS.NONE;
  }

  private void ShowDestinationPanel()
  {
    if (this.selectedDestination == null)
      return;
    this.destinationStatusLabel.text = (string) STRINGS.UI.STARMAP.ROCKETSTATUS.SELECTED;
    if ((UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null && SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.currentLaunchConditionManager).state != Spacecraft.MissionState.Grounded)
      this.destinationStatusLabel.text = (string) STRINGS.UI.STARMAP.ROCKETSTATUS.LOCKEDIN;
    SpaceDestinationType destinationType = this.selectedDestination.GetDestinationType();
    this.destinationNameLabel.text = SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) != SpacecraftManager.DestinationAnalysisState.Complete ? STRINGS.UI.STARMAP.UNKNOWN_DESTINATION.text : destinationType.Name;
    this.destinationTypeValueLabel.text = SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) != SpacecraftManager.DestinationAnalysisState.Complete ? STRINGS.UI.STARMAP.UNKNOWN_TYPE.text : destinationType.typeName;
    this.destinationDistanceValueLabel.text = this.DisplayDistance((float) this.selectedDestination.OneBasedDistance * 10000f);
    this.destinationDescriptionLabel.text = destinationType.description;
    this.destinationDetailsComposition.ClearRows();
    this.destinationDetailsResearch.ClearRows();
    this.destinationDetailsMass.ClearRows();
    this.destinationDetailsResources.ClearRows();
    this.destinationDetailsArtifacts.ClearRows();
    if (destinationType.visitable)
    {
      float num = 0.0f;
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
        num = this.selectedDestination.GetTotalMass();
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
      {
        foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.selectedDestination.researchOpportunities)
          this.destinationDetailsResearch.AddRow().ShowCheckmarkData(researchOpportunity.discoveredRareResource == SimHashes.Void ? researchOpportunity.description : string.Format("(!!) {0}", (object) researchOpportunity.description), researchOpportunity.dataValue.ToString(), !researchOpportunity.completed ? BreakdownListRow.Status.Default : BreakdownListRow.Status.Green);
      }
      this.destinationAnalysisProgressBar.SetFillPercentage(SpacecraftManager.instance.GetDestinationAnalysisScore(this.selectedDestination.id) / (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
      float mass = ConditionHasMinimumMass.CargoCapacity(this.selectedDestination, this.currentCommandModule);
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
      {
        string formattedMass1 = GameUtil.GetFormattedMass(this.selectedDestination.CurrentMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}");
        string formattedMass2 = GameUtil.GetFormattedMass((float) destinationType.minimumMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}");
        BreakdownListRow breakdownListRow1 = this.destinationDetailsMass.AddRow();
        breakdownListRow1.ShowData((string) STRINGS.UI.STARMAP.CURRENT_MASS, formattedMass1);
        if ((double) this.selectedDestination.AvailableMass < (double) mass)
        {
          breakdownListRow1.SetStatusColor(BreakdownListRow.Status.Yellow);
          breakdownListRow1.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CURRENT_MASS_TOOLTIP, (object) GameUtil.GetFormattedMass(this.selectedDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")));
        }
        this.destinationDetailsMass.AddRow().ShowData((string) STRINGS.UI.STARMAP.MAXIMUM_MASS, GameUtil.GetFormattedMass((float) destinationType.maxiumMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
        BreakdownListRow breakdownListRow2 = this.destinationDetailsMass.AddRow();
        breakdownListRow2.ShowData((string) STRINGS.UI.STARMAP.MINIMUM_MASS, formattedMass2);
        breakdownListRow2.AddTooltip((string) STRINGS.UI.STARMAP.MINIMUM_MASS_TOOLTIP);
        BreakdownListRow breakdownListRow3 = this.destinationDetailsMass.AddRow();
        breakdownListRow3.ShowData((string) STRINGS.UI.STARMAP.REPLENISH_RATE, GameUtil.GetFormattedMass(destinationType.replishmentPerCycle, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
        breakdownListRow3.AddTooltip((string) STRINGS.UI.STARMAP.REPLENISH_RATE_TOOLTIP);
      }
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
      {
        foreach (KeyValuePair<SimHashes, float> recoverableElement in this.selectedDestination.recoverableElements)
        {
          BreakdownListRow breakdownListRow = this.destinationDetailsComposition.AddRow();
          float percent = (float) ((double) this.selectedDestination.GetResourceValue(recoverableElement.Key, recoverableElement.Value) / (double) num * 100.0);
          Element elementByHash = ElementLoader.FindElementByHash(recoverableElement.Key);
          Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) elementByHash, "ui", false);
          if ((double) percent <= 1.0)
            breakdownListRow.ShowIconData(elementByHash.name, (string) STRINGS.UI.STARMAP.COMPOSITION_SMALL_AMOUNT, uiSprite.first, uiSprite.second);
          else
            breakdownListRow.ShowIconData(elementByHash.name, GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None), uiSprite.first, uiSprite.second);
          if (elementByHash.IsGas)
          {
            string properName = Assets.GetPrefab("GasCargoBay".ToTag()).GetProperName();
            if (this.currentRocketHasGasContainer)
            {
              breakdownListRow.SetHighlighted(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CAN_CARRY_ELEMENT, (object) elementByHash.name, (object) properName));
            }
            else
            {
              breakdownListRow.SetDisabled(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CONTAINER_REQUIRED, (object) properName));
            }
          }
          if (elementByHash.IsLiquid)
          {
            string properName = Assets.GetPrefab("LiquidCargoBay".ToTag()).GetProperName();
            if (this.currentRocketHasLiquidContainer)
            {
              breakdownListRow.SetHighlighted(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CAN_CARRY_ELEMENT, (object) elementByHash.name, (object) properName));
            }
            else
            {
              breakdownListRow.SetDisabled(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CONTAINER_REQUIRED, (object) properName));
            }
          }
          if (elementByHash.IsSolid)
          {
            string properName = Assets.GetPrefab("CargoBay".ToTag()).GetProperName();
            if (this.currentRocketHasSolidContainer)
            {
              breakdownListRow.SetHighlighted(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CAN_CARRY_ELEMENT, (object) elementByHash.name, (object) properName));
            }
            else
            {
              breakdownListRow.SetDisabled(true);
              breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CONTAINER_REQUIRED, (object) properName));
            }
          }
        }
        foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.selectedDestination.researchOpportunities)
        {
          if (!researchOpportunity.completed && researchOpportunity.discoveredRareResource != SimHashes.Void)
          {
            BreakdownListRow breakdownListRow = this.destinationDetailsComposition.AddRow();
            breakdownListRow.ShowData((string) STRINGS.UI.STARMAP.COMPOSITION_UNDISCOVERED, (string) STRINGS.UI.STARMAP.COMPOSITION_UNDISCOVERED_AMOUNT);
            breakdownListRow.SetDisabled(true);
            breakdownListRow.AddTooltip((string) STRINGS.UI.STARMAP.COMPOSITION_UNDISCOVERED_TOOLTIP);
          }
        }
      }
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
      {
        foreach (KeyValuePair<Tag, int> recoverableEntity in this.selectedDestination.GetRecoverableEntities())
        {
          BreakdownListRow breakdownListRow = this.destinationDetailsResources.AddRow();
          GameObject prefab = Assets.GetPrefab(recoverableEntity.Key);
          Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) prefab, "ui", false);
          breakdownListRow.ShowIconData(prefab.GetProperName(), string.Empty, uiSprite.first, uiSprite.second);
          string properName = Assets.GetPrefab("SpecialCargoBay".ToTag()).GetProperName();
          if (this.currentRocketHasEntitiesContainer)
          {
            breakdownListRow.SetHighlighted(true);
            breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CAN_CARRY_ELEMENT, (object) prefab.GetProperName(), (object) properName));
          }
          else
          {
            breakdownListRow.SetDisabled(true);
            breakdownListRow.AddTooltip(string.Format((string) STRINGS.UI.STARMAP.CANT_CARRY_ELEMENT, (object) properName, (object) prefab.GetProperName()));
          }
        }
      }
      if (SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
      {
        ArtifactDropRate artifactDropTable = this.selectedDestination.GetDestinationType().artifactDropTable;
        foreach (Tuple<ArtifactTier, float> rate in artifactDropTable.rates)
          this.destinationDetailsArtifacts.AddRow().ShowData((string) Strings.Get(rate.first.name_key), GameUtil.GetFormattedPercent((float) ((double) rate.second / (double) artifactDropTable.totalWeight * 100.0), GameUtil.TimeSlice.None));
      }
      this.destinationDetailsContainer.gameObject.SetActive(true);
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.destinationDetailsContainer);
  }

  private void ValidateTravelAbility()
  {
    if (this.selectedDestination == null || SpacecraftManager.instance.GetDestinationAnalysisState(this.selectedDestination) != SpacecraftManager.DestinationAnalysisState.Complete || (!((UnityEngine.Object) this.currentCommandModule != (UnityEngine.Object) null) || !((UnityEngine.Object) this.currentLaunchConditionManager != (UnityEngine.Object) null)))
      return;
    this.launchButton.ChangeState(!this.currentLaunchConditionManager.CheckReadyToLaunch() ? 1 : 0);
  }

  private void UpdateDistanceOverlay(LaunchConditionManager lcmToVisualize = null)
  {
    if ((UnityEngine.Object) lcmToVisualize == (UnityEngine.Object) null)
      lcmToVisualize = this.currentLaunchConditionManager;
    Spacecraft spacecraft = (Spacecraft) null;
    if ((UnityEngine.Object) lcmToVisualize != (UnityEngine.Object) null)
      spacecraft = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcmToVisualize);
    if ((UnityEngine.Object) lcmToVisualize != (UnityEngine.Object) null && spacecraft != null && spacecraft.state == Spacecraft.MissionState.Grounded)
    {
      this.distanceOverlay.gameObject.SetActive(true);
      float num = (float) (int) ((double) lcmToVisualize.GetComponent<CommandModule>().rocketStats.GetRocketMaxDistance() / 10000.0) * 10000f;
      Vector2 sizeDelta = this.distanceOverlay.rectTransform.sizeDelta;
      sizeDelta.x = this.rowsContiner.rect.width;
      sizeDelta.y = (float) (1.0 - (double) num / (double) this.planetsMaxDistance) * this.rowsContiner.rect.height + (float) this.distanceOverlayYOffset + (float) this.distanceOverlayVerticalOffset;
      this.distanceOverlay.rectTransform.sizeDelta = sizeDelta;
      this.distanceOverlay.rectTransform.anchoredPosition = (Vector2) new Vector3(0.0f, (float) this.distanceOverlayVerticalOffset, 0.0f);
    }
    else
      this.distanceOverlay.gameObject.SetActive(false);
  }

  private void UpdateMissionOverlay(LaunchConditionManager lcmToVisualize = null)
  {
    if ((UnityEngine.Object) lcmToVisualize == (UnityEngine.Object) null)
      lcmToVisualize = this.currentLaunchConditionManager;
    Spacecraft spacecraft = (Spacecraft) null;
    if ((UnityEngine.Object) lcmToVisualize != (UnityEngine.Object) null)
      spacecraft = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcmToVisualize);
    if ((UnityEngine.Object) lcmToVisualize != (UnityEngine.Object) null && spacecraft != null)
    {
      SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(lcmToVisualize);
      if (spacecraftDestination == null)
      {
        Debug.Log((object) "destination is null");
      }
      else
      {
        StarmapPlanet planetWidget = this.planetWidgets[spacecraftDestination];
        if (spacecraft == null)
          Debug.Log((object) "craft is null");
        else if ((UnityEngine.Object) planetWidget == (UnityEngine.Object) null)
        {
          Debug.Log((object) "planet is null");
        }
        else
        {
          this.UnselectAllPlanets();
          this.SelectPlanet(planetWidget);
          planetWidget.ShowAsCurrentRocketDestination(spacecraftDestination.GetDestinationType().visitable);
          if (spacecraft.state == Spacecraft.MissionState.Grounded || !spacecraftDestination.GetDestinationType().visitable)
            return;
          this.visualizeRocketImage.gameObject.SetActive(true);
          this.visualizeRocketTrajectory.gameObject.SetActive(true);
          this.visualizeRocketLabel.gameObject.SetActive(true);
          this.visualizeRocketProgress.gameObject.SetActive(true);
          float duration = spacecraft.GetDuration();
          float timeLeft = spacecraft.GetTimeLeft();
          float num = (double) duration != 0.0 ? (float) (1.0 - (double) timeLeft / (double) duration) : 0.0f;
          bool flag = (double) num > 0.5;
          Vector2 a = new Vector2(0.0f, -this.rowsContiner.rect.size.y);
          Vector3 vector3_1 = planetWidget.rectTransform().localPosition + new Vector3(planetWidget.rectTransform().sizeDelta.x * 0.5f, 0.0f, 0.0f);
          Vector3 vector3_2 = planetWidget.transform.parent.rectTransform().localPosition + vector3_1;
          Vector2 b = new Vector2(vector3_2.x, vector3_2.y);
          float x = Vector2.Distance(a, b);
          Vector2 vector2_1 = b - a;
          float z = Mathf.Atan2(vector2_1.y, vector2_1.x) * 57.29578f;
          Vector2 vector2_2 = !flag ? new Vector2(Mathf.Lerp(a.x, b.x, num * 2f), Mathf.Lerp(a.y, b.y, num * 2f)) : new Vector2(Mathf.Lerp(a.x, b.x, (float) (1.0 - (double) num * 2.0 + 1.0)), Mathf.Lerp(a.y, b.y, (float) (1.0 - (double) num * 2.0 + 1.0)));
          this.visualizeRocketLabel.text = spacecraft.state.ToString();
          this.visualizeRocketProgress.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
          this.visualizeRocketTrajectory.transform.SetLocalPosition((Vector3) a);
          this.visualizeRocketTrajectory.rectTransform.sizeDelta = new Vector2(x, this.visualizeRocketTrajectory.rectTransform.sizeDelta.y);
          this.visualizeRocketTrajectory.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, z);
          this.visualizeRocketImage.transform.SetLocalPosition((Vector3) vector2_2);
        }
      }
    }
    else
    {
      if (this.selectedDestination != null && this.planetWidgets.ContainsKey(this.selectedDestination))
      {
        this.UnselectAllPlanets();
        this.SelectPlanet(this.planetWidgets[this.selectedDestination]);
      }
      else
        this.UnselectAllPlanets();
      this.visualizeRocketImage.gameObject.SetActive(false);
      this.visualizeRocketTrajectory.gameObject.SetActive(false);
      this.visualizeRocketLabel.gameObject.SetActive(false);
      this.visualizeRocketProgress.gameObject.SetActive(false);
    }
  }
}
