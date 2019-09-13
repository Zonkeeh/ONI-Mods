// Decompiled with JetBrains decompiler
// Type: STRINGS.BUILDINGS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class BUILDINGS
  {
    public class PREFABS
    {
      public class HEADQUARTERSCOMPLETE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Printing Pod", "HEADQUARTERS");
        public static LocString UNIQUE_POPTEXT = (LocString) "Only one {0} allowed!";
      }

      public class AIRCONDITIONER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Regulator", nameof (AIRCONDITIONER));
        public static LocString DESC = (LocString) "A thermo regulator doesn't remove heat, but relocates it to a new area.";
        public static LocString EFFECT = (LocString) ("Cools the " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " piped through it, but outputs " + UI.FormatAsLink("Heat", "HEAT") + " in its immediate vicinity.");
      }

      public class ETHANOLDISTILLERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ethanol Distiller", nameof (ETHANOLDISTILLERY));
        public static LocString DESC = (LocString) ("Ethanol distillers convert " + (string) ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME + " into burnable " + (string) ELEMENTS.ETHANOL.NAME + " fuel.");
        public static LocString EFFECT = (LocString) ("Refines " + (string) ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME + " into " + UI.FormatAsLink("Ethanol", "ETHANOL") + ".");
      }

      public class ALGAEDISTILLERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Algae Distiller", nameof (ALGAEDISTILLERY));
        public static LocString DESC = (LocString) "Algae distillers convert disease-causing slime into algae for oxygen production.";
        public static LocString EFFECT = (LocString) ("Refines " + UI.FormatAsLink("Slime", "SLIMEMOLD") + " into " + UI.FormatAsLink("Algae", "ALGAE") + ".");
      }

      public class OXYLITEREFINERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oxylite Refinery", nameof (OXYLITEREFINERY));
        public static LocString DESC = (LocString) "Oxylite is a solid and easily transportable source of consumable oxygen.";
        public static LocString EFFECT = (LocString) ("Synthesizes " + UI.FormatAsLink("Oxylite", "OXYROCK") + " using " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and a small amount of " + UI.FormatAsLink("Gold", "GOLD") + ".");
      }

      public class FERTILIZERMAKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fertilizer Synthesizer", nameof (FERTILIZERMAKER));
        public static LocString DESC = (LocString) "Fertilizer synthesizers convert polluted dirt into fertilizer for domestic plants.";
        public static LocString EFFECT = (LocString) ("Uses " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " to produce " + UI.FormatAsLink("Fertilizer", "FERTILIZER") + ".");
      }

      public class ALGAEHABITAT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Algae Terrarium", nameof (ALGAEHABITAT));
        public static LocString DESC = (LocString) "Algae colony, Duplicant colony... we're more alike than we are different.";
        public static LocString EFFECT = (LocString) ("Consumes " + UI.FormatAsLink("Algae", "ALGAE") + " to produce " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and remove some " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + ".\n\nGains a 10% efficiency boost in direct " + UI.FormatAsLink("Light", "LIGHT") + ".");
        public static LocString SIDESCREEN_TITLE = (LocString) ("Empty " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " Threshold");
      }

      public class BATTERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Battery", nameof (BATTERY));
        public static LocString DESC = (LocString) "Batteries allow power from generators to be stored for later.";
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that " + UI.FormatAsLink("Power", "POWER") + " to buildings.\n\nLoses charge over time.");
        public static LocString CHARGE_LOSS = (LocString) "{Battery} charge loss";
      }

      public class FLYINGCREATUREBAIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Airborne Critter Bait", nameof (FLYINGCREATUREBAIT));
        public static LocString DESC = (LocString) "The type of critter attracted by critter bait depends on the construction material.";
        public static LocString EFFECT = (LocString) "Attracts one type of airborne critter.\n\nSingle use.";
      }

      public class AIRBORNECREATURELURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Airborne Critter Lure", nameof (AIRBORNECREATURELURE));
        public static LocString DESC = (LocString) "Lures can relocate Pufts or Shine Bugs to specific locations in my colony.";
        public static LocString EFFECT = (LocString) ("Attracts one type of airborne critter at a time.\n\nMust be baited with " + UI.FormatAsLink("Slime", "SLIMEMOLD") + " or " + UI.FormatAsLink("Phosphorite", "PHOSPHORITE") + ".");
      }

      public class BATTERYMEDIUM
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jumbo Battery", nameof (BATTERYMEDIUM));
        public static LocString DESC = (LocString) "Larger batteries hold more power and keep systems running longer before recharging.";
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that " + UI.FormatAsLink("Power", "POWER") + " to buildings.\n\nSlightly loses charge over time.");
      }

      public class BATTERYSMART
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Smart Battery", nameof (BATTERYSMART));
        public static LocString DESC = (LocString) ("Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.");
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that " + UI.FormatAsLink("Power", "POWER") + " to buildings.\n\nSends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time.");
        public static LocString LOGIC_PORT = (LocString) "Charge Parameters";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>Low Threshold</b> charged");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the battery is more than <b>High Threshold</b> charged, until <b>Low Threshold</b> is reached again");
        public static LocString ACTIVATE_TOOLTIP = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>{0}%</b> charged");
        public static LocString DEACTIVATE_TOOLTIP = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when battery is more than <b>{0}%</b> charged");
        public static LocString SIDESCREEN_TITLE = (LocString) "Logic Activation Parameters";
        public static LocString SIDESCREEN_ACTIVATE = (LocString) "Low Threshold:";
        public static LocString SIDESCREEN_DEACTIVATE = (LocString) "High Threshold:";
      }

      public class BED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Cot", nameof (BED));
        public static LocString DESC = (LocString) "Duplicants without a bed will develop sore backs from sleeping on the floor.";
        public static LocString EFFECT = (LocString) "Gives one Duplicant a place to sleep.\n\nDuplicants will automatically return to their cots to sleep at night.";
      }

      public class BOTTLEEMPTIER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Bottle Emptier", nameof (BOTTLEEMPTIER));
        public static LocString DESC = (LocString) "A bottle emptier's Element Filter can be used to designate areas for specific liquid storage.";
        public static LocString EFFECT = (LocString) ("Empties bottled " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " back into the world.");
      }

      public class BOTTLEEMPTIERGAS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Canister Emptier", nameof (BOTTLEEMPTIERGAS));
        public static LocString DESC = (LocString) "A canister emptier's Element Filter can designate areas for specific gas storage.";
        public static LocString EFFECT = (LocString) ("Empties " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " canisters back into the world.");
      }

      public class CARGOBAY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Cargo Bay", nameof (CARGOBAY));
        public static LocString DESC = (LocString) "Duplicants will fill cargo bays with any resources they find during space missions.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to store any " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.");
      }

      public class SPECIALCARGOBAY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Biological Cargo Bay", nameof (SPECIALCARGOBAY));
        public static LocString DESC = (LocString) "Biological cargo bays allow Duplicants to retrieve alien plants and wildlife from space.";
        public static LocString EFFECT = (LocString) "Allows Duplicants to store unusual or organic resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
      }

      public class COMMANDMODULE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Command Capsule", nameof (COMMANDMODULE));
        public static LocString DESC = (LocString) "At least one astronaut must be assigned to the command module to pilot a rocket.";
        public static LocString EFFECT = (LocString) ("Contains passenger seating for Duplicant " + UI.FormatAsLink("Astronauts", "ASTRONAUT") + ".\n\nA Command Capsule must be the last module installed at the top of a rocket");
        public static LocString LOGIC_PORT_READY = (LocString) "Rocket Checklist";
        public static LocString LOGIC_PORT_READY_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete");
        public static LocString LOGIC_PORT_READY_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
        public static LocString LOGIC_PORT_LAUNCH = (LocString) "Launch Rocket";
        public static LocString LOGIC_PORT_LAUNCH_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket");
        public static LocString LOGIC_PORT_LAUNCH_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command");
      }

      public class RESEARCHMODULE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Research Module", nameof (RESEARCHMODULE));
        public static LocString DESC = (LocString) "Data banks can be used at virtual planetariums to produce additional research.";
        public static LocString EFFECT = (LocString) ("Completes one " + UI.FormatAsLink("Research Task", "RESEARCH") + " per space mission.\n\nProduces a small Data Bank regardless of mission destination.\n\nGenerated " + UI.FormatAsLink("Research Points", "RESEARCH") + " become available upon the rocket's return.");
      }

      public class TOURISTMODULE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sight-Seeing Module", nameof (TOURISTMODULE));
        public static LocString DESC = (LocString) "An astronaut must accompany sight seeing Duplicants on rocket flights.";
        public static LocString EFFECT = (LocString) ("Allows one non-Astronaut Duplicant to visit space.\n\nSight-Seeing rocket flights decrease " + UI.FormatAsLink("Stress", "STRESS") + ".");
      }

      public class GANTRY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gantry", nameof (GANTRY));
        public static LocString DESC = (LocString) "A gantry can be built over rocket pieces where ladders and tile cannot.";
        public static LocString EFFECT = (LocString) "Provides scaffolding across rocket modules to allow Duplicant access.";
        public static LocString LOGIC_PORT = (LocString) "Extend/Retract";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("<b>Extends gantry</b> when a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " signal is received");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("<b>Retracts gantry</b> when a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " signal is received");
      }

      public class WATERCOOLER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Water Cooler", nameof (WATERCOOLER));
        public static LocString DESC = (LocString) "Chatting with friends improves Duplicants' moods and reduces their stress.";
        public static LocString EFFECT = (LocString) ("Provides a gathering place for Duplicants during Downtime.\n\nImproves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class ARCADEMACHINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Arcade Cabinet", nameof (ARCADEMACHINE));
        public static LocString DESC = (LocString) "Komet Kablam-O!\nFor up to two players.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to play video games on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class PHONOBOX
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jukebot", nameof (PHONOBOX));
        public static LocString DESC = (LocString) "Dancing helps Duplicants get their innermost feelings out.";
        public static LocString EFFECT = (LocString) ("Plays music for Duplicants to dance to on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class ESPRESSOMACHINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Espresso Machine", nameof (ESPRESSOMACHINE));
        public static LocString DESC = (LocString) "A shot of espresso helps Duplicants relax after a long day.";
        public static LocString EFFECT = (LocString) ("Provides refreshment for Duplicants on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class CHECKPOINT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Duplicant Checkpoint", nameof (CHECKPOINT));
        public static LocString DESC = (LocString) "Checkpoints can be connected to automated sensors to determine when it's safe to enter.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to pass when receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nPrevents Duplicants from passing when receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".");
        public static LocString LOGIC_PORT = (LocString) "Duplicant Stop/Go";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Duplicant passage");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Duplicant passage");
      }

      public class FIREPOLE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fire Pole", nameof (FIREPOLE));
        public static LocString DESC = (LocString) "Build these in addition to ladders for efficient upward and downward movement.";
        public static LocString EFFECT = (LocString) "Allows rapid Duplicant descent.\n\nSignificantly slows upward climbing.";
      }

      public class FLOORSWITCH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Weight Plate", nameof (FLOORSWITCH));
        public static LocString DESC = (LocString) "Weight plates can be used to turn on amenities only when Duplicants pass by.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when an object or Duplicant is placed atop of it.\n\nCannot be triggered by " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " or " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + ".");
        public static LocString LOGIC_PORT_DESC = (LocString) (UI.FormatAsLink("Active", "LOGIC") + "/" + UI.FormatAsLink("Inactive", "LOGIC"));
      }

      public class KILN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Kiln", nameof (KILN));
        public static LocString DESC = (LocString) "Kilns can also be used to refine coal into pure carbon.";
        public static LocString EFFECT = (LocString) ("Fires " + UI.FormatAsLink("Clay", "CLAY") + " to produce " + UI.FormatAsLink("Ceramic", "CERAMIC") + ".\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class LIQUIDFUELTANK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Fuel Tank", nameof (LIQUIDFUELTANK));
        public static LocString DESC = (LocString) "Storing additional fuel increases the distance a rocket can travel before returning.";
        public static LocString EFFECT = (LocString) ("Stores the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon.");
      }

      public class OXIDIZERTANK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solid Oxidizer Tank", nameof (OXIDIZERTANK));
        public static LocString DESC = (LocString) "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Oxylite", "OXYROCK") + " for burning rocket fuels.");
      }

      public class OXIDIZERTANKLIQUID
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Oxidizer Tank", "LIQUIDOXIDIZERTANK");
        public static LocString DESC = (LocString) "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN") + " for burning rocket fuels.");
      }

      public class LIQUIDCONDITIONER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Aquatuner", nameof (LIQUIDCONDITIONER));
        public static LocString DESC = (LocString) "A thermo aquatuner cools liquid and outputs the heat elsewhere.";
        public static LocString EFFECT = (LocString) ("Cools the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " piped through it, but outputs " + UI.FormatAsLink("Heat", "HEAT") + " in its immediate vicinity.");
      }

      public class LIQUIDCARGOBAY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Cargo Tank", nameof (LIQUIDCARGOBAY));
        public static LocString DESC = (LocString) "Duplicants will fill cargo tanks with whatever resources they find during space missions.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to store any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.");
      }

      public class LUXURYBED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Comfy Bed", nameof (LUXURYBED));
        public static LocString DESC = (LocString) "Duplicants prefer comfy beds to cots and gain more stamina from sleeping in them.";
        public static LocString EFFECT = (LocString) ("Provides a sleeping area for one Duplicant and restores additional " + UI.FormatAsLink("Stamina", "STAMINA") + ".\n\nDuplicants will automatically sleep in their assigned beds at night.");
      }

      public class MEDICALCOT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Triage Cot", nameof (MEDICALCOT));
        public static LocString DESC = (LocString) "Duplicants use triage cots to recover from physical injuries and receive aid from peers.";
        public static LocString EFFECT = (LocString) ("Accelerates " + UI.FormatAsLink("Health", "HEALTH") + " restoration and the healing of physical injuries.\n\nRevives incapacitated Duplicants.");
      }

      public class DOCTORSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sick Bay", nameof (DOCTORSTATION));
        public static LocString DESC = (LocString) "Sick bays can be placed in hospital rooms to decrease the likelihood of disease spreading.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to administer basic treatments to sick Duplicants.\n\nDuplicants must possess the Bedside Manner " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.");
      }

      public class ADVANCEDDOCTORSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Disease Clinic", nameof (ADVANCEDDOCTORSTATION));
        public static LocString DESC = (LocString) "Disease clinics require power, but treat more serious illnesses than sick bays alone.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to administer powerful treatments to sick Duplicants.\n\nDuplicants must possess the Advanced Medical Care " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.");
      }

      public class MASSAGETABLE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Massage Table", nameof (MASSAGETABLE));
        public static LocString DESC = (LocString) "Massage tables quickly reduce extreme stress, at the cost of power production.";
        public static LocString EFFECT = (LocString) ("Rapidly reduces " + UI.FormatAsLink("Stress", "STRESS") + " for the Duplicant user.\n\nDuplicants will automatically seek a massage table when " + UI.FormatAsLink("Stress", "STRESS") + " exceeds breaktime range.");
        public static LocString ACTIVATE_TOOLTIP = (LocString) "Duplicants must take a break when their stress reaches {0}";
        public static LocString DEACTIVATE_TOOLTIP = (LocString) "Breaktime ends when stress is reduced to {0}";
      }

      public class CEILINGLIGHT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ceiling Light", nameof (CEILINGLIGHT));
        public static LocString DESC = (LocString) "Light reduces Duplicant stress and is required to grow certain plants.";
        public static LocString EFFECT = (LocString) ("Provides " + UI.FormatAsLink("Light", "LIGHT") + " when " + UI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.");
      }

      public class AIRFILTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Deodorizer", nameof (AIRFILTER));
        public static LocString DESC = (LocString) "Oh! Citrus scented!";
        public static LocString EFFECT = (LocString) ("Uses " + UI.FormatAsLink("Sand", "SAND") + " to filter " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " from the air, reducing " + UI.FormatAsLink("Disease", "DISEASE") + " spread.");
      }

      public class CANVAS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Blank Canvas", nameof (CANVAS));
        public static LocString DESC = (LocString) "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";
        public static LocString EFFECT = (LocString) ("Increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be painted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "Crude Painting";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Painting";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Masterpiece";
      }

      public class CANVASWIDE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Landscape Canvas", nameof (CANVASWIDE));
        public static LocString DESC = (LocString) "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";
        public static LocString EFFECT = (LocString) ("Moderately increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be painted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "Crude Painting";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Painting";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Masterpiece";
      }

      public class CANVASTALL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Portrait Canvas", nameof (CANVASTALL));
        public static LocString DESC = (LocString) "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";
        public static LocString EFFECT = (LocString) ("Moderately increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be painted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "Crude Painting";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Painting";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Masterpiece";
      }

      public class CO2SCRUBBER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Carbon Skimmer", nameof (CO2SCRUBBER));
        public static LocString DESC = (LocString) "Skimmers remove large amounts of carbon dioxide, but produce no breathable air.";
        public static LocString EFFECT = (LocString) ("Uses " + UI.FormatAsLink("Water", "WATER") + " to filter " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " from the air.");
      }

      public class COMPOST
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Compost", nameof (COMPOST));
        public static LocString DESC = (LocString) "Composts safely deal with biological waste, producing fresh dirt.";
        public static LocString EFFECT = (LocString) ("Reduces " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND") + " and other compostables down into " + UI.FormatAsLink("Dirt", "DIRT") + ".");
      }

      public class COOKINGSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Electric Grill", nameof (COOKINGSTATION));
        public static LocString DESC = (LocString) "Proper cooking eliminates foodborne disease and produces tasty, stress-relieving meals.";
        public static LocString EFFECT = (LocString) ("Cooks a wide variety of improved " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class GOURMETCOOKINGSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Range", nameof (GOURMETCOOKINGSTATION));
        public static LocString DESC = (LocString) "Luxury meals increase Duplicants morale and prevents them from becoming stressed.";
        public static LocString EFFECT = (LocString) ("Cooks a wide variety of quality " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class DININGTABLE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mess Table", nameof (DININGTABLE));
        public static LocString DESC = (LocString) "Duplicants prefer to dine at a table, rather than eat off the floor.";
        public static LocString EFFECT = (LocString) "Gives one Duplicant a place to eat.\n\nDuplicants will automatically eat at their assigned table when hungry.";
      }

      public class DOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pneumatic Door", nameof (DOOR));
        public static LocString DESC = (LocString) "Door controls can be used to prevent Duplicants from entering restricted areas.";
        public static LocString EFFECT = (LocString) ("Encloses areas without blocking " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " or " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow.\n\nWild " + UI.FormatAsLink("Critters", "CRITTERS") + " cannot pass through doors.");
        public static LocString PRESSURE_SUIT_REQUIRED = (LocString) (UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " required {0}");
        public static LocString PRESSURE_SUIT_NOT_REQUIRED = (LocString) (UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " not required {0}");
        public static LocString ABOVE = (LocString) "above";
        public static LocString BELOW = (LocString) "below";
        public static LocString LEFT = (LocString) "on left";
        public static LocString RIGHT = (LocString) "on right";
        public static LocString LOGIC_OPEN = (LocString) "Open/Close";
        public static LocString LOGIC_OPEN_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Open");
        public static LocString LOGIC_OPEN_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Close and lock");

        public static class CONTROL_STATE
        {
          public class OPEN
          {
            public static LocString NAME = (LocString) "Open";
            public static LocString TOOLTIP = (LocString) "This door will remain open";
          }

          public class CLOSE
          {
            public static LocString NAME = (LocString) "Lock";
            public static LocString TOOLTIP = (LocString) "Nothing may pass through";
          }

          public class AUTO
          {
            public static LocString NAME = (LocString) "Auto";
            public static LocString TOOLTIP = (LocString) "Duplicants open and close this door as needed";
          }
        }
      }

      public class ELECTROLYZER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Electrolyzer", nameof (ELECTROLYZER));
        public static LocString DESC = (LocString) "Water goes in one end, life sustaining oxygen comes out the other.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + UI.FormatAsLink("Hydrogen", "HYDROGEN") + ".\n\nBecomes idle when the area reaches maximum pressure capacity.");
      }

      public class RUSTDEOXIDIZER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Rust Deoxidizer", nameof (RUSTDEOXIDIZER));
        public static LocString DESC = (LocString) "Rust and salt goes in, oxygen comes out.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Rust", "RUST") + " into " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + UI.FormatAsLink("Chlorine", "CHLORINE") + ".\n\nBecomes idle when the area reaches maximum pressure capacity.");
      }

      public class DESALINATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Desalinator", nameof (DESALINATOR));
        public static LocString DESC = (LocString) "Salt can be refined into table salt for a mealtime morale boost.";
        public static LocString EFFECT = (LocString) ("Removes " + UI.FormatAsLink("Salt", "SALT") + " from " + UI.FormatAsLink("Brine", "BRINE") + " or " + UI.FormatAsLink("Salt Water", "SALTWATER") + ", producing " + UI.FormatAsLink("Water", "WATER") + ".");
      }

      public class POWERTRANSFORMERSMALL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Power Transformer", nameof (POWERTRANSFORMERSMALL));
        public static LocString DESC = (LocString) ("Connect " + UI.FormatAsLink("Batteries", "BATTERY") + " on the large side to act as a valve and prevent " + UI.FormatAsLink("Wires", "WIRE") + " from drawing more than 1000 W and suffering overload damage.");
        public static LocString EFFECT = (LocString) ("Limits " + UI.FormatAsLink("Power", "POWER") + " flowing through the Transformer to 1000 W.");
      }

      public class POWERTRANSFORMER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Large Power Transformer", nameof (POWERTRANSFORMER));
        public static LocString DESC = (LocString) ("Connect " + UI.FormatAsLink("Batteries", "BATTERY") + " on the large side to act as a valve and prevent " + UI.FormatAsLink("Wires", "WIRE") + " from drawing more than 4 kW.");
        public static LocString EFFECT = (LocString) ("Limits " + UI.FormatAsLink("Power", "POWER") + " flowing through the Transformer to 4 kW.");
      }

      public class FLOORLAMP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Lamp", nameof (FLOORLAMP));
        public static LocString DESC = (LocString) "Any building's light emitting radius can be viewed in the light overlay.";
        public static LocString EFFECT = (LocString) ("Provides " + UI.FormatAsLink("Light", "LIGHT") + " when " + UI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.");
      }

      public class FLOWERVASE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Flower Pot", nameof (FLOWERVASE));
        public static LocString DESC = (LocString) "Flower pots allow decorative plants to be moved to new locations.";
        public static LocString EFFECT = (LocString) ("Houses a single " + UI.FormatAsLink("Plant", "PLANTS") + " when sown with a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class FLOWERVASEWALL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Wall Pot", nameof (FLOWERVASEWALL));
        public static LocString DESC = (LocString) "Placing a plant in a wall pot can add a spot of decor to otherwise bare walls.";
        public static LocString EFFECT = (LocString) ("Houses a single " + UI.FormatAsLink("Plant", "PLANTS") + " when sown with a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be hung from a wall.");
      }

      public class FLOWERVASEHANGING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hanging Pot", nameof (FLOWERVASEHANGING));
        public static LocString DESC = (LocString) "Hanging pots can add some decor to a room, without blocking buildings on the floor.";
        public static LocString EFFECT = (LocString) ("Houses a single " + UI.FormatAsLink("Plant", "PLANTS") + " when sown with a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be hung from a ceiling.");
      }

      public class FLOWERVASEHANGINGFANCY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Aero Pot", nameof (FLOWERVASEHANGINGFANCY));
        public static LocString DESC = (LocString) "Aero pots can be hung from the ceiling and have extremely high decor values.";
        public static LocString EFFECT = (LocString) ("Houses a single " + UI.FormatAsLink("Plant", "PLANTS") + " when sown with a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be hung from a ceiling.");
      }

      public class FLUSHTOILET
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Lavatory", nameof (FLUSHTOILET));
        public static LocString DESC = (LocString) "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";
        public static LocString EFFECT = (LocString) ("Gives Duplicants a place to relieve themselves.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".");
      }

      public class SHOWER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Shower", nameof (SHOWER));
        public static LocString DESC = (LocString) "Regularly showering will prevent Duplicants spreading germs to the things they touch.";
        public static LocString EFFECT = (LocString) ("Improves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + " and removes surface " + UI.FormatAsLink("Germs", "DISEASE") + ".");
      }

      public class CONDUIT
      {
        public class STATUS_ITEM
        {
          public static LocString NAME = (LocString) "Marked for Emptying";
          public static LocString TOOLTIP = (LocString) ("Awaiting a " + UI.FormatAsLink("Plumber", "PLUMBER") + " to clear this pipe");
        }
      }

      public class GASCARGOBAY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Cargo Canister", nameof (GASCARGOBAY));
        public static LocString DESC = (LocString) "Duplicants fill cargo canisters with any resources they find during space missions.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to store any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.");
      }

      public class GASCONDUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Pipe", nameof (GASCONDUIT));
        public static LocString DESC = (LocString) "Gas pipes are used to connect the inputs and outputs of ventilated buildings.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " between " + UI.FormatAsLink("Outputs", "GASPIPING") + " and " + UI.FormatAsLink("Intakes", "GASPIPING") + ".\n\nCan be run through wall and floor tile.");
      }

      public class GASCONDUITBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Bridge", nameof (GASCONDUITBRIDGE));
        public static LocString DESC = (LocString) "Separate pipe systems prevent mingled contents from causing building damage.";
        public static LocString EFFECT = (LocString) ("Runs one " + UI.FormatAsLink("Gas Pipe", "GASPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.");
      }

      public class GASCONDUITPREFERENTIALFLOW
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Priority Gas Flow", nameof (GASCONDUITPREFERENTIALFLOW));
        public static LocString DESC = (LocString) "Priority flows ensure important buildings are filled first when on a system with other buildings.";
        public static LocString EFFECT = (LocString) ("Diverts " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " to a secondary input when its primary input overflows.");
      }

      public class LIQUIDCONDUITPREFERENTIALFLOW
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Priority Liquid Flow", nameof (LIQUIDCONDUITPREFERENTIALFLOW));
        public static LocString DESC = (LocString) "Priority flows ensure important buildings are filled first when on a system with other buildings.";
        public static LocString EFFECT = (LocString) ("Diverts " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " to a secondary input when its primary input overflows.");
      }

      public class GASCONDUITOVERFLOW
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Overflow Valve", nameof (GASCONDUITOVERFLOW));
        public static LocString DESC = (LocString) "Overflow valves can be used to prioritize which buildings should receive precious resources first.";
        public static LocString EFFECT = (LocString) ("Fills a secondary" + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " output only when its primary output is blocked.");
      }

      public class LIQUIDCONDUITOVERFLOW
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Overflow Valve", nameof (LIQUIDCONDUITOVERFLOW));
        public static LocString DESC = (LocString) "Overflow valves can be used to prioritize which buildings should receive precious resources first.";
        public static LocString EFFECT = (LocString) ("Fills a secondary" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " output only when its primary output is blocked.");
      }

      public class GASFILTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Filter", nameof (GASFILTER));
        public static LocString DESC = (LocString) "All gases are sent into the building's output pipe, except the gas chosen for filtering.";
        public static LocString EFFECT = (LocString) ("Sieves one " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " from the air, sending it into a dedicated " + UI.FormatAsLink("Pipe", "GASPIPING") + ".");
        public static LocString STATUS_ITEM = (LocString) "Filters: {0}";
        public static LocString ELEMENT_NOT_SPECIFIED = (LocString) "Not Specified";
      }

      public class GASPERMEABLEMEMBRANE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Airflow Tile", nameof (GASPERMEABLEMEMBRANE));
        public static LocString DESC = (LocString) "Building with airflow tile promotes better gas circulation within a colony.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nBlocks " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " flow without obstructing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".");
      }

      public class GASPUMP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Pump", nameof (GASPUMP));
        public static LocString DESC = (LocString) "Piping a pump's output to a building's intake will send gas to that building.";
        public static LocString EFFECT = (LocString) ("Draws in " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " and runs it through " + UI.FormatAsLink("Pipes", "GASPIPING") + ".\n\nMust be immersed in " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".");
      }

      public class GASMINIPUMP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mini Gas Pump", nameof (GASMINIPUMP));
        public static LocString DESC = (LocString) "Mini pumps are useful for moving small quantities of gas with minimum power.";
        public static LocString EFFECT = (LocString) ("Draws in a small amount of " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " and runs it through " + UI.FormatAsLink("Pipes", "GASPIPING") + ".\n\nMust be immersed in " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".");
      }

      public class GASVALVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Valve", nameof (GASVALVE));
        public static LocString DESC = (LocString) "Valves control the amount of gas that moves through pipes, preventing waste.";
        public static LocString EFFECT = (LocString) ("Controls the " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " volume permitted through " + UI.FormatAsLink("Pipes", "GASPIPING") + ".");
      }

      public class GASLOGICVALVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Shutoff", nameof (GASLOGICVALVE));
        public static LocString DESC = (LocString) "Automated piping saves power and time by removing the need for Duplicant input.";
        public static LocString EFFECT = (LocString) ("Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow on or off.");
        public static LocString LOGIC_PORT = (LocString) "Open/Close";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow gas flow");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent gas flow");
      }

      public class GASVENT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Vent", nameof (GASVENT));
        public static LocString DESC = (LocString) "Vents are an exit point for gases from ventilation systems.";
        public static LocString EFFECT = (LocString) ("Releases " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " from " + UI.FormatAsLink("Gas Pipes", "GASPIPING") + ".");
      }

      public class GASVENTHIGHPRESSURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("High Pressure Gas Vent", nameof (GASVENTHIGHPRESSURE));
        public static LocString DESC = (LocString) "High pressure vents can expel gas into more highly pressurized environments.";
        public static LocString EFFECT = (LocString) ("Releases " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " from " + UI.FormatAsLink("Gas Pipes", "GASPIPING") + " into high pressure locations.");
      }

      public class GASBOTTLER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Canister Filler", nameof (GASBOTTLER));
        public static LocString DESC = (LocString) "Canisters allow Duplicants to manually deliver gases from place to place.";
        public static LocString EFFECT = (LocString) ("Automatically stores piped " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " into canisters for manual transport.");
      }

      public class GENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Coal Generator", nameof (GENERATOR));
        public static LocString DESC = (LocString) "Burning coal produces more energy than manual power, but emits heat and exhaust.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Coal", "CARBON") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nProduces " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + ".");
        public static LocString OVERPRODUCTION = (LocString) "{Generator} overproduction";
      }

      public class KEROSENEENGINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Petroleum Engine", nameof (KEROSENEENGINE));
        public static LocString DESC = (LocString) "Rockets can be used to send Duplicants into space and retrieve rare resources.";
        public static LocString EFFECT = (LocString) ("Burns " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.");
      }

      public class HYDROGENENGINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydrogen Engine", nameof (HYDROGENENGINE));
        public static LocString DESC = (LocString) "Hydrogen engines can propel rockets further than steam or petroleum engines.";
        public static LocString EFFECT = (LocString) ("Burns " + UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.");
      }

      public class GENERICFABRICATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Omniprinter", nameof (GENERICFABRICATOR));
        public static LocString DESC = (LocString) "Omniprinters are incapable of printing organic matter.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Raw Mineral", "RAWMINERAL") + " into unique materials and objects.");
      }

      public class GRAVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Tasteful Memorial", nameof (GRAVE));
        public static LocString DESC = (LocString) "Burying dead Duplicants reduces health hazards and stress on the colony.";
        public static LocString EFFECT = (LocString) "Provides a final resting place for deceased Duplicants.\n\nLiving Duplicants will automatically place an unburied corpse inside.";
      }

      public class HEADQUARTERS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Printing Pod", nameof (HEADQUARTERS));
        public static LocString DESC = (LocString) "New Duplicants come out here, but thank goodness, they never go back in.";
        public static LocString EFFECT = (LocString) "An exceptionally advanced bioprinter of unknown origin.\n\nIt periodically produces new Duplicants or care packages containing resources.";
      }

      public class HYDROGENGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydrogen Generator", nameof (HYDROGENGENERATOR));
        public static LocString DESC = (LocString) "Hydrogen generators are extremely efficient, emitting next to no waste.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Hydrogen", "HYDROGEN") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".");
      }

      public class METHANEGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Natural Gas Generator", nameof (METHANEGENERATOR));
        public static LocString DESC = (LocString) "Natural gas generators leak polluted water and are best built above a waste reservoir.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Natural Gas", "METHANE") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nProduces " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " and " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + ".");
      }

      public class NUCLEARREACTOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Nuclear Reactor", nameof (NUCLEARREACTOR));
        public static LocString DESC = (LocString) "Makes an absurd amount of heat";
        public static LocString EFFECT = (LocString) "Heat!";
      }

      public class WOODGASGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Wood Burner", nameof (WOODGASGENERATOR));
        public static LocString DESC = (LocString) "Wood burners are small and easy to maintain, but produce a fair amount of heat.";
        public static LocString EFFECT = (LocString) ("Burns " + UI.FormatAsLink("Lumber", "WOOD") + " to produce electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nProduces " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " and " + UI.FormatAsLink("Heat", "HEAT") + ".");
      }

      public class ETHANOLGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ethanol Generator", nameof (ETHANOLGENERATOR));
        public static LocString DESC = (LocString) "Ethanol generators require less Duplicant operation, but produce significant waste.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Ethanol", "ETHANOL") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nProduces " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " and " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + ".");
      }

      public class PETROLEUMGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Petroleum Generator", nameof (PETROLEUMGENERATOR));
        public static LocString DESC = (LocString) "Petroleum generators have a high energy output but produce a great deal of waste.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nProduces " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE") + " and " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + ".");
      }

      public class HYDROPONICFARM
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydroponic Farm", nameof (HYDROPONICFARM));
        public static LocString DESC = (LocString) "Hydroponic farms reduce Duplicant traffic by automating irrigating crops.";
        public static LocString EFFECT = (LocString) ("Grows one " + UI.FormatAsLink("Plant", "PLANTS") + " from a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nCan be used as floor tile and rotated before construction.\n\nMust be irrigated through " + UI.FormatAsLink("Liquid Piping", "LIQUIDPIPING") + ".");
      }

      public class INSULATEDGASCONDUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Insulated Gas Pipe", nameof (INSULATEDGASCONDUIT));
        public static LocString DESC = (LocString) "Pipe insulation prevents gas contents from significantly changing temperature in transit.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " with minimal change in " + UI.FormatAsLink("Temperature", "HEAT") + ".\n\nCan be run through wall and floor tile.");
      }

      public class GASCONDUITRADIANT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Radiant Gas Pipe", nameof (GASCONDUITRADIANT));
        public static LocString DESC = (LocString) "Radiant pipes pumping cold gas can be run through hot areas to help cool them down.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", allowing extreme " + UI.FormatAsLink("Temperature", "HEAT") + " exchange with the surrounding environment.\n\nCan be run through wall and floor tile.");
      }

      public class INSULATEDLIQUIDCONDUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Insulated Liquid Pipe", nameof (INSULATEDLIQUIDCONDUIT));
        public static LocString DESC = (LocString) "Pipe insulation prevents liquid contents from significantly changing temperature in transit.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " with minimal change in " + UI.FormatAsLink("Temperature", "HEAT") + ".\n\nCan be run through wall and floor tile.");
      }

      public class LIQUIDCONDUITRADIANT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Radiant Liquid Pipe", nameof (LIQUIDCONDUITRADIANT));
        public static LocString DESC = (LocString) "Radiant pipes pumping cold liquid can be run through hot areas to help cool them down.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ", allowing extreme " + UI.FormatAsLink("Temperature", "HEAT") + " exchange with the surrounding environment.\n\nCan be run through wall and floor tile.");
      }

      public class INSULATEDWIRE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Insulated Wire", nameof (INSULATEDWIRE));
        public static LocString DESC = (LocString) "This stuff won't go melting if things get heated.";
        public static LocString EFFECT = (LocString) ("Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources in extreme " + UI.FormatAsLink("Heat", "HEAT") + ".\n\nCan be run through wall and floor tile.");
      }

      public class INSULATIONTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Insulated Tile", nameof (INSULATIONTILE));
        public static LocString DESC = (LocString) "The low thermal conductivity of insulated tiles slows any heat passing through them.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nReduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.");
      }

      public class EXTERIORWALL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Drywall", nameof (EXTERIORWALL));
        public static LocString DESC = (LocString) "Drywall can be used in conjunction with tiles to build airtight rooms on the surface.";
        public static LocString EFFECT = (LocString) ("Prevents " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " and " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " loss in space.\n\nBuilds an insulating backwall behind buildings.");
      }

      public class FARMTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Farm Tile", nameof (FARMTILE));
        public static LocString DESC = (LocString) "Duplicants can deliver fertilizer and liquids to farm tiles, accelerating plant growth.";
        public static LocString EFFECT = (LocString) ("Grows one " + UI.FormatAsLink("Plant", "PLANTS") + " from a " + UI.FormatAsLink("Seed", "PLANTS") + ".\n\nCan be used as floor tile and rotated before construction.");
      }

      public class LADDER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ladder", nameof (LADDER));
        public static LocString DESC = (LocString) "(That means they climb it.)";
        public static LocString EFFECT = (LocString) "Enables vertical mobility for Duplicants.";
      }

      public class LADDERFAST
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Plastic Ladder", nameof (LADDERFAST));
        public static LocString DESC = (LocString) "Plastic ladders are mildly antiseptic and can help limit the spread of germs in a colony.";
        public static LocString EFFECT = (LocString) "Increases Duplicant climbing speed.";
      }

      public class LIQUIDCONDUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Pipe", nameof (LIQUIDCONDUIT));
        public static LocString DESC = (LocString) "Liquid pipes are used to connect the inputs and outputs of plumbed buildings.";
        public static LocString EFFECT = (LocString) ("Carries " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " between " + UI.FormatAsLink("Outputs", "LIQUIDPIPING") + " and " + UI.FormatAsLink("Intakes", "LIQUIDPIPING") + ".\n\nCan be run through wall and floor tile.");
      }

      public class LIQUIDCONDUITBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Bridge", nameof (LIQUIDCONDUITBRIDGE));
        public static LocString DESC = (LocString) "Separate pipe systems help prevent building damage caused by mingled pipe contents.";
        public static LocString EFFECT = (LocString) ("Runs one " + UI.FormatAsLink("Liquid Pipe", "LIQUIDPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.");
      }

      public class ICECOOLEDFAN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ice-E Fan", nameof (ICECOOLEDFAN));
        public static LocString DESC = (LocString) "A Duplicant can work an Ice-E fan to temporarily cool small areas as needed.";
        public static LocString EFFECT = (LocString) ("Uses " + UI.FormatAsLink("Ice", "ICE") + " to dissipate a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".");
      }

      public class ICEMACHINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ice Maker", nameof (ICEMACHINE));
        public static LocString DESC = (LocString) "Ice makers can be used as a small renewable source of ice.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Water", "WATER") + " into " + UI.FormatAsLink("Ice", "ICE") + ".");
      }

      public class LIQUIDCOOLEDFAN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydrofan", nameof (LIQUIDCOOLEDFAN));
        public static LocString DESC = (LocString) "A Duplicant can work a hydrofan to temporarily cool small areas as needed.";
        public static LocString EFFECT = (LocString) ("Dissipates a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".");
      }

      public class CREATURETRAP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Critter Trap", nameof (CREATURETRAP));
        public static LocString DESC = (LocString) "Critter traps cannot catch swimming or flying critters.";
        public static LocString EFFECT = (LocString) ("Captures a living " + UI.FormatAsLink("Critter", "CRITTERS") + " for transport.\n\nSingle use.");
      }

      public class CREATUREDELIVERYPOINT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Critter Drop-Off", nameof (CREATUREDELIVERYPOINT));
        public static LocString DESC = (LocString) "Duplicants automatically bring captured critters to these relocation points for release.";
        public static LocString EFFECT = (LocString) ("Releases trapped " + UI.FormatAsLink("Critters", "CRITTERS") + " back into the world.\n\nCan be used multiple times.");
      }

      public class LIQUIDFILTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Filter", nameof (LIQUIDFILTER));
        public static LocString DESC = (LocString) "All liquids are sent into the building's output pipe, except the liquid chosen for filtering.";
        public static LocString EFFECT = (LocString) ("Sieves one " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " out of a mix, sending it into a dedicated " + UI.FormatAsLink("Filtered Output Pipe", "LIQUIDPIPING") + ".\n\nCan only filter one liquid type at a time.");
      }

      public class LIQUIDPUMP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Pump", nameof (LIQUIDPUMP));
        public static LocString DESC = (LocString) "Piping a pump's output to a building's intake will send liquid to that building.";
        public static LocString EFFECT = (LocString) ("Draws in " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " and runs it through " + UI.FormatAsLink("Pipes", "LIQUIDPIPING") + ".\n\nMust be submerged in " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".");
      }

      public class LIQUIDMINIPUMP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mini Liquid Pump", nameof (LIQUIDMINIPUMP));
        public static LocString DESC = (LocString) "Mini pumps are useful for moving small quantities of liquid with minimum power.";
        public static LocString EFFECT = (LocString) ("Draws in a small amount of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " and runs it through " + UI.FormatAsLink("Pipes", "LIQUIDPIPING") + ".\n\nMust be submerged in " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".");
      }

      public class LIQUIDPUMPINGSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pitcher Pump", nameof (LIQUIDPUMPINGSTATION));
        public static LocString DESC = (LocString) "Pitcher pumps allow Duplicants to bottle and deliver liquids from place to place.";
        public static LocString EFFECT = (LocString) ("Manually pumps " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " into bottles for transport.\n\nDuplicants can only carry liquids that are bottled.");
      }

      public class LIQUIDVALVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Valve", nameof (LIQUIDVALVE));
        public static LocString DESC = (LocString) "Valves control the amount of liquid that moves through pipes, preventing waste.";
        public static LocString EFFECT = (LocString) ("Controls the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " volume permitted through " + UI.FormatAsLink("Pipes", "LIQUIDPIPING") + ".");
      }

      public class LIQUIDLOGICVALVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Shutoff", nameof (LIQUIDLOGICVALVE));
        public static LocString DESC = (LocString) "Automated piping saves power and time by removing the need for Duplicant input.";
        public static LocString EFFECT = (LocString) ("Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " flow on or off.");
        public static LocString LOGIC_PORT = (LocString) "Open/Close";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Liquid flow");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Liquid flow");
      }

      public class LIQUIDVENT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Vent", nameof (LIQUIDVENT));
        public static LocString DESC = (LocString) "Vents are an exit point for liquids from plumbing systems.";
        public static LocString EFFECT = (LocString) ("Releases " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " from " + UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING") + ".");
      }

      public class MANUALGENERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Manual Generator", nameof (MANUALGENERATOR));
        public static LocString DESC = (LocString) "Watching Duplicants run on it is adorable... the electrical power is just an added bonus.";
        public static LocString EFFECT = (LocString) ("Converts manual labor into electrical " + UI.FormatAsLink("Power", "POWER") + ".");
      }

      public class MANUALPRESSUREDOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Manual Airlock", nameof (MANUALPRESSUREDOOR));
        public static LocString DESC = (LocString) "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";
        public static LocString EFFECT = (LocString) ("Blocks " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " and " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow, maintaining pressure between areas.\n\nWild " + UI.FormatAsLink("Critters", "CRITTERS") + " cannot pass through doors.");
      }

      public class MESHTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mesh Tile", nameof (MESHTILE));
        public static LocString DESC = (LocString) "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nDoes not obstruct " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " or " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow.");
      }

      public class PLASTICTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Plastic Tile", nameof (PLASTICTILE));
        public static LocString DESC = (LocString) "Plastic tile is mildly antiseptic and can help limit the spread of germs in a colony.";
        public static LocString EFFECT = (LocString) "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
      }

      public class GLASSTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Window Tile", nameof (GLASSTILE));
        public static LocString DESC = (LocString) "Window tiles provide a barrier against liquid and gas and are completely transparent.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nAllows " + UI.FormatAsLink("Light", "LIGHT") + " and " + UI.FormatAsLink("Decor Values", "DECOR") + " to pass through.");
      }

      public class METALTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Metal Tile", nameof (METALTILE));
        public static LocString DESC = (LocString) "Heat travels much more quickly through metal tile than other types of flooring.";
        public static LocString EFFECT = (LocString) "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
      }

      public class BUNKERTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Bunker Tile", nameof (BUNKERTILE));
        public static LocString DESC = (LocString) "Bunker tile can build strong shelters in otherwise dangerous environments.";
        public static LocString EFFECT = (LocString) "Used to build the walls and floors of rooms.\n\nCan withstand extreme pressures and impacts.";
      }

      public class CARPETTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Carpeted Tile", nameof (CARPETTILE));
        public static LocString DESC = (LocString) "Carpeted tile remains decorative even when other tile is stacked atop it.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class MOULDINGTILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Trimming Tile", "MOUDLINGTILE");
        public static LocString DESC = (LocString) "Trimming is used as purely decorative lining for walls and structures.";
        public static LocString EFFECT = (LocString) ("Used to build the walls and floors of rooms.\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class MONUMENTBOTTOM
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Monument Base", nameof (MONUMENTBOTTOM));
        public static LocString DESC = (LocString) "The base of a monument must be constructed first.";
        public static LocString EFFECT = (LocString) "Builds the bottom section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
      }

      public class MONUMENTMIDDLE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Monument Midsection", nameof (MONUMENTMIDDLE));
        public static LocString DESC = (LocString) "Customized sections of a Great Monument can be mixed and matched.";
        public static LocString EFFECT = (LocString) "Builds the middle section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
      }

      public class MONUMENTTOP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Monument Top", nameof (MONUMENTTOP));
        public static LocString DESC = (LocString) "Building a Great Monument will declare to the universe that this hunk of rock is your own.";
        public static LocString EFFECT = (LocString) "Builds the top section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
      }

      public class MICROBEMUSHER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Microbe Musher", nameof (MICROBEMUSHER));
        public static LocString DESC = (LocString) "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";
        public static LocString EFFECT = (LocString) ("Produces low quality " + UI.FormatAsLink("Food", "FOOD") + " using common ingredients.\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class MINERALDEOXIDIZER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen Diffuser", nameof (MINERALDEOXIDIZER));
        public static LocString DESC = (LocString) "Oxygen diffusers are inefficient, but output enough oxygen to keep a colony breathing.";
        public static LocString EFFECT = (LocString) ("Converts large amounts of " + UI.FormatAsLink("Algae", "ALGAE") + " into " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".\n\nBecomes idle when the area reaches maximum pressure capacity.");
      }

      public class ORESCRUBBER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ore Scrubber", nameof (ORESCRUBBER));
        public static LocString DESC = (LocString) "Scrubbers sanitize freshly mined materials before they're brought into the colony.";
        public static LocString EFFECT = (LocString) ("Kills a significant amount of " + UI.FormatAsLink("Germs", "DISEASE") + " present on " + UI.FormatAsLink("Raw Ore", "RAWMINERAL") + ".");
      }

      public class OUTHOUSE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Outhouse", nameof (OUTHOUSE));
        public static LocString DESC = (LocString) "The colony that eats together, excretes together.";
        public static LocString EFFECT = (LocString) ("Gives Duplicants a place to relieve themselves.\n\nRequires no " + UI.FormatAsLink("Piping", "LIQUIDPIPING") + ".\n\nMust be periodically emptied of " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND") + ".");
      }

      public class APOTHECARY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Apothecary", nameof (APOTHECARY));
        public static LocString DESC = (LocString) "Some medications help prevent diseases, while others aim to alleviate existing illness.";
        public static LocString EFFECT = (LocString) ("Produces " + UI.FormatAsLink("Medicine", "MEDICINE") + " to cure most basic " + UI.FormatAsLink("Diseases", "DISEASE") + ".\n\nDuplicants must possess the Medicine Compounding " + UI.FormatAsLink("Skill", "ROLES") + " to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class PLANTERBOX
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Planter Box", nameof (PLANTERBOX));
        public static LocString DESC = (LocString) "Domestically grown seeds mature more quickly than wild plants.";
        public static LocString EFFECT = (LocString) ("Grows one " + UI.FormatAsLink("Plant", "PLANTS") + " from a " + UI.FormatAsLink("Seed", "PLANTS") + ".");
      }

      public class PRESSUREDOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mechanized Airlock", nameof (PRESSUREDOOR));
        public static LocString DESC = (LocString) "Mechanized airlocks open and close more quickly than other types of door.";
        public static LocString EFFECT = (LocString) ("Blocks " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " and " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow, maintaining pressure between areas.\n\nFunctions as a " + UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR") + " when no " + UI.FormatAsLink("Power", "POWER") + " is available.\n\nWild " + UI.FormatAsLink("Critters", "CRITTERS") + " cannot pass through doors.");
      }

      public class BUNKERDOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Bunker Door", nameof (BUNKERDOOR));
        public static LocString DESC = (LocString) "A massive, slow-moving door which is nearly indestructible.";
        public static LocString EFFECT = (LocString) ("Blocks " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " and " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " flow, maintaining pressure between areas.\n\nCan withstand extremely high pressures and impacts.");
      }

      public class RATIONBOX
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ration Box", nameof (RATIONBOX));
        public static LocString DESC = (LocString) "Ration boxes keep food safe from hungry critters, but don't slow food spoilage.";
        public static LocString EFFECT = (LocString) ("Stores a small amount of " + UI.FormatAsLink("Food", "FOOD") + ".\n\nFood must be delivered to boxes by Duplicants.");
      }

      public class PARKSIGN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Park Sign", nameof (PARKSIGN));
        public static LocString DESC = (LocString) "Passing through parks will increase Duplicant Morale.";
        public static LocString EFFECT = (LocString) "Classifies an area as a Park or Nature Reserve.";
      }

      public class REFRIGERATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Refrigerator", nameof (REFRIGERATOR));
        public static LocString DESC = (LocString) "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";
        public static LocString EFFECT = (LocString) ("Stores " + UI.FormatAsLink("Food", "FOOD") + " at an ideal " + UI.FormatAsLink("Temperature", "HEAT") + " to prevent spoilage.");
        public static LocString LOGIC_PORT = (LocString) "Full/Not Full";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class ROLESTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Skills Board", nameof (ROLESTATION));
        public static LocString DESC = (LocString) "A skills board can teach special skills to Duplicants they can't learn on their own.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to spend Skill Points to learn new " + UI.FormatAsLink("Skills", "JOBS") + ".");
      }

      public class RESETSKILLSSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Skill Scrubber", nameof (RESETSKILLSSTATION));
        public static LocString DESC = (LocString) "Erase skills from a Duplicant's mind, returning them to their default abilities.";
        public static LocString EFFECT = (LocString) "Refunds a Duplicant's Skill Points for reassignment.\n\nDuplicants will lose all assigned skills in the process.";
      }

      public class RESEARCHCENTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Research Station", nameof (RESEARCHCENTER));
        public static LocString DESC = (LocString) "Research stations are necessary for unlocking all research tiers.";
        public static LocString EFFECT = (LocString) ("Conducts " + UI.FormatAsLink("Novice Research", "RESEARCH") + " to unlock new technologies.");
      }

      public class ADVANCEDRESEARCHCENTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Super Computer", nameof (ADVANCEDRESEARCHCENTER));
        public static LocString DESC = (LocString) "Super computers unlock higher technology tiers than research stations alone.";
        public static LocString EFFECT = (LocString) ("Conducts " + UI.FormatAsLink("Advanced Research", "RESEARCH") + " to unlock new technologies.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Super Computer Researching", "JUNIOR_RESEARCHER") + " trait.");
      }

      public class COSMICRESEARCHCENTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Virtual Planetarium", nameof (COSMICRESEARCHCENTER));
        public static LocString DESC = (LocString) "Planetariums allow the simulated exploration of locations discovered with a telescope.";
        public static LocString EFFECT = (LocString) ("Conducts " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " using data from " + UI.FormatAsLink("Telescopes", "TELESCOPE") + " and " + UI.FormatAsLink("Research Modules", "RESEARCHMODULE") + ".\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Planetarium Researching", "SENIOR_RESEARCHER") + " trait.");
      }

      public class TELESCOPE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Telescope", nameof (TELESCOPE));
        public static LocString DESC = (LocString) "Telescopes are necessary for learning starmaps and conducting rocket missions.";
        public static LocString EFFECT = (LocString) ("Maps Starmap destinations.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Geographical Analysis", "RESEARCHER") + " trait.\n\nBuilding must be exposed to space to function.");
        public static LocString REQUIREMENT_TOOLTIP = (LocString) "A steady {0} supply is required to sustain working Duplicants.";
      }

      public class SCULPTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Large Sculpting Block", nameof (SCULPTURE));
        public static LocString DESC = (LocString) "Duplicants who have learned art skills can produce more decorative sculptures.";
        public static LocString EFFECT = (LocString) ("Moderately increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "\"Abstract\" Sculpture";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Sculpture";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Genius Sculpture";
      }

      public class ICESCULPTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Ice Block", nameof (ICESCULPTURE));
        public static LocString DESC = (LocString) "Prone to melting.";
        public static LocString EFFECT = (LocString) ("Majorly increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "\"Abstract\" Ice Sculpture";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Ice Sculpture";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Genius Ice Sculpture";
      }

      public class MARBLESCULPTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Marble Block", nameof (MARBLESCULPTURE));
        public static LocString DESC = (LocString) "Duplicants who have learned art skills can produce more decorative sculptures.";
        public static LocString EFFECT = (LocString) ("Majorly increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "\"Abstract\" Marble Sculpture";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Marble Sculpture";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Genius Marble Sculpture";
      }

      public class METALSCULPTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Metal Block", nameof (METALSCULPTURE));
        public static LocString DESC = (LocString) "Duplicants who have learned art skills can produce more decorative sculptures.";
        public static LocString EFFECT = (LocString) ("Majorly increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "\"Abstract\" Metal Sculpture";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Metal Sculpture";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Genius Metal Sculpture";
      }

      public class SMALLSCULPTURE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sculpting Block", nameof (SMALLSCULPTURE));
        public static LocString DESC = (LocString) "Duplicants who have learned art skills can produce more decorative sculptures.";
        public static LocString EFFECT = (LocString) ("Minorly increases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.");
        public static LocString POORQUALITYNAME = (LocString) "\"Abstract\" Sculpture";
        public static LocString AVERAGEQUALITYNAME = (LocString) "Mediocre Sculpture";
        public static LocString EXCELLENTQUALITYNAME = (LocString) "Genius Sculpture";
      }

      public class SHEARINGSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Shearing Station", nameof (SHEARINGSTATION));
        public static LocString DESC = (LocString) ("Shearing stations allow " + UI.FormatAsLink("Dreckos", "DRECKO") + " to be safely sheared for useful raw materials.");
        public static LocString EFFECT = (LocString) "Allows the assigned Rancher to shear Dreckos.";
      }

      public class SUITMARKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Suit Checkpoint", nameof (SUITMARKER));
        public static LocString DESC = (LocString) "A checkpoint must have an atmo suit dock built on the opposite side its arrow faces.";
        public static LocString EFFECT = (LocString) ("Marks a threshold where Duplicants must change into or out of " + UI.FormatAsLink("Atmo Suits", "ATMO_SUIT") + ".\n\nMust be built next to an " + UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER") + ".\n\nCan be rotated before construction.");
      }

      public class SUITLOCKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Suit Dock", nameof (SUITLOCKER));
        public static LocString DESC = (LocString) "An atmo suit dock will empty atmo suits of waste, but only one suit can charge at a time.";
        public static LocString EFFECT = (LocString) ("Stores and recharges " + UI.FormatAsLink("Atmo Suits", "ATMO_SUIT") + ".\n\nBuild next to an " + UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER") + " to make Duplicants change into suits when passing by.");
      }

      public class JETSUITMARKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jet Suit Checkpoint", nameof (JETSUITMARKER));
        public static LocString DESC = (LocString) "A checkpoint must have a jet suit dock built on the opposite side its arrow faces.";
        public static LocString EFFECT = (LocString) ("Marks a threshold where Duplicants must change into or out of " + UI.FormatAsLink("Jet Suits", "JET_SUIT") + ".\n\nMust be built next to a " + UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER") + "\n\nCan be rotated before construction.");
      }

      public class JETSUITLOCKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jet Suit Dock", nameof (JETSUITLOCKER));
        public static LocString DESC = (LocString) "Jet Suit Docks can refill jet suits with air and fuel, or empty them of waste.";
        public static LocString EFFECT = (LocString) ("Stores and refuels " + UI.FormatAsLink("Jet Suits", "JET_SUIT") + " with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + UI.FormatAsLink("Petroleum", "PETROLEUM") + ".\n\nBuild next to a " + UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER") + " to make Duplicants change into suits when passing by.");
      }

      public class SUITFABRICATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Exosuit Forge", nameof (SUITFABRICATOR));
        public static LocString DESC = (LocString) "Exosuits can be filled with oxygen to allow Duplicants to safely enter hazardous areas.";
        public static LocString EFFECT = (LocString) ("Forges protective " + UI.FormatAsLink("Exosuits", "EXOSUIT") + " for Duplicants to wear.\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class CLOTHINGFABRICATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Textile Loom", nameof (CLOTHINGFABRICATOR));
        public static LocString DESC = (LocString) "A textile loom can be used to spin Reed Fiber into wearable Duplicant clothing.";
        public static LocString EFFECT = (LocString) ("Tailors Duplicant " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " items.\n\nDuplicants will not fabricate items unless recipes are queued.");
      }

      public class SOLIDBOOSTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solid Fuel Thruster", nameof (SOLIDBOOSTER));
        public static LocString DESC = (LocString) "Additional thrusters allow rockets to reach far away space destinations.";
        public static LocString EFFECT = (LocString) ("Burns " + UI.FormatAsLink("Refined Iron", "IRON") + " and " + UI.FormatAsLink("Oxylite", "OXYROCK") + " to increase rocket exploration distance.");
      }

      public class SPACEHEATER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Space Heater", nameof (SPACEHEATER));
        public static LocString DESC = (LocString) "A space heater will radiate heat for as long as it's powered.";
        public static LocString EFFECT = (LocString) ("Radiates a moderate amount of " + UI.FormatAsLink("Heat", "HEAT") + ".");
      }

      public class STORAGELOCKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Storage Bin", nameof (STORAGELOCKER));
        public static LocString DESC = (LocString) "Resources left on the floor become \"debris\" and lower decor when not put away.";
        public static LocString EFFECT = (LocString) "Stores the Solid resources of your choosing.";
      }

      public class STORAGELOCKERSMART
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Smart Storage Bin", nameof (STORAGELOCKERSMART));
        public static LocString DESC = (LocString) "Smart storage bins allow for the automation of resource organization based on type and mass.";
        public static LocString EFFECT = (LocString) ("Stores the Solid resources of your choosing.\n\nSends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when bin is full.");
        public static LocString LOGIC_PORT = (LocString) "Full/Not Full";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class OBJECTDISPENSER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Automatic Dispenser", nameof (OBJECTDISPENSER));
        public static LocString DESC = (LocString) "Automatic dispensers will store and drop resources in small quantities.";
        public static LocString EFFECT = (LocString) ("Stores any " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " delivered to it by Duplicants.\n\nDumps stored materials back into the world when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".");
        public static LocString LOGIC_PORT = (LocString) "Dump Trigger";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Dump all stored materials");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Store materials");
      }

      public class LIQUIDRESERVOIR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Reservoir", nameof (LIQUIDRESERVOIR));
        public static LocString DESC = (LocString) "Reservoirs cannot receive manually delivered resources.";
        public static LocString EFFECT = (LocString) ("Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.");
      }

      public class GASRESERVOIR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Reservoir", nameof (GASRESERVOIR));
        public static LocString DESC = (LocString) "Reservoirs cannot receive manually delivered resources.";
        public static LocString EFFECT = (LocString) ("Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.");
      }

      public class LIQUIDHEATER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Tepidizer", nameof (LIQUIDHEATER));
        public static LocString DESC = (LocString) "Tepidizers heat liquid which can kill waterborne germs.";
        public static LocString EFFECT = (LocString) ("Warms large bodies of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nMust be fully submerged.");
      }

      public class SWITCH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Switch", nameof (SWITCH));
        public static LocString DESC = (LocString) "Switches can only affect buildings that come after them on a circuit.";
        public static LocString EFFECT = (LocString) ("Turns " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.");
        public static LocString TURN_ON = (LocString) "Turn On";
        public static LocString TURN_ON_TOOLTIP = (LocString) "Turn On {Hotkey}";
        public static LocString TURN_OFF = (LocString) "Turn Off";
        public static LocString TURN_OFF_TOOLTIP = (LocString) "Turn Off {Hotkey}";
      }

      public class LOGICPOWERRELAY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Power Shutoff", nameof (LOGICPOWERRELAY));
        public static LocString DESC = (LocString) "Automated systems save power and time by removing the need for Duplicant input.";
        public static LocString EFFECT = (LocString) ("Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.");
        public static LocString LOGIC_PORT = (LocString) "Kill Power";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " through connected circuits");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD + " from flowing through connected circuits");
      }

      public class TEMPERATURECONTROLLEDSWITCH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Switch", nameof (TEMPERATURECONTROLLEDSWITCH));
        public static LocString DESC = (LocString) "Automated switches can be used to manage circuits in areas where Duplicants cannot enter.";
        public static LocString EFFECT = (LocString) ("Automatically turns " + UI.FormatAsLink("Power", "POWER") + " on or off using ambient " + UI.FormatAsLink("Temperature", "HEAT") + ".\n\nDoes not affect circuitry preceding the switch.");
      }

      public class PRESSURESWITCHLIQUID
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydro Switch", nameof (PRESSURESWITCHLIQUID));
        public static LocString DESC = (LocString) "A hydro switch shuts off power when the liquid pressure surrounding it surpasses the set threshold.";
        public static LocString EFFECT = (LocString) ("Automatically turns " + UI.FormatAsLink("Power", "POWER") + " on or off using ambient " + UI.FormatAsLink("Liquid Pressure", "PRESSURE") + ".\n\nDoes not affect circuitry preceding the switch.\n\nMust be submerged in " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".");
      }

      public class PRESSURESWITCHGAS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Switch", nameof (PRESSURESWITCHGAS));
        public static LocString DESC = (LocString) "An atmo switch shuts off power when the air pressure surrounding it surpasses the set threshold.";
        public static LocString EFFECT = (LocString) ("Automatically turns " + UI.FormatAsLink("Power", "POWER") + " on or off using ambient " + UI.FormatAsLink("Gas Pressure", "PRESSURE") + " .\n\nDoes not affect circuitry preceding the switch.");
      }

      public class TILE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Tile", nameof (TILE));
        public static LocString DESC = (LocString) "Tile can be used to bridge gaps and get to unreachable areas.";
        public static LocString EFFECT = (LocString) "Used to build the walls and floors of rooms.\n\nIncreases Duplicant runspeed.";
      }

      public class WATERPURIFIER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Water Sieve", nameof (WATERPURIFIER));
        public static LocString DESC = (LocString) "Sieves cannot kill germs and pass any they receive into their waste and water output.";
        public static LocString EFFECT = (LocString) ("Produces clean " + UI.FormatAsLink("Water", "WATER") + " from " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " using " + UI.FormatAsLink("Sand", "SAND") + ".\n\nProduces " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND") + ".");
      }

      public class DISTILLATIONCOLUMN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Distillation Column", nameof (DISTILLATIONCOLUMN));
        public static LocString DESC = (LocString) "Gets hot and steamy.";
        public static LocString EFFECT = (LocString) ("Separates any " + UI.FormatAsLink("Contaminated Water", "DIRTYWATER") + " piped through it into " + UI.FormatAsLink("Steam", "STEAM") + " and " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND") + ".");
      }

      public class WIRE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Wire", nameof (WIRE));
        public static LocString DESC = (LocString) "Electrical wire is used to connect generators, batteries, and buildings.";
        public static LocString EFFECT = (LocString) ("Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.");
      }

      public class WIREBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Wire Bridge", nameof (WIREBRIDGE));
        public static LocString DESC = (LocString) "Splitting generators onto separate grids can prevent overloads and wasted electricity.";
        public static LocString EFFECT = (LocString) "Runs one wire section over another without joining them.\n\nCan be run through wall and floor tile.";
      }

      public class HIGHWATTAGEWIRE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Heavi-Watt Wire", nameof (HIGHWATTAGEWIRE));
        public static LocString DESC = (LocString) "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";
        public static LocString EFFECT = (LocString) ("Carries more " + UI.FormatAsLink("Wattage", "POWER") + " than regular " + UI.FormatAsLink("Wire", "WIRE") + " without overloading.\n\nCannot be run through wall and floor tile.");
      }

      public class WIREBRIDGEHIGHWATTAGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Heavi-Watt Joint Plate", nameof (WIREBRIDGEHIGHWATTAGE));
        public static LocString DESC = (LocString) "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";
        public static LocString EFFECT = (LocString) ("Allows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.");
      }

      public class WIREREFINED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conductive Wire", nameof (WIREREFINED));
        public static LocString DESC = (LocString) "My Duplicants prefer the look of conductive wire to the regular raggedy stuff.";
        public static LocString EFFECT = (LocString) ("Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.");
      }

      public class WIREREFINEDBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conductive Wire Bridge", nameof (WIREREFINEDBRIDGE));
        public static LocString DESC = (LocString) "Splitting generators onto separate systems can prevent overloads and wasted electricity.";
        public static LocString EFFECT = (LocString) ("Carries more " + UI.FormatAsLink("Wattage", "POWER") + " than a regular " + UI.FormatAsLink("Wire Bridge", "WIREBRIDGE") + " without overloading.\n\nRuns one wire section over another without joining them.\n\nCan be run through wall and floor tile.");
      }

      public class WIREREFINEDHIGHWATTAGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Heavi-Watt Conductive Wire", nameof (WIREREFINEDHIGHWATTAGE));
        public static LocString DESC = (LocString) "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";
        public static LocString EFFECT = (LocString) ("Carries more " + UI.FormatAsLink("Wattage", "POWER") + " than regular " + UI.FormatAsLink("Wire", "WIRE") + " without overloading.\n\nCannot be run through wall and floor tile.");
      }

      public class WIREREFINEDBRIDGEHIGHWATTAGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Heavi-Watt Conductive Joint Plate", nameof (WIREREFINEDBRIDGEHIGHWATTAGE));
        public static LocString DESC = (LocString) "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";
        public static LocString EFFECT = (LocString) ("Carries more " + UI.FormatAsLink("Wattage", "POWER") + " than a regular " + UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE") + " without overloading.\n\nAllows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.");
      }

      public class HANDSANITIZER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hand Sanitizer", nameof (HANDSANITIZER));
        public static LocString DESC = (LocString) "Hand sanitizers kill germs more effectively than wash basins.";
        public static LocString EFFECT = (LocString) ("Removes most " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Hand Sanitizers when passing by in the selected direction.");
      }

      public class WASHBASIN
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Wash Basin", nameof (WASHBASIN));
        public static LocString DESC = (LocString) "Germ spread can be reduced by building these where Duplicants often get dirty.";
        public static LocString EFFECT = (LocString) ("Removes some " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Wash Basins when passing by in the selected direction.");
      }

      public class WASHSINK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Sink", nameof (WASHSINK));
        public static LocString DESC = (LocString) "Sinks are plumbed and do not need to be manually emptied or refilled.";
        public static LocString EFFECT = (LocString) ("Removes " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Sinks when passing by in the selected direction.");
      }

      public class TILEPOI
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Tile", nameof (TILEPOI));
        public static LocString DESC = (LocString) string.Empty;
        public static LocString EFFECT = (LocString) "Used to build the walls and floor of rooms.";
      }

      public class POLYMERIZER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Polymer Press", nameof (POLYMERIZER));
        public static LocString DESC = (LocString) "Plastic can be used to craft unique buildings and goods.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " into raw " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".");
      }

      public class DIRECTIONALWORLDPUMPLIQUID
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Channel", nameof (DIRECTIONALWORLDPUMPLIQUID));
        public static LocString DESC = (LocString) "Channels move more volume than pumps and require no power, but need sufficient pressure to function.";
        public static LocString EFFECT = (LocString) ("Directionally moves large volumes of " + UI.FormatAsLink("LIQUID", "ELEMENTS_LIQUID") + " through a channel.\n\nCan be used as floor tile and rotated before construction.");
      }

      public class STEAMTURBINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("[DEPRECATED] Steam Turbine", nameof (STEAMTURBINE));
        public static LocString DESC = (LocString) "Useful for converting the geothermal energy of magma into usable power.";
        public static LocString EFFECT = (LocString) ("THIS BUILDING HAS BEEN DEPRECATED AND CANNOT BE BUILT.\n\nGenerates exceptional electrical " + UI.FormatAsLink("Power", "POWER") + " using pressurized, " + UI.FormatAsLink("Scalding", "HEAT") + " " + UI.FormatAsLink("Steam", "STEAM") + ".\n\nOutputs significantly cooler " + UI.FormatAsLink("Steam", "STEAM") + " than it receives.\n\nAir pressure beneath this building must be higher than pressure above for air to flow.");
      }

      public class STEAMTURBINE2
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Steam Turbine", nameof (STEAMTURBINE2));
        public static LocString DESC = (LocString) "Useful for converting the geothermal energy into usable power.";
        public static LocString EFFECT = (LocString) ("Draws in " + UI.FormatAsLink("Steam", "STEAM") + " from the tiles directly below the machine's foundation and uses it to generate electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nOutputs " + UI.FormatAsLink("Water", "WATER") + ".");
        public static LocString HEAT_SOURCE = (LocString) "Power Generation Waste";
      }

      public class STEAMENGINE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Steam Engine", nameof (STEAMENGINE));
        public static LocString DESC = (LocString) "Rockets can be used to send Duplicants into space and retrieve rare resources.";
        public static LocString EFFECT = (LocString) ("Utilizes " + UI.FormatAsLink("Steam", "STEAM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.");
      }

      public class SOLARPANEL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Solar Panel", nameof (SOLARPANEL));
        public static LocString DESC = (LocString) "Solar panels convert high intensity sunlight into power and produce zero waste.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Sunlight", "LIGHT") + " into electrical " + UI.FormatAsLink("Power", "POWER") + ".\n\nMust be exposed to space.");
      }

      public class COMETDETECTOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Space Scanner", nameof (COMETDETECTOR));
        public static LocString DESC = (LocString) "Networks of many scanners will scan more efficiently than one on its own.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.");
      }

      public class OILREFINERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oil Refinery", nameof (OILREFINERY));
        public static LocString DESC = (LocString) "Petroleum can only be produced from the refinement of crude oil.";
        public static LocString EFFECT = (LocString) ("Converts " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + " into " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " and " + UI.FormatAsLink("Natural Gas", "METHANE") + ".");
      }

      public class OILWELLCAP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oil Well", nameof (OILWELLCAP));
        public static LocString DESC = (LocString) "Water pumped into an oil reservoir cannot be recovered.";
        public static LocString EFFECT = (LocString) ("Extracts " + UI.FormatAsLink("Crude Oil", "CRUDEOIL") + " using clean " + UI.FormatAsLink("Water", "WATER") + ".\n\nMust be built atop an " + UI.FormatAsLink("Oil Reservoir", "OIL_WELL") + ".");
      }

      public class METALREFINERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Metal Refinery", nameof (METALREFINERY));
        public static LocString DESC = (LocString) "Refined metals are necessary to build advanced electronics and technologies.";
        public static LocString EFFECT = (LocString) ("Produces " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + " from raw " + UI.FormatAsLink("Metal Ore", "RAWMETAL") + ".\n\nSignificantly " + UI.FormatAsLink("Heats", "HEAT") + " and outputs the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " piped into it.\n\nDuplicants will not fabricate items unless recipes are queued.");
        public static LocString RECIPE_DESCRIPTION = (LocString) "Extracts pure {0} from {1}.";
      }

      public class GLASSFORGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Glass Forge", nameof (GLASSFORGE));
        public static LocString DESC = (LocString) "Glass can be used to construct window tile.";
        public static LocString EFFECT = (LocString) ("Produces " + UI.FormatAsLink("Molten Glass", "MOLTENGLASS") + " from raw " + UI.FormatAsLink("Sand", "SAND") + ".\n\nOutputs " + UI.FormatAsLink("High Temperature", "HEAT") + " " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nDuplicants will not fabricate items unless recipes are queued.");
        public static LocString RECIPE_DESCRIPTION = (LocString) "Extracts pure {0} from {1}.";
      }

      public class ROCKCRUSHER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Rock Crusher", nameof (ROCKCRUSHER));
        public static LocString DESC = (LocString) "Rock Crushers loosen nuggets from raw ore and can process many different resources.";
        public static LocString EFFECT = (LocString) "Inefficiently produces refined materials from raw resources.\n\nDuplicants will not fabricate items unless recipes are queued.";
        public static LocString RECIPE_DESCRIPTION = (LocString) "Crushes {0} into {1}.";
        public static LocString METAL_RECIPE_DESCRIPTION = (LocString) ("Crushes {1} into " + UI.FormatAsLink("Sand", "SAND") + " and pure {0}.");
        public static LocString LIME_RECIPE_DESCRIPTION = (LocString) "Crushes {1} into {0}";
        public static LocString LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION = (LocString) "Crushes {0} into {1} and a small amount of pure {2}";
      }

      public class SUPERMATERIALREFINERY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Molecular Forge", nameof (SUPERMATERIALREFINERY));
        public static LocString DESC = (LocString) "Rare materials can be procured through rocket missions into space.";
        public static LocString EFFECT = (LocString) ("Processes " + UI.FormatAsLink("Rare Materials", "RAREMATERIALS") + " into advanced industrial goods.\n\nRare materials can be retrieved from space missions.\n\nDuplicants will not fabricate items unless recipes are queued.");
        public static LocString SUPERCOOLANT_RECIPE_DESCRIPTION = (LocString) ("Super Coolant is an industrial grade " + UI.FormatAsLink("Fullerene", "FULLERENE") + " coolant.");
        public static LocString SUPERINSULATOR_RECIPE_DESCRIPTION = (LocString) ("Insulation reduces " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and is composed of recrystallized " + UI.FormatAsLink("Abyssalite", "KATAIRITE") + ".");
        public static LocString TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION = (LocString) ("Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.");
        public static LocString VISCOGEL_RECIPE_DESCRIPTION = (LocString) ("Visco-Gel is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension.");
        public static LocString YELLOWCAKE_RECIPE_DESCRIPTION = (LocString) ("Yellowcake is a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " used in uranium enrichment.");
      }

      public class THERMALBLOCK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Tempshift Plate", nameof (THERMALBLOCK));
        public static LocString DESC = (LocString) "The thermal properties of construction materials determine their heat retention.";
        public static LocString EFFECT = (LocString) ("Accelerates or buffers " + UI.FormatAsLink("Heat", "HEAT") + " dispersal based on the construction material used.\n\nHas a small area of effect.");
      }

      public class POWERCONTROLSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Power Control Station", nameof (POWERCONTROLSTATION));
        public static LocString DESC = (LocString) "Only one Duplicant may be assigned to a station at a time.";
        public static LocString EFFECT = (LocString) ("Produces " + UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS") + " to increase the " + UI.FormatAsLink("Power", "POWER") + " output of generators.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Tune Up", "POWER_TECHNICIAN") + " trait.\n\nThis building is a necessary component of the Power Plant room.");
      }

      public class FARMSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Farm Station", nameof (FARMSTATION));
        public static LocString DESC = (LocString) "This station only has an effect on crops grown within the same room.";
        public static LocString EFFECT = (LocString) ("Produces " + UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS") + " to increase " + UI.FormatAsLink("Plant", "PLANTS") + " growth rates.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Crop Tending", "FARMER") + " trait.\n\nThis building is a necessary component of the Greenhouse room.");
      }

      public class FISHDELIVERYPOINT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fish Release", nameof (FISHDELIVERYPOINT));
        public static LocString DESC = (LocString) "A fish release must be built above liquid to prevent released fish from suffocating.";
        public static LocString EFFECT = (LocString) ("Releases trapped " + UI.FormatAsLink("Pacu", "PACU") + " back into the world.\n\nCan be used multiple times.");
      }

      public class FISHFEEDER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fish Feeder", nameof (FISHFEEDER));
        public static LocString DESC = (LocString) "Build this feeder above a body of water to feed the fish within.";
        public static LocString EFFECT = (LocString) ("Automatically dispenses stored " + UI.FormatAsLink("Critter", "CRITTERS") + " food into the area below.\n\nDispenses once per day.");
      }

      public class FISHTRAP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Fish Trap", nameof (FISHTRAP));
        public static LocString DESC = (LocString) "Trapped fish will automatically be bagged for transport.";
        public static LocString EFFECT = (LocString) ("Attracts and traps swimming " + UI.FormatAsLink("Pacu", "PACU") + ".\n\nSingle use.");
      }

      public class RANCHSTATION
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Grooming Station", nameof (RANCHSTATION));
        public static LocString DESC = (LocString) "Grooming critters make them look nice, smell pretty, feel happy, and produce more.";
        public static LocString EFFECT = (LocString) ("Allows the assigned " + UI.FormatAsLink("Rancher", "RANCHER") + " to care for " + UI.FormatAsLink("Critters", "CRITTERS") + ".\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Critter Wrangling", "RANCHER") + " trait.\n\nThis building is a necessary component of the Stable room.");
      }

      public class MACHINESHOP
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Mechanics Station", nameof (MACHINESHOP));
        public static LocString DESC = (LocString) "Duplicants will only improve the efficiency of buildings in the same room as this station.";
        public static LocString EFFECT = (LocString) ("Allows the assigned " + UI.FormatAsLink("Engineer", "MACHINE_TECHNICIAN") + " to improve building production efficiency.\n\nThis building is a necessary component of the Machine Shop room.");
      }

      public class LOGICWIRE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Automation Wire", nameof (LOGICWIRE));
        public static LocString DESC = (LocString) "Automation wire is used to connect building ports to automation gates.";
        public static LocString EFFECT = (LocString) ("Connects buildings to " + UI.FormatAsLink("Sensors", "LOGIC") + ".\n\nCan be run through wall and floor tile.");
      }

      public class LOGICWIREBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Automation Wire Bridge", nameof (LOGICWIREBRIDGE));
        public static LocString DESC = (LocString) "Wire bridges allow multiple automation grids to exist in a small area without connecting.";
        public static LocString EFFECT = (LocString) ("Runs one " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " section over another without joining them.\n\nCan be run through wall and floor tile.");
        public static LocString LOGIC_PORT = (LocString) "Transmit Signal";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active));
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICGATEAND
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("AND Gate", nameof (LOGICGATEAND));
        public static LocString DESC = (LocString) "This gate outputs a Green Signal when both its inputs are receiving Green Signals at the same time.";
        public static LocString EFFECT = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when both Input A <b>AND</b> Input B are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ".\n\nOutputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when even one Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby) + ".");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString OUTPUT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if any Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
      }

      public class LOGICGATEOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("OR Gate", nameof (LOGICGATEOR));
        public static LocString DESC = (LocString) "This gate outputs a Green Signal if receiving one or more Green Signals.";
        public static LocString EFFECT = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if at least one of Input A <b>OR</b> Input B is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ".\n\nOutputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when neither Input A or Input B are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ".");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if any Input is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString OUTPUT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
      }

      public class LOGICGATENOT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("NOT Gate", nameof (LOGICGATENOT));
        public static LocString DESC = (LocString) "This gate reverses automation signals, turning a Green Signal into a Red Signal and vice versa.";
        public static LocString EFFECT = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the Input is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".\n\nOutputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when its Input is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
        public static LocString OUTPUT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
      }

      public class LOGICGATEXOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("XOR Gate", nameof (LOGICGATEXOR));
        public static LocString DESC = (LocString) "This gate outputs a Green Signal if exactly one of its Inputs is receiving a Green Signal.";
        public static LocString EFFECT = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ".\n\nOutputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both or neither Inputs are receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString OUTPUT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Input signals match (any color)");
      }

      public class LOGICGATEBUFFER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("BUFFER Gate", nameof (LOGICGATEBUFFER));
        public static LocString DESC = (LocString) "This gate continues outputting a Green Signal for a short time after the gate stops receiving a Green Signal.";
        public static LocString EFFECT = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the Input is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nContinues sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for an amount of buffer time after the Input receives a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ". After receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby) + ", will continue sending " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + " until the timer has expired");
        public static LocString OUTPUT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".");
      }

      public class LOGICGATEFILTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("FILTER Gate", nameof (LOGICGATEFILTER));
        public static LocString DESC = (LocString) "This gate only lets a Green Signal through if its Input has received a Green Signal that lasted longer than the selected filter time.";
        public static LocString EFFECT = (LocString) ("Only lets a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " through if the Input has received a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for longer than the selected filter time.\n\nWill continue outputting a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " did not last long enough.");
        public static LocString OUTPUT_NAME = (LocString) "OUTPUT";
        public static LocString OUTPUT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " after receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + " for longer than the selected filter timer");
        public static LocString OUTPUT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".");
      }

      public class LOGICMEMORY
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Memory Toggle", nameof (LOGICMEMORY));
        public static LocString DESC = (LocString) "A Memory stores a Green Signal received in the Set Port (S) until the Reset Port (R) receives a Green Signal.";
        public static LocString EFFECT = (LocString) ("Contains an internal Memory, and will output whatever signal is stored in that Memory. Signals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output. \n\nSending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to the Set Port (S) will set the memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + ". \n\nSending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to the Reset Port (R) will reset the memory back to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby) + ".");
        public static LocString STATUS_ITEM_VALUE = (LocString) "Current Value: {0}";
        public static LocString READ_PORT = (LocString) "MEMORY OUTPUT";
        public static LocString READ_PORT_ACTIVE = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString READ_PORT_INACTIVE = (LocString) ("Outputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
        public static LocString SET_PORT = (LocString) "SET PORT (S)";
        public static LocString SET_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set the internal Memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString SET_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect");
        public static LocString RESET_PORT = (LocString) "RESET PORT (R)";
        public static LocString RESET_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
        public static LocString RESET_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect");
      }

      public class LOGICSWITCH
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Signal Switch", nameof (LOGICSWITCH));
        public static LocString DESC = (LocString) "Signal switches don't turn grids on and off like power switches, but add an extra signal.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " on an " + UI.FormatAsLink("Automation", "LOGIC") + " grid.\n\nMust be manually toggled by a Duplicant.");
        public static LocString LOGIC_PORT = (LocString) "Signal Toggle";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if toggled ON");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if toggled OFF");
      }

      public class LOGICPRESSURESENSORGAS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Sensor", nameof (LOGICPRESSURESENSORGAS));
        public static LocString DESC = (LocString) "Atmo sensors can be used to prevent excess oxygen production and overpressurization.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " pressure enters the chosen range.");
        public static LocString LOGIC_PORT = (LocString) (UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Pressure");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Gas pressure is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICPRESSURESENSORLIQUID
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Hydro Sensor", nameof (LOGICPRESSURESENSORLIQUID));
        public static LocString DESC = (LocString) "A hydro sensor can tell a pump to refill its basin as soon as it contains too little liquid.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " pressure enters the chosen range.\n\nMust be submerged in " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".");
        public static LocString LOGIC_PORT = (LocString) (UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Pressure");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Liquid pressure is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICTEMPERATURESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Sensor", nameof (LOGICTEMPERATURESENSOR));
        public static LocString DESC = (LocString) "Thermo sensors can disable buildings when they approach dangerous temperatures.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when ambient " + UI.FormatAsLink("Temperature", "HEAT") + " enters the chosen range.");
        public static LocString LOGIC_PORT = (LocString) ("Ambient " + UI.FormatAsLink("Temperature", "HEAT"));
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if ambient " + UI.FormatAsLink("Temperature", "HEAT") + " is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICTIMEOFDAYSENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Clock Sensor", nameof (LOGICTIMEOFDAYSENSOR));
        public static LocString DESC = (LocString) "Clock sensors ensure systems always turn on at the same time, day or night, every cycle.";
        public static LocString EFFECT = (LocString) ("Sets an automatic " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " and " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " schedule using a timer.");
        public static LocString LOGIC_PORT = (LocString) "Cycle Time";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if current time is within the selected " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active) + " range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICCRITTERCOUNTSENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Critter Sensor", nameof (LOGICCRITTERCOUNTSENSOR));
        public static LocString DESC = (LocString) "Detecting critter populations can help adjust their automated feeding and care regiments.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on the number of eggs and critters in a room.");
        public static LocString LOGIC_PORT = (LocString) "Critter Count";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Critters and Eggs in the Room is greater than the selected threshold.");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICDUPLICANTSENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Duplicant Motion Sensor", "DUPLICANTSENSOR");
        public static LocString DESC = (LocString) "Motion sensors save power by only enabling buildings when Duplicants are nearby.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on whether a Duplicant is in the sensor's range.");
        public static LocString LOGIC_PORT = (LocString) "Duplicant Motion Sensor";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while a Duplicant is in the sensor's tile range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICDISEASESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Germ Sensor", nameof (LOGICDISEASESENSOR));
        public static LocString DESC = (LocString) "Detecting germ populations can help block off or clean up dangerous areas.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on quantity of surrounding " + UI.FormatAsLink("Germs", "DISEASE") + ".");
        public static LocString LOGIC_PORT = (LocString) (UI.FormatAsLink("Germ", "DISEASE") + " Count");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICELEMENTSENSORGAS
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Element Sensor", nameof (LOGICELEMENTSENSORGAS));
        public static LocString DESC = (LocString) "These sensors can detect the presence of a specific gas and alter systems accordingly.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " is detected on this sensor's tile.\n\nSends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the selected " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " is not present.");
        public static LocString LOGIC_PORT = (LocString) ("Specific " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Gas is detected");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LOGICELEMENTSENSORLIQUID
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Element Sensor", nameof (LOGICELEMENTSENSORLIQUID));
        public static LocString DESC = (LocString) "These sensors can detect the presence of a specific liquid and alter systems accordingly.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " is detected on this sensor's tile.\n\nSends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the selected " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " is not present.");
        public static LocString LOGIC_PORT = (LocString) ("Specific" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Liquid is detected");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class GASCONDUITDISEASESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Pipe Germ Sensor", nameof (GASCONDUITDISEASESENSOR));
        public static LocString DESC = (LocString) "Germ sensors can help control automation behavior in the presence of germs.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on the internal " + UI.FormatAsLink("Germ", "DISEASE") + " count of the pipe.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LIQUIDCONDUITDISEASESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Pipe Germ Sensor", nameof (LIQUIDCONDUITDISEASESENSOR));
        public static LocString DESC = (LocString) "Germ sensors can help control automation behavior in the presence of germs.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " based on the internal " + UI.FormatAsLink("Germ", "DISEASE") + " count of the pipe.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class GASCONDUITELEMENTSENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Pipe Element Sensor", nameof (GASCONDUITELEMENTSENSOR));
        public static LocString DESC = (LocString) "Element sensors can be used to detect the presence of a specific gas in a pipe.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " is detected within a pipe.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Gas is detected");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LIQUIDCONDUITELEMENTSENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Pipe Element Sensor", nameof (LIQUIDCONDUITELEMENTSENSOR));
        public static LocString DESC = (LocString) "Element sensors can be used to detect the presence of a specific liquid in a pipe.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " is detected within a pipe.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence");
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Liquid is detected within the pipe");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class GASCONDUITTEMPERATURESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Gas Pipe Thermo Sensor", nameof (GASCONDUITTEMPERATURESENSOR));
        public static LocString DESC = (LocString) "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when pipe contents enter the chosen " + UI.FormatAsLink("Temperature", "HEAT") + " range.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " " + UI.FormatAsLink("Temperature", "HEAT"));
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Gas is within the selected Temperature range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class LIQUIDCONDUITTEMPERATURESENSOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Liquid Pipe Thermo Sensor", nameof (LIQUIDCONDUITTEMPERATURESENSOR));
        public static LocString DESC = (LocString) "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";
        public static LocString EFFECT = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " or a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when pipe contents enter the chosen " + UI.FormatAsLink("Temperature", "HEAT") + " range.");
        public static LocString LOGIC_PORT = (LocString) ("Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Temperature", "HEAT"));
        public static LocString LOGIC_PORT_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Liquid is within the selected Temperature range");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) ("Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby));
      }

      public class TRAVELTUBEENTRANCE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Transit Tube Access", nameof (TRAVELTUBEENTRANCE));
        public static LocString DESC = (LocString) "Duplicants require access points to enter tubes, but not to exit them.";
        public static LocString EFFECT = (LocString) ("Allows Duplicants to enter the connected " + UI.FormatAsLink("Transit Tube", "TRAVELTUBE") + " system.\n\nStops drawing " + UI.FormatAsLink("Power", "POWER") + " once fully charged.");
      }

      public class TRAVELTUBE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Transit Tube", nameof (TRAVELTUBE));
        public static LocString DESC = (LocString) "Duplicants will only exit a transit tube when a safe landing area is available beneath it.";
        public static LocString EFFECT = (LocString) ("Quickly transports Duplicants from a " + UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE") + " to the tube's end.\n\nOnly transports Duplicants.");
      }

      public class TRAVELTUBEWALLBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Transit Tube Crossing", nameof (TRAVELTUBEWALLBRIDGE));
        public static LocString DESC = (LocString) "Tube crossings can run transit tubes through walls without leaking gas or liquid.";
        public static LocString EFFECT = (LocString) ("Allows " + UI.FormatAsLink("Transit Tubes", "TRAVELTUBE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.");
      }

      public class SOLIDCONDUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Rail", nameof (SOLIDCONDUIT));
        public static LocString DESC = (LocString) "Rails move materials where they'll be needed most, saving Duplicants the walk.";
        public static LocString EFFECT = (LocString) ("Transports " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " on a track between " + UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX") + " and " + UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX") + ".\n\nCan be run through wall and floor tile.");
      }

      public class SOLIDCONDUITINBOX
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Loader", nameof (SOLIDCONDUITINBOX));
        public static LocString DESC = (LocString) "Material filters can be used to determine what resources are sent down the rail.";
        public static LocString EFFECT = (LocString) ("Loads " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " onto " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " for transport.\n\nOnly loads the resources of your choosing.");
      }

      public class SOLIDCONDUITOUTBOX
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Receptacle", nameof (SOLIDCONDUITOUTBOX));
        public static LocString DESC = (LocString) "When materials reach the end of a rail they enter a receptacle to be used by Duplicants.";
        public static LocString EFFECT = (LocString) ("Unloads " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " from a " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " into storage.");
      }

      public class SOLIDTRANSFERARM
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Auto-Sweeper", nameof (SOLIDTRANSFERARM));
        public static LocString DESC = (LocString) "An auto-sweeper's range can be viewed at any time by clicking on the building.";
        public static LocString EFFECT = (LocString) ("Automates " + UI.FormatAsLink("Sweeping", "CHORES") + " and " + UI.FormatAsLink("Supplying", "CHORES") + " errands by sucking up all nearby " + UI.FormatAsLink("Debris", "DECOR") + ".\n\nMaterials are automatically delivered to any " + UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX") + ", " + UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX") + ", storage, or buildings within range.");
      }

      public class SOLIDCONDUITBRIDGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Bridge", nameof (SOLIDCONDUITBRIDGE));
        public static LocString DESC = (LocString) "Separating rail systems helps ensure materials go to the intended destinations.";
        public static LocString EFFECT = (LocString) ("Runs one " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " section over another without joining them.\n\nCan be run through wall and floor tile.");
      }

      public class SOLIDVENT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Chute", nameof (SOLIDVENT));
        public static LocString DESC = (LocString) "When materials reach the end of a rail they are dropped back into the world.";
        public static LocString EFFECT = (LocString) ("Unloads " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " from a " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " onto the floor.");
      }

      public class SOLIDLOGICVALVE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Conveyor Shutoff", nameof (SOLIDLOGICVALVE));
        public static LocString DESC = (LocString) "Automated conveyors save power and time by removing the need for Duplicant input.";
        public static LocString EFFECT = (LocString) ("Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " transport on or off.");
        public static LocString LOGIC_PORT = (LocString) "Open/Close";
        public static LocString LOGIC_PORT_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow material transport");
        public static LocString LOGIC_PORT_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent material transport");
      }

      public class AUTOMINER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Robo-Miner", nameof (AUTOMINER));
        public static LocString DESC = (LocString) "A robo-miner's range can be viewed at any time by selecting the building.";
        public static LocString EFFECT = (LocString) "Automatically digs out all materials in a set range.";
      }

      public class CREATUREFEEDER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Critter Feeder", nameof (CREATUREFEEDER));
        public static LocString DESC = (LocString) "Critters tend to stay close to their food source and wander less when given a feeder.";
        public static LocString EFFECT = (LocString) ("Automatically dispenses food for hungry " + UI.FormatAsLink("Critters", "CRITTERS") + ".");
      }

      public class ITEMPEDESTAL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Pedestal", nameof (ITEMPEDESTAL));
        public static LocString DESC = (LocString) "Perception can be drastically changed by a bit of thoughtful presentation.";
        public static LocString EFFECT = (LocString) ("Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative decor will gain some positive decor when displayed.");
        public static LocString DISPLAYED_ITEM_FMT = (LocString) "Displayed {0}";
      }

      public class CROWNMOULDING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Crown Moulding", nameof (CROWNMOULDING));
        public static LocString DESC = (LocString) "Crown moulding is used as purely decorative trim for ceilings.";
        public static LocString EFFECT = (LocString) ("Used to decorate the ceilings of rooms.\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class CORNERMOULDING
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Corner Moulding", nameof (CORNERMOULDING));
        public static LocString DESC = (LocString) "Corner moulding is used as purely decorative trim for ceiling corners.";
        public static LocString EFFECT = (LocString) ("Used to decorate the ceiling corners of rooms.\n\nIncreases " + UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + UI.FormatAsLink("Morale", "MORALE") + ".");
      }

      public class EGGINCUBATOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Incubator", nameof (EGGINCUBATOR));
        public static LocString DESC = (LocString) "Incubators can maintain the ideal internal conditions for several species of critter egg.";
        public static LocString EFFECT = (LocString) ("Incubates " + UI.FormatAsLink("Critter", "CRITTERS") + " eggs until ready to hatch.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Critter Wrangling", "RANCHER") + " trait.");
      }

      public class EGGCRACKER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Egg Cracker", nameof (EGGCRACKER));
        public static LocString DESC = (LocString) "Raw eggs are an ingredient in certain high quality food recipes.";
        public static LocString EFFECT = (LocString) ("Converts viable " + UI.FormatAsLink("Critter", "CRITTERS") + " eggs into cooking ingredients.\n\nCracked Eggs cannot hatch.\n\nDuplicants will not crack eggs unless tasks are queued.");
        public static LocString RECIPE_DESCRIPTION = (LocString) "Turns {0} into {1}.";
        public static LocString RESULT_DESCRIPTION = (LocString) "Cracked {0}";
      }

      public class URANIUMCENTRIFUGE
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Uranium Centrifuge", nameof (URANIUMCENTRIFUGE));
        public static LocString DESC = (LocString) "Enriched uranium is a specialized substance that can be used to fuel powerful nuclear reactors.";
        public static LocString EFFECT = (LocString) ("Extracts " + UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM") + " from " + UI.FormatAsLink("Uranium Ore", "URANIUMORE") + ".\n\nOutputs " + UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM") + " in molten form.");
      }

      public class ASTRONAUTTRAININGCENTER
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Space Cadet Centrifuge", nameof (ASTRONAUTTRAININGCENTER));
        public static LocString DESC = (LocString) "Duplicants must complete astronaut training in order to pilot space rockets.";
        public static LocString EFFECT = (LocString) ("Trains Duplicants to become " + UI.FormatAsLink("Astronauts", "ASTRONAUT") + ".\n\nDuplicants must possess the " + UI.FormatAsLink("Astronaut-in-Training", "ASTRONAUTTRAINEE") + " trait to receive training.");
      }

      public class MASSIVEHEATSINK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Anti Entropy Thermo-Nullifier", nameof (MASSIVEHEATSINK));
        public static LocString DESC = (LocString) string.Empty;
        public static LocString EFFECT = (LocString) ("A self-sustaining machine powered by what appears to be refined " + UI.FormatAsLink("Neutronium", "UNOBTANIUM") + ".\n\nAbsorbs and neutralizes " + UI.FormatAsLink("Heat", "HEAT") + " energy when provided with piped " + UI.FormatAsLink("Hydrogen", "HYDROGEN") + ".");
      }

      public class FACILITYBACKWALLWINDOW
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Window", nameof (FACILITYBACKWALLWINDOW));
        public static LocString DESC = (LocString) string.Empty;
        public static LocString EFFECT = (LocString) "A tall, thin window.";
      }

      public class POIBUNKEREXTERIORDOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Security Door", nameof (POIBUNKEREXTERIORDOOR));
        public static LocString EFFECT = (LocString) "A strong door with a sophisticated genetic lock.";
        public static LocString DESC = (LocString) string.Empty;
      }

      public class POIDOORINTERNAL
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Security Door", nameof (POIDOORINTERNAL));
        public static LocString EFFECT = (LocString) "A strong door with a sophisticated genetic lock.";
        public static LocString DESC = (LocString) string.Empty;
      }

      public class POIFACILITYDOOR
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Lobby Doors", "FACILITYDOOR");
        public static LocString EFFECT = (LocString) "Large double doors that were once the main entrance to a large facility.";
        public static LocString DESC = (LocString) string.Empty;
      }

      public class VENDINGMACHINE
      {
        public static LocString NAME = (LocString) "Vending Machine";
        public static LocString DESC = (LocString) ("A pristine " + UI.FormatAsLink("Field Ration", "FIELDRATION") + " dispenser.");
      }

      public class GENESHUFFLER
      {
        public static LocString NAME = (LocString) "Neural Vacillator";
        public static LocString DESC = (LocString) "A massive synthetic brain, suspended in saline solution.\n\nThere is a chair attached to the device with room for one person.";
      }

      public class PROPTABLE
      {
        public static LocString NAME = (LocString) "Table";
        public static LocString DESC = (LocString) "A table and some chairs.";
      }

      public class PROPDESK
      {
        public static LocString NAME = (LocString) "Computer Desk";
        public static LocString DESC = (LocString) "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
      }

      public class PROPFACILITYCHAIR
      {
        public static LocString NAME = (LocString) "Lobby Chair";
        public static LocString DESC = (LocString) "A chair where visitors can comfortably wait before their appointments.";
      }

      public class PROPFACILITYCOUCH
      {
        public static LocString NAME = (LocString) "Lobby Couch";
        public static LocString DESC = (LocString) "A couch where visitors can comfortably wait before their appointments.";
      }

      public class PROPFACILITYDESK
      {
        public static LocString NAME = (LocString) "Director's Desk";
        public static LocString DESC = (LocString) "A spotless desk filled with impeccably organized office supplies.\n\nA photo peeks out from beneath the desk pad, depicting two beaming young women in caps and gowns.\n\nThe photo is quite old.";
      }

      public class PROPFACILITYTABLE
      {
        public static LocString NAME = (LocString) "Coffee Table";
        public static LocString DESC = (LocString) "A low coffee table that may have once held old science magazines.";
      }

      public class PROPFACILITYSTATUE
      {
        public static LocString NAME = (LocString) "Gravitas Monument";
        public static LocString DESC = (LocString) "A large, modern sculpture that sits in the center of the lobby.\n\nIt's an artistic cross between an hourglass shape and a double helix.";
      }

      public class PROPFACILITYCHANDELIER
      {
        public static LocString NAME = (LocString) "Chandelier";
        public static LocString DESC = (LocString) "A large chandelier that hangs from the ceiling.\n\nIt does not appear to function.";
      }

      public class PROPFACILITYGLOBEDROORS
      {
        public static LocString NAME = (LocString) "Filing Cabinet";
        public static LocString DESC = (LocString) "A filing cabinet for storing hard copy employee records.\n\nThe contents have been shredded.";
      }

      public class PROPFACILITYDISPLAY1
      {
        public static LocString NAME = (LocString) "Electronic Display";
        public static LocString DESC = (LocString) "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Printing Pod.";
      }

      public class PROPFACILITYDISPLAY2
      {
        public static LocString NAME = (LocString) "Electronic Display";
        public static LocString DESC = (LocString) "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Mining Gun.";
      }

      public class PROPFACILITYDISPLAY3
      {
        public static LocString NAME = (LocString) "Electronic Display";
        public static LocString DESC = (LocString) "An electronic display projecting the blueprint of a strange device.\n\nPerhaps these displays were used to entice visitors.";
      }

      public class PROPFACILITYTALLPLANT
      {
        public static LocString NAME = (LocString) "Office Plant";
        public static LocString DESC = (LocString) "It's survived the vacuum of space by virtue of being plastic.";
      }

      public class PROPFACILITYLAMP
      {
        public static LocString NAME = (LocString) "Light Fixture";
        public static LocString DESC = (LocString) "A long light fixture that hangs from the ceiling.\n\nIt does not appear to function.";
      }

      public class PROPFACILITYWALLDEGREE
      {
        public static LocString NAME = (LocString) "Doctorate Degree";
        public static LocString DESC = (LocString) "Certification in Applied Physics, awarded in recognition of one \"Jacquelyn A. Stern\".";
      }

      public class PROPFACILITYPAINTING
      {
        public static LocString NAME = (LocString) "Landscape Portrait";
        public static LocString DESC = (LocString) "A painting featuring a copse of fir trees and a magnificent mountain range on the horizon.\n\nThe air in the room prickles with the sensation that I'm not meant to be here.";
      }

      public class PROPRECEPTIONDESK
      {
        public static LocString NAME = (LocString) "Reception Desk";
        public static LocString DESC = (LocString) "A full coffee cup and a note abandoned mid sentence sit behind the desk.\n\nIt gives me an eerie feeling, as if the receptionist has stepped out and will return any moment.";
      }

      public class PROPELEVATOR
      {
        public static LocString NAME = (LocString) "Broken Elevator";
        public static LocString DESC = (LocString) "Out of service.\n\nThe buttons inside indicate it went down more than a dozen floors at one point in time.";
      }

      public class SETLOCKER
      {
        public static LocString NAME = (LocString) "Locker";
        public static LocString DESC = (LocString) "A basic metal locker.\n\nIt contains an assortment of personal effects.";
      }

      public class PROPLIGHT
      {
        public static LocString NAME = (LocString) "Light Fixture";
        public static LocString DESC = (LocString) "An elegant ceiling lamp, slightly worse for wear.";
      }

      public class PROPLADDER
      {
        public static LocString NAME = (LocString) "Ladder";
        public static LocString DESC = (LocString) "A hard plastic ladder.";
      }

      public class PROPSKELETON
      {
        public static LocString NAME = (LocString) "Model Skeleton";
        public static LocString DESC = (LocString) "A detailed anatomical model.\n\nIt appears to be made of resin.";
      }

      public class PROPSURFACESATELLITE1
      {
        public static LocString NAME = (LocString) "Crashed Satellite";
        public static LocString DESC = (LocString) "All that remains of a once peacefully orbiting satellite.";
      }

      public class PROPSURFACESATELLITE2
      {
        public static LocString NAME = (LocString) "Wrecked Satellite";
        public static LocString DESC = (LocString) "All that remains of a once peacefully orbiting satellite.";
      }

      public class PROPSURFACESATELLITE3
      {
        public static LocString NAME = (LocString) "Crushed Satellite";
        public static LocString DESC = (LocString) "All that remains of a once peacefully orbiting satellite.";
      }

      public class PROPCLOCK
      {
        public static LocString NAME = (LocString) "Clock";
        public static LocString DESC = (LocString) "A simple wall clock.\n\nIt is no longer ticking.";
      }
    }

    public static class DAMAGESOURCES
    {
      public static LocString NOTIFICATION_TOOLTIP = (LocString) "A {0} sustained damage from {1}";
      public static LocString CONDUIT_CONTENTS_FROZE = (LocString) "pipe contents becoming too cold";
      public static LocString CONDUIT_CONTENTS_BOILED = (LocString) "pipe contents becoming too hot";
      public static LocString BUILDING_OVERHEATED = (LocString) "overheating";
      public static LocString BAD_INPUT_ELEMENT = (LocString) "receiving an incorrect substance";
      public static LocString MINION_DESTRUCTION = (LocString) "an angry Duplicant. Rude!";
      public static LocString LIQUID_PRESSURE = (LocString) "neighboring liquid pressure";
      public static LocString CIRCUIT_OVERLOADED = (LocString) "an overloaded circuit";
      public static LocString MICROMETEORITE = (LocString) "micrometeorite";
      public static LocString COMET = (LocString) "falling space rocks";
      public static LocString ROCKET = (LocString) "rocket engine";
    }

    public static class AUTODISINFECTABLE
    {
      public static class ENABLE_AUTODISINFECT
      {
        public static LocString NAME = (LocString) "Enable Disinfect";
        public static LocString TOOLTIP = (LocString) "Automatically disinfect this building when it becomes contaminated";
      }

      public static class DISABLE_AUTODISINFECT
      {
        public static LocString NAME = (LocString) "Disable Disinfect";
        public static LocString TOOLTIP = (LocString) "Do not automatically disinfect this building";
      }

      public static class NO_DISEASE
      {
        public static LocString TOOLTIP = (LocString) "This building is clean";
      }
    }

    public static class DISINFECTABLE
    {
      public static class ENABLE_DISINFECT
      {
        public static LocString NAME = (LocString) "Disinfect";
        public static LocString TOOLTIP = (LocString) "Mark this building for disinfection";
      }

      public static class DISABLE_DISINFECT
      {
        public static LocString NAME = (LocString) "Cancel Disinfect";
        public static LocString TOOLTIP = (LocString) "Cancel this disinfect order";
      }

      public static class NO_DISEASE
      {
        public static LocString TOOLTIP = (LocString) "This building is already clean";
      }
    }

    public static class REPAIRABLE
    {
      public static class ENABLE_AUTOREPAIR
      {
        public static LocString NAME = (LocString) "Enable Autorepair";
        public static LocString TOOLTIP = (LocString) "Automatically repair this building when damaged";
      }

      public static class DISABLE_AUTOREPAIR
      {
        public static LocString NAME = (LocString) "Disable Autorepair";
        public static LocString TOOLTIP = (LocString) "Only repair this building when ordered";
      }
    }

    public static class AUTOMATABLE
    {
      public static class ENABLE_AUTOMATIONONLY
      {
        public static LocString NAME = (LocString) "Disable Manual";
        public static LocString TOOLTIP = (LocString) ("This building's storage may be accessed by Auto-Sweepers only" + UI.HORIZONTAL_BR_RULE + "Duplicants will not be permitted to add or remove materials from this building");
      }

      public static class DISABLE_AUTOMATIONONLY
      {
        public static LocString NAME = (LocString) "Enable Manual";
        public static LocString TOOLTIP = (LocString) "This building's storage may be accessed by both Duplicants and Auto-Sweeper buildings";
      }
    }
  }
}
