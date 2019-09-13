// Decompiled with JetBrains decompiler
// Type: MessStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MessStation : Workable, IGameObjectEffectDescriptor
{
  private MessStation.MessStationSM.Instance smi;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
  }

  protected override void OnCompleteWork(Worker worker)
  {
    worker.workable.GetComponent<Edible>().CompleteWork(worker);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new MessStation.MessStationSM.Instance(this);
    this.smi.StartSM();
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false));
    return descriptorList;
  }

  public bool HasSalt
  {
    get
    {
      return this.smi.HasSalt;
    }
  }

  public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
  {
    public MessStation.MessStationSM.SaltState salt;
    public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.salt.none;
      this.salt.none.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => smi.HasSalt), UpdateRate.SIM_200ms).PlayAnim("off");
      this.salt.salty.Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => !smi.HasSalt), UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) null);
      this.eating.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasSalt)
          return !smi.IsEating();
        return false;
      }), UpdateRate.SIM_200ms).Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi =>
      {
        if (!smi.HasSalt)
          return !smi.IsEating();
        return false;
      }), UpdateRate.SIM_200ms).PlayAnim("off");
    }

    public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
    {
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
    }

    public class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
    {
      private Storage saltStorage;
      private Assignable assigned;

      public Instance(MessStation master)
        : base(master)
      {
        this.saltStorage = master.GetComponent<Storage>();
        this.assigned = master.GetComponent<Assignable>();
      }

      public bool HasSalt
      {
        get
        {
          return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
        }
      }

      public bool IsEating()
      {
        if ((Object) this.assigned != (Object) null && this.assigned.assignee != null)
        {
          GameObject targetGameObject = this.assigned.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
          if ((bool) ((Object) targetGameObject))
          {
            ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
            if ((Object) component != (Object) null && component.HasChore())
              return component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
            return false;
          }
        }
        return false;
      }
    }
  }
}
