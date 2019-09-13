// Decompiled with JetBrains decompiler
// Type: SolidVentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolidVentConfig : IBuildingConfig
{
  public const string ID = "SolidVent";
  private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SolidVent";
    int width = 1;
    int height = 1;
    string anim = "conveyer_dropper_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.InputConduitType = ConduitType.Solid;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidVent");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<SimpleVent>();
    go.AddOrGet<SolidConduitConsumer>();
    go.AddOrGet<SolidConduitDropper>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.capacityKg = 100f;
    defaultStorage.showInUI = true;
  }
}
