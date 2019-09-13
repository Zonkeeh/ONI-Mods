// Decompiled with JetBrains decompiler
// Type: ConditionSufficientFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ConditionSufficientFood : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionSufficientFood(CommandModule module)
  {
    this.module = module;
  }

  public override RocketLaunchCondition GetParentCondition()
  {
    return (RocketLaunchCondition) null;
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    return (double) this.module.storage.GetAmountAvailable(GameTags.Edible) > 1.0 ? RocketLaunchCondition.LaunchStatus.Ready : RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.HASFOOD.NAME;
    return (string) UI.STARMAP.NOFOOD.NAME;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.HASFOOD.TOOLTIP;
    return (string) UI.STARMAP.NOFOOD.TOOLTIP;
  }
}
