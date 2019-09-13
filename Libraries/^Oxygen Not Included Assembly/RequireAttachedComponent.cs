// Decompiled with JetBrains decompiler
// Type: RequireAttachedComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : RocketLaunchCondition
{
  private string typeNameString;
  private System.Type requiredType;
  private AttachableBuilding myAttachable;

  public RequireAttachedComponent(
    AttachableBuilding myAttachable,
    System.Type required_type,
    string type_name_string)
  {
    this.myAttachable = myAttachable;
    this.requiredType = required_type;
    this.typeNameString = type_name_string;
  }

  public System.Type RequiredType
  {
    get
    {
      return this.requiredType;
    }
    set
    {
      this.requiredType = value;
      this.typeNameString = this.requiredType.Name;
    }
  }

  public override RocketLaunchCondition.LaunchStatus EvaluateLaunchCondition()
  {
    if ((UnityEngine.Object) this.myAttachable != (UnityEngine.Object) null)
    {
      foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.myAttachable))
      {
        if ((bool) ((UnityEngine.Object) gameObject.GetComponent(this.requiredType)))
          return RocketLaunchCondition.LaunchStatus.Ready;
      }
    }
    return RocketLaunchCondition.LaunchStatus.Failure;
  }

  public override string GetLaunchStatusMessage(bool ready)
  {
    if (ready)
      return this.typeNameString + " " + (string) UI.STARMAP.LAUNCHCHECKLIST.REQUIRED;
    return this.typeNameString + " " + (string) UI.STARMAP.LAUNCHCHECKLIST.INSTALLED;
  }

  public override string GetLaunchStatusTooltip(bool ready)
  {
    if (ready)
      return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, (object) this.typeNameString);
    return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.REQUIRED_TOOLTIP, (object) this.typeNameString);
  }
}
