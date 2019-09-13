// Decompiled with JetBrains decompiler
// Type: DeathMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;
  public DeathMonitor.Dead dead;
  public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    this.serializable = true;
    this.alive.ParamTransition<Death>((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>) this.death, this.dying_duplicant, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>.Callback) ((smi, p) =>
    {
      if (p != null)
        return smi.IsDuplicant;
      return false;
    })).ParamTransition<Death>((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>) this.death, this.dying_creature, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>.Callback) ((smi, p) =>
    {
      if (p != null)
        return !smi.IsDuplicant;
      return false;
    }));
    this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim", 0.0f).ToggleTag(GameTags.Dying).ToggleChore((Func<DeathMonitor.Instance, Chore>) (smi => (Chore) new DieChore(smi.master, this.death.Get(smi))), this.die);
    this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<DeathMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.dead)));
    this.die.ToggleTag(GameTags.Dying).Enter("Die", (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      Death death = this.death.Get(smi);
      if (!smi.IsDuplicant)
        return;
      DeathMessage deathMessage = new DeathMessage(smi.gameObject, death);
      KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized", false), smi.master.transform.GetPosition());
      KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_ST", false));
      Messenger.Instance.QueueMessage((Message) deathMessage);
    })).GoTo((GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State) this.dead);
    this.dead.ToggleAnims("anim_emotes_default_kanim", 0.0f).defaultState = (StateMachine.BaseState) this.dead.ground.TriggerOnEnter(GameHashes.Died, (Func<DeathMonitor.Instance, object>) null).ToggleTag(GameTags.Dead).Enter((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      smi.ApplyDeath();
      Game.Instance.Trigger(282337316, (object) smi.gameObject);
    }));
    this.dead.ground.Enter((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      Death death = this.death.Get(smi) ?? Db.Get().Deaths.Generic;
      if (!smi.IsDuplicant)
        return;
      smi.GetComponent<KAnimControllerBase>().Play((HashedString) death.loopAnim, KAnim.PlayMode.Loop, 1f, 0.0f);
    })).EventTransition(GameHashes.OnStore, this.dead.carried, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi =>
    {
      if (smi.IsDuplicant)
        return smi.HasTag(GameTags.Stored);
      return false;
    }));
    this.dead.carried.ToggleAnims("anim_dead_carried_kanim", 0.0f).PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi => !smi.HasTag(GameTags.Stored)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
  {
    public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;
    public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
  }

  public class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
  {
    private bool isDuplicant;

    public Instance(IStateMachineTarget master, DeathMonitor.Def def)
      : base(master, def)
    {
      this.isDuplicant = (bool) ((UnityEngine.Object) this.GetComponent<MinionIdentity>());
    }

    public bool IsDuplicant
    {
      get
      {
        return this.isDuplicant;
      }
    }

    public void Kill(Death death)
    {
      this.sm.death.Set(death, this.smi);
    }

    public void PickedUp(object data = null)
    {
      if (!(data is Storage) && (data == null || !(bool) data))
        return;
      this.smi.GoTo((StateMachine.BaseState) this.sm.dead.carried);
    }

    public bool IsDead()
    {
      return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.dead);
    }

    public void ApplyDeath()
    {
      if (this.isDuplicant)
      {
        this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, (object) this.smi.sm.death.Get(this.smi));
        ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, 600f - GameClock.Instance.GetTimeSinceStartOfReport(), string.Format((string) UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, (object) DUPLICANTS.CHORES.IS_DEAD_TASK), this.smi.master.gameObject.GetProperName());
        Pickupable component = this.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.RegisterListeners();
      }
      this.GetComponent<KPrefabID>().AddTag(GameTags.Corpse, false);
    }
  }
}
