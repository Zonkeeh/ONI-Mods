using Harmony;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace AutosaveDragFix
{
    public class AutosavePatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Autosave Drag Fix", "1.0.7");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class Game_OnPrefabInit_Patch
        {
            internal static void Postfix(Game __instance)
            {
                __instance.gameObject.AddComponent<NotificationAnnouncer>();
            }
        }

        [HarmonyPatch(typeof(Game), "DelayedSave")]
        public static class Save_Patch
        {
            public static void Prefix(bool isAutoSave)
            {
                if (isAutoSave && DragCheck())
                {
                    bool[] keyCode = { false, false, false, true };
                    KButtonEvent tempEvent = new KButtonEvent(new KInputController(false), InputEventType.KeyUp, keyCode);

                    if (AutosaveConfigChecker.ResetToolOnAutosave)
                        PlayerController.Instance.ActiveTool.DeactivateTool();
                    else
                    {
                        InterfaceTool active = PlayerController.Instance.ActiveTool;
                        PlayerController.Instance.ActiveTool.DeactivateTool();
                        PlayerController.Instance.ActivateTool(active);
                    }   
                }
            }

            private static bool DragCheck()
            {
                if (!AutosaveConfigChecker.IgnoreIfOnlyDragging)
                    return true;
                else if (PlayerController.Instance.IsDragging())
                    return true;
                else
                    return false;
            }
        }

        private class NotificationAnnouncer : KMonoBehaviour, ISim1000ms
        {
            private bool hasActivated = false;
            private string annouceMessage;

            public NotificationAnnouncer()
            {
                this.annouceMessage = "The game will autosave in " + AutosaveConfigChecker.WarningSecondsBeforeAutosave + " seconds.";

                if (AutosaveConfigChecker.ResetToolOnAutosave)
                    this.annouceMessage += "\nYour tool will also be reset when the autosave is initiated.";
            }

            public void Sim1000ms(float dt)
            {
                if (AutosaveConfigChecker.SendAutosaveWarning && !hasActivated 
                    && ((GameClock.Instance.GetCycle() + 1) % SaveGame.Instance.AutoSaveCycleInterval) == 0 
                    && GameClock.Instance.GetTimeSinceStartOfCycle() >= CalculateSeconds())
                {
                    ConfirmDialogScreen dialog = UIUtils.ShowConfirmDialog("Announcer", this.annouceMessage, null, null);
                    hasActivated = true;
                }
                else if (AutosaveConfigChecker.SendAutosaveWarning && hasActivated && GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f)
                    hasActivated = false;
            }

            private float CalculateSeconds()
            {
                return (600 - AutosaveConfigChecker.WarningSecondsBeforeAutosave);
            }
        }
    }
}
