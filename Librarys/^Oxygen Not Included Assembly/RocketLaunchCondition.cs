// Decompiled with JetBrains decompiler
// Type: RocketLaunchCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class RocketLaunchCondition
{
  public abstract RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition();

  public abstract string GetLaunchStatusMessage(bool ready);

  public abstract string GetLaunchStatusTooltip(bool ready);

  public virtual RocketLaunchCondition GetParentCondition()
  {
    return (RocketLaunchCondition) null;
  }

  public enum LaunchStatus
  {
    Ready,
    Warning,
    Failure,
  }
}
