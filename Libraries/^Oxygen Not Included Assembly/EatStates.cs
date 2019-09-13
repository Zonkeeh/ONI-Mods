// Decompiled with JetBrains decompiler
// Type: EatStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;
  public EatStates.EatingState eating;
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;
  public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (EatStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EatStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback fMgCache0 = EatStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state1 = root.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (EatStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EatStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.ReserveEdible);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback fMgCache1 = EatStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state2 = state1.Enter(fMgCache1);
    // ISSUE: reference to a compiler-generated field
    if (EatStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EatStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback fMgCache2 = EatStates.\u003C\u003Ef__mg\u0024cache2;
    state2.Exit(fMgCache2);
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat = this.goingtoeat;
    // ISSUE: reference to a compiler-generated field
    if (EatStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EatStates.\u003C\u003Ef__mg\u0024cache3 = new Func<EatStates.Instance, int>(EatStates.GetEdibleCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<EatStates.Instance, int> fMgCache3 = EatStates.\u003C\u003Ef__mg\u0024cache3;
    EatStates.EatingState eating = this.eating;
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state3 = goingtoeat.MoveTo(fMgCache3, (GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) eating, (GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) null, false);
    string name1 = (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty1 = string.Empty;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state3.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, EatStates.Instance, string>) null, (Func<string, EatStates.Instance, string>) null, category1);
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state4 = this.eating.DefaultState(this.eating.pre);
    string name3 = (string) CREATURES.STATUSITEMS.EATING.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.EATING.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    string name4 = name3;
    string tooltip4 = tooltip3;
    string empty2 = string.Empty;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state4.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, EatStates.Instance, string>) null, (Func<string, EatStates.Instance, string>) null, category2);
    this.eating.pre.QueueAnim("eat_pre", false, (Func<EatStates.Instance, string>) null).OnAnimQueueComplete(this.eating.loop);
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop = this.eating.loop;
    // ISSUE: reference to a compiler-generated field
    if (EatStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EatStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback fMgCache4 = EatStates.\u003C\u003Ef__mg\u0024cache4;
    loop.Enter(fMgCache4).QueueAnim("eat_loop", true, (Func<EatStates.Instance, string>) null).ScheduleGoTo(3f, (StateMachine.BaseState) this.eating.pst);
    this.eating.pst.QueueAnim("eat_pst", false, (Func<EatStates.Instance, string>) null).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat, false);
  }

  private static void SetTarget(EatStates.Instance smi)
  {
    smi.sm.target.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdible, smi);
  }

  private static void ReserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void UnreserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (go.HasTag(GameTags.Creatures.ReservedByCreature))
      go.RemoveTag(GameTags.Creatures.ReservedByCreature);
    else
      Debug.LogWarningFormat((UnityEngine.Object) smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", (object) smi.gameObject, (object) go);
  }

  private static void EatComplete(EatStates.Instance smi)
  {
    PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      smi.lastMealElement = primaryElement.Element;
    smi.Trigger(1386391852, (object) smi.sm.target.Get<KPrefabID>(smi));
  }

  private static int GetEdibleCell(EatStates.Instance smi)
  {
    return Grid.PosToCell(smi.sm.target.Get(smi));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
  {
    public Element lastMealElement;

    public Instance(Chore<EatStates.Instance> chore, EatStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);
    }

    public Element GetLatestMealElement()
    {
      return this.lastMealElement;
    }
  }

  public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
  {
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
  }
}
