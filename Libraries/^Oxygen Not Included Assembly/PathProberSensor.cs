// Decompiled with JetBrains decompiler
// Type: PathProberSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PathProberSensor : Sensor
{
  private Navigator navigator;

  public PathProberSensor(Sensors sensors)
    : base(sensors)
  {
    this.navigator = sensors.GetComponent<Navigator>();
  }

  public override void Update()
  {
    this.navigator.UpdateProbe(false);
  }
}
