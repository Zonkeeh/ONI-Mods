// Decompiled with JetBrains decompiler
// Type: TemplateLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

public static class TemplateLoader
{
  private static TemplateContainer template;

  public static void Stamp(
    TemplateContainer template,
    Vector2 rootLocation,
    System.Action on_complete_callback)
  {
    TemplateLoader.template = template;
    TemplateLoader.BuildPhase1((int) rootLocation.x, (int) rootLocation.y, (System.Action) (() => TemplateLoader.BuildPhase2((int) rootLocation.x, (int) rootLocation.y, (System.Action) (() => TemplateLoader.BuildPhase3((int) rootLocation.x, (int) rootLocation.y, (System.Action) (() => TemplateLoader.BuildPhase4((int) rootLocation.x, (int) rootLocation.y, on_complete_callback)))))));
  }

  private static void BuildPhase1(int baseX, int baseY, System.Action callback)
  {
    if (Grid.WidthInCells < 16)
      return;
    CellOffset[] cellOffsetArray = new CellOffset[TemplateLoader.template.cells.Count];
    for (int index = 0; index < TemplateLoader.template.cells.Count; ++index)
      cellOffsetArray[index] = new CellOffset(TemplateLoader.template.cells[index].location_x, TemplateLoader.template.cells[index].location_y);
    TemplateLoader.ClearPickups(baseX, baseY, cellOffsetArray);
    if (TemplateLoader.template.cells.Count > 0)
    {
      TemplateLoader.ApplyGridProperties(baseX, baseY, TemplateLoader.template);
      TemplateLoader.PlaceCells(baseX, baseY, TemplateLoader.template, callback);
      TemplateLoader.ClearEntities<Crop>(baseX, baseY, cellOffsetArray);
      TemplateLoader.ClearEntities<Health>(baseX, baseY, cellOffsetArray);
      TemplateLoader.ClearEntities<Geyser>(baseX, baseY, cellOffsetArray);
    }
    else
      callback();
  }

