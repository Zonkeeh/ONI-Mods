// Decompiled with JetBrains decompiler
// Type: BatteryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BatteryConfig : BaseBatteryConfig
{
  public const string ID = "Battery";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Battery";
    int width = 1;
    int height = 2;
    int hitpoints = 30;
    string anim = "batterysm_kanim";
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    float exhaust_temperature_active = 0.25f;
    float self_heat_kilowatts_active = 1f;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = this.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tieR3, allMetals, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER1, none);
    buildingDef.Breakable = true;
    SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Battery battery = go.AddOrGet<Battery>();
    battery.capacity = 10000f;
    battery.joulesLostPerSecond = 1.666667f;
    base.DoPostConfigureComplete(go);
  }
}
