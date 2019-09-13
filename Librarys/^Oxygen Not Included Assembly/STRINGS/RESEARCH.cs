// Decompiled with JetBrains decompiler
// Type: STRINGS.RESEARCH
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class RESEARCH
  {
    public class MESSAGING
    {
      public static LocString NORESEARCHSELECTED = (LocString) "No research selected";
      public static LocString RESEARCHTYPEREQUIRED = (LocString) "{0} required";
      public static LocString RESEARCHTYPEALSOREQUIRED = (LocString) "{0} also required";
      public static LocString NO_RESEARCHER_SKILL = (LocString) "No Researchers assigned";
      public static LocString NO_RESEARCHER_SKILL_TOOLTIP = (LocString) ("The selected research focus requires " + UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", "[L]") + " and teach a Duplicant the " + (string) RESEARCH.TECHS.ADVANCEDRESEARCH.NAME + " Skill to use this building");
      public static LocString MISSING_RESEARCH_STATION = (LocString) "Missing Research Station";
      public static LocString MISSING_RESEARCH_STATION_TOOLTIP = (LocString) ("The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab") + " " + UI.FormatAsHotkey("[0]") + " of the Build Menu to construct one");
    }

    public class TYPES
    {
      public class ALPHA
      {
        public static LocString NAME = (LocString) "Novice Research";
        public static LocString DESC = (LocString) (UI.FormatAsLink("Novice Research", nameof (RESEARCH)) + " is required to unlock basic technologies.\nIt can be conducted at a " + (string) BUILDINGS.PREFABS.RESEARCHCENTER.NAME + ".");
        public static LocString RECIPEDESC = (LocString) "Unlocks rudimentary technologies.";
      }

      public class BETA
      {
        public static LocString NAME = (LocString) "Advanced Research";
        public static LocString DESC = (LocString) (UI.FormatAsLink("Advanced Research", nameof (RESEARCH)) + " is required to unlock improved technologies.\nIt can be conducted at a " + (string) BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + ".");
        public static LocString RECIPEDESC = (LocString) "Unlocks improved technologies.";
      }

      public class GAMMA
      {
        public static LocString NAME = (LocString) "Interstellar Research";
        public static LocString DESC = (LocString) (UI.FormatAsLink("Interstellar Research", nameof (RESEARCH)) + " is required to unlock space technologies.\nIt can be conducted at a " + (string) BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + ".");
        public static LocString RECIPEDESC = (LocString) "Unlocks cutting-edge technologies.";
      }
    }

    public class OTHER_TECH_ITEMS
    {
      public class AUTOMATION_OVERLAY
      {
        public static LocString NAME = (LocString) UI.FormatAsOverlay("Automation Overlay");
        public static LocString DESC = (LocString) ("Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".");
      }

      public class SUITS_OVERLAY
      {
        public static LocString NAME = (LocString) UI.FormatAsOverlay("Exosuit Overlay");
        public static LocString DESC = (LocString) ("Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".");
      }

      public class JET_SUIT
      {
        public static LocString NAME = (LocString) (UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern");
        public static LocString DESC = (LocString) ("Enables fabrication of " + UI.PRE_KEYWORD + "Jet Suits" + UI.PST_KEYWORD + " at the " + (string) BUILDINGS.PREFABS.SUITFABRICATOR.NAME);
      }

      public class BETA_RESEARCH_POINT
      {
        public static LocString NAME = (LocString) (UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability");
        public static LocString DESC = (LocString) ("Allows " + UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " points to be accumulated, unlocking higher technology tiers.");
      }

      public class GAMMA_RESEARCH_POINT
      {
        public static LocString NAME = (LocString) (UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability");
        public static LocString DESC = (LocString) ("Allows " + UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " points to be accumulated, unlocking higher technology tiers.");
      }

      public class CONVEYOR_OVERLAY
      {
        public static LocString NAME = (LocString) UI.FormatAsOverlay("Conveyor Overlay");
        public static LocString DESC = (LocString) ("Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".");
      }
    }

    public class TREES
    {
      public static LocString TITLE_FOOD = (LocString) "Food";
      public static LocString TITLE_POWER = (LocString) "Power";
      public static LocString TITLE_SOLIDS = (LocString) "Solid Material";
      public static LocString TITLE_COLONYDEVELOPMENT = (LocString) "Colony Development";
      public static LocString TITLE_MEDICINE = (LocString) "Medicine";
      public static LocString TITLE_LIQUIDS = (LocString) "Liquids";
      public static LocString TITLE_GASES = (LocString) "Gases";
      public static LocString TITLE_SUITS = (LocString) "Exosuits";
      public static LocString TITLE_DECOR = (LocString) "Decor";
      public static LocString TITLE_COMPUTERS = (LocString) "Computers";
      public static LocString TITLE_ROCKETS = (LocString) "Rocketry";
    }

    public class TECHS
    {
      public class JOBS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Employment", nameof (JOBS));
        public static LocString DESC = (LocString) "Exchange the skill points earned by Duplicants for new traits and abilities.";
      }

      public class IMPROVEDOXYGEN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Air Systems", nameof (IMPROVEDOXYGEN));
        public static LocString DESC = (LocString) "Maintain clean, breathable air in the colony.";
      }

      public class FARMINGTECH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Basic Farming", nameof (FARMINGTECH));
        public static LocString DESC = (LocString) ("Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.");
      }

      public class AGRICULTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Agriculture", nameof (AGRICULTURE));
        public static LocString DESC = (LocString) "Master the agricultural art of crop raising.";
      }

      public class RANCHING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ranching", nameof (RANCHING));
        public static LocString DESC = (LocString) "Tame and care for wild critters.";
      }

      public class ANIMALCONTROL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Animal Control", nameof (ANIMALCONTROL));
        public static LocString DESC = (LocString) "Useful techniques to manage critter populations in the colony.";
      }

      public class FINEDINING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Meal Preparation", nameof (FINEDINING));
        public static LocString DESC = (LocString) ("Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.");
      }

      public class FINERDINING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gourmet Meal Preparation", nameof (FINERDINING));
        public static LocString DESC = (LocString) ("Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".");
      }

      public class GASPIPING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ventilation", nameof (GASPIPING));
        public static LocString DESC = (LocString) ("Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.");
      }

      public class IMPROVEDGASPIPING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Improved Ventilation", nameof (IMPROVEDGASPIPING));
        public static LocString DESC = (LocString) (UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.");
      }

      public class TEMPERATUREMODULATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Temperature Modulation", nameof (TEMPERATUREMODULATION));
        public static LocString DESC = (LocString) ("Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.");
      }

      public class HVAC
      {
        public static LocString NAME = (LocString) UI.FormatAsLink(nameof (HVAC), nameof (HVAC));
        public static LocString DESC = (LocString) ("Regulate " + UI.FormatAsLink("Temperature", "HEAT") + " in the colony for " + UI.FormatAsLink("Plant", "PLANTS") + " cultivation and Duplicant comfort.");
      }

      public class LIQUIDTEMPERATURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Tuning", nameof (LIQUIDTEMPERATURE));
        public static LocString DESC = (LocString) ("Easily manipulate " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Heat", "Temperatures") + " with these temperature regulating technologies.");
      }

      public class INSULATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Insulation", nameof (INSULATION));
        public static LocString DESC = (LocString) ("Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.");
      }

      public class PRESSUREMANAGEMENT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pressure Management", nameof (PRESSUREMANAGEMENT));
        public static LocString DESC = (LocString) "Unlock technologies to manage colony pressure and atmosphere.";
      }

      public class DIRECTEDAIRSTREAMS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Decontamination", nameof (DIRECTEDAIRSTREAMS));
        public static LocString DESC = (LocString) ("Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.");
      }

      public class LIQUIDFILTERING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid-Based Refinement Processes", nameof (LIQUIDFILTERING));
        public static LocString DESC = (LocString) ("Use pumped liquids to remove " + UI.FormatAsLink("Salt", "SALT") + " from " + UI.FormatAsLink("Brine", "BRINE") + " or pull " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " from the air.");
      }

      public class LIQUIDPIPING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Plumbing", nameof (LIQUIDPIPING));
        public static LocString DESC = (LocString) ("Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.");
      }

      public class IMPROVEDLIQUIDPIPING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Improved Plumbing", nameof (IMPROVEDLIQUIDPIPING));
        public static LocString DESC = (LocString) (UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.");
      }

      public class PRECISIONPLUMBING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Caffeination", nameof (PRECISIONPLUMBING));
        public static LocString DESC = (LocString) "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
      }

      public class SANITATIONSCIENCES
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sanitation", nameof (SANITATIONSCIENCES));
        public static LocString DESC = (LocString) "Make daily ablutions less of a hassle.";
      }

      public class MEDICINEI
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pharmacology", nameof (MEDICINEI));
        public static LocString DESC = (LocString) ("Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.");
      }

      public class MEDICINEII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Medical Equipment", nameof (MEDICINEII));
        public static LocString DESC = (LocString) "The basic necessities doctors need to facilitate patient care.";
      }

      public class MEDICINEIII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pathogen Diagnostics", nameof (MEDICINEIII));
        public static LocString DESC = (LocString) "Stop Germs at the source using special medical automation technology.";
      }

      public class MEDICINEIV
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Micro-Targeted Medicine", nameof (MEDICINEIV));
        public static LocString DESC = (LocString) "State of the art equipment to conquer the most stubborn of illnesses.";
      }

      public class ADVANCEDFILTRATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Filtration", nameof (ADVANCEDFILTRATION));
        public static LocString DESC = (LocString) ("Basic technologies for filtering " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " and " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + ".");
      }

      public class POWERREGULATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Power Regulation", nameof (POWERREGULATION));
        public static LocString DESC = (LocString) ("Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.");
      }

      public class COMBUSTION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Internal Combustion", nameof (COMBUSTION));
        public static LocString DESC = (LocString) ("Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.");
      }

      public class IMPROVEDCOMBUSTION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fossil Fuels", nameof (IMPROVEDCOMBUSTION));
        public static LocString DESC = (LocString) ("Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.");
      }

      public class INTERIORDECOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Interior Decor", nameof (INTERIORDECOR));
        public static LocString DESC = (LocString) (UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.");
      }

      public class ARTISTRY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Artistic Expression", nameof (ARTISTRY));
        public static LocString DESC = (LocString) ("Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.");
      }

      public class CLOTHING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Textile Production", nameof (CLOTHING));
        public static LocString DESC = (LocString) ("Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.");
      }

      public class ACOUSTICS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sound Amplifiers", nameof (ACOUSTICS));
        public static LocString DESC = (LocString) "Precise control of the audio spectrum allows Duplicants to get funky.";
      }

      public class AMPLIFIERS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Power Amplifiers", nameof (AMPLIFIERS));
        public static LocString DESC = (LocString) ("Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.");
      }

      public class LUXURY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Home Luxuries", nameof (LUXURY));
        public static LocString DESC = (LocString) ("Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.");
      }

      public class FINEART
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fine Art", nameof (FINEART));
        public static LocString DESC = (LocString) ("Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.");
      }

      public class REFRACTIVEDECOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("High Culture", nameof (REFRACTIVEDECOR));
        public static LocString DESC = (LocString) "New methods for working with extremely high quality art materials.";
      }

      public class RENAISSANCEART
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Renaissance Art", nameof (RENAISSANCEART));
        public static LocString DESC = (LocString) "The kind of art that culture legacies are made of.";
      }

      public class GLASSFURNISHINGS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Glass Blowing", nameof (GLASSFURNISHINGS));
        public static LocString DESC = (LocString) "The decorative benefits of glass are both apparent and transparent.";
      }

      public class ADVANCEDPOWERREGULATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Power Regulation", nameof (ADVANCEDPOWERREGULATION));
        public static LocString DESC = (LocString) ("Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.");
      }

      public class PLASTICS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Plastic Manufacturing", nameof (PLASTICS));
        public static LocString DESC = (LocString) "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
      }

      public class SUITS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hazard Protection", nameof (SUITS));
        public static LocString DESC = (LocString) "Vital gear for surviving in extreme conditions and environments.";
      }

      public class DISTILLATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Distillation", nameof (DISTILLATION));
        public static LocString DESC = (LocString) "Distill difficult mixtures down to their most useful parts.";
      }

      public class CATALYTICS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Catalytics", nameof (CATALYTICS));
        public static LocString DESC = (LocString) "Advanced gas manipulation using unique catalysts.";
      }

      public class ADVANCEDRESEARCH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Research", nameof (ADVANCEDRESEARCH));
        public static LocString DESC = (LocString) "The tools my colony needs to conduct more advanced, in-depth research.";
      }

      public class LOGICCONTROL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Smart Home", nameof (LOGICCONTROL));
        public static LocString DESC = (LocString) "Switches that grant full control of building operations within the colony.";
      }

      public class LOGICCIRCUITS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Automation", nameof (LOGICCIRCUITS));
        public static LocString DESC = (LocString) "The only limit to colony automation is my own imagination.";
      }

      public class VALVEMINIATURIZATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Valve Miniaturization", nameof (VALVEMINIATURIZATION));
        public static LocString DESC = (LocString) "Smaller, more efficient pumps for those low-throughput situations.";
      }

      public class PRETTYGOODCONDUCTORS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Low-Resistance Conductors", nameof (PRETTYGOODCONDUCTORS));
        public static LocString DESC = (LocString) ("Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.");
      }

      public class RENEWABLEENERGY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Renewable Energy", nameof (RENEWABLEENERGY));
        public static LocString DESC = (LocString) ("Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.");
      }

      public class BASICREFINEMENT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Brute-Force Refinement", nameof (BASICREFINEMENT));
        public static LocString DESC = (LocString) "Low-tech refinement methods for producing clay and renewable sources of sand.";
      }

      public class REFINEDOBJECTS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Refined Renovations", nameof (REFINEDOBJECTS));
        public static LocString DESC = (LocString) ("Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".");
      }

      public class GENERICSENSORS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Generic Sensors", nameof (GENERICSENSORS));
        public static LocString DESC = (LocString) "Drive automation in a variety of new, inventive ways.";
      }

      public class DUPETRAFFICCONTROL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Computing", nameof (DUPETRAFFICCONTROL));
        public static LocString DESC = (LocString) "Virtually extend the boundaries of Duplicant imagination.";
      }

      public class SMELTING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Smelting", nameof (SMELTING));
        public static LocString DESC = (LocString) "High temperatures facilitate the production of purer, special use metal resources.";
      }

      public class TRAVELTUBES
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Transit Tubes", nameof (TRAVELTUBES));
        public static LocString DESC = (LocString) "A wholly futuristic way to move Duplicants around the base.";
      }

      public class SMARTSTORAGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Smart Storage", nameof (SMARTSTORAGE));
        public static LocString DESC = (LocString) "Completely automate the storage of solid resources.";
      }

      public class SOLIDTRANSPORT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solid Transport", nameof (SOLIDTRANSPORT));
        public static LocString DESC = (LocString) "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
      }

      public class HIGHTEMPFORGING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Superheated Forging", nameof (HIGHTEMPFORGING));
        public static LocString DESC = (LocString) "Craft entirely new materials by harnessing the most extreme temperatures.";
      }

      public class SKYDETECTORS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Celestial Detection", nameof (SKYDETECTORS));
        public static LocString DESC = (LocString) "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
      }

      public class JETPACKS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jetpacks", nameof (JETPACKS));
        public static LocString DESC = (LocString) "Objectively the most stylish way for Duplicants to get around.";
      }

      public class BASICROCKETRY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Introductory Rocketry", nameof (BASICROCKETRY));
        public static LocString DESC = (LocString) "Everything required for launching the colony's very first space program.";
      }

      public class ENGINESI
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solid Fuel Combustion", nameof (ENGINESI));
        public static LocString DESC = (LocString) "Rockets that fly further, longer.";
      }

      public class ENGINESII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydrocarbon Combustion", nameof (ENGINESII));
        public static LocString DESC = (LocString) "Delve deeper into the vastness of space than ever before.";
      }

      public class ENGINESIII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Cryofuel Combustion", nameof (ENGINESIII));
        public static LocString DESC = (LocString) "With this technology, the sky is your oyster. Go exploring!";
      }

      public class CARGOI
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solid Cargo", nameof (CARGOI));
        public static LocString DESC = (LocString) "Make extra use of journeys into space by mining and storing useful resources.";
      }

      public class CARGOII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid and Gas Cargo", nameof (CARGOII));
        public static LocString DESC = (LocString) "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
      }

      public class CARGOIII
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Unique Cargo", nameof (CARGOIII));
        public static LocString DESC = (LocString) "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
      }
    }
  }
}
