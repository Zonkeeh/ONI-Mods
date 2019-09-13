// Decompiled with JetBrains decompiler
// Type: WallDamageSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class WallDamageSoundEvent : SoundEvent
{
  public int tile;

  public WallDamageSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, false, min_interval, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 vector3 = new Vector3();
    AggressiveChore.StatesInstance smi = behaviour.controller.gameObject.GetSMI<AggressiveChore.StatesInstance>();
    if (smi == null)
      return;
    this.tile = smi.sm.wallCellToBreak;
    int audioCategory = WallDamageSoundEvent.GetAudioCategory(this.tile);
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, Grid.CellToPos(this.tile));
    int num = (int) instance.setParameterValue("material_ID", (float) audioCategory);
    SoundEvent.EndOneShot(instance);
  }

  private static int GetAudioCategory(int tile)
  {
    Element element = Grid.Element[tile];
    if (Grid.Foundation[tile])
      return 12;
    if (element.id == SimHashes.Dirt)
      return 0;
    if (element.id == SimHashes.CrushedIce || element.id == SimHashes.Ice || element.id == SimHashes.DirtyIce)
      return 1;
    if (element.id == SimHashes.OxyRock)
      return 3;
    if (element.HasTag(GameTags.Metal))
      return 5;
    if (element.HasTag(GameTags.RefinedMetal))
      return 6;
    if (element.id == SimHashes.Sand)
      return 8;
    return element.id == SimHashes.Algae ? 10 : 7;
  }
}
