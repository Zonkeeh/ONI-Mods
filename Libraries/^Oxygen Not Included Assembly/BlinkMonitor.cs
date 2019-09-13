// Decompiled with JetBrains decompiler
// Type: BlinkMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance>
{
  private static HashedString HASH_SNAPTO_EYES = (HashedString) "snapto_eyes";
  public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State blinking;
  public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.TargetParameter eyes;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback(BlinkMonitor.CreateEyes);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache0 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State state1 = root.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback(BlinkMonitor.DestroyEyes);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache1 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache1;
    state1.Exit(fMgCache1);
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State satisfied1 = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache2 = new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime);
    }
    // ISSUE: reference to a compiler-generated field
    Func<BlinkMonitor.Instance, float> fMgCache2 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State blinking1 = this.blinking;
    satisfied1.ScheduleGoTo(fMgCache2, (StateMachine.BaseState) blinking1);
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State blinking2 = this.blinking;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State satisfied2 = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BlinkMonitor.CanBlink);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition = GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.Not(BlinkMonitor.\u003C\u003Ef__mg\u0024cache3);
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State state2 = blinking2.EnterTransition(satisfied2, condition);
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback(BlinkMonitor.BeginBlinking);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache4 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache4;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State state3 = state2.Enter(fMgCache4);
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache5 = new System.Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<BlinkMonitor.Instance, float> fMgCache5 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State state4 = state3.Update(fMgCache5, UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied);
    // ISSUE: reference to a compiler-generated field
    if (BlinkMonitor.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BlinkMonitor.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback(BlinkMonitor.EndBlinking);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache6 = BlinkMonitor.\u003C\u003Ef__mg\u0024cache6;
    state4.Exit(fMgCache6);
  }

  private static bool CanBlink(BlinkMonitor.Instance smi)
  {
    if (SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
      return smi.Get<Navigator>().CurrentNavType != NavType.Ladder;
    return false;
  }

  private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
  {
    return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
  }

  private static void CreateEyes(BlinkMonitor.Instance smi)
  {
    smi.eyes = Util.KInstantiate(Assets.GetPrefab((Tag) EyeAnimation.ID), (GameObject) null, (string) null).GetComponent<KBatchedAnimController>();
    smi.eyes.gameObject.SetActive(true);
    smi.sm.eyes.Set(smi.eyes.gameObject, smi);
  }

  private static void DestroyEyes(BlinkMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) smi.eyes != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) smi.eyes);
    smi.eyes = (KBatchedAnimController) null;
  }

  public static void BeginBlinking(BlinkMonitor.Instance smi)
  {
    string str = "eyes1";
    smi.eyes.Play((HashedString) str, KAnim.PlayMode.Once, 1f, 0.0f);
    BlinkMonitor.UpdateBlinking(smi, 0.0f);
  }

  public static void EndBlinking(BlinkMonitor.Instance smi)
  {
    smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
  }

  public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
  {
    int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
    KAnimBatch batch = smi.eyes.GetBatch();
    if (currentFrameIndex == -1 || batch == null)
      return;
    KAnim.Anim.Frame frame = smi.eyes.GetBatch().group.data.GetFrame(currentFrameIndex);
    if (frame == KAnim.Anim.Frame.InvalidFrame)
      return;
    HashedString hashedString = HashedString.Invalid;
    for (int index1 = 0; index1 < frame.numElements; ++index1)
    {
      int index2 = frame.firstElementIdx + index1;
      if (index2 < batch.group.data.frameElements.Count)
      {
        KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[index2];
        if (!(frameElement.symbol == HashedString.Invalid))
        {
          hashedString = (HashedString) frameElement.symbol;
          break;
        }
      }
    }
    smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) hashedString), 3);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Tuning : TuningData<BlinkMonitor.Tuning>
  {
    public float randomBlinkIntervalMin;
    public float randomBlinkIntervalMax;
  }

  public class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public KBatchedAnimController eyes;

    public Instance(IStateMachineTarget master, BlinkMonitor.Def def)
      : base(master)
    {
    }

    public bool IsBlinking()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.blinking);
    }

    public void Blink()
    {
      this.GoTo((StateMachine.BaseState) this.sm.blinking);
    }
  }
}
