// Decompiled with JetBrains decompiler
// Type: CornerMouldingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CornerMouldingConfig : IBuildingConfig
{
  public const string ID = "CornerMoulding";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CornerMoulding", 1, 1, "corner_tile_kanim", 10, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.RAW_MINERALS, 800f, BuildLocationRule.InCorner, new EffectorValues()
    {
      amount = 5,
      radius = 3
    }, NOISE_POLLUTION.NONE, 0.2f);
    buildingDef.DefaultAnimState = "corner";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
