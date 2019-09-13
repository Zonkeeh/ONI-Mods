// Decompiled with JetBrains decompiler
// Type: KPopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KPopupMenu : KScreen
{
  [SerializeField]
  private KButtonMenu buttonMenu;
  private KButtonMenu.ButtonInfo[] Buttons;
  public System.Action<string, int> OnSelect;

  public void SetOptions(IList<string> options)
  {
    List<KButtonMenu.ButtonInfo> buttonInfoList = new List<KButtonMenu.ButtonInfo>();
    for (int index1 = 0; index1 < options.Count; ++index1)
    {
      int index = index1;
      string option = options[index1];
      buttonInfoList.Add(new KButtonMenu.ButtonInfo(option, Action.NumActions, (UnityAction) (() => this.SelectOption(option, index)), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null));
    }
    this.Buttons = buttonInfoList.ToArray();
  }

  public void OnClick()
  {
    if (this.Buttons == null)
      return;
    if (this.gameObject.activeSelf)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.buttonMenu.SetButtons((IList<KButtonMenu.ButtonInfo>) this.Buttons);
      this.buttonMenu.RefreshButtons();
      this.gameObject.SetActive(true);
    }
  }

  public void SelectOption(string option, int index)
  {
    if (this.OnSelect != null)
      this.OnSelect(option, index);
    this.gameObject.SetActive(false);
  }

  public IList<KButtonMenu.ButtonInfo> GetButtons()
  {
    return (IList<KButtonMenu.ButtonInfo>) this.Buttons;
  }
}
