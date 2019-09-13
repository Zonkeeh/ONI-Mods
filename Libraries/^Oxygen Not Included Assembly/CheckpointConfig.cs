// Decompiled with JetBrains decompiler
// Type: CheckpointConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CheckpointConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(Checkpoint.PORT_ID, new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_INACTIVE, true, false)
  };
  public const string ID = "Checkpoint";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Checkpoint";
    int width = 1;
    int height = 3;
    string anim = "checkpoint_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR0 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, refinedMetals, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tieR0, 0.2f);
    buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 2);
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CheckpointConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CheckpointConfig.INPUT_PORTS);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Checkpoint>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CheckpointConfig.INPUT_PORTS);
  }
}
