using STRINGS;

namespace BuildablePoiDoors
{
    public static class BuildablePOIDoorsStrings
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class BUILDABLEPOIFACILITYDOOR
                {
                    public static LocString NAME = (LocString) UI.FormatAsLink("Facility Door", "BuildablePOIFacilityDoor");
                    public static LocString DESC = (LocString)("A door fit for kings, or just some tall people...");
                    public static LocString EFFECT = (LocString)("Buildable form of the POI facility door.");
                }

                public static class BUILDABLEPOIINTERNALDOOR
                {
                    public static LocString NAME = (LocString)UI.FormatAsLink("Futuristic Door", "BuildablePOIInternalDoor");
                    public static LocString DESC = (LocString)("A door from the future! (or the past)");
                    public static LocString EFFECT = (LocString)("Buildable form of the POI internal door.");
                }

                public static class BUILDABLEPOISECURITYDOOR
                {
                    public static LocString NAME = (LocString)UI.FormatAsLink("Security Door", "BuildablePOISecurityDoor");
                    public static LocString DESC = (LocString)("A secure door?!?");
                    public static LocString EFFECT = (LocString)("Buildable form of the POI Security door.");
                }
            }
        }
    }
}
