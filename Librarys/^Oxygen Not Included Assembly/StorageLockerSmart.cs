// Decompiled with JetBrains decompiler
// Type: StorageLockerSmart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class StorageLockerSmart : StorageLocker
{
  private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>((System.Action<StorageLockerSmart, object>) ((component, data) => component.UpdateLogicCircuitCB(data)));
  [MyCmpGet]
  private LogicPorts ports;
  [MyCmpGet]
  private Operational operational;

  protected override void OnPrefabInit()
  {
    this.Initialize(true);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ports = this.gameObject.GetComponent<LogicPorts>();
    this.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
    this.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
    this.UpdateLogicAndActiveState();
  }

  private void UpdateLogicCircuitCB(object data)
  {
    this.UpdateLogicAndActiveState();
  }

  private void UpdateLogicAndActiveState()
  {
    bool flag = this.filteredStorage.IsFull();
    bool isOperational = this.operational.IsOperational;
    bool on = flag && isOperational;
    this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, !on ? 0 : 1);
    this.filteredStorage.SetLogicMeter(on);
    this.operational.SetActive(isOperational, false);
  }

  public override float UserMaxCapacity
  {
    get
    {
      return base.UserMaxCapacity;
    }
    set
    {
      base.UserMaxCapacity = value;
      this.UpdateLogicAndActiveState();
    }
  }
}
