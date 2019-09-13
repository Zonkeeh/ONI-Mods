// Decompiled with JetBrains decompiler
// Type: PeeChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PeeChore : Chore<PeeChore.StatesInstance>
{
  public PeeChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Pee, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new PeeChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.GameInstance
  {
    public Notification stressfullyEmptyingBladder = new Notification((string) DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
    public AmountInstance bladder;
    private AmountInstance bodyTemperature;

    public StatesInstance(PeeChore master, GameObject worker)
      : base(master)
    {
      this.bladder = Db.Get().Amounts.Bladder.Lookup(worker);
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(worker);
      this.sm.worker.Set(worker, this.smi);
    }

    public bool IsDonePeeing()
    {
      return (double) this.bladder.value <= 0.0;
    }

    public void SpawnDirtyWater(float dt)
    {
      int cell = Grid.PosToCell(this.sm.worker.Get<KMonoBehaviour>(this.smi));
      byte index = Db.Get().Diseases.GetIndex((HashedString) "FoodPoisoning");
      float num = dt * -this.bladder.GetDelta() / this.bladder.GetMax();
      if ((double) num <= 0.0)
        return;
      float mass = 2f * num;
      Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
      if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        equippable.GetComponent<Storage>().AddLiquid(SimHashes.DirtyWater, mass, this.bodyTemperature.value, index, Mathf.CeilToInt(100000f * num), false, true);
      else
        SimMessages.AddRemoveSubstance(cell, SimHashes.DirtyWater, CellEventLogger.Instance.Vomit, mass, this.bodyTemperature.value, index, Mathf.CeilToInt(100000f * num), true, -1);
    }
  }

  public class States : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore>
  {
    public StateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.TargetParameter worker;
    public GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.State running;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.running;
      this.Target(this.worker);
      this.running.ToggleAnims("anim_expel_kanim", 0.0f).ToggleEffect("StressfulyEmptyingBladder").DoNotification((Func<PeeChore.StatesInstance, Notification>) (smi => smi.stressfullyEmptyingBladder)).DoReport(ReportManager.ReportType.ToiletIncident, (Func<PeeChore.StatesInstance, float>) (smi => 1f), (Func<PeeChore.StatesInstance, string>) (smi => this.masterTarget.Get(smi).GetProperName())).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Transition((GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.State) null, (StateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.Transition.ConditionCallback) (smi => smi.IsDonePeeing()), UpdateRate.SIM_200ms).Update("SpawnDirtyWater", (System.Action<PeeChore.StatesInstance, float>) ((smi, dt) => smi.SpawnDirtyWater(dt)), UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
    }
  }
}
