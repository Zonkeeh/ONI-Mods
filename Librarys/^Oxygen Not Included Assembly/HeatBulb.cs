// Decompiled with JetBrains decompiler
// Type: HeatBulb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class HeatBulb : KMonoBehaviour, ISim200ms
{
  [SerializeField]
  private float minTemperature;
  [SerializeField]
  private float kjConsumptionRate;
  [SerializeField]
  private float lightKJConsumptionRate;
  [SerializeField]
  private Vector2I minCheckOffset;
  [SerializeField]
  private Vector2I maxCheckOffset;
  [MyCmpGet]
  private Light2D lightSource;
  [MyCmpGet]
  private KBatchedAnimController kanim;
  [Serialize]
  private float kjConsumed;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.kanim.Play((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  public void Sim200ms(float dt)
  {
    float num1 = this.kjConsumptionRate * dt;
    Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
    int num2 = vector2I.x * vector2I.y;
    float num3 = num1 / (float) num2;
    int x1;
    int y1;
    Grid.PosToXY(this.transform.GetPosition(), out x1, out y1);
    for (int y2 = this.minCheckOffset.y; y2 <= this.maxCheckOffset.y; ++y2)
    {
      for (int x2 = this.minCheckOffset.x; x2 <= this.maxCheckOffset.x; ++x2)
      {
        int cell = Grid.XYToCell(x1 + x2, y1 + y2);
        if (Grid.IsValidCell(cell) && (double) Grid.Temperature[cell] > (double) this.minTemperature)
        {
          this.kjConsumed += num3;
          SimMessages.ModifyEnergy(cell, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
        }
      }
    }
    float num4 = this.lightKJConsumptionRate * dt;
    if ((double) this.kjConsumed > (double) num4)
    {
      if (!this.lightSource.enabled)
      {
        this.kanim.Play((HashedString) "open", KAnim.PlayMode.Once, 1f, 0.0f);
        this.kanim.Queue((HashedString) "on", KAnim.PlayMode.Once, 1f, 0.0f);
        this.lightSource.enabled = true;
      }
      this.kjConsumed -= num4;
    }
    else
    {
      if (this.lightSource.enabled)
      {
        this.kanim.Play((HashedString) "close", KAnim.PlayMode.Once, 1f, 0.0f);
        this.kanim.Queue((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
      }
      this.lightSource.enabled = false;
    }
  }
}
