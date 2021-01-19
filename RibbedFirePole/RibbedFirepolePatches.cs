#define UsesDLC
using Harmony;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;

namespace RibbedFirePole
{
    public class RibbedFirePolePatches
    {
        public static Config config;

        public class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Ribbed Fire Pole", "1.0.4");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                RibbedFirePolePatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        public static void RegisterBuildings()
        {
            BuildingUtils.AddStrings(RibbedFirePoleConfig.ID, RibbedFirePoleConfig.DisplayName, RibbedFirePoleConfig.Description, RibbedFirePoleConfig.Effect);
            BuildingUtils.AddToPlanning("Base", RibbedFirePoleConfig.ID, "FirePole");
            BuildingUtils.AddToTechnology("HighTempForging", RibbedFirePoleConfig.ID);
        }

#if UsesDLC
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public class Db_Initialise_Patch
		{
			public static void Postfix() => RibbedFirePolePatches.RegisterBuildings();
		}
#else
        [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix() => RibbedFirePolePatches.RegisterBuildings();
        }
#endif
    }
}
