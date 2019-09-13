// Decompiled with JetBrains decompiler
// Type: BuildingChoresPanelDupeRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class BuildingChoresPanelDupeRow : KMonoBehaviour
{
  public Image icon;
  public LocText label;
  public ToolTip toolTip;
  private ChoreConsumer choreConsumer;
  public KButton button;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.OnClick);
  }

  public void Init(BuildingChoresPanel.DupeEntryData data)
  {
    this.choreConsumer = data.consumer;
    if (data.context.IsPotentialSuccess())
    {
      string newValue = !((UnityEngine.Object) data.context.chore.driver == (UnityEngine.Object) data.consumer.choreDriver) ? string.Format(DUPLICANTS.CHORES.PRECONDITIONS.RANK_FORMAT.text, (object) data.rank) : DUPLICANTS.CHORES.PRECONDITIONS.CURRENT_ERRAND.text;
      this.label.text = DUPLICANTS.CHORES.PRECONDITIONS.SUCCESS_ROW.Replace("{Duplicant}", data.consumer.name).Replace("{Rank}", newValue);
    }
    else
    {
      string str = data.context.chore.GetPreconditions()[data.context.failedPreconditionId].description;
      DebugUtil.Assert(str != null, "Chore requires description!", data.context.chore.GetPreconditions()[data.context.failedPreconditionId].id);
      if ((UnityEngine.Object) data.context.chore.driver != (UnityEngine.Object) null)
        str = str.Replace("{Assignee}", data.context.chore.driver.GetProperName());
      string newValue = str.Replace("{Selected}", data.context.chore.gameObject.GetProperName());
      this.label.text = DUPLICANTS.CHORES.PRECONDITIONS.FAILURE_ROW.Replace("{Duplicant}", data.consumer.name).Replace("{Reason}", newValue);
    }
    this.icon.sprite = JobsTableScreen.priorityInfo[data.personalPriority].sprite;
    this.toolTip.toolTip = BuildingChoresPanelDupeRow.TooltipForDupe(data.context, data.consumer, data.rank);
  }

  private void OnClick()
  {
    CameraController.Instance.SetTargetPos(this.choreConsumer.gameObject.transform.GetPosition() + Vector3.up, 10f, true);
  }

  private static string TooltipForDupe(
    Chore.Precondition.Context context,
    ChoreConsumer choreConsumer,
    int rank)
  {
    bool flag = context.IsPotentialSuccess();
    string str1 = (string) (!flag ? STRINGS.UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_FAILED : STRINGS.UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_SUCCEEDED);
    float num1 = 0.0f;
    int personalPriority = choreConsumer.GetPersonalPriority(context.chore.choreType);
    float num2 = num1 + (float) (personalPriority * 10);
    int priorityValue = context.chore.masterPriority.priority_value;
    float num3 = num2 + (float) priorityValue;
    float num4 = (float) context.priority / 10000f;
    float num5 = num3 + num4;
    string str2 = str1.Replace("{Description}", (string) (!((UnityEngine.Object) context.chore.driver == (UnityEngine.Object) choreConsumer.choreDriver) ? STRINGS.UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_DESC_INACTIVE : STRINGS.UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_DESC_ACTIVE));
    string newValue1 = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
    string newValue2 = STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_NA.text;
    if (flag && context.chore.choreType.groups.Length > 0)
    {
      ChoreGroup group = context.chore.choreType.groups[0];
      for (int index = 1; index < context.chore.choreType.groups.Length; ++index)
      {
        if (choreConsumer.GetPersonalPriority(group) < choreConsumer.GetPersonalPriority(context.chore.choreType.groups[index]))
          group = context.chore.choreType.groups[index];
      }
      newValue2 = group.Name;
    }
    string str3 = str2.Replace("{Name}", choreConsumer.name).Replace("{Errand}", GameUtil.GetChoreName(context.chore, context.data));
    return flag ? str3.Replace("{Rank}", rank.ToString()).Replace("{Groups}", newValue1).Replace("{BestGroup}", newValue2).Replace("{PersonalPriority}", JobsTableScreen.priorityInfo[personalPriority].name.text).Replace("{PersonalPriorityValue}", (personalPriority * 10).ToString()).Replace("{Building}", context.chore.gameObject.GetProperName()).Replace("{BuildingPriority}", priorityValue.ToString()).Replace("{TypePriority}", num4.ToString()).Replace("{TotalPriority}", num5.ToString()) : str3.Replace("{FailedPrecondition}", context.chore.GetPreconditions()[context.failedPreconditionId].description);
  }
}
