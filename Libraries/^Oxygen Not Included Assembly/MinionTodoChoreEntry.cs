// Decompiled with JetBrains decompiler
// Type: MinionTodoChoreEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionTodoChoreEntry : KMonoBehaviour
{
  public Image icon;
  public Image priorityIcon;
  public LocText priorityLabel;
  public LocText label;
  public LocText subLabel;
  public LocText moreLabel;
  public List<Sprite> prioritySprites;
  [SerializeField]
  private ColorStyleSetting buttonColorSettingCurrent;
  [SerializeField]
  private ColorStyleSetting buttonColorSettingStandard;
  private Chore targetChore;
  private IStateMachineTarget lastChoreTarget;
  private PrioritySetting lastPrioritySetting;

  public void SetMoreAmount(int amount)
  {
    if (amount == 0)
      this.moreLabel.gameObject.SetActive(false);
    else
      this.moreLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TRUNCATED_CHORES, (object) amount);
  }

  public void Apply(Chore.Precondition.Context context)
  {
    ChoreConsumer consumer = context.consumerState.consumer;
    if (this.targetChore == context.chore && object.ReferenceEquals((object) context.chore.target, (object) this.lastChoreTarget) && context.chore.masterPriority == this.lastPrioritySetting)
      return;
    this.targetChore = context.chore;
    this.lastChoreTarget = context.chore.target;
    this.lastPrioritySetting = context.chore.masterPriority;
    string choreName = GameUtil.GetChoreName(context.chore, context.data);
    string newValue = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
    string text1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_TARGET.Replace("{Target}", !((UnityEngine.Object) context.chore.target.gameObject == (UnityEngine.Object) consumer.gameObject) ? context.chore.target.gameObject.GetProperName() : STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.SELF_LABEL.text);
    if (newValue != null)
      text1 = text1.Replace("{Groups}", newValue);
    string text2 = context.chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic ? string.Empty : context.chore.masterPriority.priority_value.ToString();
    Sprite sprite = context.chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic ? (Sprite) null : this.prioritySprites[context.chore.masterPriority.priority_value - 1];
    ChoreGroup choreGroup = MinionTodoChoreEntry.BestPriorityGroup(context, consumer);
    this.icon.sprite = choreGroup == null ? (Sprite) null : Assets.GetSprite((HashedString) choreGroup.sprite);
    this.label.SetText(choreName);
    this.subLabel.SetText(text1);
    this.priorityLabel.SetText(text2);
    this.priorityIcon.sprite = sprite;
    this.moreLabel.text = string.Empty;
    this.GetComponent<ToolTip>().SetSimpleTooltip(MinionTodoChoreEntry.TooltipForChore(context, consumer));
    KButton componentInChildren = this.GetComponentInChildren<KButton>();
    componentInChildren.ClearOnClick();
    if ((UnityEngine.Object) componentInChildren.bgImage != (UnityEngine.Object) null)
    {
      componentInChildren.bgImage.colorStyleSetting = !((UnityEngine.Object) context.chore.driver == (UnityEngine.Object) consumer.choreDriver) ? this.buttonColorSettingStandard : this.buttonColorSettingCurrent;
      componentInChildren.bgImage.ApplyColorStyleSetting();
    }
    GameObject gameObject = context.chore.target.gameObject;
    componentInChildren.ClearOnPointerEvents();
    componentInChildren.GetComponentInChildren<KButton>().onClick += (System.Action) (() =>
    {
      if (context.chore == null || context.chore.target.isNull)
        return;
      CameraController.Instance.SetTargetPos(new Vector3(context.chore.target.gameObject.transform.position.x, context.chore.target.gameObject.transform.position.y + 1f, CameraController.Instance.transform.position.z), 10f, true);
    });
  }

  private static ChoreGroup BestPriorityGroup(
    Chore.Precondition.Context context,
    ChoreConsumer choreConsumer)
  {
    ChoreGroup group = (ChoreGroup) null;
    if (context.chore.choreType.groups.Length > 0)
    {
      group = context.chore.choreType.groups[0];
      for (int index = 1; index < context.chore.choreType.groups.Length; ++index)
      {
        if (choreConsumer.GetPersonalPriority(group) < choreConsumer.GetPersonalPriority(context.chore.choreType.groups[index]))
          group = context.chore.choreType.groups[index];
      }
    }
    return group;
  }

  private static string TooltipForChore(
    Chore.Precondition.Context context,
    ChoreConsumer choreConsumer)
  {
    bool flag = context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.basic || context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.high;
    string str1;
    switch (context.chore.masterPriority.priority_class + 1)
    {
      case PriorityScreen.PriorityClass.basic:
        str1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLE;
        break;
      case PriorityScreen.PriorityClass.topPriority:
        str1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_PERSONAL;
        break;
      case PriorityScreen.PriorityClass.compulsory:
        str1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_EMERGENCY;
        break;
      case PriorityScreen.PriorityClass.high | PriorityScreen.PriorityClass.compulsory:
        str1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_COMPULSORY;
        break;
      default:
        str1 = (string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_NORMAL;
        break;
    }
    float num1 = 0.0f;
    int num2 = (int) context.chore.masterPriority.priority_class * 100;
    float num3 = num1 + (float) num2;
    int index = !flag ? 0 : choreConsumer.GetPersonalPriority(context.chore.choreType);
    float num4 = num3 + (float) (index * 10);
    int num5 = !flag ? 0 : context.chore.masterPriority.priority_value;
    float num6 = num4 + (float) num5;
    float num7 = (float) context.priority / 10000f;
    float num8 = num6 + num7;
    string str2 = str1.Replace("{Description}", (string) (!((UnityEngine.Object) context.chore.driver == (UnityEngine.Object) choreConsumer.choreDriver) ? STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_DESC_INACTIVE : STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_DESC_ACTIVE)).Replace("{IdleDescription}", (string) (!((UnityEngine.Object) context.chore.driver == (UnityEngine.Object) choreConsumer.choreDriver) ? STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLEDESC_INACTIVE : STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLEDESC_ACTIVE));
    string newValue = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
    ChoreGroup choreGroup = MinionTodoChoreEntry.BestPriorityGroup(context, choreConsumer);
    return str2.Replace("{Name}", choreConsumer.name).Replace("{Errand}", GameUtil.GetChoreName(context.chore, context.data)).Replace("{Groups}", newValue).Replace("{BestGroup}", choreGroup == null ? context.chore.choreType.Name : choreGroup.Name).Replace("{ClassPriority}", num2.ToString()).Replace("{PersonalPriority}", JobsTableScreen.priorityInfo[index].name.text).Replace("{PersonalPriorityValue}", (index * 10).ToString()).Replace("{Building}", context.chore.gameObject.GetProperName()).Replace("{BuildingPriority}", num5.ToString()).Replace("{TypePriority}", num7.ToString()).Replace("{TotalPriority}", num8.ToString());
  }
}
