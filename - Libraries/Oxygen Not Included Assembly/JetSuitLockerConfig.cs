// Decompiled with JetBrains decompiler
// Type: JetSuitLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class JetSuitLockerConfig : IBuildingConfig
{
  private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
  public const string ID = "JetSuitLocker";
  public const float O2_CAPACITY = 200f;
  public const float SUIT_CAPACITY = 200f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "JetSuitLocker";
    int width = 2;
    int height = 4;
    string anim = "changingarea_jetsuit_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[1]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
    }, refinedMetals, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitLocker");
    return buildingDef;
  }

  private void AttachPort(GameObject go)
  {
    go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryInputPort;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<SuitLocker>().OutfitTags = new Tag[1]
    {
      GameTags.JetSuit
    };
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.capacityKG = 200f;
    go.AddComponent<JetSuitLocker>().portInfo = this.secondaryInputPort;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("JetSuitLocker"),
      new Tag("JetSuitMarker")
    };
    go.AddOrGet<Storage>().capacityKg = 500f;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AttachPort(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AttachPort(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
