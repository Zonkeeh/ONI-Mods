// Decompiled with JetBrains decompiler
// Type: NightOwl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

[SkipSaveFileSerialization]
public class NightOwl : StateMachineComponent<NightOwl.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<NightOwl> OnDeathDelegate = new EventSystem.IntraObjectHandler<NightOwl>((System.Action<NightOwl, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<NightOwl> OnRevivedDelegate = new EventSystem.IntraObjectHandler<NightOwl>((System.Action<NightOwl, object>) ((component, data) => component.OnRevived(data)));
  [MyCmpReq]
  private KPrefabID kPrefabID;
  private AttributeModifier[] attributeModifiers;

  protected override void OnPrefabInit()
  {
    this.Subscribe<NightOwl>(1623392196, NightOwl.OnDeathDelegate);
    this.Subscribe<NightOwl>(-1117766961, NightOwl.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.attributeModifiers = new AttributeModifier[11]
    {
      new AttributeModifier("Construction", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Digging", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Machinery", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Athletics", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Learning", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Cooking", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Art", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Strength", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Caring", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Botanist", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
      new AttributeModifier("Ranching", TUNING.TRAITS.NIGHTOWL_MODIFIER, (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true)
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

  public void ModifyTrait(Trait t)
  {
  }

  public class StatesInstance : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.GameInstance
  {
    public StatesInstance(NightOwl master)
      : base(master)
    {
    }

    public bool IsNight()
    {
      if ((UnityEngine.Object) GameClock.Instance == (UnityEngine.Object) null || this.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
        return false;
      return GameClock.Instance.IsNighttime();
    }
  }

  public class States : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl>
  {
    public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State idle;
    public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State early;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.Transition(this.early, (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.Transition.ConditionCallback) (smi => smi.IsNight()), UpdateRate.SIM_200ms);
      this.early.Enter("Night", (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State.Callback) (smi => smi.master.ApplyModifiers())).Exit("NotNight", (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State.Callback) (smi => smi.master.RemoveModifiers())).ToggleStatusItem(Db.Get().DuplicantStatusItems.NightTime, (object) null).ToggleExpression(Db.Get().Expressions.Happy, (Func<NightOwl.StatesInstance, bool>) null).Transition(this.idle, (StateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.Transition.ConditionCallback) (smi => !smi.IsNight()), UpdateRate.SIM_200ms);
    }
  }
}
