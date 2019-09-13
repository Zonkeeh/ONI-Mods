// Decompiled with JetBrains decompiler
// Type: TemplateClasses.Prefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace TemplateClasses
{
  [Serializable]
  public class Prefab : ICloneable
  {
    public Prefab()
    {
      this.rottable = new Rottable();
      this.storage = new List<StorageItem>();
      this.type = Prefab.Type.Other;
    }

    public Prefab(
      string _id,
      Prefab.Type _type,
      int loc_x,
      int loc_y,
      SimHashes _element,
      float _temperature = -1f,
      float _units = 1f,
      string _disease = null,
      int _disease_count = 0,
      Orientation _rotation = Orientation.Neutral,
      Prefab.template_amount_value[] _amount_values = null,
      Prefab.template_amount_value[] _other_values = null,
      int _connections = 0)
    {
      this.rottable = new Rottable();
      this.storage = new List<StorageItem>();
      this.id = _id;
      this.type = _type;
      this.location_x = loc_x;
      this.location_y = loc_y;
      this.connections = _connections;
      this.element = _element;
      this.temperature = _temperature;
      this.units = _units;
      this.diseaseName = _disease;
      this.diseaseCount = _disease_count;
      this.rotationOrientation = _rotation;
      this.amounts = _amount_values;
      this.other_values = _other_values;
    }

    public object Clone()
    {
      return this.Clone(Vector2I.zero);
    }

    public object Clone(Vector2I offset)
    {
      Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections);
      prefab.rottable.rotAmount = this.rottable.rotAmount;
      prefab.storage = new List<StorageItem>();
      foreach (StorageItem storageItem in this.storage)
        prefab.storage.Add((StorageItem) storageItem.Clone());
      return (object) prefab;
    }

    public object Clone(int offset_x, int offset_y)
    {
      Prefab prefab = (Prefab) this.Clone();
      prefab.location_x += offset_x;
      prefab.location_y += offset_y;
      return (object) prefab;
    }

    public void AssignStorage(StorageItem _storage)
    {
      if (this.storage == null)
        this.storage = new List<StorageItem>();
      this.storage.Add(_storage);
    }

    public string id { get; set; }

    public int location_x { get; set; }

    public int location_y { get; set; }

    public SimHashes element { get; set; }

    public float temperature { get; set; }

    public float units { get; set; }

    public string diseaseName { get; set; }

    public int diseaseCount { get; set; }

    public Orientation rotationOrientation { get; set; }

    public List<StorageItem> storage { get; set; }

    public Prefab.Type type { get; set; }

    public int connections { get; set; }

    public Rottable rottable { get; set; }

    public Prefab.template_amount_value[] amounts { get; set; }

    public Prefab.template_amount_value[] other_values { get; set; }

    public enum Type
    {
      Building,
      Ore,
      Pickupable,
      Other,
    }

    [Serializable]
    public class template_amount_value
    {
      public template_amount_value()
      {
      }

      public template_amount_value(string id, float value)
      {
        this.id = id;
        this.value = value;
      }

      public string id { get; set; }

      public float value { get; set; }
    }
  }
}
