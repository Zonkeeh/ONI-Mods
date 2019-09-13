// Decompiled with JetBrains decompiler
// Type: WorldGenScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using ProcGenGame;
using System;
using System.IO;

public class WorldGenScreen : NewGameFlowScreen
{
  [MyCmpReq]
  private OfflineWorldGen offlineWorldGen;
  public static WorldGenScreen Instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    WorldGenScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.TriggerLoadingMusic();
    UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
    SaveLoader.SetActiveSaveFilePath((string) null);
    try
    {
      File.Delete(WorldGen.SIM_SAVE_FILENAME);
    }
    catch (Exception ex)
    {
      DebugUtil.LogWarningArgs((object) ex.ToString());
    }
    this.offlineWorldGen.Generate();
  }

  private void TriggerLoadingMusic()
  {
    if (!AudioDebug.Get().musicEnabled || MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
      return;
    MusicManager.instance.StopSong("Music_TitleTheme", true, STOP_MODE.ALLOWFADEOUT);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
    MusicManager.instance.PlaySong("Music_FrontEnd", false);
    MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed)
      e.TryConsume(Action.Escape);
    base.OnKeyDown(e);
  }
}
