// Decompiled with JetBrains decompiler
// Type: ManualGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ManualGeneratorConfig : IBuildingConfig
{
  public const string ID = "ManualGenerator";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "ManualGenerator";
    int width = 2;
    int height = 2;
    string anim = "generatormanual_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR3_2, 0.2f);
    buildingDef.GeneratorWattageRating = 400f;
    buildingDef.GeneratorBaseCapacity = 10000f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Breakable = true;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
    go.AddOrGet<LogicOperationalController>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    go.AddOrGet<Generator>().powerDistributionOrder = 10;
    ManualGenerator manualGenerator = go.AddOrGet<ManualGenerator>();
    manualGenerator.SetSliderValue(50f, 0);
    manualGenerator.workLayer = Grid.SceneLayer.BuildingFront;
    KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
    kbatchedAnimController.initialAnim = "off";
    Tinkerable.MakePowerTinkerable(go);
  }
}
