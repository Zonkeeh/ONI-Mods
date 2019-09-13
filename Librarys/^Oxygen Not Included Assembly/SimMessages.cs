// Decompiled with JetBrains decompiler
// Type: SimMessages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public static class SimMessages
{
  public const int InvalidCallback = -1;
  public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

  public static unsafe void AddElementConsumer(
    int gameCell,
    ElementConsumer.Configuration configuration,
    SimHashes element,
    byte radius,
    int cb_handle)
  {
    Debug.Assert(Grid.IsValidCell(gameCell));
    if (!Grid.IsValidCell(gameCell))
      return;
    int elementIndex = ElementLoader.GetElementIndex(element);
    // ISSUE: untyped stack allocation
    SimMessages.AddElementConsumerMessage* elementConsumerMessagePtr = (SimMessages.AddElementConsumerMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.AddElementConsumerMessage))));
    elementConsumerMessagePtr->cellIdx = gameCell;
    elementConsumerMessagePtr->configuration = (byte) configuration;
    elementConsumerMessagePtr->elementIdx = (byte) elementIndex;
    elementConsumerMessagePtr->radius = radius;
    elementConsumerMessagePtr->callbackIdx = cb_handle;
    Sim.SIM_HandleMessage(2024405073, sizeof (SimMessages.AddElementConsumerMessage), (byte*) elementConsumerMessagePtr);
  }

  public static unsafe void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
  {
    if (!Sim.IsValidHandle(sim_handle))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.SetElementConsumerDataMessage* consumerDataMessagePtr = (SimMessages.SetElementConsumerDataMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.SetElementConsumerDataMessage))));
    consumerDataMessagePtr->handle = sim_handle;
    consumerDataMessagePtr->cell = cell;
    consumerDataMessagePtr->consumptionRate = consumptionRate;
    Sim.SIM_HandleMessage(1575539738, sizeof (SimMessages.SetElementConsumerDataMessage), (byte*) consumerDataMessagePtr);
  }

  public static unsafe void RemoveElementConsumer(int cb_handle, int sim_handle)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.RemoveElementConsumerMessage* elementConsumerMessagePtr = (SimMessages.RemoveElementConsumerMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.RemoveElementConsumerMessage))));
      elementConsumerMessagePtr->callbackIdx = cb_handle;
      elementConsumerMessagePtr->handle = sim_handle;
      Sim.SIM_HandleMessage(894417742, sizeof (SimMessages.RemoveElementConsumerMessage), (byte*) elementConsumerMessagePtr);
    }
  }

  public static unsafe void AddElementEmitter(
    float max_pressure,
    int on_registered,
    int on_blocked = -1,
    int on_unblocked = -1)
  {
    // ISSUE: untyped stack allocation
    SimMessages.AddElementEmitterMessage* elementEmitterMessagePtr = (SimMessages.AddElementEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.AddElementEmitterMessage))));
    elementEmitterMessagePtr->maxPressure = max_pressure;
    elementEmitterMessagePtr->callbackIdx = on_registered;
    elementEmitterMessagePtr->onBlockedCB = on_blocked;
    elementEmitterMessagePtr->onUnblockedCB = on_unblocked;
    Sim.SIM_HandleMessage(-505471181, sizeof (SimMessages.AddElementEmitterMessage), (byte*) elementEmitterMessagePtr);
  }

  public static unsafe void ModifyElementEmitter(
    int sim_handle,
    int game_cell,
    int max_depth,
    SimHashes element,
    float emit_interval,
    float emit_mass,
    float emit_temperature,
    float max_pressure,
    byte disease_idx,
    int disease_count)
  {
    Debug.Assert(Grid.IsValidCell(game_cell));
    if (!Grid.IsValidCell(game_cell))
      return;
    int elementIndex = ElementLoader.GetElementIndex(element);
    // ISSUE: untyped stack allocation
    SimMessages.ModifyElementEmitterMessage* elementEmitterMessagePtr = (SimMessages.ModifyElementEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyElementEmitterMessage))));
    elementEmitterMessagePtr->handle = sim_handle;
    elementEmitterMessagePtr->cellIdx = game_cell;
    elementEmitterMessagePtr->emitInterval = emit_interval;
    elementEmitterMessagePtr->emitMass = emit_mass;
    elementEmitterMessagePtr->emitTemperature = emit_temperature;
    elementEmitterMessagePtr->maxPressure = max_pressure;
    elementEmitterMessagePtr->elementIdx = (byte) elementIndex;
    elementEmitterMessagePtr->maxDepth = (byte) max_depth;
    elementEmitterMessagePtr->diseaseIdx = disease_idx;
    elementEmitterMessagePtr->diseaseCount = disease_count;
    Sim.SIM_HandleMessage(403589164, sizeof (SimMessages.ModifyElementEmitterMessage), (byte*) elementEmitterMessagePtr);
  }

  public static unsafe void RemoveElementEmitter(int cb_handle, int sim_handle)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.RemoveElementEmitterMessage* elementEmitterMessagePtr = (SimMessages.RemoveElementEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.RemoveElementEmitterMessage))));
      elementEmitterMessagePtr->callbackIdx = cb_handle;
      elementEmitterMessagePtr->handle = sim_handle;
      Sim.SIM_HandleMessage(-1524118282, sizeof (SimMessages.RemoveElementEmitterMessage), (byte*) elementEmitterMessagePtr);
    }
  }

  public static unsafe void AddElementChunk(
    int gameCell,
    SimHashes element,
    float mass,
    float temperature,
    float surface_area,
    float thickness,
    float ground_transfer_scale,
    int cb_handle)
  {
    Debug.Assert(Grid.IsValidCell(gameCell));
    if (!Grid.IsValidCell(gameCell) || (double) mass * (double) temperature <= 0.0)
      return;
    int elementIndex = ElementLoader.GetElementIndex(element);
    // ISSUE: untyped stack allocation
    SimMessages.AddElementChunkMessage* elementChunkMessagePtr = (SimMessages.AddElementChunkMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.AddElementChunkMessage))));
    elementChunkMessagePtr->gameCell = gameCell;
    elementChunkMessagePtr->callbackIdx = cb_handle;
    elementChunkMessagePtr->mass = mass;
    elementChunkMessagePtr->temperature = temperature;
    elementChunkMessagePtr->surfaceArea = surface_area;
    elementChunkMessagePtr->thickness = thickness;
    elementChunkMessagePtr->groundTransferScale = ground_transfer_scale;
    elementChunkMessagePtr->elementIdx = (byte) elementIndex;
    Sim.SIM_HandleMessage(1445724082, sizeof (SimMessages.AddElementChunkMessage), (byte*) elementChunkMessagePtr);
  }

  public static unsafe void RemoveElementChunk(int sim_handle, int cb_handle)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.RemoveElementChunkMessage* elementChunkMessagePtr = (SimMessages.RemoveElementChunkMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.RemoveElementChunkMessage))));
      elementChunkMessagePtr->callbackIdx = cb_handle;
      elementChunkMessagePtr->handle = sim_handle;
      Sim.SIM_HandleMessage(-912908555, sizeof (SimMessages.RemoveElementChunkMessage), (byte*) elementChunkMessagePtr);
    }
  }

  public static unsafe void SetElementChunkData(
    int sim_handle,
    float temperature,
    float heat_capacity)
  {
    if (!Sim.IsValidHandle(sim_handle))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.SetElementChunkDataMessage* chunkDataMessagePtr = (SimMessages.SetElementChunkDataMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.SetElementChunkDataMessage))));
    chunkDataMessagePtr->handle = sim_handle;
    chunkDataMessagePtr->temperature = temperature;
    chunkDataMessagePtr->heatCapacity = heat_capacity;
    Sim.SIM_HandleMessage(-435115907, sizeof (SimMessages.SetElementChunkDataMessage), (byte*) chunkDataMessagePtr);
  }

  public static unsafe void MoveElementChunk(int sim_handle, int cell)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.MoveElementChunkMessage* elementChunkMessagePtr = (SimMessages.MoveElementChunkMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.MoveElementChunkMessage))));
      elementChunkMessagePtr->handle = sim_handle;
      elementChunkMessagePtr->gameCell = cell;
      Sim.SIM_HandleMessage(-374911358, sizeof (SimMessages.MoveElementChunkMessage), (byte*) elementChunkMessagePtr);
    }
  }

  public static unsafe void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.ModifyElementChunkEnergyMessage* chunkEnergyMessagePtr = (SimMessages.ModifyElementChunkEnergyMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyElementChunkEnergyMessage))));
      chunkEnergyMessagePtr->handle = sim_handle;
      chunkEnergyMessagePtr->deltaKJ = delta_kj;
      Sim.SIM_HandleMessage(1020555667, sizeof (SimMessages.ModifyElementChunkEnergyMessage), (byte*) chunkEnergyMessagePtr);
    }
  }

  public static unsafe void ModifyElementChunkTemperatureAdjuster(
    int sim_handle,
    float temperature,
    float heat_capacity,
    float thermal_conductivity)
  {
    if (!Sim.IsValidHandle(sim_handle))
    {
      Debug.Assert(false, (object) "Invalid handle");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.ModifyElementChunkAdjusterMessage* chunkAdjusterMessagePtr = (SimMessages.ModifyElementChunkAdjusterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyElementChunkAdjusterMessage))));
      chunkAdjusterMessagePtr->handle = sim_handle;
      chunkAdjusterMessagePtr->temperature = temperature;
      chunkAdjusterMessagePtr->heatCapacity = heat_capacity;
      chunkAdjusterMessagePtr->thermalConductivity = thermal_conductivity;
      Sim.SIM_HandleMessage(-1387601379, sizeof (SimMessages.ModifyElementChunkAdjusterMessage), (byte*) chunkAdjusterMessagePtr);
    }
  }

  public static unsafe void AddBuildingHeatExchange(
    Extents extents,
    float mass,
    float temperature,
    float thermal_conductivity,
    float operating_kw,
    byte elem_idx,
    int callbackIdx = -1)
  {
    int cell1 = Grid.XYToCell(extents.x, extents.y);
    Debug.Assert(Grid.IsValidCell(cell1));
    if (!Grid.IsValidCell(cell1))
      return;
    int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
    if (!Grid.IsValidCell(cell2))
      Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", (object) cell2, (object) extents.x, (object) extents.y, (object) extents.width, (object) extents.height);
    if (!Grid.IsValidCell(cell2))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.AddBuildingHeatExchangeMessage* heatExchangeMessagePtr = (SimMessages.AddBuildingHeatExchangeMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.AddBuildingHeatExchangeMessage))));
    heatExchangeMessagePtr->callbackIdx = callbackIdx;
    heatExchangeMessagePtr->elemIdx = elem_idx;
    heatExchangeMessagePtr->mass = mass;
    heatExchangeMessagePtr->temperature = temperature;
    heatExchangeMessagePtr->thermalConductivity = thermal_conductivity;
    heatExchangeMessagePtr->overheatTemperature = float.MaxValue;
    heatExchangeMessagePtr->operatingKilowatts = operating_kw;
    heatExchangeMessagePtr->minX = extents.x;
    heatExchangeMessagePtr->minY = extents.y;
    heatExchangeMessagePtr->maxX = extents.x + extents.width;
    heatExchangeMessagePtr->maxY = extents.y + extents.height;
    Sim.SIM_HandleMessage(1739021608, sizeof (SimMessages.AddBuildingHeatExchangeMessage), (byte*) heatExchangeMessagePtr);
  }

  public static unsafe void ModifyBuildingHeatExchange(
    int sim_handle,
    Extents extents,
    float mass,
    float temperature,
    float thermal_conductivity,
    float overheat_temperature,
    float operating_kw,
    byte element_idx)
  {
    int cell1 = Grid.XYToCell(extents.x, extents.y);
    Debug.Assert(Grid.IsValidCell(cell1));
    if (!Grid.IsValidCell(cell1))
      return;
    int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
    Debug.Assert(Grid.IsValidCell(cell2));
    if (!Grid.IsValidCell(cell2))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.ModifyBuildingHeatExchangeMessage* heatExchangeMessagePtr = (SimMessages.ModifyBuildingHeatExchangeMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyBuildingHeatExchangeMessage))));
    heatExchangeMessagePtr->callbackIdx = sim_handle;
    heatExchangeMessagePtr->elemIdx = element_idx;
    heatExchangeMessagePtr->mass = mass;
    heatExchangeMessagePtr->temperature = temperature;
    heatExchangeMessagePtr->thermalConductivity = thermal_conductivity;
    heatExchangeMessagePtr->overheatTemperature = overheat_temperature;
    heatExchangeMessagePtr->operatingKilowatts = operating_kw;
    heatExchangeMessagePtr->minX = extents.x;
    heatExchangeMessagePtr->minY = extents.y;
    heatExchangeMessagePtr->maxX = extents.x + extents.width;
    heatExchangeMessagePtr->maxY = extents.y + extents.height;
    Sim.SIM_HandleMessage(1818001569, sizeof (SimMessages.ModifyBuildingHeatExchangeMessage), (byte*) heatExchangeMessagePtr);
  }

  public static unsafe void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
  {
    // ISSUE: untyped stack allocation
    SimMessages.RemoveBuildingHeatExchangeMessage* heatExchangeMessagePtr = (SimMessages.RemoveBuildingHeatExchangeMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.RemoveBuildingHeatExchangeMessage))));
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    heatExchangeMessagePtr->handle = sim_handle;
    heatExchangeMessagePtr->callbackIdx = callbackIdx;
    Sim.SIM_HandleMessage(-456116629, sizeof (SimMessages.RemoveBuildingHeatExchangeMessage), (byte*) heatExchangeMessagePtr);
  }

  public static unsafe void ModifyBuildingEnergy(
    int sim_handle,
    float delta_kj,
    float min_temperature,
    float max_temperature)
  {
    // ISSUE: untyped stack allocation
    SimMessages.ModifyBuildingEnergyMessage* buildingEnergyMessagePtr = (SimMessages.ModifyBuildingEnergyMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyBuildingEnergyMessage))));
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    buildingEnergyMessagePtr->handle = sim_handle;
    buildingEnergyMessagePtr->deltaKJ = delta_kj;
    buildingEnergyMessagePtr->minTemperature = min_temperature;
    buildingEnergyMessagePtr->maxTemperature = max_temperature;
    Sim.SIM_HandleMessage(-1348791658, sizeof (SimMessages.ModifyBuildingEnergyMessage), (byte*) buildingEnergyMessagePtr);
  }

  public static unsafe void AddDiseaseEmitter(int callbackIdx)
  {
    // ISSUE: untyped stack allocation
    SimMessages.AddDiseaseEmitterMessage* diseaseEmitterMessagePtr = (SimMessages.AddDiseaseEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.AddDiseaseEmitterMessage))));
    diseaseEmitterMessagePtr->callbackIdx = callbackIdx;
    Sim.SIM_HandleMessage(1486783027, sizeof (SimMessages.AddDiseaseEmitterMessage), (byte*) diseaseEmitterMessagePtr);
  }

  public static unsafe void ModifyDiseaseEmitter(
    int sim_handle,
    int cell,
    byte range,
    byte disease_idx,
    float emit_interval,
    int emit_count)
  {
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    // ISSUE: untyped stack allocation
    SimMessages.ModifyDiseaseEmitterMessage* diseaseEmitterMessagePtr = (SimMessages.ModifyDiseaseEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyDiseaseEmitterMessage))));
    diseaseEmitterMessagePtr->handle = sim_handle;
    diseaseEmitterMessagePtr->gameCell = cell;
    diseaseEmitterMessagePtr->maxDepth = range;
    diseaseEmitterMessagePtr->diseaseIdx = disease_idx;
    diseaseEmitterMessagePtr->emitInterval = emit_interval;
    diseaseEmitterMessagePtr->emitCount = emit_count;
    Sim.SIM_HandleMessage(-1899123924, sizeof (SimMessages.ModifyDiseaseEmitterMessage), (byte*) diseaseEmitterMessagePtr);
  }

  public static unsafe void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
  {
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    // ISSUE: untyped stack allocation
    SimMessages.RemoveDiseaseEmitterMessage* diseaseEmitterMessagePtr = (SimMessages.RemoveDiseaseEmitterMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.RemoveDiseaseEmitterMessage))));
    diseaseEmitterMessagePtr->handle = sim_handle;
    diseaseEmitterMessagePtr->callbackIdx = cb_handle;
    Sim.SIM_HandleMessage(468135926, sizeof (SimMessages.RemoveDiseaseEmitterMessage), (byte*) diseaseEmitterMessagePtr);
  }

  public static unsafe void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
  {
    // ISSUE: untyped stack allocation
    SimMessages.SetSavedOptionsMessage* savedOptionsMessagePtr1 = (SimMessages.SetSavedOptionsMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.SetSavedOptionsMessage))));
    if (zero_or_one == 0)
    {
      SimMessages.SetSavedOptionsMessage* savedOptionsMessagePtr2 = savedOptionsMessagePtr1;
      int num = (int) ((SimMessages.SimSavedOptions) savedOptionsMessagePtr2->clearBits | option);
      savedOptionsMessagePtr2->clearBits = (byte) num;
      savedOptionsMessagePtr1->setBits = (byte) 0;
    }
    else
    {
      savedOptionsMessagePtr1->clearBits = (byte) 0;
      SimMessages.SetSavedOptionsMessage* savedOptionsMessagePtr2 = savedOptionsMessagePtr1;
      int num = (int) ((SimMessages.SimSavedOptions) savedOptionsMessagePtr2->setBits | option);
      savedOptionsMessagePtr2->setBits = (byte) num;
    }
    Sim.SIM_HandleMessage(1154135737, sizeof (SimMessages.SetSavedOptionsMessage), (byte*) savedOptionsMessagePtr1);
  }

  private static void WriteKleiString(this BinaryWriter writer, string str)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    writer.Write(bytes.Length);
    if (bytes.Length <= 0)
      return;
    writer.Write(bytes);
  }

  public static unsafe void CreateSimElementsTable(List<Element> elements)
  {
    MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof (int)) + Marshal.SizeOf(typeof (Sim.Element)) * elements.Count);
    BinaryWriter writer = new BinaryWriter((Stream) memoryStream);
    writer.Write(elements.Count);
    for (int index = 0; index < elements.Count; ++index)
      new Sim.Element(elements[index], elements).Write(writer);
    for (int index = 0; index < elements.Count; ++index)
      writer.WriteKleiString(UI.StripLinkFormatting(elements[index].name));
    byte[] buffer = memoryStream.GetBuffer();
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg = &^(buffer == null || buffer.Length == 0 ? (byte&) IntPtr.Zero : ref buffer[0]))
      Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
  }

  public static unsafe void CreateWorldGenHACKDiseaseTable(List<string> diseaseIds)
  {
    MemoryStream memoryStream = new MemoryStream(1024);
    BinaryWriter writer = new BinaryWriter((Stream) memoryStream);
    writer.Write(diseaseIds.Count);
    List<Element> elements = ElementLoader.elements;
    writer.Write(elements.Count);
    Klei.AI.Disease.RangeInfo rangeInfo1;
    rangeInfo1.maxGrowth = 350f;
    rangeInfo1.minGrowth = 250f;
    rangeInfo1.minViable = 200f;
    rangeInfo1.maxViable = 400f;
    Klei.AI.Disease.RangeInfo rangeInfo2;
    rangeInfo2.maxGrowth = float.PositiveInfinity;
    rangeInfo2.minGrowth = float.PositiveInfinity;
    rangeInfo2.minViable = float.PositiveInfinity;
    rangeInfo2.maxViable = float.PositiveInfinity;
    for (int index1 = 0; index1 < diseaseIds.Count; ++index1)
    {
      writer.WriteKleiString(string.Empty);
      writer.Write(new HashedString(diseaseIds[index1]).GetHashCode());
      writer.Write(0.0f);
      rangeInfo1.Write(writer);
      rangeInfo2.Write(writer);
      rangeInfo1.Write(writer);
      rangeInfo2.Write(writer);
      for (int index2 = 0; index2 < elements.Count; ++index2)
        Klei.AI.Disease.DEFAULT_GROWTH_INFO.Write(writer);
    }
    byte[] buffer = memoryStream.GetBuffer();
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg = &^(buffer == null || buffer.Length == 0 ? (byte&) IntPtr.Zero : ref buffer[0]))
      Sim.SIM_HandleMessage(825301935, (int) memoryStream.Length, msg);
  }

  public static unsafe void CreateDiseaseTable()
  {
    Diseases diseases = Db.Get().Diseases;
    MemoryStream memoryStream = new MemoryStream(1024);
    BinaryWriter writer = new BinaryWriter((Stream) memoryStream);
    writer.Write(diseases.Count);
    List<Element> elements = ElementLoader.elements;
    writer.Write(elements.Count);
    for (int index1 = 0; index1 < diseases.Count; ++index1)
    {
      Klei.AI.Disease disease = diseases[index1];
      writer.WriteKleiString(UI.StripLinkFormatting(disease.Name));
      writer.Write(disease.id.GetHashCode());
      writer.Write(disease.strength);
      disease.temperatureRange.Write(writer);
      disease.temperatureHalfLives.Write(writer);
      disease.pressureRange.Write(writer);
      disease.pressureHalfLives.Write(writer);
      for (int index2 = 0; index2 < elements.Count; ++index2)
        disease.elemGrowthInfo[index2].Write(writer);
    }
    byte[] buffer = memoryStream.GetBuffer();
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg = &^(buffer == null || buffer.Length == 0 ? (byte&) IntPtr.Zero : ref buffer[0]))
      Sim.SIM_HandleMessage(825301935, (int) memoryStream.Length, msg);
  }

  public static void SimDataInitializeFromCells(
    int width,
    int height,
    Sim.Cell[] cells,
    float[] bgTemp,
    Sim.DiseaseCell[] dc)
  {
    MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof (int)) + Marshal.SizeOf(typeof (int)) + Marshal.SizeOf(typeof (Sim.Cell)) * width * height + Marshal.SizeOf(typeof (float)) * width * height + Marshal.SizeOf(typeof (Sim.DiseaseCell)) * width * height);
    BinaryWriter writer = new BinaryWriter((Stream) memoryStream);
    writer.Write(width);
    writer.Write(height);
    int num = width * height;
    for (int index = 0; index < num; ++index)
      cells[index].Write(writer);
    for (int index = 0; index < num; ++index)
      writer.Write(bgTemp[index]);
    for (int index = 0; index < num; ++index)
      dc[index].Write(writer);
    byte[] buffer = memoryStream.GetBuffer();
    Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
  }

  public static unsafe void Dig(int gameCell, int callbackIdx = -1)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.DigMessage* digMessagePtr = (SimMessages.DigMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.DigMessage))));
    digMessagePtr->cellIdx = gameCell;
    digMessagePtr->callbackIdx = callbackIdx;
    Sim.SIM_HandleMessage(833038498, sizeof (SimMessages.DigMessage), (byte*) digMessagePtr);
  }

  public static unsafe void SetInsulation(int gameCell, float value)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.SetCellFloatValueMessage* floatValueMessagePtr = (SimMessages.SetCellFloatValueMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.SetCellFloatValueMessage))));
    floatValueMessagePtr->cellIdx = gameCell;
    floatValueMessagePtr->value = value;
    Sim.SIM_HandleMessage(-898773121, sizeof (SimMessages.SetCellFloatValueMessage), (byte*) floatValueMessagePtr);
  }

  public static unsafe void SetStrength(int gameCell, int weight, float strengthMultiplier)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.SetCellFloatValueMessage* floatValueMessagePtr = (SimMessages.SetCellFloatValueMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.SetCellFloatValueMessage))));
    floatValueMessagePtr->cellIdx = gameCell;
    int num1 = (int) ((double) strengthMultiplier * 4.0) & (int) sbyte.MaxValue;
    int num2 = (weight & 1) << 7 | num1;
    floatValueMessagePtr->value = (float) (byte) num2;
    Sim.SIM_HandleMessage(1593243982, sizeof (SimMessages.SetCellFloatValueMessage), (byte*) floatValueMessagePtr);
  }

  public static unsafe void SetCellProperties(int gameCell, byte properties)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.CellPropertiesMessage* propertiesMessagePtr = (SimMessages.CellPropertiesMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.CellPropertiesMessage))));
    propertiesMessagePtr->cellIdx = gameCell;
    propertiesMessagePtr->properties = properties;
    propertiesMessagePtr->set = (byte) 1;
    Sim.SIM_HandleMessage(-469311643, sizeof (SimMessages.CellPropertiesMessage), (byte*) propertiesMessagePtr);
  }

  public static unsafe void ClearCellProperties(int gameCell, byte properties)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.CellPropertiesMessage* propertiesMessagePtr = (SimMessages.CellPropertiesMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.CellPropertiesMessage))));
    propertiesMessagePtr->cellIdx = gameCell;
    propertiesMessagePtr->properties = properties;
    propertiesMessagePtr->set = (byte) 0;
    Sim.SIM_HandleMessage(-469311643, sizeof (SimMessages.CellPropertiesMessage), (byte*) propertiesMessagePtr);
  }

  public static unsafe void ModifyCell(
    int gameCell,
    int elementIdx,
    float temperature,
    float mass,
    byte disease_idx,
    int disease_count,
    SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None,
    bool do_vertical_solid_displacement = false,
    int callbackIdx = -1)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    Element element = ElementLoader.elements[elementIdx];
    if ((double) element.maxMass == 0.0 && (double) mass > (double) element.maxMass)
    {
      Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", (object) gameCell, (object) elementIdx, (object) temperature, (object) mass, (object) element.id, (object) element.maxMass);
      mass = element.maxMass;
    }
    if ((double) temperature < 0.0 || (double) temperature > 10000.0)
    {
      Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", (object) gameCell, (object) elementIdx, (object) temperature, (object) mass, (object) element.id, (object) element.defaultValues.temperature);
      temperature = element.defaultValues.temperature;
    }
    if ((double) temperature == 0.0 && (double) mass > 0.0)
    {
      Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", (object) gameCell, (object) elementIdx, (object) temperature, (object) mass, (object) element.id, (object) element.defaultValues.temperature);
      temperature = element.defaultValues.temperature;
    }
    // ISSUE: untyped stack allocation
    SimMessages.ModifyCellMessage* modifyCellMessagePtr = (SimMessages.ModifyCellMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyCellMessage))));
    modifyCellMessagePtr->cellIdx = gameCell;
    modifyCellMessagePtr->callbackIdx = callbackIdx;
    modifyCellMessagePtr->temperature = temperature;
    modifyCellMessagePtr->mass = mass;
    modifyCellMessagePtr->elementIdx = (byte) elementIdx;
    modifyCellMessagePtr->replaceType = (byte) replace_type;
    modifyCellMessagePtr->diseaseIdx = disease_idx;
    modifyCellMessagePtr->diseaseCount = disease_count;
    modifyCellMessagePtr->addSubType = !do_vertical_solid_displacement ? (byte) 1 : (byte) 0;
    Sim.SIM_HandleMessage(-1252920804, sizeof (SimMessages.ModifyCellMessage), (byte*) modifyCellMessagePtr);
  }

  public static unsafe void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
  {
    // ISSUE: untyped stack allocation
    SimMessages.CellDiseaseModification* diseaseModificationPtr = (SimMessages.CellDiseaseModification*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.CellDiseaseModification))));
    diseaseModificationPtr->cellIdx = gameCell;
    diseaseModificationPtr->diseaseIdx = disease_idx;
    diseaseModificationPtr->diseaseCount = disease_delta;
    Sim.SIM_HandleMessage(-1853671274, sizeof (SimMessages.CellDiseaseModification), (byte*) diseaseModificationPtr);
  }

  public static int GetElementIndex(SimHashes element)
  {
    int num = -1;
    List<Element> elements = ElementLoader.elements;
    for (int index = 0; index < elements.Count; ++index)
    {
      if (elements[index].id == element)
      {
        num = index;
        break;
      }
    }
    return num;
  }

  public static unsafe void ConsumeMass(
    int gameCell,
    SimHashes element,
    float mass,
    byte radius,
    int callbackIdx = -1)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    int elementIndex = ElementLoader.GetElementIndex(element);
    // ISSUE: untyped stack allocation
    SimMessages.MassConsumptionMessage* consumptionMessagePtr = (SimMessages.MassConsumptionMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.MassConsumptionMessage))));
    consumptionMessagePtr->cellIdx = gameCell;
    consumptionMessagePtr->callbackIdx = callbackIdx;
    consumptionMessagePtr->mass = mass;
    consumptionMessagePtr->elementIdx = (byte) elementIndex;
    consumptionMessagePtr->radius = radius;
    Sim.SIM_HandleMessage(1727657959, sizeof (SimMessages.MassConsumptionMessage), (byte*) consumptionMessagePtr);
  }

  public static unsafe void EmitMass(
    int gameCell,
    byte element_idx,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    int callbackIdx = -1)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.MassEmissionMessage* massEmissionMessagePtr = (SimMessages.MassEmissionMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.MassEmissionMessage))));
    massEmissionMessagePtr->cellIdx = gameCell;
    massEmissionMessagePtr->callbackIdx = callbackIdx;
    massEmissionMessagePtr->mass = mass;
    massEmissionMessagePtr->temperature = temperature;
    massEmissionMessagePtr->elementIdx = element_idx;
    massEmissionMessagePtr->diseaseIdx = disease_idx;
    massEmissionMessagePtr->diseaseCount = disease_count;
    Sim.SIM_HandleMessage(797274363, sizeof (SimMessages.MassEmissionMessage), (byte*) massEmissionMessagePtr);
  }

  public static unsafe void ConsumeDisease(
    int game_cell,
    float percent_to_consume,
    int max_to_consume,
    int callback_idx)
  {
    if (!Grid.IsValidCell(game_cell))
      return;
    // ISSUE: untyped stack allocation
    SimMessages.ConsumeDiseaseMessage* consumeDiseaseMessagePtr = (SimMessages.ConsumeDiseaseMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ConsumeDiseaseMessage))));
    consumeDiseaseMessagePtr->callbackIdx = callback_idx;
    consumeDiseaseMessagePtr->gameCell = game_cell;
    consumeDiseaseMessagePtr->percentToConsume = percent_to_consume;
    consumeDiseaseMessagePtr->maxToConsume = max_to_consume;
    Sim.SIM_HandleMessage(-1019841536, sizeof (SimMessages.ConsumeDiseaseMessage), (byte*) consumeDiseaseMessagePtr);
  }

  public static void AddRemoveSubstance(
    int gameCell,
    SimHashes new_element,
    CellAddRemoveSubstanceEvent ev,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool do_vertical_solid_displacement = true,
    int callbackIdx = -1)
  {
    int elementIndex = SimMessages.GetElementIndex(new_element);
    SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
  }

  public static void AddRemoveSubstance(
    int gameCell,
    int elementIdx,
    CellAddRemoveSubstanceEvent ev,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool do_vertical_solid_displacement = true,
    int callbackIdx = -1)
  {
    if (elementIdx == -1)
      return;
    Element element = ElementLoader.elements[elementIdx];
    float temperature1 = (double) temperature == -1.0 ? element.defaultValues.temperature : temperature;
    SimMessages.ModifyCell(gameCell, elementIdx, temperature1, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
    if (ev == null)
      ;
  }

  public static void ReplaceElement(
    int gameCell,
    SimHashes new_element,
    CellElementEvent ev,
    float mass,
    float temperature = -1f,
    byte diseaseIdx = 255,
    int diseaseCount = 0,
    int callbackIdx = -1)
  {
    int elementIndex = SimMessages.GetElementIndex(new_element);
    if (elementIndex == -1)
      return;
    Element element = ElementLoader.elements[elementIndex];
    float temperature1 = (double) temperature == -1.0 ? element.defaultValues.temperature : temperature;
    SimMessages.ModifyCell(gameCell, elementIndex, temperature1, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
  }

  public static void ReplaceAndDisplaceElement(
    int gameCell,
    SimHashes new_element,
    CellElementEvent ev,
    float mass,
    float temperature = -1f,
    byte disease_idx = 255,
    int disease_count = 0,
    int callbackIdx = -1)
  {
    int elementIndex = SimMessages.GetElementIndex(new_element);
    if (elementIndex == -1)
      return;
    Element element = ElementLoader.elements[elementIndex];
    float temperature1 = (double) temperature == -1.0 ? element.defaultValues.temperature : temperature;
    SimMessages.ModifyCell(gameCell, elementIndex, temperature1, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
  }

  public static unsafe void ModifyEnergy(
    int gameCell,
    float kilojoules,
    float max_temperature,
    SimMessages.EnergySourceID id)
  {
    if (!Grid.IsValidCell(gameCell))
      return;
    if ((double) max_temperature <= 0.0)
    {
      Debug.LogError((object) "invalid max temperature for cell energy modification");
    }
    else
    {
      // ISSUE: untyped stack allocation
      SimMessages.ModifyCellEnergyMessage* cellEnergyMessagePtr = (SimMessages.ModifyCellEnergyMessage*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.ModifyCellEnergyMessage))));
      cellEnergyMessagePtr->cellIdx = gameCell;
      cellEnergyMessagePtr->kilojoules = kilojoules;
      cellEnergyMessagePtr->maxTemperature = max_temperature;
      cellEnergyMessagePtr->id = (int) id;
      Sim.SIM_HandleMessage(818320644, sizeof (SimMessages.ModifyCellEnergyMessage), (byte*) cellEnergyMessagePtr);
    }
  }

  public static void ModifyMass(
    int gameCell,
    float mass,
    byte disease_idx,
    int disease_count,
    CellModifyMassEvent ev,
    float temperature = -1f,
    SimHashes element = SimHashes.Vacuum)
  {
    if (element != SimHashes.Vacuum)
    {
      int elementIndex = SimMessages.GetElementIndex(element);
      if (elementIndex == -1)
        return;
      if ((double) temperature == -1.0)
        temperature = ElementLoader.elements[elementIndex].defaultValues.temperature;
      SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
    }
    else
      SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
  }

  public static unsafe void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
  {
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (SimMessages.ElementInteraction* elementInteractionPtr = &^(interactions == null || interactions.Length == 0 ? (SimMessages.ElementInteraction&) IntPtr.Zero : ref interactions[0]))
    {
      // ISSUE: untyped stack allocation
      SimMessages.CreateElementInteractionsMsg* elementInteractionsMsgPtr = (SimMessages.CreateElementInteractionsMsg*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.CreateElementInteractionsMsg))));
      elementInteractionsMsgPtr->numInteractions = interactions.Length;
      elementInteractionsMsgPtr->interactions = elementInteractionPtr;
      Sim.SIM_HandleMessage(-930289787, sizeof (SimMessages.CreateElementInteractionsMsg), (byte*) elementInteractionsMsgPtr);
    }
  }

  public static unsafe void NewGameFrame(float elapsed_seconds, Vector2I min, Vector2I max)
  {
    min = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, (min.x / 32 - 1) * 32), MathUtil.Clamp(0, Grid.HeightInCells - 1, (min.y / 32 - 1) * 32));
    max = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, ((max.x + 31) / 32 + 1) * 32), MathUtil.Clamp(0, Grid.HeightInCells - 1, ((max.y + 31) / 32 + 1) * 32));
    // ISSUE: untyped stack allocation
    Sim.NewGameFrame* newGameFramePtr = (Sim.NewGameFrame*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (Sim.NewGameFrame))));
    newGameFramePtr->elapsedSeconds = elapsed_seconds;
    newGameFramePtr->minX = min.x;
    newGameFramePtr->minY = min.y;
    newGameFramePtr->maxX = max.x;
    newGameFramePtr->maxY = max.y;
    Sim.SIM_HandleMessage(-775326397, sizeof (Sim.NewGameFrame), (byte*) newGameFramePtr);
  }

  public static unsafe void SetDebugProperties(Sim.DebugProperties properties)
  {
    // ISSUE: untyped stack allocation
    Sim.DebugProperties* debugPropertiesPtr = (Sim.DebugProperties*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (Sim.DebugProperties))));
    *debugPropertiesPtr = properties;
    debugPropertiesPtr->buildingTemperatureScale = properties.buildingTemperatureScale;
    Sim.SIM_HandleMessage(-1683118492, sizeof (Sim.DebugProperties), (byte*) debugPropertiesPtr);
  }

  public static unsafe void ModifyCellWorldZone(int cell, byte zone_id)
  {
    // ISSUE: untyped stack allocation
    SimMessages.CellWorldZoneModification* zoneModificationPtr = (SimMessages.CellWorldZoneModification*) __untypedstackalloc((int) checked (1U * unchecked ((uint) sizeof (SimMessages.CellWorldZoneModification))));
    zoneModificationPtr->cell = cell;
    zoneModificationPtr->zoneID = zone_id;
    Sim.SIM_HandleMessage(-449718014, sizeof (SimMessages.CellWorldZoneModification), (byte*) zoneModificationPtr);
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct AddElementConsumerMessage
  {
    public int cellIdx;
    public int callbackIdx;
    public byte radius;
    public byte configuration;
    public byte elementIdx;
    private byte pad0;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct SetElementConsumerDataMessage
  {
    public int handle;
    public int cell;
    public float consumptionRate;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct RemoveElementConsumerMessage
  {
    public int handle;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct AddElementEmitterMessage
  {
    public float maxPressure;
    public int callbackIdx;
    public int onBlockedCB;
    public int onUnblockedCB;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ModifyElementEmitterMessage
  {
    public int handle;
    public int cellIdx;
    public float emitInterval;
    public float emitMass;
    public float emitTemperature;
    public float maxPressure;
    public int diseaseCount;
    public byte elementIdx;
    public byte maxDepth;
    public byte diseaseIdx;
    private byte pad0;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct RemoveElementEmitterMessage
  {
    public int handle;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct AddElementChunkMessage
  {
    public int gameCell;
    public int callbackIdx;
    public float mass;
    public float temperature;
    public float surfaceArea;
    public float thickness;
    public float groundTransferScale;
    public byte elementIdx;
    public byte pad0;
    public byte pad1;
    public byte pad2;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct RemoveElementChunkMessage
  {
    public int handle;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct SetElementChunkDataMessage
  {
    public int handle;
    public float temperature;
    public float heatCapacity;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct MoveElementChunkMessage
  {
    public int handle;
    public int gameCell;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ModifyElementChunkEnergyMessage
  {
    public int handle;
    public float deltaKJ;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ModifyElementChunkAdjusterMessage
  {
    public int handle;
    public float temperature;
    public float heatCapacity;
    public float thermalConductivity;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct AddBuildingHeatExchangeMessage
  {
    public int callbackIdx;
    public byte elemIdx;
    public byte pad0;
    public byte pad1;
    public byte pad2;
    public float mass;
    public float temperature;
    public float thermalConductivity;
    public float overheatTemperature;
    public float operatingKilowatts;
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ModifyBuildingHeatExchangeMessage
  {
    public int callbackIdx;
    public byte elemIdx;
    public byte pad0;
    public byte pad1;
    public byte pad2;
    public float mass;
    public float temperature;
    public float thermalConductivity;
    public float overheatTemperature;
    public float operatingKilowatts;
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ModifyBuildingEnergyMessage
  {
    public int handle;
    public float deltaKJ;
    public float minTemperature;
    public float maxTemperature;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct RemoveBuildingHeatExchangeMessage
  {
    public int handle;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct AddDiseaseEmitterMessage
  {
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ModifyDiseaseEmitterMessage
  {
    public int handle;
    public int gameCell;
    public byte diseaseIdx;
    public byte maxDepth;
    private byte pad0;
    private byte pad1;
    public float emitInterval;
    public int emitCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct RemoveDiseaseEmitterMessage
  {
    public int handle;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct SetSavedOptionsMessage
  {
    public byte clearBits;
    public byte setBits;
  }

  public enum SimSavedOptions : byte
  {
    ENABLE_DIAGONAL_FALLING_SAND = 1,
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct DigMessage
  {
    public int cellIdx;
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct SetCellFloatValueMessage
  {
    public int cellIdx;
    public float value;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct CellPropertiesMessage
  {
    public int cellIdx;
    public byte properties;
    public byte set;
    public byte pad0;
    public byte pad1;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct SetInsulationValueMessage
  {
    public int cellIdx;
    public float value;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ModifyCellMessage
  {
    public int cellIdx;
    public int callbackIdx;
    public float temperature;
    public float mass;
    public int diseaseCount;
    public byte elementIdx;
    public byte replaceType;
    public byte diseaseIdx;
    public byte addSubType;
  }

  public enum ReplaceType
  {
    None,
    Replace,
    ReplaceAndDisplace,
  }

  private enum AddSolidMassSubType
  {
    DoVerticalDisplacement,
    OnlyIfSameElement,
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct CellDiseaseModification
  {
    public int cellIdx;
    public byte diseaseIdx;
    public byte pad0;
    public byte pad1;
    public byte pad2;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct MassConsumptionMessage
  {
    public int cellIdx;
    public int callbackIdx;
    public float mass;
    public byte elementIdx;
    public byte radius;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct MassEmissionMessage
  {
    public int cellIdx;
    public int callbackIdx;
    public float mass;
    public float temperature;
    public int diseaseCount;
    public byte elementIdx;
    public byte diseaseIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ConsumeDiseaseMessage
  {
    public int gameCell;
    public int callbackIdx;
    public float percentToConsume;
    public int maxToConsume;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct ModifyCellEnergyMessage
  {
    public int cellIdx;
    public float kilojoules;
    public float maxTemperature;
    public int id;
  }

  public enum EnergySourceID
  {
    DebugHeat = 1000, // 0x000003E8
    DebugCool = 1001, // 0x000003E9
    FierySkin = 1002, // 0x000003EA
    Overheatable = 1003, // 0x000003EB
    LiquidCooledFan = 1004, // 0x000003EC
    ConduitTemperatureManager = 1005, // 0x000003ED
    Excavator = 1006, // 0x000003EE
    HeatBulb = 1007, // 0x000003EF
    WarmBlooded = 1008, // 0x000003F0
    StructureTemperature = 1009, // 0x000003F1
    Burner = 1010, // 0x000003F2
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct VisibleCells
  {
    public Vector2I min;
    public Vector2I max;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct WakeCellMessage
  {
    public int gameCell;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ElementInteraction
  {
    public uint interactionType;
    public byte elemIdx1;
    public byte elemIdx2;
    public byte elemResultIdx;
    public byte pad;
    public float minMass;
    public float interactionProbability;
    public float elem1MassDestructionPercent;
    public float elem2MassRequiredMultiplier;
    public float elemResultMassCreationMultiplier;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  private struct CreateElementInteractionsMsg
  {
    public int numInteractions;
    public unsafe SimMessages.ElementInteraction* interactions;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct PipeChange
  {
    public int cell;
    public byte layer;
    public byte pad0;
    public byte pad1;
    public byte pad2;
    public float mass;
    public float temperature;
    public int elementHash;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct CellWorldZoneModification
  {
    public int cell;
    public byte zoneID;
  }
}
