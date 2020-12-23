using Harmony;
using Zolibrary.Logging;

namespace BiggerBrushes
{
    public class BrushPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("BiggerBrushes", "1.0.2");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "OnSpawn")]
        public static class SandboxToolParameterMenu_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance) => __instance.brushRadiusSlider.SetRange(BrushConfigChecker.MinSize, BrushConfigChecker.MaxSize);
        }
    }
}
