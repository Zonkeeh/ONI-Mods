// Decompiled with JetBrains decompiler
// Type: EasingAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EasingAnimations : MonoBehaviour
{
  public EasingAnimations.AnimationScales[] scales;
  private EasingAnimations.AnimationScales currentAnimation;
  private Coroutine animationCoroutine;
  private Dictionary<string, EasingAnimations.AnimationScales> animationMap;
  public System.Action<string> OnAnimationDone;

  private void Start()
  {
    if (this.animationMap != null && this.animationMap.Count != 0)
      return;
    this.Initialize();
  }

  private void Initialize()
  {
    this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
    foreach (EasingAnimations.AnimationScales scale in this.scales)
      this.animationMap.Add(scale.name, scale);
  }

  public void PlayAnimation(string animationName, float delay = 0.0f)
  {
    if (this.animationMap == null || this.animationMap.Count == 0)
      this.Initialize();
    if (!this.animationMap.ContainsKey(animationName))
      return;
    if (this.animationCoroutine != null)
      this.StopCoroutine(this.animationCoroutine);
    this.currentAnimation = this.animationMap[animationName];
    this.currentAnimation.currentScale = this.currentAnimation.startScale;
    this.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
    this.animationCoroutine = this.StartCoroutine(this.ExecuteAnimation(delay));
  }

  [DebuggerHidden]
  private IEnumerator ExecuteAnimation(float delay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EasingAnimations.\u003CExecuteAnimation\u003Ec__Iterator0()
    {
      delay = delay,
      \u0024this = this
    };
  }

  private float GetEasing(float t)
  {
    switch (this.currentAnimation.type)
    {
      case EasingAnimations.AnimationScales.AnimationType.EaseOutBack:
        return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
      case EasingAnimations.AnimationScales.AnimationType.EaseInBack:
        return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
      default:
        return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
    }
  }

  public float EaseInOutBack(float start, float end, float value)
  {
    float num1 = 1.70158f;
    end -= start;
    value /= 0.5f;
    if ((double) value < 1.0)
    {
      float num2 = num1 * 1.525f;
      return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num2 + 1.0) * (double) value - (double) num2))) + start;
    }
    value -= 2f;
    float num3 = num1 * 1.525f;
    return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num3 + 1.0) * (double) value + (double) num3) + 2.0)) + start;
  }

  public float EaseInBack(float start, float end, float value)
  {
    end -= start;
    value /= 1f;
    float num = 1.70158f;
    return (float) ((double) end * (double) value * (double) value * (((double) num + 1.0) * (double) value - (double) num)) + start;
  }

  public float EaseOutBack(float start, float end, float value)
  {
    float num = 1.70158f;
    end -= start;
    --value;
    return end * (float) ((double) value * (double) value * (((double) num + 1.0) * (double) value + (double) num) + 1.0) + start;
  }

  [Serializable]
  public struct AnimationScales
  {
    public string name;
    public float startScale;
    public float endScale;
    public EasingAnimations.AnimationScales.AnimationType type;
    public float easingMultiplier;
    [HideInInspector]
    public float currentScale;

    public enum AnimationType
    {
      EaseInOutBack,
      EaseOutBack,
      EaseInBack,
    }
  }
}
