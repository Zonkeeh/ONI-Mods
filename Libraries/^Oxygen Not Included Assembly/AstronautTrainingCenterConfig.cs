// Decompiled with JetBrains decompiler
// Type: AstronautTrainingCenterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class AstronautTrainingCenterConfig : IBuildingConfig
{
  public const string ID = "AstronautTrainingCenter";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "AstronautTrainingCenter";
    int width = 5;
    int height = 5;
    string anim = "centrifuge_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR1, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.PowerInputOffset = new CellOffset(-2, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.Deprecated = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    AstronautTrainingCenter astronautTrainingCenter = go.AddOrGet<AstronautTrainingCenter>();
    astronautTrainingCenter.workTime = float.PositiveInfinity;
    astronautTrainingCenter.requiredSkillPerk = Db.Get().SkillPerks.CanTrainToBeAstronaut.Id;
    astronautTrainingCenter.daysToMasterRole = 10f;
    astronautTrainingCenter.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_centrifuge_kanim")
    };
    astronautTrainingCenter.workLayer = Grid.SceneLayer.BuildingFront;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
