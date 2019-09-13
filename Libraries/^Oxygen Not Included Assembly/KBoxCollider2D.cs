// Decompiled with JetBrains decompiler
// Type: KBoxCollider2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KBoxCollider2D : KCollider2D
{
  [SerializeField]
  private Vector2 _size;

  public Vector2 size
  {
    get
    {
      return this._size;
    }
    set
    {
      this._size = value;
      this.MarkDirty(false);
    }
  }

  public override Extents GetExtents()
  {
    Vector3 vector3 = this.transform.GetPosition() + new Vector3(this.offset.x, this.offset.y, 0.0f);
    Vector2 vector2_1 = this.size * 0.9999f;
    Vector2 vector2_2 = new Vector2(vector3.x - vector2_1.x * 0.5f, vector3.y - vector2_1.y * 0.5f);
    Vector2 vector2_3 = new Vector2(vector3.x + vector2_1.x * 0.5f, vector3.y + vector2_1.y * 0.5f);
    Vector2I vector2I1 = new Vector2I((int) vector2_2.x, (int) vector2_2.y);
    Vector2I vector2I2 = new Vector2I((int) vector2_3.x, (int) vector2_3.y);
    int width = vector2I2.x - vector2I1.x + 1;
    int height = vector2I2.y - vector2I1.y + 1;
    return new Extents(vector2I1.x, vector2I1.y, width, height);
  }

  public override bool Intersects(Vector2 intersect_pos)
  {
    Vector3 vector3 = this.transform.GetPosition() + new Vector3(this.offset.x, this.offset.y, 0.0f);
    Vector2 vector2_1 = new Vector2(vector3.x - this.size.x * 0.5f, vector3.y - this.size.y * 0.5f);
    Vector2 vector2_2 = new Vector2(vector3.x + this.size.x * 0.5f, vector3.y + this.size.y * 0.5f);
    if ((double) intersect_pos.x >= (double) vector2_1.x && (double) intersect_pos.x <= (double) vector2_2.x && (double) intersect_pos.y >= (double) vector2_1.y)
      return (double) intersect_pos.y <= (double) vector2_2.y;
    return false;
  }

  public override Bounds bounds
  {
    get
    {
      return new Bounds(this.transform.GetPosition() + new Vector3(this.offset.x, this.offset.y, 0.0f), new Vector3(this._size.x, this._size.y, 0.0f));
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0.0f));
  }
}
