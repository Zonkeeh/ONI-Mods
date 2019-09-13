// Decompiled with JetBrains decompiler
// Type: IceSculptureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IceSculptureConfig : IBuildingConfig
{
  public const string ID = "IceSculpture";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("IceSculpture", 2, 2, "icesculpture_kanim", 10, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, new string[1]
    {
      "Ice"
    }, 273.15f, BuildLocationRule.OnFloor, new EffectorValues()
    {
      amount = 20,
      radius = 8
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
    artable.stages.Add(new Artable.Stage("Default", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
    artable.stages.Add(new Artable.Stage("Bad", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.POORQUALITYNAME, "crap", 5, false, Artable.Status.Ugly));
    artable.stages.Add(new Artable.Stage("Average", (string) STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.AVERAGEQUALITYNAME, "idle", 10, true, Artable.Status.Okay));
  }
}
