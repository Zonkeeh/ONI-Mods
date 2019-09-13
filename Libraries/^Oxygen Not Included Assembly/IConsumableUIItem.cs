// Decompiled with JetBrains decompiler
// Type: IConsumableUIItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IConsumableUIItem
{
  string ConsumableId { get; }

  string ConsumableName { get; }

  int MajorOrder { get; }

  int MinorOrder { get; }

  bool Display { get; }
}
