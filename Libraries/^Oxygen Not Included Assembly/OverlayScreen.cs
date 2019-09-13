// Decompiled with JetBrains decompiler
// Type: OverlayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class OverlayScreen : KMonoBehaviour
{
  public static HashSet<Tag> WireIDs = new HashSet<Tag>();
  public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();
  public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();
  public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();
  public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();
  public static HashSet<Tag> SuitIDs = new HashSet<Tag>();
  public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();
  [SerializeField]
  private Color buildingDisabledColour = Color.gray;
  private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();
  [EventRef]
  [SerializeField]
  public string techViewSoundPath;
  private EventInstance techViewSound;
  private bool techViewSoundPlaying;
  public static OverlayScreen Instance;
  [Header("Power")]
  [SerializeField]
  private Canvas powerLabelParent;
  [SerializeField]
  private LocText powerLabelPrefab;
  [SerializeField]
  private BatteryUI batUIPrefab;
  [SerializeField]
  private Vector3 powerLabelOffset;
  [SerializeField]
  private Vector3 batteryUIOffset;
  [SerializeField]
  private Vector3 batteryUITransformerOffset;
  [SerializeField]
  private Vector3 batteryUISmallTransformerOffset;
  [SerializeField]
  private Color consumerColour;
  [SerializeField]
  private Color generatorColour;
  [Header("Circuits")]
  [SerializeField]
  private Color32 circuitUnpoweredColour;
  [SerializeField]
  private Color32 circuitSafeColour;
  [SerializeField]
  private Color32 circuitStrainingColour;
  [SerializeField]
  private Color32 circuitOverloadingColour;
  [Header("Crops")]
  [SerializeField]
  private GameObject harvestableNotificationPrefab;
  [Header("Disease")]
  [SerializeField]
  private GameObject diseaseOverlayPrefab;
  [Header("Suit")]
  [SerializeField]
  private GameObject suitOverlayPrefab;
  [Header("ToolTip")]
  [SerializeField]
  private TextStyleSetting TooltipHeader;
  [SerializeField]
  private TextStyleSetting TooltipDescription;
  [Header("Logic")]
  [SerializeField]
  private LogicModeUI logicModeUIPrefab;
  public System.Action<HashedString> OnOverlayChanged;
  private OverlayScreen.ModeInfo currentModeInfo;

  public HashedString mode
  {
    get
    {
      return this.currentModeInfo.mode.ViewMode();
    }
  }

  protected override void OnPrefabInit()
  {
    Debug.Assert((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null);
    OverlayScreen.Instance = this;
    this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
  }

  protected override void OnLoadLevel()
  {
    this.harvestableNotificationPrefab = (GameObject) null;
    this.powerLabelParent = (Canvas) null;
    OverlayScreen.Instance = (OverlayScreen) null;
    OverlayModes.Mode.Clear();
    this.modeInfos = (Dictionary<HashedString, OverlayScreen.ModeInfo>) null;
    this.currentModeInfo = new OverlayScreen.ModeInfo();
    base.OnLoadLevel();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
    this.techViewSoundPlaying = false;
    Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
    this.RegisterModes();
    this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
  }

  private void RegisterModes()
  {
    this.modeInfos.Clear();
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.None());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Oxygen());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset, this.consumerColour, this.generatorColour, this.buildingDisabledColour, this.circuitUnpoweredColour, this.circuitSafeColour, this.circuitStrainingColour, this.circuitOverloadingColour));
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Temperature());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.ThermalConductivity());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Light());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.LiquidConduits());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.GasConduits());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Decor());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Harvest());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Priorities());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.HeatFlow());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Rooms());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.Logic(this.logicModeUIPrefab));
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.SolidConveyor());
    this.RegisterMode((OverlayModes.Mode) new OverlayModes.TileMode());
  }

  private void RegisterMode(OverlayModes.Mode mode)
  {
    this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo()
    {
      mode = mode
    };
  }

  private void LateUpdate()
  {
    this.currentModeInfo.mode.Update();
  }

  public void ToggleOverlay(HashedString newMode, bool allowSound = true)
  {
    bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
    if (newMode != OverlayModes.None.ID)
      ManagementMenu.Instance.CloseAll();
    this.currentModeInfo.mode.Disable();
    if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
      ManagementMenu.Instance.CloseAll();
    ResourceCategoryScreen.Instance.Show(newMode == OverlayModes.None.ID && Game.Instance.GameStarted());
    SimDebugView.Instance.SetMode(newMode);
    if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
      this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
    this.currentModeInfo.mode.Enable();
    if (flag)
      this.UpdateOverlaySounds();
    if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
    {
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().TechFilterOnMigrated, STOP_MODE.ALLOWFADEOUT);
      MusicManager.instance.SetDynamicMusicOverlayInactive();
      int num = (int) this.techViewSound.stop(STOP_MODE.ALLOWFADEOUT);
      this.techViewSoundPlaying = false;
    }
    else if (!this.techViewSoundPlaying)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
      MusicManager.instance.SetDynamicMusicOverlayActive();
      int num = (int) this.techViewSound.start();
      this.techViewSoundPlaying = true;
    }
    if (this.OnOverlayChanged != null)
      this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
    this.ActivateLegend();
  }

  private void ActivateLegend()
  {
    if ((UnityEngine.Object) OverlayLegend.Instance == (UnityEngine.Object) null)
      return;
    OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
  }

  public void Refresh()
  {
    this.LateUpdate();
  }

  public HashedString GetMode()
  {
    if (this.currentModeInfo.mode != null)
      return this.currentModeInfo.mode.ViewMode();
    return OverlayModes.None.ID;
  }

  private void UpdateOverlaySounds()
  {
    string soundName = this.currentModeInfo.mode.GetSoundName();
    if (!(soundName != string.Empty))
      return;
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(soundName, false));
  }

  private struct ModeInfo
  {
    public OverlayModes.Mode mode;
  }
}
