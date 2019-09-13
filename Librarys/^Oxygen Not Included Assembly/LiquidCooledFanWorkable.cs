// Decompiled with JetBrains decompiler
// Type: LiquidCooledFanWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class LiquidCooledFanWorkable : Workable
{
  [MyCmpGet]
  private Operational operational;

  private LiquidCooledFanWorkable()
  {
    this.showProgressBar = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = (StatusItem) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  protected override void OnStartWork(Worker worker)
  {
    this.operational.SetActive(true, false);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.operational.SetActive(false, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.operational.SetActive(false, false);
  }
}
