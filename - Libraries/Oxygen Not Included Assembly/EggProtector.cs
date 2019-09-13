// Decompiled with JetBrains decompiler
// Type: EggProtector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class EggProtector : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>
{
  public StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.BoolParameter needsToMoveCloser;
  public StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.BoolParameter hasEggToGuard;
  public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State idle;
  public EggProtector.GuardingStates guarding;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.hasEggToGuard, this.guarding.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsTrue).EventHandler(GameHashes.LayEgg, (StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi => smi.FindEggToGuard())).Update((System.Action<EggProtector.Instance, float>) ((smi, dt) => smi.FindEggToGuard()), UpdateRate.SIM_4000ms, false);
    this.guarding.Enter((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) "pincher_kanim"), (string) null, "_heat", 0);
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
    })).Exit((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim((HashedString) "pincher_kanim").GetData(), 0);
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
    })).ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.hasEggToGuard, this.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsFalse).Update((System.Action<EggProtector.Instance, float>) ((smi, dt) => smi.CanProtectEgg()), UpdateRate.SIM_1000ms, false);
    this.guarding.idle.ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.needsToMoveCloser, this.guarding.return_to_egg, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsTrue);
    this.guarding.return_to_egg.MoveTo((Func<EggProtector.Instance, int>) (smi => smi.GetEggPos()), (GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State) null, (GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State) null, true).ParamTransition<bool>((StateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.Parameter<bool>) this.needsToMoveCloser, this.guarding.idle, GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.IsFalse);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag protectorTag;
    public bool shouldProtect;

    public Def(Tag tag, bool shouldProtect)
    {
      this.protectorTag = tag;
      this.shouldProtect = shouldProtect;
    }
  }

  public class Instance : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.GameInstance
  {
    public GameObject eggToGuard;

    public Instance(Chore<EggProtector.Instance> chore, EggProtector.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.gameObject.GetSMI<EntityThreatMonitor.Instance>().allyTag = def.protectorTag;
    }

    public void CheckDistanceToEgg()
    {
      int navigationCost = this.smi.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(this.eggToGuard));
      if (navigationCost > 20)
      {
        this.sm.needsToMoveCloser.Set(true, this.smi);
      }
      else
      {
        if (navigationCost >= 0)
          return;
        this.sm.needsToMoveCloser.Set(false, this.smi);
      }
    }

    public void CanProtectEgg()
    {
      bool flag = true;
      if ((UnityEngine.Object) this.eggToGuard == (UnityEngine.Object) null)
        flag = false;
      Navigator component = this.smi.GetComponent<Navigator>();
      if (flag)
      {
        int num = 150;
        int navigationCost = component.GetNavigationCost(Grid.PosToCell(this.eggToGuard));
        if (navigationCost == -1 || navigationCost >= num)
          flag = false;
      }
      if (flag)
        return;
      this.SetEggToGuard((GameObject) null);
    }

    public void FindEggToGuard()
    {
      if (!this.def.shouldProtect)
        return;
      GameObject egg = (GameObject) null;
      int num = 100;
      Navigator component = this.smi.GetComponent<Navigator>();
      IEnumerator enumerator = Components.Pickupables.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Pickupable current = (Pickupable) enumerator.Current;
          if (current.HasTag("CrabEgg".ToTag()) && (double) Vector2.Distance((Vector2) this.smi.transform.position, (Vector2) current.transform.position) <= 25.0)
          {
            int navigationCost = component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) current));
            if (navigationCost != -1 && navigationCost < num)
            {
              egg = current.gameObject;
              num = navigationCost;
            }
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      this.SetEggToGuard(egg);
    }

    public void SetEggToGuard(GameObject egg)
    {
      this.eggToGuard = egg;
      this.gameObject.GetSMI<EntityThreatMonitor.Instance>().entityToProtect = egg;
      this.sm.hasEggToGuard.Set((UnityEngine.Object) egg != (UnityEngine.Object) null, this.smi);
    }

    public int GetEggPos()
    {
      return Grid.PosToCell(this.eggToGuard);
    }
  }

  public class GuardingStates : GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State
  {
    public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State idle;
    public GameStateMachine<EggProtector, EggProtector.Instance, IStateMachineTarget, EggProtector.Def>.State return_to_egg;
  }
}
