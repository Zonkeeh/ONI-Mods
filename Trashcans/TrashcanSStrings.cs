using Harmony;
using STRINGS;
using System;
using UnityEngine;

namespace Trashcans
{
    public static class TrashcansStrings
    {
        public static StatusItem TrashcanStatus;
        public static StatusItem ReservoirCapacityStatus;

        public static string GasTrashcanID = "GasTrashcan";
        public static string GasTrashcanName = "Gas Trashcan";
        public static string GasTrashcanEffect = "Removes trash " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " from the meteor.";
        public static string GasTrashcanDescription = "Cannot empty when disabled but it can drop it's contents at any time.";

        public static string LiquidTrashcanID = "LiquidTrashcan";
        public static string LiquidTrashcanName = "Liquid Trashcan";
        public static string LiquidTrashcanEffect = "Removes trash " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " from the meteor.";
        public static string LiquidTrashcanDescription = "Cannot empty when disabled but it can drop it's contents at any time.";

        public static string SolidTrashcanID = "SolidTrashcan";
        public static string SolidTrashcanName = "Solid Trashcan";
        public static string SolidTrashcanEffect = "Removes trash Items " + UI.FormatAsLink("(Solids)", "ELEMENTS_SOLID") + " from the meteor.";
        public static string SolidTrashcanDescription = "Cannot empty when disabled but it can drop it's contents at any time.";

        public static string TrashcanSideScreenPath = "STRINGS.UI.UISIDESCREENS.TRASHCAN_SIDE_SCREEN";

        public static LocString SideButtonKey = (LocString)TrashcanSideScreenPath + ".SIDE_BUTTON";
        public static LocString SideButtonTitle = (LocString)"Trashcan";
        public static LocString SideButtonGasTitle = (LocString)"Gas Trashcan";
        public static LocString SideButtonLiquidTitle = (LocString)"Liquid Trashcan";
        public static LocString SideButtonSolidTitle = (LocString)"Solid Trashcan";
        public static LocString SideButtonText = (LocString)"Empty Trash";
        public static LocString SideButtonStatus = (LocString)"";

        public static LocString UserMenuButtonKey = (LocString)TrashcanSideScreenPath + ".USERMENU_BUTTON";
        public static LocString UserMenuButtonTitle = (LocString)"Drop Contents";
        public static LocString UserMenuButtonTooltip = (LocString)"On click, drop all stored contents.";

        public static LocString CheckboxKey = (LocString)TrashcanSideScreenPath + ".CHECKBOX";
        public static LocString CheckboxTitle = (LocString)"Trashcan";
        public static LocString CheckboxLabel = (LocString)"Auto-Trash";
        public static LocString CheckboxTooltip = (LocString)"When selected, will automatically empty after a given time period.";

        public static LocString SliderKey = (LocString)TrashcanSideScreenPath + ".SLIDER";
        public static LocString SliderTitle = (LocString)"Trashcan";
        public static LocString SliderTooltip = (LocString)"Changes the time (in seconds) between each auto-trash.";
        public static LocString SliderUnits = (LocString)"s";


        public static string StatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.TRASHCAN";
        public static LocString StatusName = (LocString)"Auto-Trash: <b>{Remaining}</b>";
        public static LocString StatusTooltip = (LocString)("Trashcan will {Remaining}{Total}.");

        public static string ReservoirStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.RESERVOIRCAPACITY";
        public static LocString ReservoirStatusName = (LocString)"Filled: {Stored} / {Capacity} {Units}";
        public static LocString ReservoirStatusTooltip = (LocString)("Reservoir is filled with <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>");

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                AddStrings();
            }
        }

        private static void AddStrings()
        {
            Strings.Add(SideButtonKey + ".TITLE", SideButtonTitle);
            Strings.Add(SideButtonKey + ".TEXT", SideButtonText);
            Strings.Add(SideButtonKey + ".STATUS", SideButtonStatus);

            Strings.Add(UserMenuButtonKey + ".TITLE", UserMenuButtonTitle);
            Strings.Add(UserMenuButtonKey + ".TOOLTIP", UserMenuButtonTooltip);

            Strings.Add(CheckboxKey + ".TITLE", CheckboxTitle);
            Strings.Add(CheckboxKey + ".LABEL", CheckboxLabel);
            Strings.Add(CheckboxKey + ".TOOLTIP", CheckboxTooltip);

            Strings.Add(SliderKey + ".TITLE", SliderTitle);
            Strings.Add(SliderKey + ".TOOLTIP", SliderTooltip);
            Strings.Add(SliderKey + ".UNITS", SliderUnits);

            Strings.Add(StatusKey + ".NAME", StatusName);
            Strings.Add(StatusKey + ".TOOLTIP", StatusTooltip);

            Strings.Add(ReservoirStatusKey + ".NAME", ReservoirStatusName);
            Strings.Add(ReservoirStatusKey + ".TOOLTIP", ReservoirStatusTooltip);


            TrashcansStrings.TrashcanStatus = (StatusItem) Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] { 
                "Trashcan", 
                "BUILDING", 
                string.Empty, 
                StatusItem.IconType.Info, 
                NotificationType.Neutral,
                false, 
                OverlayModes.None.ID, 
                true, 
                129022 
            }).GetValue();

            TrashcansStrings.TrashcanStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                Trashcan trashcan = (Trashcan)data;
                if ((UnityEngine.Object)trashcan == (UnityEngine.Object)null)
                    return str;

                if (str.Contains("{Total}"))
                {
                    if (!trashcan.AutoTrash)
                    {
                        str = str.Replace("{Remaining}", "<b>not Empty</b>");
                        str = str.Replace("{Total}", "");
                    }
                    else
                    {
                        str = str.Replace("{Remaining}", "empty in <b>" + (trashcan.WaitTime - trashcan.CurrentTime).ToString() + "s</b> ");
                        str = str.Replace("{Total}", "from an increment of <b>" + trashcan.WaitTime + "s</b>");
                    }
                        
                }
                else if (!trashcan.AutoTrash)
                    str = str.Replace("{Remaining}", "Disabled");
                else
                    str = str.Replace("{Remaining}", (trashcan.WaitTime - trashcan.CurrentTime).ToString() + "s");                 

                return str;
            });

            TrashcansStrings.ReservoirCapacityStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "ReservoirCapacity",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID,
                true,
                129022
            }).GetValue();

            TrashcansStrings.ReservoirCapacityStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                Storage storage = (Storage)data;
                float amountStored = storage.MassStored();
                float capacity = storage.capacityKg;
                string stored = Util.FormatWholeNumber((double)amountStored <= (double)capacity - (double)storage.storageFullMargin || (double)amountStored >= (double)capacity ? Mathf.Floor(amountStored) : capacity);
                str = str.Replace("{Stored}", stored);
                str = str.Replace("{Capacity}", Util.FormatWholeNumber(capacity));
                str = str.Replace("{Units}", " Kg");
                return str;
            });
        }
    }
}
