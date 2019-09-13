// Decompiled with JetBrains decompiler
// Type: BuildMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : KScreen
{
  private static readonly HashedString ROOT_HASHSTR = new HashedString("ROOT");
  public static BuildMenu.DisplayInfo OrderedBuildings = new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ROOT"), "icon_category_base", Action.NumActions, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
  {
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Base"), "icon_category_base", Action.Plan1, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Tiles"), "icon_category_base", Action.BuildCategoryTiles, KKeyCode.T, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Tile", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("GasPermeableMembrane", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("MeshTile", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("InsulationTile", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("PlasticTile", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("MetalTile", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("GlassTile", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("BunkerTile", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("CarpetTile", Action.BuildMenuKeyL)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ladders"), "icon_category_base", Action.BuildCategoryLadders, KKeyCode.A, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Ladder", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("LadderFast", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("FirePole", Action.BuildMenuKeyF)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Doors"), "icon_category_base", Action.BuildCategoryDoors, KKeyCode.D, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Door", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("ManualPressureDoor", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("PressureDoor", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("BunkerDoor", Action.BuildMenuKeyB)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Storage"), "icon_category_base", Action.BuildCategoryStorage, KKeyCode.S, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("StorageLocker", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("RationBox", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("Refrigerator", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("StorageLockerSmart", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("LiquidReservoir", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("GasReservoir", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("ObjectDispenser", Action.BuildMenuKeyO)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Research"), "icon_category_misc", Action.BuildCategoryResearch, KKeyCode.R, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("ResearchCenter", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("AdvancedResearchCenter", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("CosmicResearchCenter", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("Telescope", Action.BuildMenuKeyT)
      })
    }),
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Food And Agriculture"), "icon_category_food", Action.Plan2, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Farming"), "icon_category_food", Action.BuildCategoryFarming, KKeyCode.F, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("PlanterBox", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("FarmTile", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("HydroponicFarm", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("Compost", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("FertilizerMaker", Action.BuildMenuKeyR)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Cooking"), "icon_category_food", Action.BuildCategoryCooking, KKeyCode.C, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("MicrobeMusher", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("CookingStation", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("GourmetCookingStation", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("EggCracker", Action.BuildMenuKeyE)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ranching"), "icon_category_food", Action.BuildCategoryRanching, KKeyCode.R, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("CreatureDeliveryPoint", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("FishDeliveryPoint", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("CreatureFeeder", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("FishFeeder", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("RanchStation", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("ShearingStation", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("EggIncubator", Action.BuildMenuKeyI),
        new BuildMenu.BuildingInfo("CreatureTrap", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("FishTrap", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("AirborneCreatureLure", Action.BuildMenuKeyL),
        new BuildMenu.BuildingInfo("FlyingCreatureBait", Action.BuildMenuKeyB)
      })
    }),
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Health And Happiness"), "icon_category_medical", Action.Plan3, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Medical"), "icon_category_medical", Action.BuildCategoryMedical, KKeyCode.C, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Apothecary", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("DoctorStation", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("AdvancedDoctorStation", Action.BuildMenuKeyO),
        new BuildMenu.BuildingInfo("MedicalCot", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("MassageTable", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("Grave", Action.BuildMenuKeyR)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Hygiene"), "icon_category_medical", Action.BuildCategoryHygiene, KKeyCode.E, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Outhouse", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("FlushToilet", Action.BuildMenuKeyV),
        new BuildMenu.BuildingInfo(ShowerConfig.ID, Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("WashBasin", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("WashSink", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("HandSanitizer", Action.BuildMenuKeyA)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Furniture"), "icon_category_furniture", Action.BuildCategoryFurniture, KKeyCode.F, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo(BedConfig.ID, Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo(LuxuryBedConfig.ID, Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("DiningTable", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("FloorLamp", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("CeilingLight", Action.BuildMenuKeyT)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Decor"), "icon_category_furniture", Action.BuildCategoryDecor, KKeyCode.D, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("FlowerVase", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("Canvas", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("CanvasWide", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("CanvasTall", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("Sculpture", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("IceSculpture", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("ItemPedestal", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("CrownMoulding", Action.BuildMenuKeyM),
        new BuildMenu.BuildingInfo("CornerMoulding", Action.BuildMenuKeyN)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Recreation"), "icon_category_medical", Action.BuildCategoryRecreation, KKeyCode.R, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("WaterCooler", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("ArcadeMachine", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("Phonobox", Action.BuildMenuKeyP),
        new BuildMenu.BuildingInfo("EspressoMachine", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("ParkSign", Action.BuildMenuKeyR)
      })
    }),
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Infrastructure"), "icon_category_utilities", Action.Plan4, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Wires"), "icon_category_electrical", Action.BuildCategoryWires, KKeyCode.W, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Wire", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("WireBridge", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("HighWattageWire", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("WireBridgeHighWattage", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("WireRefined", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("WireRefinedBridge", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("WireRefinedHighWattage", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("WireRefinedBridgeHighWattage", Action.BuildMenuKeyA)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Generators"), "icon_category_electrical", Action.BuildCategoryGenerators, KKeyCode.G, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("ManualGenerator", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("Generator", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("WoodGasGenerator", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("HydrogenGenerator", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("MethaneGenerator", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("PetroleumGenerator", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("SteamTurbine", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("SteamTurbine2", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("SolarPanel", Action.BuildMenuKeyS)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("PowerControl"), "icon_category_electrical", Action.BuildCategoryPowerControl, KKeyCode.R, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Battery", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("BatteryMedium", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("BatterySmart", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("PowerTransformerSmall", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("PowerTransformer", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo(SwitchConfig.ID, Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo(TemperatureControlledSwitchConfig.ID, Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo(PressureSwitchLiquidConfig.ID, Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo(PressureSwitchGasConfig.ID, Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo(LogicPowerRelayConfig.ID, Action.BuildMenuKeyX)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Pipes"), "icon_category_plumbing", Action.BuildCategoryPipes, KKeyCode.E, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("LiquidConduit", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("LiquidConduitBridge", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("InsulatedLiquidConduit", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("LiquidConduitRadiant", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("GasConduit", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("GasConduitBridge", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("InsulatedGasConduit", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("GasConduitRadiant", Action.BuildMenuKeyR)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Plumbing Structures"), "icon_category_plumbing", Action.BuildCategoryPlumbingStructures, KKeyCode.B, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("LiquidPumpingStation", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("BottleEmptier", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("LiquidPump", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("LiquidMiniPump", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("LiquidValve", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("LiquidLogicValve", Action.BuildMenuKeyL),
        new BuildMenu.BuildingInfo("LiquidVent", Action.BuildMenuKeyV),
        new BuildMenu.BuildingInfo("LiquidFilter", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("LiquidConduitPreferentialFlow", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("LiquidConduitOverflow", Action.BuildMenuKeyR)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ventilation Structures"), "icon_category_ventilation", Action.BuildCategoryVentilationStructures, KKeyCode.V, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("GasPump", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("GasMiniPump", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("GasValve", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("GasLogicValve", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("GasVent", Action.BuildMenuKeyV),
        new BuildMenu.BuildingInfo("GasVentHighPressure", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("GasFilter", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("GasBottler", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("BottleEmptierGas", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("GasConduitPreferentialFlow", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("GasConduitOverflow", Action.BuildMenuKeyR)
      })
    }),
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Industrial"), "icon_category_refinery", Action.Plan5, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Oxygen"), "icon_category_oxygen", Action.BuildCategoryOxygen, KKeyCode.X, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("MineralDeoxidizer", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("AlgaeHabitat", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("AirFilter", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("CO2Scrubber", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("Electrolyzer", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("RustDeoxidizer", Action.BuildMenuKeyF)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Utilities"), "icon_category_utilities", Action.BuildCategoryUtilities, KKeyCode.T, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("SpaceHeater", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("LiquidHeater", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("IceCooledFan", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("IceMachine", Action.BuildMenuKeyI),
        new BuildMenu.BuildingInfo("AirConditioner", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("LiquidConditioner", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("OreScrubber", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("ThermalBlock", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("ExteriorWall", Action.BuildMenuKeyD)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Refining"), "icon_category_refinery", Action.BuildCategoryRefining, KKeyCode.R, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("WaterPurifier", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("AlgaeDistillery", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("EthanolDistillery", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("RockCrusher", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("Kiln", Action.BuildMenuKeyZ),
        new BuildMenu.BuildingInfo("OilWellCap", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("OilRefinery", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("Polymerizer", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("MetalRefinery", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("GlassForge", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("OxyliteRefinery", Action.BuildMenuKeyO),
        new BuildMenu.BuildingInfo("SupermaterialRefinery", Action.BuildMenuKeyS)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Equipment"), "icon_category_misc", Action.BuildCategoryEquipment, KKeyCode.S, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("RoleStation", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("FarmStation", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo("PowerControlStation", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("AstronautTrainingCenter", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("ResetSkillsStation", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("ClothingFabricator", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("SuitFabricator", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("SuitMarker", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("SuitLocker", Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo("JetSuitMarker", Action.BuildMenuKeyJ),
        new BuildMenu.BuildingInfo("JetSuitLocker", Action.BuildMenuKeyO)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Rocketry"), "icon_category_rocketry", Action.BuildCategoryRocketry, KKeyCode.C, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("Gantry", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("KeroseneEngine", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("SolidBooster", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("SteamEngine", Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo("LiquidFuelTank", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("CargoBay", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("GasCargoBay", Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo("LiquidCargoBay", Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo("SpecialCargoBay", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("CommandModule", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("TouristModule", Action.BuildMenuKeyY),
        new BuildMenu.BuildingInfo("ResearchModule", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("HydrogenEngine", Action.BuildMenuKeyH)
      })
    }),
    new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Logistics"), "icon_category_ventilation", Action.Plan6, KKeyCode.None, (object) new List<BuildMenu.DisplayInfo>()
    {
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("TravelTubes"), "icon_category_ventilation", Action.BuildCategoryTravelTubes, KKeyCode.T, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("TravelTube", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("TravelTubeEntrance", Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("TravelTubeWallBridge", Action.BuildMenuKeyB)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Conveyance"), "icon_category_ventilation", Action.BuildCategoryConveyance, KKeyCode.C, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("SolidTransferArm", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("SolidConduit", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo("SolidConduitInbox", Action.BuildMenuKeyI),
        new BuildMenu.BuildingInfo("SolidConduitOutbox", Action.BuildMenuKeyO),
        new BuildMenu.BuildingInfo("SolidVent", Action.BuildMenuKeyV),
        new BuildMenu.BuildingInfo("SolidLogicValve", Action.BuildMenuKeyL),
        new BuildMenu.BuildingInfo("SolidConduitBridge", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("AutoMiner", Action.BuildMenuKeyM)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicWiring"), "icon_category_automation", Action.BuildCategoryLogicWiring, KKeyCode.W, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("LogicWire", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("LogicWireBridge", Action.BuildMenuKeyB)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicGates"), "icon_category_automation", Action.BuildCategoryLogicGates, KKeyCode.G, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo("LogicGateAND", Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo("LogicGateOR", Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("LogicGateXOR", Action.BuildMenuKeyX),
        new BuildMenu.BuildingInfo("LogicGateNOT", Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo("LogicGateBUFFER", Action.BuildMenuKeyB),
        new BuildMenu.BuildingInfo("LogicGateFILTER", Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo(LogicMemoryConfig.ID, Action.BuildMenuKeyV)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicSwitches"), "icon_category_automation", Action.BuildCategoryLogicSwitches, KKeyCode.S, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo(LogicSwitchConfig.ID, Action.BuildMenuKeyS),
        new BuildMenu.BuildingInfo(LogicPressureSensorGasConfig.ID, Action.BuildMenuKeyA),
        new BuildMenu.BuildingInfo(LogicPressureSensorLiquidConfig.ID, Action.BuildMenuKeyQ),
        new BuildMenu.BuildingInfo(LogicTemperatureSensorConfig.ID, Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo(LogicTimeOfDaySensorConfig.ID, Action.BuildMenuKeyD),
        new BuildMenu.BuildingInfo(LogicCritterCountSensorConfig.ID, Action.BuildMenuKeyV),
        new BuildMenu.BuildingInfo(LogicDiseaseSensorConfig.ID, Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo(LogicElementSensorGasConfig.ID, Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo("FloorSwitch", Action.BuildMenuKeyW),
        new BuildMenu.BuildingInfo("Checkpoint", Action.BuildMenuKeyC),
        new BuildMenu.BuildingInfo(CometDetectorConfig.ID, Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo("LogicDuplicantSensor", Action.BuildMenuKeyF)
      }),
      new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ConduitSensors"), "icon_category_automation", Action.BuildCategoryLogicConduits, KKeyCode.X, (object) new List<BuildMenu.BuildingInfo>()
      {
        new BuildMenu.BuildingInfo(LiquidConduitTemperatureSensorConfig.ID, Action.BuildMenuKeyT),
        new BuildMenu.BuildingInfo(LiquidConduitDiseaseSensorConfig.ID, Action.BuildMenuKeyG),
        new BuildMenu.BuildingInfo(LiquidConduitElementSensorConfig.ID, Action.BuildMenuKeyE),
        new BuildMenu.BuildingInfo(GasConduitTemperatureSensorConfig.ID, Action.BuildMenuKeyR),
        new BuildMenu.BuildingInfo(GasConduitDiseaseSensorConfig.ID, Action.BuildMenuKeyF),
        new BuildMenu.BuildingInfo(GasConduitElementSensorConfig.ID, Action.BuildMenuKeyS)
      })
    })
  });
  private Dictionary<HashedString, BuildMenuCategoriesScreen> submenus = new Dictionary<HashedString, BuildMenuCategoriesScreen>();
  private Stack<KIconToggleMenu> submenuStack = new Stack<KIconToggleMenu>();
  [SerializeField]
  private Vector2 rootMenuOffset = Vector2.zero;
  [SerializeField]
  private BuildMenu.PadInfo rootMenuPadding = new BuildMenu.PadInfo();
  [SerializeField]
  private Vector2 nestedMenuOffset = Vector2.zero;
  [SerializeField]
  private BuildMenu.PadInfo nestedMenuPadding = new BuildMenu.PadInfo();
  [SerializeField]
  private Vector2 buildingsMenuOffset = Vector2.zero;
  private float updateInterval = 1f;
  public const string ENABLE_HOTKEY_BUILD_MENU_KEY = "ENABLE_HOTKEY_BUILD_MENU";
  [SerializeField]
  private BuildMenuCategoriesScreen categoriesMenuPrefab;
  [SerializeField]
  private BuildMenuBuildingsScreen buildingsMenuPrefab;
  [SerializeField]
  private GameObject productInfoScreenPrefab;
  private ProductInfoScreen productInfoScreen;
  private BuildMenuBuildingsScreen buildingsScreen;
  private BuildingDef selectedBuilding;
  private HashedString selectedCategory;
  private bool selecting;
  private bool updating;
  private bool deactivateToolQueued;
  private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;
  private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;
  private Dictionary<Tag, HashedString> tagCategoryMap;
  private Dictionary<Tag, int> tagOrderMap;
  private const float NotificationPingExpire = 0.5f;
  private const float SpecialNotificationEmbellishDelay = 8f;
  private float timeSinceNotificationPing;
  private int notificationPingCount;
  private float initTime;
  private float elapsedTime;

  public override float GetSortKey()
  {
    return 6f;
  }

  public static BuildMenu Instance { get; private set; }

  public static void DestroyInstance()
  {
    BuildMenu.Instance = (BuildMenu) null;
  }

  public BuildingDef SelectedBuildingDef
  {
    get
    {
      return this.selectedBuilding;
    }
  }

  private static HashedString CacheHashString(string str)
  {
    return HashCache.Get().Add(str);
  }

  public static bool UseHotkeyBuildMenu()
  {
    return KPlayerPrefs.GetInt("ENABLE_HOTKEY_BUILD_MENU") != 0;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConsumeMouseScroll = true;
    this.initTime = KTime.Instance.UnscaledGameTime;
    bool flag = BuildMenu.UseHotkeyBuildMenu();
    if (flag)
    {
      BuildMenu.Instance = this;
      this.productInfoScreen = Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.gameObject, true);
      this.productInfoScreen.rectTransform().pivot = new Vector2(0.0f, 0.0f);
      this.productInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
      this.productInfoScreen.Show(false);
      this.buildingsScreen = Util.KInstantiateUI<BuildMenuBuildingsScreen>(this.buildingsMenuPrefab.gameObject, this.gameObject, true);
      this.buildingsScreen.onBuildingSelected += new System.Action<BuildingDef>(this.OnBuildingSelected);
      this.buildingsScreen.Show(false);
      Game.Instance.Subscribe(288942073, new System.Action<object>(this.OnUIClear));
      Game.Instance.Subscribe(-1190690038, new System.Action<object>(this.OnBuildToolDeactivated));
      this.Initialize();
      this.rectTransform().anchoredPosition = Vector2.zero;
    }
    else
      this.gameObject.SetActive(flag);
  }

  private void Initialize()
  {
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu in this.submenus)
    {
      BuildMenuCategoriesScreen categoriesScreen = submenu.Value;
      categoriesScreen.Close();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) categoriesScreen.gameObject);
    }
    this.submenuStack.Clear();
    this.tagCategoryMap = new Dictionary<Tag, HashedString>();
    this.tagOrderMap = new Dictionary<Tag, int>();
    this.categorizedBuildingMap = new Dictionary<HashedString, List<BuildingDef>>();
    this.categorizedCategoryMap = new Dictionary<HashedString, List<HashedString>>();
    int building_index = 0;
    BuildMenu.DisplayInfo orderedBuildings = BuildMenu.OrderedBuildings;
    this.PopulateCategorizedMaps(orderedBuildings.category, 0, orderedBuildings.data, this.tagCategoryMap, this.tagOrderMap, ref building_index, this.categorizedBuildingMap, this.categorizedCategoryMap);
    BuildMenuCategoriesScreen submenu1 = this.submenus[BuildMenu.ROOT_HASHSTR];
    submenu1.Show(true);
    submenu1.modalKeyInputBehaviour = false;
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu2 in this.submenus)
    {
      HashedString key = submenu2.Key;
      List<HashedString> hashedStringList;
      if (!(key == BuildMenu.ROOT_HASHSTR) && this.categorizedCategoryMap.TryGetValue(key, out hashedStringList))
      {
        Image component = submenu2.Value.GetComponent<Image>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.enabled = hashedStringList.Count > 0;
      }
    }
    this.PositionMenus();
  }

  [ContextMenu("PositionMenus")]
  private void PositionMenus()
  {
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu in this.submenus)
    {
      HashedString key = submenu.Key;
      BuildMenuCategoriesScreen cmp = submenu.Value;
      LayoutGroup component = cmp.GetComponent<LayoutGroup>();
      Vector2 vector2;
      BuildMenu.PadInfo padInfo;
      if (key == BuildMenu.ROOT_HASHSTR)
      {
        vector2 = this.rootMenuOffset;
        padInfo = this.rootMenuPadding;
        cmp.GetComponent<Image>().enabled = false;
      }
      else
      {
        vector2 = this.nestedMenuOffset;
        padInfo = this.nestedMenuPadding;
      }
      cmp.rectTransform().anchoredPosition = vector2;
      component.padding.left = padInfo.left;
      component.padding.right = padInfo.right;
      component.padding.top = padInfo.top;
      component.padding.bottom = padInfo.bottom;
    }
    this.buildingsScreen.rectTransform().anchoredPosition = this.buildingsMenuOffset;
  }

  public void Refresh()
  {
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu in this.submenus)
      submenu.Value.UpdateBuildableStates(true);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    Game.Instance.Subscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
  }

  protected override void OnCmpDisable()
  {
    Game.Instance.Unsubscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
    base.OnCmpDisable();
  }

  private BuildMenuCategoriesScreen CreateCategorySubMenu(
    HashedString category,
    int depth,
    object data,
    Dictionary<HashedString, List<BuildingDef>> categorized_building_map,
    Dictionary<HashedString, List<HashedString>> categorized_category_map,
    Dictionary<Tag, HashedString> tag_category_map,
    BuildMenuBuildingsScreen buildings_screen)
  {
    BuildMenuCategoriesScreen categoriesScreen = Util.KInstantiateUI<BuildMenuCategoriesScreen>(this.categoriesMenuPrefab.gameObject, this.gameObject, true);
    categoriesScreen.Show(false);
    categoriesScreen.Configure(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.buildingsScreen);
    categoriesScreen.onCategoryClicked += new System.Action<HashedString, int>(this.OnCategoryClicked);
    categoriesScreen.name = "BuildMenu_" + category.ToString();
    return categoriesScreen;
  }

  private void PopulateCategorizedMaps(
    HashedString category,
    int depth,
    object data,
    Dictionary<Tag, HashedString> category_map,
    Dictionary<Tag, int> order_map,
    ref int building_index,
    Dictionary<HashedString, List<BuildingDef>> categorized_building_map,
    Dictionary<HashedString, List<HashedString>> categorized_category_map)
  {
    System.Type type = data.GetType();
    if (type == typeof (BuildMenu.DisplayInfo))
    {
      BuildMenu.DisplayInfo displayInfo = (BuildMenu.DisplayInfo) data;
      List<HashedString> hashedStringList;
      if (!categorized_category_map.TryGetValue(category, out hashedStringList))
      {
        hashedStringList = new List<HashedString>();
        categorized_category_map[category] = hashedStringList;
      }
      hashedStringList.Add(displayInfo.category);
      this.PopulateCategorizedMaps(displayInfo.category, depth + 1, displayInfo.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
    }
    else if (typeof (IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
    {
      IList<BuildMenu.DisplayInfo> displayInfoList = (IList<BuildMenu.DisplayInfo>) data;
      List<HashedString> hashedStringList;
      if (!categorized_category_map.TryGetValue(category, out hashedStringList))
      {
        hashedStringList = new List<HashedString>();
        categorized_category_map[category] = hashedStringList;
      }
      foreach (BuildMenu.DisplayInfo displayInfo in (IEnumerable<BuildMenu.DisplayInfo>) displayInfoList)
      {
        hashedStringList.Add(displayInfo.category);
        this.PopulateCategorizedMaps(displayInfo.category, depth + 1, displayInfo.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
      }
    }
    else
    {
      foreach (BuildMenu.BuildingInfo buildingInfo in (IEnumerable<BuildMenu.BuildingInfo>) data)
      {
        Tag index = new Tag(buildingInfo.id);
        category_map[index] = category;
        order_map[index] = building_index;
        ++building_index;
        List<BuildingDef> buildingDefList;
        if (!categorized_building_map.TryGetValue(category, out buildingDefList))
        {
          buildingDefList = new List<BuildingDef>();
          categorized_building_map[category] = buildingDefList;
        }
        BuildingDef buildingDef = Assets.GetBuildingDef(buildingInfo.id);
        buildingDef.HotKey = buildingInfo.hotkey;
        buildingDefList.Add(buildingDef);
      }
    }
    this.submenus[category] = this.CreateCategorySubMenu(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.tagCategoryMap, this.buildingsScreen);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (!this.mouseOver || !this.ConsumeMouseScroll || (e.TryConsume(Action.ZoomIn) || !e.TryConsume(Action.ZoomOut)))
      ;
    if (!e.Consumed && this.selectedCategory.IsValid && e.TryConsume(Action.Escape))
    {
      this.OnUIClear((object) null);
    }
    else
    {
      if (e.Consumed)
        return;
      base.OnKeyDown(e);
    }
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.selectedCategory.IsValid && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
    {
      this.OnUIClear((object) null);
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
    }
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void OnUIClear(object data)
  {
    SelectTool.Instance.Activate();
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
    SelectTool.Instance.Select((KSelectable) null, true);
    this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
    this.CloseMenus();
  }

  private void OnBuildToolDeactivated(object data)
  {
    if (this.updating)
    {
      this.deactivateToolQueued = true;
    }
    else
    {
      this.CloseMenus();
      this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
    }
  }

  private void CloseMenus()
  {
    this.productInfoScreen.Close();
    while (this.submenuStack.Count > 0)
    {
      this.submenuStack.Pop().Close();
      this.productInfoScreen.Close();
    }
    this.selectedCategory = HashedString.Invalid;
    this.submenus[BuildMenu.ROOT_HASHSTR].ClearSelection();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if ((double) this.timeSinceNotificationPing < 8.0)
      this.timeSinceNotificationPing += Time.unscaledDeltaTime;
    if ((double) this.timeSinceNotificationPing < 0.5)
      return;
    this.notificationPingCount = 0;
  }

  public void PlayNewBuildingSounds()
  {
    if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 1.5)
    {
      if ((double) BuildMenu.Instance.timeSinceNotificationPing >= 8.0)
      {
        string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
        if (sound != null)
          SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition()));
      }
      string sound1 = GlobalAssets.GetSound("NewBuildable", false);
      if (sound1 != null)
      {
        EventInstance instance = SoundEvent.BeginOneShot(sound1, SoundListenerController.Instance.transform.GetPosition());
        int num = (int) instance.setParameterValue("playCount", (float) BuildMenu.Instance.notificationPingCount);
        SoundEvent.EndOneShot(instance);
      }
    }
    this.timeSinceNotificationPing = 0.0f;
    ++this.notificationPingCount;
  }

  public PlanScreen.RequirementsState BuildableState(BuildingDef def)
  {
    PlanScreen.RequirementsState requirementsState = PlanScreen.RequirementsState.Complete;
    if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
    {
      if (!Db.Get().TechItems.IsTechItemComplete(def.PrefabID))
        requirementsState = PlanScreen.RequirementsState.Tech;
      else if (!ProductInfoScreen.MaterialsMet(def.CraftRecipe))
        requirementsState = PlanScreen.RequirementsState.Materials;
    }
    return requirementsState;
  }

  private void CloseProductInfoScreen()
  {
    this.productInfoScreen.ClearProduct(true);
    this.productInfoScreen.Show(false);
  }

  private void Update()
  {
    if (this.deactivateToolQueued)
    {
      this.deactivateToolQueued = false;
      this.OnBuildToolDeactivated((object) null);
    }
    this.elapsedTime += Time.unscaledDeltaTime;
    if ((double) this.elapsedTime <= (double) this.updateInterval)
      return;
    this.elapsedTime = 0.0f;
    this.updating = true;
    if (this.productInfoScreen.gameObject.activeSelf)
      this.productInfoScreen.materialSelectionPanel.UpdateResourceToggleValues();
    foreach (KIconToggleMenu submenu in this.submenuStack)
    {
      if (submenu is BuildMenuCategoriesScreen)
        (submenu as BuildMenuCategoriesScreen).UpdateBuildableStates(false);
    }
    this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(false);
    this.updating = false;
  }

  private void OnRecipeElementsFullySelected()
  {
    if ((UnityEngine.Object) this.selectedBuilding == (UnityEngine.Object) null)
      Debug.Log((object) "No def!");
    if (this.selectedBuilding.isKAnimTile && this.selectedBuilding.isUtility)
    {
      IList<Tag> selectedElementAsList = this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
      (!((UnityEngine.Object) this.selectedBuilding.BuildingComplete.GetComponent<Wire>() != (UnityEngine.Object) null) ? (BaseUtilityBuildTool) UtilityBuildTool.Instance : (BaseUtilityBuildTool) WireBuildTool.Instance).Activate(this.selectedBuilding, selectedElementAsList);
    }
    else
      BuildTool.Instance.Activate(this.selectedBuilding, this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList, (GameObject) null);
  }

  private void OnBuildingSelected(BuildingDef def)
  {
    if (this.selecting)
      return;
    this.selecting = true;
    this.selectedBuilding = def;
    this.buildingsScreen.SetHasFocus(false);
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu in this.submenus)
      submenu.Value.SetHasFocus(false);
    ToolMenu.Instance.ClearSelection();
    if ((UnityEngine.Object) def != (UnityEngine.Object) null)
    {
      Vector2 anchoredPosition = this.productInfoScreen.rectTransform().anchoredPosition;
      RectTransform rectTransform = this.buildingsScreen.rectTransform();
      anchoredPosition.y = rectTransform.anchoredPosition.y;
      anchoredPosition.x = (float) ((double) rectTransform.anchoredPosition.x + (double) rectTransform.sizeDelta.x + 10.0);
      this.productInfoScreen.rectTransform().anchoredPosition = anchoredPosition;
      this.productInfoScreen.ClearProduct(false);
      this.productInfoScreen.Show(true);
      this.productInfoScreen.ConfigureScreen(def);
    }
    else
      this.productInfoScreen.Close();
    this.selecting = false;
  }

  private void OnCategoryClicked(HashedString new_category, int depth)
  {
    while (this.submenuStack.Count > depth)
    {
      KIconToggleMenu kiconToggleMenu = this.submenuStack.Pop();
      kiconToggleMenu.ClearSelection();
      kiconToggleMenu.Close();
    }
    this.productInfoScreen.Close();
    if (new_category != this.selectedCategory && new_category.IsValid)
    {
      foreach (KIconToggleMenu submenu in this.submenuStack)
      {
        if (submenu is BuildMenuCategoriesScreen)
          (submenu as BuildMenuCategoriesScreen).SetHasFocus(false);
      }
      this.selectedCategory = new_category;
      BuildMenuCategoriesScreen categoriesScreen;
      this.submenus.TryGetValue(new_category, out categoriesScreen);
      if ((UnityEngine.Object) categoriesScreen != (UnityEngine.Object) null)
      {
        categoriesScreen.Show(true);
        categoriesScreen.SetHasFocus(true);
        this.submenuStack.Push((KIconToggleMenu) categoriesScreen);
      }
    }
    else
      this.selectedCategory = HashedString.Invalid;
    foreach (KIconToggleMenu submenu in this.submenuStack)
    {
      if (submenu is BuildMenuCategoriesScreen)
        (submenu as BuildMenuCategoriesScreen).UpdateBuildableStates(true);
    }
    this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(true);
  }

  public void RefreshProductInfoScreen(BuildingDef def)
  {
    if (!((UnityEngine.Object) this.productInfoScreen.currentDef == (UnityEngine.Object) def))
      return;
    this.productInfoScreen.ClearProduct(false);
    this.productInfoScreen.Show(true);
    this.productInfoScreen.ConfigureScreen(def);
  }

  private HashedString GetParentCategory(HashedString desired_category)
  {
    foreach (KeyValuePair<HashedString, List<HashedString>> categorizedCategory in this.categorizedCategoryMap)
    {
      foreach (HashedString hashedString in categorizedCategory.Value)
      {
        if (hashedString == desired_category)
          return categorizedCategory.Key;
      }
    }
    return HashedString.Invalid;
  }

  private void AddParentCategories(
    HashedString child_category,
    ICollection<HashedString> categories)
  {
    while (true)
    {
      HashedString parentCategory = this.GetParentCategory(child_category);
      if (!(parentCategory == HashedString.Invalid))
      {
        categories.Add(parentCategory);
        child_category = parentCategory;
      }
      else
        break;
    }
  }

  private void OnResearchComplete(object data)
  {
    HashSet<HashedString> hashedStringSet = new HashSet<HashedString>();
    Tech tech = (Tech) data;
    foreach (TechItem unlockedItem in tech.unlockedItems)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(unlockedItem.Id);
      if ((UnityEngine.Object) buildingDef == (UnityEngine.Object) null)
      {
        DebugUtil.LogWarningArgs((object) string.Format("Tech '{0}' unlocked building '{1}' but no such building exists", (object) tech.Name, (object) unlockedItem.Id));
      }
      else
      {
        HashedString tagCategory = this.tagCategoryMap[buildingDef.Tag];
        hashedStringSet.Add(tagCategory);
        this.AddParentCategories(tagCategory, (ICollection<HashedString>) hashedStringSet);
      }
    }
    this.UpdateNotifications((ICollection<HashedString>) hashedStringSet, (object) BuildMenu.OrderedBuildings);
  }

  private void UpdateNotifications(ICollection<HashedString> updated_categories, object data)
  {
    foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> submenu in this.submenus)
      submenu.Value.UpdateNotifications(updated_categories);
  }

  public PrioritySetting GetBuildingPriority()
  {
    return this.productInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
  }

  [Serializable]
  private struct PadInfo
  {
    public int left;
    public int right;
    public int top;
    public int bottom;
  }

  public struct BuildingInfo
  {
    public string id;
    public Action hotkey;

    public BuildingInfo(string id, Action hotkey)
    {
      this.id = id;
      this.hotkey = hotkey;
    }
  }

  public struct DisplayInfo
  {
    public HashedString category;
    public string iconName;
    public Action hotkey;
    public KKeyCode keyCode;
    public object data;

    public DisplayInfo(
      HashedString category,
      string icon_name,
      Action hotkey,
      KKeyCode key_code,
      object data)
    {
      this.category = category;
      this.iconName = icon_name;
      this.hotkey = hotkey;
      this.keyCode = key_code;
      this.data = data;
    }

    public BuildMenu.DisplayInfo GetInfo(HashedString category)
    {
      BuildMenu.DisplayInfo displayInfo1 = new BuildMenu.DisplayInfo();
      if (this.data != null && typeof (IList<BuildMenu.DisplayInfo>).IsAssignableFrom(this.data.GetType()))
      {
        foreach (BuildMenu.DisplayInfo displayInfo2 in (IEnumerable<BuildMenu.DisplayInfo>) this.data)
        {
          displayInfo1 = displayInfo2.GetInfo(category);
          if (!(displayInfo1.category == category))
          {
            if (displayInfo2.category == category)
            {
              displayInfo1 = displayInfo2;
              break;
            }
          }
          else
            break;
        }
      }
      return displayInfo1;
    }
  }
}
