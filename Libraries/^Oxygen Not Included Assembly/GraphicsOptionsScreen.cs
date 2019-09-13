// Decompiled with JetBrains decompiler
// Type: GraphicsOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal class GraphicsOptionsScreen : KModalScreen
{
  public static readonly string ResolutionWidthKey = "ResolutionWidth";
  public static readonly string ResolutionHeightKey = "ResolutionHeight";
  public static readonly string RefreshRateKey = "RefreshRate";
  public static readonly string FullScreenKey = "FullScreen";
  private List<Resolution> resolutions = new List<Resolution>();
  private List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
  [SerializeField]
  private Dropdown resolutionDropdown;
  [SerializeField]
  private MultiToggle fullscreenToggle;
  [SerializeField]
  private KButton applyButton;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private ConfirmDialogScreen confirmPrefab;
  [SerializeField]
  private KSlider uiScaleSlider;
  [SerializeField]
  private LocText sliderLabel;
  [SerializeField]
  private LocText title;
  private KCanvasScaler[] CanvasScalers;
  private ConfirmDialogScreen confirmDialog;
  private GraphicsOptionsScreen.Settings originalSettings;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.TITLE);
    this.originalSettings = this.CaptureSettings();
    this.applyButton.isInteractable = false;
    this.applyButton.onClick += new System.Action(this.OnApply);
    this.applyButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.APPLYBUTTON);
    this.doneButton.onClick += new System.Action(this.OnDone);
    this.closeButton.onClick += new System.Action(this.OnDone);
    this.doneButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.DONE_BUTTON);
    this.resolutionDropdown.ClearOptions();
    this.BuildOptions();
    this.resolutionDropdown.options = this.options;
    this.resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionChanged));
    this.fullscreenToggle.ChangeState(!Screen.fullScreen ? 0 : 1);
    this.fullscreenToggle.onClick += new System.Action(this.OnFullscreenToggle);
    this.fullscreenToggle.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.FULLSCREEN);
    this.resolutionDropdown.transform.parent.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.RESOLUTION);
    if (this.fullscreenToggle.CurrentState == 1)
    {
      int resolutionIndex = this.GetResolutionIndex(this.originalSettings.resolution);
      if (resolutionIndex != -1)
        this.resolutionDropdown.value = resolutionIndex;
    }
    this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>();
    this.UpdateSliderLabel();
    this.uiScaleSlider.onValueChanged.AddListener((UnityAction<float>) (data => this.sliderLabel.text = ((double) this.uiScaleSlider.value).ToString() + "%"));
    this.uiScaleSlider.onReleaseHandle += (System.Action) (() => this.UpdateUIScale(this.uiScaleSlider.value));
  }

  public static void SetResolutionFromPrefs()
  {
    int width = Screen.currentResolution.width;
    int height = Screen.currentResolution.height;
    int preferredRefreshRate = Screen.currentResolution.refreshRate;
    bool fullscreen = Screen.fullScreen;
    DebugUtil.LogArgs((object) string.Format("Starting up with a resolution of {0}x{1} @{2}hz (fullscreen: {3})", (object) width, (object) height, (object) preferredRefreshRate, (object) fullscreen));
    if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionWidthKey) && KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionHeightKey))
    {
      int num1 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionWidthKey);
      int num2 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionHeightKey);
      int num3 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.RefreshRateKey, Screen.currentResolution.refreshRate);
      bool flag = KPlayerPrefs.GetInt(GraphicsOptionsScreen.FullScreenKey, !Screen.fullScreen ? 0 : 1) == 1;
      DebugUtil.LogArgs((object) string.Format("Found player prefs resolution {0}x{1} @{2}hz (fullscreen: {3})", (object) num1, (object) num2, (object) num3, (object) flag));
      if (num2 <= 1 || num1 <= 1)
      {
        DebugUtil.LogArgs((object) "Saved resolution was invalid, ignoring...");
      }
      else
      {
        width = num1;
        height = num2;
        preferredRefreshRate = num3;
        fullscreen = flag;
      }
    }
    if (width <= 1 || height <= 1)
    {
      DebugUtil.LogWarningArgs((object) "Detected a degenerate resolution, attempting to fix...");
      foreach (Resolution resolution in Screen.resolutions)
      {
        if (resolution.width == 1920)
        {
          width = resolution.width;
          height = resolution.height;
          preferredRefreshRate = 0;
        }
      }
      if (width <= 1 || height <= 1)
      {
        foreach (Resolution resolution in Screen.resolutions)
        {
          if (resolution.width == 1280)
          {
            width = resolution.width;
            height = resolution.height;
            preferredRefreshRate = 0;
          }
        }
      }
      if (width <= 1 || height <= 1)
      {
        foreach (Resolution resolution in Screen.resolutions)
        {
          if (resolution.width > 1 && resolution.height > 1 && resolution.refreshRate > 0)
          {
            width = resolution.width;
            height = resolution.height;
            preferredRefreshRate = 0;
          }
        }
      }
      if (width <= 1 || height <= 1)
      {
        string str = "Could not find a suitable resolution for this screen! Reported available resolutions are:";
        foreach (Resolution resolution in Screen.resolutions)
          str += string.Format("\n{0}x{1} @ {2}hz", (object) resolution.width, (object) resolution.height, (object) resolution.refreshRate);
        Debug.LogError((object) str);
        width = 1280;
        height = 720;
        fullscreen = false;
        preferredRefreshRate = 0;
      }
    }
    DebugUtil.LogArgs((object) string.Format("Applying resolution {0}x{1} @{2}hz (fullscreen: {3})", (object) width, (object) height, (object) preferredRefreshRate, (object) fullscreen));
    Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
  }

  public static void OnResize()
  {
    GraphicsOptionsScreen.Settings settings = new GraphicsOptionsScreen.Settings()
    {
      resolution = Screen.currentResolution
    };
    settings.resolution.width = Screen.width;
    settings.resolution.height = Screen.height;
    settings.fullscreen = Screen.fullScreen;
    GraphicsOptionsScreen.SaveResolutionToPrefs(settings);
  }

  private static void SaveResolutionToPrefs(GraphicsOptionsScreen.Settings settings)
  {
    Debug.LogFormat("Screen resolution updated, saving values to prefs: {0}x{1} @ {2}, fullscreen: {3}", (object) settings.resolution.width, (object) settings.resolution.height, (object) settings.resolution.refreshRate, (object) settings.fullscreen);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionWidthKey, settings.resolution.width);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionHeightKey, settings.resolution.height);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.RefreshRateKey, settings.resolution.refreshRate);
    KPlayerPrefs.SetInt(GraphicsOptionsScreen.FullScreenKey, !settings.fullscreen ? 0 : 1);
  }

  private void UpdateUIScale(float value)
  {
    foreach (KCanvasScaler canvasScaler in this.CanvasScalers)
    {
      canvasScaler.SetUserScale(value / 100f);
      KPlayerPrefs.SetFloat(KCanvasScaler.UIScalePrefKey, value);
    }
    this.UpdateSliderLabel();
  }

  private void UpdateSliderLabel()
  {
    if (this.CanvasScalers == null || this.CanvasScalers.Length <= 0 || !((UnityEngine.Object) this.CanvasScalers[0] != (UnityEngine.Object) null))
      return;
    this.uiScaleSlider.value = this.CanvasScalers[0].GetUserScale() * 100f;
    this.sliderLabel.text = ((double) this.uiScaleSlider.value).ToString() + "%";
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
    {
      this.resolutionDropdown.Hide();
      this.Deactivate();
    }
    else
      base.OnKeyDown(e);
  }

  private void BuildOptions()
  {
    this.options.Clear();
    this.resolutions.Clear();
    foreach (Resolution resolution in Screen.resolutions)
    {
      if (resolution.height >= 720)
      {
        this.options.Add(new Dropdown.OptionData(resolution.ToString()));
        this.resolutions.Add(resolution);
      }
    }
  }

  private int GetResolutionIndex(Resolution resolution)
  {
    int num1 = -1;
    int num2 = -1;
    for (int index = 0; index < this.resolutions.Count; ++index)
    {
      Resolution resolution1 = this.resolutions[index];
      if (resolution1.width == resolution.width && resolution1.height == resolution.height && resolution1.refreshRate == 0)
        num2 = index;
      if (resolution1.width == resolution.width && resolution1.height == resolution.height && Math.Abs(resolution1.refreshRate - resolution.refreshRate) <= 1)
      {
        num1 = index;
        break;
      }
    }
    if (num1 == -1)
      return num2;
    return num1;
  }

  private GraphicsOptionsScreen.Settings CaptureSettings()
  {
    return new GraphicsOptionsScreen.Settings()
    {
      fullscreen = Screen.fullScreen,
      resolution = new Resolution()
      {
        width = Screen.width,
        height = Screen.height,
        refreshRate = Screen.currentResolution.refreshRate
      }
    };
  }

  private void OnApply()
  {
    try
    {
      GraphicsOptionsScreen.Settings new_settings = new GraphicsOptionsScreen.Settings();
      new_settings.resolution = this.resolutions[this.resolutionDropdown.value];
      new_settings.fullscreen = this.fullscreenToggle.CurrentState != 0;
      this.ApplyConfirmSettings(new_settings, (System.Action) (() =>
      {
        this.applyButton.isInteractable = false;
        GraphicsOptionsScreen.SaveResolutionToPrefs(new_settings);
      }));
    }
    catch (Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Failed to apply graphics options!\nResolutions:");
      foreach (Resolution resolution in this.resolutions)
        stringBuilder.Append("\t" + resolution.ToString() + "\n");
      stringBuilder.Append("Selected Resolution Idx: " + this.resolutionDropdown.value.ToString());
      stringBuilder.Append("FullScreen: " + this.fullscreenToggle.CurrentState.ToString());
      Debug.LogError((object) stringBuilder.ToString());
      throw ex;
    }
  }

  public void OnDone()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void RefreshApplyButton()
  {
    GraphicsOptionsScreen.Settings settings = this.CaptureSettings();
    if (settings.fullscreen && this.fullscreenToggle.CurrentState == 0)
      this.applyButton.isInteractable = true;
    else if (!settings.fullscreen && this.fullscreenToggle.CurrentState == 1)
      this.applyButton.isInteractable = true;
    else
      this.applyButton.isInteractable = this.resolutionDropdown.value != this.GetResolutionIndex(settings.resolution);
  }

  private void OnFullscreenToggle()
  {
    this.fullscreenToggle.ChangeState(this.fullscreenToggle.CurrentState != 0 ? 0 : 1);
    this.RefreshApplyButton();
  }

  private void OnResolutionChanged(int idx)
  {
    this.RefreshApplyButton();
  }

  private void ApplyConfirmSettings(GraphicsOptionsScreen.Settings new_settings, System.Action on_confirm)
  {
    GraphicsOptionsScreen.Settings current_settings = this.CaptureSettings();
    this.ApplySettings(new_settings);
    this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
    System.Action action = (System.Action) (() => this.ApplySettings(current_settings));
    Coroutine timer = this.StartCoroutine(this.Timer(15f, action));
    this.confirmDialog.onDeactivateCB = (System.Action) (() => this.StopCoroutine(timer));
    this.confirmDialog.PopupConfirmDialog(STRINGS.UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES.text, on_confirm, action, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    this.confirmDialog.gameObject.SetActive(true);
  }

  private void ApplySettings(GraphicsOptionsScreen.Settings new_settings)
  {
    Resolution resolution = new_settings.resolution;
    Screen.SetResolution(resolution.width, resolution.height, new_settings.fullscreen, resolution.refreshRate);
    Screen.fullScreen = new_settings.fullscreen;
    int resolutionIndex = this.GetResolutionIndex(new_settings.resolution);
    if (resolutionIndex == -1)
      return;
    this.resolutionDropdown.value = resolutionIndex;
  }

  [DebuggerHidden]
  private IEnumerator Timer(float time, System.Action revert)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new GraphicsOptionsScreen.\u003CTimer\u003Ec__Iterator0()
    {
      time = time,
      revert = revert,
      \u0024this = this
    };
  }

  private void Update()
  {
    Debug.developerConsoleVisible = false;
  }

  private struct Settings
  {
    public bool fullscreen;
    public Resolution resolution;
  }
}
