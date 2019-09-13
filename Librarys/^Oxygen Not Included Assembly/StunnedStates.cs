// Decompiled with JetBrains decompiler
// Type: StunnedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
  public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.stunned;
    GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, StunnedStates.Instance, string>) null, (Func<string, StunnedStates.Instance, string>) null, category);
    this.stunned.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Stunned, (GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State) null, true);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsStunned = new Chore.Precondition()
    {
      id = nameof (IsStunned),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.Stunned))
    };

    public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(StunnedStates.Instance.IsStunned, (object) null);
    }
  }
}
