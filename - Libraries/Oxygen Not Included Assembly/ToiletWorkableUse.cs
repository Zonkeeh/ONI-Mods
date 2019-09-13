// Decompiled with JetBrains decompiler
// Type: ToiletWorkableUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using UnityEngine;

public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
  [Serialize]
  public int timesUsed;

  private ToiletWorkableUse()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
    this.SetWorkTime(8.5f);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject)?.roomType.TriggerRoomEffects(this.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
  }

  protected override void OnCompleteWork(Worker worker)
  {
    double num = (double) Db.Get().Amounts.Bladder.Lookup((Component) worker).SetValue(0.0f);
    ++this.timesUsed;
    this.Trigger(-350347868, (object) worker);
    base.OnCompleteWork(worker);
  }
}
