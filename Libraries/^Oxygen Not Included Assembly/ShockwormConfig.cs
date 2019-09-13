// Decompiled with JetBrains decompiler
// Type: ShockwormConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ShockwormConfig : IEntityConfig
{
  public const string ID = "ShockWorm";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ShockWorm", (string) STRINGS.CREATURES.SPECIES.SHOCKWORM.NAME, (string) STRINGS.CREATURES.SPECIES.SHOCKWORM.DESC, 50f, Assets.GetAnim((HashedString) "shockworm_kanim"), "idle", Grid.SceneLayer.Creatures, 1, 2, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Hostile, (string) null, "FlyerNavGrid1x2", NavType.Hover, 32, 2f, "Meat", 3, true, true, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.HOT_1, TUNING.CREATURES.TEMPERATURE.FREEZING_2, TUNING.CREATURES.TEMPERATURE.HOT_2);
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddWeapon(3f, 6f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 10, 4f).AddEffect("WasAttacked", 1f);
    SoundEventVolumeCache.instance.AddVolume("shockworm_kanim", "Shockworm_attack_arc", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
