using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using Zolibrary.Logging;
using Harmony;

namespace Zolibrary.Utilities
{
    public static class UIUtils
    {
        public static void DebugObjectHierarchy(this GameObject item)
        {
            string info = "null";
            if (item != null)
            {
                var result = new StringBuilder(256);
                do
                {
                    result.Append("- ");
                    result.Append(item.name ?? "Unnamed");
                    item = item.transform?.parent?.gameObject;
                    if (item != null)
                        result.AppendLine();
                } while (item != null);
                info = result.ToString();
            }
            LogManager.LogWarning("Object Tree:" + Environment.NewLine + info);
        }

        public static ConfirmDialogScreen ShowConfirmDialog(string title, string message, System.Action confirm_action, System.Action cancel_action, bool show_black_background)
        {
            ConfirmDialogScreen confirmDialogScreen = (ConfirmDialogScreen) KScreenManager.Instance.StartScreen
                (ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, PauseScreen.Instance.transform.parent.gameObject);
            confirmDialogScreen.PopupConfirmDialog(message, confirm_action, cancel_action, message, null, title, UI.FRONTEND.DONE_BUTTON.text, null, null, show_black_background);
            return confirmDialogScreen;
        }

        public static CustomizableDialogScreen ShowCustomDialog(string title, string message, CustomisableButtonOption[] buttonList)
        {
            CustomizableDialogScreen customDialogScreen = (CustomizableDialogScreen)KScreenManager.Instance.StartScreen
                (ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, PauseScreen.Instance.transform.parent.gameObject);

            foreach (CustomisableButtonOption opt in buttonList)
                customDialogScreen.AddOption(opt.text, opt.buttonAction);

            customDialogScreen.SetBackgroundActive(true);
            customDialogScreen.SetHasFocus(true);
            customDialogScreen.PopupConfirmDialog(message, title, null);
            
            return customDialogScreen;
        }
    }

    public class CustomisableButtonOption
    {
        public string text;
        public System.Action buttonAction;

        public CustomisableButtonOption(string text, System.Action buttonAction)
        {
            this.text = text;
            this.buttonAction = buttonAction;
        }
    }
}
