// Decompiled with JetBrains decompiler
// Type: EffectorEntryDecibel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal struct EffectorEntryDecibel
{
  public string name;
  public int count;
  public float value;

  public EffectorEntryDecibel(string name, float value)
  {
    this.name = name;
    this.value = value;
    this.count = 1;
  }
}
