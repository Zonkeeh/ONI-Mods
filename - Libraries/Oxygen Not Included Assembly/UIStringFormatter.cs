// Decompiled with JetBrains decompiler
// Type: UIStringFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class UIStringFormatter
{
  private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();
  private int activeStringCount;

  public string Format(string format, string s0)
  {
    return this.Replace(format, "{0}", s0);
  }

  public string Format(string format, string s0, string s1)
  {
    return this.Replace(this.Replace(format, "{0}", s0), "{1}", s1);
  }

  private string Replace(string format, string key, string value)
  {
    UIStringFormatter.Entry entry = new UIStringFormatter.Entry();
    if (this.activeStringCount >= this.entries.Count)
    {
      entry.format = format;
      entry.key = key;
      entry.value = value;
      entry.result = entry.format.Replace(key, value);
      this.entries.Add(entry);
    }
    else
    {
      entry = this.entries[this.activeStringCount];
      if (entry.format != format || entry.key != key || entry.value != value)
      {
        entry.format = format;
        entry.key = key;
        entry.value = value;
        entry.result = entry.format.Replace(key, value);
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
    public string value;
    public string result;
  }
}
