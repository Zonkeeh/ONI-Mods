// Decompiled with JetBrains decompiler
// Type: EarlyBird
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

[SkipSaveFileSerialization]
public class EarlyBird : StateMachineComponent<EarlyBird.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<EarlyBird> OnDeathDelegate = new EventSystem.IntraObjectHandler<EarlyBird>((System.Action<EarlyBird, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<EarlyBird> OnRevivedDelegate = new EventSystem.IntraObjectHandler<EarlyBird>((System.Action<EarlyBird, object>) ((component, data) => component.OnRevived(data)));
  [MyCmpReq]
  private KPrefabID kPrefabID;
  private AttributeModifier[] attributeModifiers;

  protected override void OnPrefabInit()
  {
    this.Subscribe<EarlyBird>(1623392196, EarlyBird.OnDeathDelegate);
    this.Subscribe<EarlyBird>(-1117766961, EarlyBird.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.attributeModifiers = new AttributeModifier[11]
    {
      new AttributeModifier("Construction", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Digging", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Machinery", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Athletics", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Learning", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Cooking", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Art", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Strength", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Caring", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Botanist", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
      new AttributeModifier("Ranching", TUNING.TRAITS.EARLYBIRD_MODIFIER, (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true)
    };
    this.smi.StartSM();
  }

  public void ApplyModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Add(attributeModifier);
    }
  }

  public void RemoveModifiers()
  {
    Attributes attributes = this.gameObject.GetAttributes();
    for (int index = 0; index < this.attributeModifiers.Length; ++index)
    {
      AttributeModifier attributeModifier = this.attributeModifiers[index];
      attributes.Remove(attributeModifier);
    }
  }

  private void OnDeath(object data)
  {
    this.enabled = false;
  }

  private void OnRevived(object data)
  {
    this.enabled = true;
  }

  public class StatesInstance : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.GameInstance
  {
    public StatesInstance(EarlyBird master)
      : base(master)
    {
    }

    public bool IsMorning()
    {
      if ((UnityEngine.Object) ScheduleManager.Instance == (UnityEngine.Object) null || this.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
        return false;
      return Schedule.GetBlockIdx() < TUNING.TRAITS.EARLYBIRD_SCHEDULEBLOCK;
    }
  }

  public class States : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird>
  {
    public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State idle;
    public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State early;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.Transition(this.early, (StateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.Transition.ConditionCallback) (smi => smi.IsMorning()), UpdateRate.SIM_200ms);
      this.early.Enter("Morning", (StateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State.Callback) (smi => smi.master.ApplyModifiers())).Exit("NotMorning", (StateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State.Callback) (smi => smi.master.RemoveModifiers())).ToggleStatusItem(Db.Get().DuplicantStatusItems.EarlyMorning, (object) null).ToggleExpression(Db.Get().Expressions.Happy, (Func<EarlyBird.StatesInstance, bool>) null).Transition(this.idle, (StateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.Transition.ConditionCallback) (smi => !smi.IsMorning()), UpdateRate.SIM_200ms);
    }
  }
}
