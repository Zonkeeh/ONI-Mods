// Decompiled with JetBrains decompiler
// Type: TemplateClasses.Cell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace TemplateClasses
{
  [Serializable]
  public class Cell : ICloneable
  {
    public Cell()
    {
    }

    public Cell(int loc_x, int loc_y)
    {
      this.location_x = loc_x;
      this.location_y = loc_y;
      this.element = SimHashes.Oxygen;
      this.temperature = SaveGame.Instance.worldGen.Settings.GetFloatSetting("StartAreaTemperatureOffset");
      this.mass = SaveGame.Instance.worldGen.Settings.GetFloatSetting("StartAreaPressureMultiplier");
      this.diseaseName = (string) null;
      this.diseaseCount = 0;
    }

    public Cell(
      int loc_x,
      int loc_y,
      SimHashes _element,
      float _temperature,
      float _mass,
      string _diseaseName,
      int _diseaseCount,
      bool _preventFoWReveal = false)
    {
      this.location_x = loc_x;
      this.location_y = loc_y;
      this.element = _element;
      this.temperature = _temperature;
      this.mass = _mass;
      this.diseaseName = _diseaseName;
      this.diseaseCount = _diseaseCount;
      this.preventFoWReveal = _preventFoWReveal;
    }

    public object Clone()
    {
      return (object) new Cell(this.location_x, this.location_y, this.element, this.temperature, this.mass, this.diseaseName, this.diseaseCount, this.preventFoWReveal);
    }

    public object Clone(int offset_x, int offset_y)
    {
      Cell cell = (Cell) this.Clone();
      cell.location_x += offset_x;
      cell.location_y += offset_y;
      return (object) cell;
    }

    public SimHashes element { get; set; }

    public float mass { get; set; }

    public float temperature { get; set; }

    public string diseaseName { get; set; }

    public int diseaseCount { get; set; }

    public int location_x { get; set; }

    public int location_y { get; set; }

    public bool preventFoWReveal { get; set; }
  }
}
