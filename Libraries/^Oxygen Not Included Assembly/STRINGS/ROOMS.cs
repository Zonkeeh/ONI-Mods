// Decompiled with JetBrains decompiler
// Type: STRINGS.ROOMS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class ROOMS
  {
    public class TYPES
    {
      public static LocString CONFLICTED = (LocString) "Conflicted Room";

      public class NEUTRAL
      {
        public static LocString NAME = (LocString) "Miscellaneous Room";
        public static LocString EFFECT = (LocString) "- No effect";
        public static LocString TOOLTIP = (LocString) "This area has walls and doors but no dedicated use.";
      }

      public class LATRINE
      {
        public static LocString NAME = (LocString) "Latrine";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Using a toilet in an enclosed room will improve Duplicants' Morale.";
      }

      public class PLUMBEDBATHROOM
      {
        public static LocString NAME = (LocString) "Washroom";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Using a fully plumbed Washroom will improve Duplicants' Morale.";
      }

      public class BARRACKS
      {
        public static LocString NAME = (LocString) "Barracks";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Sleeping in Barracks will improve Duplicants' Morale.";
      }

      public class BEDROOM
      {
        public static LocString NAME = (LocString) "Bedroom";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Sleeping in a private Bedroom will improve Duplicants' Morale.";
      }

      public class MESSHALL
      {
        public static LocString NAME = (LocString) "Mess Hall";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale.";
      }

      public class GREATHALL
      {
        public static LocString NAME = (LocString) "Great Hall";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Eating in a Great Hall will significantly improve Duplicants' Morale.";
      }

      public class HOSPITAL
      {
        public static LocString NAME = (LocString) "Hospital";
        public static LocString EFFECT = (LocString) "- Quarantine sick Duplicants";
        public static LocString TOOLTIP = (LocString) "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease.";
      }

      public class MASSAGE_CLINIC
      {
        public static LocString NAME = (LocString) "Massage Clinic";
        public static LocString EFFECT = (LocString) "- Massage stress relief bonus";
        public static LocString TOOLTIP = (LocString) "Receiving massages at a Massage Clinic will significantly improve Stress reduction.";
      }

      public class POWER_PLANT
      {
        public static LocString NAME = (LocString) "Power Plant";
        public static LocString EFFECT = (LocString) "- Enables Power Control Station use";
        public static LocString TOOLTIP = (LocString) "Generators built within a Power Plant can be tuned up using Power Control Stations to improve their power production.";
      }

      public class MACHINE_SHOP
      {
        public static LocString NAME = (LocString) "Machine Shop";
        public static LocString EFFECT = (LocString) "- Increased fabrication efficiency";
        public static LocString TOOLTIP = (LocString) "Duplicants working in a Machine Shop can maintain buildings and increase their production speed.";
      }

      public class FARM
      {
        public static LocString NAME = (LocString) "Greenhouse";
        public static LocString EFFECT = (LocString) "- Enables Farm Station use";
        public static LocString TOOLTIP = (LocString) "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";
      }

      public class CREATUREPEN
      {
        public static LocString NAME = (LocString) "Stable";
        public static LocString EFFECT = (LocString) "- Enables Grooming Station use";
        public static LocString TOOLTIP = (LocString) "Stabled critters can be tended at a Grooming Station to hasten their domestication and increase their production.";
      }

      public class REC_ROOM
      {
        public static LocString NAME = (LocString) "Recreation Room";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";
      }

      public class PARK
      {
        public static LocString NAME = (LocString) "Park";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "Passing through natural spaces throughout the day will raise the Morale of Duplicants.";
      }

      public class NATURERESERVE
      {
        public static LocString NAME = (LocString) "Nature Reserve";
        public static LocString EFFECT = (LocString) "- Morale bonus";
        public static LocString TOOLTIP = (LocString) "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";
      }

      public class PRIVATE_BEDROOM
      {
        public static LocString NAME = (LocString) "Private Bedroom";
        public static LocString EFFECT = (LocString) "- Stamina recovery bonus";
        public static LocString TOOLTIP = (LocString) "Duplicants recover even more stamina while sleeping in a Private Bedroom than in Barracks.";
      }

      public class PRIVATE_BATHROOM
      {
        public static LocString NAME = (LocString) "Private Bathroom";
        public static LocString EFFECT = (LocString) "- Stress relief bonus";
        public static LocString TOOLTIP = (LocString) "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine.";
      }
    }

    public class CRITERIA
    {
      public static LocString HEADER = (LocString) "<b>Requirements:</b>";
      public static LocString NEUTRAL_TYPE = (LocString) "Enclosed by wall tile";
      public static LocString POSSIBLE_TYPES_HEADER = (LocString) "Possible Room Types";
      public static LocString NO_TYPE_CONFLICTS = (LocString) "Remove conflicting buildings";

      public class CRITERIA_FAILED
      {
        public static LocString MISSING_BUILDING = (LocString) "Missing {0}";
        public static LocString FAILED = (LocString) "{0}";
      }

      public class CEILING_HEIGHT
      {
        public static LocString NAME = (LocString) "Minimum height: {0} tiles";
        public static LocString DESCRIPTION = (LocString) "Must have a ceiling height of at least {0} tiles";
      }

      public class MINIMUM_SIZE
      {
        public static LocString NAME = (LocString) "Minimum size: {0} tiles";
        public static LocString DESCRIPTION = (LocString) "Must have an area of at least {0} tiles";
      }

      public class MAXIMUM_SIZE
      {
        public static LocString NAME = (LocString) "Maximum size: {0} tiles";
        public static LocString DESCRIPTION = (LocString) "Must have an area no larger than {0} tiles";
      }

      public class BED_SINGLE
      {
        public static LocString NAME = (LocString) "Single bed";
        public static LocString DESCRIPTION = (LocString) "Requires one Cot or Comfy Bed";
      }

      public class LUXURY_BED_SINGLE
      {
        public static LocString NAME = (LocString) "Single Comfy Bed";
        public static LocString DESCRIPTION = (LocString) "Requires a Comfy Bed";
      }

      public class NO_COTS
      {
        public static LocString NAME = (LocString) "No Cots";
        public static LocString DESCRIPTION = (LocString) "Room cannot contain a Cot";
      }

      public class BED_MULTIPLE
      {
        public static LocString NAME = (LocString) "Beds";
        public static LocString DESCRIPTION = (LocString) "Requires two or more Cots or Comfy Beds";
      }

      public class BUILDING_DECOR_POSITIVE
      {
        public static LocString NAME = (LocString) "Positive decor";
        public static LocString DESCRIPTION = (LocString) "Requires at least one building with positive decor";
      }

      public class DECORATIVE_ITEM
      {
        public static LocString NAME = (LocString) "Decor item";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Paintings, Sculptures, or Vases";
      }

      public class DECORATIVE_ITEM_N
      {
        public static LocString NAME = (LocString) "Decor item: +{0} Decor";
        public static LocString DESCRIPTION = (LocString) "Requires a decorative item with a minimum Decor value of {0}";
      }

      public class CLINIC
      {
        public static LocString NAME = (LocString) "Medical equipment";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Sick Bays or Disease Clinics";
      }

      public class POWER_STATION
      {
        public static LocString NAME = (LocString) "Power Control Station";
        public static LocString DESCRIPTION = (LocString) "Requires a single Power Control Station";
      }

      public class FARM_STATION
      {
        public static LocString NAME = (LocString) "Farm Station";
        public static LocString DESCRIPTION = (LocString) "Requires a single Farm Station";
      }

      public class CREATURE_RELOCATOR
      {
        public static LocString NAME = (LocString) "Critter Relocator";
        public static LocString DESCRIPTION = (LocString) "Requires a single Critter Drop-Off";
      }

      public class CREATURE_FEEDER
      {
        public static LocString NAME = (LocString) "Critter Feeder";
        public static LocString DESCRIPTION = (LocString) "Requires a single Critter Feeder";
      }

      public class RANCH_STATION
      {
        public static LocString NAME = (LocString) "Grooming Station";
        public static LocString DESCRIPTION = (LocString) "Requires a single Grooming Station";
      }

      public class REC_BUILDING
      {
        public static LocString NAME = (LocString) "Recreational building";
        public static LocString DESCRIPTION = (LocString) "Requires one or more recreational buildings";
      }

      public class PARK_BUILDING
      {
        public static LocString NAME = (LocString) "Park Sign";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Park Signs";
      }

      public class MACHINE_SHOP
      {
        public static LocString NAME = (LocString) "Mechanics Station";
        public static LocString DESCRIPTION = (LocString) "Requires requires one or more Mechanics Stations";
      }

      public class FOOD_BOX
      {
        public static LocString NAME = (LocString) "Food storage";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Ration Boxes or Refrigerators";
      }

      public class LIGHT
      {
        public static LocString NAME = (LocString) "Light source";
        public static LocString DESCRIPTION = (LocString) "Requires one or more light sources";
      }

      public class MASSAGE_TABLE
      {
        public static LocString NAME = (LocString) "Massage Table";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Massage Tables";
      }

      public class MESS_STATION_SINGLE
      {
        public static LocString NAME = (LocString) "Mess Table";
        public static LocString DESCRIPTION = (LocString) "Requires a single Mess Table";
      }

      public class MESS_STATION_MULTIPLE
      {
        public static LocString NAME = (LocString) "Mess Tables";
        public static LocString DESCRIPTION = (LocString) "Requires two or more Mess Tables";
      }

      public class RESEARCH_STATION
      {
        public static LocString NAME = (LocString) "Research station";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Research Stations or Super Computers";
      }

      public class TOILET
      {
        public static LocString NAME = (LocString) "Toilet";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Outhouses or Lavatories";
      }

      public class FLUSH_TOILET
      {
        public static LocString NAME = (LocString) "Flush Toilet";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Lavatories";
      }

      public class NO_OUTHOUSES
      {
        public static LocString NAME = (LocString) "No Outhouses";
        public static LocString DESCRIPTION = (LocString) "Cannot contain basic Outhouses";
      }

      public class WASH_STATION
      {
        public static LocString NAME = (LocString) "Wash station";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";
      }

      public class ADVANCED_WASH_STATION
      {
        public static LocString NAME = (LocString) "Plumbed wash station";
        public static LocString DESCRIPTION = (LocString) "Requires one or more Sinks, Hand Sanitizers, or Showers";
      }

      public class NO_INDUSTRIAL_MACHINERY
      {
        public static LocString NAME = (LocString) "No industrial machinery";
        public static LocString DESCRIPTION = (LocString) "Cannot contain any building labeled Industrial Machinery";
      }

      public class WILDANIMAL
      {
        public static LocString NAME = (LocString) "Wildlife";
        public static LocString DESCRIPTION = (LocString) "At least one wild creature.";
      }

      public class WILDANIMALS
      {
        public static LocString NAME = (LocString) "More Wildlife";
        public static LocString DESCRIPTION = (LocString) "At least two wild creatures.";
      }

      public class WILDPLANT
      {
        public static LocString NAME = (LocString) "At least two Wild Plants";
        public static LocString DESCRIPTION = (LocString) "At least two wild plants.";
      }

      public class WILDPLANTS
      {
        public static LocString NAME = (LocString) "At least four Wild Plants";
        public static LocString DESCRIPTION = (LocString) "At least two wild plants.";
      }
    }

    public class DETAILS
    {
      public static LocString HEADER = (LocString) "Room Details";

      public class ASSIGNED_TO
      {
        public static LocString NAME = (LocString) "<b>Assignments:</b>\n{0}";
        public static LocString UNASSIGNED = (LocString) "Unassigned";
      }

      public class AVERAGE_TEMPERATURE
      {
        public static LocString NAME = (LocString) "Average temperature: {0}";
      }

      public class AVERAGE_ATMO_MASS
      {
        public static LocString NAME = (LocString) "Average air pressure: {0}";
      }

      public class SIZE
      {
        public static LocString NAME = (LocString) "Room size: {0} Tiles";
      }

      public class BUILDING_COUNT
      {
        public static LocString NAME = (LocString) "Buildings: {0}";
      }

      public class CREATURE_COUNT
      {
        public static LocString NAME = (LocString) "Critters: {0}";
      }

      public class PLANT_COUNT
      {
        public static LocString NAME = (LocString) "Plants: {0}";
      }
    }

    public class EFFECTS
    {
      public static LocString HEADER = (LocString) "<b>Effects:</b>";
    }
  }
}
