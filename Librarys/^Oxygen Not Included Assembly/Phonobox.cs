// Decompiled with JetBrains decompiler
// Type: Phonobox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Phonobox : StateMachineComponent<Phonobox.StatesInstance>, IEffectDescriptor
{
  private static string[] building_anims = new string[3]
  {
    "working_loop",
    "working_loop2",
    "working_loop3"
  };
  public CellOffset[] choreOffsets = new CellOffset[5]
  {
    new CellOffset(0, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 0),
    new CellOffset(-2, 0),
    new CellOffset(2, 0)
  };
  private HashSet<Worker> players = new HashSet<Worker>();
  public const string SPECIFIC_EFFECT = "Danced";
  public const string TRACKING_EFFECT = "RecentlyDanced";
  private PhonoboxWorkable[] workables;
  private Chore[] chores;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true)), (object) null, (SchedulerGroup) null);
    this.workables = new PhonoboxWorkable[this.choreOffsets.Length];
    this.chores = new Chore[this.choreOffsets.Length];
    for (int index = 0; index < this.workables.Length; ++index)
    {
      PhonoboxWorkable phonoboxWorkable = ChoreHelpers.CreateLocator("PhonoboxWorkable", Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.choreOffsets[index]), Grid.SceneLayer.Move)).AddOrGet<PhonoboxWorkable>();
      phonoboxWorkable.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Dancing);
      phonoboxWorkable.owner = this;
      this.workables[index] = phonoboxWorkable;
    }
  }

  protected override void OnCleanUp()
  {
    this.UpdateChores(false);
    for (int index = 0; index < this.workables.Length; ++index)
    {
      if ((bool) ((UnityEngine.Object) this.workables[index]))
      {
        Util.KDestroyGameObject((Component) this.workables[index]);
        this.workables[index] = (PhonoboxWorkable) null;
      }
    }
    base.OnCleanUp();
  }

  private Chore CreateChore(int i)
  {
    Workable workable = (Workable) this.workables[i];
    Chore chore = (Chore) new WorkChore<PhonoboxWorkable>(Db.Get().ChoreTypes.Relax, (IStateMachineTarget) workable, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, new System.Action<Chore>(this.OnSocialChoreEnd), false, Db.Get().ScheduleBlockTypes.Recreation, false, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
    chore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) workable);
    return chore;
  }

  private void OnSocialChoreEnd(Chore chore)
  {
    if (!this.gameObject.HasTag(GameTags.Operational))
      return;
    this.UpdateChores(true);
  }

  public void UpdateChores(bool update = true)
  {
    for (int i = 0; i < this.choreOffsets.Length; ++i)
    {
      Chore chore = this.chores[i];
      if (update)
      {
        if (chore == null || chore.isComplete)
          this.chores[i] = this.CreateChore(i);
      }
      else if (chore != null)
      {
        chore.Cancel("locator invalidated");
        this.chores[i] = (Chore) null;
      }
    }
  }

  public void AddWorker(Worker player)
  {
    this.players.Add(player);
    this.smi.sm.playerCount.Set(this.players.Count, this.smi);
  }

  public void RemoveWorker(Worker player)
  {
    this.players.Remove(player);
    this.smi.sm.playerCount.Set(this.players.Count, this.smi);
  }

  List<Descriptor> IEffectDescriptor.GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descs = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.RECREATION, (string) UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
    descs.Add(descriptor);
    Effect.AddModifierDescriptions(this.gameObject, descs, "Danced", true);
    return descs;
  }

  public class States : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox>
  {
    public StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.IntParameter playerCount;
    public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State unoperational;
    public Phonobox.States.OperationalStates operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.Enter((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State.Callback) (smi => smi.SetActive(false))).TagTransition(GameTags.Operational, (GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State) this.operational, false).PlayAnim("off");
      this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State.Callback) (smi => smi.master.UpdateChores(true))).Exit("CancelChore", (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State.Callback) (smi => smi.master.UpdateChores(false))).DefaultState(this.operational.stopped);
      this.operational.stopped.Enter((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State.Callback) (smi => smi.SetActive(false))).ParamTransition<int>((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>) this.playerCount, this.operational.pre, (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>.Callback) ((smi, p) => p > 0)).PlayAnim("on");
      this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
      GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State state = this.operational.playing.Enter((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State.Callback) (smi => smi.SetActive(true))).ScheduleGoTo(25f, (StateMachine.BaseState) this.operational.song_end).ParamTransition<int>((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>) this.playerCount, this.operational.post, (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>.Callback) ((smi, p) => p == 0));
      // ISSUE: reference to a compiler-generated field
      if (Phonobox.States.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Phonobox.States.\u003C\u003Ef__mg\u0024cache0 = new Func<Phonobox.StatesInstance, string>(Phonobox.States.GetPlayAnim);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Phonobox.StatesInstance, string> fMgCache0 = Phonobox.States.\u003C\u003Ef__mg\u0024cache0;
      state.PlayAnim(fMgCache0, KAnim.PlayMode.Loop);
      this.operational.song_end.ParamTransition<int>((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>) this.playerCount, this.operational.bridge, (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>.Callback) ((smi, p) => p > 0)).ParamTransition<int>((StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>) this.playerCount, this.operational.post, (StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.Parameter<int>.Callback) ((smi, p) => p == 0));
      this.operational.bridge.PlayAnim("working_trans").OnAnimQueueComplete(this.operational.playing);
      this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
    }

    public static string GetPlayAnim(Phonobox.StatesInstance smi)
    {
      int index = UnityEngine.Random.Range(0, Phonobox.building_anims.Length);
      return Phonobox.building_anims[index];
    }

    public class OperationalStates : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State
    {
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State stopped;
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State pre;
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State bridge;
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State playing;
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State song_end;
      public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State post;
    }
  }

  public class StatesInstance : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.GameInstance
  {
    private FetchChore chore;
    private Operational operational;

    public StatesInstance(Phonobox smi)
      : base(smi)
    {
      this.operational = this.master.GetComponent<Operational>();
    }

    public void SetActive(bool active)
    {
      this.operational.SetActive(this.operational.IsOperational && active, false);
    }
  }
}
