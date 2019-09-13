// Decompiled with JetBrains decompiler
// Type: ContentContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization.Converters;
using System.Collections.Generic;

public class ContentContainer
{
  public ContentContainer()
  {
    this.content = new List<ICodexWidget>();
  }

  public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
  {
    this.content = content;
    this.contentLayout = contentLayout;
  }

  public List<ICodexWidget> content { get; set; }

  public string lockID { get; set; }

  [StringEnumConverter]
  public ContentContainer.ContentLayout contentLayout { get; set; }

  public bool showBeforeGeneratedContent { get; set; }

  public enum ContentLayout
  {
    Vertical,
    Horizontal,
    Grid,
  }
}
