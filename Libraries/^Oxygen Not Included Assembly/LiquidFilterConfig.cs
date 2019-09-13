// Decompiled with JetBrains decompiler
// Type: LiquidFilterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidFilterConfig : IBuildingConfig
{
  private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));
  public const string ID = "LiquidFilter";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidFilter";
    int width = 3;
    int height = 1;
    string anim = "filter_liquid_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tieR1, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.PermittedRotations = PermittedRotations.R360;
    return buildingDef;
  }

  private void AttachPort(GameObject go)
  {
    go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
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

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<Structure>();
    go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
    go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Liquid;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
  }
}
