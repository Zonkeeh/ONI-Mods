// Decompiled with JetBrains decompiler
// Type: AudioOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioOptionsScreen : KModalScreen
{
  public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";
  public static readonly string AlwaysPlayAutomation = nameof (AlwaysPlayAutomation);
  public static readonly string MuteOnFocusLost = nameof (MuteOnFocusLost);
  private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();
  private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>()
  {
    {
      AudioOptionsScreen.AlwaysPlayMusicKey,
      (object) null
    }
  };
  private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();
  private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private SliderContainer sliderPrefab;
  [SerializeField]
  private GameObject sliderGroup;
  [SerializeField]
  private Image jambell;
  [SerializeField]
  private GameObject alwaysPlayMusicButton;
  [SerializeField]
  private GameObject alwaysPlayAutomationButton;
  [SerializeField]
  private GameObject muteOnFocusLostToggle;
  [SerializeField]
  private Dropdown deviceDropdown;
  private UIPool<SliderContainer> sliderPool;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.closeButton.onClick += (System.Action) (() => this.OnClose(this.gameObject));
    this.doneButton.onClick += (System.Action) (() => this.OnClose(this.gameObject));
    this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
    foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> userVolumeSetting in AudioMixer.instance.userVolumeSettings)
    {
      SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
      this.sliderBusMap.Add(newSlider.slider, userVolumeSetting.Key);
      newSlider.slider.value = userVolumeSetting.Value.busLevel;
      newSlider.nameLabel.text = userVolumeSetting.Value.labelString;
      newSlider.UpdateSliderLabel(userVolumeSetting.Value.busLevel);
      newSlider.slider.ClearReleaseHandleEvent();
      newSlider.slider.onValueChanged.AddListener((UnityAction<float>) (value => this.OnReleaseHandle(newSlider.slider)));
      if (userVolumeSetting.Key == "Master")
      {
        newSlider.transform.SetSiblingIndex(2);
        newSlider.slider.onValueChanged.AddListener(new UnityAction<float>(this.CheckMasterValue));
        this.CheckMasterValue(userVolumeSetting.Value.busLevel);
      }
    }
    HierarchyReferences component1 = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
    GameObject gameObject1 = component1.GetReference("Button").gameObject;
    gameObject1.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
    component1.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
    gameObject1.GetComponent<KButton>().onClick += (System.Action) (() => this.ToggleAlwaysPlayMusic());
    component1.GetReference<LocText>("Label").SetText((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
    if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation))
      KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, 1);
    HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
    GameObject gameObject2 = component2.GetReference("Button").gameObject;
    gameObject2.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
    gameObject2.GetComponent<KButton>().onClick += (System.Action) (() => this.ToggleAlwaysPlayAutomation());
    component2.GetReference<LocText>("Label").SetText((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
    component2.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
    if (!KPlayerPrefs.HasKey(AudioOptionsScreen.MuteOnFocusLost))
      KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, 0);
    HierarchyReferences component3 = this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>();
    GameObject gameObject3 = component3.GetReference("Button").gameObject;
    gameObject3.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST_TOOLTIP);
    gameObject3.GetComponent<KButton>().onClick += (System.Action) (() => this.ToggleMuteOnFocusLost());
    component3.GetReference<LocText>("Label").SetText((string) STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST);
    component3.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  private void CheckMasterValue(float value)
  {
    this.jambell.enabled = (double) value == 0.0;
  }

  private void OnReleaseHandle(KSlider slider)
  {
    AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
  }

  private void ToggleAlwaysPlayMusic()
  {
    MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
    this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
    KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayMusicKey, !MusicManager.instance.alwaysPlayMusic ? 0 : 1);
  }

  private void ToggleAlwaysPlayAutomation()
  {
    KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 ? 1 : 0);
    this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
  }

  private void ToggleMuteOnFocusLost()
  {
    KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) != 1 ? 1 : 0);
    this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
  }

  private void BuildAudioDeviceList()
  {
    this.audioDevices.Clear();
    this.audioDeviceOptions.Clear();
    int numdrivers;
    int numDrivers = (int) RuntimeManager.LowlevelSystem.getNumDrivers(out numdrivers);
    for (int id = 0; id < numdrivers; ++id)
    {
      KFMOD.AudioDevice audioDevice = new KFMOD.AudioDevice();
      string name;
      int driverInfo = (int) RuntimeManager.LowlevelSystem.getDriverInfo(id, out name, 64, out audioDevice.guid, out audioDevice.systemRate, out audioDevice.speakerMode, out audioDevice.speakerModeChannels);
      audioDevice.name = name;
      audioDevice.fmod_id = id;
      this.audioDevices.Add(audioDevice);
      this.audioDeviceOptions.Add(new Dropdown.OptionData(audioDevice.name));
    }
  }

  private void OnAudioDeviceChanged(int idx)
  {
    int num = (int) RuntimeManager.LowlevelSystem.setDriver(idx);
    for (int index = 0; index < this.audioDevices.Count; ++index)
    {
      if (idx == this.audioDevices[index].fmod_id)
      {
        KFMOD.currentDevice = this.audioDevices[index];
        KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
        break;
      }
    }
  }

  private void OnClose(GameObject go)
  {
    this.alwaysPlayMusicMetric[AudioOptionsScreen.AlwaysPlayMusicKey] = (object) MusicManager.instance.alwaysPlayMusic;
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric);
    UnityEngine.Object.Destroy((UnityEngine.Object) go);
  }
}
