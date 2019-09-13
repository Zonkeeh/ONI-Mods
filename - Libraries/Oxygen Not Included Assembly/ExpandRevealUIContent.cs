// Decompiled with JetBrains decompiler
// Type: ExpandRevealUIContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ExpandRevealUIContent : MonoBehaviour
{
  public float speedScale = 1f;
  private Coroutine activeRoutine;
  private System.Action<object> activeRoutineCompleteCallback;
  public AnimationCurve expandAnimation;
  public AnimationCurve collapseAnimation;
  public KRectStretcher MaskRectStretcher;
  public KRectStretcher BGRectStretcher;
  public KChildFitter MaskChildFitter;
  public KChildFitter BGChildFitter;
  public bool Collapsing;
  public bool Expanding;

  private void OnDisable()
  {
    if ((bool) ((UnityEngine.Object) this.BGChildFitter))
      this.BGChildFitter.WidthScale = this.BGChildFitter.HeightScale = 0.0f;
    if ((bool) ((UnityEngine.Object) this.MaskChildFitter))
    {
      if (this.MaskChildFitter.fitWidth)
        this.MaskChildFitter.WidthScale = 0.0f;
      if (this.MaskChildFitter.fitHeight)
        this.MaskChildFitter.HeightScale = 0.0f;
    }
    if ((bool) ((UnityEngine.Object) this.BGRectStretcher))
    {
      this.BGRectStretcher.XStretchFactor = this.BGRectStretcher.YStretchFactor = 0.0f;
      this.BGRectStretcher.UpdateStretching();
    }
    if (!(bool) ((UnityEngine.Object) this.MaskRectStretcher))
      return;
    this.MaskRectStretcher.XStretchFactor = this.MaskRectStretcher.YStretchFactor = 0.0f;
    this.MaskRectStretcher.UpdateStretching();
  }

  public void Expand(System.Action<object> completeCallback)
  {
    if ((bool) ((UnityEngine.Object) this.MaskChildFitter) && (bool) ((UnityEngine.Object) this.MaskRectStretcher))
      Debug.LogWarning((object) "ExpandRevealUIContent has references to both a MaskChildFitter and a MaskRectStretcher. It should have only one or the other. ChildFitter to match child size, RectStretcher to match parent size.");
    if ((bool) ((UnityEngine.Object) this.BGChildFitter) && (bool) ((UnityEngine.Object) this.BGRectStretcher))
      Debug.LogWarning((object) "ExpandRevealUIContent has references to both a BGChildFitter and a BGRectStretcher . It should have only one or the other.  ChildFitter to match child size, RectStretcher to match parent size.");
    if (this.activeRoutine != null)
      this.StopCoroutine(this.activeRoutine);
    this.CollapsedImmediate();
    this.activeRoutineCompleteCallback = completeCallback;
    this.activeRoutine = this.StartCoroutine(this.expand((System.Action<object>) null));
  }

  public void Collapse(System.Action<object> completeCallback)
  {
    if (this.activeRoutine != null)
    {
      if (this.activeRoutineCompleteCallback != null)
        this.activeRoutineCompleteCallback((object) null);
      this.StopCoroutine(this.activeRoutine);
    }
    this.activeRoutineCompleteCallback = completeCallback;
    if (this.gameObject.activeInHierarchy)
    {
      this.activeRoutine = this.StartCoroutine(this.collapse(completeCallback));
    }
    else
    {
      this.activeRoutine = (Coroutine) null;
      if (completeCallback == null)
        return;
      completeCallback((object) null);
    }
  }

  [DebuggerHidden]
  private IEnumerator expand(System.Action<object> completeCallback)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ExpandRevealUIContent.\u003Cexpand\u003Ec__Iterator0()
    {
      completeCallback = completeCallback,
      \u0024this = this
    };
  }

  private void SetStretch(float value)
  {
    if ((bool) ((UnityEngine.Object) this.BGRectStretcher))
    {
      if (this.BGRectStretcher.StretchX)
        this.BGRectStretcher.XStretchFactor = value;
      if (this.BGRectStretcher.StretchY)
        this.BGRectStretcher.YStretchFactor = value;
    }
    if ((bool) ((UnityEngine.Object) this.MaskRectStretcher))
    {
      if (this.MaskRectStretcher.StretchX)
        this.MaskRectStretcher.XStretchFactor = value;
      if (this.MaskRectStretcher.StretchY)
        this.MaskRectStretcher.YStretchFactor = value;
    }
    if ((bool) ((UnityEngine.Object) this.BGChildFitter))
    {
      if (this.BGChildFitter.fitWidth)
        this.BGChildFitter.WidthScale = value;
      if (this.BGChildFitter.fitHeight)
        this.BGChildFitter.HeightScale = value;
    }
    if (!(bool) ((UnityEngine.Object) this.MaskChildFitter))
      return;
    if (this.MaskChildFitter.fitWidth)
      this.MaskChildFitter.WidthScale = value;
    if (!this.MaskChildFitter.fitHeight)
      return;
    this.MaskChildFitter.HeightScale = value;
  }

  [DebuggerHidden]
  private IEnumerator collapse(System.Action<object> completeCallback)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ExpandRevealUIContent.\u003Ccollapse\u003Ec__Iterator1()
    {
      completeCallback = completeCallback,
      \u0024this = this
    };
  }

  public void CollapsedImmediate()
  {
    this.SetStretch(this.collapseAnimation.Evaluate((float) this.collapseAnimation.length));
  }
}
