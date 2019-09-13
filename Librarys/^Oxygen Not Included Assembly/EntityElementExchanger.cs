// Decompiled with JetBrains decompiler
// Type: EntityElementExchanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EntityElementExchanger : StateMachineComponent<EntityElementExchanger.StatesInstance>
{
  public Vector3 outputOffset = Vector3.zero;
  public bool reportExchange;
  [MyCmpReq]
  private KSelectable selectable;
  public SimHashes consumedElement;
  public SimHashes emittedElement;
  public float consumeRate;
  public float exchangeRatio;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void SetConsumptionRate(float consumptionRate)
  {
    this.consumeRate = consumptionRate;
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
  {
    EntityElementExchanger elementExchanger = (EntityElementExchanger) data;
    if (!((UnityEngine.Object) elementExchanger != (UnityEngine.Object) null))
      return;
    elementExchanger.OnSimConsume(mass_cb_info);
  }

  private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
  {
    float mass = mass_cb_info.mass * this.smi.master.exchangeRatio;
    if (this.reportExchange && this.smi.master.emittedElement == SimHashes.Oxygen)
    {
      string note = this.gameObject.GetProperName();
      ReceptacleMonitor component = this.GetComponent<ReceptacleMonitor>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.GetReceptacle() != (UnityEngine.Object) null)
        note = note + " (" + component.GetReceptacle().gameObject.GetProperName() + ")";
      ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, mass, note, (string) null);
    }
    SimMessages.EmitMass(Grid.PosToCell(this.smi.master.transform.GetPosition() + this.outputOffset), ElementLoader.FindElementByHash(this.smi.master.emittedElement).idx, mass, ElementLoader.FindElementByHash(this.smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
  }

  public class StatesInstance : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.GameInstance
  {
    public StatesInstance(EntityElementExchanger master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger>
  {
    public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State exchanging;
    public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State paused;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.exchanging;
      this.serializable = true;
      this.exchanging.Enter((StateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State.Callback) (smi =>
      {
        WiltCondition component = smi.master.gameObject.GetComponent<WiltCondition>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsWilting())
          return;
        smi.GoTo((StateMachine.BaseState) smi.sm.paused);
      })).EventTransition(GameHashes.Wilt, this.paused, (StateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.Transition.ConditionCallback) null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementConsume, (object) null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementOutput, (object) null).Update(nameof (EntityElementExchanger), (System.Action<EntityElementExchanger.StatesInstance, float>) ((smi, dt) =>
      {
        Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> consumedCallbackManager = Game.Instance.massConsumedCallbackManager;
        // ISSUE: reference to a compiler-generated field
        if (EntityElementExchanger.States.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EntityElementExchanger.States.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Sim.MassConsumedCallback, object>(EntityElementExchanger.OnSimConsumeCallback);
        }
        // ISSUE: reference to a compiler-generated field
        System.Action<Sim.MassConsumedCallback, object> fMgCache0 = EntityElementExchanger.States.\u003C\u003Ef__mg\u0024cache0;
        EntityElementExchanger master = smi.master;
        HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = consumedCallbackManager.Add(fMgCache0, (object) master, nameof (EntityElementExchanger));
        SimMessages.ConsumeMass(Grid.PosToCell(smi.master.gameObject), smi.master.consumedElement, smi.master.consumeRate * dt, (byte) 3, handle.index);
      }), UpdateRate.SIM_1000ms, false);
      this.paused.EventTransition(GameHashes.WiltRecover, this.exchanging, (StateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.Transition.ConditionCallback) null);
    }
  }
}
