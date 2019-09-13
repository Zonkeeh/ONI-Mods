// Decompiled with JetBrains decompiler
// Type: EggCrackerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[EntityConfigOrder(2)]
public class EggCrackerConfig : IBuildingConfig
{
  public const string ID = "EggCracker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "EggCracker";
    int width = 2;
    int height = 2;
    string anim = "egg_cracker_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_egg", false);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.labelByResult = false;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_egg_cracker_kanim")
    };
    fabricator.outputOffset = new Vector3(1f, 1f, 0.0f);
    Prioritizable.AddRef(go);
    go.AddOrGet<EggCracker>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
