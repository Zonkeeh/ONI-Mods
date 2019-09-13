// Decompiled with JetBrains decompiler
// Type: STRINGS.BUILDING
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class BUILDING
  {
    public class STATUSITEMS
    {
      public class BAITED
      {
        public static LocString NAME = (LocString) "{0} Bait";
        public static LocString TOOLTIP = (LocString) "This lure is baited with {0}";
      }

      public class ANGERDAMAGE
      {
        public static LocString NAME = (LocString) "Damage: Duplicant Tantrum";
        public static LocString TOOLTIP = (LocString) "A stressed Duplicant is damaging this building";
        public static LocString NOTIFICATION = (LocString) "Building Damage: Duplicant Tantrum";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "Stressed Duplicants are damaging these buildings:\n\n{0}";
      }

      public class PIPECONTENTS
      {
        public static LocString EMPTY = (LocString) "Empty";
        public static LocString CONTENTS = (LocString) "{0} of {1} at {2}";
        public static LocString CONTENTS_WITH_DISEASE = (LocString) "\n  {0}";
      }

      public class CONVEYOR_CONTENTS
      {
        public static LocString EMPTY = (LocString) "Empty";
        public static LocString CONTENTS = (LocString) "{0} of {1} at {2}";
        public static LocString CONTENTS_WITH_DISEASE = (LocString) "\n  {0}";
      }

      public class ASSIGNEDTO
      {
        public static LocString NAME = (LocString) "Assigned to: {Assignee}";
        public static LocString TOOLTIP = (LocString) "Only {Assignee} can use this amenity";
      }

      public class ASSIGNEDPUBLIC
      {
        public static LocString NAME = (LocString) "Assigned to: Public";
        public static LocString TOOLTIP = (LocString) "Any Duplicant can use this amenity";
      }

      public class ASSIGNEDTOROOM
      {
        public static LocString NAME = (LocString) "Assigned to: {0}";
        public static LocString TOOLTIP = (LocString) ("Any Duplicant assigned to this " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD + " can use this amenity");
      }

      public class AWAITINGSEEDDELIVERY
      {
        public static LocString NAME = (LocString) "Awaiting Delivery";
        public static LocString TOOLTIP = (LocString) ("Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD);
      }

      public class AWAITINGBAITDELIVERY
      {
        public static LocString NAME = (LocString) "Awaiting Bait";
        public static LocString TOOLTIP = (LocString) ("Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD);
      }

      public class CLINICOUTSIDEHOSPITAL
      {
        public static LocString NAME = (LocString) "Medical building outside Hospital";
        public static LocString TOOLTIP = (LocString) ("Rebuild this medical equipment in a " + UI.PRE_KEYWORD + "Hospital" + UI.PST_KEYWORD + " to more effectively quarantine sick Duplicants");
      }

      public class BOTTLE_EMPTIER
      {
        public static class ALLOWED
        {
          public static LocString NAME = (LocString) "Auto-Bottle: On";
          public static LocString TOOLTIP = (LocString) ("Duplicants may specifically fetch " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " from a " + (string) BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.NAME + " to bring to this location");
        }

        public static class DENIED
        {
          public static LocString NAME = (LocString) "Auto-Bottle: Off";
          public static LocString TOOLTIP = (LocString) ("Duplicants may not specifically fetch " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " from a " + (string) BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.NAME + " to bring to this location");
        }
      }

      public class CANISTER_EMPTIER
      {
        public static class ALLOWED
        {
          public static LocString NAME = (LocString) "Auto-Canister: On";
          public static LocString TOOLTIP = (LocString) ("Duplicants may specifically fetch " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " from a " + (string) BUILDINGS.PREFABS.GASBOTTLER.NAME + " to bring to this location");
        }

        public static class DENIED
        {
          public static LocString NAME = (LocString) "Auto-Canister: Off";
          public static LocString TOOLTIP = (LocString) ("Duplicants may not specifically fetch " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " from a " + (string) BUILDINGS.PREFABS.GASBOTTLER.NAME + " to bring to this location");
        }
      }

      public class BROKEN
      {
        public static LocString NAME = (LocString) "Broken";
        public static LocString TOOLTIP = (LocString) "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
      }

      public class CHANGEDOORCONTROLSTATE
      {
        public static LocString NAME = (LocString) "Pending Door State Change: {ControlState}";
        public static LocString TOOLTIP = (LocString) "Waiting for a Duplicant to change control state";
      }

      public class SUIT_LOCKER
      {
        public class NEED_CONFIGURATION
        {
          public static LocString NAME = (LocString) "Current Status: Needs Configuration";
          public static LocString TOOLTIP = (LocString) "Set this dock to store a suit or leave it empty";
        }

        public class READY
        {
          public static LocString NAME = (LocString) "Current Status: Empty";
          public static LocString TOOLTIP = (LocString) ("This dock is ready to receive a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD + ", either by manual delivery or from a Duplicant returning the suit they're wearing");
        }

        public class SUIT_REQUESTED
        {
          public static LocString NAME = (LocString) "Current Status: Awaiting Delivery";
          public static LocString TOOLTIP = (LocString) ("Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD);
        }

        public class CHARGING
        {
          public static LocString NAME = (LocString) "Current Status: Charging Suit";
          public static LocString TOOLTIP = (LocString) ("This " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD + " is docked and refueling");
        }

        public class NO_OXYGEN
        {
          public static LocString NAME = (LocString) "Current Status: No Oxygen";
          public static LocString TOOLTIP = (LocString) ("This dock does not contain enough " + (string) ELEMENTS.OXYGEN.NAME + " to refill a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD);
        }

        public class NO_FUEL
        {
          public static LocString NAME = (LocString) "Current Status: No Fuel";
          public static LocString TOOLTIP = (LocString) ("This dock does not contain enough " + (string) ELEMENTS.PETROLEUM.NAME + " to refill a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD);
        }

        public class NOT_OPERATIONAL
        {
          public static LocString NAME = (LocString) "Current Status: Offline";
          public static LocString TOOLTIP = (LocString) ("This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD);
        }

        public class FULLY_CHARGED
        {
          public static LocString NAME = (LocString) "Current Status: Full Fueled";
          public static LocString TOOLTIP = (LocString) "This suit is fully refueled and ready for use";
        }
      }

      public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
      {
        public static LocString NAME = (LocString) "Clearance: Vacancy Only";
        public static LocString TOOLTIP = (LocString) ("Suited Duplicants may pass only if there is room in a " + UI.PRE_KEYWORD + "Dock" + UI.PST_KEYWORD + " to store their " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD);
      }

      public class SUITMARKERTRAVERSALANYTIME
      {
        public static LocString NAME = (LocString) "Clearance: Always Permitted";
        public static LocString TOOLTIP = (LocString) ("Suited Duplicants may pass even if there is no room to store their " + UI.PRE_KEYWORD + "Suits" + UI.PST_KEYWORD + UI.HORIZONTAL_BR_RULE + "When all available docks are full, Duplicants will unequip their " + UI.PRE_KEYWORD + "Suits" + UI.PST_KEYWORD + " and drop them on the floor");
      }

      public class SUIT_LOCKER_NEEDS_CONFIGURATION
      {
        public static LocString NAME = (LocString) "Not Configured";
        public static LocString TOOLTIP = (LocString) "Dock settings not configured";
      }

      public class CURRENTDOORCONTROLSTATE
      {
        public static LocString NAME = (LocString) "Current State: {ControlState}";
        public static LocString TOOLTIP = (LocString) ("Current State: {ControlState}" + UI.HORIZONTAL_BR_RULE + "Auto: Duplicants open and close this door as needed\nLocked: Nothing may pass through\nOpen: This door will remain open");
        public static LocString OPENED = (LocString) "Opened";
        public static LocString AUTO = (LocString) "Auto";
        public static LocString LOCKED = (LocString) "Locked";
      }

      public class CONDUITBLOCKED
      {
        public static LocString NAME = (LocString) "Pipe Blocked";
        public static LocString TOOLTIP = (LocString) ("Output " + UI.PRE_KEYWORD + "Pipe" + UI.PST_KEYWORD + " is blocked");
      }

      public class OUTPUTPIPEFULL
      {
        public static LocString NAME = (LocString) "Output Pipe Full";
        public static LocString TOOLTIP = (LocString) ("Unable to flush contents, output " + UI.PRE_KEYWORD + "Pipe" + UI.PST_KEYWORD + " is blocked");
      }

      public class CONSTRUCTIONUNREACHABLE
      {
        public static LocString NAME = (LocString) "Unreachable Build";
        public static LocString TOOLTIP = (LocString) "Duplicants cannot reach this construction site";
      }

      public class MOPUNREACHABLE
      {
        public static LocString NAME = (LocString) "Unreachable Mop";
        public static LocString TOOLTIP = (LocString) "Duplicants cannot reach this area";
      }

      public class DIGUNREACHABLE
      {
        public static LocString NAME = (LocString) "Unreachable Dig";
        public static LocString TOOLTIP = (LocString) "Duplicants cannot reach this area";
      }

      public class CONSTRUCTABLEDIGUNREACHABLE
      {
        public static LocString NAME = (LocString) "Unreachable Dig";
        public static LocString TOOLTIP = (LocString) "This construction site contains cells that cannot be dug out";
      }

      public class EMPTYPUMPINGSTATION
      {
        public static LocString NAME = (LocString) "Empty";
        public static LocString TOOLTIP = (LocString) ("This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD);
      }

      public class ENTOMBED
      {
        public static LocString NAME = (LocString) "Entombed";
        public static LocString TOOLTIP = (LocString) "Must be dug out by a Duplicant";
        public static LocString NOTIFICATION_NAME = (LocString) "Building entombment";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "These buildings are entombed and need to be dug out:";
      }

      public class INVALIDPORTOVERLAP
      {
        public static LocString NAME = (LocString) "Invalid Port Overlap";
        public static LocString TOOLTIP = (LocString) ("Ports on this building overlap those on another building" + UI.HORIZONTAL_BR_RULE + "This building must be rebuilt in a valid location");
        public static LocString NOTIFICATION_NAME = (LocString) "Building has overlapping ports";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "These buildings must be rebuilt with non-overlapping ports:";
      }

      public class GENESHUFFLECOMPLETED
      {
        public static LocString NAME = (LocString) "Vacillation Complete";
        public static LocString TOOLTIP = (LocString) "The Duplicant has completed the neural vacillation process and is ready to be released";
      }

      public class OVERHEATED
      {
        public static LocString NAME = (LocString) "Damage: Overheating";
        public static LocString TOOLTIP = (LocString) "This building is taking damage and will break down if not cooled";
      }

      public class OVERLOADED
      {
        public static LocString NAME = (LocString) "Damage: Overloading";
        public static LocString TOOLTIP = (LocString) ("This " + UI.PRE_KEYWORD + "Wire" + UI.PST_KEYWORD + " is taking damage because there are too many buildings pulling " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " from this circuit\n\nSplit this " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " circuit into multiple circuits, or use higher quality " + UI.PRE_KEYWORD + "Wires" + UI.PST_KEYWORD + " to prevent overloading");
      }

      public class OPERATINGENERGY
      {
        public static LocString NAME = (LocString) "Heat Production: {0}/s";
        public static LocString TOOLTIP = (LocString) "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";
        public static LocString LINEITEM = (LocString) "    • {0}: {1}\n";
        public static LocString OPERATING = (LocString) "Normal operation";
        public static LocString EXHAUSTING = (LocString) "Excess produced";
        public static LocString PIPECONTENTS_TRANSFER = (LocString) "Transferred from pipes";
      }

      public class FLOODED
      {
        public static LocString NAME = (LocString) "Building Flooded";
        public static LocString TOOLTIP = (LocString) "Building cannot function at current saturation";
        public static LocString NOTIFICATION_NAME = (LocString) "Flooding";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "These buildings are flooded:";
      }

      public class GASVENTOBSTRUCTED
      {
        public static LocString NAME = (LocString) "Gas Vent Obstructed";
        public static LocString TOOLTIP = (LocString) ("A " + UI.PRE_KEYWORD + "Pipe" + UI.PST_KEYWORD + " has been obstructed and is preventing gas flow to this vent");
      }

      public class GASVENTOVERPRESSURE
      {
        public static LocString NAME = (LocString) "Gas Vent Overpressure";
        public static LocString TOOLTIP = (LocString) ("High " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " or " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " pressure in this area is preventing further " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " emission\nReduce pressure by pumping " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " away or clearing more space");
      }

      public class DIRECTION_CONTROL
      {
        public static LocString NAME = (LocString) "Use Direction: {Direction}";
        public static LocString TOOLTIP = (LocString) "Duplicants will only use this building when walking by it. Currently allowed direction: <b>{Direction}</b>";

        public static class DIRECTIONS
        {
          public static LocString LEFT = (LocString) "Left";
          public static LocString RIGHT = (LocString) "Right";
          public static LocString BOTH = (LocString) "Both";
        }
      }

      public class WATTSONGAMEOVER
      {
        public static LocString NAME = (LocString) "Colony Lost";
        public static LocString TOOLTIP = (LocString) "All Duplicants are dead or incapacitated";
      }

      public class INVALIDBUILDINGLOCATION
      {
        public static LocString NAME = (LocString) "Invalid Building Location";
        public static LocString TOOLTIP = (LocString) "Cannot construct building in this location";
      }

      public class LIQUIDVENTOBSTRUCTED
      {
        public static LocString NAME = (LocString) "Liquid Vent Obstructed";
        public static LocString TOOLTIP = (LocString) ("A " + UI.PRE_KEYWORD + "Pipe" + UI.PST_KEYWORD + " has been obstructed and is preventing liquid flow to this vent");
      }

      public class LIQUIDVENTOVERPRESSURE
      {
        public static LocString NAME = (LocString) "Liquid Vent Overpressure";
        public static LocString TOOLTIP = (LocString) ("High " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " or " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " pressure in this area is preventing further " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " emission\nReduce pressure by pumping " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " away or clearing more space");
      }

      public class MANUALLYCONTROLLED
      {
        public static LocString NAME = (LocString) "Manually Controlled";
        public static LocString TOOLTIP = (LocString) "This Duplicant is under my control";
      }

      public class MATERIALSUNAVAILABLE
      {
        public static LocString NAME = (LocString) "Insufficient Resources\n{ItemsRemaining}";
        public static LocString TOOLTIP = (LocString) "Crucial materials for this building are beyond reach or unavailable";
        public static LocString NOTIFICATION_NAME = (LocString) "Building lacks resources";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "Crucial materials are unavailable or beyond reach for these buildings:";
        public static LocString LINE_ITEM_MASS = (LocString) "• {0}: {1}";
        public static LocString LINE_ITEM_UNITS = (LocString) "• {0}";
      }

      public class MATERIALSUNAVAILABLEFORREFILL
      {
        public static LocString NAME = (LocString) "Resources Low\n{ItemsRemaining}";
        public static LocString TOOLTIP = (LocString) "This building will soon require materials that are unavailable";
        public static LocString LINE_ITEM = (LocString) "• {0}";
      }

      public class MELTINGDOWN
      {
        public static LocString NAME = (LocString) "Breaking Down";
        public static LocString TOOLTIP = (LocString) "This building is collapsing";
        public static LocString NOTIFICATION_NAME = (LocString) "Building break down";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "These buildings are collapsing:";
      }

      public class MISSINGFOUNDATION
      {
        public static LocString NAME = (LocString) "Missing Tile";
        public static LocString TOOLTIP = (LocString) ("Build " + UI.FormatAsLink("Tile", "TILE") + " beneath this building" + UI.HORIZONTAL_BR_RULE + "Tile can be found in the " + UI.FormatAsBuildMenuTab("Base Tab") + " " + UI.FormatAsHotkey("[1]") + " of the Build Menu");
      }

      public class NEUTRONIUMUNMINABLE
      {
        public static LocString NAME = (LocString) "Cannot Mine";
        public static LocString TOOLTIP = (LocString) "This resource cannot be mined by Duplicant tools";
      }

      public class NEEDGASIN
      {
        public static LocString NAME = (LocString) "No Gas Intake\n{GasRequired}";
        public static LocString TOOLTIP = (LocString) ("This building's " + UI.PRE_KEYWORD + "Gas Intake" + UI.PST_KEYWORD + " does not have a " + (string) BUILDINGS.PREFABS.GASCONDUIT.NAME + " connected");
        public static LocString LINE_ITEM = (LocString) "• {0}";
      }

      public class NEEDGASOUT
      {
        public static LocString NAME = (LocString) "No Gas Output";
        public static LocString TOOLTIP = (LocString) ("This building's " + UI.PRE_KEYWORD + "Gas Output" + UI.PST_KEYWORD + " does not have a " + (string) BUILDINGS.PREFABS.GASCONDUIT.NAME + " connected");
      }

      public class NEEDLIQUIDIN
      {
        public static LocString NAME = (LocString) "No Liquid Intake\n{LiquidRequired}";
        public static LocString TOOLTIP = (LocString) ("This building's " + UI.PRE_KEYWORD + "Liquid Intake" + UI.PST_KEYWORD + " does not have a " + (string) BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME + " connected");
        public static LocString LINE_ITEM = (LocString) "• {0}";
      }

      public class NEEDLIQUIDOUT
      {
        public static LocString NAME = (LocString) "No Liquid Output";
        public static LocString TOOLTIP = (LocString) ("This building's " + UI.PRE_KEYWORD + "Liquid Output" + UI.PST_KEYWORD + " does not have a " + (string) BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME + " connected");
      }

      public class LIQUIDPIPEEMPTY
      {
        public static LocString NAME = (LocString) "Empty Pipe";
        public static LocString TOOLTIP = (LocString) ("There is no " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " in this pipe");
      }

      public class LIQUIDPIPEOBSTRUCTED
      {
        public static LocString NAME = (LocString) "Not Pumping";
        public static LocString TOOLTIP = (LocString) "This pump is not active";
      }

      public class GASPIPEEMPTY
      {
        public static LocString NAME = (LocString) "Empty Pipe";
        public static LocString TOOLTIP = (LocString) ("There is no " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " in this pipe");
      }

      public class GASPIPEOBSTRUCTED
      {
        public static LocString NAME = (LocString) "Not Pumping";
        public static LocString TOOLTIP = (LocString) "This pump is not active";
      }

      public class NEEDSOLIDIN
      {
        public static LocString NAME = (LocString) "No Conveyor Loader";
        public static LocString TOOLTIP = (LocString) ("Material cannot be fed onto this Conveyor system for transport" + UI.HORIZONTAL_BR_RULE + "Enter the " + UI.FormatAsBuildMenuTab("Shipping Tab") + " " + UI.FormatAsHotkey("[7]") + " of the Build Menu to build and connect a " + (string) BUILDINGS.PREFABS.SOLIDCONDUITINBOX.NAME);
      }

      public class NEEDSOLIDOUT
      {
        public static LocString NAME = (LocString) "No Conveyor Receptacle";
        public static LocString TOOLTIP = (LocString) ("Material cannot be offloaded from this Conveyor system and will backup the rails" + UI.HORIZONTAL_BR_RULE + "Enter the " + UI.FormatAsBuildMenuTab("Shipping Tab") + " " + UI.FormatAsHotkey("[7]") + " of the Build Menu to build and connect a " + UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"));
      }

      public class SOLIDPIPEOBSTRUCTED
      {
        public static LocString NAME = (LocString) "Conveyor Rail Backup";
        public static LocString TOOLTIP = (LocString) ("This Conveyor Rail cannot carry anymore material" + UI.HORIZONTAL_BR_RULE + "Remove material from the " + UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX") + " to free space for more objects");
      }

      public class NEEDPLANT
      {
        public static LocString NAME = (LocString) "No Seeds";
        public static LocString TOOLTIP = (LocString) "Uproot wild plants to obtain seeds";
      }

      public class NEEDSEED
      {
        public static LocString NAME = (LocString) "No Seed Selected";
        public static LocString TOOLTIP = (LocString) "Uproot wild plants to obtain seeds";
      }

      public class NEEDPOWER
      {
        public static LocString NAME = (LocString) "No Power";
        public static LocString TOOLTIP = (LocString) ("All connected " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " sources have lost charge");
      }

      public class NOTENOUGHPOWER
      {
        public static LocString NAME = (LocString) "Insufficient Power";
        public static LocString TOOLTIP = (LocString) ("This building does not have enough stored " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " to run");
      }

      public class POWERLOOPDETECTED
      {
        public static LocString NAME = (LocString) "Power Loop Detected";
        public static LocString TOOLTIP = (LocString) ("A Transformer's " + UI.PRE_KEYWORD + "Power Output " + UI.PST_KEYWORD + "should not be connected back to its own " + UI.PRE_KEYWORD + "Input" + UI.PST_KEYWORD);
      }

      public class NEEDRESOURCE
      {
        public static LocString NAME = (LocString) "Resource Required";
        public static LocString TOOLTIP = (LocString) "This building is missing required materials";
      }

      public class NEWDUPLICANTSAVAILABLE
      {
        public static LocString NAME = (LocString) "Printables Available";
        public static LocString TOOLTIP = (LocString) "I am ready to print a new colony member or care package";
        public static LocString NOTIFICATION_NAME = (LocString) "New Printables are available";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) ("The Printing Pod " + UI.FormatAsHotkey("[H]") + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:");
      }

      public class NOAPPLICABLERESEARCHSELECTED
      {
        public static LocString NAME = (LocString) "Inapplicable Research";
        public static LocString TOOLTIP = (LocString) ("This building cannot produce the correct " + UI.PRE_KEYWORD + "Research Type" + UI.PST_KEYWORD + " for the current " + UI.FormatAsLink("Research Focus", "TECH"));
        public static LocString NOTIFICATION_NAME = (LocString) (UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle");
        public static LocString NOTIFICATION_TOOLTIP = (LocString) ("These buildings cannot produce the correct " + UI.PRE_KEYWORD + "Research Type" + UI.PST_KEYWORD + " for the selected " + UI.FormatAsLink("Research Focus", "TECH") + ":");
      }

      public class NOAPPLICABLEANALYSISSELECTED
      {
        public static LocString NAME = (LocString) "No Analysis Focus Selected";
        public static LocString TOOLTIP = (LocString) ("Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", "[Z]") + " to begin analysis");
        public static LocString NOTIFICATION_NAME = (LocString) (UI.FormatAsLink("Telescope", "TELESCOPE") + " idle");
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "These buildings require an analysis focus:";
      }

      public class NOAVAILABLESEED
      {
        public static LocString NAME = (LocString) "No Seed Available";
        public static LocString TOOLTIP = (LocString) ("The selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD + " is not available");
      }

      public class NOSTORAGEFILTERSET
      {
        public static LocString NAME = (LocString) "Filters Not Designated";
        public static LocString TOOLTIP = (LocString) "No resources types are marked for storage in this building";
      }

      public class NOSUITMARKER
      {
        public static LocString NAME = (LocString) "No Checkpoint";
        public static LocString TOOLTIP = (LocString) ("Docks must be placed beside a " + (string) BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces");
      }

      public class SUITMARKERWRONGSIDE
      {
        public static LocString NAME = (LocString) "Invalid Checkpoint";
        public static LocString TOOLTIP = (LocString) ("This building has been built on the wrong side of a " + (string) BUILDINGS.PREFABS.CHECKPOINT.NAME + "\n\nDocks must be placed beside a " + (string) BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces");
      }

      public class NOFILTERELEMENTSELECTED
      {
        public static LocString NAME = (LocString) "No Filter Selected";
        public static LocString TOOLTIP = (LocString) "Select a resource to filter";
      }

      public class NOLUREELEMENTSELECTED
      {
        public static LocString NAME = (LocString) "No Bait Selected";
        public static LocString TOOLTIP = (LocString) "Select a resource to use as bait";
      }

      public class NOFISHABLEWATERBELOW
      {
        public static LocString NAME = (LocString) "No Fishable Water";
        public static LocString TOOLTIP = (LocString) ("There are no edible " + UI.PRE_KEYWORD + "Fish" + UI.PST_KEYWORD + " beneath this structure");
      }

      public class NOPOWERCONSUMERS
      {
        public static LocString NAME = (LocString) "No Power Consumers";
        public static LocString TOOLTIP = (LocString) ("No buildings are connected to this " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " source");
      }

      public class NOWIRECONNECTED
      {
        public static LocString NAME = (LocString) "No Power Wire Connected";
        public static LocString TOOLTIP = (LocString) ("This building has not been connected to a " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " grid");
      }

      public class PENDINGDECONSTRUCTION
      {
        public static LocString NAME = (LocString) "Deconstruction Errand";
        public static LocString TOOLTIP = (LocString) "Building will be deconstructed once a Duplicant is available";
      }

      public class PENDINGFISH
      {
        public static LocString NAME = (LocString) "Fishing Errand";
        public static LocString TOOLTIP = (LocString) "Spot will be fished once a Duplicant is available";
      }

      public class PENDINGHARVEST
      {
        public static LocString NAME = (LocString) "Harvest Errand";
        public static LocString TOOLTIP = (LocString) "Plant will be harvested once a Duplicant is available";
      }

      public class PENDINGUPROOT
      {
        public static LocString NAME = (LocString) "Uproot Errand";
        public static LocString TOOLTIP = (LocString) "Plant will be uprooted once a Duplicant is available";
      }

      public class PENDINGREPAIR
      {
        public static LocString NAME = (LocString) "Repair Errand";
        public static LocString TOOLTIP = (LocString) "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
      }

      public class PENDINGSWITCHTOGGLE
      {
        public static LocString NAME = (LocString) "Settings Errand";
        public static LocString TOOLTIP = (LocString) "Settings will be changed once a Duplicant is available";
      }

      public class PENDINGWORK
      {
        public static LocString NAME = (LocString) "Work Errand";
        public static LocString TOOLTIP = (LocString) "Building will be operated once a Duplicant is available";
      }

      public class POWERBUTTONOFF
      {
        public static LocString NAME = (LocString) "Function Suspended";
        public static LocString TOOLTIP = (LocString) ("This building has been toggled off\nPress " + UI.PRE_KEYWORD + "Enable Building" + UI.PST_KEYWORD + " " + UI.FormatAsHotkey("[ENTER]") + "to resume its use");
      }

      public class PUMPINGSTATION
      {
        public static LocString NAME = (LocString) "Liquid Available: {Liquids}";
        public static LocString TOOLTIP = (LocString) "This pumping station has access to: {Liquids}";
      }

      public class PRESSUREOK
      {
        public static LocString NAME = (LocString) "Max Gas Pressure";
        public static LocString TOOLTIP = (LocString) ("High ambient " + UI.PRE_KEYWORD + "Gas Pressure" + UI.PST_KEYWORD + " is preventing this building from emitting gas" + UI.HORIZONTAL_BR_RULE + "Reduce pressure by pumping " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " away or clearing more space");
      }

      public class UNDERPRESSURE
      {
        public static LocString NAME = (LocString) "Low Air Pressure";
        public static LocString TOOLTIP = (LocString) "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
      }

      public class STORAGELOCKER
      {
        public static LocString NAME = (LocString) "Storing: {Stored} / {Capacity} {Units}";
        public static LocString TOOLTIP = (LocString) "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
      }

      public class SKILL_POINTS_AVAILABLE
      {
        public static LocString NAME = (LocString) "Skill Points available";
        public static LocString TOOLTIP = (LocString) ("A Duplicant has " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD + " available");
      }

      public class UNASSIGNED
      {
        public static LocString NAME = (LocString) "Unassigned";
        public static LocString TOOLTIP = (LocString) "Assign a Duplicant to use this amenity";
      }

      public class UNDERCONSTRUCTION
      {
        public static LocString NAME = (LocString) "Under Construction";
        public static LocString TOOLTIP = (LocString) "This building is currently being built";
      }

      public class UNDERCONSTRUCTIONNOWORKER
      {
        public static LocString NAME = (LocString) "Construction Errand";
        public static LocString TOOLTIP = (LocString) "Building will be constructed once a Duplicant is available";
      }

      public class WAITINGFORMATERIALS
      {
        public static LocString NAME = (LocString) "Awaiting Delivery\n{ItemsRemaining}";
        public static LocString TOOLTIP = (LocString) "These materials will be delivered once a Duplicant is available";
        public static LocString LINE_ITEM_MASS = (LocString) "• {0}: {1}";
        public static LocString LINE_ITEM_UNITS = (LocString) "• {0}";
      }

      public class WAITINGFORREPAIRMATERIALS
      {
        public static LocString NAME = (LocString) "Awaiting Repair Delivery\n{ItemsRemaining}\n";
        public static LocString TOOLTIP = (LocString) "These materials must be delivered before this building can be repaired";
        public static LocString LINE_ITEM = (LocString) ("• " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + ": <b>{1}</b>");
      }

      public class MISSINGGANTRY
      {
        public static LocString NAME = (LocString) "Missing Gantry";
        public static LocString TOOLTIP = (LocString) ("A " + UI.FormatAsLink("Gantry", "GANTRY") + " must be built below " + UI.FormatAsLink("Command Capsules", "COMMANDMODULE") + " and " + UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE") + " for Duplicants access");
      }

      public class DISEMBARKINGDUPLICANT
      {
        public static LocString NAME = (LocString) "Waiting To Disembark";
        public static LocString TOOLTIP = (LocString) ("The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended");
      }

      public class ROCKETNAME
      {
        public static LocString NAME = (LocString) "Parent Rocket: {0}";
        public static LocString TOOLTIP = (LocString) ("This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD);
      }

      public class HASGANTRY
      {
        public static LocString NAME = (LocString) "Has Gantry";
        public static LocString TOOLTIP = (LocString) "Duplicants may now enter this section of the rocket";
      }

      public class NORMAL
      {
        public static LocString NAME = (LocString) "Normal";
        public static LocString TOOLTIP = (LocString) "Nothing out of the ordinary here";
      }

      public class MANUALGENERATORCHARGINGUP
      {
        public static LocString NAME = (LocString) "Charging Up";
        public static LocString TOOLTIP = (LocString) "This power source is being charged";
      }

      public class MANUALGENERATORRELEASINGENERGY
      {
        public static LocString NAME = (LocString) "Powering";
        public static LocString TOOLTIP = (LocString) ("This generator is supplying energy to " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " consumers");
      }

      public class GENERATOROFFLINE
      {
        public static LocString NAME = (LocString) "Generator Idle";
        public static LocString TOOLTIP = (LocString) ("This " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " source is idle");
      }

      public class PIPE
      {
        public static LocString NAME = (LocString) "Contents: {Contents}";
        public static LocString TOOLTIP = (LocString) "This pipe is delivering {Contents}";
      }

      public class CONVEYOR
      {
        public static LocString NAME = (LocString) "Contents: {Contents}";
        public static LocString TOOLTIP = (LocString) "This conveyor is delivering {Contents}";
      }

      public class FABRICATORIDLE
      {
        public static LocString NAME = (LocString) "No Fabrications Queued";
        public static LocString TOOLTIP = (LocString) "Select a recipe to begin fabrication";
      }

      public class FABRICATOREMPTY
      {
        public static LocString NAME = (LocString) "Waiting For Materials";
        public static LocString TOOLTIP = (LocString) "Fabrication will begin once materials have been deliverd";
      }

      public class TOILET
      {
        public static LocString NAME = (LocString) "{FlushesRemaining} \"Visits\" Remaining";
        public static LocString TOOLTIP = (LocString) "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
      }

      public class TOILETNEEDSEMPTYING
      {
        public static LocString NAME = (LocString) "Requires Emptying";
        public static LocString TOOLTIP = (LocString) ("This amenity cannot be used while full" + UI.HORIZONTAL_BR_RULE + "Emptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND"));
      }

      public class DESALINATORNEEDSEMPTYING
      {
        public static LocString NAME = (LocString) "Requires Emptying";
        public static LocString TOOLTIP = (LocString) ("This building is full of " + UI.FormatAsLink("Salt", "SALT") + " and can no longer operate." + UI.HORIZONTAL_BR_RULE + "A Duplicant must empty it for operation to continue.");
      }

      public class HABITATNEEDSEMPTYING
      {
        public static LocString NAME = (LocString) "Requires Emptying";
        public static LocString TOOLTIP = (LocString) ("This " + UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT") + " needs to be emptied of " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + UI.HORIZONTAL_BR_RULE + UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER") + " can be used to transport and dispose of " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " in designated areas");
      }

      public class UNUSABLE
      {
        public static LocString NAME = (LocString) "Out of Order";
        public static LocString TOOLTIP = (LocString) "This amenity requires maintenance";
      }

      public class NORESEARCHSELECTED
      {
        public static LocString NAME = (LocString) "No Research Focus Selected";
        public static LocString TOOLTIP = (LocString) ("Open the " + UI.FormatAsManagementMenu("Research Tree", "[R]") + " to select a new " + UI.FormatAsLink("Research", "TECH") + " project");
        public static LocString NOTIFICATION_NAME = (LocString) ("No " + UI.FormatAsLink("Research Focus", "TECH") + " selected");
        public static LocString NOTIFICATION_TOOLTIP = (LocString) ("Open the " + UI.FormatAsManagementMenu("Research Tree", "[R]") + " to select a new " + UI.FormatAsLink("Research", "TECH") + " project");
      }

      public class NORESEARCHORDESTINATIONSELECTED
      {
        public static LocString NAME = (LocString) "No Research Focus or Starmap Destination Selected";
        public static LocString TOOLTIP = (LocString) ("Select a " + UI.FormatAsLink("Research", "TECH") + " project in the " + UI.FormatAsManagementMenu("Research Tree", "{Hotkey}") + " or a Destination in the " + UI.FormatAsManagementMenu("Starmap", "[Z]"));
        public static LocString NOTIFICATION_NAME = (LocString) ("No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected");
        public static LocString NOTIFICATION_TOOLTIP = (LocString) ("Select a " + UI.FormatAsLink("Research", "TECH") + " project in the " + UI.FormatAsManagementMenu("Research Tree", "[R]") + " or a Destination in the " + UI.FormatAsManagementMenu("Starmap", "[Z]"));
      }

      public class RESEARCHING
      {
        public static LocString NAME = (LocString) ("Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}");
        public static LocString TOOLTIP = (LocString) "Research produced at this station will be invested in {Tech}";
      }

      public class TINKERING
      {
        public static LocString NAME = (LocString) "Tinkering: {0}";
        public static LocString TOOLTIP = (LocString) "This Duplicant is creating {0} to use somewhere else";
      }

      public class VALVE
      {
        public static LocString NAME = (LocString) "Max Flow Rate: {MaxFlow}";
        public static LocString TOOLTIP = (LocString) "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
      }

      public class VALVEREQUEST
      {
        public static LocString NAME = (LocString) "Requested Flow Rate: {QueuedMaxFlow}";
        public static LocString TOOLTIP = (LocString) "Waiting for a Duplicant to adjust flow rate";
      }

      public class EMITTINGLIGHT
      {
        public static LocString NAME = (LocString) "Emitting Light";
        public static LocString TOOLTIP = (LocString) ("Open the " + UI.FormatAsOverlay("Light Overlay", "[{LightGridOverlay}]") + " to view this light's visibility radius");
      }

      public class RATIONBOXCONTENTS
      {
        public static LocString NAME = (LocString) "Storing: {Stored}";
        public static LocString TOOLTIP = (LocString) ("This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD);
      }

      public class EMITTINGELEMENT
      {
        public static LocString NAME = (LocString) "Emitting {ElementType}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class EMITTINGCO2
      {
        public static LocString NAME = (LocString) "Emitting CO<sub>2</sub>: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("Producing " + (string) ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class EMITTINGOXYGENAVG
      {
        public static LocString NAME = (LocString) ("Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}");
        public static LocString TOOLTIP = (LocString) ("Producing " + (string) ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class EMITTINGGASAVG
      {
        public static LocString NAME = (LocString) "Emitting {Element}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class PUMPINGLIQUIDORGAS
      {
        public static LocString NAME = (LocString) "Average Flow Rate: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class WIRECIRCUITSTATUS
      {
        public static LocString NAME = (LocString) "Current Load: <color=#{Color}>{CurrentLoad}</color> / {MaxLoad}";
        public static LocString TOOLTIP = (LocString) ("The current " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break");
      }

      public class WIREMAXWATTAGESTATUS
      {
        public static LocString NAME = (LocString) "Potential Load: <color=#{Color}>{TotalPotentialLoad}</color> / {MaxLoad}";
        public static LocString TOOLTIP = (LocString) ("How much wattage this network will draw if all " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " consumers on the network become active at once");
      }

      public class NOLIQUIDELEMENTTOPUMP
      {
        public static LocString NAME = (LocString) "Pump Not In Liquid";
        public static LocString TOOLTIP = (LocString) ("This pump must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " to work");
      }

      public class NOGASELEMENTTOPUMP
      {
        public static LocString NAME = (LocString) "Pump Not In Gas";
        public static LocString TOOLTIP = (LocString) ("This pump must be submerged in " + UI.PRE_KEYWORD + "Gas" + UI.PST_KEYWORD + " to work");
      }

      public class PIPEMAYMELT
      {
        public static LocString NAME = (LocString) "High Melt Risk";
        public static LocString TOOLTIP = (LocString) ("This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD);
      }

      public class ELEMENTEMITTEROUTPUT
      {
        public static LocString NAME = (LocString) "Emitting {ElementTypes}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class ELEMENTCONSUMER
      {
        public static LocString NAME = (LocString) "Consuming {ElementTypes}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) "This object is utilizing ambient {ElementTypes} from the environment";
      }

      public class SPACECRAFTREADYTOLAND
      {
        public static LocString NAME = (LocString) "Spacecraft ready to land";
        public static LocString TOOLTIP = (LocString) "A spacecraft is ready to land";
        public static LocString NOTIFICATION = (LocString) "Ready to land";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) "A spacecraft is ready to land";
      }

      public class CONSUMINGFROMSTORAGE
      {
        public static LocString NAME = (LocString) "Consuming {ElementTypes}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) "This building is consuming {ElementTypes} from storage";
      }

      public class ELEMENTCONVERTEROUTPUT
      {
        public static LocString NAME = (LocString) "Emitting {ElementTypes}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}"));
      }

      public class ELEMENTCONVERTERINPUT
      {
        public static LocString NAME = (LocString) "Using {ElementTypes}: {FlowRate}";
        public static LocString TOOLTIP = (LocString) ("This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}"));
      }

      public class AWAITINGCOMPOSTFLIP
      {
        public static LocString NAME = (LocString) "Requires Flipping";
        public static LocString TOOLTIP = (LocString) ("Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT"));
      }

      public class AWAITINGWASTE
      {
        public static LocString NAME = (LocString) "Awaiting Compostables";
        public static LocString TOOLTIP = (LocString) "More waste material is required to begin the composting process";
      }

      public class BATTERIESSUFFICIENTLYFULL
      {
        public static LocString NAME = (LocString) "Batteries Sufficiently Full";
        public static LocString TOOLTIP = (LocString) "All batteries are above the refill threshold";
      }

      public class NEEDRESOURCEMASS
      {
        public static LocString NAME = (LocString) "Insufficient Resources\n{ResourcesRequired}";
        public static LocString TOOLTIP = (LocString) "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";
        public static LocString LINE_ITEM = (LocString) "• <b>{0}</b>";
      }

      public class JOULESAVAILABLE
      {
        public static LocString NAME = (LocString) "Power Available: {JoulesAvailable} / {JoulesCapacity}";
        public static LocString TOOLTIP = (LocString) ("<b>{JoulesAvailable}</b> of stored " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " available for use");
      }

      public class WATTAGE
      {
        public static LocString NAME = (LocString) "Wattage: {Wattage}";
        public static LocString TOOLTIP = (LocString) ("This building is generating " + UI.FormatAsPositiveRate("{Wattage}") + " of " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD);
      }

      public class SOLARPANELWATTAGE
      {
        public static LocString NAME = (LocString) "Current Wattage: {Wattage}";
        public static LocString TOOLTIP = (LocString) ("This panel is generating " + UI.FormatAsPositiveRate("{Wattage}") + " of " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD);
      }

      public class WATTSON
      {
        public static LocString NAME = (LocString) "Next Print: {TimeRemaining}";
        public static LocString TOOLTIP = (LocString) "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";
        public static LocString UNAVAILABLE = (LocString) nameof (UNAVAILABLE);
      }

      public class FLUSHTOILET
      {
        public static LocString NAME = (LocString) "Lavatory Ready";
        public static LocString TOOLTIP = (LocString) "This bathroom is ready to receive visitors";
      }

      public class FLUSHTOILETINUSE
      {
        public static LocString NAME = (LocString) "Lavatory In Use";
        public static LocString TOOLTIP = (LocString) "This bathroom is occupied";
      }

      public class WIRECONNECTED
      {
        public static LocString NAME = (LocString) "Wire Connected";
        public static LocString TOOLTIP = (LocString) "This wire is connected to a network";
      }

      public class WIRENOMINAL
      {
        public static LocString NAME = (LocString) "Wire Nominal";
        public static LocString TOOLTIP = (LocString) "This wire is in good condition";
      }

      public class WIREDISCONNECTED
      {
        public static LocString NAME = (LocString) "Wire Disconnected";
        public static LocString TOOLTIP = (LocString) ("This wire is not connecting a " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " consumer to a " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " generator");
      }

      public class COOLING
      {
        public static LocString NAME = (LocString) "Cooling";
        public static LocString TOOLTIP = (LocString) "This building is cooling the surrounding area";
      }

      public class COOLINGSTALLEDHOTENV
      {
        public static LocString NAME = (LocString) "Gas Too Hot";
        public static LocString TOOLTIP = (LocString) "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
      }

      public class COOLINGSTALLEDCOLDGAS
      {
        public static LocString NAME = (LocString) "Gas Too Cold";
        public static LocString TOOLTIP = (LocString) "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
      }

      public class COOLINGSTALLEDHOTLIQUID
      {
        public static LocString NAME = (LocString) "Liquid Too Hot";
        public static LocString TOOLTIP = (LocString) "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
      }

      public class COOLINGSTALLEDCOLDLIQUID
      {
        public static LocString NAME = (LocString) "Liquid Too Cold";
        public static LocString TOOLTIP = (LocString) "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
      }

      public class CANNOTCOOLFURTHER
      {
        public static LocString NAME = (LocString) "Minimum Temperature Reached";
        public static LocString TOOLTIP = (LocString) "This building cannot cool the surrounding environment below <b>{0}</b>";
      }

      public class HEATINGSTALLEDHOTENV
      {
        public static LocString NAME = (LocString) "Target Temperature Reached";
        public static LocString TOOLTIP = (LocString) "This building cannot heat the surrounding environment beyond <b>{0}</b>";
      }

      public class HEATINGSTALLEDLOWMASS_GAS
      {
        public static LocString NAME = (LocString) "Low Air Pressure";
        public static LocString TOOLTIP = (LocString) "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to heat up";
      }

      public class HEATINGSTALLEDLOWMASS_LIQUID
      {
        public static LocString NAME = (LocString) "Not Submerged In Liquid";
        public static LocString TOOLTIP = (LocString) ("This building must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD + " to function");
      }

      public class BUILDINGDISABLED
      {
        public static LocString NAME = (LocString) "Building Disabled";
        public static LocString TOOLTIP = (LocString) ("Press " + UI.PRE_KEYWORD + "Enable Building" + UI.PST_KEYWORD + " " + UI.FormatAsHotkey("[ENTER]") + " to resume use");
      }

      public class WORKING
      {
        public static LocString NAME = (LocString) "Nominal";
        public static LocString TOOLTIP = (LocString) "This building is working as intended";
      }

      public class GRAVEEMPTY
      {
        public static LocString NAME = (LocString) "Empty";
        public static LocString TOOLTIP = (LocString) "This memorial honors no one.";
      }

      public class GRAVE
      {
        public static LocString NAME = (LocString) "RIP {DeadDupe}";
        public static LocString TOOLTIP = (LocString) "{Epitaph}";
      }

      public class AWAITINGARTING
      {
        public static LocString NAME = (LocString) "Incomplete Artwork";
        public static LocString TOOLTIP = (LocString) "This building requires a Duplicant's artistic touch";
      }

      public class LOOKINGUGLY
      {
        public static LocString NAME = (LocString) "Crude";
        public static LocString TOOLTIP = (LocString) "Honestly, Morbs could've done better";
      }

      public class LOOKINGOKAY
      {
        public static LocString NAME = (LocString) "Quaint";
        public static LocString TOOLTIP = (LocString) "Duplicants find this art piece quite charming";
      }

      public class LOOKINGGREAT
      {
        public static LocString NAME = (LocString) "Masterpiece";
        public static LocString TOOLTIP = (LocString) "This poignant piece stirs something deep within each Duplicant's soul";
      }

      public class EXPIRED
      {
        public static LocString NAME = (LocString) "Depleted";
        public static LocString TOOLTIP = (LocString) "This building has no more use";
      }

      public class EXCAVATOR_BOMB
      {
        public class UNARMED
        {
          public static LocString NAME = (LocString) "Unarmed";
          public static LocString TOOLTIP = (LocString) "This explosive is currently inactive";
        }

        public class ARMED
        {
          public static LocString NAME = (LocString) "Armed";
          public static LocString TOOLTIP = (LocString) "Stand back, this baby's ready to blow!";
        }

        public class COUNTDOWN
        {
          public static LocString NAME = (LocString) "Countdown: {0}";
          public static LocString TOOLTIP = (LocString) "<b>{0}</b> seconds until detonation";
        }

        public class DUPE_DANGER
        {
          public static LocString NAME = (LocString) "Duplicant Preservation Override";
          public static LocString TOOLTIP = (LocString) "Explosive disabled due to close Duplicant proximity";
        }

        public class EXPLODING
        {
          public static LocString NAME = (LocString) "Exploding";
          public static LocString TOOLTIP = (LocString) "Kaboom!";
        }
      }

      public class BURNER
      {
        public class BURNING_FUEL
        {
          public static LocString NAME = (LocString) "Consuming Fuel: {0}";
          public static LocString TOOLTIP = (LocString) "<b>{0}</b> fuel remaining";
        }

        public class HAS_FUEL
        {
          public static LocString NAME = (LocString) "Fueled: {0}";
          public static LocString TOOLTIP = (LocString) "<b>{0}</b> fuel remaining";
        }
      }

      public class CREATURE_TRAP
      {
        public class NEEDSBAIT
        {
          public static LocString NAME = (LocString) "Needs Bait";
          public static LocString TOOLTIP = (LocString) "This trap needs to be baited before it can be set";
        }

        public class READY
        {
          public static LocString NAME = (LocString) "Set";
          public static LocString TOOLTIP = (LocString) ("This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD);
        }

        public class SPRUNG
        {
          public static LocString NAME = (LocString) "Sprung";
          public static LocString TOOLTIP = (LocString) "This trap has caught a {0}!";
        }
      }

      public class ACCESS_CONTROL
      {
        public class ACTIVE
        {
          public static LocString NAME = (LocString) "Access Restrictions";
          public static LocString TOOLTIP = (LocString) ("Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD);
        }

        public class OFFLINE
        {
          public static LocString NAME = (LocString) "Access Control Offline";
          public static LocString TOOLTIP = (LocString) ("This door has granted Emergency " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD + UI.HORIZONTAL_BR_RULE + "All Duplicants are permitted to pass through it until " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " is restored");
        }
      }

      public class REQUIRESSKILLPERK
      {
        public static LocString NAME = (LocString) "Skill-Required Operation";
        public static LocString TOOLTIP = (LocString) ("Only Duplicants with one of the following " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD + " can operate this building:\n{Skills}");
      }

      public class DIGREQUIRESSKILLPERK
      {
        public static LocString NAME = (LocString) "Skill-Required Dig";
        public static LocString TOOLTIP = (LocString) ("Only Duplicants with one of the following " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD + " can mine this material:\n{Skills}");
      }

      public class COLONYLACKSREQUIREDSKILLPERK
      {
        public static LocString NAME = (LocString) "Colony Lacks {Skills} Skill";
        public static LocString TOOLTIP = (LocString) ("Open the " + UI.FormatAsManagementMenu("Skills Panel", "[L]") + " and teach a Duplicant the {Skills} Skill to use this");
      }

      public class SWITCHSTATUSACTIVE
      {
        public static LocString NAME = (LocString) "Active";
        public static LocString TOOLTIP = (LocString) "This switch is currently toggled <b>On</b>";
      }

      public class SWITCHSTATUSINACTIVE
      {
        public static LocString NAME = (LocString) "Inactive";
        public static LocString TOOLTIP = (LocString) "This switch is currently toggled <b>Off</b>";
      }

      public class LOGICSWITCHSTATUSACTIVE
      {
        public static LocString NAME = (LocString) ("Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active));
        public static LocString TOOLTIP = (LocString) ("This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active));
      }

      public class LOGICSWITCHSTATUSINACTIVE
      {
        public static LocString NAME = (LocString) ("Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
        public static LocString TOOLTIP = (LocString) ("This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICSENSORSTATUSACTIVE
      {
        public static LocString NAME = (LocString) ("Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active));
        public static LocString TOOLTIP = (LocString) ("This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active));
      }

      public class LOGICSENSORSTATUSINACTIVE
      {
        public static LocString NAME = (LocString) ("Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
        public static LocString TOOLTIP = (LocString) ("This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class FOOD_CONTAINERS_OUTSIDE_RANGE
      {
        public static LocString NAME = (LocString) "Unreachable food";
        public static LocString TOOLTIP = (LocString) ("Recuperating Duplicants must have " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD + " available within <b>{0}</b> cells");
      }

      public class TOILETS_OUTSIDE_RANGE
      {
        public static LocString NAME = (LocString) "Unreachable restroom";
        public static LocString TOOLTIP = (LocString) ("Recuperating Duplicants must have " + UI.PRE_KEYWORD + "Toilets" + UI.PST_KEYWORD + " available within <b>{0}</b> cells");
      }

      public class BUILDING_DEPRECATED
      {
        public static LocString NAME = (LocString) "Building Deprecated";
        public static LocString TOOLTIP = (LocString) "This building is from an older version of the game and its use is not intended";
      }

      public class TURBINE_BLOCKED_INPUT
      {
        public static LocString NAME = (LocString) "All Inputs Blocked";
        public static LocString TOOLTIP = (LocString) ("This turbine's " + UI.PRE_KEYWORD + "Input Vents" + UI.PST_KEYWORD + " are blocked, so it can't intake any " + (string) ELEMENTS.STEAM.NAME + ".\n\nThe " + UI.PRE_KEYWORD + "Input Vents" + UI.PST_KEYWORD + " are located directly below the foundation " + UI.PRE_KEYWORD + "Tile" + UI.PST_KEYWORD + " this building is resting on.");
      }

      public class TURBINE_PARTIALLY_BLOCKED_INPUT
      {
        public static LocString NAME = (LocString) "{Blocked}/{Total} Inputs Blocked";
        public static LocString TOOLTIP = (LocString) "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput.";
      }

      public class TURBINE_TOO_HOT
      {
        public static LocString NAME = (LocString) "Turbine Too Hot";
        public static LocString TOOLTIP = (LocString) "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
      }

      public class TURBINE_BLOCKED_OUTPUT
      {
        public static LocString NAME = (LocString) "Output Blocked";
        public static LocString TOOLTIP = (LocString) ("A blocked " + UI.PRE_KEYWORD + "Output" + UI.PST_KEYWORD + " has stopped this turbine from functioning");
      }

      public class TURBINE_INSUFFICIENT_MASS
      {
        public static LocString NAME = (LocString) "Not Enough {Src_Element}";
        public static LocString TOOLTIP = (LocString) "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
      }

      public class TURBINE_INSUFFICIENT_TEMPERATURE
      {
        public static LocString NAME = (LocString) "{Src_Element} Temperature Below {Active_Temperature}";
        public static LocString TOOLTIP = (LocString) "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
      }

      public class TURBINE_ACTIVE_WATTAGE
      {
        public static LocString NAME = (LocString) "Current Wattage: {Wattage}";
        public static LocString TOOLTIP = (LocString) ("This turbine is generating " + UI.FormatAsPositiveRate("{Wattage}") + " of " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + "\n\nIt is running at <b>{Efficiency}</b> of full capacity. Increase {Src_Element} " + UI.PRE_KEYWORD + "Mass" + UI.PST_KEYWORD + " and " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD + " to improve output.");
      }

      public class TURBINE_SPINNING_UP
      {
        public static LocString NAME = (LocString) "Spinning Up";
        public static LocString TOOLTIP = (LocString) "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
      }

      public class TURBINE_ACTIVE
      {
        public static LocString NAME = (LocString) "Active";
        public static LocString TOOLTIP = (LocString) "This turbine is running at <b>{0}RPM</b>";
      }

      public class WELL_PRESSURIZING
      {
        public static LocString NAME = (LocString) "Backpressure: {0}";
        public static LocString TOOLTIP = (LocString) "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
      }

      public class WELL_OVERPRESSURE
      {
        public static LocString NAME = (LocString) "Overpressure";
        public static LocString TOOLTIP = (LocString) "This well can no longer function due to excessive backpressure";
      }

      public class NOTINANYROOM
      {
        public static LocString NAME = (LocString) "Outside of room";
        public static LocString TOOLTIP = (LocString) ("This building must be built inside a " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD + " for full functionality\n\nOpen the " + UI.FormatAsOverlay("Room Overlay", "[F11]") + " to view full " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD + " status");
      }

      public class NOTINREQUIREDROOM
      {
        public static LocString NAME = (LocString) "Outside of {0}";
        public static LocString TOOLTIP = (LocString) ("This building must be built inside a {0} for full functionality\n\nOpen the " + UI.FormatAsOverlay("Room Overlay", "[F11]") + " to view full " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD + " status");
      }

      public class NOTINRECOMMENDEDROOM
      {
        public static LocString NAME = (LocString) "Outside of {0}";
        public static LocString TOOLTIP = (LocString) ("It is recommended to build this building inside a {0}\n\nOpen the " + UI.FormatAsOverlay("Room Overlay", "[F11]") + " to view full " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD + " status");
      }

      public class RELEASING_PRESSURE
      {
        public static LocString NAME = (LocString) "Releasing Pressure";
        public static LocString TOOLTIP = (LocString) "Pressure buildup is being safely released";
      }

      public class LOGIC_FEEDBACK_LOOP
      {
        public static LocString NAME = (LocString) "Feedback Loop";
        public static LocString TOOLTIP = (LocString) ("Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the " + UI.PRE_KEYWORD + "Output" + UI.PST_KEYWORD + " of an automated building connects back to its own " + UI.PRE_KEYWORD + "Input" + UI.PST_KEYWORD + " through the Automation grid");
      }

      public class ENOUGH_COOLANT
      {
        public static LocString NAME = (LocString) "Awaiting Coolant";
        public static LocString TOOLTIP = (LocString) "<b>{1}</b> of {0} must be present in storage to begin production";
      }

      public class ENOUGH_FUEL
      {
        public static LocString NAME = (LocString) "Awaiting Fuel";
        public static LocString TOOLTIP = (LocString) "<b>{1}</b> of {0} must be present in storage to begin production";
      }

      public class LOGIC
      {
        public static LocString LOGIC_CONTROLLED_ENABLED = (LocString) "Enabled by Automation Grid";
        public static LocString LOGIC_CONTROLLED_DISABLED = (LocString) "Disabled by Automation Grid";
      }

      public class GANTRY
      {
        public static LocString AUTOMATION_CONTROL = (LocString) "Automation Control: {0}";
        public static LocString MANUAL_CONTROL = (LocString) "Manual Control: {0}";
        public static LocString EXTENDED = (LocString) "Extended";
        public static LocString RETRACTED = (LocString) "Retracted";
      }

      public class OBJECTDISPENSER
      {
        public static LocString AUTOMATION_CONTROL = (LocString) "Automation Control: {0}";
        public static LocString MANUAL_CONTROL = (LocString) "Manual Control: {0}";
        public static LocString OPENED = (LocString) "Opened";
        public static LocString CLOSED = (LocString) "Closed";
      }

      public class TOO_COLD
      {
        public static LocString NAME = (LocString) "Too Cold";
        public static LocString TOOLTIP = (LocString) "Either this building or its surrounding environment is too cold to operate";
      }

      public class CHECKPOINT
      {
        public static LocString LOGIC_CONTROLLED_OPEN = (LocString) "Clearance: Permitted";
        public static LocString LOGIC_CONTROLLED_CLOSED = (LocString) "Clearance: Not Permitted";
        public static LocString LOGIC_CONTROLLED_DISCONNECTED = (LocString) "No Automation";

        public class TOOLTIPS
        {
          public static LocString LOGIC_CONTROLLED_OPEN = (LocString) ("Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing");
          public static LocString LOGIC_CONTROLLED_CLOSED = (LocString) ("Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass");
          public static LocString LOGIC_CONTROLLED_DISCONNECTED = (LocString) ("This Checkpoint has not been connected to an " + UI.PRE_KEYWORD + "Automation" + UI.PST_KEYWORD + " grid");
        }
      }

      public class AWAITINGFUEL
      {
        public static LocString NAME = (LocString) "Awaiting Fuel: {0}";
        public static LocString TOOLTIP = (LocString) "This building requires <b>{1}</b> of {0} to operate";
      }

      public class NOLOGICWIRECONNECTED
      {
        public static LocString NAME = (LocString) "No Automation Wire Connected";
        public static LocString TOOLTIP = (LocString) ("This building has not been connected to an " + UI.PRE_KEYWORD + "Automation" + UI.PST_KEYWORD + " grid");
      }

      public class NOTUBECONNECTED
      {
        public static LocString NAME = (LocString) "No Tube Connected";
        public static LocString TOOLTIP = (LocString) ("The first section of tube extending from a " + (string) BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward");
      }

      public class NOTUBEEXITS
      {
        public static LocString NAME = (LocString) "No Landing Available";
        public static LocString TOOLTIP = (LocString) "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
      }

      public class STOREDCHARGE
      {
        public static LocString NAME = (LocString) "Charge Available: {0}/{1}";
        public static LocString TOOLTIP = (LocString) ("This building has <b>{0}</b> of stored " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + "\n\nIt consumes " + UI.FormatAsNegativeRate("{2}") + " per use");
      }

      public class NEEDEGG
      {
        public static LocString NAME = (LocString) "No Egg Selected";
        public static LocString TOOLTIP = (LocString) ("Collect " + UI.PRE_KEYWORD + "Eggs" + UI.PST_KEYWORD + " from " + UI.FormatAsLink("Critters", "CREATURES") + " to incubate");
      }

      public class NOAVAILABLEEGG
      {
        public static LocString NAME = (LocString) "No Egg Available";
        public static LocString TOOLTIP = (LocString) ("The selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD + " is not currently available");
      }

      public class AWAITINGEGGDELIVERY
      {
        public static LocString NAME = (LocString) "Awaiting Delivery";
        public static LocString TOOLTIP = (LocString) ("Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD);
      }

      public class INCUBATORPROGRESS
      {
        public static LocString NAME = (LocString) "Incubating: {Percent}";
        public static LocString TOOLTIP = (LocString) ("This " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD + " incubating cozily" + UI.HORIZONTAL_BR_RULE + "It will hatch when " + UI.PRE_KEYWORD + "Incubation" + UI.PST_KEYWORD + " reaches <b>100%</b>");
      }

      public class DETECTORQUALITY
      {
        public static LocString NAME = (LocString) "Scan Quality: {Quality}";
        public static LocString TOOLTIP = (LocString) "This scanner dish is currently scanning at <b>{Quality}</b> effectiveness\n\nDecreased scan quality may be due to:\n    • Interference from nearby heavy machinery\n    • Rock or tile obstructing the dish's line of sight on space";
      }

      public class NETWORKQUALITY
      {
        public static LocString NAME = (LocString) "Scan Network Quality: {TotalQuality}";
        public static LocString TOOLTIP = (LocString) ("This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + (string) BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s and ensure they're each scanning effectively for the best detection results");
      }

      public class DETECTORSCANNING
      {
        public static LocString NAME = (LocString) "Scanning";
        public static LocString TOOLTIP = (LocString) "This scanner is currently scouring space for anything of interest";
      }

      public class INCOMINGMETEORS
      {
        public static LocString NAME = (LocString) "Incoming Object Detected";
        public static LocString TOOLTIP = (LocString) "Warning!\n\nHigh velocity objects on approach!";
      }

      public class SPACE_VISIBILITY_NONE
      {
        public static LocString NAME = (LocString) "No Line of Sight";
        public static LocString TOOLTIP = (LocString) ("This building has no view of space\n\nEnsure an unblocked view of the sky is available to collect " + UI.FormatAsManagementMenu("Starmap") + " data\n    • Visibility: <b>{VISIBILITY}</b>\n    • Scan Radius: <b>{RADIUS}</b> cells");
      }

      public class SPACE_VISIBILITY_REDUCED
      {
        public static LocString NAME = (LocString) "Reduced Visibility";
        public static LocString TOOLTIP = (LocString) ("This building has an inadequate or obscured view of space\n\nEnsure an unblocked view of the sky is available to collect " + UI.FormatAsManagementMenu("Starmap") + " data\n    • Visibility: <b>{VISIBILITY}</b>\n    • Scan Radius: <b>{RADIUS}</b> cells");
      }

      public class PATH_NOT_CLEAR
      {
        public static LocString NAME = (LocString) "Launch Path Blocked";
        public static LocString TOOLTIP = (LocString) "There are solid obstructions in this rocket's launch trajectory:\n    • {0}\n\nThis rocket requires a clear flight path for launch";
        public static LocString TILE_FORMAT = (LocString) "Solid {0}";
      }

      public class TOP_PRIORITY_CHORE
      {
        public static LocString NAME = (LocString) "Top Priority";
        public static LocString TOOLTIP = (LocString) ("This errand has been set to " + UI.PRE_KEYWORD + "Top Priority" + UI.PST_KEYWORD + UI.HORIZONTAL_BR_RULE + "The colony will be in " + UI.PRE_KEYWORD + "Yellow Alert" + UI.PST_KEYWORD + " until this task is completed");
        public static LocString NOTIFICATION_NAME = (LocString) "Yellow Alert";
        public static LocString NOTIFICATION_TOOLTIP = (LocString) ("The following errands have been set to " + UI.PRE_KEYWORD + "Top Priority" + UI.PST_KEYWORD + ":");
      }
    }

    public class DETAILS
    {
      public static LocString USE_COUNT = (LocString) "Uses: {0}";
      public static LocString USE_COUNT_TOOLTIP = (LocString) "This building has been used {0} times";
    }
  }
}
