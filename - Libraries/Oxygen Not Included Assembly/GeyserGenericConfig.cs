// Decompiled with JetBrains decompiler
// Type: GeyserGenericConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GeyserGenericConfig : IMultiEntityConfig
{
  public const string Steam = "steam";
  public const string HotSteam = "hot_steam";
  public const string HotWater = "hot_water";
  public const string SlushWater = "slush_water";
  public const string FilthyWater = "filthy_water";
  public const string SaltWater = "salt_water";
  public const string SmallVolcano = "small_volcano";
  public const string BigVolcano = "big_volcano";
  public const string LiquidCO2 = "liquid_co2";
  public const string HotCO2 = "hot_co2";
  public const string HotHydrogen = "hot_hydrogen";
  public const string HotPO2 = "hot_po2";
  public const string SlimyPO2 = "slimy_po2";
  public const string ChlorineGas = "chlorine_gas";
  public const string Methane = "methane";
  public const string MoltenCopper = "molten_copper";
  public const string MoltenIron = "molten_iron";
  public const string MoltenGold = "molten_gold";
  public const string OilDrip = "oil_drip";

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    List<GeyserGenericConfig.GeyserPrefabParams> configs = this.GenerateConfigs();
    foreach (GeyserGenericConfig.GeyserPrefabParams geyserPrefabParams in configs)
      gameObjectList.Add(this.CreateGeyser(geyserPrefabParams.id, geyserPrefabParams.anim, geyserPrefabParams.width, geyserPrefabParams.height, (string) Strings.Get(geyserPrefabParams.nameStringKey), (string) Strings.Get(geyserPrefabParams.descStringKey), geyserPrefabParams.geyserType.idHash));
    GameObject entity = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
    entity.AddOrGet<SaveLoadRoot>();
    entity.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      int num = 0;
      if (SaveLoader.Instance.worldDetailSave != null)
        num = SaveLoader.Instance.worldDetailSave.globalWorldSeed;
      else
        Debug.LogWarning((object) "Could not load global world seed for geysers");
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) configs[new System.Random(num + (int) inst.transform.GetPosition().x + (int) inst.transform.GetPosition().y).Next(0, configs.Count)].id), inst.transform.GetPosition(), Grid.SceneLayer.BuildingBack, (string) null, 0).SetActive(true);
      inst.DeleteObject();
    });
    gameObjectList.Add(entity);
    return gameObjectList;
  }

  public GameObject CreateGeyser(
    string id,
    string anim,
    int width,
    int height,
    string name,
    string desc,
    HashedString presetType)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    float mass = 2000f;
    int width1 = width;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, mass, Assets.GetAnim((HashedString) anim), "inactive", Grid.SceneLayer.BuildingBack, width1, height, BUILDINGS.DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER6, SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Katairite);
    component.Temperature = 372.15f;
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uncoverable>();
    placedEntity.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
    placedEntity.AddOrGet<GeyserConfigurator>().presetType = presetType;
    Studyable studyable = placedEntity.AddOrGet<Studyable>();
    studyable.meterTrackerSymbol = "geotracker_target";
    studyable.meterAnim = "tracker";
    placedEntity.AddOrGet<LoopingSounds>();
    SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
    SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  private List<GeyserGenericConfig.GeyserPrefabParams> GenerateConfigs()
  {
    return new List<GeyserGenericConfig.GeyserPrefabParams>()
    {
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim", 2, 4, new GeyserConfigurator.GeyserType("steam", SimHashes.Steam, 383.15f, 1000f, 2000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_steam", SimHashes.Steam, 773.15f, 500f, 1000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim", 4, 2, new GeyserConfigurator.GeyserType("hot_water", SimHashes.Water, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_water", SimHashes.DirtyWater, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim", 4, 2, new GeyserConfigurator.GeyserType("filthy_water", SimHashes.DirtyWater, 303.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f).AddDisease(new SimUtil.DiseaseInfo()
      {
        idx = Db.Get().Diseases.GetIndex((HashedString) "FoodPoisoning"),
        count = 20000
      })),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 4, 2, new GeyserConfigurator.GeyserType("salt_water", SimHashes.SaltWater, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim", 3, 3, new GeyserConfigurator.GeyserType("small_volcano", SimHashes.Magma, 2000f, 400f, 800f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim", 3, 3, new GeyserConfigurator.GeyserType("big_volcano", SimHashes.Magma, 2000f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_co2_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_co2", SimHashes.LiquidCarbonDioxide, 218f, 100f, 200f, 50f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_co2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_co2", SimHashes.CarbonDioxide, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_hydrogen", SimHashes.Hydrogen, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_po2", SimHashes.ContaminatedOxygen, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_slimy_kanim", 2, 4, new GeyserConfigurator.GeyserType("slimy_po2", SimHashes.ContaminatedOxygen, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f).AddDisease(new SimUtil.DiseaseInfo()
      {
        idx = Db.Get().Diseases.GetIndex((HashedString) "SlimeLung"),
        count = 5000
      })),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas", SimHashes.ChlorineGas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim", 2, 4, new GeyserConfigurator.GeyserType("methane", SimHashes.Methane, 423.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_copper", SimHashes.MoltenCopper, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.01666667f, 0.1f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_iron", SimHashes.MoltenIron, 2800f, 200f, 400f, 150f, 480f, 1080f, 0.01666667f, 0.1f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_gold", SimHashes.MoltenGold, 2900f, 200f, 400f, 150f, 480f, 1080f, 0.01666667f, 0.1f, 15000f, 135000f, 0.4f, 0.8f)),
      new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("oil_drip", SimHashes.CrudeOil, 600f, 1f, 250f, 50f, 600f, 600f, 1f, 1f, 100f, 500f, 0.4f, 0.8f))
    };
  }

  public struct GeyserPrefabParams
  {
    public string id;
    public string anim;
    public int width;
    public int height;
    public StringKey nameStringKey;
    public StringKey descStringKey;
    public GeyserConfigurator.GeyserType geyserType;

    public GeyserPrefabParams(
      string anim,
      int width,
      int height,
      GeyserConfigurator.GeyserType geyserType)
    {
      this.id = "GeyserGeneric_" + geyserType.id;
      this.anim = anim;
      this.width = width;
      this.height = height;
      this.nameStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".NAME");
      this.descStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".DESC");
      this.geyserType = geyserType;
    }
  }

  private static class TEMPERATURES
  {
    public const float BELOW_FREEZING = 263.15f;
    public const float DUPE_NORMAL = 303.15f;
    public const float DUPE_HOT = 333.15f;
    public const float BELOW_BOILING = 368.15f;
    public const float ABOVE_BOILING = 383.15f;
    public const float HOT1 = 423.15f;
    public const float HOT2 = 773.15f;
    public const float MOLTEN_MAGMA = 2000f;
  }

  private static class RATES
  {
    public const float GAS_SMALL_MIN = 40f;
    public const float GAS_SMALL_MAX = 80f;
    public const float GAS_NORMAL_MIN = 70f;
    public const float GAS_NORMAL_MAX = 140f;
    public const float GAS_BIG_MIN = 100f;
    public const float GAS_BIG_MAX = 200f;
    public const float LIQUID_SMALL_MIN = 500f;
    public const float LIQUID_SMALL_MAX = 1000f;
    public const float LIQUID_NORMAL_MIN = 1000f;
    public const float LIQUID_NORMAL_MAX = 2000f;
    public const float LIQUID_BIG_MIN = 2000f;
    public const float LIQUID_BIG_MAX = 4000f;
    public const float MOLTEN_NORMAL_MIN = 200f;
    public const float MOLTEN_NORMAL_MAX = 400f;
    public const float MOLTEN_BIG_MIN = 400f;
    public const float MOLTEN_BIG_MAX = 800f;
    public const float MOLTEN_HUGE_MIN = 800f;
    public const float MOLTEN_HUGE_MAX = 1600f;
  }

  private static class MAX_PRESURES
  {
    public const float GAS = 5f;
    public const float GAS_HIGH = 15f;
    public const float MOLTEN = 150f;
    public const float LIQUID_SMALL = 50f;
    public const float LIQUID = 500f;
  }

  private static class ITERATIONS
  {
    public static class INFREQUENT_MOLTEN
    {
      public const float PCT_MIN = 0.005f;
      public const float PCT_MAX = 0.01f;
      public const float LEN_MIN = 6000f;
      public const float LEN_MAX = 12000f;
    }

    public static class FREQUENT_MOLTEN
    {
      public const float PCT_MIN = 0.01666667f;
      public const float PCT_MAX = 0.1f;
      public const float LEN_MIN = 480f;
      public const float LEN_MAX = 1080f;
    }
  }
}
