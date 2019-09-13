// Decompiled with JetBrains decompiler
// Type: EmoteReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EmoteReactable : Reactable
{
  private List<EmoteReactable.EmoteStep> emoteSteps = new List<EmoteReactable.EmoteStep>();
  private int currentStep = -1;
  private KBatchedAnimController kbac;
  public Expression expression;
  public Thought thought;
  private KAnimFile animset;
  private float elapsed;

  public EmoteReactable(
    GameObject gameObject,
    HashedString id,
    ChoreType chore_type,
    HashedString animset,
    int range_width = 15,
    int range_height = 8,
    float min_reactable_time = 0.0f,
    float min_reactor_time = 20f,
    float max_trigger_time = float.PositiveInfinity)
    : base(gameObject, id, chore_type, range_width, range_height, true, min_reactable_time, min_reactor_time, max_trigger_time)
  {
    this.animset = Assets.GetAnim(animset);
  }

  public EmoteReactable AddStep(EmoteReactable.EmoteStep step)
  {
    this.emoteSteps.Add(step);
    return this;
  }

  public EmoteReactable AddExpression(Expression expression)
  {
    this.expression = expression;
    return this;
  }

  public EmoteReactable AddThought(Thought thought)
  {
    this.thought = thought;
    return this;
  }

  public override bool InternalCanBegin(
    GameObject new_reactor,
    Navigator.ActiveTransition transition)
  {
    if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null || (UnityEngine.Object) new_reactor == (UnityEngine.Object) null)
      return false;
    Navigator component = new_reactor.GetComponent<Navigator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.IsMoving() || (component.CurrentNavType == NavType.Tube || component.CurrentNavType == NavType.Ladder) || component.CurrentNavType == NavType.Pole)
      return false;
    return (UnityEngine.Object) this.gameObject != (UnityEngine.Object) new_reactor;
  }

  public override void Update(float dt)
  {
    if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
    {
      Facing component = this.reactor.GetComponent<Facing>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Face(this.gameObject.transform.GetPosition());
    }
    if (this.currentStep >= 0 && (double) this.emoteSteps[this.currentStep].timeout > 0.0 && (double) this.emoteSteps[this.currentStep].timeout < (double) this.elapsed)
      this.NextStep((HashedString) ((string) null));
    else
      this.elapsed += dt;
  }

  protected override void InternalBegin()
  {
    this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
    this.kbac.AddAnimOverrides(this.animset, 0.0f);
    if (this.expression != null)
      this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
    if (this.thought != null)
      this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
    this.NextStep((HashedString) ((string) null));
  }

  protected override void InternalEnd()
  {
    if ((UnityEngine.Object) this.kbac != (UnityEngine.Object) null)
    {
      if (this.currentStep >= 0 && this.currentStep < this.emoteSteps.Count && (double) this.emoteSteps[this.currentStep].timeout <= 0.0)
        this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.NextStep);
      this.kbac.RemoveAnimOverrides(this.animset);
      this.kbac = (KBatchedAnimController) null;
    }
    if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
    {
      if (this.expression != null)
        this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
      if (this.thought != null)
        this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
    }
    this.currentStep = -1;
  }

  protected override void InternalCleanup()
  {
  }

  private void NextStep(HashedString finishedAnim)
  {
    if (this.currentStep >= 0 && (double) this.emoteSteps[this.currentStep].timeout <= 0.0)
    {
      this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.NextStep);
      if (this.emoteSteps[this.currentStep].finishcb != null)
        this.emoteSteps[this.currentStep].finishcb(this.reactor);
    }
    ++this.currentStep;
    if (this.currentStep >= this.emoteSteps.Count || (UnityEngine.Object) this.kbac == (UnityEngine.Object) null)
    {
      this.End();
    }
    else
    {
      if (this.emoteSteps[this.currentStep].anim != HashedString.Invalid)
      {
        this.kbac.Play(this.emoteSteps[this.currentStep].anim, this.emoteSteps[this.currentStep].mode, 1f, 0.0f);
        if (this.kbac.IsStopped())
        {
          DebugUtil.DevAssertArgs(false, (object) "Emote is missing anim:", (object) this.emoteSteps[this.currentStep].anim);
          this.emoteSteps[this.currentStep].timeout = 0.25f;
        }
      }
      if ((double) this.emoteSteps[this.currentStep].timeout <= 0.0)
        this.kbac.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.NextStep);
      else
        this.elapsed = 0.0f;
      if (this.emoteSteps[this.currentStep].startcb == null)
        return;
      this.emoteSteps[this.currentStep].startcb(this.reactor);
    }
  }

  public class EmoteStep
  {
    public HashedString anim = HashedString.Invalid;
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;
    public float timeout = -1f;
    public System.Action<GameObject> startcb;
    public System.Action<GameObject> finishcb;
  }
}
