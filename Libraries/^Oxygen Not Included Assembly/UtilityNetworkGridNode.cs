// Decompiled with JetBrains decompiler
// Type: UtilityNetworkGridNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
  public UtilityConnections connections;
  public int networkIdx;
  public const int InvalidNetworkIdx = -1;

  public bool Equals(UtilityNetworkGridNode other)
  {
    if (this.connections == other.connections)
      return this.networkIdx == other.networkIdx;
    return false;
  }

  public override bool Equals(object obj)
  {
    return ((UtilityNetworkGridNode) obj).Equals(this);
  }

  public override int GetHashCode()
  {
    return base.GetHashCode();
  }

  public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
  {
    return x.Equals(y);
  }

  public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
  {
    return !x.Equals(y);
  }
}
