// Decompiled with JetBrains decompiler
// Type: Database.SpaceDestinationTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Database
{
  public class SpaceDestinationTypes : ResourceSet<SpaceDestinationType>
  {
    public static Dictionary<SimHashes, MathUtil.MinMax> extendedElementTable = new Dictionary<SimHashes, MathUtil.MinMax>()
    {
      {
        SimHashes.Niobium,
        new MathUtil.MinMax(10f, 20f)
      },
      {
        SimHashes.Katairite,
        new MathUtil.MinMax(50f, 100f)
      },
      {
        SimHashes.Isoresin,
        new MathUtil.MinMax(30f, 60f)
      },
      {
        SimHashes.Fullerene,
        new MathUtil.MinMax(0.5f, 1f)
      }
    };
    public SpaceDestinationType Satellite;
    public SpaceDestinationType MetallicAsteroid;
    public SpaceDestinationType RockyAsteroid;
    public SpaceDestinationType CarbonaceousAsteroid;
    public SpaceDestinationType IcyDwarf;
    public SpaceDestinationType OrganicDwarf;
    public SpaceDestinationType DustyMoon;
    public SpaceDestinationType TerraPlanet;
    public SpaceDestinationType VolcanoPlanet;
    public SpaceDestinationType GasGiant;
    public SpaceDestinationType IceGiant;
    public SpaceDestinationType Wormhole;
    public SpaceDestinationType SaltDwarf;
    public SpaceDestinationType RustPlanet;
    public SpaceDestinationType ForestPlanet;
    public SpaceDestinationType RedDwarf;
    public SpaceDestinationType GoldAsteroid;
    public SpaceDestinationType HydrogenGiant;
    public SpaceDestinationType OilyAsteroid;
    public SpaceDestinationType ShinyPlanet;
    public SpaceDestinationType ChlorinePlanet;
    public SpaceDestinationType SaltDesertPlanet;
    public SpaceDestinationType Earth;

    public SpaceDestinationTypes(ResourceSet parent)
      : base("SpaceDestinations", parent)
    {
      this.Satellite = this.Add(new SpaceDestinationType(nameof (Satellite), parent, (string) UI.SPACEDESTINATIONS.DEBRIS.SATELLITE.NAME, (string) UI.SPACEDESTINATIONS.DEBRIS.SATELLITE.DESCRIPTION, 16, "asteroid", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Steel,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Copper,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Glass,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Bad, 64000000, 63994000, 18, true));
      this.MetallicAsteroid = this.Add(new SpaceDestinationType(nameof (MetallicAsteroid), parent, (string) UI.SPACEDESTINATIONS.ASTEROIDS.METALLICASTEROID.NAME, (string) UI.SPACEDESTINATIONS.ASTEROIDS.METALLICASTEROID.DESCRIPTION, 32, "nebula", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Iron,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Copper,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Obsidian,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "HatchMetal",
          3
        }
      }, Db.Get().ArtifactDropRates.Mediocre, 128000000, 127988000, 12, true));
      this.RockyAsteroid = this.Add(new SpaceDestinationType(nameof (RockyAsteroid), parent, (string) UI.SPACEDESTINATIONS.ASTEROIDS.ROCKYASTEROID.NAME, (string) UI.SPACEDESTINATIONS.ASTEROIDS.ROCKYASTEROID.DESCRIPTION, 32, "new_12", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Cuprite,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SedimentaryRock,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.IgneousRock,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "HatchHard",
          3
        }
      }, Db.Get().ArtifactDropRates.Good, 128000000, 127988000, 18, true));
      this.CarbonaceousAsteroid = this.Add(new SpaceDestinationType(nameof (CarbonaceousAsteroid), parent, (string) UI.SPACEDESTINATIONS.ASTEROIDS.CARBONACEOUSASTEROID.NAME, (string) UI.SPACEDESTINATIONS.ASTEROIDS.CARBONACEOUSASTEROID.DESCRIPTION, 32, "new_08", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.RefinedCarbon,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Carbon,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Diamond,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Mediocre, 128000000, 127988000, 6, true));
      this.IcyDwarf = this.Add(new SpaceDestinationType(nameof (IcyDwarf), parent, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ICYDWARF.NAME, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ICYDWARF.DESCRIPTION, 64, "icyMoon", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Ice,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidCarbonDioxide,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidOxygen,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "ColdBreatherSeed",
          3
        },
        {
          "ColdWheatSeed",
          4
        }
      }, Db.Get().ArtifactDropRates.Great, 256000000, 255982000, 24, true));
      this.OrganicDwarf = this.Add(new SpaceDestinationType(nameof (OrganicDwarf), parent, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ORGANICDWARF.NAME, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ORGANICDWARF.DESCRIPTION, 64, "organicAsteroid", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.SlimeMold,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Algae,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.ContaminatedOxygen,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "Moo",
          1
        },
        {
          "GasGrassSeed",
          4
        }
      }, Db.Get().ArtifactDropRates.Great, 256000000, 255982000, 30, true));
      this.DustyMoon = this.Add(new SpaceDestinationType(nameof (DustyMoon), parent, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.DUSTYDWARF.NAME, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.DUSTYDWARF.DESCRIPTION, 64, "new_05", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Regolith,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.MaficRock,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SedimentaryRock,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Amazing, 256000000, 255982000, 42, true));
      this.TerraPlanet = this.Add(new SpaceDestinationType(nameof (TerraPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.TERRAPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.TERRAPLANET.DESCRIPTION, 96, "terra", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Water,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Algae,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Oxygen,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Dirt,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "PrickleFlowerSeed",
          4
        },
        {
          "PacuEgg",
          4
        }
      }, Db.Get().ArtifactDropRates.Amazing, 384000000, 383980000, 54, true));
      this.VolcanoPlanet = this.Add(new SpaceDestinationType(nameof (VolcanoPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.VOLCANOPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.VOLCANOPLANET.DESCRIPTION, 96, "planet", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Magma,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.IgneousRock,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Katairite,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Amazing, 384000000, 383980000, 54, true));
      this.GasGiant = this.Add(new SpaceDestinationType(nameof (GasGiant), parent, (string) UI.SPACEDESTINATIONS.GIANTS.GASGIANT.NAME, (string) UI.SPACEDESTINATIONS.GIANTS.GASGIANT.DESCRIPTION, 96, "gasGiant", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Methane,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Hydrogen,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Perfect, 384000000, 383980000, 60, true));
      this.IceGiant = this.Add(new SpaceDestinationType(nameof (IceGiant), parent, (string) UI.SPACEDESTINATIONS.GIANTS.ICEGIANT.NAME, (string) UI.SPACEDESTINATIONS.GIANTS.ICEGIANT.DESCRIPTION, 96, "icyMoon", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Ice,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidCarbonDioxide,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidOxygen,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidMethane,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Perfect, 384000000, 383980000, 60, true));
      this.SaltDwarf = this.Add(new SpaceDestinationType(nameof (SaltDwarf), parent, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.SALTDWARF.NAME, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.SALTDWARF.DESCRIPTION, 64, "new_01", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.SaltWater,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidCarbonDioxide,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Brine,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "SaltPlantSeed",
          3
        }
      }, Db.Get().ArtifactDropRates.Bad, 256000000, 255982000, 30, true));
      this.RustPlanet = this.Add(new SpaceDestinationType(nameof (RustPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.RUSTPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.RUSTPLANET.DESCRIPTION, 96, "new_06", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Rust,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidCarbonDioxide,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Perfect, 384000000, 383980000, 60, true));
      this.ForestPlanet = this.Add(new SpaceDestinationType(nameof (ForestPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.FORESTPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.FORESTPLANET.DESCRIPTION, 96, "new_07", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.AluminumOre,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidOxygen,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>()
      {
        {
          "Squirrel",
          1
        },
        {
          "ForestTreeSeed",
          4
        }
      }, Db.Get().ArtifactDropRates.Mediocre, 384000000, 383980000, 24, true));
      this.RedDwarf = this.Add(new SpaceDestinationType(nameof (RedDwarf), parent, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.REDDWARF.NAME, (string) UI.SPACEDESTINATIONS.DWARFPLANETS.REDDWARF.DESCRIPTION, 64, "sun", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Aluminum,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.LiquidMethane,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Fossil,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Amazing, 256000000, 255982000, 42, true));
      this.GoldAsteroid = this.Add(new SpaceDestinationType(nameof (GoldAsteroid), parent, (string) UI.SPACEDESTINATIONS.ASTEROIDS.GOLDASTEROID.NAME, (string) UI.SPACEDESTINATIONS.ASTEROIDS.GOLDASTEROID.DESCRIPTION, 32, "new_02", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Gold,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Fullerene,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.FoolsGold,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Bad, 128000000, 127988000, 90, true));
      this.HydrogenGiant = this.Add(new SpaceDestinationType("HeliumGiant", parent, (string) UI.SPACEDESTINATIONS.GIANTS.HYDROGENGIANT.NAME, (string) UI.SPACEDESTINATIONS.GIANTS.HYDROGENGIANT.DESCRIPTION, 96, "new_11", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.LiquidHydrogen,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Water,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Niobium,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Mediocre, 384000000, 383980000, 78, true));
      this.OilyAsteroid = this.Add(new SpaceDestinationType("OilyAsteriod", parent, (string) UI.SPACEDESTINATIONS.ASTEROIDS.OILYASTEROID.NAME, (string) UI.SPACEDESTINATIONS.ASTEROIDS.OILYASTEROID.DESCRIPTION, 32, "new_09", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.SolidMethane,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.SolidCarbonDioxide,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.CrudeOil,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Petroleum,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Mediocre, 128000000, 127988000, 12, true));
      this.ShinyPlanet = this.Add(new SpaceDestinationType(nameof (ShinyPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.SHINYPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.SHINYPLANET.DESCRIPTION, 96, "new_04", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Tungsten,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.Wolframite,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Good, 384000000, 383980000, 84, true));
      this.ChlorinePlanet = this.Add(new SpaceDestinationType(nameof (ChlorinePlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.CHLORINEPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.CHLORINEPLANET.DESCRIPTION, 96, "new_10", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.SolidChlorine,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.BleachStone,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Bad, 256000000, 255982000, 90, true));
      this.SaltDesertPlanet = this.Add(new SpaceDestinationType(nameof (SaltDesertPlanet), parent, (string) UI.SPACEDESTINATIONS.PLANETS.SALTDESERTPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.SALTDESERTPLANET.DESCRIPTION, 96, "new_10", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Salt,
          new MathUtil.MinMax(100f, 200f)
        },
        {
          SimHashes.CrushedRock,
          new MathUtil.MinMax(100f, 200f)
        }
      }, new Dictionary<string, int>() { { "Crab", 1 } }, Db.Get().ArtifactDropRates.Bad, 384000000, 383980000, 60, true));
      this.Wormhole = this.Add(new SpaceDestinationType(nameof (Wormhole), parent, (string) UI.SPACEDESTINATIONS.WORMHOLE.NAME, (string) UI.SPACEDESTINATIONS.WORMHOLE.DESCRIPTION, 96, "new_03", new Dictionary<SimHashes, MathUtil.MinMax>()
      {
        {
          SimHashes.Vacuum,
          new MathUtil.MinMax(100f, 200f)
        }
      }, (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.Perfect, 0, 0, 0, true));
      this.Earth = this.Add(new SpaceDestinationType(nameof (Earth), parent, (string) UI.SPACEDESTINATIONS.PLANETS.SHATTEREDPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.SHATTEREDPLANET.DESCRIPTION, 96, "earth", new Dictionary<SimHashes, MathUtil.MinMax>(), (Dictionary<string, int>) null, Db.Get().ArtifactDropRates.None, 0, 0, 0, false));
    }
  }
}
