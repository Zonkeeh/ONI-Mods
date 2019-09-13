// Decompiled with JetBrains decompiler
// Type: Valve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Valve : Workable, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<Valve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Valve>((System.Action<Valve, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private float desiredFlow = 0.5f;
  [MyCmpReq]
  private ValveBase valveBase;
  private Chore chore;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  public float QueuedMaxFlow
  {
    get
    {
      if (this.chore != null)
        return this.desiredFlow;
      return -1f;
    }
  }

  public float DesiredFlow
  {
    get
    {
      return this.desiredFlow;
    }
  }

  public float MaxFlow
  {
    get
    {
      return this.valveBase.MaxFlow;
    }
  }

  private void OnCopySettings(object data)
  {
    Valve component = ((GameObject) data).GetComponent<Valve>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.desiredFlow = component.desiredFlow;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.synchronizeAnims = false;
    this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
    this.desiredFlow = this.valveBase.MaxFlow;
    this.Subscribe<Valve>(-905833192, Valve.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    this.ChangeFlow(this.desiredFlow);
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    Prioritizable.RemoveRef(this.gameObject);
    base.OnCleanUp();
  }

  public void ChangeFlow(float amount)
  {
    this.desiredFlow = Mathf.Clamp(amount, 0.0f, this.valveBase.MaxFlow);
    KSelectable component = this.GetComponent<KSelectable>();
    component.ToggleStatusItem(Db.Get().BuildingStatusItems.PumpingLiquidOrGas, (double) this.desiredFlow >= 0.0, (object) this.valveBase.AccumulatorHandle);
    if (DebugHandler.InstantBuildMode)
      this.UpdateFlow();
    else if ((double) this.desiredFlow != (double) this.valveBase.CurrentFlow)
    {
      if (this.chore != null)
        return;
      component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, (object) this);
      component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, (object) this);
      this.chore = (Chore) new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }
    else
    {
      if (this.chore != null)
      {
        this.chore.Cancel("desiredFlow == currentFlow");
        this.chore = (Chore) null;
      }
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
    }
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.UpdateFlow();
  }

  public void UpdateFlow()
  {
    this.valveBase.CurrentFlow = this.desiredFlow;
    this.valveBase.UpdateAnim();
    if (this.chore != null)
      this.chore.Cancel("forced complete");
    this.chore = (Chore) null;
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
  }
}
