// Decompiled with JetBrains decompiler
// Type: SkillWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SkillWidget : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IEventSystemHandler
{
  private List<SkillWidget> prerequisiteSkillWidgets = new List<SkillWidget>();
  private List<Vector2> linePoints = new List<Vector2>();
  [SerializeField]
  private LocText Name;
  [SerializeField]
  private LocText Description;
  [SerializeField]
  private Image TitleBarBG;
  [SerializeField]
  private SkillsScreen skillsScreen;
  [SerializeField]
  private ToolTip tooltip;
  [SerializeField]
  private RectTransform lines_left;
  [SerializeField]
  public RectTransform lines_right;
  [SerializeField]
  private Color header_color_has_skill;
  [SerializeField]
  private Color header_color_can_assign;
  [SerializeField]
  private Color header_color_disabled;
  [SerializeField]
  private Color line_color_default;
  [SerializeField]
  private Color line_color_active;
  [SerializeField]
  private Image hatImage;
  [SerializeField]
  private GameObject borderHighlight;
  [SerializeField]
  private ToolTip masteryCount;
  [SerializeField]
  private GameObject aptitudeBox;
  [SerializeField]
  private GameObject traitDisabledIcon;
  public TextStyleSetting TooltipTextStyle_Header;
  public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;
  private UILineRenderer[] lines;
  public Material defaultMaterial;
  public Material desaturatedMaterial;

  public string skillID { get; private set; }

  public void Refresh(string skillID)
  {
    Skill skill = Db.Get().Skills.Get(skillID);
    if (skill == null)
    {
      Debug.LogWarning((object) ("DbSkills is missing skillId " + skillID));
    }
    else
    {
      this.Name.text = skill.Name;
      LocText name = this.Name;
      name.text = name.text + "\n(" + Db.Get().SkillGroups.Get(skill.skillGroup).Name + ")";
      this.skillID = skillID;
      this.tooltip.SetSimpleTooltip(this.SkillTooltip(skill));
      MinionIdentity currentlySelectedMinion1 = this.skillsScreen.CurrentlySelectedMinion as MinionIdentity;
      StoredMinionIdentity currentlySelectedMinion2 = this.skillsScreen.CurrentlySelectedMinion as StoredMinionIdentity;
      MinionResume minionResume = (MinionResume) null;
      if ((UnityEngine.Object) currentlySelectedMinion1 != (UnityEngine.Object) null)
      {
        minionResume = currentlySelectedMinion1.GetComponent<MinionResume>();
        MinionResume.SkillMasteryConditions[] masteryConditions = minionResume.GetSkillMasteryConditions(skillID);
        bool flag = minionResume.CanMasterSkill(masteryConditions);
        if (!((UnityEngine.Object) minionResume == (UnityEngine.Object) null) && (minionResume.HasMasteredSkill(skillID) || flag))
        {
          this.TitleBarBG.color = !minionResume.HasMasteredSkill(skillID) ? this.header_color_can_assign : this.header_color_has_skill;
          this.hatImage.material = this.defaultMaterial;
        }
        else
        {
          this.TitleBarBG.color = this.header_color_disabled;
          this.hatImage.material = this.desaturatedMaterial;
        }
      }
      else if ((UnityEngine.Object) currentlySelectedMinion2 != (UnityEngine.Object) null)
      {
        if (currentlySelectedMinion2.HasMasteredSkill(skillID))
        {
          this.TitleBarBG.color = this.header_color_has_skill;
          this.hatImage.material = this.defaultMaterial;
        }
        else
        {
          this.TitleBarBG.color = this.header_color_disabled;
          this.hatImage.material = this.desaturatedMaterial;
        }
      }
      this.hatImage.sprite = Assets.GetSprite((HashedString) skill.badge);
      bool flag1 = false;
      if ((UnityEngine.Object) minionResume != (UnityEngine.Object) null)
      {
        float num;
        minionResume.AptitudeBySkillGroup.TryGetValue((HashedString) skill.skillGroup, out num);
        flag1 = (double) num > 0.0;
      }
      this.aptitudeBox.SetActive(flag1);
      this.traitDisabledIcon.SetActive((UnityEngine.Object) minionResume != (UnityEngine.Object) null && !minionResume.IsAbleToLearnSkill(skill.Id));
      string str1 = string.Empty;
      List<string> stringList = new List<string>();
      foreach (Component component1 in Components.LiveMinionIdentities.Items)
      {
        MinionResume component2 = component1.GetComponent<MinionResume>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasMasteredSkill(skillID))
          stringList.Add(component2.GetProperName());
      }
      foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
      {
        foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
        {
          if (info.serializedMinion != null)
          {
            StoredMinionIdentity storedMinionIdentity = info.serializedMinion.Get<StoredMinionIdentity>();
            if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null && storedMinionIdentity.HasMasteredSkill(skillID))
              stringList.Add(storedMinionIdentity.GetProperName());
          }
        }
      }
      this.masteryCount.gameObject.SetActive(stringList.Count > 0);
      foreach (string str2 in stringList)
        str1 = str1 + "\n    • " + str2;
      this.masteryCount.SetSimpleTooltip(stringList.Count <= 0 ? STRINGS.UI.ROLES_SCREEN.WIDGET.NO_MASTERS_TOOLTIP.text : string.Format((string) STRINGS.UI.ROLES_SCREEN.WIDGET.NUMBER_OF_MASTERS_TOOLTIP, (object) str1));
      this.masteryCount.GetComponentInChildren<LocText>().text = stringList.Count.ToString();
    }
  }

  public void RefreshLines()
  {
    this.prerequisiteSkillWidgets.Clear();
    List<Vector2> vector2List = new List<Vector2>();
    foreach (string priorSkill in Db.Get().Skills.Get(this.skillID).priorSkills)
    {
      vector2List.Add(this.skillsScreen.GetSkillWidgetLineTargetPosition(priorSkill));
      this.prerequisiteSkillWidgets.Add(this.skillsScreen.GetSkillWidget(priorSkill));
    }
    if (this.lines != null)
    {
      for (int index = this.lines.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.lines[index].gameObject);
    }
    this.linePoints.Clear();
    for (int index = 0; index < vector2List.Count; ++index)
    {
      float num = (float) ((double) this.lines_left.GetPosition().x - (double) vector2List[index].x - 12.0);
      float y = 0.0f;
      this.linePoints.Add(new Vector2(0.0f, y));
      this.linePoints.Add(new Vector2(-num, y));
      this.linePoints.Add(new Vector2(-num, y));
      this.linePoints.Add(new Vector2(-num, (float) -((double) this.lines_left.GetPosition().y - (double) vector2List[index].y)));
      this.linePoints.Add(new Vector2(-num, (float) -((double) this.lines_left.GetPosition().y - (double) vector2List[index].y)));
      this.linePoints.Add(new Vector2((float) -((double) this.lines_left.GetPosition().x - (double) vector2List[index].x), (float) -((double) this.lines_left.GetPosition().y - (double) vector2List[index].y)));
    }
    this.lines = new UILineRenderer[this.linePoints.Count / 2];
    int index1 = 0;
    for (int index2 = 0; index2 < this.linePoints.Count; index2 += 2)
    {
      GameObject go = new GameObject("Line");
      go.AddComponent<RectTransform>();
      go.transform.SetParent(this.lines_left.transform);
      go.transform.SetLocalPosition(Vector3.zero);
      go.rectTransform().sizeDelta = Vector2.zero;
      this.lines[index1] = go.AddComponent<UILineRenderer>();
      this.lines[index1].color = new Color(0.6509804f, 0.6509804f, 0.6509804f, 1f);
      this.lines[index1].Points = new Vector2[2]
      {
        this.linePoints[index2],
        this.linePoints[index2 + 1]
      };
      ++index1;
    }
  }

  public void ToggleBorderHighlight(bool on)
  {
    this.borderHighlight.SetActive(on);
    if (this.lines != null)
    {
      foreach (UILineRenderer line in this.lines)
      {
        line.color = !on ? this.line_color_default : this.line_color_active;
        line.LineThickness = !on ? 2f : 4f;
        line.SetAllDirty();
      }
    }
    for (int index = 0; index < this.prerequisiteSkillWidgets.Count; ++index)
      this.prerequisiteSkillWidgets[index].ToggleBorderHighlight(on);
  }

  public string SkillTooltip(Skill skill)
  {
    return string.Empty + this.SkillPerksString(skill) + "\n" + this.DuplicantSkillString(skill);
  }

  public string SkillPerksString(Skill skill)
  {
    string str = string.Empty;
    foreach (SkillPerk perk in skill.perks)
    {
      if (!string.IsNullOrEmpty(str))
        str += "\n";
      str = str + "• " + perk.Name;
    }
    return str;
  }

  public string CriteriaString(Skill skill)
  {
    bool flag = false;
    string str = string.Empty + "<b>" + (string) STRINGS.UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.TITLE + "</b>\n";
    SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
    if (skillGroup != null && skillGroup.relevantAttributes != null)
    {
      foreach (Klei.AI.Attribute relevantAttribute in skillGroup.relevantAttributes)
      {
        if (relevantAttribute != null)
        {
          str = str + "    • " + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.SKILLGROUP_ENABLED.DESCRIPTION, (object) relevantAttribute.Name) + "\n";
          flag = true;
        }
      }
    }
    if (skill.priorSkills.Count > 0)
    {
      flag = true;
      for (int index = 0; index < skill.priorSkills.Count; ++index)
      {
        str = str + "    • " + string.Format("{0}", (object) Db.Get().Skills.Get(skill.priorSkills[index]).Name) + "</color>";
        if (index != skill.priorSkills.Count - 1)
          str += "\n";
      }
    }
    if (!flag)
      str = str + "    • " + string.Format((string) STRINGS.UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.NONE, (object) skill.Name);
    return str;
  }

  public string DuplicantSkillString(Skill skill)
  {
    string str = string.Empty;
    MinionIdentity currentlySelectedMinion = this.skillsScreen.CurrentlySelectedMinion as MinionIdentity;
    if ((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null)
    {
      MinionResume component = currentlySelectedMinion.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return string.Empty;
      LocString canMaster = STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CAN_MASTER;
      if (!component.HasMasteredSkill(skill.Id))
      {
        MinionResume.SkillMasteryConditions[] masteryConditions = component.GetSkillMasteryConditions(skill.Id);
        if (!component.CanMasterSkill(masteryConditions))
        {
          bool flag = false;
          str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CANNOT_MASTER, (object) currentlySelectedMinion.GetProperName(), (object) skill.Name);
          if (Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.UnableToLearn)))
          {
            flag = true;
            Trait trait1 = (Trait) null;
            string choreGroupId = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
            if (!string.IsNullOrEmpty(choreGroupId))
            {
              foreach (Trait trait2 in currentlySelectedMinion.GetComponent<Traits>().TraitList)
              {
                if (trait2.disabledChoreGroups != null)
                {
                  foreach (Resource disabledChoreGroup in trait2.disabledChoreGroups)
                  {
                    if (disabledChoreGroup.Id == choreGroupId && trait1 == null)
                      trait1 = trait2;
                  }
                }
              }
            }
            str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.PREVENTED_BY_TRAIT, (object) trait1.Name);
          }
          if (!flag && Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.MissingPreviousSkill)))
            str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_PREVIOUS_SKILLS);
          if (!flag && Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.NeedsSkillPoints)))
            str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_MORE_SKILL_POINTS);
        }
        else
        {
          if (Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.StressWarning)))
            str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.STRESS_WARNING_MESSAGE, (object) skill.Name, (object) currentlySelectedMinion.GetProperName());
          if (Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.SkillAptitude)))
            str = str + "\n" + string.Format((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_APTITUDE, (object) currentlySelectedMinion.GetProperName(), (object) skill.Name);
        }
      }
    }
    return str;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.ToggleBorderHighlight(true);
    this.skillsScreen.HoverSkill(this.skillID);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.ToggleBorderHighlight(false);
    this.skillsScreen.HoverSkill((string) null);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    MinionIdentity currentlySelectedMinion = this.skillsScreen.CurrentlySelectedMinion as MinionIdentity;
    if (!((UnityEngine.Object) currentlySelectedMinion != (UnityEngine.Object) null))
      return;
    MinionResume component = currentlySelectedMinion.GetComponent<MinionResume>();
    if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
      component.ForceAddSkillPoint();
    MinionResume.SkillMasteryConditions[] masteryConditions = component.GetSkillMasteryConditions(this.skillID);
    bool flag = component.CanMasterSkill(masteryConditions);
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.HasMasteredSkill(this.skillID) || !flag)
      return;
    component.MasterSkill(this.skillID);
    this.skillsScreen.RefreshAll();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    MinionResume component = (this.skillsScreen.CurrentlySelectedMinion as MinionIdentity).GetComponent<MinionResume>();
    MinionResume.SkillMasteryConditions[] masteryConditions = component.GetSkillMasteryConditions(this.skillID);
    bool flag = component.CanMasterSkill(masteryConditions);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.HasMasteredSkill(this.skillID) && flag)
      KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
    else
      KFMOD.PlayOneShot(GlobalAssets.GetSound("Negative", false));
  }
}
