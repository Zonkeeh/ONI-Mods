// Decompiled with JetBrains decompiler
// Type: TitleBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class TitleBar : KMonoBehaviour
{
  public bool setCameraControllerState = true;
  public LocText titleText;
  public LocText subtextText;
  public GameObject WarningNotification;
  public Text NotificationText;
  public Image NotificationIcon;
  public Sprite techIcon;
  public Sprite materialIcon;
  public TitleBarPortrait portrait;
  public bool userEditable;

  public void SetTitle(string Name)
  {
    this.titleText.text = Name;
  }

  public void SetSubText(string subtext, string tooltip = "")
  {
    this.subtextText.text = subtext;
    this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
  }

  public void SetWarningActve(bool state)
  {
    this.WarningNotification.SetActive(state);
  }

  public void SetWarning(Sprite icon, string label)
  {
    this.SetWarningActve(true);
    this.NotificationIcon.sprite = icon;
    this.NotificationText.text = label;
  }

  public void SetPortrait(GameObject target)
  {
    this.portrait.SetPortrait(target);
  }
}
