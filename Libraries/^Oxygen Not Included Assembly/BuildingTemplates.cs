// Decompiled with JetBrains decompiler
// Type: BuildingTemplates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BuildingTemplates
{
  public static BuildingDef CreateBuildingDef(
    string id,
    int width,
    int height,
    string anim,
    int hitpoints,
    float construction_time,
    float[] construction_mass,
    string[] construction_materials,
    float melting_point,
    BuildLocationRule build_location_rule,
    EffectorValues decor,
    EffectorValues noise,
    float temperature_modification_mass_scale = 0.2f)
  {
    BuildingDef instance = ScriptableObject.CreateInstance<BuildingDef>();
    instance.PrefabID = id;
    instance.InitDef();
    instance.name = id;
    instance.Mass = construction_mass;
    instance.MassForTemperatureModification = construction_mass[0] * temperature_modification_mass_scale;
    instance.WidthInCells = width;
    instance.HeightInCells = height;
    instance.HitPoints = hitpoints;
    instance.ConstructionTime = construction_time;
    instance.SceneLayer = Grid.SceneLayer.Building;
    instance.MaterialCategory = construction_materials;
    instance.BaseMeltingPoint = melting_point;
    switch (build_location_rule)
    {
      case BuildLocationRule.Tile:
      case BuildLocationRule.Conduit:
      case BuildLocationRule.LogicBridge:
      case BuildLocationRule.WireBridge:
        instance.ContinuouslyCheckFoundation = false;
        break;
      default:
        if (build_location_rule != BuildLocationRule.Anywhere)
        {
          instance.ContinuouslyCheckFoundation = true;
          break;
        }
        goto case BuildLocationRule.Tile;
    }
    instance.BuildLocationRule = build_location_rule;
    instance.ObjectLayer = ObjectLayer.Building;
    instance.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) anim)
    };
    instance.GenerateOffsets();
    instance.BaseDecor = (float) decor.amount;
    instance.BaseDecorRadius = (float) decor.radius;
    instance.BaseNoisePollution = noise.amount;
    instance.BaseNoisePollutionRadius = noise.radius;
    return instance;
  }

  public static void CreateStandardBuildingDef(BuildingDef def)
  {
    def.Breakable = true;
  }

  public static void CreateFoundationTileDef(BuildingDef def)
  {
    def.IsFoundation = true;
    def.TileLayer = ObjectLayer.FoundationTile;
    def.ReplacementLayer = ObjectLayer.ReplacementTile;
    def.ReplacementCandidateLayers = new List<ObjectLayer>()
    {
      ObjectLayer.FoundationTile,
      ObjectLayer.LadderTile
    };
    def.ReplacementTags = new List<Tag>()
    {
      GameTags.FloorTiles,
      GameTags.Ladders
    };
    def.EquivalentReplacementLayers = new List<ObjectLayer>()
    {
      ObjectLayer.ReplacementLadder
    };
  }

  public static void CreateLadderDef(BuildingDef def)
  {
    def.TileLayer = ObjectLayer.LadderTile;
    def.ReplacementLayer = ObjectLayer.ReplacementLadder;
    def.ReplacementTags = new List<Tag>()
    {
      GameTags.Ladders
    };
    def.EquivalentReplacementLayers = new List<ObjectLayer>()
    {
      ObjectLayer.ReplacementTile
    };
  }

  public static void CreateElectricalBuildingDef(BuildingDef def)
  {
    BuildingTemplates.CreateStandardBuildingDef(def);
    def.RequiresPowerInput = true;
    def.ViewMode = OverlayModes.Power.ID;
    def.AudioCategory = "HollowMetal";
  }

  public static void CreateRocketBuildingDef(BuildingDef def)
  {
    BuildingTemplates.CreateStandardBuildingDef(def);
    def.Invincible = true;
    def.DefaultAnimState = "grounded";
  }

  public static void CreateMonumentBuildingDef(BuildingDef def)
  {
    BuildingTemplates.CreateStandardBuildingDef(def);
    def.Invincible = true;
  }

  public static Storage CreateDefaultStorage(GameObject go, bool forceCreate = false)
  {
    Storage storage = !forceCreate ? go.AddOrGet<Storage>() : go.AddComponent<Storage>();
    storage.capacityKg = 2000f;
    return storage;
  }

  public static void CreateComplexFabricatorStorage(GameObject go, ComplexFabricator fabricator)
  {
    fabricator.inStorage = go.AddComponent<Storage>();
    fabricator.inStorage.capacityKg = 20000f;
    fabricator.inStorage.showInUI = true;
    fabricator.inStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    fabricator.buildStorage = go.AddComponent<Storage>();
    fabricator.buildStorage.capacityKg = 20000f;
    fabricator.buildStorage.showInUI = true;
    fabricator.buildStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    fabricator.outStorage = go.AddComponent<Storage>();
    fabricator.outStorage.capacityKg = 20000f;
    fabricator.outStorage.showInUI = true;
    fabricator.outStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
  }

  public static void DoPostConfigure(GameObject go)
  {
  }
}
