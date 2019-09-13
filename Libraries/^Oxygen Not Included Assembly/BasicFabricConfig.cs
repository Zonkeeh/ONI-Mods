// Decompiled with JetBrains decompiler
// Type: BasicFabricConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class BasicFabricConfig : IEntityConfig
{
  public static string ID = "BasicFabric";
  private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, (string) ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true, false, true);

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(BasicFabricConfig.ID, (string) ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.DESC, 1f, true, Assets.GetAnim((HashedString) "swampreedwool_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, SimHashes.Creature, new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.BuildingFiber
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
