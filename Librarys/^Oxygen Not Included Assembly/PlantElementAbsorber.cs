// Decompiled with JetBrains decompiler
// Type: PlantElementAbsorber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct PlantElementAbsorber
{
  public Storage storage;
  public PlantElementAbsorber.LocalInfo localInfo;
  public HandleVector<int>.Handle[] accumulators;
  public PlantElementAbsorber.ConsumeInfo[] consumedElements;

  public void Clear()
  {
    this.storage = (Storage) null;
    this.consumedElements = (PlantElementAbsorber.ConsumeInfo[]) null;
  }

  public struct ConsumeInfo
  {
    public Tag tag;
    public float massConsumptionRate;

    public ConsumeInfo(Tag tag, float mass_consumption_rate)
    {
      this.tag = tag;
      this.massConsumptionRate = mass_consumption_rate;
    }
  }

  public struct LocalInfo
  {
    public Tag tag;
    public float massConsumptionRate;
  }
}
