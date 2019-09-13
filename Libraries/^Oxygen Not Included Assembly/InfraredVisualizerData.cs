// Decompiled with JetBrains decompiler
// Type: InfraredVisualizerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public struct InfraredVisualizerData
{
  public KAnimControllerBase controller;
  public AmountInstance temperatureAmount;
  public HandleVector<int>.Handle structureTemperature;
  public PrimaryElement primaryElement;
  public TemperatureVulnerable temperatureVulnerable;

  public InfraredVisualizerData(GameObject go)
  {
    this.controller = (KAnimControllerBase) go.GetComponent<KBatchedAnimController>();
    if ((Object) this.controller != (Object) null)
    {
      this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
      this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
      this.primaryElement = go.GetComponent<PrimaryElement>();
      this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
    }
    else
    {
      this.temperatureAmount = (AmountInstance) null;
      this.structureTemperature = HandleVector<int>.InvalidHandle;
      this.primaryElement = (PrimaryElement) null;
      this.temperatureVulnerable = (TemperatureVulnerable) null;
    }
  }

  public void Update()
  {
    float temperature = 0.0f;
    if (this.temperatureAmount != null)
      temperature = this.temperatureAmount.value;
    else if (this.structureTemperature.IsValid())
      temperature = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
    else if ((Object) this.primaryElement != (Object) null)
      temperature = this.primaryElement.Temperature;
    else if ((Object) this.temperatureVulnerable != (Object) null)
      temperature = this.temperatureVulnerable.InternalTemperature;
    if ((double) temperature < 0.0)
      return;
    this.controller.OverlayColour = (Color) (Color32) SimDebugView.Instance.NormalizedTemperature(temperature);
  }
}
