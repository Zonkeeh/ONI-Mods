// Decompiled with JetBrains decompiler
// Type: Ladder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class Ladder : KMonoBehaviour, IEffectDescriptor
{
  public float upwardsMovementSpeedMultiplier = 1f;
  public float downwardsMovementSpeedMultiplier = 1f;
  public bool isPole;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.HasPole[cell] = this.isPole;
    Grid.HasLadder[cell] = !this.isPole;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Ladders, false);
    Components.Ladders.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, (object) null);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if ((Object) Grid.Objects[cell, 24] == (Object) null)
    {
      Grid.HasPole[cell] = false;
      Grid.HasLadder[cell] = false;
    }
    Components.Ladders.Remove(this);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = (List<Descriptor>) null;
    if ((double) this.upwardsMovementSpeedMultiplier != 1.0)
    {
      descriptorList = new List<Descriptor>();
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0), GameUtil.TimeSlice.None)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0), GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }
}
