// Decompiled with JetBrains decompiler
// Type: WireConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireConfig : BaseWireConfig
{
  public const string ID = "Wire";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Wire";
    string anim = "utilities_electric_kanim";
    float construction_time = 3f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    float insulation = 0.05f;
    EffectorValues none = NOISE_POLLUTION.NONE;
    return this.CreateBuildingDef(id, anim, construction_time, tieR0, insulation, BUILDINGS.DECOR.PENALTY.TIER0, none);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
  }
}
