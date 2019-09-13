// Decompiled with JetBrains decompiler
// Type: ChlorineGeyserConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ChlorineGeyserConfig : IEntityConfig
{
  public const string ID = "ChlorineGeyser";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ChlorineGeyser", (string) STRINGS.CREATURES.SPECIES.CHLORINEGEYSER.NAME, (string) STRINGS.CREATURES.SPECIES.CHLORINEGEYSER.DESC, 2000f, Assets.GetAnim((HashedString) "geyser_side_chlorine_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, TUNING.BUILDINGS.DECOR.BONUS.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER5, SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.IgneousRock);
    component.Temperature = 372.15f;
    placedEntity.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
    GeyserConfigurator geyserConfigurator = placedEntity.AddOrGet<GeyserConfigurator>();
    geyserConfigurator.presetType = (HashedString) "chlorine_gas";
    geyserConfigurator.presetMin = 0.35f;
    geyserConfigurator.presetMax = 0.65f;
    Studyable studyable = placedEntity.AddOrGet<Studyable>();
    studyable.meterTrackerSymbol = "geotracker_target";
    studyable.meterAnim = "tracker";
    placedEntity.AddOrGet<LoopingSounds>();
    SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim", "GeyserMethane_shake_LP", TUNING.NOISE_POLLUTION.NOISY.TIER5);
    SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim", "GeyserMethane_shake_LP", TUNING.NOISE_POLLUTION.NOISY.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
