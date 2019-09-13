// Decompiled with JetBrains decompiler
// Type: MusicManager
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

public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
  private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();
  public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();
  [NonSerialized]
  public List<string> MusicDebugLog = new List<string>();
  private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();
  private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();
  [Space]
  [Header("Tuning Values")]
  [Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
  [SerializeField]
  private float duskTimePercentage = 85f;
  [Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
  [SerializeField]
  private float loadGameCutoffPercentage = 50f;
  [Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
  [SerializeField]
  private float dynamicMusicSFXAttenuationPercentage = 65f;
  private float timeOfDayUpdateRate = 2f;
  private const string VARIATION_ID = "variation";
  private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";
  private MusicManager.SongInfo[] songs;
  [Header("Song Lists")]
  [Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
  [SerializeField]
  private MusicManager.DynamicSong[] fullSongs;
  [Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
  [SerializeField]
  private MusicManager.Stinger[] miniSongs;
  [Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
  [SerializeField]
  private MusicManager.Stinger[] stingers;
  [Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
  [SerializeField]
  private MusicManager.SongInfo[] menuSongs;
  [NonSerialized]
  public MusicManager.SongInfo activeDynamicSong;
  [NonSerialized]
  public MusicManager.DynamicSongPlaylist activePlaylist;
  private MusicManager.TypeOfMusic nextMusicType;
  private int musicTypeIterator;
  [Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
  [SerializeField]
  private float miniSongSFXAttenuationPercentage;
  [SerializeField]
  private MusicManager.TypeOfMusic[] musicStyleOrder;
  [NonSerialized]
  public bool alwaysPlayMusic;
  private float time;
  private static MusicManager _instance;

  private void Log(string s)
  {
  }

  public Dictionary<string, MusicManager.SongInfo> SongMap
  {
    get
    {
      return this.songMap;
    }
  }

  public Dictionary<string, MusicManager.SongInfo> ActiveSongs
  {
    get
    {
      return this.activeSongs;
    }
  }

  public void PlaySong(string song_name, bool canWait = false)
  {
    this.Log("Play: " + song_name);
    if (!AudioDebug.Get().musicEnabled)
      return;
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    if (!this.songMap.TryGetValue(song_name, out songInfo))
      DebugUtil.LogErrorArgs((object) "Unknown song:", (object) song_name);
    else if (this.activeSongs.ContainsKey(song_name))
      DebugUtil.LogWarningArgs((object) "Trying to play duplicate song:", (object) song_name);
    else if (this.activeSongs.Count == 0)
    {
      songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
      if (!songInfo.ev.isValid())
        DebugUtil.LogWarningArgs((object) ("Failed to find FMOD event [" + songInfo.fmodEvent + "]"));
      int num1 = songInfo.numberOfVariations <= 0 ? -1 : UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1);
      if (num1 != -1)
      {
        int num2 = (int) songInfo.ev.setParameterValue("variation", (float) num1);
      }
      int num3 = (int) songInfo.ev.start();
      this.activeSongs[song_name] = songInfo;
      if (!songInfo.dynamic)
        return;
      this.activeDynamicSong = songInfo;
    }
    else
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.activeSongs.Keys);
      if (songInfo.interruptsActiveMusic)
      {
        for (int index = 0; index < stringList.Count; ++index)
        {
          if (!this.activeSongs[stringList[index]].interruptsActiveMusic)
          {
            MusicManager.SongInfo activeSong = this.activeSongs[stringList[index]];
            int num = (int) activeSong.ev.setParameterValue("interrupted_dimmed", 1f);
            this.Log("Dimming: " + Assets.GetSimpleSoundEventName(activeSong.fmodEvent));
            songInfo.songsOnHold.Add(stringList[index]);
          }
        }
        songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
        if (!songInfo.ev.isValid())
          DebugUtil.LogWarningArgs((object) ("Failed to find FMOD event [" + songInfo.fmodEvent + "]"));
        int num1 = (int) songInfo.ev.start();
        int num2 = (int) songInfo.ev.release();
        this.activeSongs[song_name] = songInfo;
      }
      else
      {
        int num1 = 0;
        foreach (string key in this.activeSongs.Keys)
        {
          MusicManager.SongInfo activeSong = this.activeSongs[key];
          if (!activeSong.interruptsActiveMusic && activeSong.priority > num1)
            num1 = activeSong.priority;
        }
        if (songInfo.priority < num1)
          return;
        for (int index = 0; index < stringList.Count; ++index)
        {
          MusicManager.SongInfo activeSong = this.activeSongs[stringList[index]];
          FMOD.Studio.EventInstance ev = activeSong.ev;
          if (!activeSong.interruptsActiveMusic)
          {
            int num2 = (int) ev.setParameterValue("interrupted_dimmed", 1f);
            int num3 = (int) ev.stop(STOP_MODE.ALLOWFADEOUT);
            this.activeSongs.Remove(stringList[index]);
            stringList.Remove(stringList[index]);
          }
        }
        songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
        if (!songInfo.ev.isValid())
          DebugUtil.LogWarningArgs((object) ("Failed to find FMOD event [" + songInfo.fmodEvent + "]"));
        int num4 = songInfo.numberOfVariations <= 0 ? -1 : UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1);
        if (num4 != -1)
        {
          int num5 = (int) songInfo.ev.setParameterValue("variation", (float) num4);
        }
        int num6 = (int) songInfo.ev.start();
        this.activeSongs[song_name] = songInfo;
      }
    }
  }

  public void StopSong(string song_name, bool shouldLog = true, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
  {
    if (shouldLog)
      this.Log("Stop: " + song_name);
    MusicManager.SongInfo songInfo1 = (MusicManager.SongInfo) null;
    if (!this.songMap.TryGetValue(song_name, out songInfo1))
      DebugUtil.LogErrorArgs((object) "Unknown song:", (object) song_name);
    else if (!this.activeSongs.ContainsKey(song_name))
    {
      DebugUtil.LogWarningArgs((object) "Trying to stop a song that isn't playing:", (object) song_name);
    }
    else
    {
      FMOD.Studio.EventInstance ev1 = songInfo1.ev;
      int num1 = (int) ev1.stop(stopMode);
      int num2 = (int) ev1.release();
      if (songInfo1.dynamic)
        this.activeDynamicSong = (MusicManager.SongInfo) null;
      if (songInfo1.songsOnHold.Count > 0)
      {
        for (int index = 0; index < songInfo1.songsOnHold.Count; ++index)
        {
          MusicManager.SongInfo songInfo2;
          if (this.activeSongs.TryGetValue(songInfo1.songsOnHold[index], out songInfo2) && songInfo2.ev.isValid())
          {
            FMOD.Studio.EventInstance ev2 = songInfo2.ev;
            this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
            int num3 = (int) ev2.setParameterValue("interrupted_dimmed", 0.0f);
            songInfo1.songsOnHold.Remove(songInfo1.songsOnHold[index]);
          }
          else
            songInfo1.songsOnHold.Remove(songInfo1.songsOnHold[index]);
        }
      }
      this.activeSongs.Remove(song_name);
    }
  }

  public void KillAllSongs(STOP_MODE stop_mode = STOP_MODE.IMMEDIATE)
  {
    this.Log("Kill All Songs");
    if (this.DynamicMusicIsActive())
      this.StopDynamicMusic(true);
    List<string> stringList = new List<string>((IEnumerable<string>) this.activeSongs.Keys);
    for (int index = 0; index < stringList.Count; ++index)
      this.StopSong(stringList[index], true, STOP_MODE.ALLOWFADEOUT);
  }

  public void SetSongParameter(
    string song_name,
    string parameter_name,
    float parameter_value,
    bool shouldLog = true)
  {
    if (shouldLog)
      this.Log(string.Format("Set Param {0}: {1}, {2}", (object) song_name, (object) parameter_name, (object) parameter_value));
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    if (!this.activeSongs.TryGetValue(song_name, out songInfo))
      return;
    FMOD.Studio.EventInstance ev = songInfo.ev;
    if (!ev.isValid())
      return;
    int num = (int) ev.setParameterValue(parameter_name, parameter_value);
  }

  public bool SongIsPlaying(string song_name)
  {
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
  }

  private void Update()
  {
    this.ClearFinishedSongs();
    if (!this.DynamicMusicIsActive())
      return;
    this.SetDynamicMusicZoomLevel();
    this.SetDynamicMusicTimeSinceLastJob();
    if (this.activeDynamicSong.useTimeOfDay)
      this.SetDynamicMusicTimeOfDay();
    if (!((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || (double) GameClock.Instance.GetCurrentCycleAsPercentage() < (double) this.duskTimePercentage / 100.0)
      return;
    this.StopDynamicMusic(false);
  }

  private void ClearFinishedSongs()
  {
    if (this.activeSongs.Count <= 0)
      return;
    ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
    foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
    {
      MusicManager.SongInfo songInfo = activeSong.Value;
      int playbackState = (int) songInfo.ev.getPlaybackState(out songInfo.musicPlaybackState);
      if (songInfo.musicPlaybackState == PLAYBACK_STATE.STOPPED || songInfo.musicPlaybackState == PLAYBACK_STATE.STOPPING)
      {
        pooledList.Add(activeSong.Key);
        foreach (string song_name in songInfo.songsOnHold)
          this.SetSongParameter(song_name, "interrupted_dimmed", 0.0f, true);
        songInfo.songsOnHold.Clear();
      }
    }
    foreach (string key in (List<string>) pooledList)
      this.activeSongs.Remove(key);
    pooledList.Recycle();
  }

  public void OnEscapeMenu(bool paused)
  {
    foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
    {
      if (activeSong.Value != null)
        this.StartFadeToPause(activeSong.Value.ev, paused, 0.25f);
    }
  }

  public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
  {
    if (paused)
      this.StartCoroutine(this.FadeToPause(inst, fadeTime));
    else
      this.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
  }

  [DebuggerHidden]
  private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new MusicManager.\u003CFadeToPause\u003Ec__Iterator0()
    {
      inst = inst,
      fadeTime = fadeTime
    };
  }

  [DebuggerHidden]
  private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new MusicManager.\u003CFadeToUnpause\u003Ec__Iterator1()
    {
      inst = inst,
      fadeTime = fadeTime
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!RuntimeManager.IsInitialized)
    {
      this.enabled = false;
    }
    else
    {
      if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
        return;
      this.alwaysPlayMusic = KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1;
    }
  }

  public void PlayDynamicMusic()
  {
    if (this.DynamicMusicIsActive())
    {
      this.Log("Trying to play DynamicMusic when it is already playing.");
    }
    else
    {
      string nextDynamicSong = this.GetNextDynamicSong();
      if (nextDynamicSong == "NONE")
        return;
      this.PlaySong(nextDynamicSong, false);
      MusicManager.SongInfo songInfo;
      if (this.activeSongs.TryGetValue(nextDynamicSong, out songInfo))
      {
        this.activeDynamicSong = songInfo;
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
        if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null && SpeedControlScreen.Instance.IsPaused)
          this.SetDynamicMusicPaused();
        if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
          this.SetDynamicMusicOverlayActive();
        this.SetDynamicMusicPlayHook();
        string key = "Volume_Music";
        if (KPlayerPrefs.HasKey(key))
          AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", KPlayerPrefs.GetFloat(key), true);
        AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
      }
      else
      {
        this.Log("DynamicMusic song " + nextDynamicSong + " did not start.");
        string str = string.Empty;
        foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
        {
          str = str + activeSong.Key + ", ";
          Debug.Log((object) str);
        }
        DebugUtil.DevAssert(false, "Song failed to play: " + nextDynamicSong);
      }
    }
  }

  public void StopDynamicMusic(bool stopImmediate = false)
  {
    if (this.activeDynamicSong == null)
      return;
    STOP_MODE stopMode = !stopImmediate ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;
    this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
    this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
    this.activeDynamicSong = (MusicManager.SongInfo) null;
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, STOP_MODE.ALLOWFADEOUT);
  }

  public string GetNextDynamicSong()
  {
    string str = string.Empty;
    if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
    {
      while (this.nextMusicType == MusicManager.TypeOfMusic.None)
        this.CycleToNextMusicType();
    }
    switch (this.nextMusicType)
    {
      case MusicManager.TypeOfMusic.DynamicSong:
        str = this.fullSongPlaylist.GetNextSong();
        this.activePlaylist = this.fullSongPlaylist;
        break;
      case MusicManager.TypeOfMusic.MiniSong:
        str = this.miniSongPlaylist.GetNextSong();
        this.activePlaylist = this.miniSongPlaylist;
        break;
      case MusicManager.TypeOfMusic.None:
        str = "NONE";
        this.activePlaylist = (MusicManager.DynamicSongPlaylist) null;
        break;
    }
    this.CycleToNextMusicType();
    return str;
  }

  private void CycleToNextMusicType()
  {
    this.musicTypeIterator = ++this.musicTypeIterator % this.musicStyleOrder.Length;
    this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
  }

  public bool DynamicMusicIsActive()
  {
    return this.activeDynamicSong != null;
  }

  public void SetDynamicMusicPaused()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
  }

  public void SetDynamicMusicUnpaused()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0.0f, true);
  }

  public void SetDynamicMusicZoomLevel()
  {
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", (float) (100.0 - (double) Camera.main.orthographicSize / 20.0 * 100.0), false);
  }

  public void SetDynamicMusicTimeSinceLastJob()
  {
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
  }

  public void SetDynamicMusicTimeOfDay()
  {
    if ((double) this.time >= (double) this.timeOfDayUpdateRate)
    {
      this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
      this.time = 0.0f;
    }
    this.time += Time.deltaTime;
  }

  public void SetDynamicMusicOverlayActive()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
  }

  public void SetDynamicMusicOverlayInactive()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0.0f, true);
  }

  public void SetDynamicMusicPlayHook()
  {
    if (!this.DynamicMusicIsActive())
      return;
    string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
    this.SetSongParameter(simpleSoundEventName, "playHook", !this.activeDynamicSong.playHook ? 0.0f : 1f, true);
    this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
  }

  public bool ShouldPlayDynamicMusicLoadedGame()
  {
    return (double) GameClock.Instance.GetCurrentCycleAsPercentage() <= (double) this.loadGameCutoffPercentage / 100.0;
  }

  public static MusicManager instance
  {
    get
    {
      return MusicManager._instance;
    }
  }

  protected override void OnPrefabInit()
  {
    MusicManager._instance = this;
    this.fullSongPlaylist.ResetUnplayedSongs();
    this.miniSongPlaylist.ResetUnplayedSongs();
    this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
  }

  protected override void OnCleanUp()
  {
    MusicManager._instance = (MusicManager) null;
  }

  [ContextMenu("Reload")]
  private void ReloadSongs()
  {
    this.songMap.Clear();
    foreach (MusicManager.DynamicSong fullSong in this.fullSongs)
    {
      string simpleSoundEventName = Assets.GetSimpleSoundEventName(fullSong.fmodEvent);
      MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
      songInfo.fmodEvent = fullSong.fmodEvent;
      songInfo.priority = 100;
      songInfo.interruptsActiveMusic = false;
      songInfo.dynamic = true;
      songInfo.useTimeOfDay = fullSong.useTimeOfDay;
      songInfo.numberOfVariations = fullSong.numberOfVariations;
      songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
      this.songMap[simpleSoundEventName] = songInfo;
      this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
    }
    foreach (MusicManager.Stinger miniSong in this.miniSongs)
    {
      string simpleSoundEventName = Assets.GetSimpleSoundEventName(miniSong.fmodEvent);
      MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
      songInfo.fmodEvent = miniSong.fmodEvent;
      songInfo.priority = 100;
      songInfo.interruptsActiveMusic = false;
      songInfo.dynamic = true;
      songInfo.useTimeOfDay = false;
      songInfo.numberOfVariations = 5;
      songInfo.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
      this.songMap[simpleSoundEventName] = songInfo;
      this.miniSongPlaylist.songMap[simpleSoundEventName] = songInfo;
    }
    foreach (MusicManager.Stinger stinger in this.stingers)
      this.SongMap[Assets.GetSimpleSoundEventName(stinger.fmodEvent)] = new MusicManager.SongInfo()
      {
        fmodEvent = stinger.fmodEvent,
        priority = 100,
        interruptsActiveMusic = true,
        dynamic = false,
        useTimeOfDay = false,
        numberOfVariations = 0
      };
    foreach (MusicManager.SongInfo menuSong in this.menuSongs)
      this.SongMap[Assets.GetSimpleSoundEventName(menuSong.fmodEvent)] = new MusicManager.SongInfo()
      {
        fmodEvent = menuSong.fmodEvent,
        priority = 100,
        interruptsActiveMusic = true,
        dynamic = false,
        useTimeOfDay = false,
        numberOfVariations = 0
      };
  }

  public void OnBeforeSerialize()
  {
  }

  public void OnAfterDeserialize()
  {
    this.ReloadSongs();
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class SongInfo
  {
    [NonSerialized]
    public List<string> songsOnHold = new List<string>();
    [NonSerialized]
    public bool playHook = true;
    [NonSerialized]
    public float sfxAttenuationPercentage = 65f;
    [EventRef]
    public string fmodEvent;
    [NonSerialized]
    public int priority;
    [NonSerialized]
    public bool interruptsActiveMusic;
    [NonSerialized]
    public bool dynamic;
    [NonSerialized]
    public bool useTimeOfDay;
    [NonSerialized]
    public int numberOfVariations;
    [NonSerialized]
    public FMOD.Studio.EventInstance ev;
    [NonSerialized]
    public PLAYBACK_STATE musicPlaybackState;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class DynamicSong
  {
    [EventRef]
    public string fmodEvent;
    [Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
    [SerializeField]
    public bool useTimeOfDay;
    [Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
    [SerializeField]
    public int numberOfVariations;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class Stinger
  {
    [EventRef]
    public string fmodEvent;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class Minisong
  {
    [EventRef]
    public string fmodEvent;
  }

  public class DynamicSongPlaylist
  {
    public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();
    public List<string> unplayedSongs = new List<string>();
    private string lastSongPlayed = string.Empty;

    public string GetNextSong()
    {
      string unplayedSong;
      if (this.unplayedSongs.Count > 0)
      {
        int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
        unplayedSong = this.unplayedSongs[index];
        this.unplayedSongs.RemoveAt(index);
      }
      else
      {
        this.ResetUnplayedSongs();
        bool flag = this.unplayedSongs.Count > 1;
        if (flag)
        {
          for (int index = 0; index < this.unplayedSongs.Count; ++index)
          {
            if (this.unplayedSongs[index] == this.lastSongPlayed)
            {
              this.unplayedSongs.Remove(this.unplayedSongs[index]);
              break;
            }
          }
        }
        int index1 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
        unplayedSong = this.unplayedSongs[index1];
        this.unplayedSongs.RemoveAt(index1);
        if (flag)
          this.unplayedSongs.Add(this.lastSongPlayed);
      }
      this.lastSongPlayed = unplayedSong;
      return Assets.GetSimpleSoundEventName(this.songMap[unplayedSong].fmodEvent);
    }

    public void ResetUnplayedSongs()
    {
      this.unplayedSongs.Clear();
      foreach (KeyValuePair<string, MusicManager.SongInfo> song in this.songMap)
        this.unplayedSongs.Add(song.Key);
    }
  }

  public enum TypeOfMusic
  {
    DynamicSong,
    MiniSong,
    None,
  }
}
