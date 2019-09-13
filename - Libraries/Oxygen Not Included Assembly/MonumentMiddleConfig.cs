// Decompiled with JetBrains decompiler
// Type: MonumentMiddleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MonumentMiddleConfig : IBuildingConfig
{
  public const string ID = "MonumentMiddle";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MonumentMiddle";
    int width = 5;
    int height = 5;
    string anim = "victory_monument_mid_kanim";
    int hitpoints = 1000;
    float construction_time = 60f;
    float[] construction_mass = new float[3]
    {
      2500f,
      2500f,
      5000f
    };
    string[] construction_materials = new string[3]
    {
      SimHashes.Ceramic.ToString(),
      SimHashes.Polypropylene.ToString(),
      SimHashes.Steel.ToString()
    };
    float melting_point = 9999f;
    BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tieR2, 0.2f);
    BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = (Tag) "MonumentMiddle";
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), (Tag) "MonumentTop", (AttachableBuilding) null)
    };
    go.AddOrGet<MonumentPart>().part = MonumentPart.Part.Middle;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
      monumentPart.part = MonumentPart.Part.Middle;
      monumentPart.selectableStatesAndSymbols = new List<Tuple<string, string>>();
      monumentPart.stateUISymbol = "mid";
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_a", "thumbs_up"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_b", "wrench"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_c", "hmmm"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_d", "hips_hands"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_e", "hold_face"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_f", "finger_gun"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_g", "model_pose"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_h", "punch"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_i", "holding_hatch"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_j", "model_pose2"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_k", "balancing"));
      monumentPart.selectableStatesAndSymbols.Add(new Tuple<string, string>("option_l", "holding_babies"));
    });
  }
}
