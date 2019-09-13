// Decompiled with JetBrains decompiler
// Type: OperationalControlledSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
  private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>((System.Action<OperationalControlledSwitch, object>) ((component, data) => component.OnOperationalChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.manuallyControlled = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
  }

  private void OnOperationalChanged(object data)
  {
    this.SetState((bool) data);
  }
}
