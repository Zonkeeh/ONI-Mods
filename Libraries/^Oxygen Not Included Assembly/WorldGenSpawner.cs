// Decompiled with JetBrains decompiler
// Type: WorldGenSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TemplateClasses;
using UnityEngine;

public class WorldGenSpawner : KMonoBehaviour
{
  private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();
  [Serialize]
  private Prefab[] spawnInfos;
  [Serialize]
  private bool hasPlacedTemplates;

  public bool SpawnsRemain()
  {
    return this.spawnables.Count > 0;
  }

  public void SpawnEverything()
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
      this.spawnables[index].TrySpawn();
  }

  public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[index].cell, area))
        this.spawnables[index].FreeResources();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.hasPlacedTemplates)
      return;
    this.DoReveal();
  }

  protected override void OnSpawn()
  {
    if (!this.hasPlacedTemplates)
    {
      this.PlaceTemplates();
      this.hasPlacedTemplates = true;
    }
    if (this.spawnInfos == null)
      return;
    for (int index = 0; index < this.spawnInfos.Length; ++index)
      this.AddSpawnable(this.spawnInfos[index]);
  }

  [OnSerializing]
  private void OnSerializing()
  {
    List<Prefab> prefabList = new List<Prefab>();
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      WorldGenSpawner.Spawnable spawnable = this.spawnables[index];
      if (!spawnable.isSpawned)
        prefabList.Add(spawnable.spawnInfo);
    }
    this.spawnInfos = prefabList.ToArray();
  }

  private void AddSpawnable(Prefab prefab)
  {
    this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
  }

  public void AddLegacySpawner(Tag tag, int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, xy.x, xy.y, SimHashes.Carbon, -1f, 1f, (string) null, 0, Orientation.Neutral, (Prefab.template_amount_value[]) null, (Prefab.template_amount_value[]) null, 0));
  }

  private void PlaceTemplates()
  {
    this.spawnables = new List<WorldGenSpawner.Spawnable>();
    foreach (Prefab building in SaveGame.Instance.worldGen.SpawnData.buildings)
    {
      building.type = Prefab.Type.Building;
      this.AddSpawnable(building);
    }
    foreach (Prefab elementalOre in SaveGame.Instance.worldGen.SpawnData.elementalOres)
    {
      elementalOre.type = Prefab.Type.Ore;
      this.AddSpawnable(elementalOre);
    }
    foreach (Prefab otherEntity in SaveGame.Instance.worldGen.SpawnData.otherEntities)
    {
      otherEntity.type = Prefab.Type.Other;
      this.AddSpawnable(otherEntity);
    }
    foreach (Prefab pickupable in SaveGame.Instance.worldGen.SpawnData.pickupables)
    {
      pickupable.type = Prefab.Type.Pickupable;
      this.AddSpawnable(pickupable);
    }
    SaveGame.Instance.worldGen.SpawnData.buildings.Clear();
    SaveGame.Instance.worldGen.SpawnData.elementalOres.Clear();
    SaveGame.Instance.worldGen.SpawnData.otherEntities.Clear();
    SaveGame.Instance.worldGen.SpawnData.pickupables.Clear();
  }

  private void DoReveal()
  {
    Game.Instance.Reset(SaveGame.Instance.worldGen.SpawnData);
    for (int index = 0; index < Grid.CellCount; ++index)
    {
      Grid.Revealed[index] = false;
      Grid.Spawnable[index] = (byte) 0;
    }
    float innerRadius = 16.5f;
    float radius = 18f;
    Vector2I baseStartPos = SaveGame.Instance.worldGen.SpawnData.baseStartPos;
    GridVisibility.Reveal(baseStartPos.x, baseStartPos.y, radius, innerRadius);
  }

  private class Spawnable
  {
    private HandleVector<int>.Handle fogOfWarPartitionerEntry;
    private HandleVector<int>.Handle solidChangedPartitionerEntry;

    public Spawnable(Prefab spawn_info)
    {
      this.spawnInfo = spawn_info;
      int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
      GameObject prefab = Assets.GetPrefab((Tag) spawn_info.id);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
        if (def != null && def.adjustSpawnLocationCb != null)
          num = def.adjustSpawnLocationCb(num);
      }
      this.cell = num;
      Debug.Assert(Grid.IsValidCell(this.cell));
      if (Grid.Spawnable[this.cell] > (byte) 0)
        this.TrySpawn();
      else
        this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", (object) this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new System.Action<object>(this.OnReveal));
    }

    public Prefab spawnInfo { get; private set; }

    public bool isSpawned { get; private set; }

    public int cell { get; private set; }

    private void OnReveal(object data)
    {
      if (Grid.Spawnable[this.cell] <= (byte) 0)
        return;
      this.TrySpawn();
    }

    private void OnSolidChanged(object data)
    {
      if (Grid.Solid[this.cell])
        return;
      GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
      Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      this.Spawn();
    }

    public void FreeResources()
    {
      if (this.solidChangedPartitionerEntry.IsValid())
      {
        GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
        if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
          Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      }
      GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
      this.isSpawned = true;
    }

    public void TrySpawn()
    {
      if (this.isSpawned || this.solidChangedPartitionerEntry.IsValid())
        return;
      GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
      GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        bool flag = false;
        if ((UnityEngine.Object) prefab.GetComponent<Pickupable>() != (UnityEngine.Object) null && !prefab.HasTag(GameTags.Creatures.Digger))
          flag = true;
        else if (prefab.GetDef<BurrowMonitor.Def>() != null)
          flag = true;
        if (flag && Grid.Solid[this.cell])
        {
          this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", (object) this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
          Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
        }
        else
          this.Spawn();
      }
      else
        this.Spawn();
    }

    private Tag GetPrefabTag()
    {
      Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
      if (mob != null && mob.prefabName != null)
        return new Tag(mob.prefabName);
      return new Tag(this.spawnInfo.id);
    }

    private void Spawn()
    {
      this.isSpawned = true;
      GameObject go = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && (bool) ((UnityEngine.Object) go))
      {
        go.SetActive(true);
        go.Trigger(1119167081, (object) null);
      }
      this.FreeResources();
    }

    public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(
      Prefab.Type type)
    {
      switch (type)
      {
        case Prefab.Type.Building:
          // ISSUE: reference to a compiler-generated field
          if (WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache0 = new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
          }
          // ISSUE: reference to a compiler-generated field
          return WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache0;
        case Prefab.Type.Ore:
          // ISSUE: reference to a compiler-generated field
          if (WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache1 = new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
          }
          // ISSUE: reference to a compiler-generated field
          return WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache1;
        case Prefab.Type.Pickupable:
          // ISSUE: reference to a compiler-generated field
          if (WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache3 = new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
          }
          // ISSUE: reference to a compiler-generated field
          return WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache3;
        case Prefab.Type.Other:
          // ISSUE: reference to a compiler-generated field
          if (WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache2 = new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
          }
          // ISSUE: reference to a compiler-generated field
          return WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache2;
        default:
          // ISSUE: reference to a compiler-generated field
          if (WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache4 = new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
          }
          // ISSUE: reference to a compiler-generated field
          return WorldGenSpawner.Spawnable.\u003C\u003Ef__mg\u0024cache4;
      }
    }

    public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
  }
}
