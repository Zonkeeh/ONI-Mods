// Decompiled with JetBrains decompiler
// Type: EffectorEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

internal struct EffectorEntry
{
  public string name;
  public int count;
  public float value;

  public EffectorEntry(string name, float value)
  {
    this.name = name;
    this.value = value;
    this.count = 1;
  }

  public override string ToString()
  {
    string str = string.Empty;
    if (this.count > 1)
      str = string.Format((string) UI.OVERLAYS.DECOR.COUNT, (object) this.count);
    return string.Format((string) UI.OVERLAYS.DECOR.ENTRY, (object) GameUtil.GetFormattedDecor(this.value, false), (object) this.name, (object) str);
  }
}
