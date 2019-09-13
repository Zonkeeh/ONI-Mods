// Decompiled with JetBrains decompiler
// Type: SchedulePaintButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SchedulePaintButton : KMonoBehaviour
{
  [SerializeField]
  private LocText label;
  [SerializeField]
  private ImageToggleState toggleState;
  [SerializeField]
  private MultiToggle toggle;
  [SerializeField]
  private ToolTip toolTip;

  public ScheduleGroup group { get; private set; }

  public void SetGroup(
    ScheduleGroup group,
    Dictionary<string, ColorStyleSetting> styles,
    System.Action<SchedulePaintButton> onClick)
  {
    this.group = group;
    if (styles.ContainsKey(group.Id))
      this.toggleState.SetColorStyle(styles[group.Id]);
    this.label.text = group.Name;
    this.toggle.onClick += (System.Action) (() => onClick(this));
    this.toolTip.SetSimpleTooltip(group.GetTooltip());
    this.gameObject.name = "PaintButton_" + group.Id;
  }

  public void SetToggle(bool on)
  {
    this.toggle.ChangeState(!on ? 0 : 1);
  }
}
