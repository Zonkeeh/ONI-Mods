// Decompiled with JetBrains decompiler
// Type: Database.Techs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
  public class Techs : ResourceSet<Tech>
  {
    public static Dictionary<string, string[]> TECH_GROUPING = new Dictionary<string, string[]>()
    {
      {
        "FarmingTech",
        new string[4]
        {
          "AlgaeHabitat",
          "PlanterBox",
          "RationBox",
          "Compost"
        }
      },
      {
        "FineDining",
        new string[4]
        {
          "DiningTable",
          "FarmTile",
          "CookingStation",
          "EggCracker"
        }
      },
      {
        "FinerDining",
        new string[1]{ "GourmetCookingStation" }
      },
      {
        "Agriculture",
        new string[5]
        {
          "FertilizerMaker",
          "HydroponicFarm",
          "Refrigerator",
          "FarmStation",
          "ParkSign"
        }
      },
      {
        "Ranching",
        new string[7]
        {
          "CreatureDeliveryPoint",
          "FishDeliveryPoint",
          "CreatureFeeder",
          "FishFeeder",
          "RanchStation",
          "ShearingStation",
          "FlyingCreatureBait"
        }
      },
      {
        "AnimalControl",
        new string[5]
        {
          "CreatureTrap",
          "FishTrap",
          "AirborneCreatureLure",
          "EggIncubator",
          LogicCritterCountSensorConfig.ID
        }
      },
      {
        "ImprovedOxygen",
        new string[2]{ "Electrolyzer", "RustDeoxidizer" }
      },
      {
        "GasPiping",
        new string[4]
        {
          "GasConduit",
          "GasPump",
          "GasVent",
          "GasConduitBridge"
        }
      },
      {
        "ImprovedGasPiping",
        new string[6]
        {
          "InsulatedGasConduit",
          LogicPressureSensorGasConfig.ID,
          "GasVentHighPressure",
          "GasLogicValve",
          "GasConduitPreferentialFlow",
          "GasConduitOverflow"
        }
      },
      {
        "PressureManagement",
        new string[4]
        {
          "LiquidValve",
          "GasValve",
          "ManualPressureDoor",
          "GasPermeableMembrane"
        }
      },
      {
        "DirectedAirStreams",
        new string[3]{ "PressureDoor", "AirFilter", "CO2Scrubber" }
      },
      {
        "LiquidFiltering",
        new string[2]{ "OreScrubber", "Desalinator" }
      },
      {
        "MedicineI",
        new string[1]{ "Apothecary" }
      },
      {
        "MedicineII",
        new string[2]{ "DoctorStation", "HandSanitizer" }
      },
      {
        "MedicineIII",
        new string[3]
        {
          LogicDiseaseSensorConfig.ID,
          GasConduitDiseaseSensorConfig.ID,
          LiquidConduitDiseaseSensorConfig.ID
        }
      },
      {
        "MedicineIV",
        new string[1]{ "AdvancedDoctorStation" }
      },
      {
        "LiquidPiping",
        new string[4]
        {
          "LiquidConduit",
          "LiquidPump",
          "LiquidVent",
          "LiquidConduitBridge"
        }
      },
      {
        "ImprovedLiquidPiping",
        new string[6]
        {
          "InsulatedLiquidConduit",
          LogicPressureSensorLiquidConfig.ID,
          "LiquidLogicValve",
          "LiquidConduitPreferentialFlow",
          "LiquidConduitOverflow",
          "LiquidReservoir"
        }
      },
      {
        "PrecisionPlumbing",
        new string[1]{ "EspressoMachine" }
      },
      {
        "SanitationSciences",
        new string[4]
        {
          "WashSink",
          "FlushToilet",
          ShowerConfig.ID,
          "MeshTile"
        }
      },
      {
        "AdvancedFiltration",
        new string[2]{ "GasFilter", "LiquidFilter" }
      },
      {
        "Distillation",
        new string[5]
        {
          "WaterPurifier",
          "AlgaeDistillery",
          "EthanolDistillery",
          "GasBottler",
          "BottleEmptierGas"
        }
      },
      {
        "Catalytics",
        new string[2]{ "OxyliteRefinery", "SupermaterialRefinery" }
      },
      {
        "PowerRegulation",
        new string[3]
        {
          SwitchConfig.ID,
          "BatteryMedium",
          "WireBridge"
        }
      },
      {
        "AdvancedPowerRegulation",
        new string[5]
        {
          "HydrogenGenerator",
          "HighWattageWire",
          "WireBridgeHighWattage",
          "PowerTransformerSmall",
          LogicPowerRelayConfig.ID
        }
      },
      {
        "PrettyGoodConductors",
        new string[5]
        {
          "WireRefined",
          "WireRefinedBridge",
          "WireRefinedHighWattage",
          "WireRefinedBridgeHighWattage",
          "PowerTransformer"
        }
      },
      {
        "RenewableEnergy",
        new string[3]
        {
          "SteamTurbine",
          "SteamTurbine2",
          "SolarPanel"
        }
      },
      {
        "Combustion",
        new string[2]{ "Generator", "WoodGasGenerator" }
      },
      {
        "ImprovedCombustion",
        new string[3]
        {
          "MethaneGenerator",
          "OilRefinery",
          "PetroleumGenerator"
        }
      },
      {
        "InteriorDecor",
        new string[3]{ "FlowerVase", "FloorLamp", "CeilingLight" }
      },
      {
        "Artistry",
        new string[7]
        {
          "CrownMoulding",
          "CornerMoulding",
          "SmallSculpture",
          "IceSculpture",
          "ItemPedestal",
          "FlowerVaseWall",
          "FlowerVaseHanging"
        }
      },
      {
        "Clothing",
        new string[2]{ "ClothingFabricator", "CarpetTile" }
      },
      {
        "Acoustics",
        new string[3]
        {
          "Phonobox",
          "BatterySmart",
          "PowerControlStation"
        }
      },
      {
        "FineArt",
        new string[2]{ "Canvas", "Sculpture" }
      },
      {
        "Luxury",
        new string[3]
        {
          LuxuryBedConfig.ID,
          "LadderFast",
          "PlasticTile"
        }
      },
      {
        "RefractiveDecor",
        new string[2]{ "MetalSculpture", "CanvasWide" }
      },
      {
        "GlassFurnishings",
        new string[2]{ "GlassTile", "FlowerVaseHangingFancy" }
      },
      {
        "RenaissanceArt",
        new string[5]
        {
          "MarbleSculpture",
          "CanvasTall",
          "MonumentBottom",
          "MonumentMiddle",
          "MonumentTop"
        }
      },
      {
        "Plastics",
        new string[2]{ "Polymerizer", "OilWellCap" }
      },
      {
        "ValveMiniaturization",
        new string[2]{ "LiquidMiniPump", "GasMiniPump" }
      },
      {
        "Suits",
        new string[5]
        {
          "ExteriorWall",
          "SuitMarker",
          "SuitLocker",
          "SuitFabricator",
          "SuitsOverlay"
        }
      },
      {
        "Jobs",
        new string[2]{ "RoleStation", "WaterCooler" }
      },
      {
        "AdvancedResearch",
        new string[3]
        {
          "AdvancedResearchCenter",
          "BetaResearchPoint",
          "ResetSkillsStation"
        }
      },
      {
        "BasicRefinement",
        new string[2]{ "RockCrusher", "Kiln" }
      },
      {
        "RefinedObjects",
        new string[2]{ "ThermalBlock", "FirePole" }
      },
      {
        "Smelting",
        new string[2]{ "MetalRefinery", "MetalTile" }
      },
      {
        "HighTempForging",
        new string[3]{ "GlassForge", "BunkerTile", "BunkerDoor" }
      },
      {
        "TemperatureModulation",
        new string[5]
        {
          "LiquidCooledFan",
          "IceCooledFan",
          "IceMachine",
          "SpaceHeater",
          "InsulationTile"
        }
      },
      {
        "HVAC",
        new string[6]
        {
          "AirConditioner",
          LogicTemperatureSensorConfig.ID,
          "GasConduitRadiant",
          GasConduitTemperatureSensorConfig.ID,
          GasConduitElementSensorConfig.ID,
          "GasReservoir"
        }
      },
      {
        "LiquidTemperature",
        new string[5]
        {
          "LiquidHeater",
          "LiquidConditioner",
          "LiquidConduitRadiant",
          LiquidConduitTemperatureSensorConfig.ID,
          LiquidConduitElementSensorConfig.ID
        }
      },
      {
        "LogicControl",
        new string[5]
        {
          "LogicWire",
          "LogicDuplicantSensor",
          LogicSwitchConfig.ID,
          "LogicWireBridge",
          "AutomationOverlay"
        }
      },
      {
        "GenericSensors",
        new string[6]
        {
          LogicTimeOfDaySensorConfig.ID,
          "FloorSwitch",
          LogicElementSensorGasConfig.ID,
          LogicElementSensorLiquidConfig.ID,
          "BatterySmart",
          "LogicGateNOT"
        }
      },
      {
        "LogicCircuits",
        new string[4]
        {
          "LogicGateAND",
          "LogicGateOR",
          "LogicGateBUFFER",
          "LogicGateFILTER"
        }
      },
      {
        "DupeTrafficControl",
        new string[5]
        {
          "Checkpoint",
          LogicMemoryConfig.ID,
          "ArcadeMachine",
          "CosmicResearchCenter",
          "LogicGateXOR"
        }
      },
      {
        "SkyDetectors",
        new string[3]
        {
          CometDetectorConfig.ID,
          "Telescope",
          "AstronautTrainingCenter"
        }
      },
      {
        "TravelTubes",
        new string[3]
        {
          "TravelTubeEntrance",
          "TravelTube",
          "TravelTubeWallBridge"
        }
      },
      {
        "SmartStorage",
        new string[4]
        {
          "StorageLockerSmart",
          "SolidTransferArm",
          "ObjectDispenser",
          "ConveyorOverlay"
        }
      },
      {
        "SolidTransport",
        new string[7]
        {
          "SolidConduit",
          "SolidConduitBridge",
          "SolidConduitInbox",
          "SolidConduitOutbox",
          "SolidVent",
          "SolidLogicValve",
          "AutoMiner"
        }
      },
      {
        "BasicRocketry",
        new string[4]
        {
          "CommandModule",
          "SteamEngine",
          "ResearchModule",
          "Gantry"
        }
      },
      {
        "CargoI",
        new string[1]{ "CargoBay" }
      },
      {
        "CargoII",
        new string[2]{ "LiquidCargoBay", "GasCargoBay" }
      },
      {
        "CargoIII",
        new string[2]{ "TouristModule", "SpecialCargoBay" }
      },
      {
        "EnginesI",
        new string[1]{ "SolidBooster" }
      },
      {
        "EnginesII",
        new string[3]
        {
          "KeroseneEngine",
          "LiquidFuelTank",
          "OxidizerTank"
        }
      },
      {
        "EnginesIII",
        new string[2]{ "OxidizerTankLiquid", "HydrogenEngine" }
      },
      {
        "Jetpacks",
        new string[3]{ "JetSuit", "JetSuitMarker", "JetSuitLocker" }
      }
    };
    private readonly List<List<Tuple<string, float>>> TECH_TIERS = new List<List<Tuple<string, float>>>()
    {
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 15f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 20f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 30f),
        new Tuple<string, float>("beta", 20f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 35f),
        new Tuple<string, float>("beta", 30f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 40f),
        new Tuple<string, float>("beta", 50f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 50f),
        new Tuple<string, float>("beta", 70f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 70f),
        new Tuple<string, float>("beta", 100f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 70f),
        new Tuple<string, float>("beta", 100f),
        new Tuple<string, float>("gamma", 200f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 70f),
        new Tuple<string, float>("beta", 100f),
        new Tuple<string, float>("gamma", 400f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 70f),
        new Tuple<string, float>("beta", 100f),
        new Tuple<string, float>("gamma", 800f)
      },
      new List<Tuple<string, float>>()
      {
        new Tuple<string, float>("alpha", 70f),
        new Tuple<string, float>("beta", 100f),
        new Tuple<string, float>("gamma", 1600f)
      }
    };
    public int tierCount;

    public Techs(ResourceSet parent)
      : base(nameof (Techs), parent)
    {
    }

    public void Load(TextAsset tree_file)
    {
      foreach (ResourceTreeNode node in (ResourceLoader<ResourceTreeNode>) new ResourceTreeLoader<ResourceTreeNode>(tree_file))
      {
        if (!string.Equals(node.Id.Substring(0, 1), "_"))
        {
          Tech tech1 = this.TryGet(node.Id) ?? new Tech(node.Id, (ResourceSet) this, (string) Strings.Get("STRINGS.RESEARCH.TECHS." + node.Id.ToUpper() + ".NAME"), (string) Strings.Get("STRINGS.RESEARCH.TECHS." + node.Id.ToUpper() + ".DESC"), node);
          foreach (ResourceTreeNode reference in node.references)
          {
            Tech tech2 = this.TryGet(reference.Id) ?? new Tech(reference.Id, (ResourceSet) this, (string) Strings.Get("STRINGS.RESEARCH.TECHS." + reference.Id.ToUpper() + ".NAME"), (string) Strings.Get("STRINGS.RESEARCH.TECHS." + reference.Id.ToUpper() + ".DESC"), reference);
            tech2.requiredTech.Add(tech1);
            tech1.unlockedTech.Add(tech2);
          }
        }
      }
      this.tierCount = 0;
      foreach (Tech resource in this.resources)
      {
        resource.tier = this.GetTier(resource);
        foreach (Tuple<string, float> tuple in this.TECH_TIERS[resource.tier])
          resource.costsByResearchTypeID.Add(tuple.first, tuple.second);
        this.tierCount = Math.Max(resource.tier + 1, this.tierCount);
      }
    }

    private int GetTier(Tech tech)
    {
      if (tech.requiredTech.Count == 0)
        return 0;
      int val1 = 0;
      foreach (Tech tech1 in tech.requiredTech)
        val1 = Math.Max(val1, this.GetTier(tech1));
      return val1 + 1;
    }

    private void AddPrerequisite(Tech tech, string prerequisite_name)
    {
      Tech tech1 = this.TryGet(prerequisite_name);
      if (tech1 == null)
        return;
      tech.requiredTech.Add(tech1);
      tech1.unlockedTech.Add(tech);
    }

    public bool IsTechItemComplete(string id)
    {
      foreach (Tech resource in this.resources)
      {
        foreach (Resource unlockedItem in resource.unlockedItems)
        {
          if (unlockedItem.Id == id)
            return resource.IsComplete();
        }
      }
      return true;
    }
  }
}
