// Decompiled with JetBrains decompiler
// Type: CategoryEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CategoryEntry : CodexEntry
{
  public List<CodexEntry> entriesInCategory = new List<CodexEntry>();

  public CategoryEntry(
    string category,
    List<ContentContainer> contentContainers,
    string name,
    List<CodexEntry> entriesInCategory,
    bool largeFormat,
    bool sort)
    : base(category, contentContainers, name)
  {
    this.entriesInCategory = entriesInCategory;
    this.largeFormat = largeFormat;
    this.sort = sort;
  }

  public bool largeFormat { get; set; }

  public bool sort { get; set; }
}
