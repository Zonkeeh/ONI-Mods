using STRINGS;

namespace AdvancedSpaceScanner
{
    public static class AdvancedSpaceScannerStrings
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class ADVANCEDSPACESCANNER
                {
                    public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Space Scanner", "AdvancedSpaceScanner");
                    public static LocString DESC = (LocString)("Networks of many scanners will scan more efficiently than one on its own.");
                    public static LocString EFFECT = (LocString)("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.\n\nBunkered to protect from meteor damage.");
                }
            }
        }
    }
}
