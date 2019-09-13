// Decompiled with JetBrains decompiler
// Type: CodexEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class CodexEntry
{
  public List<SubEntry> subEntries = new List<SubEntry>();
  public Color iconColor = Color.white;
  public bool searchOnly;
  public int customContentLength;

  public CodexEntry()
  {
  }

  public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
  {
    this.category = category;
    this.name = name;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    this.sortString = UI.StripLinkFormatting(name);
  }

  public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
  {
    this.category = category;
    this.title = titleKey;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    this.sortString = UI.StripLinkFormatting(this.title);
  }

  public List<ContentContainer> contentContainers { get; set; }

  public ICodexWidget GetFirstWidget()
  {
    for (int index1 = 0; index1 < this.contentContainers.Count; ++index1)
    {
      if (this.contentContainers[index1].content != null)
      {
        for (int index2 = 0; index2 < this.contentContainers[index1].content.Count; ++index2)
        {
          if (this.contentContainers[index1].content[index2] != null)
            return this.contentContainers[index1].content[index2];
        }
      }
    }
    return (ICodexWidget) null;
  }

  public string id { get; set; }

  public string parentId { get; set; }

  public string category { get; set; }

  public string title { get; set; }

  public string name { get; set; }

  public string subtitle { get; set; }

  public Sprite icon { get; set; }

  public string iconPrefabID { get; set; }

  public bool disabled { get; set; }

  public string sortString { get; set; }
}
