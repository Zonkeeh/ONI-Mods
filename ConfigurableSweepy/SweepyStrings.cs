using Harmony;
using System;
using UnityEngine;
using Klei.AI;

namespace ConfigurableSweepy
{
    public static class SweepyStrings
    {
        public static string CustomTraitName = "SweepBotCustomTrait";

        public static StatusItem CustomDebugStatus;
        public static StatusItem BatteryLevelStatus;
        public static StatusItem StorageStatus;

        public static string CustomDebugStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.SWEEPYDEBUG";
        public static LocString CustomDebugStatusName = (LocString)"<b>Sweepy Debug:</b> {Settings}";
        public static LocString CustomDebugStatusTooltip = (LocString)("{Settings}");

        public static string BatteryLevelStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.SWEEPYBATTERYLEVEL";
        public static LocString BatteryLevelStatusName = (LocString)"Battery: {Percentage}";
        public static LocString BatteryLevelStatusTooltip = (LocString)("Battery level is at <b>{Remaining}</b> of a total <b>{Total}</b> ({Percentage}).");

        public static string StorageStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.SWEEPYSTORAGECAPACITY";
        public static LocString StorageStatusName = (LocString)"Stored: {Stored} / {Capacity} {Units}";
        public static LocString StorageStatusTooltip = (LocString)("Storage is filled with <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>");

        public static string SideScreenKey = (LocString)"STRINGS.UI.UISIDESCREENS.SWEEPBOTSTATION_SIDE_SCREEN";
        public static LocString SideScreenTitleText = (LocString)"New Sweepy Configurator";

        public static string MoveSpeedTitleKey = (LocString)SideScreenKey + ".MOVESPEEDLABEL";
        public static LocString MoveSpeedTitleName = (LocString)"Move Speed:";
        public static LocString MoveSpeedTitleTooltip = (LocString)("Determines how fast, in <b>tiles per second</b>, the sweep bot will move.");

        public static string ProbingRadiusTitleKey = (LocString)SideScreenKey + ".PROBINGRADIUSLABEL";
        public static LocString ProbingRadiusTitleName = (LocString)"Probing Radius:";
        public static LocString ProbingRadiusTitleTooltip = (LocString)("Determines how far away, in <b>tiles</b>, the sweep bot will venture from it's base station.");

        public static string FindSweepyButtonKey = (LocString)SideScreenKey + ".FINDSWEEPYBUTTON";
        public static LocString FindSweepyButtonText = (LocString)"Find Sweepy";
        public static LocString FindSweepyButtonTooltip = (LocString)("On press, camera will focus on this station's sweepy.");

        public static string ResetSweepyButtonKey = (LocString)SideScreenKey + ".RESETSWEEPYBUTTON";
        public static LocString ResetSweepyButtonText = (LocString)"Reset Sweepy";
        public static LocString ResetSweepyButtonTooltip = (LocString)("On press, will reset the sweepy stats back to their base/default values.");

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                AddStrings();
            }
        }

        private static void AddStrings()
        {
            Strings.Add(CustomDebugStatusKey + ".NAME", CustomDebugStatusName);
            Strings.Add(CustomDebugStatusKey + ".TOOLTIP", CustomDebugStatusTooltip);

            Strings.Add(BatteryLevelStatusKey + ".NAME", BatteryLevelStatusName);
            Strings.Add(BatteryLevelStatusKey + ".TOOLTIP", BatteryLevelStatusTooltip);

            Strings.Add(StorageStatusKey + ".NAME", StorageStatusName);
            Strings.Add(StorageStatusKey + ".TOOLTIP", StorageStatusTooltip);

            Strings.Add(SideScreenKey + ".TITLE", SideScreenTitleText);

            Strings.Add(MoveSpeedTitleKey + ".NAME", MoveSpeedTitleName);
            Strings.Add(MoveSpeedTitleKey + ".TOOLTIP", MoveSpeedTitleTooltip);

            Strings.Add(ProbingRadiusTitleKey + ".NAME", ProbingRadiusTitleName);
            Strings.Add(ProbingRadiusTitleKey + ".TOOLTIP", ProbingRadiusTitleTooltip);

            Strings.Add(FindSweepyButtonKey + ".TEXT", FindSweepyButtonText);
            Strings.Add(FindSweepyButtonKey + ".TOOLTIP", FindSweepyButtonTooltip);

            Strings.Add(ResetSweepyButtonKey + ".TEXT", ResetSweepyButtonText);
            Strings.Add(ResetSweepyButtonKey + ".TOOLTIP", ResetSweepyButtonTooltip);

            CustomDebugStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "SweepyDebug",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID,
                true,
                129022
            }).GetValue();

            CustomDebugStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                GameObject sweepBot = (GameObject)data;
                if ((UnityEngine.Object)sweepBot == (UnityEngine.Object)null)
                    return str;

                Navigator nav = sweepBot.GetComponent<Navigator>();
                if ((UnityEngine.Object)nav == (UnityEngine.Object)null)
                    return str;

                int distanceTravelled = 0;
                nav.distanceTravelledByNavType.TryGetValue(nav.CurrentNavType, out distanceTravelled);
                float probingRadius = nav.maxProbingRadius;
                float moveSpeed = nav.defaultSpeed;
                float currentBatteryDelta = sweepBot.GetAmounts().Get(Db.Get().Amounts.InternalBattery.Id).GetDelta();

                str = str.Replace("{Settings}", "\n\tMove Speed: " + moveSpeed + "\n\tMax Distance From Station: " + probingRadius + "\n\tBattery Delta: " + currentBatteryDelta + "\n\tDistance Travelled: " + distanceTravelled);

                return str;
            });


            BatteryLevelStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "SweepyBatteryLevel",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID,
                true,
                129022
            }).GetValue();

            BatteryLevelStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                GameObject sweepBot = (GameObject)data;
                if ((UnityEngine.Object)sweepBot == (UnityEngine.Object)null)
                    return str;
                float currentLevel = sweepBot.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id);
                float maxBattery = sweepBot.GetAmounts().Get(Db.Get().Amounts.InternalBattery.Id).GetMax();

                int percentageLevel = Mathf.RoundToInt(currentLevel/maxBattery*100);

                str = str.Replace("{Percentage}", percentageLevel + "%");
                str = str.Replace("{Remaining}", currentLevel.ToString());
                str = str.Replace("{Total}", maxBattery.ToString());

                return str;
            });

            StorageStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "SweepyStorageCapacity",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID,
                true,
                129022
            }).GetValue();

            StorageStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
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
