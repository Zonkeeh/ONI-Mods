// Decompiled with JetBrains decompiler
// Type: RoomTypeCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RoomTypeCategory : Resource
{
  public RoomTypeCategory(string id, string name, Color color)
    : base(id, name)
  {
    this.color = color;
  }

  public Color color { get; private set; }
}
