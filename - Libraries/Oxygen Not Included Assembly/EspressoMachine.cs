// Decompiled with JetBrains decompiler
// Type: EspressoMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;

public class EspressoMachine : StateMachineComponent<EspressoMachine.StatesInstance>, IEffectDescriptor
{
  public static Tag INGREDIENT_TAG = new Tag("SpiceNut");
  public static float INGREDIENT_MASS_PER_USE = 1f;
  public static float WATER_MASS_PER_USE = 1f;
  public const string SPECIFIC_EFFECT = "Espresso";
  public const string TRACKING_EFFECT = "RecentlyEspresso";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true)), (object) null, (SchedulerGroup) null);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
  {
    string str = tag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
    descs.Add(descriptor);
  }

  List<Descriptor> IEffectDescriptor.GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descs = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.RECREATION, (string) UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
    descs.Add(descriptor);
    Effect.AddModifierDescriptions(this.gameObject, descs, "Espresso", true);
    this.AddRequirementDesc(descs, EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE);
    this.AddRequirementDesc(descs, GameTags.Water, EspressoMachine.WATER_MASS_PER_USE);
    return descs;
  }

  public class States : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine>
  {
    private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State unoperational;
    private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State operational;
    private EspressoMachine.States.ReadyStates ready;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
      this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition((GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State) this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State) this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady));
      this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<EspressoMachine.StatesInstance, Chore>(this.CreateChore), this.operational);
      this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((Func<EspressoMachine.StatesInstance, Workable>) (smi => (Workable) smi.master.GetComponent<EspressoMachineWorkable>()), this.ready.working).Transition(this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)));
      this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, (Func<EspressoMachine.StatesInstance, string>) null).WorkableStopTransition((Func<EspressoMachine.StatesInstance, Workable>) (smi => (Workable) smi.master.GetComponent<EspressoMachineWorkable>()), this.ready.post);
      this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State) this.ready);
    }

    private Chore CreateChore(EspressoMachine.StatesInstance smi)
    {
      Workable component = (Workable) smi.master.GetComponent<EspressoMachineWorkable>();
      Chore chore = (Chore) new WorkChore<EspressoMachineWorkable>(Db.Get().ChoreTypes.Relax, (IStateMachineTarget) component, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
      chore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) component);
      return chore;
    }

    private bool IsReady(EspressoMachine.StatesInstance smi)
    {
      PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
      return !((UnityEngine.Object) primaryElement == (UnityEngine.Object) null) && (double) primaryElement.Mass >= (double) EspressoMachine.WATER_MASS_PER_USE && (double) smi.GetComponent<Storage>().GetAmountAvailable(EspressoMachine.INGREDIENT_TAG) >= (double) EspressoMachine.INGREDIENT_MASS_PER_USE;
    }

    public class ReadyStates : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State
    {
      public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State idle;
      public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State working;
      public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State post;
    }
  }

  public class StatesInstance : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.GameInstance
  {
    public StatesInstance(EspressoMachine smi)
      : base(smi)
    {
    }
  }
}
