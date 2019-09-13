// Decompiled with JetBrains decompiler
// Type: ConditionHasAstronaut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

public class ConditionHasAstronaut : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionHasAstronaut(CommandModule module)
  {
    this.module = module;
  }

  public override RocketLaunchCondition GetParentCondition()
  {
    return (RocketLaunchCondition) null;
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
    return storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null ? RocketLaunchCondition.LaunchStatus.Ready : RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
    return (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
    return (string) UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
  }
}
