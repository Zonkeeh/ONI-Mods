using Harmony;
using STRINGS;
using System;

namespace DuplicantLifecycles
{
    public static class DuplicantLifecycleStrings
    {
        public static StatusItem AgingYouth;
        public static StatusItem AgingMiddle;
        public static StatusItem AgingElderly;
        public static StatusItem AgingDying;
        public static StatusItem Immortal;

        public static LocString AgingID = (LocString)"DuplicantLifecycles.Aging";
        public static LocString AgingNAME = (LocString) "Simulated Lifespan";
        public static LocString AgingDESC = (LocString) "Unless immortal, this duplicant will slowly age, dwindle and die!";

        public static LocString ImmortalID = (LocString)"DuplicantLifecycles.Immortal";
        public static LocString ImmortalNAME = (LocString)"Immortal";
        public static LocString ImmortalDESC = (LocString)"This duplicant will no longer age, or suffer age side effects!";

        public static string AgingYouthKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGYOUTH";
        public static LocString AgingYouthName = "Youthful";
        public static LocString AgingYouthTooltip = (LocString)("This duplicant is young and spritely: \n {0}");

        public static string AgingMiddleKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGMIDDLE";
        public static LocString AgingMiddleName = "Middle Aged";
        public static LocString AgingMiddleTooltip = (LocString)("This duplicant is fitting well into adult life: \n {0}");

        public static string AgingElderlyKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGELDERLY";
        public static LocString AgingElderlyName = "Elderly";
        public static LocString AgingElderlyTooltip = (LocString)("This duplicant has become old and fragile: \n {0}");

        public static string AgingDyingKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGDYING";
        public static LocString AgingDyingName = "Frail (Will Pass Soon)";
        public static LocString AgingDyingTooltip = (LocString)("This duplicant is too old to function and will likely pass soon: \n {0}");

        public static string ImmortalKey = "STRINGS.DUPLICANTS.STATUSITEMS.IMMORTAL";
        public static LocString ImmortalName = "Immortal";
        public static LocString ImmortalTooltip = (LocString)("This duplicant is immortal and no longer ages: \n {0}");

        public static string DeathMessage = "{Target} has sadly passed away due to their old age.";

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                SetupStringsFromConfig();
                AddStrings();
            }
        }

        private static string GetSign(float value)
        {
            if (value == 0)
                return "";
            else if (value > 0)
                return "+";
            else
                return "-";
        }

        public static void SetupStringsFromConfig()
        {
            string description;
            float custom_base;

            if (DuplicantLifecycleConfigChecker.UsePercentage)
            {
                description = "• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " {0}<b>{1}%</b>";
                custom_base = 100f;
            }
            else
            {
                description = "• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are {0}<b>{1}</b>";
                custom_base = DuplicantLifecycleConfigChecker.CustomBase;
            }

            DuplicantLifecycleStrings.AgingYouthTooltip = string.Format(
                DuplicantLifecycleStrings.AgingYouthTooltip,
                string.Format(
                    description,
                    GetSign(DuplicantLifecycleConfigChecker.YouthAttributeMultiplier),
                    Math.Abs(custom_base * DuplicantLifecycleConfigChecker.YouthAttributeMultiplier).ToString()
                ));

            DuplicantLifecycleStrings.AgingMiddleTooltip = string.Format(
                DuplicantLifecycleStrings.AgingMiddleTooltip,
                string.Format(
                    description,
                    GetSign(DuplicantLifecycleConfigChecker.MiddleAttributeMultiplier),
                    Math.Abs(custom_base * DuplicantLifecycleConfigChecker.MiddleAttributeMultiplier).ToString()
                ));

            DuplicantLifecycleStrings.AgingElderlyTooltip = string.Format(
                DuplicantLifecycleStrings.AgingElderlyTooltip,
                string.Format(
                    description,
                    GetSign(DuplicantLifecycleConfigChecker.ElderlyAttributeMultiplier),
                    Math.Abs(custom_base * DuplicantLifecycleConfigChecker.ElderlyAttributeMultiplier).ToString()
                ));

            DuplicantLifecycleStrings.AgingDyingTooltip = string.Format(
                DuplicantLifecycleStrings.AgingDyingTooltip,
                string.Format(
                    description,
                    GetSign(DuplicantLifecycleConfigChecker.DyingAttributeMultiplier),
                    Math.Abs(custom_base * DuplicantLifecycleConfigChecker.DyingAttributeMultiplier).ToString()
                ));

            DuplicantLifecycleStrings.ImmortalTooltip = string.Format(
                DuplicantLifecycleStrings.ImmortalTooltip,
                string.Format(
                    description,
                    GetSign(DuplicantLifecycleConfigChecker.ImmortalAttributeMultiplier),
                    Math.Abs(custom_base * DuplicantLifecycleConfigChecker.ImmortalAttributeMultiplier).ToString()
                ));
        }

        private static void AddStrings()
        {
            Strings.Add(DuplicantLifecycleStrings.AgingYouthKey + ".NAME", DuplicantLifecycleStrings.AgingYouthName);
            Strings.Add(DuplicantLifecycleStrings.AgingYouthKey + ".TOOLTIP", DuplicantLifecycleStrings.AgingYouthTooltip);
            Strings.Add(DuplicantLifecycleStrings.AgingMiddleKey + ".NAME", DuplicantLifecycleStrings.AgingMiddleName);
            Strings.Add(DuplicantLifecycleStrings.AgingMiddleKey + ".TOOLTIP", DuplicantLifecycleStrings.AgingMiddleTooltip);
            Strings.Add(DuplicantLifecycleStrings.AgingElderlyKey + ".NAME", DuplicantLifecycleStrings.AgingElderlyName);
            Strings.Add(DuplicantLifecycleStrings.AgingElderlyKey + ".TOOLTIP", DuplicantLifecycleStrings.AgingElderlyTooltip);
            Strings.Add(DuplicantLifecycleStrings.AgingDyingKey + ".NAME", DuplicantLifecycleStrings.AgingDyingName);
            Strings.Add(DuplicantLifecycleStrings.AgingDyingKey + ".TOOLTIP", DuplicantLifecycleStrings.AgingDyingTooltip);
            Strings.Add(DuplicantLifecycleStrings.ImmortalKey + ".NAME", DuplicantLifecycleStrings.ImmortalName);
            Strings.Add(DuplicantLifecycleStrings.ImmortalKey + ".TOOLTIP", DuplicantLifecycleStrings.ImmortalTooltip);

            DuplicantLifecycleStrings.AgingYouth =
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "AgingYouth",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();

            DuplicantLifecycleStrings.AgingMiddle =
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "AgingMiddle",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();

            DuplicantLifecycleStrings.AgingElderly =
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "AgingElderly",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Exclamation,
                    NotificationType.BadMinor,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();

            DuplicantLifecycleStrings.AgingDying =
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "AgingDying",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Exclamation,
                    NotificationType.DuplicantThreatening,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();

            DuplicantLifecycleStrings.Immortal=
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "Immortal",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Good,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();
        }
    }
}
