// Decompiled with JetBrains decompiler
// Type: Rotatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
  [SerializeField]
  private Vector3 pivot = Vector3.zero;
  [SerializeField]
  private Vector3 visualizerOffset = Vector3.zero;
  [MyCmpReq]
  private KBatchedAnimController batchedAnimController;
  [MyCmpGet]
  private Building building;
  [Serialize]
  [SerializeField]
  private Orientation orientation;
  public PermittedRotations permittedRotations;
  [SerializeField]
  private int width;
  [SerializeField]
  private int height;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.building != (UnityEngine.Object) null)
    {
      BuildingDef def = this.GetComponent<Building>().Def;
      this.SetSize(def.WidthInCells, def.HeightInCells);
    }
    this.OrientVisualizer(this.orientation);
    this.OrientCollider(this.orientation);
  }

  public void SetSize(int width, int height)
  {
    this.width = width;
    this.height = height;
    if (width % 2 == 0)
    {
      this.pivot = new Vector3(-0.5f, 0.5f, 0.0f);
      this.visualizerOffset = new Vector3(0.5f, 0.0f, 0.0f);
    }
    else
    {
      this.pivot = new Vector3(0.0f, 0.5f, 0.0f);
      this.visualizerOffset = Vector3.zero;
    }
  }

  public Orientation Rotate()
  {
    switch (this.permittedRotations)
    {
      case PermittedRotations.R90:
        this.orientation = this.orientation != Orientation.Neutral ? Orientation.Neutral : Orientation.R90;
        break;
      case PermittedRotations.R360:
        this.orientation = (Orientation) ((int) (this.orientation + 1) % 4);
        break;
      case PermittedRotations.FlipH:
        this.orientation = this.orientation != Orientation.Neutral ? Orientation.Neutral : Orientation.FlipH;
        break;
      case PermittedRotations.FlipV:
        this.orientation = this.orientation != Orientation.Neutral ? Orientation.Neutral : Orientation.FlipV;
        break;
    }
    this.OrientVisualizer(this.orientation);
    return this.orientation;
  }

  public void SetOrientation(Orientation new_orientation)
  {
    this.orientation = new_orientation;
    this.OrientVisualizer(new_orientation);
    this.OrientCollider(new_orientation);
  }

  public void Match(Rotatable other)
  {
    this.pivot = other.pivot;
    this.visualizerOffset = other.visualizerOffset;
    this.permittedRotations = other.permittedRotations;
    this.orientation = other.orientation;
    this.OrientVisualizer(this.orientation);
    this.OrientCollider(this.orientation);
  }

  public float GetVisualizerRotation()
  {
    switch (this.permittedRotations)
    {
      case PermittedRotations.R90:
      case PermittedRotations.R360:
        return -90f * (float) this.orientation;
      default:
        return 0.0f;
    }
  }

  public bool GetVisualizerFlipX()
  {
    return this.orientation == Orientation.FlipH;
  }

  public bool GetVisualizerFlipY()
  {
    return this.orientation == Orientation.FlipV;
  }

  public Vector3 GetVisualizerPivot()
  {
    Vector3 pivot = this.pivot;
    switch (this.orientation)
    {
      case Orientation.FlipH:
        pivot.x = -this.pivot.x;
        break;
    }
    return pivot;
  }

  private Vector3 GetVisualizerOffset()
  {
    Vector3 vector3;
    switch (this.orientation)
    {
      case Orientation.FlipH:
        vector3 = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
        break;
      case Orientation.FlipV:
        vector3 = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
        break;
      default:
        vector3 = this.visualizerOffset;
        break;
    }
    return vector3;
  }

  private void OrientVisualizer(Orientation orientation)
  {
    float visualizerRotation = this.GetVisualizerRotation();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Pivot = this.GetVisualizerPivot();
    component.Rotation = visualizerRotation;
    component.Offset = this.GetVisualizerOffset();
    component.FlipX = this.GetVisualizerFlipX();
    component.FlipY = this.GetVisualizerFlipY();
    this.Trigger(-1643076535, (object) this);
  }

  private void OrientCollider(Orientation orientation)
  {
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    float num1 = 0.0f;
    switch (orientation)
    {
      case Orientation.R90:
        num1 = -90f;
        break;
      case Orientation.R180:
        num1 = -180f;
        break;
      case Orientation.R270:
        num1 = -270f;
        break;
      case Orientation.FlipH:
        component.offset = new Vector2((float) (this.width % 2 - 1), 0.5f * (float) this.height);
        component.size = new Vector2((float) this.width, (float) this.height);
        break;
      case Orientation.FlipV:
        component.offset = new Vector2(0.0f, -0.5f * (float) (this.height - 2));
        component.size = new Vector2((float) this.width, (float) this.height);
        break;
      default:
        component.offset = new Vector2(0.0f, 0.5f * (float) this.height);
        component.size = new Vector2((float) this.width, (float) this.height);
        break;
    }
    if ((double) num1 == 0.0)
      return;
    Matrix2x3 matrix2x3_1 = Matrix2x3.Translate((Vector2) (-this.pivot));
    Matrix2x3 matrix2x3_2 = Matrix2x3.Translate((Vector2) this.pivot) * Matrix2x3.Rotate(num1 * ((float) Math.PI / 180f)) * matrix2x3_1;
    Vector2 vector2_1 = new Vector2(-0.5f * (float) this.width, 0.0f);
    Vector2 vector2_2 = new Vector2(0.5f * (float) this.width, (float) this.height);
    Vector2 vector2_3 = new Vector2(0.0f, 0.5f * (float) this.height);
    vector2_1 = (Vector2) matrix2x3_2.MultiplyPoint((Vector3) vector2_1);
    Vector2 vector2_4 = (Vector2) matrix2x3_2.MultiplyPoint((Vector3) vector2_2);
    Vector2 vector2_5 = (Vector2) matrix2x3_2.MultiplyPoint((Vector3) vector2_3);
    float num2 = Mathf.Min(vector2_1.x, vector2_4.x);
    float num3 = Mathf.Max(vector2_1.x, vector2_4.x);
    float num4 = Mathf.Min(vector2_1.y, vector2_4.y);
    float num5 = Mathf.Max(vector2_1.y, vector2_4.y);
    component.offset = vector2_5;
    component.size = new Vector2(num3 - num2, num5 - num4);
  }

  public CellOffset GetRotatedCellOffset(CellOffset offset)
  {
    return Rotatable.GetRotatedCellOffset(offset, this.orientation);
  }

  public static CellOffset GetRotatedCellOffset(
    CellOffset offset,
    Orientation orientation)
  {
    switch (orientation)
    {
      case Orientation.R90:
        return new CellOffset(offset.y, -offset.x);
      case Orientation.R180:
        return new CellOffset(-offset.x, -offset.y);
      case Orientation.R270:
        return new CellOffset(-offset.y, offset.x);
      case Orientation.FlipH:
        return new CellOffset(-offset.x, offset.y);
      case Orientation.FlipV:
        return new CellOffset(offset.x, -offset.y);
      default:
        return offset;
    }
  }

  public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
  {
    return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
  }

  public Vector3 GetRotatedOffset(Vector3 offset)
  {
    return Rotatable.GetRotatedOffset(offset, this.orientation);
  }

  public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
  {
    switch (orientation)
    {
      case Orientation.R90:
        return new Vector3(offset.y, -offset.x);
      case Orientation.R180:
        return new Vector3(-offset.x, -offset.y);
      case Orientation.R270:
        return new Vector3(-offset.y, offset.x);
      case Orientation.FlipH:
        return new Vector3(-offset.x, offset.y);
      case Orientation.FlipV:
        return new Vector3(offset.x, -offset.y);
      default:
        return offset;
    }
  }

  public Orientation GetOrientation()
  {
    return this.orientation;
  }

  public bool IsRotated
  {
    get
    {
      return this.orientation != Orientation.Neutral;
    }
  }
}
