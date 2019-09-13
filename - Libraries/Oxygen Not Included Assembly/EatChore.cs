// Decompiled with JetBrains decompiler
// Type: EatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class EatChore : Chore<EatChore.StatesInstance>
{
  public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition()
  {
    id = nameof (EdibleIsNotNull),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) null != (UnityEngine.Object) context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible())
  };

  public EatChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.Eat, master, master.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
  {
    this.smi = new EatChore.StatesInstance(this);
    this.showAvailabilityInHoverText = false;
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(EatChore.EdibleIsNotNull, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "EATCHORE null context.consumer");
    }
    else
    {
      RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "EATCHORE null RationMonitor.Instance");
      }
      else
      {
        Edible edible = smi.GetEdible();
        if ((UnityEngine.Object) edible.gameObject == (UnityEngine.Object) null)
          Debug.LogError((object) "EATCHORE null edible.gameObject");
        else if (this.smi == null)
          Debug.LogError((object) "EATCHORE null smi");
        else if (this.smi.sm == null)
          Debug.LogError((object) "EATCHORE null smi.sm");
        else if (this.smi.sm.ediblesource == null)
        {
          Debug.LogError((object) "EATCHORE null smi.sm.ediblesource");
        }
        else
        {
          this.smi.sm.ediblesource.Set(edible.gameObject, this.smi);
          KCrashReporter.Assert((double) edible.FoodInfo.CaloriesPerUnit > 0.0, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs");
          AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.gameObject);
          float num1 = (amountInstance.GetMax() - amountInstance.value) / edible.FoodInfo.CaloriesPerUnit;
          KCrashReporter.Assert((double) num1 > 0.0, "EatChore is requesting an invalid amount of food");
          double num2 = (double) this.smi.sm.requestedfoodunits.Set(num1, this.smi);
          this.smi.sm.eater.Set(context.consumerState.gameObject, this.smi);
          base.Begin(context);
        }
      }
    }
  }

  public class StatesInstance : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.GameInstance
  {
    private int locatorCell;

    public StatesInstance(EatChore master)
      : base(master)
    {
    }

    public void UpdateMessStation()
    {
      Ownables soleOwner = this.sm.eater.Get(this.smi).GetComponent<MinionIdentity>().GetSoleOwner();
      List<Assignable> preferredAssignables = Game.Instance.assignmentManager.GetPreferredAssignables((Assignables) soleOwner, Db.Get().AssignableSlots.MessStation);
      if (preferredAssignables.Count == 0)
      {
        soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.MessStation);
        preferredAssignables = Game.Instance.assignmentManager.GetPreferredAssignables((Assignables) soleOwner, Db.Get().AssignableSlots.MessStation);
      }
      this.smi.sm.messstation.Set(preferredAssignables.Count <= 0 ? (KMonoBehaviour) null : (KMonoBehaviour) preferredAssignables[0], this.smi);
    }

    public bool UseSalt()
    {
      if (this.smi.sm.messstation == null || !((UnityEngine.Object) this.smi.sm.messstation.Get(this.smi) != (UnityEngine.Object) null))
        return false;
      MessStation component = this.smi.sm.messstation.Get(this.smi).GetComponent<MessStation>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component.HasSalt;
      return false;
    }

    public void CreateLocator()
    {
      int cell = this.sm.eater.Get<Sensors>(this.smi).GetSensor<SafeCellSensor>().GetCellQuery();
      if (cell == Grid.InvalidCell)
        cell = Grid.PosToCell(this.sm.eater.Get<Transform>(this.smi).GetPosition());
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
      Grid.Reserved[cell] = true;
      this.sm.locator.Set(ChoreHelpers.CreateLocator("EatLocator", posCbc), this);
      this.locatorCell = cell;
    }

    public void DestroyLocator()
    {
      Grid.Reserved[this.locatorCell] = false;
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
    }

    public void SetZ(GameObject go, float z)
    {
      Vector3 position = go.transform.GetPosition();
      position.z = z;
      go.transform.SetPosition(position);
    }

    public void ApplyRoomEffects()
    {
      Game.Instance.roomProber.GetRoomOfGameObject(this.sm.messstation.Get(this.smi).gameObject)?.roomType.TriggerRoomEffects(this.sm.messstation.Get(this.smi).gameObject.GetComponent<KPrefabID>(), this.sm.eater.Get(this.smi).gameObject.GetComponent<Effects>());
    }

    public void ApplySaltEffect()
    {
      Storage component = this.sm.messstation.Get(this.smi).gameObject.GetComponent<Storage>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Has(TableSaltConfig.ID.ToTag()))
        return;
      component.ConsumeIgnoringDisease(TableSaltConfig.ID.ToTag(), TableSaltTuning.CONSUMABLE_RATE);
      this.sm.eater.Get(this.smi).gameObject.GetComponent<Worker>().GetComponent<Effects>().Add("MessTableSalt", true);
      this.sm.messstation.Get(this.smi).gameObject.Trigger(1356255274, (object) null);
    }
  }

  public class States : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore>
  {
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter eater;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblesource;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblechunk;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter messstation;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter requestedfoodunits;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter actualfoodunits;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter locator;
    public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FetchSubState fetch;
    public EatChore.States.EatOnFloorState eatonfloorstate;
    public EatChore.States.EatAtMessStationState eatatmessstation;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.eater);
      this.root.Enter("SetMessStation", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.UpdateMessStation())).EventHandler(GameHashes.AssignablesChanged, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.UpdateMessStation()));
      this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatatmessstation, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null);
      this.eatatmessstation.DefaultState((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatatmessstation.moveto).ParamTransition<GameObject>((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>) this.messstation, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null));
      this.eatatmessstation.moveto.InitializeStates(this.eater, this.messstation, this.eatatmessstation.eat, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate, (CellOffset[]) null, (NavTactic) null);
      this.eatatmessstation.eat.ToggleAnims("anim_eat_table_kanim", 0.0f).DoEat(this.ediblechunk, this.actualfoodunits, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null).Enter((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi =>
      {
        smi.SetZ(this.eater.Get(smi), Grid.GetLayerZ(Grid.SceneLayer.BuildingFront));
        smi.ApplyRoomEffects();
        smi.ApplySaltEffect();
      })).Exit((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.SetZ(this.eater.Get(smi), Grid.GetLayerZ(Grid.SceneLayer.Move))));
      this.eatonfloorstate.DefaultState((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate.moveto).Enter("CreateLocator", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.CreateLocator())).Exit("DestroyLocator", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.DestroyLocator()));
      this.eatonfloorstate.moveto.InitializeStates(this.eater, this.locator, this.eatonfloorstate.eat, this.eatonfloorstate.eat, (CellOffset[]) null, (NavTactic) null);
      this.eatonfloorstate.eat.ToggleAnims("anim_eat_floor_kanim", 0.0f).DoEat(this.ediblechunk, this.actualfoodunits, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null);
    }

    public class EatOnFloorState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
    {
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
    }

    public class EatAtMessStationState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
    {
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<MessStation> moveto;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
    }
  }
}
