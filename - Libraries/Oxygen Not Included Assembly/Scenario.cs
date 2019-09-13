// Decompiled with JetBrains decompiler
// Type: Scenario
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Scenario : KMonoBehaviour
{
  public bool PropaneGeneratorTest = true;
  public bool HatchTest = true;
  public bool DoorTest = true;
  public bool AirLockTest = true;
  public bool BedTest = true;
  public bool SuitTest = true;
  public bool SuitRechargeTest = true;
  public bool FabricatorTest = true;
  public bool ElectrolyzerTest = true;
  public bool HexapedTest = true;
  public bool FallTest = true;
  public bool FeedingTest = true;
  public bool OrePerformanceTest = true;
  public bool TwoKelvinsOneSuitTest = true;
  public bool LiquifierTest = true;
  public bool RockCrusherTest = true;
  public bool CementMixerTest = true;
  public bool KilnTest = true;
  public bool ClearExistingScene = true;
  private int Bot;
  private int Left;
  public int RootCell;
  public static Scenario Instance;

  public bool[] ReplaceElementMask { get; set; }

  public static void DestroyInstance()
  {
    Scenario.Instance = (Scenario) null;
  }

  protected override void OnPrefabInit()
  {
    Scenario.Instance = this;
    SaveLoader.Instance.OnWorldGenComplete += new System.Action(this.OnWorldGenComplete);
  }

  private void OnWorldGenComplete()
  {
    this.Init();
  }

  private void Init()
  {
    this.Bot = Grid.HeightInCells / 4;
    this.Left = 150;
    this.RootCell = Grid.OffsetCell(0, this.Left, this.Bot);
    this.ReplaceElementMask = new bool[Grid.CellCount];
  }

  private void DigHole(int x, int y, int width, int height)
  {
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + index1, y + index2), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 200f, -1f, byte.MaxValue, 0, -1);
        SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + index2), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
        SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + width, y + index2), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
      }
      SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + index1, y - 1), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
      SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + index1, y + height), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
    }
  }

  private void Fill(int x, int y, SimHashes id = SimHashes.Ice)
  {
    SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), id, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
  }

  private void PlaceColumn(int x, int y, int height)
  {
    for (int index = 0; index < height; ++index)
      SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + index), SimHashes.Ice, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
  }

  private void PlaceTileX(int left, int bot, int amount)
  {
    for (int index = 0; index < amount; ++index)
      this.PlaceBuilding(left + index, bot, "Tile", SimHashes.Cuprite);
  }

  private void PlaceTileY(int left, int bot, int amount)
  {
    for (int index = 0; index < amount; ++index)
      this.PlaceBuilding(left, bot + index, "Tile", SimHashes.Cuprite);
  }

  private void Clear(int x, int y)
  {
    SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
  }

  private void PlacerLadder(int x, int y, int amount)
  {
    int num = 1;
    if (amount < 0)
    {
      amount = -amount;
      num = -1;
    }
    for (int index = 0; index < amount; ++index)
      this.PlaceBuilding(x, y + index * num, "Ladder", SimHashes.Cuprite);
  }

  private void PlaceBuildings(int left, int bot)
  {
    this.PlaceBuilding(++left, bot, "ManualGenerator", SimHashes.Iron);
    this.PlaceBuilding(left += 2, bot, "OxygenMachine", SimHashes.Steel);
    this.PlaceBuilding(left += 2, bot, "SpaceHeater", SimHashes.Steel);
    this.PlaceBuilding(++left, bot, "Electrolyzer", SimHashes.Steel);
    this.PlaceBuilding(++left, bot, "Smelter", SimHashes.Steel);
    this.SpawnOre(left, bot + 1, SimHashes.Ice);
  }

  [DebuggerHidden]
  private IEnumerator TurnOn(GameObject go)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Scenario.\u003CTurnOn\u003Ec__Iterator0()
    {
      go = go
    };
  }

  private void SetupPlacerTest(Scenario.Builder b, Element element)
  {
    foreach (BuildingDef buildingDef in Assets.BuildingDefs)
    {
      if (buildingDef.Name != "Excavator")
        b.Placer(buildingDef.PrefabID, element);
    }
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  private void SetupBuildingTest(
    Scenario.RowLayout row_layout,
    bool is_powered,
    bool break_building)
  {
    Scenario.Builder builder = (Scenario.Builder) null;
    int num = 0;
    foreach (BuildingDef buildingDef in Assets.BuildingDefs)
    {
      if (builder == null)
      {
        builder = row_layout.NextRow();
        num = this.Left;
        if (is_powered)
        {
          builder.Minion((System.Action<GameObject>) null);
          builder.Minion((System.Action<GameObject>) null);
        }
      }
      if (buildingDef.Name != "Excavator")
      {
        GameObject gameObject = builder.Building(buildingDef.PrefabID);
        if (break_building)
        {
          BuildingHP component = gameObject.GetComponent<BuildingHP>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.DoDamage(int.MaxValue);
        }
      }
      if (builder.Left > num + 100)
      {
        builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
        builder = (Scenario.Builder) null;
      }
    }
    builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  [DebuggerHidden]
  private IEnumerator RunAfterNextUpdateRoutine(System.Action action)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Scenario.\u003CRunAfterNextUpdateRoutine\u003Ec__Iterator1()
    {
      action = action
    };
  }

  private void RunAfterNextUpdate(System.Action action)
  {
    this.StartCoroutine(this.RunAfterNextUpdateRoutine(action));
  }

  public void SetupFabricatorTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Building("ManualGenerator");
    b.Ore(3, SimHashes.Cuprite);
    b.Minion((System.Action<GameObject>) null);
    b.Building("Masonry");
    b.InAndOuts();
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupDoorTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Jump(1, 0);
    b.Building("Door");
    b.Building("ManualGenerator");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupHatchTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Building("Door");
    b.Building("ManualGenerator");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupPropaneGeneratorTest(Scenario.Builder b)
  {
    b.Building("PropaneGenerator");
    b.Building("OxygenMachine");
    b.FinalizeRoom(SimHashes.Propane, SimHashes.Steel);
  }

  public void SetupAirLockTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Jump(1, 0);
    b.Minion((System.Action<GameObject>) null);
    b.Jump(1, 0);
    b.Building("PoweredAirlock");
    b.Building("ManualGenerator");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupBedTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) (go => go.GetAmounts().SetValue("Stamina", 10f)));
    b.Building("ManualGenerator");
    b.Minion((System.Action<GameObject>) (go => go.GetAmounts().SetValue("Stamina", 10f)));
    b.Building("ComfyBed");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupHexapedTest(Scenario.Builder b)
  {
    b.Fill(4, 4, SimHashes.Oxygen);
    b.Prefab("Hexaped", (System.Action<GameObject>) null);
    b.Jump(2, 0);
    b.Ore(1, SimHashes.Iron);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupElectrolyzerTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Building("ManualGenerator");
    b.Ore(3, SimHashes.Ice);
    b.Minion((System.Action<GameObject>) null);
    b.Building("Electrolyzer");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupOrePerformanceTest(Scenario.Builder b)
  {
    int num1 = 20;
    int num2 = 20;
    int left = b.Left;
    int bot = b.Bot;
    for (int x = 0; x < num1; ++x)
    {
      for (int y = 0; y < num2; y += 2)
      {
        b.Jump(x, y);
        b.Ore(1, SimHashes.Cuprite);
        b.JumpTo(left, bot);
      }
    }
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupFeedingTest(Scenario.Builder b)
  {
    b.FillOffsets(SimHashes.IgneousRock, 1, 0, 3, 0, 3, 1, 5, 0, 5, 1, 5, 2, 7, 0, 7, 1, 7, 2, 9, 0, 9, 1, 11, 0);
    b.PrefabOffsets("Hexaped", 0, 0, 2, 0, 4, 0, 7, 3, 9, 2, 11, 1);
    b.OreOffsets(1, SimHashes.IronOre, 1, 1, 3, 2, 5, 3, 8, 0, 10, 0, 12, 0);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupLiquifierTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Minion((System.Action<GameObject>) null);
    b.Ore(2, SimHashes.Ice);
    b.Building("ManualGenerator");
    b.Building("Liquifier");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupFallTest(Scenario.Builder b)
  {
    b.Jump(0, 5);
    b.Minion((System.Action<GameObject>) null);
    b.Jump(0, -1);
    b.Building("Tile");
    b.Building("Tile");
    b.Building("Tile");
    b.Jump(-1, 1);
    b.Minion((System.Action<GameObject>) null);
    b.Jump(2, 0);
    b.Minion((System.Action<GameObject>) null);
    b.Jump(0, -1);
    b.Building("Tile");
    b.Jump(2, 1);
    b.Minion((System.Action<GameObject>) null);
    b.Building("Ladder");
    b.Jump(-1, -1);
    b.Building("Tile");
    b.Jump(-1, -3);
    b.Building("Ladder");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupClimbTest(int left, int bot)
  {
    this.DigHole(left, bot, 13, 5);
    this.SpawnPrefab(left + 1, bot + 1, "Minion", Grid.SceneLayer.Ore);
    int x1 = left + 2;
    int num1 = x1 + 1;
    this.Clear(x1, bot - 1);
    int x2 = num1 + 1;
    int num2 = x2 + 1;
    this.Fill(x2, bot, SimHashes.Ice);
    int x3 = num2 + 1;
    this.Clear(x3, bot - 1);
    int x4 = x3;
    int num3 = x4 + 1;
    this.Clear(x4, bot - 2);
    int x5 = num3;
    int x6 = x5 + 1;
    this.Fill(x5, bot, SimHashes.Ice);
    this.Clear(x6, bot - 1);
    int x7 = x6;
    int num4 = x7 + 1;
    this.Clear(x7, bot - 2);
    int x8 = num4 + 1;
    this.Fill(x8, bot, SimHashes.Ice);
    this.Fill(x8, bot + 1, SimHashes.Ice);
  }

  private void SetupSuitRechargeTest(Scenario.Builder b)
  {
    b.Prefab("PressureSuit", (System.Action<GameObject>) (go => go.GetComponent<SuitTank>().Empty()));
    b.Building("ManualGenerator");
    b.Minion((System.Action<GameObject>) null);
    b.Building("SuitRecharger");
    b.Minion((System.Action<GameObject>) null);
    b.Building("GasVent");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  private void SetupSuitTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Prefab("PressureSuit", (System.Action<GameObject>) null);
    b.Jump(1, 2);
    b.Building("Tile");
    b.Jump(-1, -2);
    b.Building("Door");
    b.Building("ManualGenerator");
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  private void SetupTwoKelvinsOneSuitTest(Scenario.Builder b)
  {
    b.Minion((System.Action<GameObject>) null);
    b.Jump(2, 0);
    b.Building("Door");
    b.Jump(2, 0);
    b.Minion((System.Action<GameObject>) null);
    b.Prefab("PressureSuit", (System.Action<GameObject>) null);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void Clear()
  {
    foreach (Component component in Components.Brains.Items)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in Components.Pickupables.Items)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in Components.BuildingCompletes.Items)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }

  public void SetupGameplayTest()
  {
    this.Init();
    CameraController.Instance.SnapTo(Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background));
    if (this.ClearExistingScene)
      this.Clear();
    Scenario.RowLayout rowLayout = new Scenario.RowLayout(0, 0);
    if (this.CementMixerTest)
      this.SetupCementMixerTest(rowLayout.NextRow());
    if (this.RockCrusherTest)
      this.SetupRockCrusherTest(rowLayout.NextRow());
    if (this.PropaneGeneratorTest)
      this.SetupPropaneGeneratorTest(rowLayout.NextRow());
    if (this.DoorTest)
      this.SetupDoorTest(rowLayout.NextRow());
    if (this.HatchTest)
      this.SetupHatchTest(rowLayout.NextRow());
    if (this.AirLockTest)
      this.SetupAirLockTest(rowLayout.NextRow());
    if (this.BedTest)
      this.SetupBedTest(rowLayout.NextRow());
    if (this.LiquifierTest)
      this.SetupLiquifierTest(rowLayout.NextRow());
    if (this.SuitTest)
      this.SetupSuitTest(rowLayout.NextRow());
    if (this.SuitRechargeTest)
      this.SetupSuitRechargeTest(rowLayout.NextRow());
    if (this.TwoKelvinsOneSuitTest)
      this.SetupTwoKelvinsOneSuitTest(rowLayout.NextRow());
    if (this.FabricatorTest)
      this.SetupFabricatorTest(rowLayout.NextRow());
    if (this.ElectrolyzerTest)
      this.SetupElectrolyzerTest(rowLayout.NextRow());
    if (this.HexapedTest)
      this.SetupHexapedTest(rowLayout.NextRow());
    if (this.FallTest)
      this.SetupFallTest(rowLayout.NextRow());
    if (this.FeedingTest)
      this.SetupFeedingTest(rowLayout.NextRow());
    if (this.OrePerformanceTest)
      this.SetupOrePerformanceTest(rowLayout.NextRow());
    if (!this.KilnTest)
      return;
    this.SetupKilnTest(rowLayout.NextRow());
  }

  private GameObject SpawnMinion(int x, int y)
  {
    return this.SpawnPrefab(x, y, "Minion", Grid.SceneLayer.Move);
  }

  private void SetupLadderTest(int left, int bot)
  {
    int num1 = 5;
    this.DigHole(left, bot, 13, num1);
    this.SpawnMinion(left + 1, bot);
    int x1 = left + 1;
    int num2 = x1 + 1;
    this.PlacerLadder(x1, bot, num1);
    int x2 = num2;
    int x3 = x2 + 1;
    this.PlaceColumn(x2, bot, num1);
    this.SpawnMinion(x3, bot);
    int x4 = x3;
    int num3 = x4 + 1;
    this.PlacerLadder(x4, bot + 1, num1 - 1);
    int x5 = num3;
    int num4 = x5 + 1;
    this.PlaceColumn(x5, bot, num1);
    int x6 = num4;
    int num5 = x6 + 1;
    this.SpawnMinion(x6, bot);
    int x7 = num5;
    int num6 = x7 + 1;
    this.PlacerLadder(x7, bot, num1);
    int x8 = num6;
    int num7 = x8 + 1;
    this.PlaceColumn(x8, bot, num1);
    int x9 = num7;
    int num8 = x9 + 1;
    this.SpawnMinion(x9, bot);
    int x10 = num8;
    int num9 = x10 + 1;
    this.PlacerLadder(x10, bot + 1, num1 - 1);
    int x11 = num9;
    int num10 = x11 + 1;
    this.PlaceColumn(x11, bot, num1);
    int x12 = num10;
    int num11 = x12 + 1;
    this.SpawnMinion(x12, bot);
    int x13 = num11;
    int num12 = x13 + 1;
    this.PlacerLadder(x13, bot - 1, -num1);
  }

  public void PlaceUtilitiesX(int left, int bot, int amount)
  {
    for (int index = 0; index < amount; ++index)
      this.PlaceUtilities(left + index, bot);
  }

  public void PlaceUtilities(int left, int bot)
  {
    this.PlaceBuilding(left, bot, "Wire", SimHashes.Cuprite);
    this.PlaceBuilding(left, bot, "GasConduit", SimHashes.Cuprite);
  }

  public void SetupVisualTest()
  {
    this.Init();
    this.SetupBuildingTest(new Scenario.RowLayout(this.Left, this.Bot), false, false);
  }

  private void SpawnMaterialTest(Scenario.Builder b)
  {
    foreach (Element element in ElementLoader.elements)
    {
      if (element.IsSolid)
      {
        b.Element = element.id;
        b.Building("Generator");
      }
    }
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public GameObject PlaceBuilding(int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
  {
    return Scenario.PlaceBuilding(this.RootCell, x, y, prefab_id, element);
  }

  public static GameObject PlaceBuilding(
    int root_cell,
    int x,
    int y,
    string prefab_id,
    SimHashes element = SimHashes.Cuprite)
  {
    int cell = Grid.OffsetCell(root_cell, x, y);
    BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
    if ((UnityEngine.Object) buildingDef == (UnityEngine.Object) null || buildingDef.PlacementOffsets == null)
      DebugUtil.LogErrorArgs((object) "Missing def for", (object) prefab_id);
    Element elementByHash = ElementLoader.FindElementByHash(element);
    Debug.Assert(elementByHash != null, (object) "Missing primary element.");
    GameObject gameObject = buildingDef.Build(buildingDef.GetBuildingCell(cell), Orientation.Neutral, (Storage) null, (IList<Tag>) new Tag[2]
    {
      elementByHash.tag,
      ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
    }, 293.15f, false, -1f);
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    component.InternalTemperature = 300f;
    component.Temperature = 300f;
    return gameObject;
  }

  private void SpawnOre(int x, int y, SimHashes element = SimHashes.Cuprite)
  {
    this.RunAfterNextUpdate((System.Action) (() =>
    {
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(this.RootCell, x, y), Grid.SceneLayer.Ore);
      posCcc.x += UnityEngine.Random.Range(-0.1f, 0.1f);
      ElementLoader.FindElementByHash(element).substance.SpawnResource(posCcc, 4000f, 293f, byte.MaxValue, 0, false, false, false);
    }));
  }

  public GameObject SpawnPrefab(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
  {
    return Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
  }

  public void SpawnPrefabLate(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
  {
    this.RunAfterNextUpdate((System.Action) (() => Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer)));
  }

  public static GameObject SpawnPrefab(
    int RootCell,
    int x,
    int y,
    string name,
    Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
  {
    int cell = Grid.OffsetCell(RootCell, x, y);
    GameObject prefab = Assets.GetPrefab(TagManager.Create(name));
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return (GameObject) null;
    return GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell, scene_layer), scene_layer, (string) null, 0);
  }

  public void SetupElementTest()
  {
    this.Init();
    PropertyTextures.FogOfWarScale = 1f;
    CameraController.Instance.SnapTo(Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background));
    this.Clear();
    Scenario.Builder builder1 = new Scenario.RowLayout(0, 0).NextRow();
    HashSet<Element> elements = new HashSet<Element>();
    int bot = builder1.Bot;
    foreach (Element element1 in ElementLoader.elements.Where<Element>((Func<Element, bool>) (element => element.IsSolid)).OrderBy<Element, SimHashes>((Func<Element, SimHashes>) (element => element.highTempTransitionTarget)).ToList<Element>())
    {
      if (element1.IsSolid)
      {
        Element element2 = element1;
        int left = builder1.Left;
        bool hasTransitionUp;
        do
        {
          elements.Add(element2);
          builder1.Hole(2, 3);
          builder1.Fill(2, 2, element2.id);
          builder1.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
          builder1 = new Scenario.Builder(left, builder1.Bot + 4, SimHashes.Copper);
          hasTransitionUp = element2.HasTransitionUp;
          if (hasTransitionUp)
            element2 = element2.highTempTransition;
        }
        while (hasTransitionUp);
        builder1 = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
      }
    }
    foreach (Element element1 in ElementLoader.elements.Where<Element>((Func<Element, bool>) (element =>
    {
      if (element.IsLiquid)
        return !elements.Contains(element);
      return false;
    })).OrderBy<Element, SimHashes>((Func<Element, SimHashes>) (element => element.highTempTransitionTarget)).ToList<Element>())
    {
      Element element2 = element1;
      int left = builder1.Left;
      bool hasTransitionUp;
      do
      {
        elements.Add(element2);
        builder1.Hole(2, 3);
        builder1.Fill(2, 2, element2.id);
        builder1.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
        builder1 = new Scenario.Builder(left, builder1.Bot + 4, SimHashes.Copper);
        hasTransitionUp = element2.HasTransitionUp;
        if (hasTransitionUp)
          element2 = element2.highTempTransition;
      }
      while (hasTransitionUp);
      builder1 = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
    }
    foreach (Element element in ElementLoader.elements.Where<Element>((Func<Element, bool>) (element =>
    {
      if (element.state == Element.State.Gas)
        return !elements.Contains(element);
      return false;
    })).ToList<Element>())
    {
      int left = builder1.Left;
      builder1.Hole(2, 3);
      builder1.Fill(2, 2, element.id);
      builder1.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
      Scenario.Builder builder2 = new Scenario.Builder(left, builder1.Bot + 4, SimHashes.Copper);
      builder1 = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
    }
  }

  private void InitDebugScenario()
  {
    this.Init();
    PropertyTextures.FogOfWarScale = 1f;
    CameraController.Instance.SnapTo(Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background));
    this.Clear();
  }

  public void SetupTileTest()
  {
    this.InitDebugScenario();
    for (int y = 0; y < Grid.HeightInCells; ++y)
    {
      for (int x = 0; x < Grid.WidthInCells; ++x)
        SimMessages.ReplaceElement(Grid.XYToCell(x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 100f, -1f, byte.MaxValue, 0, -1);
    }
    Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
    for (int index = 0; index < 16; ++index)
    {
      builder.Jump(0, 0);
      builder.Fill(1, 1, (index & 1) == 0 ? SimHashes.Diamond : SimHashes.Copper);
      builder.Jump(1, 0);
      builder.Fill(1, 1, (index & 2) == 0 ? SimHashes.Diamond : SimHashes.Copper);
      builder.Jump(-1, 1);
      builder.Fill(1, 1, (index & 4) == 0 ? SimHashes.Diamond : SimHashes.Copper);
      builder.Jump(1, 0);
      builder.Fill(1, 1, (index & 8) == 0 ? SimHashes.Diamond : SimHashes.Copper);
      builder.Jump(2, -1);
    }
  }

  public void SetupRiverTest()
  {
    this.InitDebugScenario();
    int num1 = Mathf.Min(64, Grid.WidthInCells);
    int num2 = Mathf.Min(64, Grid.HeightInCells);
    List<Element> elementList = new List<Element>();
    foreach (Element element in ElementLoader.elements)
    {
      if (element.IsLiquid)
        elementList.Add(element);
    }
    for (int y = 0; y < num2; ++y)
    {
      for (int x = 0; x < num1; ++x)
      {
        SimHashes new_element = y != 0 ? SimHashes.Oxygen : SimHashes.Unobtanium;
        SimMessages.ReplaceElement(Grid.XYToCell(x, y), new_element, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
      }
    }
  }

  public void SetupRockCrusherTest(Scenario.Builder b)
  {
    this.InitDebugScenario();
    b.Building("ManualGenerator");
    b.Minion((System.Action<GameObject>) null);
    b.Building("Crusher");
    b.Minion((System.Action<GameObject>) null);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupCementMixerTest(Scenario.Builder b)
  {
    this.InitDebugScenario();
    b.Building("Generator");
    b.Minion((System.Action<GameObject>) null);
    b.Building("Crusher");
    b.Minion((System.Action<GameObject>) null);
    b.Minion((System.Action<GameObject>) null);
    b.Building("Mixer");
    b.Ore(20, SimHashes.SedimentaryRock);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public void SetupKilnTest(Scenario.Builder b)
  {
    this.InitDebugScenario();
    b.Building("ManualGenerator");
    b.Minion((System.Action<GameObject>) null);
    b.Building("Kiln");
    b.Minion((System.Action<GameObject>) null);
    b.Ore(20, SimHashes.SandCement);
    b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
  }

  public class RowLayout
  {
    public int Left;
    public int Bot;
    public Scenario.Builder Builder;

    public RowLayout(int left, int bot)
    {
      this.Left = left;
      this.Bot = bot;
    }

    public Scenario.Builder NextRow()
    {
      if (this.Builder != null)
        this.Bot = this.Builder.Max.y + 1;
      this.Builder = new Scenario.Builder(this.Left, this.Bot, SimHashes.Copper);
      return this.Builder;
    }
  }

  public class Builder
  {
    public bool PlaceUtilities;
    public int Left;
    public int Bot;
    public Vector2I Min;
    public Vector2I Max;
    public SimHashes Element;
    private Scenario Scenario;

    public Builder(int left, int bot, SimHashes element = SimHashes.Copper)
    {
      this.Left = left;
      this.Bot = bot;
      this.Element = element;
      this.Scenario = Scenario.Instance;
      this.PlaceUtilities = true;
      this.Min = new Vector2I(left, bot);
      this.Max = new Vector2I(left, bot);
    }

    private void UpdateMinMax(int x, int y)
    {
      this.Min.x = Math.Min(x, this.Min.x);
      this.Min.y = Math.Min(y, this.Min.y);
      this.Max.x = Math.Max(x + 1, this.Max.x);
      this.Max.y = Math.Max(y + 1, this.Max.y);
    }

    public void Utilities(int count)
    {
      for (int index = 0; index < count; ++index)
      {
        this.Scenario.PlaceUtilities(this.Left, this.Bot);
        ++this.Left;
      }
    }

    public void BuildingOffsets(string prefab_id, params int[] offsets)
    {
      int left = this.Left;
      int bot = this.Bot;
      for (int index = 0; index < offsets.Length / 2; ++index)
      {
        this.Jump(offsets[index * 2], offsets[index * 2 + 1]);
        this.Building(prefab_id);
        this.JumpTo(left, bot);
      }
    }

    public void Placer(string prefab_id, Element element)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
      Vector3 pos = Grid.CellToPosCBC(buildingDef.GetBuildingCell(Grid.OffsetCell(Scenario.Instance.RootCell, this.Left, this.Bot)), Grid.SceneLayer.Building);
      this.UpdateMinMax(this.Left, this.Bot);
      this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
      this.Left += buildingDef.WidthInCells;
      this.Scenario.RunAfterNextUpdate((System.Action) (() => Assets.GetBuildingDef(prefab_id).TryPlace((GameObject) null, pos, Orientation.Neutral, (IList<Tag>) new Tag[2]
      {
        element.tag,
        ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
      }, 0)));
    }

    public GameObject Building(string prefab_id)
    {
      GameObject gameObject = this.Scenario.PlaceBuilding(this.Left, this.Bot, prefab_id, this.Element);
      BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
      this.UpdateMinMax(this.Left, this.Bot);
      this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
      if (this.PlaceUtilities)
      {
        for (int index = 0; index < buildingDef.WidthInCells; ++index)
        {
          this.UpdateMinMax(this.Left + index, this.Bot);
          this.Scenario.PlaceUtilities(this.Left + index, this.Bot);
        }
      }
      this.Left += buildingDef.WidthInCells;
      return gameObject;
    }

    public void Minion(System.Action<GameObject> on_spawn = null)
    {
      this.UpdateMinMax(this.Left, this.Bot);
      int left = this.Left;
      int bot = this.Bot;
      this.Scenario.RunAfterNextUpdate((System.Action) (() =>
      {
        GameObject gameObject = this.Scenario.SpawnMinion(left, bot);
        if (on_spawn == null)
          return;
        on_spawn(gameObject);
      }));
    }

    private GameObject Hexaped()
    {
      return this.Scenario.SpawnPrefab(this.Left, this.Bot, nameof (Hexaped), Grid.SceneLayer.Front);
    }

    public void OreOffsets(int count, SimHashes element, params int[] offsets)
    {
      int left = this.Left;
      int bot = this.Bot;
      for (int index = 0; index < offsets.Length / 2; ++index)
      {
        this.Jump(offsets[index * 2], offsets[index * 2 + 1]);
        this.Ore(count, element);
        this.JumpTo(left, bot);
      }
    }

    public void Ore(int count = 1, SimHashes element = SimHashes.Cuprite)
    {
      this.UpdateMinMax(this.Left, this.Bot);
      for (int index = 0; index < count; ++index)
        this.Scenario.SpawnOre(this.Left, this.Bot, element);
    }

    public void PrefabOffsets(string prefab_id, params int[] offsets)
    {
      int left = this.Left;
      int bot = this.Bot;
      for (int index = 0; index < offsets.Length / 2; ++index)
      {
        this.Jump(offsets[index * 2], offsets[index * 2 + 1]);
        this.Prefab(prefab_id, (System.Action<GameObject>) null);
        this.JumpTo(left, bot);
      }
    }

    public void Prefab(string prefab_id, System.Action<GameObject> on_spawn = null)
    {
      this.UpdateMinMax(this.Left, this.Bot);
      int left = this.Left;
      int bot = this.Bot;
      this.Scenario.RunAfterNextUpdate((System.Action) (() =>
      {
        GameObject gameObject = this.Scenario.SpawnPrefab(left, bot, prefab_id, Grid.SceneLayer.Ore);
        if (on_spawn == null)
          return;
        on_spawn(gameObject);
      }));
    }

    public void Wall(int height)
    {
      for (int index = 0; index < height; ++index)
      {
        this.Scenario.PlaceBuilding(this.Left, this.Bot + index, "Tile", SimHashes.Cuprite);
        this.UpdateMinMax(this.Left, this.Bot + index);
        if (this.PlaceUtilities)
          this.Scenario.PlaceUtilities(this.Left, this.Bot + index);
      }
      ++this.Left;
    }

    public void Jump(int x = 0, int y = 0)
    {
      this.Left += x;
      this.Bot += y;
    }

    public void JumpTo(int left, int bot)
    {
      this.Left = left;
      this.Bot = bot;
    }

    public void Hole(int width, int height)
    {
      for (int index1 = 0; index1 < width; ++index1)
      {
        for (int index2 = 0; index2 < height; ++index2)
        {
          int gameCell = Grid.OffsetCell(this.Scenario.RootCell, this.Left + index1, this.Bot + index2);
          this.UpdateMinMax(this.Left + index1, this.Bot + index2);
          SimMessages.ReplaceElement(gameCell, SimHashes.Vacuum, CellEventLogger.Instance.Scenario, 0.0f, -1f, byte.MaxValue, 0, -1);
          this.Scenario.ReplaceElementMask[gameCell] = true;
        }
      }
    }

    public void FillOffsets(SimHashes element, params int[] offsets)
    {
      int left = this.Left;
      int bot = this.Bot;
      for (int index = 0; index < offsets.Length / 2; ++index)
      {
        this.Jump(offsets[index * 2], offsets[index * 2 + 1]);
        this.Fill(1, 1, element);
        this.JumpTo(left, bot);
      }
    }

    public void Fill(int width, int height, SimHashes element)
    {
      for (int index1 = 0; index1 < width; ++index1)
      {
        for (int index2 = 0; index2 < height; ++index2)
        {
          int gameCell = Grid.OffsetCell(this.Scenario.RootCell, this.Left + index1, this.Bot + index2);
          this.UpdateMinMax(this.Left + index1, this.Bot + index2);
          SimMessages.ReplaceElement(gameCell, element, CellEventLogger.Instance.Scenario, 5000f, -1f, byte.MaxValue, 0, -1);
          this.Scenario.ReplaceElementMask[gameCell] = true;
        }
      }
    }

    public void InAndOuts()
    {
      this.Wall(3);
      this.Building("GasVent");
      this.Hole(3, 3);
      this.Utilities(2);
      this.Wall(3);
      this.Building("LiquidVent");
      this.Hole(3, 3);
      this.Utilities(2);
      this.Wall(3);
      this.Fill(3, 3, SimHashes.Water);
      this.Utilities(2);
      GameObject pump = this.Building("Pump");
      this.Scenario.RunAfterNextUpdate((System.Action) (() => pump.GetComponent<BuildingEnabledButton>().IsEnabled = true));
    }

    public Scenario.Builder FinalizeRoom(SimHashes element = SimHashes.Oxygen, SimHashes tileElement = SimHashes.Steel)
    {
      for (int x = this.Min.x - 1; x <= this.Max.x; ++x)
      {
        if (x == this.Min.x - 1 || x == this.Max.x)
        {
          for (int y = this.Min.y - 1; y <= this.Max.y; ++y)
            this.Scenario.PlaceBuilding(x, y, "Tile", tileElement);
        }
        else
        {
          int num = 500;
          if (element == SimHashes.Void)
            num = 0;
          for (int y = this.Min.y; y < this.Max.y; ++y)
          {
            int gameCell = Grid.OffsetCell(this.Scenario.RootCell, x, y);
            if (!this.Scenario.ReplaceElementMask[gameCell])
              SimMessages.ReplaceElement(gameCell, element, CellEventLogger.Instance.Scenario, (float) num, -1f, byte.MaxValue, 0, -1);
          }
        }
        this.Scenario.PlaceBuilding(x, this.Min.y - 1, "Tile", tileElement);
        this.Scenario.PlaceBuilding(x, this.Max.y, "Tile", tileElement);
      }
      return new Scenario.Builder(this.Max.x + 1, this.Min.y, SimHashes.Copper);
    }
  }
}
