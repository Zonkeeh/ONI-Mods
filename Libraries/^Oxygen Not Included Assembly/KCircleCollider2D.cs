// Decompiled with JetBrains decompiler
// Type: KCircleCollider2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KCircleCollider2D : KCollider2D
{
  [SerializeField]
  private float _radius;

  public float radius
  {
    get
    {
      return this._radius;
    }
    set
    {
      this._radius = value;
      this.MarkDirty(false);
    }
  }

  public override Extents GetExtents()
  {
    Vector3 vector3 = this.transform.GetPosition() + new Vector3(this.offset.x, this.offset.y, 0.0f);
    Vector2 vector2_1 = new Vector2(vector3.x - this.radius, vector3.y - this.radius);
    Vector2 vector2_2 = new Vector2(vector3.x + this.radius, vector3.y + this.radius);
    int width = (int) vector2_2.x - (int) vector2_1.x + 1;
    int height = (int) vector2_2.y - (int) vector2_1.y + 1;
    return new Extents((int) ((double) vector3.x - (double) this._radius), (int) ((double) vector3.y - (double) this._radius), width, height);
  }

  public override Bounds bounds
  {
    get
    {
      return new Bounds(this.transform.GetPosition() + new Vector3(this.offset.x, this.offset.y, 0.0f), new Vector3(this._radius, this._radius, 0.0f));
    }
  }

  public override bool Intersects(Vector2 pos)
  {
    Vector3 position = this.transform.GetPosition();
    Vector2 vector2 = new Vector2(position.x, position.y) + this.offset;
    return (double) (pos - vector2).sqrMagnitude <= (double) this._radius * (double) this._radius;
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(this.bounds.center, this.radius);
  }
}
