// Decompiled with JetBrains decompiler
// Type: NewGameSettingsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class NewGameSettingsScreen : NewGameFlowScreen
{
  [Header("Static UI Refs")]
  [SerializeField]
  private MultiToggle toggle_standard_game;
  [SerializeField]
  private MultiToggle toggle_custom_game;
  [SerializeField]
  private KButton button_cancel;
  [SerializeField]
  private KButton button_start;
  [SerializeField]
  private KButton button_close;
  [SerializeField]
  private GameObject disable_custom_settings_shroud;
  [MyCmpReq]
  private NewGameSettingsPanel panel;

  protected override void OnSpawn()
  {
  }

  private void SetGameTypeToggle(bool custom_game)
  {
  }

  private void Cancel()
  {
    this.panel.Cancel();
    this.NavigateBackward();
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
}
