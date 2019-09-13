// Decompiled with JetBrains decompiler
// Type: SpeedControlScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControlScreen : KScreen
{
  public GameObject playButtonWidget;
  public GameObject pauseButtonWidget;
  public Image playIcon;
  public Image pauseIcon;
  [SerializeField]
  private TextStyleSetting TooltipTextStyle;
  public GameObject speedButtonWidget_slow;
  public GameObject speedButtonWidget_medium;
  public GameObject speedButtonWidget_fast;
  public GameObject mainMenuWidget;
  public float normalSpeed;
  public float fastSpeed;
  public float ultraSpeed;
  private KToggle pauseButton;
  private KToggle slowButton;
  private KToggle mediumButton;
  private KToggle fastButton;
  private int speed;
  private int pauseCount;

  public static SpeedControlScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    SpeedControlScreen.Instance = (SpeedControlScreen) null;
  }

  public bool IsPaused
  {
    get
    {
      return this.pauseCount > 0;
    }
  }

  protected override void OnPrefabInit()
  {
    SpeedControlScreen.Instance = this;
    this.pauseButton = this.pauseButtonWidget.GetComponent<KToggle>();
    this.slowButton = this.speedButtonWidget_slow.GetComponent<KToggle>();
    this.mediumButton = this.speedButtonWidget_medium.GetComponent<KToggle>();
    this.fastButton = this.speedButtonWidget_fast.GetComponent<KToggle>();
    KToggle[] ktoggleArray = new KToggle[4]
    {
      this.pauseButton,
      this.slowButton,
      this.mediumButton,
      this.fastButton
    };
    foreach (KToggle ktoggle in ktoggleArray)
      ktoggle.soundPlayer.Enabled = false;
    this.slowButton.onClick += (System.Action) (() =>
    {
      this.PlaySpeedChangeSound(1f);
      this.SetSpeed(0);
    });
    this.mediumButton.onClick += (System.Action) (() =>
    {
      this.PlaySpeedChangeSound(2f);
      this.SetSpeed(1);
    });
    this.fastButton.onClick += (System.Action) (() =>
    {
      this.PlaySpeedChangeSound(3f);
      this.SetSpeed(2);
    });
    this.pauseButton.onClick += (System.Action) (() => this.TogglePause(true));
    this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.TOOLTIPS.SPEEDBUTTON_SLOW, Action.CycleSpeed), (ScriptableObject) this.TooltipTextStyle);
    this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, Action.CycleSpeed), (ScriptableObject) this.TooltipTextStyle);
    this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.TOOLTIPS.SPEEDBUTTON_FAST, Action.CycleSpeed), (ScriptableObject) this.TooltipTextStyle);
    this.playButtonWidget.GetComponent<KButton>().onClick += (System.Action) (() => this.TogglePause(true));
  }

  protected override void OnSpawn()
  {
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.speed = SaveGame.Instance.GetSpeed();
      this.SetSpeed(this.speed);
    }
    base.OnSpawn();
    this.OnChanged();
  }

  public int GetSpeed()
  {
    return this.speed;
  }

  public void SetSpeed(int Speed)
  {
    this.speed = Speed % 3;
    switch (this.speed)
    {
      case 0:
        this.slowButton.Select();
        this.slowButton.isOn = true;
        this.mediumButton.isOn = false;
        this.fastButton.isOn = false;
        break;
      case 1:
        this.mediumButton.Select();
        this.slowButton.isOn = false;
        this.mediumButton.isOn = true;
        this.fastButton.isOn = false;
        break;
      case 2:
        this.fastButton.Select();
        this.slowButton.isOn = false;
        this.mediumButton.isOn = false;
        this.fastButton.isOn = true;
        break;
    }
    this.OnSpeedChange();
  }

  public void ToggleRidiculousSpeed()
  {
    this.ultraSpeed = (double) this.ultraSpeed != 3.0 ? 3f : 10f;
    this.speed = 2;
    this.OnChanged();
  }

  public void TogglePause(bool playsound = true)
  {
    if (this.IsPaused)
      this.Unpause(playsound);
    else
      this.Pause(playsound);
  }

  public void Pause(bool playSound = true)
  {
    ++this.pauseCount;
    if (this.pauseCount != 1)
      return;
    if (playSound)
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Pause", false));
      if ((UnityEngine.Object) SoundListenerController.Instance != (UnityEngine.Object) null)
        SoundListenerController.Instance.SetLoopingVolume(0.0f);
    }
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().SpeedPausedMigrated);
    MusicManager.instance.SetDynamicMusicPaused();
    this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
    this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.TOOLTIPS.UNPAUSE, Action.TogglePause), (ScriptableObject) this.TooltipTextStyle);
    this.pauseButton.isOn = true;
    this.OnPause();
  }

  public void Unpause(bool playSound = true)
  {
    this.pauseCount = Mathf.Max(0, this.pauseCount - 1);
    if (this.pauseCount != 0)
      return;
    if (playSound)
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Unpause", false));
      if ((UnityEngine.Object) SoundListenerController.Instance != (UnityEngine.Object) null)
        SoundListenerController.Instance.SetLoopingVolume(1f);
    }
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().SpeedPausedMigrated, STOP_MODE.ALLOWFADEOUT);
    MusicManager.instance.SetDynamicMusicUnpaused();
    this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
    this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.TOOLTIPS.PAUSE, Action.TogglePause), (ScriptableObject) this.TooltipTextStyle);
    this.pauseButton.isOn = false;
    this.SetSpeed(this.speed);
    this.OnPlay();
  }

  private void OnPause()
  {
    this.OnChanged();
  }

  private void OnPlay()
  {
    this.OnChanged();
  }

  public void OnSpeedChange()
  {
    if (Game.IsQuitting())
      return;
    this.OnChanged();
  }

  private void OnChanged()
  {
    if (this.IsPaused)
      Time.timeScale = 0.0f;
    else if (this.speed == 0)
      Time.timeScale = this.normalSpeed;
    else if (this.speed == 1)
    {
      Time.timeScale = this.fastSpeed;
    }
    else
    {
      if (this.speed != 2)
        return;
      Time.timeScale = this.ultraSpeed;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.TogglePause))
      this.TogglePause(true);
    else if (e.TryConsume(Action.CycleSpeed))
    {
      this.PlaySpeedChangeSound((float) ((this.speed + 1) % 3 + 1));
      this.SetSpeed(this.speed + 1);
      this.OnSpeedChange();
    }
    else if (e.TryConsume(Action.SpeedUp))
    {
      ++this.speed;
      this.speed = Math.Min(this.speed, 2);
      this.SetSpeed(this.speed);
    }
    else
    {
      if (!e.TryConsume(Action.SlowDown))
        return;
      --this.speed;
      this.speed = Math.Max(this.speed, 0);
      this.SetSpeed(this.speed);
    }
  }

  private void PlaySpeedChangeSound(float speed)
  {
    string sound = GlobalAssets.GetSound("Speed_Change", false);
    if (sound == null)
      return;
    FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, Vector3.zero);
    int num = (int) instance.setParameterValue("Speed", speed);
    SoundEvent.EndOneShot(instance);
  }

  public void DebugStepFrame()
  {
    DebugUtil.LogArgs((object) "Stepping one frame");
    this.Unpause(false);
    this.StartCoroutine(this.DebugStepFrameDelay());
  }

  [DebuggerHidden]
  private IEnumerator DebugStepFrameDelay()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SpeedControlScreen.\u003CDebugStepFrameDelay\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
