// Decompiled with JetBrains decompiler
// Type: SpeechMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance>
{
  public static string PREFIX_SAD = "sad";
  public static string PREFIX_HAPPY = "happy";
  private static HashedString HASH_SNAPTO_MOUTH = (HashedString) "snapto_mouth";
  public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State talking;
  public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.TargetParameter mouth;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (SpeechMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SpeechMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback(SpeechMonitor.CreateMouth);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache0 = SpeechMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State state1 = root.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (SpeechMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SpeechMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback(SpeechMonitor.DestroyMouth);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache1 = SpeechMonitor.\u003C\u003Ef__mg\u0024cache1;
    state1.Exit(fMgCache1);
    this.satisfied.DoNothing();
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State talking = this.talking;
    // ISSUE: reference to a compiler-generated field
    if (SpeechMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SpeechMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback(SpeechMonitor.BeginTalking);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache2 = SpeechMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State state2 = talking.Enter(fMgCache2);
    // ISSUE: reference to a compiler-generated field
    if (SpeechMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SpeechMonitor.\u003C\u003Ef__mg\u0024cache3 = new System.Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SpeechMonitor.Instance, float> fMgCache3 = SpeechMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State state3 = state2.Update(fMgCache3, UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied);
    // ISSUE: reference to a compiler-generated field
    if (SpeechMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SpeechMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback(SpeechMonitor.EndTalking);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache4 = SpeechMonitor.\u003C\u003Ef__mg\u0024cache4;
    state3.Exit(fMgCache4);
  }

  private static void CreateMouth(SpeechMonitor.Instance smi)
  {
    smi.mouth = Util.KInstantiate(Assets.GetPrefab((Tag) MouthAnimation.ID), (GameObject) null, (string) null).GetComponent<KBatchedAnimController>();
    smi.mouth.gameObject.SetActive(true);
    smi.sm.mouth.Set(smi.mouth.gameObject, smi);
  }

  private static void DestroyMouth(SpeechMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) smi.mouth != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) smi.mouth);
    smi.mouth = (KBatchedAnimController) null;
  }

  private static string GetRandomSpeechAnim(string speech_prefix)
  {
    return speech_prefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString();
  }

  public static bool IsAllowedToPlaySpeech(GameObject go)
  {
    if (go.HasTag(GameTags.Dead))
      return false;
    if (go.GetComponent<Navigator>().IsMoving())
      return true;
    KAnim.Anim currentAnim = go.GetComponent<KBatchedAnimController>().GetCurrentAnim();
    if (currentAnim == null)
      return true;
    return GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim);
  }

  public static void BeginTalking(SpeechMonitor.Instance smi)
  {
    smi.ev.clearHandle();
    if (smi.voiceEvent != null)
      smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0.0f, false);
    if (smi.ev.isValid())
    {
      smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    else
    {
      smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
      smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    SpeechMonitor.UpdateTalking(smi, 0.0f);
  }

  public static void EndTalking(SpeechMonitor.Instance smi)
  {
    smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
  }

  public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
  {
    KAnim.Anim.FrameElement frameElement1 = new KAnim.Anim.FrameElement();
    frameElement1.symbol = (KAnimHashedString) HashedString.Invalid;
    int currentFrameIndex = controller.GetCurrentFrameIndex();
    KAnimBatch batch = controller.GetBatch();
    if (currentFrameIndex == -1 || batch == null)
      return frameElement1;
    KAnim.Anim.Frame frame = controller.GetBatch().group.data.GetFrame(currentFrameIndex);
    if (frame == KAnim.Anim.Frame.InvalidFrame)
      return frameElement1;
    for (int index1 = 0; index1 < frame.numElements; ++index1)
    {
      int index2 = frame.firstElementIdx + index1;
      if (index2 < batch.group.data.frameElements.Count)
      {
        KAnim.Anim.FrameElement frameElement2 = batch.group.data.frameElements[index2];
        if (!(frameElement2.symbol == HashedString.Invalid))
        {
          frameElement1 = frameElement2;
          break;
        }
      }
    }
    return frameElement1;
  }

  public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
  {
    if (smi.ev.isValid())
    {
      PLAYBACK_STATE state;
      int playbackState = (int) smi.ev.getPlaybackState(out state);
      if (state != PLAYBACK_STATE.PLAYING && state != PLAYBACK_STATE.STARTING)
      {
        smi.GoTo((StateMachine.BaseState) smi.sm.satisfied);
        smi.ev.clearHandle();
        return;
      }
    }
    KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
    if (firstFrameElement.symbol == HashedString.Invalid)
      return;
    smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Tuning : TuningData<SpeechMonitor.Tuning>
  {
    public float randomSpeechIntervalMin;
    public float randomSpeechIntervalMax;
    public int speechCount;
  }

  public class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public string speechPrefix = "happy";
    public KBatchedAnimController mouth;
    public string voiceEvent;
    public EventInstance ev;

    public Instance(IStateMachineTarget master, SpeechMonitor.Def def)
      : base(master)
    {
    }

    public bool IsPlayingSpeech()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.talking);
    }

    public void PlaySpeech(string speech_prefix, string voice_event)
    {
      this.speechPrefix = speech_prefix;
      this.voiceEvent = voice_event;
      this.GoTo((StateMachine.BaseState) this.sm.talking);
    }

    public void DrawMouth()
    {
      KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(this.smi.mouth);
      if (firstFrameElement.symbol == HashedString.Invalid)
        return;
      KAnim.Build.Symbol symbol1 = this.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      this.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, this.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
      KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol((KAnimHashedString) SpeechMonitor.HASH_SNAPTO_MOUTH);
      KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol1.build.batchTag).symbolFrameInstances[symbol1.firstFrameIdx + firstFrameElement.frame];
      symbolFrameInstance.buildImageIdx = this.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol1.build.GetTexture(0));
      component.SetSymbolOverride(symbol2.firstFrameIdx, symbolFrameInstance);
    }
  }
}
