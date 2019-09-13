// Decompiled with JetBrains decompiler
// Type: UtilityNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class UtilityNetwork
{
  public int id;
  public ConduitType conduitType;

  public virtual void AddItem(int cell, object item)
  {
  }

  public virtual void RemoveItem(int cell, object item)
  {
  }

  public virtual void ConnectItem(int cell, object item)
  {
  }

  public virtual void DisconnectItem(int cell, object item)
  {
  }

  public virtual void Reset(UtilityNetworkGridNode[] grid)
  {
  }
}
