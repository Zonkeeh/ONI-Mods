// Decompiled with JetBrains decompiler
// Type: PickupableSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PickupableSensor : Sensor
{
  private PathProber pathProber;
  private Worker worker;

  public PickupableSensor(Sensors sensors)
    : base(sensors)
  {
    this.worker = this.GetComponent<Worker>();
    this.pathProber = this.GetComponent<PathProber>();
  }

  public override void Update()
  {
    GlobalChoreProvider.Instance.UpdateFetches(this.pathProber);
    Game.Instance.fetchManager.UpdatePickups(this.pathProber, this.worker);
  }
}
