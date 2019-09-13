// Decompiled with JetBrains decompiler
// Type: MainMenuSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class MainMenuSoundEvent : SoundEvent
{
  public MainMenuSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, true, false, (float) SoundEvent.IGNORE_INTERVAL, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    EventInstance instance = KFMOD.BeginOneShot(this.sound, Vector3.zero);
    if (!instance.isValid())
      return;
    int num = (int) instance.setParameterValue("frame", (float) this.frame);
    KFMOD.EndOneShot(instance);
  }
}
