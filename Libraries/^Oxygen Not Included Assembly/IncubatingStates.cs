// Decompiled with JetBrains decompiler
// Type: IncubatingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class IncubatingStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
  public IncubatingStates.IncubatorStates incubator;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.incubator;
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, IncubatingStates.Instance, string>) null, (Func<string, IncubatingStates.Instance, string>) null, category);
    this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, (GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State) null, true);
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle1 = this.incubator.idle;
    // ISSUE: reference to a compiler-generated field
    if (IncubatingStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      IncubatingStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback fMgCache0 = IncubatingStates.\u003C\u003Ef__mg\u0024cache0;
    idle1.Enter("VariantUpdate", fMgCache0).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose = this.incubator.choose;
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant = this.incubator.variant;
    // ISSUE: reference to a compiler-generated field
    if (IncubatingStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      IncubatingStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback fMgCache1 = IncubatingStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State state = choose.Transition(variant, fMgCache1, UpdateRate.SIM_200ms);
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle2 = this.incubator.idle;
    // ISSUE: reference to a compiler-generated field
    if (IncubatingStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      IncubatingStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback condition = GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(IncubatingStates.\u003C\u003Ef__mg\u0024cache2);
    state.Transition(idle2, condition, UpdateRate.SIM_200ms);
    this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
  }

  public static bool DoVariant(IncubatingStates.Instance smi)
  {
    return smi.variant_time == 0;
  }

  public static void VariantUpdate(IncubatingStates.Instance smi)
  {
    if (smi.variant_time <= 0)
      smi.variant_time = UnityEngine.Random.Range(3, 7);
    else
      --smi.variant_time;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition()
    {
      id = nameof (IsInIncubator),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator))
    };
    public int variant_time = 3;

    public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, (object) null);
    }
  }

  public class IncubatorStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
  {
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
  }
}
