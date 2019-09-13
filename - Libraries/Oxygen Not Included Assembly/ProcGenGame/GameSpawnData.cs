// Decompiled with JetBrains decompiler
// Type: ProcGenGame.GameSpawnData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using TemplateClasses;

namespace ProcGenGame
{
  [SerializationConfig(MemberSerialization.OptOut)]
  public class GameSpawnData
  {
    public List<Prefab> buildings = new List<Prefab>();
    public List<Prefab> pickupables = new List<Prefab>();
    public List<Prefab> elementalOres = new List<Prefab>();
    public List<Prefab> otherEntities = new List<Prefab>();
    public List<KeyValuePair<Vector2I, bool>> preventFoWReveal = new List<KeyValuePair<Vector2I, bool>>();
    public Vector2I baseStartPos;

    public void AddRange(IEnumerable<KeyValuePair<int, string>> newItems)
    {
      foreach (KeyValuePair<int, string> newItem in newItems)
      {
        this.ClearTemplatesInCell(newItem.Key);
        Vector2I xy = Grid.CellToXY(newItem.Key);
        this.otherEntities.Add(new Prefab(newItem.Value, Prefab.Type.Other, xy.x, xy.y, (SimHashes) 0, -1f, 1f, (string) null, 0, Orientation.Neutral, (Prefab.template_amount_value[]) null, (Prefab.template_amount_value[]) null, 0));
      }
    }

    public void ClearTemplatesInArea(int root_cell, CellOffset[] area)
    {
      foreach (CellOffset offset in area)
        this.ClearTemplatesInCell(Grid.OffsetCell(root_cell, offset));
    }

    public void ClearTemplatesInCell(int cell)
    {
      this.ClearCellFromCollection(cell, this.buildings);
      this.ClearCellFromCollection(cell, this.pickupables);
      this.ClearCellFromCollection(cell, this.elementalOres);
      this.ClearCellFromCollection(cell, this.otherEntities);
      for (int index = 0; index < this.preventFoWReveal.Count; ++index)
      {
        if (this.preventFoWReveal[index].Key == Grid.CellToXY(cell))
        {
          this.preventFoWReveal.RemoveAt(index);
          --index;
        }
      }
    }

    private void ClearCellFromCollection(int checkCell, List<Prefab> collection)
    {
      for (int index = 0; index < collection.Count; ++index)
      {
        if (checkCell == Grid.XYToCell(collection[index].location_x, collection[index].location_y))
        {
          collection.RemoveAt(index);
          --index;
        }
      }
    }

    public void AddTemplate(TemplateContainer template, Vector2I position)
    {
      CellOffset[] area = new CellOffset[template.cells.Count];
      for (int index = 0; index < template.cells.Count; ++index)
        area[index] = new CellOffset(template.cells[index].location_x, template.cells[index].location_y);
      this.ClearTemplatesInArea(Grid.XYToCell(position.x, position.y), area);
      for (int index = 0; index < template.buildings.Count; ++index)
        this.buildings.Add((Prefab) template.buildings[index].Clone(position));
      for (int index = 0; index < template.pickupables.Count; ++index)
        this.pickupables.Add((Prefab) template.pickupables[index].Clone(position));
      for (int index = 0; index < template.elementalOres.Count; ++index)
        this.elementalOres.Add((Prefab) template.elementalOres[index].Clone(position));
      for (int index = 0; index < template.otherEntities.Count; ++index)
        this.otherEntities.Add((Prefab) template.otherEntities[index].Clone(position));
      for (int index = 0; index < template.cells.Count; ++index)
        this.preventFoWReveal.Add(new KeyValuePair<Vector2I, bool>(new Vector2I(position.x + template.cells[index].location_x, position.y + template.cells[index].location_y), template.cells[index].preventFoWReveal));
    }
  }
}
