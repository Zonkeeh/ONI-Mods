// Decompiled with JetBrains decompiler
// Type: ColonyAchievementTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, ISim33ms
{
  private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>((System.Action<ColonyAchievementTracker, object>) ((component, data) => component.OnNewDay(data)));
  public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();
  [Serialize]
  public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();
  [Serialize]
  public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();
  [Serialize]
  public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();
  private int forceCheckAchievementHandle = -1;
  [Serialize]
  private List<string> completedAchievementsToDisplay = new List<string>();
  private List<string> newlyCompletedAchievements = new List<string>();
  private SchedulerHandle checkAchievementsHandle;
  [Serialize]
  private int updatingAchievement;
  private SchedulerHandle victorySchedulerHandle;

  public List<string> achievementsToDisplay
  {
    get
    {
      return this.completedAchievementsToDisplay;
    }
  }

  public void ClearDisplayAchievements()
  {
    this.achievementsToDisplay.Clear();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (!this.achievements.ContainsKey(resource.Id))
      {
        ColonyAchievementStatus achievementStatus = new ColonyAchievementStatus();
        achievementStatus.SetRequirements(resource.requirementChecklist);
        this.achievements.Add(resource.Id, achievementStatus);
      }
    }
    this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new System.Action<object>(this.CheckAchievements));
    this.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
  }

  public void Sim33ms(float dt)
  {
    if (this.updatingAchievement >= this.achievements.Count)
      this.updatingAchievement = 0;
    KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt<KeyValuePair<string, ColonyAchievementStatus>>(this.updatingAchievement);
    ++this.updatingAchievement;
    if (keyValuePair.Value.success || keyValuePair.Value.failed)
      return;
    keyValuePair.Value.UpdateAchievement();
    if (!keyValuePair.Value.success || keyValuePair.Value.failed)
      return;
    ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
    this.completedAchievementsToDisplay.Add(keyValuePair.Key);
    this.TriggerNewAchievementCompleted((GameObject) null);
    RetireColonyUtility.SaveColonySummaryData();
  }

  private void CheckAchievements(object data = null)
  {
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement in this.achievements)
    {
      if (!achievement.Value.success && !achievement.Value.failed)
      {
        achievement.Value.UpdateAchievement();
        if (achievement.Value.success && !achievement.Value.failed)
          this.newlyCompletedAchievements.Add(achievement.Key);
      }
    }
    if (this.newlyCompletedAchievements.Count > 0)
    {
      foreach (string completedAchievement in this.newlyCompletedAchievements)
      {
        ColonyAchievementTracker.UnlockPlatformAchievement(completedAchievement);
        this.completedAchievementsToDisplay.Add(completedAchievement);
      }
      this.TriggerNewAchievementCompleted((GameObject) null);
      RetireColonyUtility.SaveColonySummaryData();
    }
    this.newlyCompletedAchievements.Clear();
  }

  private static void UnlockPlatformAchievement(string achievement_id)
  {
    if (DebugHandler.InstantBuildMode)
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", (object) achievement_id);
    else if (SaveGame.Instance.sandboxEnabled)
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", (object) achievement_id);
    else if (Game.Instance.debugWasUsed)
    {
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", (object) achievement_id);
    }
    else
    {
      ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
      if (colonyAchievement == null || string.IsNullOrEmpty(colonyAchievement.steamAchievementId))
        return;
      if ((bool) ((UnityEngine.Object) SteamAchievementService.Instance))
        SteamAchievementService.Instance.Unlock(colonyAchievement.steamAchievementId);
      else
        Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", (object) colonyAchievement.steamAchievementId);
    }
  }

  public void DebugTriggerAchievement(string id)
  {
    this.newlyCompletedAchievements.Add(id);
    this.achievements[id].failed = false;
    this.achievements[id].success = true;
  }

  private void BeginVictorySequence(string achievementID)
  {
    RootMenu.Instance.canTogglePauseScreen = false;
    CameraController.Instance.DisableUserCameraControl = true;
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
    this.ToggleVictoryUI(true);
    StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject, (GameObject) null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<StoryMessageScreen>();
    component.restoreInterfaceOnClose = false;
    component.title = (string) COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
    component.body = string.Format((string) COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, (object) ("<b>" + Db.Get().ColonyAchievements.Get(achievementID).Name + "</b>\n" + Db.Get().ColonyAchievements.Get(achievementID).description));
    component.Show(true);
    CameraController.Instance.SetWorldInteractive(false);
    component.OnClose += (System.Action) (() =>
    {
      SpeedControlScreen.Instance.SetSpeed(1);
      if (!SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      CameraController.Instance.SetWorldInteractive(true);
      Db.Get().ColonyAchievements.Get(achievementID).victorySequence((KMonoBehaviour) this);
    });
  }

  protected override void OnCleanUp()
  {
    this.victorySchedulerHandle.ClearScheduler();
    Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
    this.checkAchievementsHandle.ClearScheduler();
    base.OnCleanUp();
  }

  private void TriggerNewAchievementCompleted(GameObject cameraTarget = null)
  {
    bool flag = false;
    for (int index = 0; index < this.newlyCompletedAchievements.Count; ++index)
    {
      if (Db.Get().ColonyAchievements.Get(this.newlyCompletedAchievements[index]).isVictoryCondition)
      {
        flag = true;
        this.BeginVictorySequence(this.newlyCompletedAchievements[index]);
        break;
      }
    }
    if (flag)
      return;
    AchievementEarnedMessage achievementEarnedMessage = new AchievementEarnedMessage();
    Messenger.Instance.QueueMessage((Message) achievementEarnedMessage);
  }

  private void ToggleVictoryUI(bool victoryUIActive)
  {
    List<KScreen> kscreenList = new List<KScreen>();
    kscreenList.Add((KScreen) NotificationScreen.Instance);
    kscreenList.Add((KScreen) OverlayMenu.Instance);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      kscreenList.Add((KScreen) PlanScreen.Instance);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      kscreenList.Add((KScreen) BuildMenu.Instance);
    kscreenList.Add((KScreen) ManagementMenu.Instance);
    kscreenList.Add((KScreen) ToolMenu.Instance);
    kscreenList.Add((KScreen) ToolMenu.Instance.PriorityScreen);
    kscreenList.Add((KScreen) ResourceCategoryScreen.Instance);
    kscreenList.Add((KScreen) TopLeftControlScreen.Instance);
    kscreenList.Add((KScreen) DateTime.Instance);
    kscreenList.Add((KScreen) BuildWatermark.Instance);
    kscreenList.Add((KScreen) HoverTextScreen.Instance);
    kscreenList.Add((KScreen) DetailsScreen.Instance);
    kscreenList.Add((KScreen) DebugPaintElementScreen.Instance);
    kscreenList.Add((KScreen) DebugBaseTemplateButton.Instance);
    kscreenList.Add((KScreen) StarmapScreen.Instance);
    foreach (KScreen kscreen in kscreenList)
    {
      if ((UnityEngine.Object) kscreen != (UnityEngine.Object) null)
        kscreen.Show(!victoryUIActive);
    }
  }

  public void Serialize(BinaryWriter writer)
  {
    writer.Write(this.achievements.Count);
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement in this.achievements)
    {
      writer.WriteKleiString(achievement.Key);
      achievement.Value.Serialize(writer);
    }
  }

  public void Deserialize(IReader reader)
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
      return;
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      string str = reader.ReadKleiString();
      ColonyAchievementStatus achievementStatus = new ColonyAchievementStatus();
      achievementStatus.Deserialize(reader);
      if (Db.Get().ColonyAchievements.Exists(str))
        this.achievements.Add(str, achievementStatus);
    }
  }

  public void LogFetchChore(GameObject fetcher, ChoreType choreType)
  {
    if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || (choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch) || choreType == Db.Get().ChoreTypes.Transport)
      return;
    Dictionary<int, int> dictionary1 = (Dictionary<int, int>) null;
    if ((UnityEngine.Object) fetcher.GetComponent<SolidTransferArm>() != (UnityEngine.Object) null)
      dictionary1 = this.fetchAutomatedChoreDeliveries;
    else if ((UnityEngine.Object) fetcher.GetComponent<MinionIdentity>() != (UnityEngine.Object) null)
      dictionary1 = this.fetchDupeChoreDeliveries;
    if (dictionary1 == null)
      return;
    int cycle = GameClock.Instance.GetCycle();
    if (!dictionary1.ContainsKey(cycle))
      dictionary1.Add(cycle, 0);
    Dictionary<int, int> dictionary2;
    int index;
    (dictionary2 = dictionary1)[index = cycle] = dictionary2[index] + 1;
  }

  public void LogSuitChore(ChoreDriver driver)
  {
    if ((UnityEngine.Object) driver == (UnityEngine.Object) null || (UnityEngine.Object) driver.GetComponent<MinionIdentity>() == (UnityEngine.Object) null)
      return;
    bool flag = false;
    foreach (AssignableSlotInstance slot in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((bool) ((UnityEngine.Object) assignable))
      {
        KPrefabID component = assignable.GetComponent<KPrefabID>();
        if (component.HasTag(GameTags.AtmoSuit) || component.HasTag(GameTags.JetSuit))
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag)
      return;
    int cycle = GameClock.Instance.GetCycle();
    int instanceId = driver.GetComponent<KPrefabID>().InstanceID;
    if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
    {
      this.dupesCompleteChoresInSuits.Add(cycle, new List<int>()
      {
        instanceId
      });
    }
    else
    {
      if (this.dupesCompleteChoresInSuits[cycle].Contains(instanceId))
        return;
      this.dupesCompleteChoresInSuits[cycle].Add(instanceId);
    }
  }

  public void OnNewDay(object data)
  {
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      if ((UnityEngine.Object) minionStorage.GetComponent<CommandModule>() != (UnityEngine.Object) null)
      {
        List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
        if (storedMinionInfo.Count > 0)
        {
          int cycle = GameClock.Instance.GetCycle();
          if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
            this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
          for (int index = 0; index < storedMinionInfo.Count; ++index)
          {
            KPrefabID kprefabId = storedMinionInfo[index].serializedMinion.Get();
            if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
              this.dupesCompleteChoresInSuits[cycle].Add(kprefabId.InstanceID);
          }
        }
      }
    }
  }
}
