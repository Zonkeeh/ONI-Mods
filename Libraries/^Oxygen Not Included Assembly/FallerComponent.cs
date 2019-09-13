// Decompiled with JetBrains decompiler
// Type: FallerComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct FallerComponent
{
  public Transform transform;
  public int transformInstanceId;
  public bool isFalling;
  public float offset;
  public Vector2 initialVelocity;
  public HandleVector<int>.Handle partitionerEntry;
  public System.Action<object> solidChangedCB;
  public System.Action cellChangedCB;

  public FallerComponent(Transform transform, Vector2 initial_velocity)
  {
    this.transform = transform;
    this.transformInstanceId = transform.GetInstanceID();
    this.isFalling = false;
    this.initialVelocity = initial_velocity;
    this.partitionerEntry = new HandleVector<int>.Handle();
    this.solidChangedCB = (System.Action<object>) null;
    this.cellChangedCB = (System.Action) null;
    KCircleCollider2D component1 = transform.GetComponent<KCircleCollider2D>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      this.offset = component1.radius;
    }
    else
    {
      KCollider2D component2 = transform.GetComponent<KCollider2D>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        this.offset = transform.GetPosition().y - component2.bounds.min.y;
      else
        this.offset = 0.0f;
    }
  }
}
