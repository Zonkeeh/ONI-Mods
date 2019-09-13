// Decompiled with JetBrains decompiler
// Type: WireRefinedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedConfig : BaseWireConfig
{
  public const string ID = "WireRefined";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WireRefined";
    string anim = "utilities_electric_conduct_kanim";
    float construction_time = 3f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    float insulation = 0.05f;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = this.CreateBuildingDef(id, anim, construction_time, tieR0, insulation, BUILDINGS.DECOR.NONE, none);
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
  }
}
