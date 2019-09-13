// Decompiled with JetBrains decompiler
// Type: EffectConfigs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EffectConfigs : IMultiEntityConfig
{
  public static string EffectTemplateId = "EffectTemplateFx";
  public static string AttackSplashId = "AttackSplashFx";
  public static string OreAbsorbId = "OreAbsorbFx";
  public static string PlantDeathId = "PlantDeathFx";
  public static string BuildSplashId = "BuildSplashFx";

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>[] anonType0Array = new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>[5]
    {
      new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>(EffectConfigs.EffectTemplateId, new string[0], string.Empty, KAnim.PlayMode.Once, false),
      new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>(EffectConfigs.AttackSplashId, new string[1]
      {
        "attack_beam_contact_fx_kanim"
      }, "loop", KAnim.PlayMode.Loop, false),
      new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>(EffectConfigs.OreAbsorbId, new string[1]
      {
        "ore_collision_kanim"
      }, "idle", KAnim.PlayMode.Once, true),
      new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>(EffectConfigs.PlantDeathId, new string[1]
      {
        "plant_death_fx_kanim"
      }, "plant_death", KAnim.PlayMode.Once, true),
      new \u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool>(EffectConfigs.BuildSplashId, new string[1]
      {
        "sparks_radial_build_kanim"
      }, "loop", KAnim.PlayMode.Loop, false)
    };
    foreach (\u003C\u003E__AnonType0<string, string[], string, KAnim.PlayMode, bool> anonType0 in anonType0Array)
    {
      GameObject entity = EntityTemplates.CreateEntity(anonType0.id, anonType0.id, false);
      KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.Simple;
      kbatchedAnimController.initialAnim = anonType0.initialAnim;
      kbatchedAnimController.initialMode = anonType0.initialMode;
      kbatchedAnimController.isMovable = true;
      kbatchedAnimController.destroyOnAnimComplete = anonType0.destroyOnAnimComplete;
      if (anonType0.animFiles.Length > 0)
      {
        KAnimFile[] kanimFileArray = new KAnimFile[anonType0.animFiles.Length];
        for (int index = 0; index < kanimFileArray.Length; ++index)
          kanimFileArray[index] = Assets.GetAnim((HashedString) anonType0.animFiles[index]);
        kbatchedAnimController.AnimFiles = kanimFileArray;
      }
      entity.AddOrGet<LoopingSounds>();
      gameObjectList.Add(entity);
    }
    return gameObjectList;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
