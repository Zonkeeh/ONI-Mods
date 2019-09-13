// Decompiled with JetBrains decompiler
// Type: SkillMinionWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillMinionWidget : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
{
  [SerializeField]
  private SkillsScreen skillsScreen;
  [SerializeField]
  private CrewPortrait portrait;
  [SerializeField]
  private LocText masteryPoints;
  [SerializeField]
  private LocText morale;
  [SerializeField]
  private Image background;
  [SerializeField]
  private Image hat_background;
  [SerializeField]
  private Color selected_color;
  [SerializeField]
  private Color unselected_color;
  [SerializeField]
  private Color hover_color;
  [SerializeField]
  private DropDown hatDropDown;
  [SerializeField]
  private TextStyleSetting TooltipTextStyle_Header;
  [SerializeField]
  private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

  public IAssignableIdentity minion { get; private set; }

  public void SetMinon(IAssignableIdentity identity)
  {
    this.minion = identity;
    this.portrait.SetIdentityObject(this.minion, true);
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.ToggleHover(true);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.ToggleHover(false);
  }

  private void ToggleHover(bool on)
  {
    if (this.skillsScreen.CurrentlySelectedMinion == this.minion)
      return;
    this.SetColor(!on ? this.unselected_color : this.hover_color);
  }

  private void SetColor(Color color)
  {
    this.background.color = color;
    if (this.minion == null || !((UnityEngine.Object) (this.minion as StoredMinionIdentity) != (UnityEngine.Object) null))
      return;
    this.GetComponent<CanvasGroup>().alpha = 0.6f;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    this.skillsScreen.CurrentlySelectedMinion = this.minion;
    KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
  }

  public void Refresh()
  {
    if (this.minion == null)
      return;
    this.portrait.SetIdentityObject(this.minion, true);
    string empty = string.Empty;
    MinionIdentity minion1 = this.minion as MinionIdentity;
    this.hatDropDown.gameObject.SetActive(true);
    string hat;
    if ((UnityEngine.Object) minion1 != (UnityEngine.Object) null)
    {
      MinionResume component = minion1.GetComponent<MinionResume>();
      int availableSkillpoints = component.AvailableSkillpoints;
      int skillPointsGained = component.TotalSkillPointsGained;
      this.masteryPoints.text = availableSkillpoints <= 0 ? "0" : GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString()));
      this.morale.text = string.Format("{0}/{1}", (object) Db.Get().Attributes.QualityOfLife.Lookup((Component) component).GetTotalValue(), (object) Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component).GetTotalValue());
      this.RefreshToolTip(component);
      List<IListableOption> listableOptionList = new List<IListableOption>();
      foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
      {
        if (keyValuePair.Value)
          listableOptionList.Add((IListableOption) new SkillListable(keyValuePair.Key));
      }
      this.hatDropDown.Initialize((IEnumerable<IListableOption>) listableOptionList, new System.Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new System.Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, (object) this.minion);
      hat = !string.IsNullOrEmpty(component.TargetHat) ? component.TargetHat : component.CurrentHat;
    }
    else
    {
      StoredMinionIdentity minion2 = this.minion as StoredMinionIdentity;
      ToolTip component = this.GetComponent<ToolTip>();
      component.ClearMultiStringTooltip();
      component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) minion2.GetStorageReason(), (object) this.minion.GetProperName()), (ScriptableObject) null);
      hat = !string.IsNullOrEmpty(minion2.targetHat) ? minion2.targetHat : minion2.currentHat;
      this.masteryPoints.text = (string) STRINGS.UI.TABLESCREENS.NA;
      this.morale.text = (string) STRINGS.UI.TABLESCREENS.NA;
    }
    this.SetColor(this.skillsScreen.CurrentlySelectedMinion != this.minion ? this.unselected_color : this.selected_color);
    HierarchyReferences component1 = this.GetComponent<HierarchyReferences>();
    this.RefreshHat(hat);
    component1.GetReference("openButton").gameObject.SetActive((UnityEngine.Object) minion1 != (UnityEngine.Object) null);
  }

  private void RefreshToolTip(MinionResume resume)
  {
    if (!((UnityEngine.Object) resume != (UnityEngine.Object) null))
      return;
    AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) resume);
    AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) resume);
    ToolTip component = this.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    component.AddMultiStringTooltip(this.minion.GetProperName() + "\n\n", (ScriptableObject) this.TooltipTextStyle_Header);
    component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.SKILLS_SCREEN.CURRENT_MORALE, (object) attributeInstance1.GetTotalValue(), (object) attributeInstance2.GetTotalValue()), (ScriptableObject) null);
    component.AddMultiStringTooltip("\n" + (string) STRINGS.UI.DETAILTABS.STATS.NAME + "\n\n", (ScriptableObject) this.TooltipTextStyle_Header);
    foreach (AttributeInstance attribute in resume.GetAttributes())
    {
      if (attribute.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
      {
        string str = UIConstants.ColorPrefixWhite;
        if ((double) attribute.GetTotalValue() > 0.0)
          str = UIConstants.ColorPrefixGreen;
        else if ((double) attribute.GetTotalValue() < 0.0)
          str = UIConstants.ColorPrefixRed;
        component.AddMultiStringTooltip("    • " + attribute.Name + ": " + str + (object) attribute.GetTotalValue() + UIConstants.ColorSuffix, (ScriptableObject) null);
      }
    }
  }

  public void RefreshHat(string hat)
  {
    this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) (!string.IsNullOrEmpty(hat) ? hat : "hat_role_none"));
  }

  private void OnHatDropEntryClick(IListableOption skill, object data)
  {
    MinionIdentity minion = this.minion as MinionIdentity;
    if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      return;
    MinionResume component = minion.GetComponent<MinionResume>();
    if (skill != null)
    {
      this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) (skill as SkillListable).skillHat);
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
      this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) "hat_role_none");
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.SetHats(component.CurrentHat, (string) null);
        component.ApplyTargetHat();
      }
    }
    if (this.minion != this.skillsScreen.CurrentlySelectedMinion)
      return;
    this.skillsScreen.selectedHat.sprite = Assets.GetSprite((HashedString) (!string.IsNullOrEmpty(component.TargetHat) ? component.TargetHat : "hat_role_none"));
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
}
