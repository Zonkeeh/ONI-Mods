// Decompiled with JetBrains decompiler
// Type: Unsealable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class Unsealable : Workable
{
  [Serialize]
  public bool facingRight;
  [Serialize]
  public bool unsealed;

  private Unsealable()
  {
  }

  public override CellOffset[] GetOffsets(int cell)
  {
    if (this.facingRight)
      return OffsetGroups.RightOnly;
    return OffsetGroups.LeftOnly;
  }

  protected override void OnPrefabInit()
  {
    this.faceTargetWhenWorking = true;
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_door_poi_kanim")
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(3f);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.unsealed = true;
    base.OnCompleteWork(worker);
    Deconstructable component = this.GetComponent<Deconstructable>();
    if (!((Object) component != (Object) null))
      return;
    component.allowDeconstruction = true;
    Game.Instance.Trigger(1980521255, (object) this.gameObject);
  }
}
