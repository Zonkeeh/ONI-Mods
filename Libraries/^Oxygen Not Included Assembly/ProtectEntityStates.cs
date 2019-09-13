// Decompiled with JetBrains decompiler
// Type: ProtectEntityStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class ProtectEntityStates : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>
{
  public StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.TargetParameter target;
  public ProtectEntityStates.ProtectStates protectEntity;
  public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.protectEntity.moveToThreat;
    GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State state = this.root.Enter("SetTarget", (StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State.Callback) (smi => this.target.Set(smi.GetSMI<EntityThreatMonitor.Instance>().MainThreat, smi)));
    string name1 = (string) CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, ProtectEntityStates.Instance, string>) null, (Func<string, ProtectEntityStates.Instance, string>) null, category);
    this.protectEntity.DoNothing();
    this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, this.protectEntity.attackThreat, (GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State) null, new CellOffset[5]
    {
      new CellOffset(0, 0),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1)
    }, (NavTactic) null);
    this.protectEntity.attackThreat.Enter((StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State.Callback) (smi =>
    {
      smi.Play("slap_pre", KAnim.PlayMode.Once);
      smi.Queue("slap", KAnim.PlayMode.Once);
      smi.Queue("slap_pst", KAnim.PlayMode.Once);
      smi.Schedule(0.5f, (System.Action<object>) (_param1 => smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi))), (object) null);
    })).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend, false);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.GameInstance
  {
    public Instance(Chore<ProtectEntityStates.Instance> chore, ProtectEntityStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Defend);
    }
  }

  public class ProtectStates : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State
  {
    public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.ApproachSubState<AttackableBase> moveToThreat;
    public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State attackThreat;
  }
}
