// Decompiled with JetBrains decompiler
// Type: KAnimSynchronizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class KAnimSynchronizer
{
  private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();
  private KAnimControllerBase masterController;

  public KAnimSynchronizer(KAnimControllerBase master_controller)
  {
    this.masterController = master_controller;
  }

  private void Clear(KAnimControllerBase controller)
  {
    controller.Play((HashedString) "idle_default", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public void Add(KAnimControllerBase controller)
  {
    this.Targets.Add(controller);
  }

  public void Remove(KAnimControllerBase controller)
  {
    this.Clear(controller);
    this.Targets.Remove(controller);
  }

  public void Clear()
  {
    foreach (KAnimControllerBase target in this.Targets)
      this.Clear(target);
    this.Targets.Clear();
  }

  public void Sync(KAnimControllerBase controller)
  {
    if ((Object) this.masterController == (Object) null || (Object) controller == (Object) null)
      return;
    KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
    if (currentAnim == null)
      return;
    KAnim.PlayMode mode = this.masterController.GetMode();
    float playSpeed = this.masterController.GetPlaySpeed();
    float elapsedTime = this.masterController.GetElapsedTime();
    controller.Play((HashedString) currentAnim.name, mode, playSpeed, elapsedTime);
    Facing component = controller.GetComponent<Facing>();
    if ((Object) component != (Object) null)
    {
      float target_x = component.transform.GetPosition().x + (!this.masterController.FlipX ? 0.5f : -0.5f);
      component.Face(target_x);
    }
    else
    {
      controller.FlipX = this.masterController.FlipX;
      controller.FlipY = this.masterController.FlipY;
    }
  }

  public void Sync()
  {
    for (int index = 0; index < this.Targets.Count; ++index)
      this.Sync(this.Targets[index]);
  }

  public void SyncTime()
  {
    float elapsedTime = this.masterController.GetElapsedTime();
    for (int index = 0; index < this.Targets.Count; ++index)
      this.Targets[index].SetElapsedTime(elapsedTime);
  }
}
