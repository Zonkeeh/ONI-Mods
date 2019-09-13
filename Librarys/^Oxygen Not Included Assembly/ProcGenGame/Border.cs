// Decompiled with JetBrains decompiler
// Type: ProcGenGame.Border
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGenGame
{
  public class Border : ProcGen.Path, SymbolicMapElement
  {
    public Neighbors neighbors;
    public List<WeightedSimHash> element;
    public float width;

    public Border(Neighbors neighbors, Vector2 e0, Vector2 e1)
    {
      this.neighbors = neighbors;
      this.AddSegment(e0, e1);
    }

    public Border(TerrainCell a, TerrainCell b, Vector2 e0, Vector2 e1)
    {
      Debug.Assert(a != null && b != null, (object) "NULL neighbor for Border");
      this.neighbors.n0 = a;
      this.neighbors.n1 = b;
      this.AddSegment(e0, e1);
    }

    public void ConvertToMap(
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
      for (int index1 = 0; index1 < this.pathElements.Count; ++index1)
      {
        Vector2 vector2_1 = this.pathElements[index1].e1 - this.pathElements[index1].e0;
        Vector2 normalized = new Vector2(-vector2_1.y, vector2_1.x).normalized;
        List<Vector2I> line = ProcGen.Util.GetLine(this.pathElements[index1].e0, this.pathElements[index1].e1);
        for (int index2 = 0; index2 < line.Count; ++index2)
        {
          int cell1 = Grid.XYToCell(line[index2].x, line[index2].y);
          if (Grid.IsValidCell(cell1))
          {
            Element elementByName = ElementLoader.FindElementByName(WeightedRandom.Choose<WeightedSimHash>(this.element, rnd).element);
            Sim.PhysicsData defaultValues = elementByName.defaultValues;
            defaultValues.temperature = temperatureMin + world.heatOffset[cell1] * temperatureRange;
            SetValues(cell1, (object) elementByName, defaultValues, invalid);
          }
          for (float num = 0.5f; (double) num <= (double) this.width; ++num)
          {
            Vector2 vector2_2 = (Vector2) line[index2] + normalized * num;
            int cell2 = Grid.XYToCell((int) vector2_2.x, (int) vector2_2.y);
            if (Grid.IsValidCell(cell2))
            {
              Element elementByName = ElementLoader.FindElementByName(WeightedRandom.Choose<WeightedSimHash>(this.element, rnd).element);
              Sim.PhysicsData defaultValues = elementByName.defaultValues;
              defaultValues.temperature = temperatureMin + world.heatOffset[cell2] * temperatureRange;
              SetValues(cell2, (object) elementByName, defaultValues, invalid);
            }
            Vector2 vector2_3 = (Vector2) line[index2] - normalized * num;
            int cell3 = Grid.XYToCell((int) vector2_3.x, (int) vector2_3.y);
            if (Grid.IsValidCell(cell3))
            {
              Element elementByName = ElementLoader.FindElementByName(WeightedRandom.Choose<WeightedSimHash>(this.element, rnd).element);
              Sim.PhysicsData defaultValues = elementByName.defaultValues;
              defaultValues.temperature = temperatureMin + world.heatOffset[cell3] * temperatureRange;
              SetValues(cell3, (object) elementByName, defaultValues, invalid);
            }
          }
        }
      }
    }
  }
}
