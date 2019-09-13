// Decompiled with JetBrains decompiler
// Type: InfiniteGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class InfiniteGenerator : Generator
{
  public override bool IsEmpty
  {
    get
    {
      return false;
    }
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    this.ApplyDeltaJoules(this.WattageRating * dt, false);
  }
}
