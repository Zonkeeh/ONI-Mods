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
        public static ConfirmDialogScreen ShowConfirmDialog(string title, string message, System.Action confirm_action, System.Action cancel_action)
        {
            ConfirmDialogScreen confirmDialogScreen = (ConfirmDialogScreen) KScreenManager.Instance.StartScreen
                (ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, PauseScreen.Instance.transform.parent.gameObject);
            confirmDialogScreen.PopupConfirmDialog(
                message, 
                confirm_action, 
                cancel_action, 
                message, 
                null, 
                title, 
                UI.FRONTEND.DONE_BUTTON.text, 
                null, 
                null
            );
            return confirmDialogScreen;
        }

        public static CustomizableDialogScreen ShowCustomDialog(string title, string message, CustomisableButtonOption[] buttonList, GameObject parent)
        {
            CustomizableDialogScreen customDialogScreen = (CustomizableDialogScreen)KScreenManager.Instance.StartScreen
                (ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, PauseScreen.Instance.transform.parent.gameObject);

            //foreach (CustomisableButtonOption opt in buttonList)
                //customDialogScreen.AddOption(opt.text, opt.buttonAction);

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
