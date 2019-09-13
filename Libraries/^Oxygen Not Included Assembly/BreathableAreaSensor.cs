// Decompiled with JetBrains decompiler
// Type: BreathableAreaSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BreathableAreaSensor : Sensor
{
  private bool isBreathable;
  private OxygenBreather breather;

  public BreathableAreaSensor(Sensors sensors)
    : base(sensors)
  {
  }

  public override void Update()
  {
    if ((Object) this.breather == (Object) null)
      this.breather = this.GetComponent<OxygenBreather>();
    bool isBreathable = this.isBreathable;
    this.isBreathable = this.breather.IsBreathableElement;
    if (this.isBreathable == isBreathable)
      return;
    if (this.isBreathable)
      this.Trigger(99949694, (object) null);
    else
      this.Trigger(-1189351068, (object) null);
  }

  public bool IsBreathable()
  {
    return this.isBreathable;
  }

  public bool IsUnderwater()
  {
    return this.breather.IsUnderLiquid;
  }
}
