// Decompiled with JetBrains decompiler
// Type: EnergyConsumerSelfSustaining
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Diagnostics;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
  private bool isSustained;
  private CircuitManager.ConnectionStatus connectionStatus;

  public event System.Action OnConnectionChanged;

  public override bool IsPowered
  {
    get
    {
      if (this.isSustained)
        return true;
      return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
    }
  }

  public bool IsExternallyPowered
  {
    get
    {
      return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
    }
  }

  public void SetSustained(bool isSustained)
  {
    this.isSustained = isSustained;
  }

  public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
  {
    CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
    switch (connection_status)
    {
      case CircuitManager.ConnectionStatus.NotConnected:
        this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
        break;
      case CircuitManager.ConnectionStatus.Unpowered:
        if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && (UnityEngine.Object) this.GetComponent<Battery>() == (UnityEngine.Object) null)
        {
          this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
          break;
        }
        break;
      case CircuitManager.ConnectionStatus.Powered:
        if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
        {
          this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
          break;
        }
        break;
    }
    this.UpdatePoweredStatus();
    if (connectionStatus == this.connectionStatus || this.OnConnectionChanged == null)
      return;
    this.OnConnectionChanged();
  }

  public void UpdatePoweredStatus()
  {
    this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
  }
}
