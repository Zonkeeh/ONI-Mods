// Decompiled with JetBrains decompiler
// Type: ConfirmDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialogScreen : KModalScreen
{
  private System.Action confirmAction;
  private System.Action cancelAction;
  private System.Action configurableAction;
  public System.Action onDeactivateCB;
  [SerializeField]
  private GameObject confirmButton;
  [SerializeField]
  private GameObject cancelButton;
  [SerializeField]
  private GameObject configurableButton;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText popupMessage;
  [SerializeField]
  private Image image;
  [SerializeField]
  private GameObject fadeBG;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
  }

  public override bool IsModal()
  {
    return true;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.OnSelect_CANCEL();
    else
      base.OnKeyDown(e);
  }

  public void PopupConfirmDialog(
    string text,
    System.Action on_confirm,
    System.Action on_cancel,
    string configurable_text = null,
    System.Action on_configurable_clicked = null,
    string title_text = null,
    string confirm_text = null,
    string cancel_text = null,
    Sprite image_sprite = null,
    bool activateBlackBackground = true)
  {
    while ((UnityEngine.Object) this.transform.parent.GetComponent<Canvas>() == (UnityEngine.Object) null && (UnityEngine.Object) this.transform.parent.parent != (UnityEngine.Object) null)
      this.transform.SetParent(this.transform.parent.parent);
    this.transform.SetAsLastSibling();
    this.fadeBG.SetActive(activateBlackBackground);
    this.confirmAction = on_confirm;
    this.cancelAction = on_cancel;
    this.configurableAction = on_configurable_clicked;
    int num1 = 0;
    if (this.confirmAction != null)
      ++num1;
    if (this.cancelAction != null)
      ++num1;
    if (this.configurableAction != null)
    {
      int num2 = num1 + 1;
    }
    this.confirmButton.GetComponentInChildren<LocText>().text = confirm_text != null ? confirm_text : STRINGS.UI.CONFIRMDIALOG.OK.text;
    this.cancelButton.GetComponentInChildren<LocText>().text = cancel_text != null ? cancel_text : STRINGS.UI.CONFIRMDIALOG.CANCEL.text;
    this.confirmButton.GetComponent<KButton>().onClick += new System.Action(this.OnSelect_OK);
    this.cancelButton.GetComponent<KButton>().onClick += new System.Action(this.OnSelect_CANCEL);
    this.configurableButton.GetComponent<KButton>().onClick += new System.Action(this.OnSelect_third);
    this.cancelButton.SetActive(on_cancel != null);
    if ((UnityEngine.Object) this.configurableButton != (UnityEngine.Object) null)
    {
      this.configurableButton.SetActive(this.configurableAction != null);
      if (configurable_text != null)
        this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
    }
    if ((UnityEngine.Object) image_sprite != (UnityEngine.Object) null)
    {
      this.image.sprite = image_sprite;
      this.image.gameObject.SetActive(true);
    }
    if (title_text != null)
      this.titleText.text = title_text;
    this.popupMessage.text = text;
  }

  public void OnSelect_OK()
  {
    this.Deactivate();
    if (this.confirmAction == null)
      return;
    this.confirmAction();
  }

  public void OnSelect_CANCEL()
  {
    this.Deactivate();
    if (this.cancelAction == null)
      return;
    this.cancelAction();
  }

  public void OnSelect_third()
  {
    this.Deactivate();
    if (this.configurableAction == null)
      return;
    this.configurableAction();
  }

  protected override void OnDeactivate()
  {
    if (this.onDeactivateCB != null)
      this.onDeactivateCB();
    base.OnDeactivate();
  }
}
