// Decompiled with JetBrains decompiler
// Type: LadderSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LadderSoundEvent : SoundEvent
{
  public LadderSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    int cell = Grid.PosToCell(position);
    BuildingDef buildingDef = (BuildingDef) null;
    if (Grid.IsValidCell(cell))
    {
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((Object) gameObject != (Object) null && (Object) gameObject.GetComponent<Ladder>() != (Object) null)
      {
        Building component = (Building) gameObject.GetComponent<BuildingComplete>();
        if ((Object) component != (Object) null)
          buildingDef = component.Def;
      }
    }
    if (!((Object) buildingDef != (Object) null))
      return;
    string sound = GlobalAssets.GetSound(!(buildingDef.PrefabID == "LadderFast") ? this.name : StringFormatter.Combine(this.name, "_Plastic"), false);
    if (sound == null)
      return;
    SoundEvent.PlayOneShot(sound, position);
  }
}
