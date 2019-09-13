// Decompiled with JetBrains decompiler
// Type: StateMachineExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class StateMachineExtensions
{
  public static bool IsNullOrStopped(this StateMachine.Instance smi)
  {
    if (smi != null)
      return !smi.IsRunning();
    return true;
  }
}
