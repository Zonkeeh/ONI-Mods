// Decompiled with JetBrains decompiler
// Type: AchievementWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class AchievementWidget : KMonoBehaviour
{
  private Color color_dark_red = new Color(0.282353f, 0.1607843f, 0.1490196f);
  private Color color_gold = new Color(1f, 0.6352941f, 0.2862745f);
  private Color color_dark_grey = new Color(0.2156863f, 0.2156863f, 0.2156863f);
  private Color color_grey = new Color(0.6901961f, 0.6901961f, 0.6901961f);
  [SerializeField]
  private RectTransform sheenTransform;
  public AnimationCurve flourish_iconScaleCurve;
  public AnimationCurve flourish_sheenPositionCurve;
  public KBatchedAnimController[] sparks;
  [SerializeField]
  private RectTransform progressParent;
  [SerializeField]
  private GameObject requirementPrefab;
  [SerializeField]
  private Sprite statusSuccessIcon;
  [SerializeField]
  private Sprite statusFailureIcon;
  private int numRequirementsDisplayed;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.ExpandAchievement());
  }

  private void Update()
  {
  }

  private void ExpandAchievement()
  {
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null))
      return;
    this.progressParent.gameObject.SetActive(!this.progressParent.gameObject.activeSelf);
  }

  public void ActivateNewlyAchievedFlourish(float delay = 1f)
  {
    this.StartCoroutine(this.Flourish(delay));
  }

  [DebuggerHidden]
  private IEnumerator Flourish(float startDelay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AchievementWidget.\u003CFlourish\u003Ec__Iterator0()
    {
      startDelay = startDelay,
      \u0024this = this
    };
  }

  public void SetAchievedNow()
  {
    this.GetComponent<MultiToggle>().ChangeState(1);
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("iconBG").color = this.color_dark_red;
    component.GetReference<Image>("iconBorder").color = this.color_gold;
    component.GetReference<Image>("icon").color = this.color_gold;
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.color = Color.white;
    this.ConfigureToolTip(this.GetComponent<ToolTip>(), (string) COLONY_ACHIEVEMENTS.ACHIEVED_THIS_COLONY_TOOLTIP);
  }

  public void SetAchievedBefore()
  {
    this.GetComponent<MultiToggle>().ChangeState(1);
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("iconBG").color = this.color_dark_red;
    component.GetReference<Image>("iconBorder").color = this.color_gold;
    component.GetReference<Image>("icon").color = this.color_gold;
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.color = Color.white;
    this.ConfigureToolTip(this.GetComponent<ToolTip>(), (string) COLONY_ACHIEVEMENTS.ACHIEVED_OTHER_COLONY_TOOLTIP);
  }

  public void SetNeverAchieved()
  {
    this.GetComponent<MultiToggle>().ChangeState(2);
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("iconBG").color = this.color_dark_grey;
    component.GetReference<Image>("iconBorder").color = this.color_grey;
    component.GetReference<Image>("icon").color = this.color_grey;
    foreach (LocText componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.color = new Color(componentsInChild.color.r, componentsInChild.color.g, componentsInChild.color.b, 0.6f);
    this.ConfigureToolTip(this.GetComponent<ToolTip>(), (string) COLONY_ACHIEVEMENTS.NOT_ACHIEVED_EVER);
  }

  public void SetNotAchieved()
  {
    this.GetComponent<MultiToggle>().ChangeState(2);
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("iconBG").color = this.color_dark_grey;
    component.GetReference<Image>("iconBorder").color = this.color_grey;
    component.GetReference<Image>("icon").color = this.color_grey;
    foreach (LocText componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.color = new Color(componentsInChild.color.r, componentsInChild.color.g, componentsInChild.color.b, 0.6f);
    this.ConfigureToolTip(this.GetComponent<ToolTip>(), (string) COLONY_ACHIEVEMENTS.NOT_ACHIEVED_THIS_COLONY);
  }

  public void SetFailed()
  {
    this.GetComponent<MultiToggle>().ChangeState(2);
    HierarchyReferences component = this.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("iconBG").color = this.color_dark_grey;
    component.GetReference<Image>("iconBG").SetAlpha(0.5f);
    component.GetReference<Image>("iconBorder").color = this.color_grey;
    component.GetReference<Image>("iconBorder").SetAlpha(0.5f);
    component.GetReference<Image>("icon").color = this.color_grey;
    component.GetReference<Image>("icon").SetAlpha(0.5f);
    foreach (LocText componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.color = new Color(componentsInChild.color.r, componentsInChild.color.g, componentsInChild.color.b, 0.25f);
    this.ConfigureToolTip(this.GetComponent<ToolTip>(), (string) COLONY_ACHIEVEMENTS.FAILED_THIS_COLONY);
  }

  private void ConfigureToolTip(ToolTip tooltip, string status)
  {
    tooltip.ClearMultiStringTooltip();
    tooltip.AddMultiStringTooltip(status, (ScriptableObject) null);
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null) || this.progressParent.gameObject.activeSelf)
      return;
    tooltip.AddMultiStringTooltip((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXPAND_TOOLTIP, (ScriptableObject) null);
  }

  public void ShowProgress(ColonyAchievementStatus achievement)
  {
    if ((UnityEngine.Object) this.progressParent == (UnityEngine.Object) null)
      return;
    this.numRequirementsDisplayed = 0;
    for (int index = 0; index < achievement.Requirements.Count; ++index)
    {
      ColonyAchievementRequirement requirement = achievement.Requirements[index];
      if (requirement is CritterTypesWithTraits)
        this.ShowCritterChecklist(requirement);
      else if (requirement is DupesCompleteChoreInExoSuitForCycles)
        this.ShowDupesInExoSuitsRequirement(achievement.success, requirement);
      else if (requirement is DupesVsSolidTransferArmFetch)
        this.ShowArmsOutPeformingDupesRequirement(achievement.success, requirement);
      else if (requirement is ProduceXEngeryWithoutUsingYList)
        this.ShowEngeryWithoutUsing(achievement.success, requirement);
      else if (requirement is MinimumMorale)
        this.ShowMinimumMoraleRequirement(achievement.success, requirement);
      else
        this.ShowRequirement(achievement.success, requirement);
    }
  }

  private HierarchyReferences GetNextRequirementWidget()
  {
    GameObject gameObject;
    if (this.progressParent.childCount <= this.numRequirementsDisplayed)
    {
      gameObject = Util.KInstantiateUI(this.requirementPrefab, this.progressParent.gameObject, true);
    }
    else
    {
      gameObject = this.progressParent.GetChild(this.numRequirementsDisplayed).gameObject;
      gameObject.SetActive(true);
    }
    ++this.numRequirementsDisplayed;
    return gameObject.GetComponent<HierarchyReferences>();
  }

  private void SetDescription(string str, HierarchyReferences refs)
  {
    refs.GetReference<LocText>("Desc").SetText(str);
  }

  private void SetIcon(Sprite sprite, Color color, HierarchyReferences refs)
  {
    Image reference = refs.GetReference<Image>("Icon");
    reference.sprite = sprite;
    reference.color = color;
    reference.gameObject.SetActive(true);
  }

  private void ShowIcon(bool show, HierarchyReferences refs)
  {
    refs.GetReference<Image>("Icon").gameObject.SetActive(show);
  }

  private void ShowRequirement(bool succeed, ColonyAchievementRequirement req)
  {
    HierarchyReferences requirementWidget = this.GetNextRequirementWidget();
    bool complete = req.Success() || succeed;
    bool flag = req.Fail();
    if (complete && !flag)
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget);
    else if (flag)
      this.SetIcon(this.statusFailureIcon, Color.red, requirementWidget);
    else
      this.ShowIcon(false, requirementWidget);
    this.SetDescription(req.GetProgress(complete), requirementWidget);
  }

  private void ShowCritterChecklist(ColonyAchievementRequirement req)
  {
    CritterTypesWithTraits critterTypesWithTraits = req as CritterTypesWithTraits;
    if (req == null)
      return;
    foreach (KeyValuePair<Tag, bool> keyValuePair in critterTypesWithTraits.critterTypesToCheck)
    {
      HierarchyReferences requirementWidget = this.GetNextRequirementWidget();
      if (keyValuePair.Value)
        this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget);
      else
        this.ShowIcon(false, requirementWidget);
      this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TAME_A_CRITTER, (object) (Tag) keyValuePair.Key.Name.ProperName()), requirementWidget);
    }
  }

  private void ShowArmsOutPeformingDupesRequirement(bool succeed, ColonyAchievementRequirement req)
  {
    DupesVsSolidTransferArmFetch transferArmFetch = req as DupesVsSolidTransferArmFetch;
    if (transferArmFetch == null)
      return;
    HierarchyReferences requirementWidget1 = this.GetNextRequirementWidget();
    if (succeed)
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget1);
    this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_PERFORMANCE, (object) (!succeed ? transferArmFetch.currentCycleCount : transferArmFetch.numCycles), (object) transferArmFetch.numCycles), requirementWidget1);
    if (succeed)
      return;
    Dictionary<int, int> dupeChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchDupeChoreDeliveries;
    Dictionary<int, int> automatedChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchAutomatedChoreDeliveries;
    int num1 = 0;
    dupeChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num1);
    int num2 = 0;
    automatedChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num2);
    HierarchyReferences requirementWidget2 = this.GetNextRequirementWidget();
    if ((double) num1 < (double) num2 * (double) transferArmFetch.percentage)
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget2);
    else
      this.ShowIcon(false, requirementWidget2);
    this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_VS_DUPE_FETCHES, (object) "SolidTransferArm", (object) num2, (object) num1), requirementWidget2);
  }

  private void ShowDupesInExoSuitsRequirement(bool succeed, ColonyAchievementRequirement req)
  {
    DupesCompleteChoreInExoSuitForCycles exoSuitForCycles = req as DupesCompleteChoreInExoSuitForCycles;
    if (exoSuitForCycles == null)
      return;
    HierarchyReferences requirementWidget1 = this.GetNextRequirementWidget();
    if (succeed)
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget1);
    else
      this.ShowIcon(false, requirementWidget1);
    this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_CYCLES, (object) (!succeed ? exoSuitForCycles.currentCycleStreak : exoSuitForCycles.numCycles), (object) exoSuitForCycles.numCycles), requirementWidget1);
    if (succeed)
      return;
    HierarchyReferences requirementWidget2 = this.GetNextRequirementWidget();
    int num = exoSuitForCycles.GetNumberOfDupesForCycle(GameClock.Instance.GetCycle());
    if (num >= Components.LiveMinionIdentities.Count)
    {
      num = Components.LiveMinionIdentities.Count;
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget2);
    }
    this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_THIS_CYCLE, (object) num, (object) Components.LiveMinionIdentities.Count), requirementWidget2);
  }

  private void ShowEngeryWithoutUsing(bool succeed, ColonyAchievementRequirement req)
  {
    ProduceXEngeryWithoutUsingYList withoutUsingYlist = req as ProduceXEngeryWithoutUsingYList;
    if (req == null)
      return;
    HierarchyReferences requirementWidget1 = this.GetNextRequirementWidget();
    float productionAmount = withoutUsingYlist.GetProductionAmount(succeed);
    this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.GENERATE_POWER, (object) GameUtil.GetFormattedRoundedJoules(productionAmount), (object) GameUtil.GetFormattedRoundedJoules(withoutUsingYlist.amountToProduce * 1000f)), requirementWidget1);
    if (succeed)
      this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget1);
    else
      this.ShowIcon(false, requirementWidget1);
    foreach (Tag disallowedBuilding in withoutUsingYlist.disallowedBuildings)
    {
      HierarchyReferences requirementWidget2 = this.GetNextRequirementWidget();
      if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(disallowedBuilding))
        this.SetIcon(this.statusFailureIcon, Color.red, requirementWidget2);
      else
        this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget2);
      BuildingDef buildingDef = Assets.GetBuildingDef(disallowedBuilding.Name);
      this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_BUILDING, (object) buildingDef.Name), requirementWidget2);
    }
  }

  private void ShowMinimumMoraleRequirement(bool success, ColonyAchievementRequirement req)
  {
    MinimumMorale minimumMorale = req as MinimumMorale;
    if (minimumMorale == null)
      return;
    if (success)
    {
      this.ShowRequirement(success, req);
    }
    else
    {
      IEnumerator enumerator = Components.MinionAssignablesProxy.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          GameObject targetGameObject = ((MinionAssignablesProxy) enumerator.Current).GetTargetGameObject();
          if ((UnityEngine.Object) targetGameObject != (UnityEngine.Object) null && !targetGameObject.HasTag(GameTags.Dead))
          {
            AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) targetGameObject.GetComponent<MinionModifiers>());
            if (attributeInstance != null)
            {
              HierarchyReferences requirementWidget = this.GetNextRequirementWidget();
              if ((double) attributeInstance.GetTotalValue() >= (double) minimumMorale.minimumMorale)
                this.SetIcon(this.statusSuccessIcon, Color.green, requirementWidget);
              else
                this.ShowIcon(false, requirementWidget);
              this.SetDescription(string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MORALE, (object) targetGameObject.GetProperName(), (object) attributeInstance.GetTotalDisplayValue()), requirementWidget);
            }
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }
}
