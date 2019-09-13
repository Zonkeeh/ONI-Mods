// Decompiled with JetBrains decompiler
// Type: SmallSculptureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SmallSculptureConfig : IBuildingConfig
{
  public const string ID = "SmallSculpture";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SmallSculpture", 1, 2, "sculpture_1x2_kanim", 10, 60f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.RAW_MINERALS, 1600f, BuildLocationRule.OnFloor, new EffectorValues()
    {
      amount = 5,
      radius = 4
    }, TUNING.NOISE_POLLUTION.NONE, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.DefaultAnimState = "slab";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isArtable = true;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Artable artable = (Artable) go.AddComponent<Sculpture>();
    artable.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
    artable.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.POORQUALITYNAME, "crap_1", 5, false, Artable.Status.Ugly));
    artable.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.AVERAGEQUALITYNAME, "good_1", 10, false, Artable.Status.Okay));
    artable.stages.Add(new Artable.Stage("Good", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_1", 15, true, Artable.Status.Great));
    artable.stages.Add(new Artable.Stage("Good2", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_2", 15, true, Artable.Status.Great));
    artable.stages.Add(new Artable.Stage("Good3", (string) STRINGS.BUILDINGS.PREFABS.SMALLSCULPTURE.EXCELLENTQUALITYNAME, "amazing_3", 15, true, Artable.Status.Great));
  }
}
