// Decompiled with JetBrains decompiler
// Type: ReportErrorDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class ReportErrorDialog : MonoBehaviour
{
  public static string MOST_RECENT_SAVEFILE;
  private System.Action confirmAction;
  private System.Action quitAction;
  private System.Action continueAction;
  public TMP_InputField messageInputField;
  public GameObject referenceMessage;
  [SerializeField]
  private KButton submitButton;
  [SerializeField]
  private KButton quitButton;
  [SerializeField]
  private KButton continueGameButton;
  [SerializeField]
  private LocText CrashLabel;
  [SerializeField]
  private LocText VCCrashLabel;
  [SerializeField]
  private UnityEngine.UI.Button VCLinkButton;
  [SerializeField]
  private GameObject InfoBox;
  [SerializeField]
  private GameObject uploadSaveDialog;
  [SerializeField]
  private KButton uploadSaveButton;
  [SerializeField]
  private KButton skipUploadSaveButton;
  [SerializeField]
  private LocText saveFileInfoLabel;
  public static bool hasCrash;

  private void Start()
  {
    ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
    if ((bool) ((UnityEngine.Object) SpeedControlScreen.Instance))
      SpeedControlScreen.Instance.Pause(false);
    if ((bool) ((UnityEngine.Object) KScreenManager.Instance))
      KScreenManager.Instance.DisableInput(true);
    this.continueGameButton.onClick += new System.Action(this.OnSelect_CONTINUE);
    this.continueGameButton.gameObject.SetActive(!KCrashReporter.terminateOnError);
    this.submitButton.onClick += new System.Action(this.OnSelect_SUBMIT);
    this.quitButton.onClick += new System.Action(this.OnSelect_QUIT);
    this.uploadSaveButton.onClick += new System.Action(this.OnSelect_UPLOADSAVE);
    this.skipUploadSaveButton.onClick += new System.Action(this.OnSelect_SKIPUPLOADSAVE);
    this.messageInputField.text = (string) STRINGS.UI.CRASHSCREEN.BODY;
    ReportErrorDialog.hasCrash = true;
  }

  private void Update()
  {
    Debug.developerConsoleVisible = false;
  }

  private void OnDestroy()
  {
    if (KCrashReporter.terminateOnError)
      App.Quit();
    if (!(bool) ((UnityEngine.Object) KScreenManager.Instance))
      return;
    KScreenManager.Instance.DisableInput(false);
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Escape))
      return;
    this.OnSelect_QUIT();
  }

  public void PopupConfirmDialog(System.Action onConfirm, System.Action onQuit, System.Action onContinue)
  {
    this.confirmAction = onConfirm;
    this.quitAction = onQuit;
    this.continueAction = onContinue;
    this.continueGameButton.gameObject.SetActive(this.continueAction != null);
    this.VCCrashLabel.gameObject.SetActive(false);
    this.VCLinkButton.gameObject.SetActive(false);
    this.quitButton.gameObject.SetActive(onQuit != null);
  }

  public void OnSelect_SUBMIT()
  {
    this.submitButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.CRASHSCREEN.REPORTING;
    this.submitButton.GetComponent<KButton>().isInteractable = false;
    this.StartCoroutine(this.WaitForUIUpdateBeforeReporting());
  }

  [DebuggerHidden]
  private IEnumerator WaitForUIUpdateBeforeReporting()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ReportErrorDialog.\u003CWaitForUIUpdateBeforeReporting\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public void OnSelect_QUIT()
  {
    if (this.quitAction == null)
      return;
    this.quitAction();
  }

  public void OnSelect_CONTINUE()
  {
    ReportErrorDialog.hasCrash = false;
    if (this.continueAction == null)
      return;
    this.continueAction();
  }

  public void OpenRefMessage()
  {
    this.submitButton.gameObject.SetActive(false);
    this.referenceMessage.SetActive(true);
  }

  public string UserMessage()
  {
    return this.messageInputField.text;
  }

  private void OnSelect_UPLOADSAVE()
  {
    this.uploadSaveDialog.SetActive(false);
    KCrashReporter.MOST_RECENT_SAVEFILE = ReportErrorDialog.MOST_RECENT_SAVEFILE;
    this.Submit();
  }

  private void OnSelect_SKIPUPLOADSAVE()
  {
    this.uploadSaveDialog.SetActive(false);
    KCrashReporter.MOST_RECENT_SAVEFILE = (string) null;
    this.Submit();
  }

  private void Submit()
  {
    this.confirmAction();
    this.OpenRefMessage();
  }
}
