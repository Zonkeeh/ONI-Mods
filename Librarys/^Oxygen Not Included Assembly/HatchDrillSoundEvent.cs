// Decompiled with JetBrains decompiler
// Type: HatchDrillSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class HatchDrillSoundEvent : SoundEvent
{
  public HatchDrillSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, true, min_interval, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    float audioCategory = (float) HatchDrillSoundEvent.GetAudioCategory(Grid.CellBelow(Grid.PosToCell(position)));
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, position);
    int num = (int) instance.setParameterValue("material_ID", audioCategory);
    SoundEvent.EndOneShot(instance);
  }

  private static int GetAudioCategory(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return 7;
    Element element = Grid.Element[cell];
    if (element.id == SimHashes.Dirt)
      return 0;
    if (element.HasTag(GameTags.IceOre))
      return 1;
    if (element.id == SimHashes.CrushedIce)
      return 12;
    if (element.id == SimHashes.DirtyIce)
      return 13;
    if (Grid.Foundation[cell])
      return 2;
    if (element.id == SimHashes.OxyRock)
      return 3;
    if (element.id == SimHashes.PhosphateNodules || element.id == SimHashes.Phosphorus || element.id == SimHashes.Phosphorite)
      return 4;
    if (element.HasTag(GameTags.Metal))
      return 5;
    if (element.HasTag(GameTags.RefinedMetal))
      return 6;
    if (element.id == SimHashes.Sand)
      return 8;
    if (element.id == SimHashes.Clay)
      return 9;
    if (element.id == SimHashes.Algae)
      return 10;
    return element.id == SimHashes.SlimeMold ? 11 : 7;
  }
}
