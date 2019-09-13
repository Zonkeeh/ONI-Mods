// Decompiled with JetBrains decompiler
// Type: BuildingDamageSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class BuildingDamageSoundEvent : SoundEvent
{
  public BuildingDamageSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    Worker component1 = behaviour.GetComponent<Worker>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      SoundEvent.PlayOneShot(GlobalAssets.GetSound("Building_Dmg_Metal", false), position);
    }
    else
    {
      Workable workable = component1.workable;
      if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
        return;
      Building component2 = workable.GetComponent<Building>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      string sound = GlobalAssets.GetSound(StringFormatter.Combine(this.name, "_", component2.Def.AudioCategory), false) ?? GlobalAssets.GetSound("Building_Dmg_Metal", false);
      if (sound == null)
        return;
      SoundEvent.PlayOneShot(sound, position);
    }
  }
}
