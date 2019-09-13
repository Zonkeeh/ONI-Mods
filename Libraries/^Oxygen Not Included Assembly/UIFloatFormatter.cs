// Decompiled with JetBrains decompiler
// Type: UIFloatFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class UIFloatFormatter
{
  private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();
  private int activeStringCount;

  public string Format(string format, float value)
  {
    return this.Replace(format, "{0}", value);
  }

  private string Replace(string format, string key, float value)
  {
    UIFloatFormatter.Entry entry = new UIFloatFormatter.Entry();
    if (this.activeStringCount >= this.entries.Count)
    {
      entry.format = format;
      entry.key = key;
      entry.value = value;
      entry.result = entry.format.Replace(key, value.ToString());
      this.entries.Add(entry);
    }
    else
    {
      entry = this.entries[this.activeStringCount];
      if (entry.format != format || entry.key != key || (double) entry.value != (double) value)
      {
        entry.format = format;
        entry.key = key;
        entry.value = value;
        entry.result = entry.format.Replace(key, value.ToString());
        this.entries[this.activeStringCount] = entry;
      }
    }
    ++this.activeStringCount;
    return entry.result;
  }

  public void BeginDrawing()
  {
    this.activeStringCount = 0;
  }

  public void EndDrawing()
  {
  }

  private struct Entry
  {
    public string format;
    public string key;
    public float value;
    public string result;
  }
}
