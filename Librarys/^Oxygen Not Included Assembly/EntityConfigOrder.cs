// Decompiled with JetBrains decompiler
// Type: EntityConfigOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

internal class EntityConfigOrder : Attribute
{
  public int sortOrder;

  public EntityConfigOrder(int sort_order)
  {
    this.sortOrder = sort_order;
  }
}