  private static void BuildPhase2(int baseX, int baseY, System.Action callback)
  {
    int index1 = Grid.OffsetCell(0, baseX, baseY);
    if (TemplateLoader.template == null)
      Debug.LogError((object) "No stamp template");
    if (TemplateLoader.template.buildings != null)
    {
      for (int index2 = 0; index2 < TemplateLoader.template.buildings.Count; ++index2)
        TemplateLoader.PlaceBuilding(TemplateLoader.template.buildings[index2], index1);
    }
    HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(callback, false));
    SimMessages.ReplaceElement(index1, ElementLoader.elements[(int) Grid.ElementIdx[index1]].id, CellEventLogger.Instance.TemplateLoader, Grid.Mass[index1], Grid.Temperature[index1], Grid.DiseaseIdx[index1], Grid.DiseaseCount[index1], handle.index);
    handle.index = -1;
  }

  public static GameObject PlaceBuilding(Prefab prefab, int root_cell)
  {
    if (prefab == null || prefab.id == string.Empty)
      return (GameObject) null;
    if ((UnityEngine.Object) Assets.GetBuildingDef(prefab.id) == (UnityEngine.Object) null)
      return (GameObject) null;
    int locationX = prefab.location_x;
    int locationY = prefab.location_y;
    if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, locationX, locationY)))
      return (GameObject) null;
    if (Assets.GetBuildingDef(prefab.id).WidthInCells >= 3)
      --locationX;
    GameObject gameObject1 = Scenario.PlaceBuilding(root_cell, locationX, locationY, prefab.id, prefab.element);
    if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) ("Null prefab for " + prefab.id));
      return gameObject1;
    }
    BuildingComplete component1 = gameObject1.GetComponent<BuildingComplete>();
    gameObject1.GetComponent<KPrefabID>().AddTag(GameTags.TemplateBuilding, true);
    Rotatable component2 = gameObject1.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.SetOrientation(prefab.rotationOrientation);
    PrimaryElement component3 = component1.GetComponent<PrimaryElement>();
    if ((double) prefab.temperature > 0.0)
      component3.Temperature = prefab.temperature;
    component3.AddDisease(Db.Get().Diseases.GetIndex((HashedString) prefab.diseaseName), prefab.diseaseCount, "TemplateLoader.PlaceBuilding");
    if (prefab.id == "Door")
    {
      for (int index = 0; index < component1.PlacementCells.Length; ++index)
        SimMessages.ReplaceElement(component1.PlacementCells[index], SimHashes.Vacuum, CellEventLogger.Instance.TemplateLoader, 0.0f, 0.0f, byte.MaxValue, 0, -1);
    }
    if (prefab.amounts != null)
    {
      foreach (Prefab.template_amount_value amount in prefab.amounts)
      {
        try
        {
          if (Db.Get().Amounts.Get(amount.id) != null)
            gameObject1.GetAmounts().SetValue(amount.id, amount.value);
        }
        catch
        {
          Debug.LogWarning((object) string.Format("Building does not have amount with ID {0}", (object) amount.id));
        }
      }
    }
    if (prefab.other_values != null)
    {
      foreach (Prefab.template_amount_value otherValue in prefab.other_values)
      {
        string id = otherValue.id;
        if (id != null)
        {
          if (!(id == "joulesAvailable"))
          {
            if (!(id == "sealedDoorDirection"))
            {
              if (id == "switchSetting")
              {
                LogicSwitch s = gameObject1.GetComponent<LogicSwitch>();
                if ((bool) ((UnityEngine.Object) s) && (s.IsSwitchedOn && (double) otherValue.value == 0.0 || !s.IsSwitchedOn && (double) otherValue.value == 1.0))
                  s.SetFirstFrameCallback((System.Action) (() => s.HandleToggle()));
              }
            }
            else
            {
              Unsealable component4 = gameObject1.GetComponent<Unsealable>();
              if ((bool) ((UnityEngine.Object) component4))
                component4.facingRight = (double) otherValue.value != 0.0;
            }
          }
          else
          {
            Battery component4 = gameObject1.GetComponent<Battery>();
            if ((bool) ((UnityEngine.Object) component4))
              component4.AddEnergy(otherValue.value);
          }
        }
      }
    }
    if (prefab.storage != null && prefab.storage.Count > 0)
    {
      Storage component4 = component1.gameObject.GetComponent<Storage>();
      if ((UnityEngine.Object) component4 == (UnityEngine.Object) null)
        Debug.LogWarning((object) ("No storage component on stampTemplate building " + prefab.id + ". Saved storage contents will be ignored."));
      for (int index = 0; index < prefab.storage.Count; ++index)
      {
        StorageItem storageItem = prefab.storage[index];
        string id = storageItem.id;
        GameObject go;
        if (storageItem.isOre)
        {
          go = ElementLoader.FindElementByHash(storageItem.element).substance.SpawnResource(Vector3.zero, storageItem.units, storageItem.temperature, Db.Get().Diseases.GetIndex((HashedString) storageItem.diseaseName), storageItem.diseaseCount, false, false, false);
        }
        else
        {
          go = Scenario.SpawnPrefab(root_cell, 0, 0, id, Grid.SceneLayer.Ore);
          if ((UnityEngine.Object) go == (UnityEngine.Object) null)
          {
            Debug.LogWarning((object) ("Null prefab for " + id));
            continue;
          }
          go.SetActive(true);
          PrimaryElement component5 = go.GetComponent<PrimaryElement>();
          component5.Units = storageItem.units;
          component5.Temperature = storageItem.temperature;
          component5.AddDisease(Db.Get().Diseases.GetIndex((HashedString) storageItem.diseaseName), storageItem.diseaseCount, "TemplateLoader.PlaceBuilding");
          Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
          if (smi != null)
            smi.RotValue = storageItem.rottable.rotAmount;
        }
        GameObject gameObject2 = component4.Store(go, true, true, true, false);
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
          gameObject2.GetComponent<Pickupable>().OnStore((object) component4);
      }
    }
    if (prefab.connections != 0)
      TemplateLoader.PlaceUtilityConnection(gameObject1, prefab, root_cell);
    return gameObject1;
  }

  public static void PlaceUtilityConnection(GameObject spawned, Prefab bc, int root_cell)
  {
    int cell = Grid.OffsetCell(root_cell, bc.location_x, bc.location_y);
    UtilityConnections connection = (UtilityConnections) bc.connections;
    string id = bc.id;
    if (id == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (TemplateLoader.\u003C\u003Ef__switch\u0024map1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TemplateLoader.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(10)
      {
        {
          "Wire",
          0
        },
        {
          "InsulatedWire",
          0
        },
        {
          "HighWattageWire",
          0
        },
        {
          "GasConduit",
          1
        },
        {
          "InsulatedGasConduit",
          1
        },
        {
          "LiquidConduit",
          2
        },
        {
          "InsulatedLiquidConduit",
          2
        },
        {
          "SolidConduit",
          3
        },
        {
          "LogicWire",
          4
        },
        {
          "TravelTube",
          5
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!TemplateLoader.\u003C\u003Ef__switch\u0024map1.TryGetValue(id, out num))
      return;
    switch (num)
    {
      case 0:
        spawned.GetComponent<Wire>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.electricalConduitSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
      case 1:
        spawned.GetComponent<Conduit>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.gasConduitSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
      case 2:
        spawned.GetComponent<Conduit>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.liquidConduitSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
      case 3:
        spawned.GetComponent<SolidConduit>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.solidConduitSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
      case 4:
        spawned.GetComponent<LogicWire>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.logicCircuitSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
      case 5:
        spawned.GetComponent<TravelTube>().SetFirstFrameCallback((System.Action) (() =>
        {
          Game.Instance.travelTubeSystem.SetConnections(connection, cell, true);
          KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          component.Refresh();
        }));
        break;
    }
  }

  public static GameObject PlacePickupables(Prefab prefab, int root_cell)
  {
    int locationX = prefab.location_x;
    int locationY = prefab.location_y;
    if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, locationX, locationY)))
      return (GameObject) null;
    GameObject go = Scenario.SpawnPrefab(root_cell, locationX, locationY, prefab.id, Grid.SceneLayer.Ore);
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) ("Null prefab for " + prefab.id));
      return (GameObject) null;
    }
    go.SetActive(true);
    if ((double) prefab.units != 0.0)
    {
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      component.Units = prefab.units;
      component.Temperature = (double) prefab.temperature <= 0.0 ? component.Element.defaultValues.temperature : prefab.temperature;
      component.AddDisease(Db.Get().Diseases.GetIndex((HashedString) prefab.diseaseName), prefab.diseaseCount, "TemplateLoader.PlacePickupables");
    }
    Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
    if (smi != null)
      smi.RotValue = prefab.rottable.rotAmount;
    return go;
  }

  public static GameObject PlaceOtherEntities(Prefab prefab, int root_cell)
  {
    int locationX = prefab.location_x;
    int locationY = prefab.location_y;
    if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, locationX, locationY)))
      return (GameObject) null;
    GameObject prefab1 = Assets.GetPrefab(new Tag(prefab.id));
    if ((UnityEngine.Object) prefab1 == (UnityEngine.Object) null)
      return (GameObject) null;
    Grid.SceneLayer scene_layer = Grid.SceneLayer.Front;
    KBatchedAnimController component = prefab1.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      scene_layer = component.sceneLayer;
    GameObject go = Scenario.SpawnPrefab(root_cell, locationX, locationY, prefab.id, scene_layer);
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) ("Null prefab for " + prefab.id));
      return (GameObject) null;
    }
    go.SetActive(true);
    if (prefab.amounts != null)
    {
      foreach (Prefab.template_amount_value amount in prefab.amounts)
      {
        try
        {
          go.GetAmounts().SetValue(amount.id, amount.value);
        }
        catch
        {
          Debug.LogWarning((object) string.Format("Entity {0} does not have amount with ID {1}", (object) go.GetProperName(), (object) amount.id));
        }
      }
    }
    return go;
  }

  public static GameObject PlaceElementalOres(Prefab prefab, int root_cell)
  {
    int locationX = prefab.location_x;
    int locationY = prefab.location_y;
    if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, locationX, locationY)))
      return (GameObject) null;
    Substance substance = ElementLoader.FindElementByHash(prefab.element).substance;
    Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(root_cell, locationX, locationY), Grid.SceneLayer.Ore);
    byte index = Db.Get().Diseases.GetIndex((HashedString) prefab.diseaseName);
    if ((double) prefab.temperature <= 0.0)
    {
      Debug.LogWarning((object) "Template trying to spawn zero temperature substance!");
      prefab.temperature = 300f;
    }
    return substance.SpawnResource(posCcc, prefab.units, prefab.temperature, index, prefab.diseaseCount, false, false, false);
  }

  private static void BuildPhase3(int baseX, int baseY, System.Action callback)
  {
    if (TemplateLoader.template != null)
    {
      int root_cell = Grid.OffsetCell(0, baseX, baseY);
      foreach (Component component1 in Components.BuildingCompletes.Items)
      {
        KAnimGraphTileVisualizer component2 = component1.GetComponent<KAnimGraphTileVisualizer>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.Refresh();
      }
      for (int index = 0; index < TemplateLoader.template.pickupables.Count; ++index)
      {
        if (TemplateLoader.template.pickupables[index] != null && !(TemplateLoader.template.pickupables[index].id == string.Empty))
          TemplateLoader.PlacePickupables(TemplateLoader.template.pickupables[index], root_cell);
      }
      for (int index = 0; index < TemplateLoader.template.elementalOres.Count; ++index)
      {
        if (TemplateLoader.template.elementalOres[index] != null && !(TemplateLoader.template.elementalOres[index].id == string.Empty))
          TemplateLoader.PlaceElementalOres(TemplateLoader.template.elementalOres[index], root_cell);
      }
    }
    if (callback == null)
      return;
    callback();
  }

  private static void BuildPhase4(int baseX, int baseY, System.Action callback)
  {
    if (TemplateLoader.template != null)
    {
      int root_cell = Grid.OffsetCell(0, baseX, baseY);
      for (int index = 0; index < TemplateLoader.template.otherEntities.Count; ++index)
      {
        if (TemplateLoader.template.otherEntities[index] != null && !(TemplateLoader.template.otherEntities[index].id == string.Empty))
          TemplateLoader.PlaceOtherEntities(TemplateLoader.template.otherEntities[index], root_cell);
      }
      TemplateLoader.template = (TemplateContainer) null;
    }
    if (callback == null)
      return;
    callback();
  }

  private static void ClearPickups(int baseX, int baseY, CellOffset[] template_as_offsets)
  {
    if ((UnityEngine.Object) SaveGame.Instance.worldGenSpawner != (UnityEngine.Object) null)
      SaveGame.Instance.worldGenSpawner.ClearSpawnersInArea(new Vector2((float) baseX, (float) baseY), template_as_offsets);
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      if (Grid.IsCellOffsetOf(Grid.XYToCell(baseX, baseY), pickupable.gameObject, template_as_offsets))
        Util.KDestroyGameObject(pickupable.gameObject);
    }
  }

  private static void ClearEntities<T>(int rootX, int rootY, CellOffset[] TemplateOffsets) where T : KMonoBehaviour
  {
    foreach (T obj in (T[]) UnityEngine.Object.FindObjectsOfType(typeof (T)))
    {
      if (Grid.IsCellOffsetOf(Grid.PosToCell(obj.gameObject), Grid.XYToCell(rootX, rootY), TemplateOffsets))
        Util.KDestroyGameObject(obj.gameObject);
    }
  }

  private static void PlaceCells(
    int baseX,
    int baseY,
    TemplateContainer template,
    System.Action callback)
  {
    HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(callback, false));
    if (template == null)
      Debug.LogError((object) "Template Loader does not have template.");
    for (int index1 = 0; index1 < template.cells.Count; ++index1)
    {
      int cell = Grid.XYToCell(template.cells[index1].location_x + baseX, template.cells[index1].location_y + baseY);
      if (!Grid.IsValidCell(cell))
        Debug.LogError((object) string.Format("Trying to replace invalid cells cell{0} root{1}:{2} offset{3}:{4}", (object) cell, (object) baseX, (object) baseY, (object) template.cells[index1].location_x, (object) template.cells[index1].location_y));
      SimHashes element = template.cells[index1].element;
      float mass = template.cells[index1].mass;
      float temperature = template.cells[index1].temperature;
      byte index2 = Db.Get().Diseases.GetIndex((HashedString) template.cells[index1].diseaseName);
      int diseaseCount = template.cells[index1].diseaseCount;
      SimMessages.ReplaceElement(cell, element, CellEventLogger.Instance.TemplateLoader, mass, temperature, index2, diseaseCount, handle.index);
      handle.index = -1;
    }
  }

  public static void ApplyGridProperties(int baseX, int baseY, TemplateContainer template)
  {
    for (int index = 0; index < template.cells.Count; ++index)
    {
      int cell = Grid.XYToCell(template.cells[index].location_x + baseX, template.cells[index].location_y + baseY);
      if (Grid.IsValidCell(cell) && template.cells[index].preventFoWReveal)
      {
        Grid.PreventFogOfWarReveal[cell] = true;
        Grid.Visible[cell] = (byte) 0;
      }
    }
  }
}
