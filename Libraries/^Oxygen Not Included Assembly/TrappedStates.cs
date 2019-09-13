// Decompiled with JetBrains decompiler
// Type: TrappedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class TrappedStates : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
  private GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State trapped;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.trapped;
    GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.TRAPPED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.TRAPPED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, TrappedStates.Instance, string>) null, (Func<string, TrappedStates.Instance, string>) null, category);
    this.trapped.Enter((StateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State.Callback) (smi =>
    {
      Navigator component = smi.GetComponent<Navigator>();
      if (!component.IsValidNavType(NavType.Floor))
        return;
      component.SetCurrentNavType(NavType.Floor);
    })).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim("trapped", KAnim.PlayMode.Loop).TagTransition(GameTags.Trapped, (GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State) null, true);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsTrapped = new Chore.Precondition()
    {
      id = nameof (IsTrapped),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Trapped))
    };

    public Instance(Chore<TrappedStates.Instance> chore, TrappedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(TrappedStates.Instance.IsTrapped, (object) null);
    }
  }
}
