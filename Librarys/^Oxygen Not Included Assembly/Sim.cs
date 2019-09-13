// Decompiled with JetBrains decompiler
// Type: Sim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Sim
{
  public const int InvalidHandle = -1;
  public const int QueuedRegisterHandle = -2;
  public const byte InvalidDiseaseIdx = 255;
  public const byte InvalidElementIdx = 255;
  public const byte SpaceZoneID = 255;
  public const byte SolidZoneID = 0;
  public const int ChunkEdgeSize = 32;
  public const float StateTransitionEnergy = 3f;
  public const float ZeroDegreesCentigrade = 273.15f;
  public const float StandardTemperature = 293.15f;
  public const float StandardPressure = 101.3f;
  public const float Epsilon = 0.0001f;
  public const float MaxTemperature = 10000f;
  public const float MinTemperature = 0.0f;
  public const float MaxMass = 10000f;
  public const float MinMass = 1.0001f;
  private const int PressureUpdateInterval = 1;
  private const int TemperatureUpdateInterval = 1;
  private const int LiquidUpdateInterval = 1;
  private const int LifeUpdateInterval = 1;
  public const byte ClearSkyGridValue = 253;
  public const int PACKING_ALIGNMENT = 4;

  public static bool IsValidHandle(int h)
  {
    if (h != -1)
      return h != -2;
    return false;
  }

  public static int GetHandleIndex(int h)
  {
    return h & 16777215;
  }

  [DllImport("SimDLL")]
  public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

  [DllImport("SimDLL")]
  public static extern void SIM_Shutdown();

  [DllImport("SimDLL")]
  public static extern unsafe IntPtr SIM_HandleMessage(
    int sim_msg_id,
    int msg_length,
    byte* msg);

  [DllImport("SimDLL")]
  private static extern unsafe byte* SIM_BeginSave(int* size);

  [DllImport("SimDLL")]
  private static extern void SIM_EndSave();

  [DllImport("SimDLL")]
  public static extern void SIM_DebugCrash();

  public static unsafe IntPtr HandleMessage(
    SimMessageHashes sim_msg_id,
    int msg_length,
    byte[] msg)
  {
    IntPtr num;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg1 = &^(msg == null || msg.Length == 0 ? (byte&) IntPtr.Zero : ref msg[0]))
      num = Sim.SIM_HandleMessage((int) sim_msg_id, msg_length, msg1);
    return num;
  }

  public static unsafe void Save(BinaryWriter writer)
  {
    int length;
    byte* numPtr = Sim.SIM_BeginSave(&length);
    byte[] numArray = new byte[length];
    Marshal.Copy((IntPtr) ((void*) numPtr), numArray, 0, length);
    Sim.SIM_EndSave();
    writer.Write(length);
    writer.Write(numArray);
  }

  public static unsafe int Load(FastReader reader)
  {
    int num1 = reader.ReadInt32();
    byte[] numArray = reader.ReadBytes(num1);
    IntPtr num2;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg = &^(numArray == null || numArray.Length == 0 ? (byte&) IntPtr.Zero : ref numArray[0]))
      num2 = Sim.SIM_HandleMessage(-672538170, num1, msg);
    if (num2 == IntPtr.Zero)
      return -1;
    Sim.GameDataUpdate* gameDataUpdatePtr = (Sim.GameDataUpdate*) (void*) num2;
    Grid.elementIdx = gameDataUpdatePtr->elementIdx;
    Grid.temperature = gameDataUpdatePtr->temperature;
    Grid.mass = gameDataUpdatePtr->mass;
    Grid.properties = gameDataUpdatePtr->properties;
    Grid.strengthInfo = gameDataUpdatePtr->strengthInfo;
    Grid.insulation = gameDataUpdatePtr->insulation;
    Grid.diseaseIdx = gameDataUpdatePtr->diseaseIdx;
    Grid.diseaseCount = gameDataUpdatePtr->diseaseCount;
    Grid.AccumulatedFlowValues = gameDataUpdatePtr->accumulatedFlow;
    PropertyTextures.externalFlowTex = gameDataUpdatePtr->propertyTextureFlow;
    PropertyTextures.externalLiquidTex = gameDataUpdatePtr->propertyTextureLiquid;
    PropertyTextures.externalExposedToSunlight = gameDataUpdatePtr->propertyTextureExposedToSunlight;
    Grid.InitializeCells();
    return 0;
  }

  public static unsafe void Shutdown()
  {
    Sim.SIM_Shutdown();
    Grid.mass = (float*) null;
  }

  [DllImport("SimDLL")]
  public static extern unsafe char* SYSINFO_Acquire();

  [DllImport("SimDLL")]
  public static extern void SYSINFO_Release();

  public static unsafe int DLL_MessageHandler(int message_id, IntPtr data)
  {
    switch ((Sim.GameHandledMessages) message_id)
    {
      case Sim.GameHandledMessages.ExceptionHandler:
        Sim.DLLExceptionHandlerMessage* exceptionHandlerMessagePtr = (Sim.DLLExceptionHandlerMessage*) (void*) data;
        KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", Marshal.PtrToStringAnsi(exceptionHandlerMessagePtr->callstack), Marshal.PtrToStringAnsi(exceptionHandlerMessagePtr->dmpFilename));
        return 0;
      case Sim.GameHandledMessages.ReportMessage:
        Sim.DLLReportMessageMessage* reportMessageMessagePtr = (Sim.DLLReportMessageMessage*) (void*) data;
        KCrashReporter.ReportSimDLLCrash("SimMessage: " + Marshal.PtrToStringAnsi(reportMessageMessagePtr->message), !(reportMessageMessagePtr->callstack != IntPtr.Zero) ? Marshal.PtrToStringAnsi(reportMessageMessagePtr->file) + ":" + (object) reportMessageMessagePtr->line : Marshal.PtrToStringAnsi(reportMessageMessagePtr->callstack), (string) null);
        return 0;
      default:
        return -1;
    }
  }

  public delegate int GAME_MessageHandler(int message_id, IntPtr data);

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DLLExceptionHandlerMessage
  {
    public IntPtr callstack;
    public IntPtr dmpFilename;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DLLReportMessageMessage
  {
    public IntPtr callstack;
    public IntPtr message;
    public IntPtr file;
    public int line;
  }

  private enum GameHandledMessages
  {
    ExceptionHandler,
    ReportMessage,
  }

  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct PhysicsData
  {
    public float temperature;
    public float mass;
    public float pressure;

    public void Write(BinaryWriter writer)
    {
      writer.Write(this.temperature);
      writer.Write(this.mass);
      writer.Write(this.pressure);
    }
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct Cell
  {
    public byte elementIdx;
    public byte properties;
    public byte insulation;
    public byte strengthInfo;
    public float temperature;
    public float mass;

    public void Write(BinaryWriter writer)
    {
      writer.Write(this.elementIdx);
      writer.Write((byte) 0);
      writer.Write(this.insulation);
      writer.Write((byte) 0);
      writer.Write(this.temperature);
      writer.Write(this.mass);
    }

    public void SetValues(global::Element elem, List<global::Element> elements)
    {
      this.SetValues(elem, elem.defaultValues, elements);
    }

    public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
    {
      this.elementIdx = (byte) elements.IndexOf(elem);
      this.temperature = pd.temperature;
      this.mass = pd.mass;
      this.insulation = byte.MaxValue;
    }

    public void SetValues(byte new_elem_idx, float new_temperature, float new_mass)
    {
      this.elementIdx = new_elem_idx;
      this.temperature = new_temperature;
      this.mass = new_mass;
      this.insulation = byte.MaxValue;
    }

    public enum Properties
    {
      GasImpermeable = 1,
      LiquidImpermeable = 2,
      SolidImpermeable = 4,
      Unbreakable = 8,
      Transparent = 16, // 0x00000010
      Opaque = 32, // 0x00000020
      NotifyOnMelt = 64, // 0x00000040
    }
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct Element
  {
    public SimHashes id;
    public byte state;
    public byte lowTempTransitionIdx;
    public byte highTempTransitionIdx;
    public byte elementsTableIdx;
    public float specificHeatCapacity;
    public float thermalConductivity;
    public float molarMass;
    public float solidSurfaceAreaMultiplier;
    public float liquidSurfaceAreaMultiplier;
    public float gasSurfaceAreaMultiplier;
    public float flow;
    public float viscosity;
    public float minHorizontalFlow;
    public float minVerticalFlow;
    public float maxMass;
    public float lowTemp;
    public float highTemp;
    public float strength;
    public SimHashes lowTempTransitionOreID;
    public float lowTempTransitionOreMassConversion;
    public SimHashes highTempTransitionOreID;
    public float highTempTransitionOreMassConversion;
    public sbyte sublimateIndex;
    public sbyte convertIndex;
    public byte pack0;
    public byte pack1;
    public uint colour;
    public SpawnFXHashes sublimateFX;
    public float lightAbsorptionFactor;
    public Sim.PhysicsData defaultValues;

    public Element(global::Element e, List<global::Element> elements)
    {
      this.id = e.id;
      this.state = (byte) e.state;
      if (e.HasTag(GameTags.Unstable))
        this.state |= (byte) 8;
      int index1 = elements.FindIndex((Predicate<global::Element>) (ele => ele.id == e.lowTempTransitionTarget));
      int index2 = elements.FindIndex((Predicate<global::Element>) (ele => ele.id == e.highTempTransitionTarget));
      this.lowTempTransitionIdx = index1 < 0 ? byte.MaxValue : (byte) index1;
      this.highTempTransitionIdx = index2 < 0 ? byte.MaxValue : (byte) index2;
      this.elementsTableIdx = (byte) elements.IndexOf(e);
      this.specificHeatCapacity = e.specificHeatCapacity;
      this.thermalConductivity = e.thermalConductivity;
      this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
      this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
      this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
      this.molarMass = e.molarMass;
      this.strength = e.strength;
      this.flow = e.flow;
      this.viscosity = e.viscosity;
      this.minHorizontalFlow = e.minHorizontalFlow;
      this.minVerticalFlow = e.minVerticalFlow;
      this.maxMass = e.maxMass;
      this.lowTemp = e.lowTemp;
      this.highTemp = e.highTemp;
      this.highTempTransitionOreID = e.highTempTransitionOreID;
      this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
      this.lowTempTransitionOreID = e.lowTempTransitionOreID;
      this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
      this.sublimateIndex = (sbyte) elements.FindIndex((Predicate<global::Element>) (ele => ele.id == e.sublimateId));
      this.convertIndex = (sbyte) elements.FindIndex((Predicate<global::Element>) (ele => ele.id == e.convertId));
      this.pack0 = (byte) 0;
      this.pack1 = (byte) 0;
      if (e.substance == null)
      {
        this.colour = 0U;
      }
      else
      {
        Color32 colour = e.substance.colour;
        this.colour = (uint) ((int) colour.a << 24 | (int) colour.b << 16 | (int) colour.g << 8) | (uint) colour.r;
      }
      this.sublimateFX = e.sublimateFX;
      this.lightAbsorptionFactor = e.lightAbsorptionFactor;
      this.defaultValues = e.defaultValues;
    }

    public void Write(BinaryWriter writer)
    {
      writer.Write((int) this.id);
      writer.Write(this.state);
      writer.Write((sbyte) this.lowTempTransitionIdx);
      writer.Write((sbyte) this.highTempTransitionIdx);
      writer.Write(this.elementsTableIdx);
      writer.Write(this.specificHeatCapacity);
      writer.Write(this.thermalConductivity);
      writer.Write(this.molarMass);
      writer.Write(this.solidSurfaceAreaMultiplier);
      writer.Write(this.liquidSurfaceAreaMultiplier);
      writer.Write(this.gasSurfaceAreaMultiplier);
      writer.Write(this.flow);
      writer.Write(this.viscosity);
      writer.Write(this.minHorizontalFlow);
      writer.Write(this.minVerticalFlow);
      writer.Write(this.maxMass);
      writer.Write(this.lowTemp);
      writer.Write(this.highTemp);
      writer.Write(this.strength);
      writer.Write((int) this.lowTempTransitionOreID);
      writer.Write(this.lowTempTransitionOreMassConversion);
      writer.Write((int) this.highTempTransitionOreID);
      writer.Write(this.highTempTransitionOreMassConversion);
      writer.Write(this.sublimateIndex);
      writer.Write(this.convertIndex);
      writer.Write(this.pack0);
      writer.Write(this.pack1);
      writer.Write(this.colour);
      writer.Write((int) this.sublimateFX);
      writer.Write(this.lightAbsorptionFactor);
      this.defaultValues.Write(writer);
    }
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DiseaseCell
  {
    public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell()
    {
      diseaseIdx = byte.MaxValue,
      elementCount = 0
    };
    public byte diseaseIdx;
    private byte reservedInfestationTickCount;
    private byte pad1;
    private byte pad2;
    public int elementCount;
    private float reservedAccumulatedError;

    public void Write(BinaryWriter writer)
    {
      writer.Write(this.diseaseIdx);
      writer.Write(this.reservedInfestationTickCount);
      writer.Write(this.pad1);
      writer.Write(this.pad2);
      writer.Write(this.elementCount);
      writer.Write(this.reservedAccumulatedError);
    }
  }

  public delegate void GAME_Callback();

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SolidInfo
  {
    public int cellIdx;
    public int isSolid;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct LiquidChangeInfo
  {
    public int cellIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SolidSubstanceChangeInfo
  {
    public int cellIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SubstanceChangeInfo
  {
    public int cellIdx;
    public byte oldElemIdx;
    public byte newElemIdx;
    private byte pad0;
    private byte pad1;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct CallbackInfo
  {
    public int callbackIdx;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct GameDataUpdate
  {
    public int numFramesProcessed;
    public unsafe byte* elementIdx;
    public unsafe float* temperature;
    public unsafe float* mass;
    public unsafe byte* properties;
    public unsafe byte* insulation;
    public unsafe byte* strengthInfo;
    public unsafe byte* diseaseIdx;
    public unsafe int* diseaseCount;
    public int numSolidInfo;
    public unsafe Sim.SolidInfo* solidInfo;
    public int numLiquidChangeInfo;
    public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;
    public int numSolidSubstanceChangeInfo;
    public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;
    public int numSubstanceChangeInfo;
    public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;
    public int numCallbackInfo;
    public unsafe Sim.CallbackInfo* callbackInfo;
    public int numSpawnFallingLiquidInfo;
    public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;
    public int numDigInfo;
    public unsafe Sim.SpawnOreInfo* digInfo;
    public int numSpawnOreInfo;
    public unsafe Sim.SpawnOreInfo* spawnOreInfo;
    public int numSpawnFXInfo;
    public unsafe Sim.SpawnFXInfo* spawnFXInfo;
    public int numUnstableCellInfo;
    public unsafe Sim.UnstableCellInfo* unstableCellInfo;
    public int numWorldDamageInfo;
    public unsafe Sim.WorldDamageInfo* worldDamageInfo;
    public int numBuildingTemperatures;
    public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;
    public int numMassConsumedCallbacks;
    public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;
    public int numMassEmittedCallbacks;
    public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;
    public int numDiseaseConsumptionCallbacks;
    public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;
    public int numComponentStateChangedMessages;
    public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;
    public int numRemovedMassEntries;
    public unsafe Sim.ConsumedMassInfo* removedMassEntries;
    public int numEmittedMassEntries;
    public unsafe Sim.EmittedMassInfo* emittedMassEntries;
    public int numElementChunkInfos;
    public unsafe Sim.ElementChunkInfo* elementChunkInfos;
    public int numElementChunkMeltedInfos;
    public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;
    public int numBuildingOverheatInfos;
    public unsafe Sim.MeltedInfo* buildingOverheatInfos;
    public int numBuildingNoLongerOverheatedInfos;
    public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;
    public int numBuildingMeltedInfos;
    public unsafe Sim.MeltedInfo* buildingMeltedInfos;
    public int numCellMeltedInfos;
    public unsafe Sim.CellMeltedInfo* cellMeltedInfos;
    public int numDiseaseEmittedInfos;
    public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;
    public int numDiseaseConsumedInfos;
    public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
    public unsafe float* accumulatedFlow;
    public IntPtr propertyTextureFlow;
    public IntPtr propertyTextureLiquid;
    public IntPtr propertyTextureExposedToSunlight;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SpawnFallingLiquidInfo
  {
    public int cellIdx;
    public byte elemIdx;
    public byte diseaseIdx;
    public byte pad0;
    public byte pad1;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SpawnOreInfo
  {
    public int cellIdx;
    public byte elemIdx;
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SpawnFXInfo
  {
    public int cellIdx;
    public int fxHash;
    public float rotation;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct UnstableCellInfo
  {
    public int cellIdx;
    public byte fallingInfo;
    public byte elemIdx;
    public byte diseaseIdx;
    private byte pad0;
    public float mass;
    public float temperature;
    public int diseaseCount;

    public enum FallingInfo
    {
      StartedFalling,
      StoppedFalling,
    }
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct NewGameFrame
  {
    public float elapsedSeconds;
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct WorldDamageInfo
  {
    public int gameCell;
    public int damageSourceOffset;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct PipeTemperatureChange
  {
    public int cellIdx;
    public float temperature;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MassConsumedCallback
  {
    public int callbackIdx;
    public byte elemIdx;
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MassEmittedCallback
  {
    public int callbackIdx;
    public byte suceeded;
    public byte elemIdx;
    public byte diseaseIdx;
    private byte pad0;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DiseaseConsumptionCallback
  {
    public int callbackIdx;
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    private byte pad2;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ComponentStateChangedMessage
  {
    public int callbackIdx;
    public int simHandle;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DebugProperties
  {
    public float buildingTemperatureScale;
    public float contaminatedOxygenEmitProbability;
    public float contaminatedOxygenConversionPercent;
    public float biomeTemperatureLerpRate;
    public byte isDebugEditing;
    public byte pad0;
    public byte pad1;
    public byte pad2;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct EmittedMassInfo
  {
    public byte elemIdx;
    public byte diseaseIdx;
    public byte pad0;
    public byte pad1;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ConsumedMassInfo
  {
    public int simHandle;
    public byte removedElemIdx;
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    public float mass;
    public float temperature;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ConsumedDiseaseInfo
  {
    public int simHandle;
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    private byte pad2;
    public int diseaseCount;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ElementChunkInfo
  {
    public float temperature;
    public float deltaKJ;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MeltedInfo
  {
    public int handle;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct CellMeltedInfo
  {
    public int gameCell;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct BuildingTemperatureInfo
  {
    public int handle;
    public float temperature;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct BuildingConductivityData
  {
    public float temperature;
    public float heatCapacity;
    public float thermalConductivity;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DiseaseEmittedInfo
  {
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    private byte pad2;
    public int count;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct DiseaseConsumedInfo
  {
    public byte diseaseIdx;
    private byte pad0;
    private byte pad1;
    private byte pad2;
    public int count;
  }
}
