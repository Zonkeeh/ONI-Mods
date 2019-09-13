// Decompiled with JetBrains decompiler
// Type: CargoBayIsEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class CargoBayIsEmpty : RocketLaunchCondition
{
  private CommandModule commandModule;

  public CargoBayIsEmpty(CommandModule module)
  {
    this.commandModule = module;
  }

  public override RocketLaunchCondition GetParentCondition()
  {
    return (RocketLaunchCondition) null;
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((Object) component != (Object) null && (double) component.storage.MassStored() != 0.0)
        return RocketLaunchCondition.LaunchStatus.Failure;
    }
    return RocketLaunchCondition.LaunchStatus.Ready;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.CARGOEMPTY.NAME;
    return (string) UI.STARMAP.CARGOEMPTY.NAME;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready)
      return (string) UI.STARMAP.CARGOEMPTY.TOOLTIP;
    return (string) UI.STARMAP.CARGOEMPTY.TOOLTIP;
  }
}
