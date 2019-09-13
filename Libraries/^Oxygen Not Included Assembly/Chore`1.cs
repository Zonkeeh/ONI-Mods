// Decompiled with JetBrains decompiler
// Type: Chore`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Chore<StateMachineInstanceType> : Chore, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
  public Chore(
    ChoreType chore_type,
    IStateMachineTarget target,
    ChoreProvider chore_provider,
    bool run_until_complete = true,
    System.Action<Chore> on_complete = null,
    System.Action<Chore> on_begin = null,
    System.Action<Chore> on_end = null,
    PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic,
    int master_priority_value = 5,
    bool is_preemptable = false,
    bool allow_in_context_menu = true,
    int priority_mod = 0,
    bool add_to_daily_report = false,
    ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime)
    : base(chore_type, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
  {
    this.target = target;
    target.Subscribe(1969584890, new System.Action<object>(this.OnTargetDestroyed));
    this.reportType = report_type;
    this.addToDailyReport = add_to_daily_report;
    if (!this.addToDailyReport)
      return;
    ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName((Chore) this, (object) null));
  }

  public StateMachineInstanceType smi { get; protected set; }

  protected override StateMachine.Instance GetSMI()
  {
    return (StateMachine.Instance) this.smi;
  }

  public int Subscribe(int hash, System.Action<object> handler)
  {
    return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
  }

  public void Unsubscribe(int hash, System.Action<object> handler)
  {
    this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
  }

  public void Unsubscribe(int id)
  {
    this.GetComponent<KPrefabID>().Unsubscribe(id);
  }

  public void Trigger(int hash, object data = null)
  {
    this.GetComponent<KPrefabID>().Trigger(hash, data);
  }

  public ComponentType GetComponent<ComponentType>()
  {
    return this.target.GetComponent<ComponentType>();
  }

  public override GameObject gameObject
  {
    get
    {
      return this.target.gameObject;
    }
  }

  public Transform transform
  {
    get
    {
      return this.target.gameObject.transform;
    }
  }

  public string name
  {
    get
    {
      return this.gameObject.name;
    }
  }

  public override bool isNull
  {
    get
    {
      return this.target.isNull;
    }
  }

  public override string ResolveString(string str)
  {
    if (!this.target.isNull)
      str = str.Replace("{Target}", this.target.gameObject.GetProperName());
    return base.ResolveString(str);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    if (this.target != null)
      this.target.Unsubscribe(1969584890, new System.Action<object>(this.OnTargetDestroyed));
    if (this.onCleanup == null)
      return;
    this.onCleanup((Chore) this);
  }

  private void OnTargetDestroyed(object data)
  {
    this.Cancel("Target Destroyed");
  }

  public override bool CanPreempt(Chore.Precondition.Context context)
  {
    return base.CanPreempt(context);
  }
}
