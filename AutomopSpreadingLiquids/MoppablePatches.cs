using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;

namespace NoMoreFlooding
{
    public class MoppablePatches
    {

        private static List<CellOffset> offsetList;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("AutomopSpreadingLiquids", "1.0.0");
                LogManager.LogInit();
                
                offsetList = new List<CellOffset>()
                {
                    new CellOffset(1, 0),
                    new CellOffset(1, -1),
                    new CellOffset(-1, 0),
                    new CellOffset(-1, -1)
                };
            }
        }

        [HarmonyPatch(typeof(FallingWater), "AddToSim")]
        public static class FallingWater_AddToSim_Patch
        {

            public static void Postfix(ref int cell)
            {
                    GameObject gameObject = Grid.Objects[cell, 8];
                LogManager.LogDebug("@" + cell);
                    gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("MopPlacer")), null, null);
                    Grid.Objects[cell, 8] = gameObject;
                    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.SolidConduits);
                    float num = -0.15f;
                    posCbc.z += num;
                    gameObject.transform.SetPosition(posCbc);
                    gameObject.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(Moppable), "OnCellMopped")]
        public static class Moppable_OnCellMopped_Patch
        {

            public static void Postfix(Moppable __instance)
            {
                int cell = Grid.PosToCell((KMonoBehaviour)__instance);

                foreach (CellOffset co in offsetList)
                {
                    int offsetCell = Grid.OffsetCell(cell, co);
                    GameObject gameObject = Grid.Objects[offsetCell, 8];

                    if (Grid.Solid[Grid.CellBelow(offsetCell)] &&
                        Grid.Element[offsetCell].IsLiquid &&
                        Grid.Mass[offsetCell] <= MopTool.maxMopAmt &&
                        gameObject == null)
                    {
                        gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("MopPlacer")), null, null);
                        Grid.Objects[offsetCell, 8] = gameObject;
                        Vector3 posCbc = Grid.CellToPosCBC(offsetCell, Grid.SceneLayer.SolidConduits);
                        float num = -0.15f;
                        posCbc.z += num;
                        gameObject.transform.SetPosition(posCbc);
                        gameObject.SetActive(true);
                    }
                    else if (!Grid.Element[offsetCell].IsLiquid && gameObject != null)
                    {
                        UnityEngine.GameObject.Destroy(gameObject);
                    }
                }
            }
        }
    }
}
