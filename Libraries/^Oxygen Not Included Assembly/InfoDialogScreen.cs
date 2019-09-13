// Decompiled with JetBrains decompiler
// Type: InfoDialogScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class InfoDialogScreen : KModalScreen
{
  [SerializeField]
  private InfoScreenPlainText subHeaderTemplate;
  [SerializeField]
  private InfoScreenPlainText plainTextTemplate;
  [SerializeField]
  private InfoScreenLineItem lineItemTemplate;
  [Space(10f)]
  [SerializeField]
  private LocText header;
  [SerializeField]
  private GameObject contentContainer;
  [SerializeField]
  private GameObject confirmButton;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private GameObject buttonPanel;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.SetActive(false);
    this.confirmButton.GetComponent<KButton>().onClick += new System.Action(this.OnSelect_OK);
  }

  public override bool IsModal()
  {
    return true;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.OnSelect_OK();
    else if (PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.OnSelect_OK();
    else
      base.OnKeyDown(e);
  }

  public void AddOption(string text, System.Action<InfoDialogScreen> action)
  {
    GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
    gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
    gameObject.gameObject.GetComponent<KButton>().onClick += (System.Action) (() => action(this));
  }

  public InfoDialogScreen SetHeader(string header)
  {
    this.header.text = header;
    return this;
  }

  public InfoDialogScreen AddPlainText(string text)
  {
    Util.KInstantiateUI(this.plainTextTemplate.gameObject, this.contentContainer, false).GetComponent<InfoScreenPlainText>().SetText(text);
    return this;
  }

  public InfoDialogScreen AddLineItem(string text, string tooltip)
  {
    InfoScreenLineItem component = Util.KInstantiateUI(this.lineItemTemplate.gameObject, this.contentContainer, false).GetComponent<InfoScreenLineItem>();
    component.SetText(text);
    component.SetTooltip(tooltip);
    return this;
  }

  public InfoDialogScreen AddSubHeader(string text)
  {
    Util.KInstantiateUI(this.subHeaderTemplate.gameObject, this.contentContainer, false).GetComponent<InfoScreenPlainText>().SetText(text);
    return this;
  }

  public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
  {
    for (int index = 0; index < descriptors.Count; ++index)
      this.AddLineItem(descriptors[index].IndentedText(), descriptors[index].tooltipText);
    return this;
  }

  public void OnSelect_OK()
  {
    this.Deactivate();
  }
}
