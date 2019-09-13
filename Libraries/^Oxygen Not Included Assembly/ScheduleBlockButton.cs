// Decompiled with JetBrains decompiler
// Type: ScheduleBlockButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ScheduleBlockButton : KMonoBehaviour
{
  [SerializeField]
  private KImage image;
  [SerializeField]
  private ToolTip toolTip;
  private Dictionary<string, ColorStyleSetting> paintStyles;

  public int idx { get; private set; }

  public void Setup(int idx, Dictionary<string, ColorStyleSetting> paintStyles, int totalBlocks)
  {
    this.idx = idx;
    this.paintStyles = paintStyles;
    if (idx < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
      this.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
    else if (idx >= totalBlocks - 3)
      this.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
    this.gameObject.name = "ScheduleBlock_" + idx.ToString();
  }

  public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
  {
    ScheduleGroup forScheduleTypes = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
    if (forScheduleTypes != null && this.paintStyles.ContainsKey(forScheduleTypes.Id))
    {
      this.image.colorStyleSetting = this.paintStyles[forScheduleTypes.Id];
      this.image.ApplyColorStyleSetting();
      this.toolTip.SetSimpleTooltip(forScheduleTypes.GetTooltip());
    }
    else
      this.toolTip.SetSimpleTooltip("UNKNOWN");
  }
}
