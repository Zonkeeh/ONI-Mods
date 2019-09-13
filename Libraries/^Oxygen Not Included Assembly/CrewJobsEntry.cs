// Decompiled with JetBrains decompiler
// Type: CrewJobsEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrewJobsEntry : CrewListEntry
{
  private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();
  public GameObject Prefab_JobPriorityButton;
  public GameObject Prefab_JobPriorityButtonAllTasks;
  private CrewJobsEntry.PriorityButton AllTasksButton;
  public TextStyleSetting TooltipTextStyle_Title;
  public TextStyleSetting TooltipTextStyle_Ability;
  public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;
  public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;
  private bool dirty;
  private CrewJobsScreen.everyoneToggleState rowToggleState;

  public ChoreConsumer consumer { get; private set; }

  public override void Populate(MinionIdentity _identity)
  {
    base.Populate(_identity);
    this.consumer = _identity.GetComponent<ChoreConsumer>();
    this.consumer.choreRulesChanged += new System.Action(this.Dirty);
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      this.CreateChoreButton(resource);
    this.CreateAllTaskButton();
    this.dirty = true;
  }

  private void CreateChoreButton(ChoreGroup chore_group)
  {
    GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButton, this.transform.gameObject, false);
    gameObject.GetComponent<OverviewColumnIdentity>().columnID = chore_group.Id;
    gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = chore_group.Name;
    CrewJobsEntry.PriorityButton priorityButton = new CrewJobsEntry.PriorityButton()
    {
      button = gameObject.GetComponent<UnityEngine.UI.Button>(),
      border = gameObject.transform.GetChild(1).GetComponent<Image>()
    };
    priorityButton.baseBorderColor = priorityButton.border.color;
    priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
    priorityButton.baseBackgroundColor = priorityButton.background.color;
    priorityButton.choreGroup = chore_group;
    priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
    priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
    priorityButton.tooltip.OnToolTip = (Func<string>) (() => this.OnPriorityButtonTooltip(priorityButton));
    priorityButton.button.onClick.AddListener((UnityAction) (() => this.OnPriorityPress(chore_group)));
    this.PriorityButtons.Add(priorityButton);
  }

  private void CreateAllTaskButton()
  {
    GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButtonAllTasks, this.transform.gameObject, false);
    gameObject.GetComponent<OverviewColumnIdentity>().columnID = "AllTasks";
    gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = string.Empty;
    UnityEngine.UI.Button b = gameObject.GetComponent<UnityEngine.UI.Button>();
    b.onClick.AddListener((UnityAction) (() => this.ToggleTasksAll(b)));
    CrewJobsEntry.PriorityButton priorityButton = new CrewJobsEntry.PriorityButton()
    {
      button = gameObject.GetComponent<UnityEngine.UI.Button>(),
      border = gameObject.transform.GetChild(1).GetComponent<Image>()
    };
    priorityButton.baseBorderColor = priorityButton.border.color;
    priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
    priorityButton.baseBackgroundColor = priorityButton.background.color;
    priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
    priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
    this.AllTasksButton = priorityButton;
  }

  private void ToggleTasksAll(UnityEngine.UI.Button button)
  {
    bool is_allowed = this.rowToggleState != CrewJobsScreen.everyoneToggleState.on;
    string name = "HUD_Click_Deselect";
    if (is_allowed)
      name = "HUD_Click";
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      this.consumer.SetPermittedByUser(resource, is_allowed);
  }

  private void OnPriorityPress(ChoreGroup chore_group)
  {
    bool flag = this.consumer.IsPermittedByUser(chore_group);
    string name = "HUD_Click";
    if (flag)
      name = "HUD_Click_Deselect";
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
    this.consumer.SetPermittedByUser(chore_group, !this.consumer.IsPermittedByUser(chore_group));
  }

  private void Refresh(object data = null)
  {
    if ((UnityEngine.Object) this.identity == (UnityEngine.Object) null)
    {
      this.dirty = false;
    }
    else
    {
      if (!this.dirty)
        return;
      Attributes attributes = this.identity.GetAttributes();
      foreach (CrewJobsEntry.PriorityButton priorityButton in this.PriorityButtons)
      {
        bool flag1 = this.consumer.IsPermittedByUser(priorityButton.choreGroup);
        if (priorityButton.ToggleIcon.activeSelf != flag1)
          priorityButton.ToggleIcon.SetActive(flag1);
        float t = Mathf.Min(attributes.Get(priorityButton.choreGroup.attribute).GetTotalValue() / 10f, 1f);
        Color baseBorderColor = priorityButton.baseBorderColor;
        baseBorderColor.r = Mathf.Lerp(priorityButton.baseBorderColor.r, 0.7215686f, t);
        baseBorderColor.g = Mathf.Lerp(priorityButton.baseBorderColor.g, 0.4431373f, t);
        baseBorderColor.b = Mathf.Lerp(priorityButton.baseBorderColor.b, 0.5803922f, t);
        if (priorityButton.border.color != baseBorderColor)
          priorityButton.border.color = baseBorderColor;
        Color color = priorityButton.baseBackgroundColor;
        color.a = Mathf.Lerp(0.0f, 1f, t);
        bool flag2 = this.consumer.IsPermittedByTraits(priorityButton.choreGroup);
        if (!flag2)
        {
          color = Color.clear;
          priorityButton.border.color = Color.clear;
          priorityButton.ToggleIcon.SetActive(false);
        }
        priorityButton.button.interactable = flag2;
        if (priorityButton.background.color != color)
          priorityButton.background.color = color;
      }
      int num1 = 0;
      int num2 = 0;
      foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      {
        if (this.consumer.IsPermittedByTraits(resource))
        {
          ++num2;
          if (this.consumer.IsPermittedByUser(resource))
            ++num1;
        }
      }
      this.rowToggleState = num1 != 0 ? (num1 >= num2 ? CrewJobsScreen.everyoneToggleState.on : CrewJobsScreen.everyoneToggleState.mixed) : CrewJobsScreen.everyoneToggleState.off;
      ImageToggleState component = this.AllTasksButton.ToggleIcon.GetComponent<ImageToggleState>();
      switch (this.rowToggleState)
      {
        case CrewJobsScreen.everyoneToggleState.off:
          component.SetDisabled();
          break;
        case CrewJobsScreen.everyoneToggleState.mixed:
          component.SetInactive();
          break;
        case CrewJobsScreen.everyoneToggleState.on:
          component.SetActive();
          break;
      }
      this.dirty = false;
    }
  }

  private string OnPriorityButtonTooltip(CrewJobsEntry.PriorityButton b)
  {
    b.tooltip.ClearMultiStringTooltip();
    if ((UnityEngine.Object) this.identity != (UnityEngine.Object) null)
    {
      Attributes attributes = this.identity.GetAttributes();
      if (attributes != null)
      {
        if (!this.consumer.IsPermittedByTraits(b.choreGroup))
        {
          string newString = string.Format((string) STRINGS.UI.TOOLTIPS.JOBSSCREEN_CANNOTPERFORMTASK, (object) this.consumer.GetComponent<MinionIdentity>().GetProperName());
          b.tooltip.AddMultiStringTooltip(newString, (ScriptableObject) this.TooltipTextStyle_AbilityNegativeModifier);
          return string.Empty;
        }
        b.tooltip.AddMultiStringTooltip((string) STRINGS.UI.TOOLTIPS.JOBSSCREEN_RELEVANT_ATTRIBUTES, (ScriptableObject) this.TooltipTextStyle_Ability);
        Klei.AI.Attribute attribute = b.choreGroup.attribute;
        AttributeInstance attributeInstance = attributes.Get(attribute);
        float totalValue = attributeInstance.GetTotalValue();
        TextStyleSetting textStyleSetting = this.TooltipTextStyle_Ability;
        if ((double) totalValue > 0.0)
          textStyleSetting = this.TooltipTextStyle_AbilityPositiveModifier;
        else if ((double) totalValue < 0.0)
          textStyleSetting = this.TooltipTextStyle_AbilityNegativeModifier;
        b.tooltip.AddMultiStringTooltip(attribute.Name + " " + (object) attributeInstance.GetTotalValue(), (ScriptableObject) textStyleSetting);
      }
    }
    return string.Empty;
  }

  private void LateUpdate()
  {
    this.Refresh((object) null);
  }

  private void OnLevelUp(object data)
  {
    this.Dirty();
  }

  private void Dirty()
  {
    this.dirty = true;
    CrewJobsScreen.Instance.Dirty((object) null);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!((UnityEngine.Object) this.consumer != (UnityEngine.Object) null))
      return;
    this.consumer.choreRulesChanged -= new System.Action(this.Dirty);
  }

  [Serializable]
  public struct PriorityButton
  {
    public UnityEngine.UI.Button button;
    public GameObject ToggleIcon;
    public ChoreGroup choreGroup;
    public ToolTip tooltip;
    public Image border;
    public Image background;
    public Color baseBorderColor;
    public Color baseBackgroundColor;
  }
}
