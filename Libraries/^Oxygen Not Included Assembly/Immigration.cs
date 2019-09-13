// Decompiled with JetBrains decompiler
// Type: Immigration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Immigration : KMonoBehaviour, ISaveLoadable, ISim200ms, IPersonalPriorityManager
{
  [Serialize]
  private Dictionary<HashedString, int> defaultPersonalPriorities = new Dictionary<HashedString, int>();
  [Serialize]
  public float timeBeforeSpawn = float.PositiveInfinity;
  public float[] spawnInterval;
  public int[] spawnTable;
  [Serialize]
  private bool bImmigrantAvailable;
  [Serialize]
  private int spawnIdx;
  [Serialize]
  private bool stopped;
  private CarePackageInfo[] carePackages;
  public static Immigration Instance;
  private const int CYCLE_THRESHOLD_A = 6;
  private const int CYCLE_THRESHOLD_B = 12;
  private const int CYCLE_THRESHOLD_C = 24;
  private const int CYCLE_THRESHOLD_D = 48;

  public static void DestroyInstance()
  {
    Immigration.Instance = (Immigration) null;
  }

  protected override void OnPrefabInit()
  {
    this.bImmigrantAvailable = false;
    Immigration.Instance = this;
    this.timeBeforeSpawn = this.spawnInterval[Math.Min(this.spawnIdx, this.spawnInterval.Length - 1)];
    this.ResetPersonalPriorities();
    this.ConfigureCarePackages();
  }

  private void ConfigureCarePackages()
  {
    this.carePackages = new CarePackageInfo[58]
    {
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, (Func<bool>) (() => this.CycleCondition(12))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(12))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(12))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(24))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(24))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag);
        return false;
      })),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, (Func<bool>) (() =>
      {
        if (this.CycleCondition(48))
          return this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag);
        return false;
      })),
      new CarePackageInfo("PrickleGrassSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("LeafyPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("CactusPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("MushroomSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("PrickleFlowerSeed", 2f, (Func<bool>) null),
      new CarePackageInfo("OxyfernSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("ForestTreeSeed", 1f, (Func<bool>) null),
      new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("SwampLilySeed", 1f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("ColdBreatherSeed", 1f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("SpiceVineSeed", 1f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("FieldRation", 5f, (Func<bool>) null),
      new CarePackageInfo("BasicForagePlant", 6f, (Func<bool>) null),
      new CarePackageInfo("CookedEgg", 3f, (Func<bool>) (() => this.CycleCondition(6))),
      new CarePackageInfo(PrickleFruitConfig.ID, 3f, (Func<bool>) (() => this.CycleCondition(12))),
      new CarePackageInfo("FriedMushroom", 3f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("CookedMeat", 3f, (Func<bool>) (() => this.CycleCondition(48))),
      new CarePackageInfo("SpicyTofu", 3f, (Func<bool>) (() => this.CycleCondition(48))),
      new CarePackageInfo("LightBugBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("HatchBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("PuftBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("SquirrelBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("CrabBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("DreckoBaby", 1f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("Pacu", 8f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("MoleBaby", 1f, (Func<bool>) (() => this.CycleCondition(48))),
      new CarePackageInfo("OilfloaterBaby", 1f, (Func<bool>) (() => this.CycleCondition(48))),
      new CarePackageInfo("LightBugEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("HatchEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("PuftEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("OilfloaterEgg", 3f, (Func<bool>) (() => this.CycleCondition(12))),
      new CarePackageInfo("MoleEgg", 3f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("DreckoEgg", 3f, (Func<bool>) (() => this.CycleCondition(24))),
      new CarePackageInfo("SquirrelEgg", 2f, (Func<bool>) null),
      new CarePackageInfo("BasicCure", 3f, (Func<bool>) null),
      new CarePackageInfo("Funky_Vest", 1f, (Func<bool>) null)
    };
  }

  private bool CycleCondition(int cycle)
  {
    return GameClock.Instance.GetCycle() >= cycle;
  }

  private bool DiscoveredCondition(Tag tag)
  {
    return WorldInventory.Instance.IsDiscovered(tag);
  }

  public bool ImmigrantsAvailable
  {
    get
    {
      return this.bImmigrantAvailable;
    }
  }

  public int EndImmigration()
  {
    this.bImmigrantAvailable = false;
    ++this.spawnIdx;
    int index = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
    this.timeBeforeSpawn = this.spawnInterval[index];
    return this.spawnTable[index];
  }

  public float GetTimeRemaining()
  {
    return this.timeBeforeSpawn;
  }

  public float GetTotalWaitTime()
  {
    return this.spawnInterval[Math.Min(this.spawnIdx, this.spawnInterval.Length - 1)];
  }

  public void Sim200ms(float dt)
  {
    if (this.stopped || this.bImmigrantAvailable)
      return;
    this.timeBeforeSpawn -= dt;
    this.timeBeforeSpawn = Math.Max(this.timeBeforeSpawn, 0.0f);
    if ((double) this.timeBeforeSpawn > 0.0)
      return;
    this.bImmigrantAvailable = true;
  }

  public void Stop()
  {
    this.stopped = true;
    this.bImmigrantAvailable = false;
    this.timeBeforeSpawn = this.spawnInterval[Math.Min(this.spawnIdx, this.spawnInterval.Length - 1)];
  }

  public void Restart()
  {
    this.stopped = false;
  }

  public int GetPersonalPriority(ChoreGroup group)
  {
    int num;
    if (!this.defaultPersonalPriorities.TryGetValue(group.IdHash, out num))
      num = 3;
    return num;
  }

  public CarePackageInfo RandomCarePackage()
  {
    List<CarePackageInfo> carePackageInfoList = new List<CarePackageInfo>();
    foreach (CarePackageInfo carePackage in this.carePackages)
    {
      if (carePackage.requirement == null || carePackage.requirement())
        carePackageInfoList.Add(carePackage);
    }
    return carePackageInfoList[UnityEngine.Random.Range(0, carePackageInfoList.Count)];
  }

  public void SetPersonalPriority(ChoreGroup group, int value)
  {
    this.defaultPersonalPriorities[group.IdHash] = value;
  }

  public int GetAssociatedSkillLevel(ChoreGroup group)
  {
    return 0;
  }

  public void ApplyDefaultPersonalPriorities(GameObject minion)
  {
    IPersonalPriorityManager instance = (IPersonalPriorityManager) Immigration.Instance;
    IPersonalPriorityManager component = (IPersonalPriorityManager) minion.GetComponent<ChoreConsumer>();
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
    {
      int personalPriority = instance.GetPersonalPriority(resource);
      component.SetPersonalPriority(resource, personalPriority);
    }
  }

  public void ResetPersonalPriorities()
  {
    bool personalPriorities = Game.Instance.advancedPersonalPriorities;
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      this.defaultPersonalPriorities[resource.IdHash] = !personalPriorities ? 3 : resource.DefaultPersonalPriority;
  }

  public bool IsChoreGroupDisabled(ChoreGroup g)
  {
    return false;
  }
}
