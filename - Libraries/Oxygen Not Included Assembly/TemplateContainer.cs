// Decompiled with JetBrains decompiler
// Type: TemplateContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using TemplateClasses;
using UnityEngine;

[Serializable]
public class TemplateContainer
{
  public TemplateContainer()
  {
    this.Init();
  }

  public string name { get; private set; }

  public int priority { get; set; }

  public TemplateContainer.Info info { get; set; }

  public List<TemplateClasses.Cell> cells { get; set; }

  public List<Prefab> buildings { get; set; }

  public List<Prefab> pickupables { get; set; }

  public List<Prefab> elementalOres { get; set; }

  public List<Prefab> otherEntities { get; set; }

  public void Init(
    List<TemplateClasses.Cell> _cells,
    List<Prefab> _buildings,
    List<Prefab> _pickupables,
    List<Prefab> _elementalOres,
    List<Prefab> _otherEntities)
  {
    this.cells = _cells;
    this.buildings = _buildings;
    this.pickupables = _pickupables;
    this.elementalOres = _elementalOres;
    this.otherEntities = _otherEntities;
    this.info = new TemplateContainer.Info();
    this.RefreshInfo();
  }

  public void Init()
  {
    this.cells = new List<TemplateClasses.Cell>();
    this.buildings = new List<Prefab>();
    this.pickupables = new List<Prefab>();
    this.elementalOres = new List<Prefab>();
    this.otherEntities = new List<Prefab>();
    this.info = new TemplateContainer.Info();
  }

  public void Init(TemplateContainer template)
  {
    this.cells = new List<TemplateClasses.Cell>((IEnumerable<TemplateClasses.Cell>) template.cells);
    this.buildings = new List<Prefab>((IEnumerable<Prefab>) template.buildings);
    this.pickupables = new List<Prefab>((IEnumerable<Prefab>) template.pickupables);
    this.elementalOres = new List<Prefab>((IEnumerable<Prefab>) template.elementalOres);
    this.otherEntities = new List<Prefab>((IEnumerable<Prefab>) template.otherEntities);
    this.info = new TemplateContainer.Info();
    this.info.size = template.info.size;
    this.info.area = template.info.area;
    this.info.tags = (Tag[]) template.info.tags.Clone();
  }

  public void RefreshInfo()
  {
    int num1 = 1;
    int num2 = -1;
    int num3 = 1;
    int num4 = -1;
    foreach (TemplateClasses.Cell cell in this.cells)
    {
      if (cell.location_x < num1)
        num1 = cell.location_x;
      if (cell.location_x > num2)
        num2 = cell.location_x;
      if (cell.location_y < num3)
        num3 = cell.location_y;
      if (cell.location_y > num4)
        num4 = cell.location_y;
      this.info.size = (Vector2f) new Vector2((float) (1 + (num2 - num1)), (float) (1 + (num4 - num3)));
      this.info.area = this.cells.Count;
    }
  }

  public void SaveToYaml(string save_name)
  {
    string str = save_name;
    while (str.Contains("/"))
    {
      int startIndex = str.IndexOf('/') + 1;
      if (str.Length > startIndex)
        str = str.Substring(startIndex);
    }
    this.name = str;
    string templatePath = TemplateCache.GetTemplatePath();
    if (!File.Exists(templatePath))
      Directory.CreateDirectory(templatePath);
    YamlIO.Save<TemplateContainer>(this, templatePath + "/" + save_name + ".yaml", (List<Tuple<string, System.Type>>) null);
  }

  [Serializable]
  public class Info
  {
    public Vector2f size { get; set; }

    public int area { get; set; }

    public Tag[] tags { get; set; }
  }
}
