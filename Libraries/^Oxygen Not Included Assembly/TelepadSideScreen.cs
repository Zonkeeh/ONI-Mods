// Decompiled with JetBrains decompiler
// Type: TelepadSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelepadSideScreen : SideScreenContent
{
  private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();
  [SerializeField]
  private LocText timeLabel;
  [SerializeField]
  private KButton viewImmigrantsBtn;
  [SerializeField]
  private Telepad targetTelepad;
  [SerializeField]
  private KButton viewColonySummaryBtn;
  [SerializeField]
  private Image newAchievementsEarned;
  [SerializeField]
  private KButton openRolesScreenButton;
  [SerializeField]
  private GameObject victoryConditionsContainer;
  [SerializeField]
  private GameObject conditionContainerTemplate;
  [SerializeField]
  private GameObject checkboxLinePrefab;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.viewImmigrantsBtn.onClick += (System.Action) (() =>
    {
      ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
      Game.Instance.Trigger(288942073, (object) null);
    });
    this.viewColonySummaryBtn.onClick += (System.Action) (() =>
    {
      this.newAchievementsEarned.gameObject.SetActive(false);
      RetireColonyUtility.SaveColonySummaryData();
      MainMenu.ActivateRetiredColoniesScreen(PauseScreen.Instance.transform.parent.gameObject, SaveGame.Instance.BaseName, (string[]) null);
    });
    this.openRolesScreenButton.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleSkills());
    this.BuildVictoryConditions();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Telepad>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    Telepad component = target.GetComponent<Telepad>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a telepad associated with it.");
    }
    else
    {
      this.targetTelepad = component;
      if ((UnityEngine.Object) this.targetTelepad != (UnityEngine.Object) null)
        this.gameObject.SetActive(false);
      else
        this.gameObject.SetActive(true);
    }
  }

  private void Update()
  {
    if (!((UnityEngine.Object) this.targetTelepad != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null && GameFlowManager.Instance.IsGameOver())
    {
      this.gameObject.SetActive(false);
      this.timeLabel.text = (string) STRINGS.UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
      this.SetContentState(true);
    }
    else
    {
      if (this.targetTelepad.GetComponent<Operational>().IsOperational)
        this.timeLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, (object) GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining(), "F1"));
      else
        this.gameObject.SetActive(false);
      this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
    }
    this.UpdateVictoryConditions();
    this.UpdateAchievementsUnlocked();
  }

  private void SetContentState(bool isLabel)
  {
    if (this.timeLabel.gameObject.activeInHierarchy != isLabel)
      this.timeLabel.gameObject.SetActive(isLabel);
    if (this.viewImmigrantsBtn.gameObject.activeInHierarchy != isLabel)
      return;
    this.viewImmigrantsBtn.gameObject.SetActive(!isLabel);
  }

  private void BuildVictoryConditions()
  {
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (resource.isVictoryCondition)
      {
        Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
        GameObject parent = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
        parent.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(resource.Name);
        foreach (ColonyAchievementRequirement key in resource.requirementChecklist)
        {
          VictoryColonyAchievementRequirement achievementRequirement = key as VictoryColonyAchievementRequirement;
          if (achievementRequirement != null)
          {
            GameObject gameObject = Util.KInstantiateUI(this.checkboxLinePrefab, parent, true);
            gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(achievementRequirement.Name());
            gameObject.GetComponent<ToolTip>().SetSimpleTooltip(achievementRequirement.Description());
            dictionary.Add(key, gameObject);
          }
          else
            Debug.LogWarning((object) string.Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.", (object) key.GetType().ToString(), (object) resource.Name));
        }
        this.entries.Add(resource.Id, dictionary);
      }
    }
  }

  private void UpdateVictoryConditions()
  {
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (resource.isVictoryCondition)
      {
        foreach (ColonyAchievementRequirement index in resource.requirementChecklist)
          this.entries[resource.Id][index].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = index.Success();
      }
    }
  }

  private void UpdateAchievementsUnlocked()
  {
    if (SaveGame.Instance.GetComponent<ColonyAchievementTracker>().achievementsToDisplay.Count <= 0)
      return;
    this.newAchievementsEarned.gameObject.SetActive(true);
  }
}
