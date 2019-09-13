// Decompiled with JetBrains decompiler
// Type: BatteryMediumConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BatteryMediumConfig : BaseBatteryConfig
{
  public const string ID = "BatteryMedium";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "BatteryMedium";
    int width = 2;
    int height = 2;
    int hitpoints = 30;
    string anim = "batterymed_kanim";
    float construction_time = 60f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    float exhaust_temperature_active = 0.25f;
    float self_heat_kilowatts_active = 1f;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = this.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tieR4, allMetals, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER2, tieR1);
    SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Battery battery = go.AddOrGet<Battery>();
    battery.capacity = 40000f;
    battery.joulesLostPerSecond = 3.333333f;
    base.DoPostConfigureComplete(go);
  }
}
