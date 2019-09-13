// Decompiled with JetBrains decompiler
// Type: SingleButtonSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SingleButtonSideScreen : SideScreenContent
{
  private ISidescreenButtonControl target;
  public LocText statusText;
  public KButton button;
  public LocText buttonLabel;

  protected override void OnPrefabInit()
  {
    this.button.onClick += new System.Action(this.OnButonClick);
  }

  private void OnButonClick()
  {
    if (this.target == null)
      return;
    this.target.OnSidescreenButtonPressed();
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<ISidescreenButtonControl>() != null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<ISidescreenButtonControl>();
      if (this.target == null)
        DebugUtil.LogErrorArgs((object) "The gameObject received does not contain a", (object) typeof (ISidescreenButtonControl).ToString());
      else
        this.Refresh();
    }
  }

  private void Refresh()
  {
    this.titleKey = this.target.SidescreenTitleKey;
    this.statusText.text = this.target.SidescreenStatusMessage;
    this.buttonLabel.text = this.target.SidescreenButtonText;
  }
}
