// Decompiled with JetBrains decompiler
// Type: OfflineWorldGen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VoronoiTree;

public class OfflineWorldGen : KMonoBehaviour
{
  private Mutex errorMutex = new Mutex();
  private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();
  private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[1]
  {
    new OfflineWorldGen.ValidDimensions()
    {
      width = 256,
      height = 384,
      name = STRINGS.UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
    }
  };
  public string frontendGameLevel = "frontend";
  public string mainGameLevel = "backend";
  private bool trackProgress = true;
  private List<LocString> convertList = new List<LocString>()
  {
    STRINGS.UI.WORLDGEN.SETTLESIM,
    STRINGS.UI.WORLDGEN.BORDERS,
    STRINGS.UI.WORLDGEN.PROCESSING,
    STRINGS.UI.WORLDGEN.COMPLETELAYOUT,
    STRINGS.UI.WORLDGEN.WORLDLAYOUT,
    STRINGS.UI.WORLDGEN.GENERATENOISE,
    STRINGS.UI.WORLDGEN.BUILDNOISESOURCE,
    STRINGS.UI.WORLDGEN.GENERATESOLARSYSTEM
  };
  [SerializeField]
  private RectTransform buttonRoot;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private RectTransform chooseLocationPanel;
  [SerializeField]
  private GameObject locationButtonPrefab;
  private const float baseScale = 0.005f;
  private bool shouldStop;
  private StringKey currentConvertedCurrentStage;
  private float currentPercent;
  public bool debug;
  private bool doWorldGen;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText mainText;
  [SerializeField]
  private LocText updateText;
  [SerializeField]
  private LocText percentText;
  [SerializeField]
  private LocText seedText;
  [SerializeField]
  private KBatchedAnimController meterAnim;
  [SerializeField]
  private KBatchedAnimController asteriodAnim;
  private WorldGen worldGen;
  private List<VoronoiTree.Node> startNodes;
  private StringKey currentStringKeyRoot;
  private WorldGenProgressStages.Stages currentStage;
  private bool loadTriggered;
  private bool shownStartingLocations;
  private bool startedExitFlow;
  private bool generateThreadComplete;
  private bool renderThreadComplete;
  private bool firstPassGeneration;
  private bool secondPassGeneration;
  private int seed;

  private void TrackProgress(string text)
  {
    if (!this.trackProgress)
      return;
    Debug.Log((object) text);
  }

  public static bool CanLoadSave()
  {
    bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
    if (!flag)
    {
      SaveLoader.SetActiveSaveFilePath((string) null);
      flag = WorldGen.CanLoad(WorldGen.SIM_SAVE_FILENAME);
    }
    return flag;
  }

