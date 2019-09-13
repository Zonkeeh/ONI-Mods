// Decompiled with JetBrains decompiler
// Type: MessageDialogFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MessageDialogFrame : KScreen
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KToggle nextMessageButton;
  [SerializeField]
  private GameObject dontShowAgainElement;
  [SerializeField]
  private MultiToggle dontShowAgainButton;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private RectTransform body;
  private System.Action dontShowAgainDelegate;

  public override float GetSortKey()
  {
    return 9999f;
  }

  protected override void OnActivate()
  {
    this.closeButton.onClick += new System.Action(this.OnClickClose);
    this.nextMessageButton.onClick += new System.Action(this.OnClickNextMessage);
    this.dontShowAgainButton.onClick += new System.Action(this.OnClickDontShowAgain);
    this.dontShowAgainButton.ChangeState(KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) != 1 ? 1 : 0);
    this.Subscribe(Messenger.Instance.gameObject, -599791736, new System.Action<object>(this.OnMessagesChanged));
    this.OnMessagesChanged((object) null);
  }

  protected override void OnDeactivate()
  {
    this.Unsubscribe(Messenger.Instance.gameObject, -599791736, new System.Action<object>(this.OnMessagesChanged));
  }

  private void OnClickClose()
  {
    this.TryDontShowAgain();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void OnClickNextMessage()
  {
    this.TryDontShowAgain();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    NotificationScreen.Instance.OnClickNextMessage();
  }

  private void OnClickDontShowAgain()
  {
    this.dontShowAgainButton.NextState();
    KPlayerPrefs.SetInt("HideTutorial_CheckState", this.dontShowAgainButton.CurrentState != 0 ? 0 : 1);
  }

  private void OnMessagesChanged(object data)
  {
    this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
  }

  public void SetMessage(MessageDialog dialog, Message message)
  {
    this.title.text = message.GetTitle().ToUpper();
    dialog.GetComponent<RectTransform>().SetParent((Transform) this.body.GetComponent<RectTransform>());
    RectTransform component = dialog.GetComponent<RectTransform>();
    component.offsetMin = Vector2.zero;
    component.offsetMax = Vector2.zero;
    dialog.transform.SetLocalPosition(Vector3.zero);
    dialog.SetMessage(message);
    dialog.OnClickAction();
    if (dialog.CanDontShowAgain)
    {
      this.dontShowAgainElement.SetActive(true);
      this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
    }
    else
    {
      this.dontShowAgainElement.SetActive(false);
      this.dontShowAgainDelegate = (System.Action) null;
    }
  }

  private void TryDontShowAgain()
  {
    if (this.dontShowAgainDelegate == null || this.dontShowAgainButton.CurrentState != 0)
      return;
    this.dontShowAgainDelegate();
  }
}
