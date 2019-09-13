// Decompiled with JetBrains decompiler
// Type: MouthFlapSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class MouthFlapSoundEvent : SoundEvent
{
  public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping)
    : base(file_name, sound_name, frame, false, is_looping, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(this.name, (string) null);
  }
}
