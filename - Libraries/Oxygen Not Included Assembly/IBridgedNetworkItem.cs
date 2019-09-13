// Decompiled with JetBrains decompiler
// Type: IBridgedNetworkItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IBridgedNetworkItem
{
  void AddNetworks(ICollection<UtilityNetwork> networks);

  bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

  int GetNetworkCell();
}
