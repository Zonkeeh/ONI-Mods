using Harmony;

namespace DestructibleFeatures
{
    public class DestructibleStrings
    {

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                DestructibleStrings.AddStrings();
            }
        }

        public static StatusItem DuplicantStatus;
        public static StatusItem AwaitingDeconstruct;

        public static string TitleKey = "STRINGS.UI.UISIDESCREENS.DESTRUCTIBLE_SIDE_SCREEN.TITLE";
        public static string Title = "Destroy Natural Feature";
        public static string StatusKey = "STRINGS.UI.UISIDESCREENS.DESTRUCTIBLE_SIDE_SCREEN.STATUS";
        public static string Status = "Send a worker to destroy this feature.\n\nDeconstructing a natural feature takes time, a duplicant and will not drop any resources.";
        public static string ButtonKey = "STRINGS.UI.UISIDESCREENS.DESTRUCTIBLE_SIDE_SCREEN.BUTTON";
        public static string Button = "DESTROY";
        public static string PendingStatusKey = "STRINGS.UI.UISIDESCREENS.DESTRUCTIBLE_SIDE_SCREEN.PENDINGSTATUS";
        public static string PendingStatus = "A worker is in the process of destroying this feature.";
        public static string PendingButtonKey = "STRINGS.UI.UISIDESCREENS.DESTRUCTIBLE_SIDE_SCREEN.PENDINGBUTTON";
        public static string PendingButton = "CANCEL DESTRUCTION";

        public static string DuplicantStatusKey = "STRINGS.DUPLICANTS.STATUSITEMS.GEYSERDECONSTRUCTING";
        public static string DuplicantStatusName = "Deconstructing Feature";
        public static string DuplicantStatusTooltip = "This worker is attempting to remove a feature.";
        public static string GeyserStatusKey = "STRINGS.MISC.STATUSITEMS.AWAITINGDECONSTRUCT";
        public static string GeyserStatusName = "Deconstruction Pending";
        public static string GeyserStatusTooltip = "A worker is in the process of removing this feature.";

        private static void AddStrings()
        {
            Strings.Add(DestructibleStrings.TitleKey, DestructibleStrings.Title);
            Strings.Add(DestructibleStrings.StatusKey, DestructibleStrings.Status);
            Strings.Add(DestructibleStrings.ButtonKey, DestructibleStrings.Button);
            Strings.Add(DestructibleStrings.PendingStatusKey, DestructibleStrings.PendingStatus);
            Strings.Add(DestructibleStrings.PendingButtonKey, DestructibleStrings.PendingButton);
            Strings.Add(DestructibleStrings.DuplicantStatusKey + ".NAME", DestructibleStrings.DuplicantStatusName);
            Strings.Add(DestructibleStrings.DuplicantStatusKey + ".TOOLTIP", DestructibleStrings.DuplicantStatusTooltip);
            Strings.Add(DestructibleStrings.GeyserStatusKey + ".NAME", DestructibleStrings.GeyserStatusName);
            Strings.Add(DestructibleStrings.GeyserStatusKey + ".TOOLTIP", DestructibleStrings.GeyserStatusTooltip);

            DestructibleStrings.DuplicantStatus = 
                (StatusItem)Traverse.Create(Db.Get().DuplicantStatusItems).Method("CreateStatusItem", new object[] {
                    "GeyserDeconstructing",
                    "DUPLICANTS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    true,
                    2 }
                ).GetValue();

            DestructibleStrings.AwaitingDeconstruct = 
                (StatusItem)Traverse.Create(Db.Get().MiscStatusItems).Method("CreateStatusItem", new object[] {
                    "AwaitingDeconstruct",
                    "MISC",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    true,
                    129022 }
                ).GetValue();
        }
    }
}
