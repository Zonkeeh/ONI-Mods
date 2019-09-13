// Decompiled with JetBrains decompiler
// Type: Usable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
  private StateMachine.Instance smi;

  public abstract void StartUsing(User user);

  protected void StartUsing(StateMachine.Instance smi, User user)
  {
    DebugUtil.Assert(this.smi == null);
    DebugUtil.Assert(smi != null);
    this.smi = smi;
    smi.OnStop += new System.Action<string, StateMachine.Status>(user.OnStateMachineStop);
    smi.StartSM();
  }

  public void StopUsing(User user)
  {
    if (this.smi == null)
      return;
    this.smi.OnStop -= new System.Action<string, StateMachine.Status>(user.OnStateMachineStop);
    this.smi.StopSM("Usable.StopUsing");
    this.smi = (StateMachine.Instance) null;
  }
}
