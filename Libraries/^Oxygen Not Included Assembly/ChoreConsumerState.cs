// Decompiled with JetBrains decompiler
// Type: ChoreConsumerState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class ChoreConsumerState
{
  public KPrefabID prefabid;
  public GameObject gameObject;
  public ChoreConsumer consumer;
  public ChoreProvider choreProvider;
  public Navigator navigator;
  public Ownable ownable;
  public Assignables assignables;
  public MinionResume resume;
  public ChoreDriver choreDriver;
  public Schedulable schedulable;
  public Traits traits;
  public Equipment equipment;
  public Storage storage;
  public ConsumableConsumer consumableConsumer;
  public KSelectable selectable;
  public Worker worker;
  public SolidTransferArm solidTransferArm;
  public bool hasSolidTransferArm;
  public ScheduleBlock scheduleBlock;

  public ChoreConsumerState(ChoreConsumer consumer)
  {
    this.consumer = consumer;
    this.navigator = consumer.GetComponent<Navigator>();
    this.prefabid = consumer.GetComponent<KPrefabID>();
    this.ownable = consumer.GetComponent<Ownable>();
    this.gameObject = consumer.gameObject;
    this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
    this.hasSolidTransferArm = (Object) this.solidTransferArm != (Object) null;
    this.resume = consumer.GetComponent<MinionResume>();
    this.choreDriver = consumer.GetComponent<ChoreDriver>();
    this.schedulable = consumer.GetComponent<Schedulable>();
    this.traits = consumer.GetComponent<Traits>();
    this.choreProvider = consumer.GetComponent<ChoreProvider>();
    MinionIdentity component = consumer.GetComponent<MinionIdentity>();
    if ((Object) component != (Object) null)
    {
      if (component.assignableProxy == null)
        component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, (IAssignableIdentity) component);
      this.assignables = (Assignables) component.GetSoleOwner();
      this.equipment = component.GetEquipment();
    }
    else
    {
      this.assignables = consumer.GetComponent<Assignables>();
      this.equipment = consumer.GetComponent<Equipment>();
    }
    this.storage = consumer.GetComponent<Storage>();
    this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
    this.worker = consumer.GetComponent<Worker>();
    this.selectable = consumer.GetComponent<KSelectable>();
    if (!((Object) this.schedulable != (Object) null))
      return;
    this.scheduleBlock = this.schedulable.GetSchedule().GetBlock(Schedule.GetBlockIdx());
  }

  public void Refresh()
  {
    if (!((Object) this.schedulable != (Object) null))
      return;
    this.scheduleBlock = this.schedulable.GetSchedule().GetBlock(Schedule.GetBlockIdx());
  }
}
