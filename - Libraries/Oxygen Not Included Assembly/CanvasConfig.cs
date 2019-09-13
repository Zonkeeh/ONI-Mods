// Decompiled with JetBrains decompiler
// Type: CanvasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CanvasConfig : IBuildingConfig
{
  public const string ID = "Canvas";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Canvas", 2, 2, "painting_kanim", 30, 120f, new float[2]
    {
      400f,
      1f
    }, new string[2]{ "Metal", "BuildingFiber" }, 1600f, BuildLocationRule.Anywhere, new EffectorValues()
    {
      amount = 10,
      radius = 6
    }, TUNING.NOISE_POLLUTION.NONE, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.DefaultAnimState = "off";
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
    Artable artable = (Artable) go.AddComponent<Painting>();
    artable.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.NAME, "off", 0, false, Artable.Status.Ready));
    artable.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.POORQUALITYNAME, "art_a", 5, false, Artable.Status.Ugly));
    artable.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.AVERAGEQUALITYNAME, "art_b", 10, false, Artable.Status.Okay));
    artable.stages.Add(new Artable.Stage("Good", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_c", 15, true, Artable.Status.Great));
    artable.stages.Add(new Artable.Stage("Good2", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_d", 15, true, Artable.Status.Great));
    artable.stages.Add(new Artable.Stage("Good3", (string) STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_e", 15, true, Artable.Status.Great));
  }
}
