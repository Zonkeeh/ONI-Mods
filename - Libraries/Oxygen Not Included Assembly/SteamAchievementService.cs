// Decompiled with JetBrains decompiler
// Type: SteamAchievementService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Diagnostics;
using UnityEngine;

public class SteamAchievementService : MonoBehaviour
{
  private Callback<UserStatsReceived_t> cbUserStatsReceived;
  private Callback<UserStatsStored_t> cbUserStatsStored;
  private Callback<UserAchievementStored_t> cbUserAchievementStored;
  private bool setupComplete;
  private static SteamAchievementService instance;

  public static SteamAchievementService Instance
  {
    get
    {
      return SteamAchievementService.instance;
    }
  }

  public static void Initialize()
  {
    if (!((Object) SteamAchievementService.instance == (Object) null))
      return;
    GameObject gameObject = GameObject.Find("/SteamManager");
    SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
    if (!((Object) SteamAchievementService.instance == (Object) null))
      return;
    SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
  }

  public void Awake()
  {
    this.setupComplete = false;
    Debug.Assert((Object) SteamAchievementService.instance == (Object) null);
    SteamAchievementService.instance = this;
  }

  private void OnDestroy()
  {
    Debug.Assert((Object) SteamAchievementService.instance == (Object) this);
    SteamAchievementService.instance = (SteamAchievementService) null;
  }

  private void Update()
  {
    if (!SteamManager.Initialized || (Object) Game.Instance != (Object) null || (this.setupComplete || !DistributionPlatform.Initialized))
      return;
    this.Setup();
  }

  private void Setup()
  {
    this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
    this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
    this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
    this.setupComplete = true;
    this.RefreshStats();
  }

  private void RefreshStats()
  {
    SteamUserStats.RequestCurrentStats();
  }

  private void OnUserStatsReceived(UserStatsReceived_t data)
  {
    if (data.m_eResult == EResult.k_EResultOK)
      return;
    DebugUtil.LogWarningArgs((object) nameof (OnUserStatsReceived), (object) data.m_eResult, (object) data.m_steamIDUser);
  }

  private void OnUserStatsStored(UserStatsStored_t data)
  {
    if (data.m_eResult == EResult.k_EResultOK)
      return;
    DebugUtil.LogWarningArgs((object) nameof (OnUserStatsStored), (object) data.m_eResult);
  }

  private void OnUserAchievementStored(UserAchievementStored_t data)
  {
  }

  public void Unlock(string achievement_id)
  {
    bool flag = SteamUserStats.SetAchievement(achievement_id);
    Debug.LogFormat("SetAchievement {0} {1}", (object) achievement_id, (object) flag);
    Debug.LogFormat("StoreStats {0}", (object) SteamUserStats.StoreStats());
  }

  [Conditional("UNITY_EDITOR")]
  [ContextMenu("Reset All Achievements")]
  private void ResetAllAchievements()
  {
    bool flag = SteamUserStats.ResetAllStats(true);
    Debug.LogFormat("ResetAllStats {0}", (object) flag);
    if (!flag)
      return;
    this.RefreshStats();
  }
}