  public void Generate()
  {
    this.doWorldGen = !OfflineWorldGen.CanLoadSave();
    this.updateText.gameObject.SetActive(false);
    this.percentText.gameObject.SetActive(false);
    this.doWorldGen |= this.debug;
    if (this.doWorldGen)
    {
      this.seedText.text = string.Format((string) STRINGS.UI.WORLDGEN.USING_PLAYER_SEED, (object) this.seed);
      this.titleText.text = STRINGS.UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
      this.mainText.text = STRINGS.UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
      for (int index = 0; index < this.validDimensions.Length; ++index)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
        gameObject.SetActive(true);
        RectTransform component = gameObject.GetComponent<RectTransform>();
        component.SetParent((Transform) this.buttonRoot);
        component.localScale = Vector3.one;
        gameObject.GetComponentInChildren<LocText>().text = this.validDimensions[index].name.ToString();
        int idx = index;
        gameObject.GetComponent<KButton>().onClick += (System.Action) (() =>
        {
          this.DoWorldGen(idx);
          this.ToggleGenerationUI();
        });
      }
      if (this.validDimensions.Length == 1)
      {
        this.DoWorldGen(0);
        this.ToggleGenerationUI();
      }
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
      this.OnResize();
    }
    else
    {
      this.titleText.text = STRINGS.UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
      this.mainText.gameObject.SetActive(false);
      this.currentConvertedCurrentStage = STRINGS.UI.WORLDGEN.COMPLETE.key;
      this.currentPercent = 100f;
      this.updateText.gameObject.SetActive(false);
      this.percentText.gameObject.SetActive(false);
      this.RemoveButtons();
    }
    this.buttonPrefab.SetActive(false);
  }

  private void OnResize()
  {
    float canvasScale = this.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
    if (!((UnityEngine.Object) this.asteriodAnim != (UnityEngine.Object) null))
      return;
    this.asteriodAnim.animScale = (float) (0.00499999988824129 * (1.0 / (double) canvasScale));
  }

  private void ToggleGenerationUI()
  {
    this.percentText.gameObject.SetActive(false);
    this.updateText.gameObject.SetActive(true);
    this.titleText.text = STRINGS.UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
    if ((UnityEngine.Object) this.titleText != (UnityEngine.Object) null && (UnityEngine.Object) this.titleText.gameObject != (UnityEngine.Object) null)
      this.titleText.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.buttonRoot != (UnityEngine.Object) null) || !((UnityEngine.Object) this.buttonRoot.gameObject != (UnityEngine.Object) null))
      return;
    this.buttonRoot.gameObject.SetActive(false);
  }

  private void ChooseBaseLocation(VoronoiTree.Node startNode)
  {
    this.worldGen.ChooseBaseLocation(startNode);
    this.DoRenderWorld();
    this.RemoveLocationButtons();
  }

  private void ShowStartingLocationChoices()
  {
    if ((UnityEngine.Object) this.titleText != (UnityEngine.Object) null)
      this.titleText.text = "Choose Starting Location";
    this.startNodes = this.worldGen.WorldLayout.GetStartNodes();
    this.startNodes.Shuffle<VoronoiTree.Node>();
    if (this.startNodes.Count > 0)
    {
      this.ChooseBaseLocation(this.startNodes[0]);
    }
    else
    {
      List<SubWorld> subWorldList = new List<SubWorld>();
      for (int index = 0; index < this.startNodes.Count; ++index)
      {
        Tree node = this.startNodes[index] as Tree;
        if (node == null)
        {
          node = this.worldGen.GetOverworldForNode(this.startNodes[index] as Leaf);
          if (node == null)
            continue;
        }
        SubWorld subWorldForNode = this.worldGen.GetSubWorldForNode(node);
        if (subWorldForNode != null && !subWorldList.Contains(subWorldForNode))
        {
          subWorldList.Add(subWorldForNode);
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.locationButtonPrefab);
          RectTransform component = gameObject.GetComponent<RectTransform>();
          component.SetParent((Transform) this.chooseLocationPanel);
          component.localScale = Vector3.one;
          Text componentInChildren = gameObject.GetComponentInChildren<Text>();
          SubWorld subWorld = (SubWorld) null;
          Tree parent = this.startNodes[index].parent;
          while (subWorld == null && parent != null)
          {
            subWorld = this.worldGen.GetSubWorldForNode(parent);
            if (subWorld == null)
              parent = parent.parent;
          }
          TagSet tagSet = new TagSet(this.startNodes[index].tags);
          tagSet.Remove(WorldGenTags.Feature);
          tagSet.Remove(WorldGenTags.StartLocation);
          tagSet.Remove(WorldGenTags.IgnoreCaveOverride);
          componentInChildren.text = tagSet.ToString();
          int idx = index;
          UnityEngine.UI.Button.ButtonClickedEvent buttonClickedEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
          buttonClickedEvent.AddListener((UnityAction) (() => this.ChooseBaseLocation(this.startNodes[idx])));
          gameObject.GetComponent<UnityEngine.UI.Button>().onClick = buttonClickedEvent;
        }
      }
    }
  }

  private void RemoveLocationButtons()
  {
    for (int index = this.chooseLocationPanel.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.chooseLocationPanel.GetChild(index).gameObject);
    if (!((UnityEngine.Object) this.titleText != (UnityEngine.Object) null) || !((UnityEngine.Object) this.titleText.gameObject != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.titleText.gameObject);
  }

  private bool UpdateProgress(
    StringKey stringKeyRoot,
    float completePercent,
    WorldGenProgressStages.Stages stage)
  {
    if (this.currentStage != stage)
      this.currentStage = stage;
    if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
    {
      this.currentConvertedCurrentStage = stringKeyRoot;
      this.currentStringKeyRoot = stringKeyRoot;
    }
    else
    {
      int num = (int) completePercent / 10;
      LocString locString = this.convertList.Find((Predicate<LocString>) (s => s.key.Hash == stringKeyRoot.Hash));
      if (num != 0 && locString != null)
        this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = WorldGenProgressStages.StageWeights[(int) stage].Value * completePercent;
    for (int index = 0; index < WorldGenProgressStages.StageWeights.Length; ++index)
    {
      num2 += WorldGenProgressStages.StageWeights[index].Value * 100f;
      if ((WorldGenProgressStages.Stages) index < this.currentStage)
        num1 += WorldGenProgressStages.StageWeights[index].Value * 100f;
    }
    this.currentPercent = (float) (100.0 * (((double) num1 + (double) num3) / (double) num2));
    return !this.shouldStop;
  }

  private void Update()
  {
    if (this.loadTriggered || this.currentConvertedCurrentStage.String == null)
      return;
    this.errorMutex.WaitOne();
    int count = this.errors.Count;
    this.errorMutex.ReleaseMutex();
    if (count > 0)
    {
      this.DoExitFlow();
    }
    else
    {
      this.updateText.text = (string) Strings.Get(this.currentConvertedCurrentStage.String);
      if (!this.debug && (this.currentConvertedCurrentStage.Hash == STRINGS.UI.WORLDGEN.COMPLETE.key.Hash && (double) this.currentPercent >= 100.0))
      {
        if (KCrashReporter.terminateOnError && ReportErrorDialog.hasCrash)
          return;
        this.percentText.text = string.Empty;
        this.loadTriggered = true;
        App.LoadScene(this.mainGameLevel);
      }
      else if ((double) this.currentPercent < 0.0)
      {
        this.DoExitFlow();
      }
      else
      {
        if ((double) this.currentPercent > 0.0 && !this.percentText.gameObject.activeSelf)
          this.percentText.gameObject.SetActive(false);
        this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent, GameUtil.TimeSlice.None);
        this.meterAnim.SetPositionPercent(this.currentPercent / 100f);
        if (this.firstPassGeneration)
        {
          this.generateThreadComplete = this.worldGen.IsGenerateComplete();
          if (!this.generateThreadComplete)
            this.renderThreadComplete = false;
        }
        if (this.secondPassGeneration)
          this.renderThreadComplete = this.worldGen.IsRenderComplete();
        if (!this.shownStartingLocations && this.firstPassGeneration && this.generateThreadComplete)
        {
          this.shownStartingLocations = true;
          this.ShowStartingLocationChoices();
        }
        if (!this.renderThreadComplete)
          return;
        int num = 0 + 1;
      }
    }
  }

  private void DisplayErrors()
  {
    this.errorMutex.WaitOne();
    if (this.errors.Count > 0)
    {
      foreach (OfflineWorldGen.ErrorInfo error in this.errors)
        Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(error.errorDesc, new System.Action(this.OnConfirmExit), (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    }
    this.errorMutex.ReleaseMutex();
  }

  private void DoExitFlow()
  {
    if (this.startedExitFlow)
      return;
    this.startedExitFlow = true;
    this.percentText.text = STRINGS.UI.WORLDGEN.RESTARTING.ToString();
    this.loadTriggered = true;
    Sim.Shutdown();
    this.DisplayErrors();
  }

  private void OnConfirmExit()
  {
    App.LoadScene(this.frontendGameLevel);
  }

  private void RemoveButtons()
  {
    for (int index = this.buttonRoot.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.buttonRoot.GetChild(index).gameObject);
  }

  private void DoWorldGen(int selectedDimension)
  {
    this.RemoveButtons();
    this.DoWorldGenInitialize();
  }

  private void DoWorldGenInitialize()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.World);
    this.seed = int.Parse(CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed).id);
    List<string> randomTraits = SettingsCache.GetRandomTraits(this.seed);
    this.worldGen = new WorldGen(currentQualitySetting.id, randomTraits);
    Vector2I worldsize = this.worldGen.Settings.world.worldsize;
    GridSettings.Reset(worldsize.x, worldsize.y);
    this.worldGen.Initialise(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new System.Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed);
    this.firstPassGeneration = true;
    this.worldGen.GenerateOfflineThreaded();
  }

  private void DoRenderWorld()
  {
    this.firstPassGeneration = false;
    this.secondPassGeneration = true;
    this.worldGen.RenderWorldThreaded();
  }

  private void OnError(OfflineWorldGen.ErrorInfo error)
  {
    this.errorMutex.WaitOne();
    this.errors.Add(error);
    this.errorMutex.ReleaseMutex();
  }

  public struct ErrorInfo
  {
    public string errorDesc;
    public Exception exception;
  }

  [Serializable]
  private struct ValidDimensions
  {
    public int width;
    public int height;
    public StringKey name;
  }
}
