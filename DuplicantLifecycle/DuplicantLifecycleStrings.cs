using Harmony;
using STRINGS;

namespace DuplicantLifecycle
{
    public static class DuplicantLifecycleStrings
    {
        public static StatusItem AgingYouth;
        public static StatusItem AgingMiddle;
        public static StatusItem AgingElderly;
        public static StatusItem AgingDying;
        public static LocString ID = (LocString)"Aging";
        public static LocString NAME = (LocString) "Limited Lifespan";
        public static LocString DESC = (LocString) "This Duplicant will be affected by DuplicantLifecycle!";
        public static LocString EXTENDED_DESC = (LocString)(
            "This Duplicant has a limited lifespan (Max: {0} cycles)"
            + "\n" +
            "• Youthful: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>{1}%</b> of their default values"
            + "\n" +
            "• Middle Aged: All" + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>{2}%</b> of their default values"
            + "\n" +
            "• Elderly: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>{3}%</b> of their default values"
            + "\n" +
            "• Dying: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>{4}%</b> of their default values. Duplicants will also appear sick and shortly die of old age."
            );
        public static LocString SHORT_DESC = (LocString)"This Duplicant will be affected by DuplicantLifecycle!";
        public static LocString SHORT_DESC_TOOLTIP = (LocString)(
            "This Duplicant has a limited lifespan (Max: 1000 cycles)"
            + "\n" +
            "• Youthful: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>120%</b> of their default values"
            + "\n" +
            "• Middle Aged: All" + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are reset to their default values"
            + "\n" +
            "• Elderly: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>70%</b> of their default values"
            + "\n" +
            "• Dying: All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>20%</b> of their default values. Duplicants will also appear sick and shortly die of old age."
            );

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix() => DuplicantLifecycleStrings.AddStrings();
        }

        public static string AgingYouthKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGYOUTH";
        public static LocString AgingYouthName = "Youthful";
        public static LocString AgingYouthTooltip = (LocString)("This Duplicant is young and spritely: \n• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>120%</b>");

        public static string AgingMiddleKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGMIDDLE";
        public static LocString AgingMiddleName = "Middle Aged";
        public static LocString AgingMiddleTooltip = (LocString)("This Duplicant is fitting well into adult life: \n• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>100%</b>");

        public static string AgingElderlyKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGELDERLY";
        public static LocString AgingElderlyName = "Elderly";
        public static LocString AgingElderlyTooltip = (LocString)("This Duplicant has become old and fragile: \n• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>70%</b>");

        public static string AgingDyingKey = "STRINGS.DUPLICANTS.STATUSITEMS.AGINGDYING";
        public static LocString AgingDyingName = "Dying";
        public static LocString AgingDyingTooltip = (LocString)("This Duplicant is too old to function and will likely pass soon: \n• All " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD + " are at <b>20%</b>");

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
        }
    }
}
