// Decompiled with JetBrains decompiler
// Type: AutomatableSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class AutomatableSideScreen : SideScreenContent
{
  public KToggle allowManualToggle;
  public KImage allowManualToggleCheckMark;
  public GameObject content;
  private GameObject target;
  public LocText DescriptionText;
  private Automatable targetAutomatable;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
    this.allowManualToggle.onValueChanged += new System.Action<bool>(this.OnAllowManualChanged);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Automatable>() != (UnityEngine.Object) null;
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
      this.targetAutomatable = target.GetComponent<Automatable>();
      if ((UnityEngine.Object) this.targetAutomatable == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The target provided does not have an Automatable component");
      }
      else
      {
        this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
        this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
      }
    }
  }

  private void OnAllowManualChanged(bool value)
  {
    this.targetAutomatable.SetAutomationOnly(!value);
    this.allowManualToggleCheckMark.enabled = value;
  }
}
