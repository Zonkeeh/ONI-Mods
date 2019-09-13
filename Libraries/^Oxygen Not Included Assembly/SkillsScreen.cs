// Decompiled with JetBrains decompiler
// Type: SkillsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillsScreen : KModalScreen
{
  public float baseCharacterScale = 0.38f;
  private List<GameObject> moraleNotches = new List<GameObject>();
  private List<GameObject> expectationNotches = new List<GameObject>();
  private List<SkillMinionWidget> minionWidgets = new List<SkillMinionWidget>();
  private string hoveredSkillID = string.Empty;
  private Dictionary<string, GameObject> skillWidgets = new Dictionary<string, GameObject>();
  private Dictionary<string, int> skillGroupRow = new Dictionary<string, int>();
  private List<GameObject> skillColumns = new List<GameObject>();
  private int layoutRowHeight = 80;
  public new const float SCREEN_SORT_KEY = 101f;
  [SerializeField]
  private KButton CloseButton;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject Prefab_skillWidget;
  [SerializeField]
  private GameObject Prefab_skillColumn;
  [SerializeField]
  private GameObject Prefab_minion;
  [SerializeField]
  private GameObject Prefab_minionLayout;
  [SerializeField]
  private GameObject Prefab_tableLayout;
  [Header("Sort Toggles")]
  [SerializeField]
  private MultiToggle dupeSortingToggle;
  [SerializeField]
  private MultiToggle experienceSortingToggle;
  [SerializeField]
  private MultiToggle moraleSortingToggle;
  private MultiToggle activeSortToggle;
  private bool sortReversed;
  [Header("Duplicant Animation")]
  [SerializeField]
  private GameObject duplicantAnimAnchor;
  [SerializeField]
  private KBatchedAnimController animController;
  private KAnimFile idle_anim;
  [Header("Progress Bars")]
  [SerializeField]
  private ToolTip expectationsTooltip;
  [SerializeField]
  private LocText moraleProgressLabel;
  [SerializeField]
  private GameObject moraleWarning;
  [SerializeField]
  private GameObject moraleNotch;
  [SerializeField]
  private Color moraleNotchColor;
  [SerializeField]
  private LocText expectationsProgressLabel;
  [SerializeField]
  private GameObject expectationWarning;
  [SerializeField]
  private GameObject expectationNotch;
  [SerializeField]
  private Color expectationNotchColor;
  [SerializeField]
  private Color expectationNotchProspectColor;
  [SerializeField]
  private ToolTip experienceBarTooltip;
  [SerializeField]
  private Image experienceProgressFill;
  [SerializeField]
  private LocText EXPCount;
  [SerializeField]
  private LocText duplicantLevelIndicator;
  [SerializeField]
  private KScrollRect scrollRect;
  [SerializeField]
  private DropDown hatDropDown;
  [SerializeField]
  public Image selectedHat;
  private IAssignableIdentity currentlySelectedMinion;
  private bool dirty;
  private bool linesPending;
  private Coroutine delayRefreshRoutine;

  public IAssignableIdentity CurrentlySelectedMinion
  {
    get
    {
      if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
        return (IAssignableIdentity) null;
      return this.currentlySelectedMinion;
    }
    set
    {
      this.currentlySelectedMinion = value;
      if (!this.IsActive())
        return;
      this.RefreshSelectedMinion();
      this.RefreshSkillWidgets();
    }
  }

  protected override void OnActivate()
  {
    this.ConsumeMouseScroll = true;
    base.OnActivate();
    this.BuildMinions();
    this.RefreshAll();
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnAddMinionIdentity);
    Components.LiveMinionIdentities.OnRemove += new System.Action<MinionIdentity>(this.OnRemoveMinionIdentity);
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.dupeSortingToggle.onClick += (System.Action) (() => this.SortByMinon());
    this.moraleSortingToggle.onClick += (System.Action) (() => this.SortByMorale());
    this.experienceSortingToggle.onClick += (System.Action) (() => this.SortByExperience());
  }

  protected override void OnShow(bool show)
  {
    if (show)
    {
      if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
        this.CurrentlySelectedMinion = (IAssignableIdentity) Components.LiveMinionIdentities.Items[0];
      this.RefreshAll();
    }
    base.OnShow(show);
  }

  public void RefreshAll()
  {
    this.dirty = false;
    this.RefreshSkillWidgets();
    this.RefreshSelectedMinion();
    this.linesPending = true;
  }

  private void RefreshSelectedMinion()
  {
    this.SetPortraitAnimator(this.currentlySelectedMinion);
    this.RefreshProgressBars();
    this.RefreshHat();
  }

  private void RefreshProgressBars()
  {
    if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
      return;
    MinionIdentity currentlySelectedMinion = this.currentlySelectedMinion as MinionIdentity;
    HierarchyReferences component1 = this.expectationsTooltip.GetComponent<HierarchyReferences>();
    component1.GetReference("Labels").gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null);
    component1.GetReference("MoraleBar").gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null);
    component1.GetReference("ExpectationBar").gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null);
    component1.GetReference("StoredMinion").gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion == (UnityEngine.Object) null);
    this.experienceProgressFill.gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null);
    if ((UnityEngine.Object) currentlySelectedMinion == (UnityEngine.Object) null)
    {
      this.expectationsTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) (this.currentlySelectedMinion as StoredMinionIdentity).GetStorageReason(), (object) this.currentlySelectedMinion.GetProperName()));
      this.experienceBarTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) (this.currentlySelectedMinion as StoredMinionIdentity).GetStorageReason(), (object) this.currentlySelectedMinion.GetProperName()));
      this.EXPCount.text = string.Empty;
      this.duplicantLevelIndicator.text = (string) STRINGS.UI.TABLESCREENS.NA;
    }
    else
    {
      MinionResume component2 = currentlySelectedMinion.GetComponent<MinionResume>();
      float previousExperienceBar = MinionResume.CalculatePreviousExperienceBar(component2.TotalSkillPointsGained);
      float nextExperienceBar = MinionResume.CalculateNextExperienceBar(component2.TotalSkillPointsGained);
      float num1 = (float) (((double) component2.TotalExperienceGained - (double) previousExperienceBar) / ((double) nextExperienceBar - (double) previousExperienceBar));
      this.EXPCount.text = Mathf.RoundToInt(component2.TotalExperienceGained - previousExperienceBar).ToString() + " / " + (object) Mathf.RoundToInt(nextExperienceBar - previousExperienceBar);
      this.duplicantLevelIndicator.text = component2.AvailableSkillpoints.ToString();
      this.experienceProgressFill.fillAmount = num1;
      this.experienceBarTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.SKILLS_SCREEN.EXPERIENCE_TOOLTIP, (object) (Mathf.RoundToInt(nextExperienceBar - previousExperienceBar) - Mathf.RoundToInt(component2.TotalExperienceGained - previousExperienceBar))));
      AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) component2);
      AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component2);
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (!string.IsNullOrEmpty(this.hoveredSkillID) && !component2.HasMasteredSkill(this.hoveredSkillID))
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        stringList1.Add(this.hoveredSkillID);
        while (stringList1.Count > 0)
        {
          for (int index = stringList1.Count - 1; index >= 0; --index)
          {
            if (!component2.HasMasteredSkill(stringList1[index]))
            {
              num2 += (float) (Db.Get().Skills.Get(stringList1[index]).tier + 1);
              if (component2.AptitudeBySkillGroup.ContainsKey((HashedString) Db.Get().Skills.Get(stringList1[index]).skillGroup) && (double) component2.AptitudeBySkillGroup[(HashedString) Db.Get().Skills.Get(stringList1[index]).skillGroup] > 0.0)
                ++num3;
              foreach (string priorSkill in Db.Get().Skills.Get(stringList1[index]).priorSkills)
                stringList2.Add(priorSkill);
            }
          }
          stringList1.Clear();
          stringList1.AddRange((IEnumerable<string>) stringList2);
          stringList2.Clear();
        }
      }
      float num4 = attributeInstance1.GetTotalValue() + num3 / (attributeInstance2.GetTotalValue() + num2);
      float f = Mathf.Max(attributeInstance1.GetTotalValue() + num3, attributeInstance2.GetTotalValue() + num2);
      while (this.moraleNotches.Count < Mathf.RoundToInt(f))
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.moraleNotch, this.moraleNotch.transform.parent);
        gameObject.SetActive(true);
        this.moraleNotches.Add(gameObject);
      }
      while (this.moraleNotches.Count > Mathf.RoundToInt(f))
      {
        GameObject moraleNotch = this.moraleNotches[this.moraleNotches.Count - 1];
        this.moraleNotches.Remove(moraleNotch);
        UnityEngine.Object.Destroy((UnityEngine.Object) moraleNotch);
      }
      for (int index = 0; index < this.moraleNotches.Count; ++index)
      {
        if ((double) index < (double) attributeInstance1.GetTotalValue() + (double) num3)
          this.moraleNotches[index].GetComponentsInChildren<Image>()[1].color = this.moraleNotchColor;
        else
          this.moraleNotches[index].GetComponentsInChildren<Image>()[1].color = Color.clear;
      }
      this.moraleProgressLabel.text = (string) STRINGS.UI.SKILLS_SCREEN.MORALE + ": " + attributeInstance1.GetTotalValue().ToString();
      if ((double) num3 > 0.0)
      {
        LocText moraleProgressLabel = this.moraleProgressLabel;
        moraleProgressLabel.text = moraleProgressLabel.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) this.moraleNotchColor, num3.ToString()));
      }
      while (this.expectationNotches.Count < Mathf.RoundToInt(f))
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.expectationNotch, this.expectationNotch.transform.parent);
        gameObject.SetActive(true);
        this.expectationNotches.Add(gameObject);
      }
      while (this.expectationNotches.Count > Mathf.RoundToInt(f))
      {
        GameObject expectationNotch = this.expectationNotches[this.expectationNotches.Count - 1];
        this.expectationNotches.Remove(expectationNotch);
        UnityEngine.Object.Destroy((UnityEngine.Object) expectationNotch);
      }
      for (int index = 0; index < this.expectationNotches.Count; ++index)
      {
        if ((double) index < (double) attributeInstance2.GetTotalValue() + (double) num2)
        {
          if ((double) index < (double) attributeInstance2.GetTotalValue())
            this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = this.expectationNotchColor;
          else
            this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = this.expectationNotchProspectColor;
        }
        else
          this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = Color.clear;
      }
      this.expectationsProgressLabel.text = (string) STRINGS.UI.SKILLS_SCREEN.MORALE_EXPECTATION + ": " + attributeInstance2.GetTotalValue().ToString();
      if ((double) num2 > 0.0)
      {
        LocText expectationsProgressLabel = this.expectationsProgressLabel;
        expectationsProgressLabel.text = expectationsProgressLabel.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) this.expectationNotchColor, num2.ToString()));
      }
      if ((double) num4 < 1.0)
      {
        this.expectationWarning.SetActive(true);
        this.moraleWarning.SetActive(false);
      }
      else
      {
        this.expectationWarning.SetActive(false);
        this.moraleWarning.SetActive(true);
      }
      string empty = string.Empty;
      Dictionary<string, float> source = new Dictionary<string, float>();
      string str = empty + GameUtil.ApplyBoldString((string) STRINGS.UI.SKILLS_SCREEN.MORALE) + ": " + (object) attributeInstance1.GetTotalValue() + "\n";
      for (int index = 0; index < attributeInstance1.Modifiers.Count; ++index)
        source.Add(attributeInstance1.Modifiers[index].GetDescription(), attributeInstance1.Modifiers[index].Value);
      List<KeyValuePair<string, float>> list = source.ToList<KeyValuePair<string, float>>();
      list.Sort((Comparison<KeyValuePair<string, float>>) ((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)));
      foreach (KeyValuePair<string, float> keyValuePair in list)
        str = str + "    • " + keyValuePair.Key + ": " + ((double) keyValuePair.Value <= 0.0 ? UIConstants.ColorPrefixRed : UIConstants.ColorPrefixGreen) + keyValuePair.Value.ToString() + UIConstants.ColorSuffix + "\n";
      string message = str + "\n" + GameUtil.ApplyBoldString((string) STRINGS.UI.SKILLS_SCREEN.MORALE_EXPECTATION) + ": " + (object) attributeInstance2.GetTotalValue() + "\n";
      for (int index = 0; index < attributeInstance2.Modifiers.Count; ++index)
        message = message + "    • " + attributeInstance2.Modifiers[index].GetDescription() + ": " + ((double) attributeInstance2.Modifiers[index].Value <= 0.0 ? UIConstants.ColorPrefixGreen : UIConstants.ColorPrefixRed) + attributeInstance2.Modifiers[index].GetFormattedString(component2.gameObject) + UIConstants.ColorSuffix + "\n";
      this.expectationsTooltip.SetSimpleTooltip(message);
    }
  }

  private void RefreshHat()
  {
    if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
      return;
    List<IListableOption> listableOptionList = new List<IListableOption>();
    string empty = string.Empty;
    MinionIdentity currentlySelectedMinion1 = this.currentlySelectedMinion as MinionIdentity;
    string str;
    if ((UnityEngine.Object) currentlySelectedMinion1 != (UnityEngine.Object) null)
    {
      MinionResume component = currentlySelectedMinion1.GetComponent<MinionResume>();
      str = !string.IsNullOrEmpty(component.TargetHat) ? component.TargetHat : component.CurrentHat;
      foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
      {
        if (keyValuePair.Value)
          listableOptionList.Add((IListableOption) new SkillListable(keyValuePair.Key));
      }
      this.hatDropDown.Initialize((IEnumerable<IListableOption>) listableOptionList, new System.Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new System.Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, (object) this.currentlySelectedMinion);
    }
    else
    {
      StoredMinionIdentity currentlySelectedMinion2 = this.currentlySelectedMinion as StoredMinionIdentity;
      str = !string.IsNullOrEmpty(currentlySelectedMinion2.targetHat) ? currentlySelectedMinion2.targetHat : currentlySelectedMinion2.currentHat;
    }
    this.hatDropDown.openButton.enabled = (UnityEngine.Object) currentlySelectedMinion1 != (UnityEngine.Object) null;
    this.selectedHat.transform.Find("Arrow").gameObject.SetActive((UnityEngine.Object) currentlySelectedMinion1 != (UnityEngine.Object) null);
    this.selectedHat.sprite = Assets.GetSprite((HashedString) (!string.IsNullOrEmpty(str) ? str : "hat_role_none"));
  }

  private void OnHatDropEntryClick(IListableOption skill, object data)
  {
    MinionIdentity currentlySelectedMinion = this.currentlySelectedMinion as MinionIdentity;
    if ((UnityEngine.Object) currentlySelectedMinion == (UnityEngine.Object) null)
      return;
    MinionResume component = currentlySelectedMinion.GetComponent<MinionResume>();
    string str = "hat_role_none";
    if (skill != null)
    {
      this.selectedHat.sprite = Assets.GetSprite((HashedString) (skill as SkillListable).skillHat);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        string skillHat = (skill as SkillListable).skillHat;
        component.SetHats(component.CurrentHat, skillHat);
        if (component.OwnsHat(skillHat))
        {
          PutOnHatChore putOnHatChore = new PutOnHatChore((IStateMachineTarget) component, Db.Get().ChoreTypes.SwitchHat);
        }
      }
    }
    else
    {
      this.selectedHat.sprite = Assets.GetSprite((HashedString) str);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.SetHats(component.CurrentHat, (string) null);
        component.ApplyTargetHat();
      }
    }
    foreach (SkillMinionWidget minionWidget in this.minionWidgets)
    {
      if (minionWidget.minion == this.currentlySelectedMinion)
        minionWidget.RefreshHat(component.TargetHat);
    }
  }

  private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    SkillListable entryData = entry.entryData as SkillListable;
    entry.image.sprite = Assets.GetSprite((HashedString) entryData.skillHat);
  }

  private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
  {
    return 0;
  }

  private void Update()
  {
    if (this.dirty)
      this.RefreshAll();
    if (!this.linesPending)
      return;
    foreach (GameObject gameObject in this.skillWidgets.Values)
      gameObject.GetComponent<SkillWidget>().RefreshLines();
    this.linesPending = false;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed && !this.scrollRect.isDragging && e.TryConsume(Action.MouseRight))
      ManagementMenu.Instance.CloseAll();
    else
      base.OnKeyUp(e);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && e.TryConsume(Action.Escape))
      ManagementMenu.Instance.CloseAll();
    else
      base.OnKeyDown(e);
  }

  private void RefreshSkillWidgets()
  {
    int num = 1;
    foreach (SkillGroup resource in Db.Get().SkillGroups.resources)
    {
      List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(resource.Id);
      if (skillsBySkillGroup.Count > 0)
      {
        if (!this.skillGroupRow.ContainsKey(resource.Id))
          this.skillGroupRow.Add(resource.Id, num++);
        for (int index = 0; index < skillsBySkillGroup.Count; ++index)
        {
          Skill skill = skillsBySkillGroup[index];
          if (!this.skillWidgets.ContainsKey(skill.Id))
          {
            while (skill.tier >= this.skillColumns.Count)
            {
              GameObject gameObject = Util.KInstantiateUI(this.Prefab_skillColumn, this.Prefab_tableLayout, true);
              this.skillColumns.Add(gameObject);
              HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
              if (this.skillColumns.Count % 2 == 0)
                component.GetReference("BG").gameObject.SetActive(false);
            }
            GameObject gameObject1 = Util.KInstantiateUI(this.Prefab_skillWidget, this.skillColumns[skill.tier], true);
            this.skillWidgets.Add(skill.Id, gameObject1);
          }
          this.skillWidgets[skill.Id].GetComponent<SkillWidget>().Refresh(skill.Id);
        }
      }
    }
    foreach (SkillMinionWidget minionWidget in this.minionWidgets)
      minionWidget.Refresh();
    this.RefreshWidgetPositions();
  }

  public void HoverSkill(string skillID)
  {
    this.hoveredSkillID = skillID;
    if (this.delayRefreshRoutine != null)
    {
      this.StopCoroutine(this.delayRefreshRoutine);
      this.delayRefreshRoutine = (Coroutine) null;
    }
    if (string.IsNullOrEmpty(this.hoveredSkillID))
      this.delayRefreshRoutine = this.StartCoroutine(this.DelayRefreshProgressBars());
    else
      this.RefreshProgressBars();
  }

  [DebuggerHidden]
  private IEnumerator DelayRefreshProgressBars()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SkillsScreen.\u003CDelayRefreshProgressBars\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public void RefreshWidgetPositions()
  {
    float num1 = 0.0f;
    foreach (KeyValuePair<string, GameObject> skillWidget in this.skillWidgets)
    {
      float rowPosition = this.GetRowPosition(skillWidget.Key);
      num1 = Mathf.Max(rowPosition, num1);
      skillWidget.Value.rectTransform().anchoredPosition = Vector2.down * rowPosition;
    }
    float num2 = Mathf.Max(num1, (float) this.layoutRowHeight);
    float layoutRowHeight = (float) this.layoutRowHeight;
    foreach (GameObject skillColumn in this.skillColumns)
      skillColumn.GetComponent<LayoutElement>().minHeight = num2 + layoutRowHeight;
    this.linesPending = true;
  }

  public float GetRowPosition(string skillID)
  {
    return (float) (this.layoutRowHeight * (this.skillGroupRow[Db.Get().Skills.Get(skillID).skillGroup] - 1));
  }

  private void OnAddMinionIdentity(MinionIdentity add)
  {
    this.BuildMinions();
    this.RefreshAll();
  }

  private void OnRemoveMinionIdentity(MinionIdentity remove)
  {
    if (this.CurrentlySelectedMinion == remove)
      this.CurrentlySelectedMinion = (IAssignableIdentity) null;
    this.BuildMinions();
    this.RefreshAll();
  }

  private void BuildMinions()
  {
    for (int index = this.minionWidgets.Count - 1; index >= 0; --index)
      this.minionWidgets[index].DeleteObject();
    this.minionWidgets.Clear();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
      gameObject.GetComponent<SkillMinionWidget>().SetMinon((IAssignableIdentity) minionIdentity);
      this.minionWidgets.Add(gameObject.GetComponent<SkillMinionWidget>());
    }
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
        {
          StoredMinionIdentity storedMinionIdentity = info.serializedMinion.Get<StoredMinionIdentity>();
          GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
          gameObject.GetComponent<SkillMinionWidget>().SetMinon((IAssignableIdentity) storedMinionIdentity);
          this.minionWidgets.Add(gameObject.GetComponent<SkillMinionWidget>());
        }
      }
    }
    if (this.CurrentlySelectedMinion != null || Components.LiveMinionIdentities.Count <= 0)
      return;
    this.CurrentlySelectedMinion = (IAssignableIdentity) Components.LiveMinionIdentities.Items[0];
  }

  public Vector2 GetSkillWidgetLineTargetPosition(string skillID)
  {
    return (Vector2) this.skillWidgets[skillID].GetComponent<SkillWidget>().lines_right.GetPosition();
  }

  public SkillWidget GetSkillWidget(string skill)
  {
    return this.skillWidgets[skill].GetComponent<SkillWidget>();
  }

  public List<Skill> GetSkillsBySkillGroup(string skillGrp)
  {
    List<Skill> skillList = new List<Skill>();
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      if (resource.skillGroup == skillGrp)
        skillList.Add(resource);
    }
    return skillList;
  }

  private void SelectSortToggle(MultiToggle toggle)
  {
    this.dupeSortingToggle.ChangeState(0);
    this.experienceSortingToggle.ChangeState(0);
    this.moraleSortingToggle.ChangeState(0);
    if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.activeSortToggle == (UnityEngine.Object) toggle)
        this.sortReversed = !this.sortReversed;
      this.activeSortToggle = toggle;
    }
    this.activeSortToggle.ChangeState(!this.sortReversed ? 1 : 2);
  }

  private void SortByMorale()
  {
    this.SelectSortToggle(this.moraleSortingToggle);
    List<SkillMinionWidget> minionWidgets = this.minionWidgets;
    minionWidgets.Sort((Comparison<SkillMinionWidget>) ((a, b) =>
    {
      MinionIdentity minion1 = a.minion as MinionIdentity;
      MinionIdentity minion2 = b.minion as MinionIdentity;
      if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null && (UnityEngine.Object) minion2 == (UnityEngine.Object) null)
        return 0;
      if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null)
        return -1;
      if ((UnityEngine.Object) minion2 == (UnityEngine.Object) null)
        return 1;
      MinionResume component1 = minion1.GetComponent<MinionResume>();
      MinionResume component2 = minion2.GetComponent<MinionResume>();
      return (Db.Get().Attributes.QualityOfLife.Lookup((Component) component1).GetTotalValue() / Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component1).GetTotalValue()).CompareTo(Db.Get().Attributes.QualityOfLife.Lookup((Component) component2).GetTotalValue() / Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component2).GetTotalValue());
    }));
    this.ReorderEntries(minionWidgets, this.sortReversed);
  }

  private void SortByMinon()
  {
    this.SelectSortToggle(this.dupeSortingToggle);
    List<SkillMinionWidget> minionWidgets = this.minionWidgets;
    minionWidgets.Sort((Comparison<SkillMinionWidget>) ((a, b) => a.minion.GetProperName().CompareTo(b.minion.GetProperName())));
    this.ReorderEntries(minionWidgets, this.sortReversed);
  }

  private void SortByExperience()
  {
    this.SelectSortToggle(this.experienceSortingToggle);
    List<SkillMinionWidget> minionWidgets = this.minionWidgets;
    minionWidgets.Sort((Comparison<SkillMinionWidget>) ((a, b) =>
    {
      MinionIdentity minion1 = a.minion as MinionIdentity;
      MinionIdentity minion2 = b.minion as MinionIdentity;
      if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null && (UnityEngine.Object) minion2 == (UnityEngine.Object) null)
        return 0;
      if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null)
        return -1;
      if ((UnityEngine.Object) minion2 == (UnityEngine.Object) null)
        return 1;
      MinionResume component1 = minion1.GetComponent<MinionResume>();
      MinionResume component2 = minion2.GetComponent<MinionResume>();
      return ((float) (component1.AvailableSkillpoints / (component1.TotalSkillPointsGained + 1))).CompareTo((float) (component2.AvailableSkillpoints / (component2.TotalSkillPointsGained + 1)));
    }));
    this.ReorderEntries(minionWidgets, this.sortReversed);
  }

  protected void ReorderEntries(List<SkillMinionWidget> sortedEntries, bool reverse)
  {
    for (int index = 0; index < sortedEntries.Count; ++index)
    {
      if (reverse)
        sortedEntries[index].transform.SetSiblingIndex(sortedEntries.Count - 1 - index);
      else
        sortedEntries[index].transform.SetSiblingIndex(index);
    }
  }

  private void SetPortraitAnimator(IAssignableIdentity identity)
  {
    if (identity == null || identity.IsNull())
      return;
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
    {
      this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject, false).GetComponent<KBatchedAnimController>();
      this.animController.gameObject.SetActive(true);
      this.animController.animScale = this.baseCharacterScale * (1f / UnityEngine.Object.FindObjectOfType<KCanvasScaler>().GetCanvasScale());
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    }
    string str = string.Empty;
    Accessorizer component = this.animController.GetComponent<Accessorizer>();
    for (int index = component.GetAccessories().Count - 1; index >= 0; --index)
      component.RemoveAccessory(component.GetAccessories()[index].Get());
    MinionIdentity minionIdentity = identity as MinionIdentity;
    StoredMinionIdentity storedMinionIdentity = identity as StoredMinionIdentity;
    Accessorizer accessorizer = (Accessorizer) null;
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      accessorizer = minionIdentity.GetComponent<Accessorizer>();
      foreach (ResourceRef<Accessory> accessory in accessorizer.GetAccessories())
        component.AddAccessory(accessory.Get());
      str = minionIdentity.GetComponent<MinionResume>().CurrentHat;
    }
    else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
    {
      foreach (ResourceRef<Accessory> accessory in storedMinionIdentity.accessories)
        component.AddAccessory(accessory.Get());
      str = storedMinionIdentity.currentHat;
    }
    this.idle_anim = Assets.GetAnim((HashedString) "anim_idle_healthy_kanim");
    if ((UnityEngine.Object) this.idle_anim != (UnityEngine.Object) null)
      this.animController.AddAnimOverrides(this.idle_anim, 0.0f);
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop, 1f, 0.0f);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, !string.IsNullOrEmpty(str));
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(str));
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(str));
    KAnim.Build.Symbol source_symbol1 = (KAnim.Build.Symbol) null;
    KAnim.Build.Symbol source_symbol2 = (KAnim.Build.Symbol) null;
    if ((bool) ((UnityEngine.Object) accessorizer))
    {
      source_symbol1 = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
      source_symbol2 = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
    }
    else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
    {
      source_symbol1 = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
      source_symbol2 = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
    }
    this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HairAlways.targetSymbolId, source_symbol1, 1);
    this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, source_symbol2, 1);
  }

  private void OnResize()
  {
    this.animController.animScale = this.baseCharacterScale * (1f / UnityEngine.Object.FindObjectOfType<KCanvasScaler>().GetCanvasScale());
  }
}
