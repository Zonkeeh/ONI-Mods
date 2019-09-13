// Decompiled with JetBrains decompiler
// Type: ThreatMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
  private FactionAlignment alignment;
  private Navigator navigator;
  public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State safe;
  public ThreatMonitor.ThreatenedStates threatened;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.safe;
    this.root.EventHandler(GameHashes.SafeFromThreats, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.OnSafe(d))).EventHandler(GameHashes.Attacked, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.OnAttacked(d))).EventHandler(GameHashes.ObjectDestroyed, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.Cleanup(d)));
    this.safe.Enter((StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback) (smi =>
    {
      smi.revengeThreat.Clear();
      smi.RefreshThreat((object) null);
    })).Update("safe", (System.Action<ThreatMonitor.Instance, float>) ((smi, dt) => smi.RefreshThreat((object) null)), UpdateRate.SIM_1000ms, true);
    this.threatened.duplicant.Transition(this.safe, (StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback) (smi => !smi.CheckForThreats()), UpdateRate.SIM_200ms);
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State state1 = this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe);
    // ISSUE: reference to a compiler-generated field
    if (ThreatMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ThreatMonitor.\u003C\u003Ef__mg\u0024cache0 = new System.Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<ThreatMonitor.Instance, float> fMgCache0 = ThreatMonitor.\u003C\u003Ef__mg\u0024cache0;
    state1.Update("DupeUpdateTarget", fMgCache0, UpdateRate.SIM_200ms, false);
    this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State state2 = this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback) (smi => !smi.WillFight()), (System.Action<ThreatMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.safe))).ToggleBehaviour(GameTags.Creatures.Attack, (StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback) (smi => smi.WillFight()), (System.Action<ThreatMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.safe)));
    // ISSUE: reference to a compiler-generated field
    if (ThreatMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ThreatMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterUpdateThreats);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<ThreatMonitor.Instance, float> fMgCache1 = ThreatMonitor.\u003C\u003Ef__mg\u0024cache1;
    state2.Update("Threatened", fMgCache1, UpdateRate.SIM_200ms, false);
  }

  private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
  {
    if (!((UnityEngine.Object) smi.MainThreat == (UnityEngine.Object) null) && smi.MainThreat.GetComponent<FactionAlignment>().targeted)
      return;
    smi.Trigger(2144432245, (object) null);
  }

  private static void CritterUpdateThreats(ThreatMonitor.Instance smi, float dt)
  {
    if (smi.isMasterNull)
      return;
    if ((UnityEngine.Object) smi.revengeThreat.target != (UnityEngine.Object) null && smi.revengeThreat.Calm(dt, smi.alignment))
    {
      smi.Trigger(-21431934, (object) null);
    }
    else
    {
      if (smi.CheckForThreats())
        return;
      smi.GoTo((StateMachine.BaseState) smi.sm.safe);
    }
  }

  private Chore CreateAttackChore(ThreatMonitor.Instance smi)
  {
    return (Chore) new AttackChore(smi.master, smi.MainThreat);
  }

  private Chore CreateFleeChore(ThreatMonitor.Instance smi)
  {
    return (Chore) new FleeChore(smi.master, smi.MainThreat);
  }

  public class Def : StateMachine.BaseDef
  {
    public Health.HealthState fleethresholdState = Health.HealthState.Injured;
  }

  public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
  {
    public ThreatMonitor.ThreatenedDuplicantStates duplicant;
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
  }

  public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
  {
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
  }

  public struct Grudge
  {
    public FactionAlignment target;
    public float grudgeTime;

    public void Reset(FactionAlignment revengeTarget)
    {
      this.target = revengeTarget;
      this.grudgeTime = 10f;
    }

    public bool Calm(float dt, FactionAlignment self)
    {
      if ((double) this.grudgeTime <= 0.0)
        return true;
      this.grudgeTime = Mathf.Max(0.0f, this.grudgeTime - dt);
      if ((double) this.grudgeTime != 0.0)
        return false;
      if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
      this.Clear();
      return true;
    }

    public void Clear()
    {
      this.grudgeTime = 0.0f;
      this.target = (FactionAlignment) null;
    }
  }

  public class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
  {
    private List<FactionAlignment> threats = new List<FactionAlignment>();
    public FactionAlignment alignment;
    private Navigator navigator;
    public ChoreDriver choreDriver;
    private Health health;
    private ChoreConsumer choreConsumer;
    public ThreatMonitor.Grudge revengeThreat;
    private GameObject mainThreat;
    private System.Action<object> refreshThreatDelegate;

    public Instance(IStateMachineTarget master, ThreatMonitor.Def def)
      : base(master, def)
    {
      this.alignment = master.GetComponent<FactionAlignment>();
      this.navigator = master.GetComponent<Navigator>();
      this.choreDriver = master.GetComponent<ChoreDriver>();
      this.health = master.GetComponent<Health>();
      this.choreConsumer = master.GetComponent<ChoreConsumer>();
      this.refreshThreatDelegate = new System.Action<object>(this.RefreshThreat);
    }

    public GameObject MainThreat
    {
      get
      {
        return this.mainThreat;
      }
    }

    public bool IAmADuplicant
    {
      get
      {
        return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
      }
    }

    public void ClearMainThreat()
    {
      this.SetMainThreat((GameObject) null);
    }

    public void SetMainThreat(GameObject threat)
    {
      if ((UnityEngine.Object) threat == (UnityEngine.Object) this.mainThreat)
        return;
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
        if ((UnityEngine.Object) threat == (UnityEngine.Object) null)
          this.Trigger(2144432245, (object) null);
      }
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
      }
      this.mainThreat = threat;
      if (!((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null))
        return;
      this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
    }

    public void OnSafe(object data)
    {
      if (!((UnityEngine.Object) this.revengeThreat.target != (UnityEngine.Object) null))
        return;
      if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
        this.revengeThreat.Clear();
      this.ClearMainThreat();
    }

    public void OnAttacked(object data)
    {
      FactionAlignment revengeTarget = (FactionAlignment) data;
      this.revengeThreat.Reset(revengeTarget);
      if ((UnityEngine.Object) this.mainThreat == (UnityEngine.Object) null)
      {
        this.SetMainThreat(revengeTarget.gameObject);
        this.GoToThreatened();
      }
      else
      {
        if (this.WillFight())
          return;
        this.GoToThreatened();
      }
    }

    public bool WillFight()
    {
      if ((UnityEngine.Object) this.choreConsumer != (UnityEngine.Object) null && (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat) || !this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat)))
        return false;
      return this.health.State < this.smi.def.fleethresholdState;
    }

    private void GotoThreatResponse()
    {
      bool flag = this.smi.master.GetComponent<Navigator>().IsMoving();
      Chore currentChore = this.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
      if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().targeted)
      {
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.threatened.duplicant.ShouldFight);
      }
      else
      {
        if (flag || currentChore != null && currentChore.target != null && (UnityEngine.Object) currentChore.target.GetComponent<Pickupable>() != (UnityEngine.Object) null)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.threatened.duplicant.ShoudFlee);
      }
    }

    public void GoToThreatened()
    {
      if (this.IAmADuplicant)
        this.GotoThreatResponse();
      else
        this.smi.GoTo((StateMachine.BaseState) this.sm.threatened.creature);
    }

    public void Cleanup(object data)
    {
      if (!(bool) ((UnityEngine.Object) this.mainThreat))
        return;
      this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
    }

    public void RefreshThreat(object data)
    {
      if (!this.IsRunning())
        return;
      if (this.smi.CheckForThreats())
      {
        this.GoToThreatened();
      }
      else
      {
        if (this.smi.GetCurrentState() == this.sm.safe)
          return;
        this.Trigger(-21431934, (object) null);
        this.smi.GoTo((StateMachine.BaseState) this.sm.safe);
      }
    }

    public bool CheckForThreats()
    {
      GameObject threat = !((UnityEngine.Object) this.revengeThreat.target != (UnityEngine.Object) null) || !this.revengeThreat.target.IsAlignmentActive() || this.revengeThreat.target.health.IsDefeated() || this.IAmADuplicant && this.revengeThreat.target.targeted ? this.FindThreat() : this.revengeThreat.target.gameObject;
      this.SetMainThreat(threat);
      return (UnityEngine.Object) threat != (UnityEngine.Object) null;
    }

    public GameObject FindThreat()
    {
      this.threats.Clear();
      if (this.isMasterNull)
        return (GameObject) null;
      bool flag = this.WillFight();
      if (this.IAmADuplicant && flag)
      {
        for (int index = 0; index < 6; ++index)
        {
          if (index != 0)
          {
            foreach (FactionAlignment member in FactionManager.Instance.GetFaction((FactionManager.FactionID) index).Members)
            {
              if (member.targeted && !member.health.IsDefeated() && (!this.threats.Contains(member) && this.navigator.CanReach((IApproachable) member.attackable)))
                this.threats.Add(member);
            }
          }
        }
      }
      if (this.threats.Count == 0)
        return (GameObject) null;
      return this.PickBestTarget(this.threats);
    }

    public GameObject PickBestTarget(List<FactionAlignment> threats)
    {
      float num1 = 1f;
      Vector2 position = (Vector2) this.gameObject.transform.GetPosition();
      GameObject gameObject = (GameObject) null;
      float num2 = float.PositiveInfinity;
      for (int index = threats.Count - 1; index >= 0; --index)
      {
        FactionAlignment threat = threats[index];
        float num3 = Vector2.Distance(position, (Vector2) threat.transform.GetPosition()) / num1;
        if ((double) num3 < (double) num2)
        {
          num2 = num3;
          gameObject = threat.gameObject;
        }
      }
      return gameObject;
    }
  }
}
