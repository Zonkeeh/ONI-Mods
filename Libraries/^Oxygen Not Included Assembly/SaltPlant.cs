// Decompiled with JetBrains decompiler
// Type: SaltPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>((System.Action<SaltPlant, object>) ((component, data) => component.OnWilt(data)));
  private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>((System.Action<SaltPlant, object>) ((component, data) => component.OnWiltRecover(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
    this.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
  }

  private void OnWilt(object data = null)
  {
    this.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
  }

  private void OnWiltRecover(object data = null)
  {
    this.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
  }

  public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
  {
    public StatesInstance(SaltPlant master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
  {
    public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = true;
      default_state = (StateMachine.BaseState) this.alive;
      this.alive.DoNothing();
    }
  }
}
