// Decompiled with JetBrains decompiler
// Type: LaserSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class LaserSoundEvent : SoundEvent
{
  public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, true, min_interval, false)
  {
    this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (LaserSoundEvent), sound_name);
  }
}
