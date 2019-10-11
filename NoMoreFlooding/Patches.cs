using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;

namespace NoMoreFlooding
{
    public class Patches
    {
        private static float deltaTime = 0.033f;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("NoMoreFlooding", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(Constructable), "OnSpawn")]
        public static class Constructable_OnSpawn_Patch
        {

            public static void Prefix(Constructable __instance)
            {
                int cell = __instance.GetCell();

                if (HasSurroundingLiquid(cell))
                {
                    __instance.IsReplacementTile = true;
                    Dictionary<int, GameObject> building_layer = Grid.ObjectLayers[1]; // Building Layer (1) [FoundationTile 9]
                    GameObject go = building_layer.GetValueSafe(cell);
                    Deconstructable dec = go.GetComponent<Deconstructable>();

                    if (dec != null)
                    { 
                        float new_workTime = __instance.workTime + dec.workTime;
#if DEBUG
                        LogManager.LogDebug("Work Time was increased: Deconstructable (" + dec + ") \n" +
                            "\t Building Time:" + __instance.workTime + "\n" +
                            "\t Deconstruct Time:" + dec.workTime + "\n" +
                            "\t New Work Time:" + new_workTime
                            );
#endif
                        __instance.workTime = new_workTime;
                    }
                    else if (Grid.Solid[cell])
                    {
                        float calc_destory_time = CalculateDigWorkTime(cell);
                        float new_workTime = __instance.workTime + calc_destory_time;
#if DEBUG
                        LogManager.LogDebug("Work Time was increased: Diggable\n" +
                            "\t Building Time:" + __instance.workTime + "\n" +
                            "\t Calc Destroy Time:" + calc_destory_time + "\n" +
                            "\t New Work Time:" + new_workTime
                            );
#endif
                        __instance.workTime = new_workTime;
                    }
                }
            }
        }

        private static float CalculateDigWorkTime(int cell) 
        {
            float hardness = Grid.Element[cell].hardness;
            Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Ice);
            float num1 = hardness / (float) elementByHash.hardness;
            float num2 = 4f * (Mathf.Min(Grid.Mass[cell], 400f) / 400f);
            float damage = num2 + num1 * num2;
            float tick_damage = Patches.deltaTime / (float) damage;
#if DEBUG
            LogManager.LogDebug(
                "\n\tTick Damage: " + tick_damage + "\n" +
                "\tNum 1: " + num1 + "\n" +
                "\tNum 2: " + num2 + "\n" +
                "\tTime.DeltaTime: " + deltaTime + "\n" +
                "\t Damage: " + damage 
                );
#endif
            return 1f / tick_damage;
        }

        [HarmonyPatch(typeof(Diggable), "DoDigTick")]
        public static class Diggable_DoDigTick_Patch
        {
            public static void Postfix(ref int cell, ref float dt)
            {
#if DEBUG
                float hardness = (float)Grid.Element[cell].hardness;
                if ((double)hardness == (double)byte.MaxValue)
                    return;
                Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Ice);
                float num1 = hardness / (float)elementByHash.hardness;
                float num2 = 4f * (Mathf.Min(Grid.Mass[cell], 400f) / 400f);
                float num3 = num2 + num1 * num2;
                float amount = dt / num3;
                LogManager.LogDebug("\n\tOnWorkTick Damage: " + amount);
#endif
            }
        }

        [HarmonyPatch(typeof(Game), "SimEveryTick")]
        public static class Game_Update_Patch
        {

            public static void Postfix(float dt)
            {
                Patches.deltaTime = dt;
            }
        }


        [HarmonyPatch(typeof(Constructable), "OnPressCancel")]
        public static class Constructable_OnPressCancel_Patch
        {

            public static void Prefix(Constructable __instance)
            {
                if (HasSurroundingLiquid(__instance.GetCell()))
                    __instance.IsReplacementTile = false;
            }
        }

        [HarmonyPatch(typeof(Constructable), "OnCancel")]
        public static class Constructable_OnCancel_Patch
        {

            public static void Prefix(Constructable __instance)
            {
                if (HasSurroundingLiquid(__instance.GetCell()))
                    __instance.IsReplacementTile = false;
            }
        }

        private static bool HasSurroundingLiquid(int cell)
        {
            bool return_state = false;

            if (Grid.IsLiquid(Grid.CellAbove(cell)) || Grid.IsLiquid(Grid.CellRight(cell)) || Grid.IsLiquid(Grid.CellLeft(cell)))
                return_state = true;

            return return_state;
        }
    }
}
