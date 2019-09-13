// Decompiled with JetBrains decompiler
// Type: SelectMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SelectMarker : KMonoBehaviour
{
  public float animationOffset = 0.1f;
  private Transform targetTransform;

  public void SetTargetTransform(Transform target_transform)
  {
    this.targetTransform = target_transform;
    this.LateUpdate();
  }

  private void LateUpdate()
  {
    if ((Object) this.targetTransform == (Object) null)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      Vector3 position = this.targetTransform.GetPosition();
      KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
      if ((Object) component != (Object) null)
      {
        position.x = component.bounds.center.x;
        position.y = (float) ((double) component.bounds.center.y + (double) component.bounds.size.y / 2.0 + 0.100000001490116);
      }
      else
        position.y += 2f;
      Vector3 vector3 = new Vector3(0.0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0.0f);
      this.transform.SetPosition(position + vector3);
    }
  }
}
