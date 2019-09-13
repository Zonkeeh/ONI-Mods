// Decompiled with JetBrains decompiler
// Type: KBatchedAnimTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class KBatchedAnimTracker : MonoBehaviour
{
  [SerializeField]
  public Vector3 offset = Vector3.zero;
  public Vector3 targetPoint = Vector3.zero;
  public bool fadeOut = true;
  private bool alive = true;
  [SerializeField]
  public KBatchedAnimController controller;
  public HashedString symbol;
  public bool useTargetPoint;
  public bool skipInitialDisable;
  public bool forceAlwaysVisible;
  private bool forceUpdate;
  private Matrix2x3 previousMatrix;
  private Vector3 previousPosition;
  private KBatchedAnimController myAnim;

  private void Start()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      for (Transform parent = this.transform.parent; (UnityEngine.Object) parent != (UnityEngine.Object) null; parent = parent.parent)
      {
        this.controller = parent.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
          break;
      }
    }
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Controller Null for tracker on " + this.gameObject.name), (UnityEngine.Object) this.gameObject);
      this.enabled = false;
    }
    else
    {
      this.controller.onAnimEnter += new KAnimControllerBase.KAnimEvent(this.OnAnimStart);
      this.controller.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnAnimStop);
      this.controller.onLayerChanged += new System.Action<int>(this.OnLayerChanged);
      this.forceUpdate = true;
      this.myAnim = this.GetComponent<KBatchedAnimController>();
      List<KAnimControllerBase> kanimControllerBaseList = new List<KAnimControllerBase>((IEnumerable<KAnimControllerBase>) this.GetComponentsInChildren<KAnimControllerBase>(true));
      if (!this.skipInitialDisable)
      {
        for (int index = 0; index < this.transform.childCount; ++index)
          this.transform.GetChild(index).gameObject.SetActive(false);
      }
      for (int index = kanimControllerBaseList.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) kanimControllerBaseList[index].gameObject == (UnityEngine.Object) this.gameObject)
          kanimControllerBaseList.RemoveAt(index);
      }
    }
  }

  private void OnDestroy()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
    {
      this.controller.onAnimEnter -= new KAnimControllerBase.KAnimEvent(this.OnAnimStart);
      this.controller.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.OnAnimStop);
      this.controller.onLayerChanged -= new System.Action<int>(this.OnLayerChanged);
      this.controller = (KBatchedAnimController) null;
    }
    this.myAnim = (KBatchedAnimController) null;
  }

  private void LateUpdate()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
      this.UpdateFrame();
    if (this.alive)
      return;
    this.enabled = false;
  }

  private void UpdateFrame()
  {
    this.forceUpdate = false;
    bool symbolVisible = false;
    if (this.controller.CurrentAnim != null)
    {
      Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out symbolVisible);
      Vector3 position1 = this.controller.transform.GetPosition();
      if (symbolVisible && (this.previousMatrix != symbolLocalTransform || position1 != this.previousPosition || this.useTargetPoint))
      {
        this.previousMatrix = symbolLocalTransform;
        this.previousPosition = position1;
        Matrix2x3 transform_matrix = this.controller.GetTransformMatrix() * symbolLocalTransform;
        float z = this.transform.GetPosition().z;
        this.transform.SetPosition(transform_matrix.MultiplyPoint(this.offset));
        if (this.useTargetPoint)
        {
          Vector3 position2 = this.transform.GetPosition();
          position2.z = 0.0f;
          Vector3 from = this.targetPoint - position2;
          float angle = Vector3.Angle(from, Vector3.right);
          if ((double) from.y < 0.0)
            angle = 360f - angle;
          this.transform.localRotation = Quaternion.identity;
          this.transform.RotateAround(position2, new Vector3(0.0f, 0.0f, 1f), angle);
          this.myAnim.GetBatchInstanceData().SetClipRadius(this.transform.GetPosition().x, this.transform.GetPosition().y, from.sqrMagnitude, true);
        }
        else
        {
          Vector3 v1 = !this.controller.FlipX ? Vector3.right : Vector3.left;
          Vector3 v2 = !this.controller.FlipY ? Vector3.up : Vector3.down;
          this.transform.up = transform_matrix.MultiplyVector(v2);
          this.transform.right = transform_matrix.MultiplyVector(v1);
          if ((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null)
            this.myAnim.GetBatchInstanceData()?.SetOverrideTransformMatrix(transform_matrix);
        }
        this.transform.SetPosition(new Vector3(this.transform.GetPosition().x, this.transform.GetPosition().y, z));
        this.myAnim.SetDirty();
      }
    }
    if (!((UnityEngine.Object) this.myAnim != (UnityEngine.Object) null) || symbolVisible == this.myAnim.enabled)
      return;
    this.myAnim.enabled = symbolVisible;
  }

  [ContextMenu("ForceAlive")]
  private void OnAnimStart(HashedString name)
  {
    this.alive = true;
    this.enabled = true;
    this.forceUpdate = true;
  }

  private void OnAnimStop(HashedString name)
  {
    this.alive = false;
  }

  private void OnLayerChanged(int layer)
  {
    this.myAnim.SetLayer(layer);
  }

  public void SetTarget(Vector3 target)
  {
    this.targetPoint = target;
    this.targetPoint.z = 0.0f;
  }
}
