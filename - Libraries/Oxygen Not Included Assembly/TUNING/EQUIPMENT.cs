// Decompiled with JetBrains decompiler
// Type: TUNING.EQUIPMENT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace TUNING
{
  public class EQUIPMENT
  {
    public class ATTRIBUTE_MOD_IDS
    {
      public static string DECOR = "Decor";
      public static string INSULATION = "Insulation";
      public static string ATHLETICS = "Athletics";
      public static string DIGGING = "Digging";
      public static string MAX_UNDERWATER_TRAVELCOST = "MaxUnderwaterTravelCost";
      public static string THERMAL_CONDUCTIVITY_BARRIER = "ThermalConductivityBarrier";
    }

    public class TOOLS
    {
      public static string TOOLSLOT = "Multitool";
      public static string TOOLFABRICATOR = "MultitoolWorkbench";
      public static string TOOL_ANIM = "constructor_gun_kanim";
    }

    public class CLOTHING
    {
      public static string SLOT = "Outfit";
    }

    public class SUITS
    {
      public static string SLOT = "Suit";
      public static string FABRICATOR = "SuitFabricator";
      public static string ANIM = "clothing_kanim";
      public static string SNAPON = "snapTo_neck";
      public static int ATMOSUIT_FABTIME = 40;
      public static int ATMOSUIT_INSULATION = 50;
      public static int ATMOSUIT_ATHLETICS = -6;
      public static float ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.2f;
      public static int ATMOSUIT_DIGGING = 10;
      public static int ATMOSUIT_CONSTRUCTION = 10;
      public static float ATMOSUIT_BLADDER = -0.1833333f;
      public static int ATMOSUIT_MASS = 200;
      public static int ATMOSUIT_SCALDING = 1000;
      public static int AQUASUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;
      public static int AQUASUIT_INSULATION = 0;
      public static int AQUASUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;
      public static int AQUASUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;
      public static int AQUASUIT_UNDERWATER_TRAVELCOST = 6;
      public static int TEMPERATURESUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;
      public static float TEMPERATURESUIT_INSULATION = 0.2f;
      public static int TEMPERATURESUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;
      public static int TEMPERATURESUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;
      public static float MINIMUM_USABLE_SUIT_CHARGE = 0.95f;
    }

    public class VESTS
    {
      public static string SLOT = "Suit";
      public static string FABRICATOR = "ClothingFabricator";
      public static string SNAPON0 = "snapTo_body";
      public static string SNAPON1 = "snapTo_arm";
      public static string WARM_VEST_ANIM0 = "body_shirt_hot01_kanim";
      public static string WARM_VEST_ANIM1 = "body_shirt_hot02_kanim";
      public static string WARM_VEST_ICON0 = "shirt_hot01_kanim";
      public static string WARM_VEST_ICON1 = "shirt_hot02_kanim";
      public static float WARM_VEST_FABTIME = 180f;
      public static float WARM_VEST_INSULATION = 0.01f;
      public static int WARM_VEST_MASS = 4;
      public static string COOL_VEST_ANIM0 = "body_shirt_cold01_kanim";
      public static string COOL_VEST_ANIM1 = "body_shirt_cold02_kanim";
      public static string COOL_VEST_ICON0 = "shirt_cold01_kanim";
      public static string COOL_VEST_ICON1 = "shirt_cold02_kanim";
      public static float COOL_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;
      public static float COOL_VEST_INSULATION = 0.01f;
      public static int COOL_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;
      public static string FUNKY_VEST_ANIM0 = "body_shirt_decor01_kanim";
      public static string FUNKY_VEST_ICON0 = "shirt_decor01_kanim";
      public static float FUNKY_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;
      public static float FUNKY_VEST_DECOR = 1f;
      public static int FUNKY_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;
    }
  }
}
