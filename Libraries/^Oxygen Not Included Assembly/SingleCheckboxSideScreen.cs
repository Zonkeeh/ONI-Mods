// Decompiled with JetBrains decompiler
// Type: SingleCheckboxSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SingleCheckboxSideScreen : SideScreenContent
{
  public KToggle toggle;
  public KImage toggleCheckMark;
  public LocText label;
  private ICheckboxControl target;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.toggle.onValueChanged += new System.Action<bool>(this.OnValueChanged);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    if (target.GetComponent<ICheckboxControl>() == null)
      return target.GetSMI<ICheckboxControl>() != null;
    return true;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object provided was null");
    }
    else
    {
      this.target = target.GetComponent<ICheckboxControl>();
      if (this.target == null)
        this.target = target.GetSMI<ICheckboxControl>();
      if (this.target == null)
      {
        Debug.LogError((object) "The target provided does not have an ICheckboxControl component");
      }
      else
      {
        this.label.text = this.target.CheckboxLabel;
        this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
        this.titleKey = this.target.CheckboxTitleKey;
        this.toggle.isOn = this.target.GetCheckboxValue();
        this.toggleCheckMark.enabled = this.toggle.isOn;
      }
    }
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    this.target = (ICheckboxControl) null;
  }

  private void OnValueChanged(bool value)
  {
    this.target.SetCheckboxValue(value);
    this.toggleCheckMark.enabled = value;
  }
}
