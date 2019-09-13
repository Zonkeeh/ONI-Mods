// Decompiled with JetBrains decompiler
// Type: SuitMarkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SuitMarkerConfig : IBuildingConfig
{
  public const string ID = "SuitMarker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SuitMarker";
    int width = 1;
    int height = 3;
    string anim = "changingarea_arrow_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, refinedMetals, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
    suitMarker.LockerTags = new Tag[1]
    {
      new Tag("SuitLocker")
    };
    suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("SuitMarker"),
      new Tag("SuitLocker")
    };
    go.AddTag(GameTags.JetSuitBlocker);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
