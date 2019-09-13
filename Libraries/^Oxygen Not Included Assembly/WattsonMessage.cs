// Decompiled with JetBrains decompiler
// Type: WattsonMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class WattsonMessage : KScreen
{
  private static readonly HashedString[] WorkLoopAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private List<KScreen> hideScreensWhileActive = new List<KScreen>();
  private List<SchedulerHandle> scheduleHandles = new List<SchedulerHandle>();
  private const float STARTTIME = 0.1f;
  private const float ENDTIME = 6.6f;
  private const float ALPHA_SPEED = 0.01f;
  private const float expandedHeight = 300f;
  [SerializeField]
  private GameObject dialog;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private KButton button;
  [SerializeField]
  [EventRef]
  private string dialogSound;
  private bool startFade;

  public override float GetSortKey()
  {
    return 8f;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Game.Instance.Subscribe(-122303817, new System.Action<object>(this.OnNewBaseCreated));
  }

  [DebuggerHidden]
  private IEnumerator ExpandPanel()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new WattsonMessage.\u003CExpandPanel\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator CollapsePanel()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new WattsonMessage.\u003CCollapsePanel\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hideScreensWhileActive.Add((KScreen) NotificationScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) OverlayMenu.Instance);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) PlanScreen.Instance);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) BuildMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ManagementMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance.PriorityScreen);
    this.hideScreensWhileActive.Add((KScreen) ResourceCategoryScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) TopLeftControlScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) DateTime.Instance);
    this.hideScreensWhileActive.Add((KScreen) BuildWatermark.Instance);
    foreach (KScreen kscreen in this.hideScreensWhileActive)
      kscreen.Show(false);
  }

  public void Update()
  {
    if (!this.startFade)
      return;
    Color color = this.bg.color;
    color.a -= 0.01f;
    if ((double) color.a <= 0.0)
      color.a = 0.0f;
    this.bg.color = color;
  }

  protected override void OnActivate()
  {
    Debug.Log((object) "WattsonMessage OnActivate");
    base.OnActivate();
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().NewBaseSetupSnapshot, STOP_MODE.ALLOWFADEOUT);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().IntroNIS);
    AudioMixer.instance.activeNIS = true;
    this.button.onClick += (System.Action) (() => this.StartCoroutine(this.CollapsePanel()));
    this.dialog.GetComponent<KScreen>().Show(false);
    this.startFade = false;
    GameObject telepad = GameUtil.GetTelepad();
    if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
    {
      KAnimControllerBase kac = telepad.GetComponent<KAnimControllerBase>();
      kac.Play(WattsonMessage.WorkLoopAnims, KAnim.PlayMode.Loop);
      for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
      {
        int idx = index + 1;
        MinionIdentity liveMinionIdentity = Components.LiveMinionIdentities[index];
        liveMinionIdentity.gameObject.transform.SetPosition(new Vector3((float) ((double) telepad.transform.GetPosition().x + (double) idx - 1.5), telepad.transform.GetPosition().y, liveMinionIdentity.gameObject.transform.GetPosition().z));
        ChoreProvider chore_provider = liveMinionIdentity.gameObject.GetComponent<ChoreProvider>();
        EmoteChore chorePre = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
        {
          (HashedString) ("portalbirth_pre_" + (object) idx)
        }, KAnim.PlayMode.Loop, false);
        UIScheduler.Instance.Schedule("DupeBirth", (float) idx * 0.5f, (System.Action<object>) (data =>
        {
          chorePre.Cancel("Done looping");
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
          {
            (HashedString) ("portalbirth_" + (object) idx)
          }, (Func<StatusItem>) null);
        }), (object) null, (SchedulerGroup) null);
      }
      UIScheduler.Instance.Schedule("Welcome", 6.6f, (System.Action<object>) (data => kac.Play(new HashedString[2]
      {
        (HashedString) "working_pst",
        (HashedString) "idle"
      }, KAnim.PlayMode.Once)), (object) null, (SchedulerGroup) null);
      CameraController.Instance.DisableUserCameraControl = true;
    }
    else
      Debug.LogWarning((object) "Failed to spawn telepad - does the starting base template lack a 'Headquarters' ?");
    this.scheduleHandles.Add(UIScheduler.Instance.Schedule("GoHome", 0.1f, (System.Action<object>) (data =>
    {
      CameraController.Instance.SetOrthographicsSize(TuningData<WattsonMessage.Tuning>.Get().initialOrthographicSize);
      CameraController.Instance.CameraGoHome(1f);
      this.startFade = true;
      this.StartCoroutine(this.ExpandPanel());
      MusicManager.instance.PlaySong("Music_WattsonMessage", false);
    }), (object) null, (SchedulerGroup) null));
    this.scheduleHandles.Add(UIScheduler.Instance.Schedule("WelcomeDialog", 7.6f, (System.Action<object>) (d =>
    {
      SpeedControlScreen.Instance.Pause(false);
      KFMOD.PlayOneShot(this.dialogSound);
      this.dialog.GetComponent<KScreen>().Activate();
      this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
      this.dialog.GetComponent<KScreen>().Show(true);
    }), (object) null, (SchedulerGroup) null));
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().IntroNIS, STOP_MODE.ALLOWFADEOUT);
    AudioMixer.instance.StartPersistentSnapshots();
    MusicManager.instance.StopSong("Music_WattsonMessage", true, STOP_MODE.ALLOWFADEOUT);
    MusicManager.instance.PlayDynamicMusic();
    AudioMixer.instance.activeNIS = false;
    DemoTimer.Instance.CountdownActive = true;
    SpeedControlScreen.Instance.Unpause(false);
    CameraController.Instance.DisableUserCameraControl = false;
    foreach (SchedulerHandle scheduleHandle in this.scheduleHandles)
      scheduleHandle.ClearScheduler();
    UIScheduler.Instance.Schedule("fadeInUI", 0.5f, (System.Action<object>) (d =>
    {
      GameScheduler.Instance.Schedule("BasicTutorial", 1.5f, (System.Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Basics, true)), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("WelcomeTutorial", 2f, (System.Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Welcome, true)), (object) null, (SchedulerGroup) null);
      foreach (KScreen kscreen in this.hideScreensWhileActive)
      {
        kscreen.SetShouldFadeIn(true);
        kscreen.Show(true);
      }
      CameraController.Instance.SetMaxOrthographicSize(20f);
      Game.Instance.timelapser.SaveScreenshot();
    }), (object) null, (SchedulerGroup) null);
    Game.Instance.SetGameStarted();
    if (!((UnityEngine.Object) TopLeftControlScreen.Instance != (UnityEngine.Object) null))
      return;
    TopLeftControlScreen.Instance.RefreshName();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
    {
      CameraController.Instance.CameraGoHome(2f);
      this.Deactivate();
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    e.Consumed = true;
  }

  private void OnNewBaseCreated(object data)
  {
    this.gameObject.SetActive(true);
  }

  public class Tuning : TuningData<WattsonMessage.Tuning>
  {
    public float initialOrthographicSize;
  }
}
