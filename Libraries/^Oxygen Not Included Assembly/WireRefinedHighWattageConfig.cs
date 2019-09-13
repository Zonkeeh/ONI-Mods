// Decompiled with JetBrains decompiler
// Type: WireRefinedHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedHighWattageConfig : BaseWireConfig
{
  public const string ID = "WireRefinedHighWattage";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WireRefinedHighWattage";
    string anim = "utilities_electric_conduct_hiwatt_kanim";
    float construction_time = 3f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    float insulation = 0.05f;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = this.CreateBuildingDef(id, anim, construction_time, tieR2, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }
}
