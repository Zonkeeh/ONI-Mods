// Decompiled with JetBrains decompiler
// Type: CustomizableDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizableDialogScreen : KModalScreen
{
  public System.Action onDeactivateCB;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private GameObject buttonPanel;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText popupMessage;
  [SerializeField]
  private Image image;
  private List<CustomizableDialogScreen.Button> buttons;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
    this.buttons = new List<CustomizableDialogScreen.Button>();
  }

  public override bool IsModal()
  {
    return true;
  }

  public void AddOption(string text, System.Action action)
  {
    GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
    this.buttons.Add(new CustomizableDialogScreen.Button()
    {
      label = text,
      action = action,
      gameObject = gameObject
    });
  }

  public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
  {
    foreach (CustomizableDialogScreen.Button button in this.buttons)
    {
      button.gameObject.GetComponentInChildren<LocText>().text = button.label;
      button.gameObject.GetComponent<KButton>().onClick += button.action;
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

  protected override void OnDeactivate()
  {
    if (this.onDeactivateCB != null)
      this.onDeactivateCB();
    base.OnDeactivate();
  }

  private struct Button
  {
    public System.Action action;
    public GameObject gameObject;
    public string label;
  }
}
