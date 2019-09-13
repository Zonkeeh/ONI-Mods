// Decompiled with JetBrains decompiler
// Type: SeasonManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SeasonManager : KMonoBehaviour, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<SeasonManager> OnNewDayDelegate = new EventSystem.IntraObjectHandler<SeasonManager>((System.Action<SeasonManager, object>) ((component, data) => component.OnNewDay(data)));
  [Serialize]
  private int currentSeasonIndex;
  [Serialize]
  private int currentSeasonsCyclesElapsed;
  [Serialize]
  private float bombardmentPeriodRemaining;
  [Serialize]
  private bool bombardmentOn;
  [Serialize]
  private float secondsUntilNextBombardment;
  private GameObject activeMeteorBackground;
  private const string SEASONNAME_DEFAULT = "Default";
  private const string SEASONNAME_METEORSHOWER_IRON = "MeteorShowerIron";
  private const string SEASONNAME_METEORSHOWER_GOLD = "MeteorShowerGold";
  private const string SEASONNAME_METEORSHOWER_COPPER = "MeteorShowerCopper";
  private Dictionary<string, SeasonManager.Season> seasons;
  private string[] SeasonLoop;

  public SeasonManager()
  {
    Dictionary<string, SeasonManager.Season> dictionary1 = new Dictionary<string, SeasonManager.Season>();
    dictionary1.Add("Default", new SeasonManager.Season()
    {
      durationInCycles = 4
    });
    Dictionary<string, SeasonManager.Season> dictionary2 = dictionary1;
    SeasonManager.Season season1 = new SeasonManager.Season();
    season1.durationInCycles = 10;
    season1.secondsBombardmentOff = new MathUtil.MinMax(300f, 1200f);
    season1.secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
    season1.secondsBetweenBombardments = new MathUtil.MinMax(1f, 1.5f);
    season1.meteorBackground = true;
    // ISSUE: explicit reference operation
    (^ref season1).bombardmentInfo = new SeasonManager.BombardmentInfo[3]
    {
      new SeasonManager.BombardmentInfo()
      {
        prefab = IronCometConfig.ID,
        weight = 1f
      },
      new SeasonManager.BombardmentInfo()
      {
        prefab = RockCometConfig.ID,
        weight = 2f
      },
      new SeasonManager.BombardmentInfo()
      {
        prefab = DustCometConfig.ID,
        weight = 5f
      }
    };
    SeasonManager.Season season2 = season1;
    dictionary2.Add("MeteorShowerIron", season2);
    Dictionary<string, SeasonManager.Season> dictionary3 = dictionary1;
    SeasonManager.Season season3 = new SeasonManager.Season();
    season3.durationInCycles = 5;
    season3.secondsBombardmentOff = new MathUtil.MinMax(800f, 1200f);
    season3.secondsBombardmentOn = new MathUtil.MinMax(50f, 100f);
    season3.secondsBetweenBombardments = new MathUtil.MinMax(0.3f, 0.5f);
    season3.meteorBackground = true;
    // ISSUE: explicit reference operation
    (^ref season3).bombardmentInfo = new SeasonManager.BombardmentInfo[3]
    {
      new SeasonManager.BombardmentInfo()
      {
        prefab = GoldCometConfig.ID,
        weight = 2f
      },
      new SeasonManager.BombardmentInfo()
      {
        prefab = RockCometConfig.ID,
        weight = 0.5f
      },
      new SeasonManager.BombardmentInfo()
      {
        prefab = DustCometConfig.ID,
        weight = 5f
      }
    };
    SeasonManager.Season season4 = season3;
    dictionary3.Add("MeteorShowerGold", season4);
    Dictionary<string, SeasonManager.Season> dictionary4 = dictionary1;
    SeasonManager.Season season5 = new SeasonManager.Season();
    season5.durationInCycles = 7;
    season5.secondsBombardmentOff = new MathUtil.MinMax(300f, 1200f);
    season5.secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
    season5.secondsBetweenBombardments = new MathUtil.MinMax(4f, 6.5f);
    season5.meteorBackground = true;
    // ISSUE: explicit reference operation
    (^ref season5).bombardmentInfo = new SeasonManager.BombardmentInfo[2]
    {
      new SeasonManager.BombardmentInfo()
      {
        prefab = CopperCometConfig.ID,
        weight = 1f
      },
      new SeasonManager.BombardmentInfo()
      {
        prefab = RockCometConfig.ID,
        weight = 1f
      }
    };
    SeasonManager.Season season6 = season5;
    dictionary4.Add("MeteorShowerCopper", season6);
    this.seasons = dictionary1;
    this.SeasonLoop = new string[6]
    {
      "Default",
      "MeteorShowerIron",
      "Default",
      "MeteorShowerCopper",
      "Default",
      "MeteorShowerGold"
    };
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SeasonManager>(631075836, SeasonManager.OnNewDayDelegate);
    if (this.currentSeasonIndex >= this.SeasonLoop.Length)
    {
      this.currentSeasonIndex = this.SeasonLoop.Length - 1;
      this.currentSeasonsCyclesElapsed = int.MaxValue;
    }
    this.UpdateState();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unsubscribe<SeasonManager>(631075836, SeasonManager.OnNewDayDelegate, false);
  }

  private void OnNewDay(object data)
  {
    ++this.currentSeasonsCyclesElapsed;
    this.UpdateState();
  }

  private void UpdateState()
  {
    if (this.currentSeasonsCyclesElapsed < this.seasons[this.SeasonLoop[this.currentSeasonIndex]].durationInCycles)
      return;
    this.currentSeasonIndex = (this.currentSeasonIndex + 1) % this.SeasonLoop.Length;
    this.ResetSeasonProgress();
  }

  private void ResetSeasonProgress()
  {
    SeasonManager.Season season = this.seasons[this.SeasonLoop[this.currentSeasonIndex]];
    this.currentSeasonsCyclesElapsed = 0;
    this.bombardmentOn = false;
    this.bombardmentPeriodRemaining = season.secondsBombardmentOff.Get();
    this.secondsUntilNextBombardment = season.secondsBetweenBombardments.Get();
  }

  public void Sim200ms(float dt)
  {
    SeasonManager.Season season = this.seasons[this.SeasonLoop[this.currentSeasonIndex]];
    this.bombardmentPeriodRemaining -= dt;
    if ((double) this.bombardmentPeriodRemaining <= 0.0)
    {
      float bombardmentPeriodRemaining = this.bombardmentPeriodRemaining;
      this.bombardmentOn = !this.bombardmentOn;
      this.bombardmentPeriodRemaining = !this.bombardmentOn ? season.secondsBombardmentOff.Get() : season.secondsBombardmentOn.Get();
      if ((double) this.bombardmentPeriodRemaining != 0.0)
        this.bombardmentPeriodRemaining += bombardmentPeriodRemaining;
    }
    if (this.bombardmentOn && season.bombardmentInfo != null && season.bombardmentInfo.Length > 0)
    {
      if ((UnityEngine.Object) this.activeMeteorBackground == (UnityEngine.Object) null)
      {
        this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, (GameObject) null, (string) null);
        this.activeMeteorBackground.transform.SetPosition(new Vector3(125f, 435f, 25f));
        this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      }
      this.secondsUntilNextBombardment -= dt;
      if ((double) this.secondsUntilNextBombardment > 0.0)
        return;
      float untilNextBombardment = this.secondsUntilNextBombardment;
      this.DoBombardment(season.bombardmentInfo);
      this.secondsUntilNextBombardment = season.secondsBetweenBombardments.Get();
      if ((double) this.secondsUntilNextBombardment == 0.0)
        return;
      this.secondsUntilNextBombardment += untilNextBombardment;
    }
    else
    {
      if (!((UnityEngine.Object) this.activeMeteorBackground != (UnityEngine.Object) null))
        return;
      ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
      component.Stop();
      if (component.IsAlive())
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.activeMeteorBackground);
      this.activeMeteorBackground = (GameObject) null;
    }
  }

  private void DoBombardment(SeasonManager.BombardmentInfo[] bombardment_info)
  {
    float max = 0.0f;
    foreach (SeasonManager.BombardmentInfo bombardmentInfo in bombardment_info)
      max += bombardmentInfo.weight;
    float num1 = UnityEngine.Random.Range(0.0f, max);
    SeasonManager.BombardmentInfo bombardmentInfo1 = bombardment_info[0];
    int num2 = 0;
    for (; (double) num1 - (double) bombardmentInfo1.weight > 0.0; bombardmentInfo1 = bombardment_info[++num2])
      num1 -= bombardmentInfo1.weight;
    Game.Instance.Trigger(-84771526, (object) null);
    this.SpawnBombard(bombardmentInfo1.prefab);
  }

  private GameObject SpawnBombard(string prefab)
  {
    Vector3 position = new Vector3(UnityEngine.Random.value * (float) Grid.WidthInCells, 1.2f * (float) Grid.HeightInCells, Grid.GetLayerZ(Grid.SceneLayer.FXFront));
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) prefab), position, Quaternion.identity, (GameObject) null, (string) null, true, 0);
    gameObject.SetActive(true);
    return gameObject;
  }

  public bool CurrentSeasonHasBombardment()
  {
    SeasonManager.Season season = this.seasons[this.SeasonLoop[this.currentSeasonIndex]];
    if (season.bombardmentInfo != null)
      return season.bombardmentInfo.Length > 0;
    return false;
  }

  public float TimeUntilNextBombardment()
  {
    if (!this.CurrentSeasonHasBombardment())
      return float.MaxValue;
    if (this.bombardmentOn)
      return 0.0f;
    return this.bombardmentPeriodRemaining;
  }

  public float GetBombardmentDuration()
  {
    if (!this.CurrentSeasonHasBombardment())
      return 0.0f;
    SeasonManager.Season season = this.seasons[this.SeasonLoop[this.currentSeasonIndex]];
    if (this.bombardmentOn)
      return 0.0f;
    return season.secondsBombardmentOn.Get();
  }

  public void ForceBeginMeteorSeasonWithShower()
  {
    for (int index = 0; index < this.SeasonLoop.Length; ++index)
    {
      if (this.SeasonLoop[index] == "MeteorShowerIron")
        this.currentSeasonIndex = index;
    }
    this.ResetSeasonProgress();
    SeasonManager.Season season = this.seasons[this.SeasonLoop[this.currentSeasonIndex]];
    this.bombardmentOn = true;
    this.bombardmentPeriodRemaining = season.secondsBombardmentOn.Get();
  }

  [ContextMenu("Bombard")]
  public void Debug_Bombardment()
  {
    this.DoBombardment(this.seasons[this.SeasonLoop[this.currentSeasonIndex]].bombardmentInfo);
  }

  [ContextMenu("Force Shower")]
  public void Debug_ForceShower()
  {
    this.currentSeasonIndex = Array.IndexOf<string>(this.SeasonLoop, "MeteorShowerIron");
    this.ResetSeasonProgress();
    this.bombardmentOn = true;
    this.bombardmentPeriodRemaining = float.MaxValue;
    this.secondsUntilNextBombardment = 0.0f;
  }

  public void DrawDebugger()
  {
  }

  private struct BombardmentInfo
  {
    public string prefab;
    public float weight;
  }

  private struct Season
  {
    public string name;
    public int durationInCycles;
    public MathUtil.MinMax secondsBombardmentOff;
    public MathUtil.MinMax secondsBombardmentOn;
    public MathUtil.MinMax secondsBetweenBombardments;
    public bool meteorBackground;
    public SeasonManager.BombardmentInfo[] bombardmentInfo;
  }
}
