// Decompiled with JetBrains decompiler
// Type: BaggedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class BaggedStates : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
  public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;
  public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.bagged;
    this.serializable = true;
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.BAGGED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.BAGGED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, BaggedStates.Instance, string>) null, (Func<string, BaggedStates.Instance, string>) null, category);
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged = this.bagged;
    // ISSUE: reference to a compiler-generated field
    if (BaggedStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaggedStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback fMgCache0 = BaggedStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State state1 = bagged.Enter(fMgCache0).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim("trussed", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, (GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State) null, true);
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape1 = this.escape;
    // ISSUE: reference to a compiler-generated field
    if (BaggedStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaggedStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback fMgCache1 = BaggedStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State state2 = state1.Transition(escape1, fMgCache1, UpdateRate.SIM_4000ms);
    // ISSUE: reference to a compiler-generated field
    if (BaggedStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaggedStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback fMgCache2 = BaggedStates.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State state3 = state2.EventHandler(GameHashes.OnStore, fMgCache2);
    // ISSUE: reference to a compiler-generated field
    if (BaggedStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaggedStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback fMgCache3 = BaggedStates.\u003C\u003Ef__mg\u0024cache3;
    state3.Exit(fMgCache3);
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape2 = this.escape;
    // ISSUE: reference to a compiler-generated field
    if (BaggedStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaggedStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback fMgCache4 = BaggedStates.\u003C\u003Ef__mg\u0024cache4;
    escape2.Enter(fMgCache4).PlayAnim("escape").OnAnimQueueComplete((GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State) null);
  }

  private static void BagStart(BaggedStates.Instance smi)
  {
    if ((double) smi.baggedTime == 0.0)
      smi.baggedTime = GameClock.Instance.GetTime();
    smi.UpdateFaller(true);
  }

  private static void BagEnd(BaggedStates.Instance smi)
  {
    smi.baggedTime = 0.0f;
    smi.UpdateFaller(false);
  }

  private static void Unbag(BaggedStates.Instance smi)
  {
    Baggable component = smi.gameObject.GetComponent<Baggable>();
    if (!(bool) ((UnityEngine.Object) component))
      return;
    component.Free();
  }

  private static void OnStore(BaggedStates.Instance smi)
  {
    smi.UpdateFaller(true);
  }

  private static bool ShouldEscape(BaggedStates.Instance smi)
  {
    return !smi.gameObject.HasTag(GameTags.Stored) && (double) (GameClock.Instance.GetTime() - smi.baggedTime) >= (double) smi.def.escapeTime;
  }

  public class Def : StateMachine.BaseDef
  {
    public float escapeTime = 300f;
  }

  public class Instance : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsBagged = new Chore.Precondition()
    {
      id = nameof (IsBagged),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged))
    };
    [Serialize]
    public float baggedTime;

    public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(BaggedStates.Instance.IsBagged, (object) null);
    }

    public void UpdateFaller(bool bagged)
    {
      bool flag1 = bagged && !this.gameObject.HasTag(GameTags.Stored);
      bool flag2 = GameComps.Fallers.Has((object) this.gameObject);
      if (flag1 == flag2)
        return;
      if (flag1)
        GameComps.Fallers.Add(this.gameObject, Vector2.zero);
      else
        GameComps.Fallers.Remove(this.gameObject);
    }
  }
}
