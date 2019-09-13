// Decompiled with JetBrains decompiler
// Type: IEnergyConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IEnergyConsumer
{
  float WattsUsed { get; }

  float WattsNeededWhenActive { get; }

  int PowerSortOrder { get; }

  void SetConnectionStatus(CircuitManager.ConnectionStatus status);

  string Name { get; }

  int PowerCell { get; }

  bool IsConnected { get; }

  bool IsPowered { get; }
}
