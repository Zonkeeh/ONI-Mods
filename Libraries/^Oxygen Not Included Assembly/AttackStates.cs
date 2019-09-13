// Decompiled with JetBrains decompiler
// Type: AttackStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
  public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State attack;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter("SetTarget", (StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State.Callback) (smi => this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi)));
    GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state1 = this.approach.InitializeStates(this.masterTarget, this.target, this.attack, (GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State) null, new CellOffset[5]
    {
      new CellOffset(0, 0),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1)
    }, (NavTactic) null);
    string name1 = (string) CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty1 = string.Empty;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay1, 129022, (Func<string, AttackStates.Instance, string>) null, (Func<string, AttackStates.Instance, string>) null, category1);
    GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state2 = this.attack.Enter((StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State.Callback) (smi =>
    {
      smi.Play("eat_pre", KAnim.PlayMode.Once);
      smi.Queue("eat_pst", KAnim.PlayMode.Once);
      smi.Schedule(0.5f, (System.Action<object>) (_param1 => smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi))), (object) null);
    }));
    string name3 = (string) CREATURES.STATUSITEMS.ATTACK.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.ATTACK.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    string name4 = name3;
    string tooltip4 = tooltip3;
    string empty2 = string.Empty;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay2, 129022, (Func<string, AttackStates.Instance, string>) null, (Func<string, AttackStates.Instance, string>) null, category2).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack, false);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
  {
    public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Attack);
    }
  }
}
