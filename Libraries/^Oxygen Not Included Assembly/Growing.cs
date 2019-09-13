// Decompiled with JetBrains decompiler
// Type: Growing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>((System.Action<Growing, object>) ((component, data) => component.OnNewGameSpawn(data)));
  private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>((System.Action<Growing, object>) ((component, data) => component.ResetGrowth(data)));
  public bool shouldGrowOld = true;
  public float maxAge = 2400f;
  public float growthTime;
  private AmountInstance maturity;
  private AmountInstance oldAge;
  private AttributeModifier baseMaturityMax;
  [MyCmpGet]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  private Modifiers modifiers;
  [MyCmpReq]
  private ReceptacleMonitor rm;
  private Crop _crop;

  private Crop crop
  {
    get
    {
      if ((UnityEngine.Object) this._crop == (UnityEngine.Object) null)
        this._crop = this.GetComponent<Crop>();
      return this._crop;
    }
  }

  protected override void OnPrefabInit()
  {
    Amounts amounts = this.gameObject.GetAmounts();
    this.maturity = amounts.Add(new AmountInstance(Db.Get().Amounts.Maturity, this.gameObject));
    this.baseMaturityMax = new AttributeModifier(this.maturity.maxAttribute.Id, this.growthTime / 600f, (string) null, false, false, true);
    this.maturity.maxAttribute.Add(this.baseMaturityMax);
    this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, this.gameObject));
    this.oldAge.maxAttribute.ClearModifiers();
    this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, (string) null, false, false, true));
    base.OnPrefabInit();
    this.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
    this.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.gameObject.AddTag(GameTags.GrowingPlant);
  }

  private void OnNewGameSpawn(object data)
  {
    double num = (double) this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0.0f, 1f));
  }

  public void OverrideMaturityLevel(float percent)
  {
    double num = (double) this.maturity.SetValue(this.maturity.GetMax() * percent);
  }

  public bool ReachedNextHarvest()
  {
    return (double) this.PercentOfCurrentHarvest() >= 1.0;
  }

  public bool IsGrown()
  {
    return (double) this.maturity.value == (double) this.maturity.GetMax();
  }

  public bool CanGrow()
  {
    return !this.IsGrown();
  }

  public bool IsGrowing()
  {
    return (double) this.maturity.GetDelta() > 0.0;
  }

  public void ClampGrowthToHarvest()
  {
    this.maturity.value = this.maturity.GetMax();
  }

  public float PercentOfCurrentHarvest()
  {
    return this.maturity.value / this.maturity.GetMax();
  }

  public float TimeUntilNextHarvest()
  {
    return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
  }

  public float DomesticGrowthTime()
  {
    return this.maturity.GetMax() / this.smi.baseGrowingRate.Value;
  }

  public float WildGrowthTime()
  {
    return this.maturity.GetMax() / this.smi.wildGrowingRate.Value;
  }

  public float PercentGrown()
  {
    return this.maturity.value / this.maturity.GetMax();
  }

  public void ResetGrowth(object data = null)
  {
    this.maturity.value = 0.0f;
  }

  public float PercentOldAge()
  {
    if (this.shouldGrowOld)
      return this.oldAge.value / this.oldAge.GetMax();
    return 0.0f;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.GROWTHTIME_SIMPLE, (object) GameUtil.GetFormattedCycles(this.growthTime, string.Empty)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.GROWTHTIME_SIMPLE, (object) GameUtil.GetFormattedCycles(this.growthTime, string.Empty)), Descriptor.DescriptorType.Requirement, false)
    };
  }

  public void ConsumeMass(float mass_to_consume)
  {
    float b = this.maturity.value;
    mass_to_consume = Mathf.Min(mass_to_consume, b);
    this.maturity.value -= mass_to_consume;
    this.gameObject.Trigger(-1793167409, (object) null);
  }

  public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
  {
    public AttributeModifier baseGrowingRate;
    public AttributeModifier wildGrowingRate;
    public AttributeModifier getOldRate;

    public StatesInstance(Growing master)
      : base(master)
    {
      this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, 1f / 600f, (string) CREATURES.STATS.MATURITY.GROWING, false, false, true);
      this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, 0.0004166667f, (string) CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
      this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, !master.shouldGrowOld ? 0.0f : 1f, (string) null, false, false, true);
    }

    public bool IsGrown()
    {
      return this.master.IsGrown();
    }

    public bool ReachedNextHarvest()
    {
      return this.master.ReachedNextHarvest();
    }

    public void ClampGrowthToHarvest()
    {
      this.master.ClampGrowthToHarvest();
    }

    public bool IsWilting()
    {
      if ((UnityEngine.Object) this.master.wiltCondition != (UnityEngine.Object) null)
        return this.master.wiltCondition.IsWilting();
      return false;
    }

    public bool IsSleeping()
    {
      CropSleepingMonitor.Instance smi = this.master.GetSMI<CropSleepingMonitor.Instance>();
      if (smi != null)
        return smi.IsSleeping();
      return false;
    }

    public bool CanExitStalled()
    {
      if (!this.IsWilting())
        return !this.IsSleeping();
      return false;
    }
  }

  public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
  {
    public Growing.States.GrowingStates growing;
    public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;
    public Growing.States.GrownStates grown;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.growing;
      this.serializable = true;
      this.growing.EventTransition(GameHashes.Wilt, this.stalled, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.IsWilting())).EventTransition(GameHashes.CropSleep, this.stalled, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.IsSleeping())).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.master.rm.Replanted)).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => !smi.master.rm.Replanted)).TriggerOnEnter(GameHashes.Grow, (Func<Growing.StatesInstance, object>) null).Update("CheckGrown", (System.Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.ReachedNextHarvest())
          return;
        smi.GoTo((StateMachine.BaseState) this.grown);
      }), UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Func<Growing.StatesInstance, object>) (smi => (object) smi.master.GetComponent<Growing>())).Enter((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi =>
      {
        GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = !smi.master.rm.Replanted ? this.growing.wild : this.growing.planted;
        smi.GoTo((StateMachine.BaseState) state);
      }));
      this.growing.wild.ToggleAttributeModifier("GrowingWild", (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.wildGrowingRate), (Func<Growing.StatesInstance, bool>) null);
      this.growing.planted.ToggleAttributeModifier(nameof (Growing), (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.baseGrowingRate), (Func<Growing.StatesInstance, bool>) null);
      this.stalled.EventTransition(GameHashes.WiltRecover, (GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State) this.growing, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.CanExitStalled())).EventTransition(GameHashes.CropWakeUp, (GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State) this.growing, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.CanExitStalled()));
      double num1;
      this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, (Func<Growing.StatesInstance, object>) null).Update("CheckNotGrown", (System.Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.ReachedNextHarvest())
          return;
        smi.GoTo((StateMachine.BaseState) this.growing);
      }), UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.getOldRate), (Func<Growing.StatesInstance, bool>) null).Enter((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi => smi.ClampGrowthToHarvest())).Exit((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi => num1 = (double) smi.master.oldAge.SetValue(0.0f)));
      this.grown.idle.Update("CheckNotGrown", (System.Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.master.shouldGrowOld || (double) smi.master.oldAge.value < (double) smi.master.oldAge.GetMax())
          return;
        smi.GoTo((StateMachine.BaseState) this.grown.try_self_harvest);
      }), UpdateRate.SIM_4000ms, false);
      this.grown.try_self_harvest.Enter((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi =>
      {
        Harvestable component = smi.master.GetComponent<Harvestable>();
        if ((bool) ((UnityEngine.Object) component) && component.CanBeHarvested)
        {
          bool harvestWhenReady = component.harvestDesignatable.HarvestWhenReady;
          component.ForceCancelHarvest((object) null);
          component.Harvest();
          if (harvestWhenReady && (UnityEngine.Object) component != (UnityEngine.Object) null)
            component.harvestDesignatable.SetHarvestWhenReady(true);
        }
        double num2 = (double) smi.master.maturity.SetValue(0.0f);
        double num3 = (double) smi.master.oldAge.SetValue(0.0f);
      })).GoTo(this.grown.idle);
    }

    public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
    {
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
    }

    public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
    {
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State try_self_harvest;
    }
  }
}
