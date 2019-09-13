// Decompiled with JetBrains decompiler
// Type: VideoOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class VideoOverlay : KMonoBehaviour
{
  public List<LocText> textFields;

  public void SetText(List<string> strings)
  {
    if (strings.Count != this.textFields.Count)
      DebugUtil.LogErrorArgs((object) this.name, (object) "expects", (object) this.textFields.Count, (object) "strings passed to it, got", (object) strings.Count);
    for (int index = 0; index < this.textFields.Count; ++index)
      this.textFields[index].text = strings[index];
  }
}
