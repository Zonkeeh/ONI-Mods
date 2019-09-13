// Decompiled with JetBrains decompiler
// Type: DropElementStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.dropping;
    GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.EXPELLING_GAS.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, DropElementStates.Instance, string>) null, (Func<string, DropElementStates.Instance, string>) null, category);
    this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.Enter("DropElement", (StateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State.Callback) (smi => smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement())).QueueAnim("idle_loop", true, (Func<DropElementStates.Instance, string>) null).BehaviourComplete(GameTags.Creatures.WantsToDropElements, false);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
  {
    public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToDropElements);
    }
  }
}
