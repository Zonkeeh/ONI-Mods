// Decompiled with JetBrains decompiler
// Type: StateEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class StateEvent
{
  protected string name;
  private string debugName;

  public StateEvent(string name)
  {
    this.name = name;
    this.debugName = "(Event)" + name;
  }

  public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
  {
    return new StateEvent.Context(this);
  }

  public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
  {
  }

  public string GetName()
  {
    return this.name;
  }

  public string GetDebugName()
  {
    return this.debugName;
  }

  public struct Context
  {
    public StateEvent stateEvent;
    public int data;

    public Context(StateEvent state_event)
    {
      this.stateEvent = state_event;
      this.data = 0;
    }
  }
}
