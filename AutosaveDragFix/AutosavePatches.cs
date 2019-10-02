using System.Collections;
using Database;
using Harmony;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;
using System;

namespace CompostOutputsFertilizer
{
    public class AutosavePatches
    {
        private static Config config;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Autosave Drag Fix", "1.0.4");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                AutosavePatches.config = cm.LoadConfig(new Config());
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
        public static class Game_DelayedSave_Patch
        {

            public static void Prefix()
            {
                if (DragCheck())
                {
                    bool[] keyCode = { false, false, false, true };
                    KButtonEvent tempEvent = new KButtonEvent(new KInputController(false), InputEventType.KeyUp, keyCode);

                    if (!config.IgnoreToolQueue && config.ResetToolOnAutosave)
                    {
                        PlayerController.Instance.OnKeyUp(tempEvent);
                        PlayerController.Instance.ActiveTool.DeactivateTool();
                    }
                    else if (config.ResetToolOnAutosave)
                        PlayerController.Instance.ActiveTool.DeactivateTool();
                    else if (!config.IgnoreToolQueue)
                        PlayerController.Instance.OnKeyUp(tempEvent);
                    else
                    {
                        InterfaceTool active = PlayerController.Instance.ActiveTool;
                        PlayerController.Instance.ActivateTool(PlayerController.Instance.tools[0]);
                        PlayerController.Instance.ActivateTool(active);
                    }   
                }
            }

            private static bool DragCheck()
            {
                if (AutosavePatches.config.IgnoreIfOnlyDragging)
                    return true;
                else if (PlayerController.Instance.IsDragging())
                    return true;
                else
                    return false;
            }
        }

        private class NotificationAnnouncer : KMonoBehaviour, ISim1000ms
        {
            private int warningTime;
            private bool hasActivated = false;
            private string annouceMessage;

            public NotificationAnnouncer()
            {
                CheckConfigTime();

                this.annouceMessage = "The game will autosave in " + this.warningTime + " seconds.";

                if (config.ResetToolOnAutosave)
                    this.annouceMessage += "\nYour tool will also be reset when the autosave is initiated.";
            }

            public void Sim1000ms(float dt)
            {
                if (config.SendAutosaveWarning && !hasActivated 
                    && ((GameClock.Instance.GetCycle() + 1) % SaveGame.Instance.AutoSaveCycleInterval) == 0 
                    && GameClock.Instance.GetTimeSinceStartOfCycle() >= CalculateSeconds())
                {
                    ConfirmDialogScreen dialog = UIUtils.ShowConfirmDialog("Announcer", this.annouceMessage, null, null, true);
                    hasActivated = true;
                }
                else if (config.SendAutosaveWarning && hasActivated && GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f)
                    hasActivated = false;
            }

            private void CheckConfigTime()
            {
                if (config.WarningSecondsBeforeAutosave > 60 || config.WarningSecondsBeforeAutosave <= 5)
                {
                    this.warningTime = 10;
                    LogManager.LogError("Config file Warning Seconds invalid. Value should be between 5 and 60 seconds, therefore defaulting to 10 seconds. \nConfig Value:" + config.WarningSecondsBeforeAutosave);
                }
                else
                    this.warningTime = config.WarningSecondsBeforeAutosave;
            }

            private float CalculateSeconds()
            {
                return (600 - this.warningTime);
            }
        }
    }
}
