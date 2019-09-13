using System;
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

namespace AutosaveDragFix
{
    public class AutosavePatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Autosave Drag Fix", "1.0.3");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(Game), "DelayedSave")]
        public static class Game_DelayedSave_Patch
        {
            public static void Prefix()
            {
                if (PlayerController.Instance.IsDragging())
                {
                    bool[] keyCode = { false, false, false, true };
                    KButtonEvent tempEvent = new KButtonEvent(new KInputController(false), InputEventType.KeyUp, keyCode);
                    PlayerController.Instance.OnKeyUp(tempEvent);
                }
            }
        }
    }
}
