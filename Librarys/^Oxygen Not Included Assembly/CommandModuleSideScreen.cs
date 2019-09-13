// Decompiled with JetBrains decompiler
// Type: CommandModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandModuleSideScreen : SideScreenContent
{
  private Dictionary<RocketLaunchCondition, GameObject> conditionTable = new Dictionary<RocketLaunchCondition, GameObject>();
  private LaunchConditionManager target;
  public GameObject conditionListContainer;
  public GameObject prefabConditionLineItem;
  public MultiToggle destinationButton;
  public MultiToggle debugVictoryButton;
  private SchedulerHandle updateHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ScheduleUpdate();
    this.debugVictoryButton.onClick += (System.Action) (() =>
    {
      SpaceDestination destination = SpacecraftManager.instance.destinations.Find((Predicate<SpaceDestination>) (match => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole));
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
      this.target.Launch(destination);
    });
    this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
  }

  private bool CheckHydrogenRocket()
  {
    RocketModule rocketModule = this.target.rocketModules.Find((Predicate<RocketModule>) (match => (bool) ((UnityEngine.Object) match.GetComponent<RocketEngine>())));
    if ((UnityEngine.Object) rocketModule != (UnityEngine.Object) null)
      return rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
    return false;
  }

  private void ScheduleUpdate()
  {
    this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, (System.Action<object>) (o =>
    {
      this.RefreshConditions();
      this.ScheduleUpdate();
    }), (object) null, (SchedulerGroup) null);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LaunchConditionManager>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<LaunchConditionManager>();
      if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a LaunchConditionManager component");
      }
      else
      {
        this.ClearConditions();
        this.ConfigureConditions();
        this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
      }
    }
  }

  private void ClearConditions()
  {
    foreach (KeyValuePair<RocketLaunchCondition, GameObject> keyValuePair in this.conditionTable)
      Util.KDestroyGameObject(keyValuePair.Value);
    this.conditionTable.Clear();
  }

  private void ConfigureConditions()
  {
    foreach (RocketLaunchCondition launchCondition in this.target.GetLaunchConditionList())
      this.conditionTable.Add(launchCondition, Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true));
    this.RefreshConditions();
  }

  public void RefreshConditions()
  {
    bool flag = false;
    List<RocketLaunchCondition> launchConditionList = this.target.GetLaunchConditionList();
    foreach (RocketLaunchCondition key in launchConditionList)
    {
      if (this.conditionTable.ContainsKey(key))
      {
        GameObject gameObject = this.conditionTable[key];
        HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
        if (key.GetParentCondition() != null && key.GetParentCondition().EvaluateLaunchCondition() == RocketLaunchCondition.LaunchStatus.Failure)
          gameObject.SetActive(false);
        else if (!gameObject.activeSelf)
          gameObject.SetActive(true);
        bool ready = key.EvaluateLaunchCondition() != RocketLaunchCondition.LaunchStatus.Failure;
        component.GetReference<LocText>("Label").text = key.GetLaunchStatusMessage(ready);
        component.GetReference<LocText>("Label").color = !ready ? Color.red : Color.black;
        component.GetReference<Image>("Box").color = !ready ? Color.red : Color.black;
        component.GetReference<Image>("Check").gameObject.SetActive(ready);
        gameObject.GetComponent<ToolTip>().SetSimpleTooltip(key.GetLaunchStatusTooltip(ready));
      }
      else
      {
        flag = true;
        break;
      }
    }
    foreach (KeyValuePair<RocketLaunchCondition, GameObject> keyValuePair in this.conditionTable)
    {
      if (!launchConditionList.Contains(keyValuePair.Key))
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      this.ClearConditions();
      this.ConfigureConditions();
    }
    this.destinationButton.onClick = (System.Action) (() => ManagementMenu.Instance.ToggleStarmap());
  }

  protected override void OnCleanUp()
  {
    this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }
}
