// Decompiled with JetBrains decompiler
// Type: STRINGS.WORLD_TRAITS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace STRINGS
{
  public static class WORLD_TRAITS
  {
    public static class NO_TRAITS
    {
      public static LocString NAME = (LocString) "<i>This world is stable and has no unusual features.</i>";
      public static LocString DESCRIPTION = (LocString) "This world exists in a particularly stable configuration each time it is encountered.";
    }

    public static class BOULDERS_LARGE
    {
      public static LocString NAME = (LocString) "Large Boulders";
      public static LocString DESCRIPTION = (LocString) "A number of large but hard-to-dig boulders are scattered around this world";
    }

    public static class BOULDERS_MEDIUM
    {
      public static LocString NAME = (LocString) "Medium Boulders";
      public static LocString DESCRIPTION = (LocString) "A number of moderately-sized but hard-to-dig boulders are scattered around this world";
    }

    public static class BOULDERS_MIXED
    {
      public static LocString NAME = (LocString) "Mixed Boulders";
      public static LocString DESCRIPTION = (LocString) "A number of hard-to-dig boulders of various sizes are scattered around this world";
    }

    public static class BOULDERS_SMALL
    {
      public static LocString NAME = (LocString) "Small Boulders";
      public static LocString DESCRIPTION = (LocString) "A number of small but hard-to-dig boulders are scattered around this world";
    }

    public static class DEEP_OIL
    {
      public static LocString NAME = (LocString) "Buried Oil";
      public static LocString DESCRIPTION = (LocString) ("Most of the " + UI.PRE_KEYWORD + "Oil" + UI.PST_KEYWORD + " in this world remains inside the rock and must be extracted with " + (string) BUILDINGS.PREFABS.OILWELLCAP.NAME + "s");
    }

    public static class FROZEN_CORE
    {
      public static LocString NAME = (LocString) "Frozen Core";
      public static LocString DESCRIPTION = (LocString) ("The core of this world is a block of " + (string) ELEMENTS.ICE.NAME + string.Empty);
    }

    public static class GEOACTIVE
    {
      public static LocString NAME = (LocString) "Geoactive";
      public static LocString DESCRIPTION = (LocString) ("This world has more " + UI.PRE_KEYWORD + "Geysers" + UI.PST_KEYWORD + " and " + UI.PRE_KEYWORD + "Vents" + UI.PST_KEYWORD + " than usual");
    }

    public static class GEODES
    {
      public static LocString NAME = (LocString) "Geodes";
      public static LocString DESCRIPTION = (LocString) "A number of large geodes containing caches of rare material have been deposited around this asteroid";
    }

    public static class GEODORMANT
    {
      public static LocString NAME = (LocString) "Geodormant";
      public static LocString DESCRIPTION = (LocString) ("This world has fewer " + UI.PRE_KEYWORD + "Geysers" + UI.PST_KEYWORD + " and " + UI.PRE_KEYWORD + "Vents" + UI.PST_KEYWORD + " than usual");
    }

    public static class GLACIERS_LARGE
    {
      public static LocString NAME = (LocString) "Large Glaciers";
      public static LocString DESCRIPTION = (LocString) ("Huge chunks of primordial " + (string) ELEMENTS.ICE.NAME + " have been captured in this world");
    }

    public static class IRREGULAR_OIL
    {
      public static LocString NAME = (LocString) "Irregular Oil";
      public static LocString DESCRIPTION = (LocString) ("The " + UI.PRE_KEYWORD + "Oil" + UI.PST_KEYWORD + " on this asteroid is anything but regular!");
    }

    public static class MAGMA_VENTS
    {
      public static LocString NAME = (LocString) "Magma Channels";
      public static LocString DESCRIPTION = (LocString) ("The " + (string) ELEMENTS.MAGMA.NAME + " of the core has spread up in to the body of this world");
    }

    public static class METAL_POOR
    {
      public static LocString NAME = (LocString) "Metal Poor";
      public static LocString DESCRIPTION = (LocString) ("There is less " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD + " than expected here, proceed with caution!");
    }

    public static class METAL_RICH
    {
      public static LocString NAME = (LocString) "Metal Rich";
      public static LocString DESCRIPTION = (LocString) ("This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD);
    }

    public static class MISALIGNED_START
    {
      public static LocString NAME = (LocString) "Miscalculated Pod Location";
      public static LocString DESCRIPTION = (LocString) ("The " + (string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " failed to land in the center of this world");
    }

    public static class SLIME_SPLATS
    {
      public static LocString NAME = (LocString) "Slime Molds";
      public static LocString DESCRIPTION = (LocString) ("Sickly " + (string) ELEMENTS.SLIMEMOLD.NAME + " growths have been located all over this world");
    }

    public static class SUBSURFACE_OCEAN
    {
      public static LocString NAME = (LocString) "Subsurface Ocean";
      public static LocString DESCRIPTION = (LocString) ("Below the crust of this world is a " + (string) ELEMENTS.SALTWATER.NAME + " sea");
    }

    public static class VOLCANOES
    {
      public static LocString NAME = (LocString) "Volcanoes";
      public static LocString DESCRIPTION = (LocString) ("Several active " + UI.PRE_KEYWORD + "Volcanoes" + UI.PST_KEYWORD + " have been detected in this world");
    }
  }
}
