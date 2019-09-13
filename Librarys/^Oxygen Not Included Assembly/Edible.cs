// Decompiled with JetBrains decompiler
// Type: Edible
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Edible : Workable, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>((System.Action<Edible, object>) ((component, data) => component.OnCraft(data)));
  private static readonly HashedString[] normalWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[2]
  {
    (HashedString) "hat_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] saltWorkAnims = new HashedString[2]
  {
    (HashedString) "salt_pre",
    (HashedString) "salt_loop"
  };
  private static readonly HashedString[] saltHatWorkAnims = new HashedString[2]
  {
    (HashedString) "salt_hat_pre",
    (HashedString) "salt_hat_loop"
  };
  private static readonly HashedString normalWorkPstAnim = (HashedString) "working_pst";
  private static readonly HashedString hatWorkPstAnim = (HashedString) "hat_pst";
  private static readonly HashedString saltWorkPstAnim = (HashedString) "salt_pst";
  private static readonly HashedString saltHatWorkPstAnim = (HashedString) "salt_hat_pst";
  private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>()
  {
    {
      -1,
      "EdibleMinus3"
    },
    {
      0,
      "EdibleMinus2"
    },
    {
      1,
      "EdibleMinus1"
    },
    {
      2,
      "Edible0"
    },
    {
      3,
      "Edible1"
    },
    {
      4,
      "Edible2"
    },
    {
      5,
      "Edible3"
    }
  };
  private float consumptionTime = float.NaN;
  public float unitsConsumed = float.NaN;
  public float caloriesConsumed = float.NaN;
  private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, (string) DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, false, true);
  public string FoodID;
  private EdiblesManager.FoodInfo foodInfo;

  private Edible()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.shouldTransferDiseaseWithWorker = false;
  }

  public float Units
  {
    get
    {
      return this.GetComponent<PrimaryElement>().Units;
    }
    set
    {
      this.GetComponent<PrimaryElement>().Units = value;
    }
  }

  public float Calories
  {
    get
    {
      return this.Units * this.foodInfo.CaloriesPerUnit;
    }
    set
    {
      this.Units = value / this.foodInfo.CaloriesPerUnit;
    }
  }

  public EdiblesManager.FoodInfo FoodInfo
  {
    get
    {
      return this.foodInfo;
    }
    set
    {
      this.foodInfo = value;
      this.FoodID = this.foodInfo.Id;
    }
  }

  public bool isBeingConsumed { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.foodInfo == null)
    {
      if (this.FoodID == null)
        Debug.LogError((object) "No food FoodID");
      this.foodInfo = Game.Instance.ediblesManager.GetFoodInfo(this.FoodID);
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Edible, false);
    this.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
    this.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
    this.synchronizeAnims = false;
    Components.Edibles.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, (object) this);
  }

  public override HashedString[] GetWorkAnims(Worker worker)
  {
    EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
    bool flag = smi != null && smi.UseSalt();
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null)
    {
      if (flag)
        return Edible.saltHatWorkAnims;
      return Edible.hatWorkAnims;
    }
    if (flag)
      return Edible.saltWorkAnims;
    return Edible.normalWorkAnims;
  }

  public override HashedString GetWorkPstAnim(
    Worker worker,
    bool successfully_completed)
  {
    EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
    bool flag = smi != null && smi.UseSalt();
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null)
    {
      if (flag)
        return Edible.saltHatWorkPstAnim;
      return Edible.hatWorkPstAnim;
    }
    if (flag)
      return Edible.saltWorkPstAnim;
    return Edible.normalWorkPstAnim;
  }

  private void OnCraft(object data)
  {
    RationTracker.Get().RegisterCaloriesProduced(this.Calories);
  }

  public float GetFeedingTime(Worker worker)
  {
    float num = this.Calories * 2E-05f;
    if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
    {
      BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
      if (smi != null && smi.IsBingeEating())
        num /= 2f;
    }
    return num;
  }

  protected override void OnStartWork(Worker worker)
  {
    this.SetWorkTime(this.GetFeedingTime(worker));
    worker.GetAttributes().Add(this.caloriesModifier);
    worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
    this.StartConsuming();
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    this.consumptionTime += dt;
    return false;
  }

  protected override void OnStopWork(Worker worker)
  {
    worker.GetAttributes().Remove(this.caloriesModifier);
    worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
    this.StopConsuming(worker);
  }

  private void StartConsuming()
  {
    DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started");
    this.isBeingConsumed = true;
    this.consumptionTime = 0.0f;
    this.worker.Trigger(1406130139, (object) this);
  }

  private void StopConsuming(Worker worker)
  {
    DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()");
    this.isBeingConsumed = false;
    if (float.IsNaN(this.consumptionTime))
    {
      DebugUtil.DevAssert(false, "consumptionTime NaN in StopConsuming()");
    }
    else
    {
      PrimaryElement component = this.gameObject.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.DiseaseCount > 0)
      {
        EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_react_contaminated_food_kanim", new HashedString[1]
        {
          (HashedString) "react"
        }, (Func<StatusItem>) null);
      }
      this.unitsConsumed = this.Units * Mathf.Clamp01(this.consumptionTime / this.GetFeedingTime(worker));
      if (float.IsNaN(this.unitsConsumed))
      {
        KCrashReporter.Assert(false, "Why is unitsConsumed NaN?");
        this.unitsConsumed = this.Units;
      }
      this.caloriesConsumed = this.unitsConsumed * this.foodInfo.CaloriesPerUnit;
      this.Units -= this.unitsConsumed;
      for (int index = 0; index < this.foodInfo.Effects.Count; ++index)
        worker.GetComponent<Effects>().Add(this.foodInfo.Effects[index], true);
      ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
      this.AddQualityEffects(worker);
      worker.Trigger(1121894420, (object) this);
      this.Trigger(-10536414, (object) worker.gameObject);
      this.unitsConsumed = float.NaN;
      this.caloriesConsumed = float.NaN;
      this.consumptionTime = float.NaN;
      if ((double) this.Units > 0.0)
        return;
      this.gameObject.DeleteObject();
    }
  }

  public static string GetEffectForFoodQuality(int qualityLevel)
  {
    qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
    return Edible.qualityEffects[qualityLevel];
  }

  private void AddQualityEffects(Worker worker)
  {
    int qualityLevel = this.FoodInfo.Quality + Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
    worker.GetComponent<Effects>().Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Edibles.Remove(this);
  }

  public int GetQuality()
  {
    return this.foodInfo.Quality;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.CALORIES, (object) GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, (object) GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
    descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
    foreach (string effect in this.foodInfo.Effects)
      descriptorList.Add(new Descriptor((string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".NAME"), (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".DESCRIPTION"), Descriptor.DescriptorType.Effect, false));
    return descriptorList;
  }

  public class EdibleStartWorkInfo : Worker.StartWorkInfo
  {
    public EdibleStartWorkInfo(Workable workable, float amount)
      : base(workable)
    {
      this.amount = amount;
    }

    public float amount { get; private set; }
  }
}
