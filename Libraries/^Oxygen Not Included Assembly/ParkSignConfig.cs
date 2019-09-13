// Decompiled with JetBrains decompiler
// Type: ParkSignConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ParkSignConfig : IBuildingConfig
{
  public const string ID = "ParkSign";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ParkSign", 1, 2, "parksign_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ANY_BUILDABLE, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
    buildingDef.AudioCategory = "Metal";
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Park, false);
    go.AddOrGet<ParkSign>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
