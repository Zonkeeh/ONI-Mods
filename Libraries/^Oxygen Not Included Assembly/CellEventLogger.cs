// Decompiled with JetBrains decompiler
// Type: CellEventLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;

public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
  public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
  public static CellEventLogger Instance;
  public CellSolidEvent SimMessagesSolid;
  public CellSolidEvent SimCellOccupierDestroy;
  public CellSolidEvent SimCellOccupierForceSolid;
  public CellSolidEvent SimCellOccupierSolidChanged;
  public CellElementEvent DoorOpen;
  public CellElementEvent DoorClose;
  public CellElementEvent Excavator;
  public CellElementEvent DebugTool;
  public CellElementEvent SandBoxTool;
  public CellElementEvent TemplateLoader;
  public CellElementEvent Scenario;
  public CellElementEvent SimCellOccupierOnSpawn;
  public CellElementEvent SimCellOccupierDestroySelf;
  public CellElementEvent WorldGapManager;
  public CellElementEvent ReceiveElementChanged;
  public CellElementEvent ObjectSetSimOnSpawn;
  public CellElementEvent DecompositionDirtyWater;
  public CellCallbackEvent SendCallback;
  public CellCallbackEvent ReceiveCallback;
  public CellDigEvent Dig;
  public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;
  public CellAddRemoveSubstanceEvent SublimatesEmit;
  public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;
  public CellAddRemoveSubstanceEvent LiquidChunkOnStore;
  public CellAddRemoveSubstanceEvent FallingWaterAddToSim;
  public CellAddRemoveSubstanceEvent ExploderOnSpawn;
  public CellAddRemoveSubstanceEvent ExhaustSimUpdate;
  public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;
  public CellAddRemoveSubstanceEvent ElementChunkTransition;
  public CellAddRemoveSubstanceEvent OxyrockEmit;
  public CellAddRemoveSubstanceEvent BleachstoneEmit;
  public CellAddRemoveSubstanceEvent UnstableGround;
  public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;
  public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;
  public CellAddRemoveSubstanceEvent OverheatableMeltingDown;
  public CellAddRemoveSubstanceEvent FabricatorProduceMelted;
  public CellAddRemoveSubstanceEvent PumpSimUpdate;
  public CellAddRemoveSubstanceEvent WallPumpSimUpdate;
  public CellAddRemoveSubstanceEvent Vomit;
  public CellAddRemoveSubstanceEvent Tears;
  public CellAddRemoveSubstanceEvent Pee;
  public CellAddRemoveSubstanceEvent AlgaeHabitat;
  public CellAddRemoveSubstanceEvent CO2FilterOxygen;
  public CellAddRemoveSubstanceEvent ToiletEmit;
  public CellAddRemoveSubstanceEvent ElementEmitted;
  public CellAddRemoveSubstanceEvent Mop;
  public CellAddRemoveSubstanceEvent OreMelted;
  public CellAddRemoveSubstanceEvent ConstructTile;
  public CellAddRemoveSubstanceEvent Dumpable;
  public CellAddRemoveSubstanceEvent Cough;
  public CellAddRemoveSubstanceEvent Meteor;
  public CellModifyMassEvent CO2ManagerFixedUpdate;
  public CellModifyMassEvent EnvironmentConsumerFixedUpdate;
  public CellModifyMassEvent ExcavatorShockwave;
  public CellModifyMassEvent OxygenBreatherSimUpdate;
  public CellModifyMassEvent CO2ScrubberSimUpdate;
  public CellModifyMassEvent RiverSourceSimUpdate;
  public CellModifyMassEvent RiverTerminusSimUpdate;
  public CellModifyMassEvent DebugToolModifyMass;
  public CellModifyMassEvent EnergyGeneratorModifyMass;
  public CellSolidFilterEvent SolidFilterEvent;

  public static void DestroyInstance()
  {
    CellEventLogger.Instance = (CellEventLogger) null;
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void LogCallbackSend(int cell, int callback_id)
  {
    if (callback_id == -1)
      return;
    this.CallbackToCellMap[callback_id] = cell;
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void LogCallbackReceive(int callback_id)
  {
    int invalidCell = Grid.InvalidCell;
    if (!this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell))
      ;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    CellEventLogger.Instance = this;
    this.SimMessagesSolid = this.AddEvent((CellEvent) new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent;
    this.SimCellOccupierDestroy = this.AddEvent((CellEvent) new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent;
    this.SimCellOccupierForceSolid = this.AddEvent((CellEvent) new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent;
    this.SimCellOccupierSolidChanged = this.AddEvent((CellEvent) new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent;
    this.DoorOpen = this.AddEvent((CellEvent) new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent;
    this.DoorClose = this.AddEvent((CellEvent) new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent;
    this.Excavator = this.AddEvent((CellEvent) new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent;
    this.DebugTool = this.AddEvent((CellEvent) new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent;
    this.SandBoxTool = this.AddEvent((CellEvent) new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent;
    this.TemplateLoader = this.AddEvent((CellEvent) new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent;
    this.Scenario = this.AddEvent((CellEvent) new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent;
    this.SimCellOccupierOnSpawn = this.AddEvent((CellEvent) new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent;
    this.SimCellOccupierDestroySelf = this.AddEvent((CellEvent) new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent;
    this.WorldGapManager = this.AddEvent((CellEvent) new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent;
    this.ReceiveElementChanged = this.AddEvent((CellEvent) new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent;
    this.ObjectSetSimOnSpawn = this.AddEvent((CellEvent) new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent;
    this.DecompositionDirtyWater = this.AddEvent((CellEvent) new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent;
    this.SendCallback = this.AddEvent((CellEvent) new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent;
    this.ReceiveCallback = this.AddEvent((CellEvent) new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent;
    this.Dig = this.AddEvent((CellEvent) new CellDigEvent(true)) as CellDigEvent;
    this.WorldDamageDelayedSpawnFX = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent;
    this.OxygenModifierSimUpdate = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent;
    this.LiquidChunkOnStore = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent;
    this.FallingWaterAddToSim = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent;
    this.ExploderOnSpawn = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent;
    this.ExhaustSimUpdate = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent;
    this.ElementConsumerSimUpdate = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent;
    this.SublimatesEmit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent;
    this.Mop = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent;
    this.OreMelted = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent;
    this.ConstructTile = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent;
    this.Dumpable = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent;
    this.Cough = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent;
    this.Meteor = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent;
    this.ElementChunkTransition = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent;
    this.OxyrockEmit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent;
    this.BleachstoneEmit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent;
    this.UnstableGround = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent;
    this.ConduitFlowEmptyConduit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent;
    this.ConduitConsumerWrongElement = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent;
    this.OverheatableMeltingDown = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent;
    this.FabricatorProduceMelted = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent;
    this.PumpSimUpdate = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent;
    this.WallPumpSimUpdate = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent;
    this.Vomit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent;
    this.Tears = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent;
    this.Pee = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent;
    this.AlgaeHabitat = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent;
    this.CO2FilterOxygen = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent;
    this.ToiletEmit = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent;
    this.ElementEmitted = this.AddEvent((CellEvent) new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent;
    this.CO2ManagerFixedUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent;
    this.EnvironmentConsumerFixedUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent;
    this.ExcavatorShockwave = this.AddEvent((CellEvent) new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent;
    this.OxygenBreatherSimUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent;
    this.CO2ScrubberSimUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent;
    this.RiverSourceSimUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent;
    this.RiverTerminusSimUpdate = this.AddEvent((CellEvent) new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent;
    this.DebugToolModifyMass = this.AddEvent((CellEvent) new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent;
    this.EnergyGeneratorModifyMass = this.AddEvent((CellEvent) new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent;
    this.SolidFilterEvent = this.AddEvent((CellEvent) new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent;
  }
}
