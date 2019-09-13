// Decompiled with JetBrains decompiler
// Type: GravityComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct GravityComponent
{
  public Transform transform;
  public Vector2 velocity;
  public float radius;
  public float elapsedTime;
  public System.Action onLanded;
  public bool landOnFakeFloors;

  public GravityComponent(
    Transform transform,
    System.Action on_landed,
    Vector2 initial_velocity,
    bool land_on_fake_floors)
  {
    this.transform = transform;
    this.elapsedTime = 0.0f;
    this.velocity = initial_velocity;
    this.onLanded = on_landed;
    this.radius = GravityComponent.GetRadius(transform);
    this.landOnFakeFloors = land_on_fake_floors;
  }

  public static float GetRadius(Transform transform)
  {
    KCircleCollider2D component1 = transform.GetComponent<KCircleCollider2D>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      return component1.radius;
    KCollider2D component2 = transform.GetComponent<KCollider2D>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      return transform.GetPosition().y - component2.bounds.min.y;
    return 0.0f;
  }
}
