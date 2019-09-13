// Decompiled with JetBrains decompiler
// Type: JetSuitMarkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class JetSuitMarkerConfig : IBuildingConfig
{
  public const string ID = "JetSuitMarker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "JetSuitMarker";
    int width = 2;
    int height = 4;
    string anim = "changingarea_jetsuit_arrow_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[1]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
    }, refinedMetals, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingUse;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitMarker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
    suitMarker.LockerTags = new Tag[1]
    {
      new Tag("JetSuitLocker")
    };
    suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasJetPack;
    suitMarker.interactAnim = Assets.GetAnim((HashedString) "anim_interacts_changingarea_jetsuit_arrow_kanim");
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("JetSuitMarker"),
      new Tag("JetSuitLocker")
    };
    go.AddTag(GameTags.JetSuitBlocker);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
