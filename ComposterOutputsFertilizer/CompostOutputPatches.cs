using Harmony;
using UnityEngine;
using Zolibrary.Logging;

namespace ComposterOutputsFertilizer
{
    public class ComposterOutputsFertilizer
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("CompostOutputsFertilizer", "1.0.3");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(CompostConfig), "ConfigureBuildingTemplate")]
        public static class CompostConfig_ConfigureBuildingTemplate_Patch
        {

            public static void Postfix(ref GameObject go)
            {
                ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
                ElementDropper elementDropper = go.AddOrGet<ElementDropper>();

                if (!CompostConfigChecker.GenerateFertilizer)
                    SetupDirt(elementConverter, elementDropper);
                else
                    SetupFertilizer(elementConverter, elementDropper);

                #if DEBUG
                    foreach (ElementConverter.OutputElement e in elementConverter.outputElements)
                        LogManager.LogDebug("\nOutputElement: " + e.ToString() + "\n   "
                            + "element hash: " + e.elementHash + "\n   "
                            + "element rate: " + e.massGenerationRate
                            );

                    foreach (ElementDropper d in go.GetComponents<ElementDropper>())
                        LogManager.LogDebug("\nElementDropper: " + d.ToString() + "\n   "
                            + "element tag: " + d.emitTag + "\n   "
                            + "emit mass: " + d.emitMass
                            );
                #endif
            }

            private static void SetupDirt(ElementConverter converter, ElementDropper dropper)
            {
                var dirt_output = converter.outputElements[0];
                dirt_output.elementHash = (SimHashes.Dirt);
                dirt_output.massGenerationRate = (CompostConfigChecker.GenMultiplier * 0.1f);
                dropper.emitMass = CompostConfigChecker.EmitMass;
            }

            private static void SetupFertilizer(ElementConverter converter, ElementDropper dropper)
            {
                converter.outputElements = new ElementConverter.OutputElement[1] {
                    new ElementConverter.OutputElement(
                        kgPerSecond: (CompostConfigChecker.GenMultiplier * 0.1f),
                        element: SimHashes.Fertilizer,
                        minOutputTemperature: 348.15f,
                        useEntityTemperature: false,
                        storeOutput: true,
                        outputElementOffsetx: 0.0f,
                        outputElementOffsety: 0.5f,
                        diseaseWeight: 1f,
                        addedDiseaseIdx: byte.MaxValue,
                        addedDiseaseCount: 0
                        )};

                dropper.emitMass = CompostConfigChecker.EmitMass;
                dropper.emitTag = SimHashes.Fertilizer.CreateTag();
            }
        }
    }
}
