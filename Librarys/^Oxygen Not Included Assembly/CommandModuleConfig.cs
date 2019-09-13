// Decompiled with JetBrains decompiler
// Type: CommandModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CommandModuleConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort((HashedString) "TriggerLaunch", new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_INACTIVE, false, false)
  };
  private static readonly LogicPorts.Port[] OUTPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.OutputPort((HashedString) "LaunchReady", new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_INACTIVE, false, false)
  };
  public const string ID = "CommandModule";
  private const string TRIGGER_LAUNCH_PORT_ID = "TriggerLaunch";
  private const string LAUNCH_READY_PORT_ID = "LaunchReady";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "CommandModule";
    int width = 5;
    int height = 5;
    string anim = "rocket_command_module_kanim";
    int hitpoints = 1000;
    float construction_time = 60f;
    float[] commandModuleMass = TUNING.BUILDINGS.ROCKETRY_MASS_KG.COMMAND_MODULE_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 9999f;
    BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
    EffectorValues tieR2 = TUNING.NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, commandModuleMass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_command_module_bg_kanim"));
    LaunchConditionManager conditionManager = go.AddOrGet<LaunchConditionManager>();
    conditionManager.triggerPort = (HashedString) "TriggerLaunch";
    conditionManager.statusPort = (HashedString) "LaunchReady";
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    go.AddOrGet<CommandModule>();
    go.AddOrGet<CommandModuleWorkable>();
    go.AddOrGet<MinionStorage>();
    go.AddOrGet<ArtifactFinder>();
    go.AddOrGet<LaunchableRocket>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CommandModuleConfig.INPUT_PORTS, CommandModuleConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CommandModuleConfig.INPUT_PORTS, CommandModuleConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, CommandModuleConfig.INPUT_PORTS, CommandModuleConfig.OUTPUT_PORTS);
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.RocketCommandModule.Id;
    ownable.canBePublic = false;
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
