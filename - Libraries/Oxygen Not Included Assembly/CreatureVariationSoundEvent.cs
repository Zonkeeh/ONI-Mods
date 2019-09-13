// Decompiled with JetBrains decompiler
// Type: CreatureVariationSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CreatureVariationSoundEvent : SoundEvent
{
  public CreatureVariationSoundEvent(
    string file_name,
    string sound_name,
    int frame,
    bool do_load,
    bool is_looping,
    float min_interval,
    bool is_dynamic)
    : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    string sound1 = this.sound;
    CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
    if ((Object) component != (Object) null && !string.IsNullOrEmpty(component.symbolPrefix))
    {
      string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, this.name), false);
      if (!string.IsNullOrEmpty(sound2))
        sound1 = sound2;
    }
    this.PlaySound(behaviour, sound1);
  }
}
