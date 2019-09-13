// Decompiled with JetBrains decompiler
// Type: Automatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Automatable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>((System.Action<Automatable, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private bool automationOnly = true;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    Automatable component = ((GameObject) data).GetComponent<Automatable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.automationOnly = component.automationOnly;
  }

  public bool GetAutomationOnly()
  {
    return this.automationOnly;
  }

  public void SetAutomationOnly(bool only)
  {
    this.automationOnly = only;
  }

  public bool AllowedByAutomation(bool is_transfer_arm)
  {
    return !this.GetAutomationOnly() || is_transfer_arm;
  }
}
