// Decompiled with JetBrains decompiler
// Type: STRINGS.EQUIPMENT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public class EQUIPMENT
  {
    public class PREFABS
    {
      public class ATMO_SUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Suit", nameof (ATMO_SUIT));
        public static LocString DESC = (LocString) "Ensures my Duplicants can breathe easy, anytime, anywhere.";
        public static LocString EFFECT = (LocString) "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an Atmo Suit Dock when depleted.";
        public static LocString RECIPE_DESC = (LocString) "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";
        public static LocString GENERICNAME = (LocString) "Suit";
      }

      public class AQUA_SUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Aqua Suit", nameof (AQUA_SUIT));
        public static LocString DESC = (LocString) "Because breathing underwater is better than... not.";
        public static LocString EFFECT = (LocString) ("Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " at an Atmo Suit Dock when depleted.");
        public static LocString RECIPE_DESC = (LocString) "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";
      }

      public class TEMPERATURE_SUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Suit", nameof (TEMPERATURE_SUIT));
        public static LocString DESC = (LocString) "Keeps my Duplicants cool in case things heat up.";
        public static LocString EFFECT = (LocString) "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";
        public static LocString RECIPE_DESC = (LocString) "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";
      }

      public class JET_SUIT
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Jet Suit", nameof (JET_SUIT));
        public static LocString DESC = (LocString) "Allows my Duplicants to take to the skies, for a time.";
        public static LocString EFFECT = (LocString) ("Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nMust be refilled with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " at a Jet Suit Dock when depleted.");
        public static LocString RECIPE_DESC = (LocString) ("Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.");
        public static LocString GENERICNAME = (LocString) "Jet Suit";
        public static LocString TANK_EFFECT_NAME = (LocString) "Fuel Tank";
      }

      public class COOL_VEST
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Cool Vest", nameof (COOL_VEST));
        public static LocString GENERICNAME = (LocString) "Clothing";
        public static LocString DESC = (LocString) "Don't sweat it!";
        public static LocString EFFECT = (LocString) "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";
        public static LocString RECIPE_DESC = (LocString) "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";
      }

      public class WARM_VEST
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Warm Sweater", nameof (WARM_VEST));
        public static LocString GENERICNAME = (LocString) "Clothing";
        public static LocString DESC = (LocString) "Happiness is a warm Duplicant.";
        public static LocString EFFECT = (LocString) "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";
        public static LocString RECIPE_DESC = (LocString) "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";
      }

      public class FUNKY_VEST
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Snazzy Suit", nameof (FUNKY_VEST));
        public static LocString GENERICNAME = (LocString) "Clothing";
        public static LocString DESC = (LocString) "This transforms my Duplicant into a walking beacon of charm and style.";
        public static LocString EFFECT = (LocString) "Increases Decor in a small area effect around the wearer.";
        public static LocString RECIPE_DESC = (LocString) "Increases Decor in a small area effect around the wearer.";
      }

      public class OXYGEN_TANK
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen Tank", nameof (OXYGEN_TANK));
        public static LocString GENERICNAME = (LocString) "Equipment";
        public static LocString DESC = (LocString) string.Empty;
        public static LocString EFFECT = (LocString) "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";
        public static LocString RECIPE_DESC = (LocString) "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";
      }

      public class OXYGEN_TANK_UNDERWATER
      {
        public static LocString NAME = (LocString) "Oxygen Rebreather";
        public static LocString GENERICNAME = (LocString) "Equipment";
        public static LocString DESC = (LocString) string.Empty;
        public static LocString EFFECT = (LocString) "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";
        public static LocString RECIPE_DESC = (LocString) "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";
      }
    }
  }
}
