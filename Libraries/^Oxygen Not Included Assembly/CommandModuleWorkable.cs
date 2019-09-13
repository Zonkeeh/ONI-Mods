// Decompiled with JetBrains decompiler
// Type: CommandModuleWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CommandModuleWorkable : Workable
{
  private static CellOffset[] entryOffsets = new CellOffset[5]
  {
    new CellOffset(0, 0),
    new CellOffset(0, 1),
    new CellOffset(0, 2),
    new CellOffset(0, 3),
    new CellOffset(0, 4)
  };
  private static readonly EventSystem.IntraObjectHandler<CommandModuleWorkable> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CommandModuleWorkable>((System.Action<CommandModuleWorkable, object>) ((component, data) => component.OnLaunch(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsets(CommandModuleWorkable.entryOffsets);
    this.synchronizeAnims = false;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_incubator_kanim")
    };
    this.SetWorkTime(float.PositiveInfinity);
    this.showProgressBar = false;
    this.Subscribe<CommandModuleWorkable>(-1056989049, CommandModuleWorkable.OnLaunchDelegate);
  }

  private void OnLaunch(object data)
  {
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
      return base.OnWorkTick(worker, dt);
    GameObject gameObject = worker.gameObject;
    this.CompleteWork(worker);
    this.GetComponent<MinionStorage>().SerializeMinion(gameObject);
    return true;
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
  }

  protected override void OnCompleteWork(Worker worker)
  {
  }
}
