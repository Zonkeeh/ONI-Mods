// Decompiled with JetBrains decompiler
// Type: LiquidPumpingStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

public class LiquidPumpingStation : Workable, ISim200ms
{
  private static readonly CellOffset[] floorOffsets = new CellOffset[2]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0)
  };
  private static readonly CellOffset[] liquidOffsets = new CellOffset[10]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0),
    new CellOffset(0, -1),
    new CellOffset(1, -1),
    new CellOffset(0, -2),
    new CellOffset(1, -2),
    new CellOffset(0, -3),
    new CellOffset(1, -3),
    new CellOffset(0, -4),
    new CellOffset(1, -4)
  };
  private int depthAvailable = -1;
  private LiquidPumpingStation.LiquidInfo[] infos;
  private int infoCount;
  private LiquidPumpingStation.WorkSession session;
  private MeterController meter;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.resetProgressOnStop = true;
    this.showProgressBar = false;
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    for (int index = 0; index < LiquidPumpingStation.floorOffsets.Length; ++index)
    {
      int cell2 = Grid.OffsetCell(cell1, LiquidPumpingStation.floorOffsets[index]);
      Grid.FakeFloor[cell2] = true;
      Pathfinding.Instance.AddDirtyNavGridCell(cell2);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.infos = new LiquidPumpingStation.LiquidInfo[LiquidPumpingStation.liquidOffsets.Length * 2];
    this.GetComponent<KSelectable>().SetStatusIndicatorOffset(this.GetComponent<Building>().Def.placementPivot);
    this.RefreshStatusItem();
    this.Sim200ms(0.0f);
    this.SetWorkTime(10f);
    this.RefreshDepthAvailable();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    foreach (GameObject go in this.GetComponent<Storage>().items)
    {
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && (UnityEngine.Object) go != (UnityEngine.Object) null)
        go.DeleteObject();
    }
  }

  private void RefreshDepthAvailable()
  {
    int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell((KMonoBehaviour) this), this.gameObject);
    int num = 4;
    if (depthAvailable <= this.depthAvailable)
      return;
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    for (int index = 1; index <= num; ++index)
      component.SetSymbolVisiblity((KAnimHashedString) ("pipe" + index.ToString()), index <= depthAvailable);
    PumpingStationGuide.OccupyArea(this.gameObject, depthAvailable);
    this.depthAvailable = depthAvailable;
  }

  public void Sim200ms(float dt)
  {
    if (this.session != null)
      return;
    int infoCount = this.infoCount;
    for (int index = 0; index < this.infoCount; ++index)
      this.infos[index].amount = 0.0f;
    if (this.GetComponent<Operational>().IsOperational)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      for (int index1 = 0; index1 < LiquidPumpingStation.liquidOffsets.Length; ++index1)
      {
        if (this.depthAvailable >= Math.Abs(LiquidPumpingStation.liquidOffsets[index1].y))
        {
          int index2 = Grid.OffsetCell(cell, LiquidPumpingStation.liquidOffsets[index1]);
          bool flag = false;
          Element element = Grid.Element[index2];
          if (element.IsLiquid)
          {
            float num = Grid.Mass[index2];
            for (int index3 = 0; index3 < this.infoCount; ++index3)
            {
              if (this.infos[index3].element == element)
              {
                this.infos[index3].amount += num;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              this.infos[this.infoCount].amount = num;
              this.infos[this.infoCount].element = element;
              ++this.infoCount;
            }
          }
        }
      }
    }
    int index4 = 0;
    while (index4 < this.infoCount)
    {
      LiquidPumpingStation.LiquidInfo info = this.infos[index4];
      if ((double) info.amount <= 1.0)
      {
        if ((UnityEngine.Object) info.source != (UnityEngine.Object) null)
          info.source.DeleteObject();
        this.infos[index4] = this.infos[this.infoCount - 1];
        --this.infoCount;
      }
      else
      {
        if ((UnityEngine.Object) info.source == (UnityEngine.Object) null)
        {
          info.source = this.GetComponent<Storage>().AddLiquid(info.element.id, info.amount, info.element.defaultValues.temperature, byte.MaxValue, 0, false, true).GetComponent<SubstanceChunk>();
          Pickupable component = info.source.GetComponent<Pickupable>();
          component.GetComponent<KPrefabID>().AddTag(GameTags.LiquidSource, false);
          component.SetOffsets(new CellOffset[1]
          {
            new CellOffset(0, 1)
          });
          component.targetWorkable = (Workable) this;
          component.OnReservationsChanged += new System.Action(this.OnReservationsChanged);
        }
        info.source.GetComponent<Pickupable>().TotalAmount = info.amount;
        this.infos[index4] = info;
        ++index4;
      }
    }
    if (infoCount != this.infoCount)
      this.RefreshStatusItem();
    this.RefreshDepthAvailable();
  }

  private void RefreshStatusItem()
  {
    if (this.infoCount > 0)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, (object) this);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, (object) this);
  }

  public string ResolveString(string base_string)
  {
    string newValue = string.Empty;
    for (int index = 0; index < this.infoCount; ++index)
    {
      if ((UnityEngine.Object) this.infos[index].source != (UnityEngine.Object) null)
        newValue = newValue + "\n" + this.infos[index].element.name + ": " + GameUtil.GetFormattedMass(this.infos[index].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
    }
    return base_string.Replace("{Liquids}", newValue);
  }

  public static bool IsLiquidAccessible(Element element)
  {
    return true;
  }

  public override float GetPercentComplete()
  {
    if (this.session != null)
      return this.session.GetPercentComplete();
    return 0.0f;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    Pickupable.PickupableStartWorkInfo startWorkInfo = (Pickupable.PickupableStartWorkInfo) worker.startWorkInfo;
    float amount = startWorkInfo.amount;
    Element element = startWorkInfo.originalPickupable.GetComponent<PrimaryElement>().Element;
    this.session = new LiquidPumpingStation.WorkSession(Grid.PosToCell((KMonoBehaviour) this), element.id, startWorkInfo.originalPickupable.GetComponent<SubstanceChunk>(), amount, this.gameObject);
    this.meter.SetPositionPercent(0.0f);
    this.meter.SetSymbolTint(new KAnimHashedString("meter_target"), element.substance.colour);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    if (this.session != null)
    {
      this.session.Cleanup();
      this.session = (LiquidPumpingStation.WorkSession) null;
    }
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "on", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void OnReservationsChanged()
  {
    bool is_unfetchable = false;
    for (int index = 0; index < this.infoCount; ++index)
    {
      if ((UnityEngine.Object) this.infos[index].source != (UnityEngine.Object) null && (double) this.infos[index].source.GetComponent<Pickupable>().ReservedAmount > 0.0)
      {
        is_unfetchable = true;
        break;
      }
    }
    for (int index = 0; index < this.infoCount; ++index)
    {
      if ((UnityEngine.Object) this.infos[index].source != (UnityEngine.Object) null)
        this.infos[index].source.GetSMI<FetchableMonitor.Instance>()?.SetForceUnfetchable(is_unfetchable);
    }
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    if (this.session == null)
      return;
    Storage component1 = worker.GetComponent<Storage>();
    float consumedAmount = this.session.GetConsumedAmount();
    if ((double) consumedAmount > 0.0)
    {
      SubstanceChunk source = this.session.GetSource();
      SimUtil.DiseaseInfo diseaseInfo = this.session == null ? SimUtil.DiseaseInfo.Invalid : this.session.GetDiseaseInfo();
      PrimaryElement component2 = source.GetComponent<PrimaryElement>();
      Pickupable component3 = LiquidSourceManager.Instance.CreateChunk(component2.Element, consumedAmount, this.session.GetTemperature(), diseaseInfo.idx, diseaseInfo.count, this.transform.GetPosition()).GetComponent<Pickupable>();
      component3.TotalAmount = consumedAmount;
      component3.Trigger(1335436905, (object) source.GetComponent<Pickupable>());
      worker.workCompleteData = (object) component3;
      this.Sim200ms(0.0f);
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        component1.Store(component3.gameObject, false, false, true, false);
    }
    this.session.Cleanup();
    this.session = (LiquidPumpingStation.WorkSession) null;
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (this.session != null)
    {
      this.meter.SetPositionPercent(this.session.GetPercentComplete());
      if ((double) this.session.GetLastTickAmount() <= 0.0)
        return true;
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.session != null)
    {
      this.session.Cleanup();
      this.session = (LiquidPumpingStation.WorkSession) null;
    }
    for (int index = 0; index < this.infoCount; ++index)
    {
      if ((UnityEngine.Object) this.infos[index].source != (UnityEngine.Object) null)
        this.infos[index].source.DeleteObject();
    }
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    for (int index = 0; index < LiquidPumpingStation.floorOffsets.Length; ++index)
    {
      int cell2 = Grid.OffsetCell(cell1, LiquidPumpingStation.floorOffsets[index]);
      Grid.FakeFloor[cell2] = false;
      Pathfinding.Instance.AddDirtyNavGridCell(cell2);
    }
  }

  private class WorkSession
  {
    private int cell;
    private float amountToPickup;
    private float consumedAmount;
    private float temperature;
    private float amountPerTick;
    private SimHashes element;
    private float lastTickAmount;
    private SubstanceChunk source;
    private SimUtil.DiseaseInfo diseaseInfo;
    private GameObject pump;

    public WorkSession(
      int cell,
      SimHashes element,
      SubstanceChunk source,
      float amount_to_pickup,
      GameObject pump)
    {
      this.cell = cell;
      this.element = element;
      this.source = source;
      this.amountToPickup = amount_to_pickup;
      this.temperature = ElementLoader.FindElementByHash(element).defaultValues.temperature;
      this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
      this.amountPerTick = 40f;
      this.pump = pump;
      this.lastTickAmount = this.amountPerTick;
      this.ConsumeMass();
    }

    private void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
    {
      ((LiquidPumpingStation.WorkSession) data).OnSimConsume(mass_cb_info);
    }

    private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
    {
      this.temperature = (double) this.consumedAmount != 0.0 ? GameUtil.GetFinalTemperature(this.temperature, this.consumedAmount, mass_cb_info.temperature, mass_cb_info.mass) : mass_cb_info.temperature;
      this.consumedAmount += mass_cb_info.mass;
      this.lastTickAmount = mass_cb_info.mass;
      this.diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseInfo.idx, this.diseaseInfo.count, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
      if ((double) this.consumedAmount >= (double) this.amountToPickup)
      {
        this.amountPerTick = 0.0f;
        this.lastTickAmount = 0.0f;
      }
      this.ConsumeMass();
    }

    private void ConsumeMass()
    {
      if ((double) this.amountPerTick <= 0.0)
        return;
      SimMessages.ConsumeMass(Grid.OffsetCell(this.cell, new CellOffset(0, -PumpingStationGuide.GetDepthAvailable(this.cell, this.pump))), this.element, Mathf.Max(Mathf.Min(this.amountPerTick, this.amountToPickup - this.consumedAmount), 1f), (byte) 3, Game.Instance.massConsumedCallbackManager.Add(new System.Action<Sim.MassConsumedCallback, object>(this.OnSimConsumeCallback), (object) this, nameof (LiquidPumpingStation)).index);
    }

    public float GetPercentComplete()
    {
      return this.consumedAmount / this.amountToPickup;
    }

    public float GetLastTickAmount()
    {
      return this.lastTickAmount;
    }

    public SimUtil.DiseaseInfo GetDiseaseInfo()
    {
      return this.diseaseInfo;
    }

    public SubstanceChunk GetSource()
    {
      return this.source;
    }

    public float GetConsumedAmount()
    {
      return this.consumedAmount;
    }

    public float GetTemperature()
    {
      if ((double) this.temperature > 0.0)
        return this.temperature;
      Debug.LogWarning((object) "TODO(YOG): Fix bad temperature in liquid pumping station.");
      return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
    }

    public void Cleanup()
    {
      this.amountPerTick = 0.0f;
      this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
    }
  }

  private struct LiquidInfo
  {
    public float amount;
    public Element element;
    public SubstanceChunk source;
  }
}
