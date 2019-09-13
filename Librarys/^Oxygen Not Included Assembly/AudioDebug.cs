// Decompiled with JetBrains decompiler
// Type: AudioDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AudioDebug : KMonoBehaviour
{
  private static AudioDebug instance;
  public bool musicEnabled;
  public bool debugSoundEvents;
  public bool debugFloorSounds;
  public bool debugGameEventSounds;
  public bool debugNotificationSounds;
  public bool debugVoiceSounds;

  public static AudioDebug Get()
  {
    return AudioDebug.instance;
  }

  protected override void OnPrefabInit()
  {
    AudioDebug.instance = this;
  }

  public void ToggleMusic()
  {
    if ((Object) Game.Instance != (Object) null)
      Game.Instance.SetMusicEnabled(this.musicEnabled);
    this.musicEnabled = !this.musicEnabled;
  }
}
