// Decompiled with JetBrains decompiler
// Type: StructureTemperatureComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureTemperatureComponents : KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
  private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();
  private const float MAX_PRESSURE = 1.5f;
  private StatusItem operatingEnergyStatusItem;

  public HandleVector<int>.Handle Add(GameObject go)
  {
    StructureTemperaturePayload payload = new StructureTemperaturePayload(go);
    return this.Add(go, new StructureTemperatureHeader()
    {
      dirty = false,
      simHandle = -1,
      isActiveBuilding = false
    }, ref payload);
  }

  public static void ClearInstanceMap()
  {
    StructureTemperatureComponents.handleInstanceMap.Clear();
  }

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    this.InitializeStatusItem();
    base.OnPrefabInit(handle);
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    PrimaryElement primaryElement1 = payload.primaryElement;
    // ISSUE: reference to a compiler-generated field
    if (StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache0 = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    PrimaryElement.GetTemperatureCallback fMgCache0 = StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache0;
    primaryElement1.getTemperatureCallback = fMgCache0;
    PrimaryElement primaryElement2 = payload.primaryElement;
    // ISSUE: reference to a compiler-generated field
    if (StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache1 = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    PrimaryElement.SetTemperatureCallback fMgCache1 = StructureTemperatureComponents.\u003C\u003Ef__mg\u0024cache1;
    primaryElement2.setTemperatureCallback = fMgCache1;
    header.isActiveBuilding = (double) payload.building.Def.SelfHeatKilowattsWhenActive != 0.0 || (double) payload.ExhaustKilowatts != 0.0;
    this.SetHeader(handle, header);
  }

  private void InitializeStatusItem()
  {
    if (this.operatingEnergyStatusItem != null)
      return;
    this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    this.operatingEnergyStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, ev_data) =>
    {
      int index = (int) ev_data;
      StructureTemperaturePayload payload = this.GetPayload(StructureTemperatureComponents.handleInstanceMap[index]);
      if (str != (string) BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
      {
        try
        {
          str = string.Format(str, (object) GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) ex);
          Debug.LogWarning((object) BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
          Debug.LogWarning((object) str);
        }
      }
      else
      {
        string empty = string.Empty;
        foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
          empty += string.Format((string) BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, (object) energySource.source, (object) GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
        str = string.Format(str, (object) GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), (object) empty);
      }
      return str;
    });
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    if ((UnityEngine.Object) payload.operational != (UnityEngine.Object) null && header.isActiveBuilding)
      payload.primaryElement.Subscribe(824508782, (System.Action<object>) (ev_data => StructureTemperatureComponents.OnActiveChanged(handle)));
    payload.maxTemperature = !((UnityEngine.Object) payload.overheatable != (UnityEngine.Object) null) ? 10000f : payload.overheatable.OverheatTemperature;
    if ((double) payload.maxTemperature <= 0.0)
      Debug.LogError((object) "invalid max temperature");
    this.SetPayload(handle, ref payload);
    this.SimRegister(handle, ref header, ref payload);
  }

  private static void OnActiveChanged(HandleVector<int>.Handle handle)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    payload.primaryElement.InternalTemperature = payload.Temperature;
    header.dirty = true;
    GameComps.StructureTemperatures.SetHeader(handle, header);
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    this.SimUnregister(handle);
    base.OnCleanUp(handle);
  }

  public override void Sim200ms(float dt)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    List<StructureTemperatureHeader> headers;
    List<StructureTemperaturePayload> payloads;
    this.GetDataLists(out headers, out payloads);
    ListPool<int, StructureTemperatureComponents>.PooledList pooledList1 = ListPool<int, StructureTemperatureComponents>.Allocate();
    pooledList1.Capacity = Math.Max(pooledList1.Capacity, headers.Count);
    ListPool<int, StructureTemperatureComponents>.PooledList pooledList2 = ListPool<int, StructureTemperatureComponents>.Allocate();
    pooledList2.Capacity = Math.Max(pooledList2.Capacity, headers.Count);
    ListPool<int, StructureTemperatureComponents>.PooledList pooledList3 = ListPool<int, StructureTemperatureComponents>.Allocate();
    pooledList3.Capacity = Math.Max(pooledList3.Capacity, headers.Count);
    for (int index = 0; index != headers.Count; ++index)
    {
      StructureTemperatureHeader temperatureHeader = headers[index];
      if (Sim.IsValidHandle(temperatureHeader.simHandle))
      {
        pooledList1.Add(index);
        if (temperatureHeader.dirty)
        {
          pooledList2.Add(index);
          temperatureHeader.dirty = false;
          headers[index] = temperatureHeader;
        }
        if (temperatureHeader.isActiveBuilding)
          pooledList3.Add(index);
      }
    }
    foreach (int index in (List<int>) pooledList2)
    {
      StructureTemperaturePayload payload = payloads[index];
      StructureTemperatureComponents.UpdateSimState(ref payload);
    }
    foreach (int index in (List<int>) pooledList2)
    {
      if ((double) payloads[index].pendingEnergyModifications != 0.0)
      {
        StructureTemperaturePayload temperaturePayload = payloads[index];
        SimMessages.ModifyBuildingEnergy(temperaturePayload.simHandleCopy, temperaturePayload.pendingEnergyModifications, 0.0f, 10000f);
        temperaturePayload.pendingEnergyModifications = 0.0f;
        payloads[index] = temperaturePayload;
      }
    }
    foreach (int index1 in (List<int>) pooledList3)
    {
      StructureTemperaturePayload temperaturePayload = payloads[index1];
      if ((UnityEngine.Object) temperaturePayload.operational == (UnityEngine.Object) null || temperaturePayload.operational.IsActive)
      {
        ++num1;
        if (!temperaturePayload.isActiveStatusItemSet)
        {
          ++num3;
          temperaturePayload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, (object) temperaturePayload.simHandleCopy);
          temperaturePayload.isActiveStatusItemSet = true;
        }
        temperaturePayload.energySourcesKW = this.AccumulateProducedEnergyKW(temperaturePayload.energySourcesKW, temperaturePayload.OperatingKilowatts, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
        if ((double) temperaturePayload.ExhaustKilowatts != 0.0)
        {
          ++num2;
          Extents extents = temperaturePayload.GetExtents();
          int num4 = extents.width * extents.height;
          float num5 = temperaturePayload.ExhaustKilowatts * dt / (float) num4;
          for (int index2 = 0; index2 < extents.height; ++index2)
          {
            int num6 = extents.y + index2;
            for (int index3 = 0; index3 < extents.width; ++index3)
            {
              int num7 = extents.x + index3;
              int gameCell = num6 * Grid.WidthInCells + num7;
              float num8 = Mathf.Min(Grid.Mass[gameCell], 1.5f) / 1.5f;
              float kilojoules = num5 * num8;
              SimMessages.ModifyEnergy(gameCell, kilojoules, temperaturePayload.maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
            }
          }
          temperaturePayload.energySourcesKW = this.AccumulateProducedEnergyKW(temperaturePayload.energySourcesKW, temperaturePayload.ExhaustKilowatts, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
        }
      }
      else if (temperaturePayload.isActiveStatusItemSet)
      {
        ++num3;
        temperaturePayload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, (StatusItem) null, (object) null);
        temperaturePayload.isActiveStatusItemSet = false;
      }
      payloads[index1] = temperaturePayload;
    }
    pooledList3.Recycle();
    pooledList2.Recycle();
    pooledList1.Recycle();
  }

  private static void UpdateSimState(ref StructureTemperaturePayload payload)
  {
    DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
    float internalTemperature = payload.primaryElement.InternalTemperature;
    BuildingDef def = payload.building.Def;
    float mass = def.MassForTemperatureModification;
    float operatingKilowatts = payload.OperatingKilowatts;
    float overheat_temperature = !((UnityEngine.Object) payload.overheatable != (UnityEngine.Object) null) ? 10000f : payload.overheatable.OverheatTemperature;
    if (!payload.enabled || payload.bypass)
      mass = 0.0f;
    Extents extents = payload.GetExtents();
    byte idx = payload.primaryElement.Element.idx;
    SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
  }

  private static unsafe float OnGetTemperature(PrimaryElement primary_element)
  {
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
    StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
    float num;
    if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
    {
      if (!payload.bypass)
      {
        num = Game.Instance.simData.buildingTemperatures[Sim.GetHandleIndex(payload.simHandleCopy)].temperature;
      }
      else
      {
        int cell = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
        num = Grid.Temperature[cell];
      }
    }
    else
      num = payload.primaryElement.InternalTemperature;
    return num;
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    payload.primaryElement.InternalTemperature = temperature;
    header.dirty = true;
    GameComps.StructureTemperatures.SetHeader(handle, header);
    if (header.isActiveBuilding || !Sim.IsValidHandle(payload.simHandleCopy))
      return;
    StructureTemperatureComponents.UpdateSimState(ref payload);
    if ((double) payload.pendingEnergyModifications == 0.0)
      return;
    SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, payload.pendingEnergyModifications, 0.0f, 10000f);
    payload.pendingEnergyModifications = 0.0f;
    GameComps.StructureTemperatures.SetPayload(handle, ref payload);
  }

  public void ProduceEnergy(
    HandleVector<int>.Handle handle,
    float delta_kilojoules,
    string source,
    float display_dt)
  {
    StructureTemperaturePayload payload = this.GetPayload(handle);
    if (Sim.IsValidHandle(payload.simHandleCopy))
    {
      SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0.0f, 10000f);
    }
    else
    {
      payload.pendingEnergyModifications += delta_kilojoules;
      StructureTemperatureHeader header = this.GetHeader(handle);
      header.dirty = true;
      this.SetHeader(handle, header);
    }
    payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
    this.SetPayload(handle, ref payload);
  }

  private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(
    List<StructureTemperaturePayload.EnergySource> sources,
    float kw,
    string source)
  {
    if (sources == null)
      sources = new List<StructureTemperaturePayload.EnergySource>();
    bool flag = false;
    for (int index = 0; index < sources.Count; ++index)
    {
      if (sources[index].source == source)
      {
        sources[index].Accumulate(kw);
        flag = true;
        break;
      }
    }
    if (!flag)
      sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
    return sources;
  }

  public static void DoStateTransition(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
  }

  public static void DoMelt(PrimaryElement primary_element)
  {
    Element element = primary_element.Element;
    if (element.highTempTransitionTarget == SimHashes.Unobtanium)
      return;
    SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
    Util.KDestroyGameObject(primary_element.gameObject);
  }

  public static void DoOverheat(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615, (object) null);
  }

  public static void DoNoLongerOverheated(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937, (object) null);
  }

  public bool IsEnabled(HandleVector<int>.Handle handle)
  {
    return this.GetPayload(handle).enabled;
  }

  private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    header.dirty = true;
    payload.enabled = isEnabled;
    this.SetData(handle, header, ref payload);
  }

  public void Enable(HandleVector<int>.Handle handle)
  {
    this.Enable(handle, true);
  }

  public void Disable(HandleVector<int>.Handle handle)
  {
    this.Enable(handle, false);
  }

  public bool IsBypassed(HandleVector<int>.Handle handle)
  {
    return this.GetPayload(handle).bypass;
  }

  private void Bypass(HandleVector<int>.Handle handle, bool bypass)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    header.dirty = true;
    payload.bypass = bypass;
    this.SetData(handle, header, ref payload);
  }

  public void Bypass(HandleVector<int>.Handle handle)
  {
    this.Bypass(handle, true);
  }

  public void UnBypass(HandleVector<int>.Handle handle)
  {
    this.Bypass(handle, false);
  }

  protected void SimRegister(
    HandleVector<int>.Handle handle,
    ref StructureTemperatureHeader header,
    ref StructureTemperaturePayload payload)
  {
    if (payload.simHandleCopy != -1)
      return;
    PrimaryElement primaryElement = payload.primaryElement;
    if ((double) primaryElement.Mass <= 0.0 || primaryElement.Element.IsTemperatureInsulated)
      return;
    payload.simHandleCopy = -2;
    string dbg_name = primaryElement.name;
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle1 = Game.Instance.simComponentCallbackManager.Add((System.Action<int, object>) ((sim_handle, callback_data) => StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name)), (object) null, "StructureTemperature.SimRegister");
    BuildingDef def = primaryElement.GetComponent<Building>().Def;
    float internalTemperature = primaryElement.InternalTemperature;
    float temperatureModification = def.MassForTemperatureModification;
    float operatingKilowatts = payload.OperatingKilowatts;
    Extents extents = payload.GetExtents();
    byte elem_idx = (byte) ElementLoader.elements.IndexOf(primaryElement.Element);
    SimMessages.AddBuildingHeatExchange(extents, temperatureModification, internalTemperature, def.ThermalConductivity, operatingKilowatts, elem_idx, handle1.index);
    header.simHandle = payload.simHandleCopy;
    this.SetData(handle, header, ref payload);
  }

  private static void OnSimRegistered(
    HandleVector<int>.Handle handle,
    int sim_handle,
    string dbg_name)
  {
    if (!GameComps.StructureTemperatures.IsValid(handle) || !GameComps.StructureTemperatures.IsVersionValid(handle))
      return;
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    if (payload.simHandleCopy == -2)
    {
      StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
      header.simHandle = sim_handle;
      payload.simHandleCopy = sim_handle;
      GameComps.StructureTemperatures.SetData(handle, header, ref payload);
      payload.primaryElement.Trigger(-1555603773, (object) null);
    }
    else
      SimMessages.RemoveBuildingHeatExchange(sim_handle, -1);
  }

  protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
  {
    if (!GameComps.StructureTemperatures.IsVersionValid(handle))
    {
      KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister");
    }
    else
    {
      if (KMonoBehaviour.isLoadingScene)
        return;
      StructureTemperatureHeader header;
      StructureTemperaturePayload payload;
      GameComps.StructureTemperatures.GetData(handle, out header, out payload);
      if (payload.simHandleCopy == -1)
        return;
      if (Sim.IsValidHandle(payload.simHandleCopy))
      {
        int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
        payload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
        SimMessages.RemoveBuildingHeatExchange(payload.simHandleCopy, -1);
        StructureTemperatureComponents.handleInstanceMap.Remove(payload.simHandleCopy);
      }
      payload.simHandleCopy = -1;
      header.simHandle = -1;
      this.SetData(handle, header, ref payload);
    }
  }
}
