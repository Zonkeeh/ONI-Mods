// Decompiled with JetBrains decompiler
// Type: VideoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScreen : KModalScreen
{
  private string victoryLoopMessage = string.Empty;
  private string victoryLoopClip = string.Empty;
  private bool videoSkippable = true;
  public static VideoScreen Instance;
  [SerializeField]
  private VideoPlayer videoPlayer;
  [SerializeField]
  private Slideshow slideshow;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton proceedButton;
  [SerializeField]
  private RectTransform overlayContainer;
  [SerializeField]
  private List<VideoOverlay> overlayPrefabs;
  private RawImage screen;
  private RenderTexture renderTexture;
  private string activeAudioSnapshot;
  [SerializeField]
  private Image fadeOverlay;
  private FMOD.Studio.EventInstance audioHandle;
  private bool victoryLoopQueued;
  public System.Action OnStop;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    this.closeButton.onClick += (System.Action) (() => this.Stop());
    this.proceedButton.onClick += (System.Action) (() => this.Stop());
    this.videoPlayer.isLooping = false;
    this.videoPlayer.loopPointReached += (VideoPlayer.EventHandler) (data =>
    {
      if (this.victoryLoopQueued)
      {
        this.StartCoroutine(this.SwitchToVictoryLoop());
      }
      else
      {
        if (this.videoPlayer.isLooping)
          return;
        this.Stop();
      }
    });
    VideoScreen.Instance = this;
    this.Show(false);
  }

  protected override void OnShow(bool show)
  {
    this.transform.SetAsLastSibling();
    base.OnShow(show);
    this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
  }

  public void DisableAllMedia()
  {
    this.overlayContainer.gameObject.SetActive(false);
    this.videoPlayer.gameObject.SetActive(false);
    this.slideshow.gameObject.SetActive(false);
  }

  public void PlaySlideShow(Sprite[] sprites)
  {
    this.Show(true);
    this.DisableAllMedia();
    this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
    this.slideshow.gameObject.SetActive(true);
    this.slideshow.SetSprites(sprites);
    this.slideshow.SetPaused(false);
  }

  public void PlaySlideShow(string[] files)
  {
    this.Show(true);
    this.DisableAllMedia();
    this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
    this.slideshow.gameObject.SetActive(true);
    this.slideshow.SetFiles(files, 0);
    this.slideshow.SetPaused(false);
  }

  public override float GetSortKey()
  {
    return 100000f;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape))
    {
      if (this.slideshow.gameObject.activeSelf && e.TryConsume(Action.Escape))
      {
        this.Stop();
        return;
      }
      if (e.TryConsume(Action.Escape))
      {
        if (!this.videoSkippable)
          return;
        this.Stop();
        return;
      }
    }
    base.OnKeyDown(e);
  }

  public void PlayVideo(
    VideoClip clip,
    bool unskippable = false,
    string overrideAudioSnapshot = "",
    bool showProceedButton = false)
  {
    for (int index = 0; index < this.overlayContainer.childCount; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.overlayContainer.GetChild(index).gameObject);
    this.Show(true);
    this.videoPlayer.isLooping = false;
    this.activeAudioSnapshot = !string.IsNullOrEmpty(overrideAudioSnapshot) ? overrideAudioSnapshot : AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot;
    AudioMixer.instance.Start(this.activeAudioSnapshot);
    this.DisableAllMedia();
    this.videoPlayer.gameObject.SetActive(true);
    this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
    this.screen.texture = (Texture) this.renderTexture;
    this.videoPlayer.targetTexture = this.renderTexture;
    this.videoPlayer.clip = clip;
    this.videoPlayer.Play();
    if (this.audioHandle.isValid())
    {
      KFMOD.EndOneShot(this.audioHandle);
      this.audioHandle.clearHandle();
    }
    this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name, false), Vector3.zero);
    KFMOD.EndOneShot(this.audioHandle);
    this.videoSkippable = !unskippable;
    this.closeButton.gameObject.SetActive(this.videoSkippable);
    this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
  }

  public void QueueVictoryVideoLoop(
    bool queue,
    string message = "",
    string victoryAchievement = "",
    string loopVideo = "")
  {
    this.victoryLoopQueued = queue;
    this.victoryLoopMessage = message;
    this.victoryLoopClip = loopVideo;
    this.OnStop += (System.Action) (() =>
    {
      RetireColonyUtility.SaveColonySummaryData();
      MainMenu.ActivateRetiredColoniesScreen(this.transform.parent.gameObject, SaveGame.Instance.BaseName, SaveGame.Instance.GetComponent<ColonyAchievementTracker>().achievementsToDisplay.ToArray());
    });
  }

  public void SetOverlayText(string overlayTemplate, List<string> strings)
  {
    VideoOverlay videoOverlay = (VideoOverlay) null;
    foreach (VideoOverlay overlayPrefab in this.overlayPrefabs)
    {
      if (overlayPrefab.name == overlayTemplate)
      {
        videoOverlay = overlayPrefab;
        break;
      }
    }
    DebugUtil.Assert((UnityEngine.Object) videoOverlay != (UnityEngine.Object) null, "Could not find a template named ", overlayTemplate);
    Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
    this.overlayContainer.gameObject.SetActive(true);
  }

  [DebuggerHidden]
  private IEnumerator SwitchToVictoryLoop()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new VideoScreen.\u003CSwitchToVictoryLoop\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public void Stop()
  {
    this.videoPlayer.Stop();
    this.screen.texture = (Texture) null;
    this.videoPlayer.targetTexture = (RenderTexture) null;
    AudioMixer.instance.Stop((HashedString) this.activeAudioSnapshot, STOP_MODE.ALLOWFADEOUT);
    int num = (int) this.audioHandle.stop(STOP_MODE.IMMEDIATE);
    if (this.OnStop != null)
      this.OnStop();
    this.Show(false);
  }
}
