// Decompiled with JetBrains decompiler
// Type: ConditionHasAtmoSuit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ConditionHasAtmoSuit : RocketLaunchCondition
{
  private CommandModule module;

  public ConditionHasAtmoSuit(CommandModule module)
  {
    this.module = module;
    ManualDeliveryKG orAdd = this.module.FindOrAdd<ManualDeliveryKG>();
    orAdd.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    orAdd.SetStorage(module.storage);
    orAdd.requestedItemTag = GameTags.AtmoSuit;
    orAdd.minimumMass = 1f;
    orAdd.refillMass = 0.1f;
    orAdd.capacity = 1f;
  }

  public override RocketLaunchCondition GetParentCondition()
  {
    return (RocketLaunchCondition) null;
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    return (double) this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) >= 1.0 ? RocketLaunchCondition.LaunchStatus.Ready : RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.HASSUIT.NAME;
    return (string) UI.STARMAP.NOSUIT.NAME;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.HASSUIT.TOOLTIP;
    return (string) UI.STARMAP.NOSUIT.TOOLTIP;
  }
}
