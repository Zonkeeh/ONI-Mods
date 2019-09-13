// Decompiled with JetBrains decompiler
// Type: CreatureChewSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class CreatureChewSoundEvent : SoundEvent
{
  private static string DEFAULT_CHEW_SOUND = "Rock";
  private const string FMOD_PARAM_IS_BABY_ID = "isBaby";

  public CreatureChewSoundEvent(
    string file_name,
    string sound_name,
    int frame,
    float min_interval)
    : base(file_name, sound_name, frame, false, false, min_interval, true)
  {
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    string sound = GlobalAssets.GetSound(StringFormatter.Combine(this.name, "_", CreatureChewSoundEvent.GetChewSound(behaviour)), false);
    if (!SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic))
      return;
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    EventInstance instance = SoundEvent.BeginOneShot(sound, position);
    if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
    {
      int num = (int) instance.setParameterValue("isBaby", 1f);
    }
    SoundEvent.EndOneShot(instance);
  }

  private static string GetChewSound(AnimEventManager.EventPlayerData behaviour)
  {
    string str = CreatureChewSoundEvent.DEFAULT_CHEW_SOUND;
    EatStates.Instance smi = behaviour.controller.GetSMI<EatStates.Instance>();
    if (smi != null)
    {
      Element latestMealElement = smi.GetLatestMealElement();
      if (latestMealElement != null)
      {
        string creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
        if (!string.IsNullOrEmpty(creatureChewSound))
          str = creatureChewSound;
      }
    }
    return str;
  }
}
