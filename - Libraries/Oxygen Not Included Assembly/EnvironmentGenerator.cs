// Decompiled with JetBrains decompiler
// Type: EnvironmentGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class EnvironmentGenerator : Generator
{
  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    if (!this.operational.IsOperational)
      return;
    this.ApplyDeltaJoules(this.WattageRating * dt, false);
    this.operational.SetActive(this.operational.IsOperational, false);
  }
}
