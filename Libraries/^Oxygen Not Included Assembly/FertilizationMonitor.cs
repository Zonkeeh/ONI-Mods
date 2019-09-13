// Decompiled with JetBrains decompiler
// Type: FertilizationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FertilizationMonitor : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
  public GameHashes ResourceRecievedEvent = GameHashes.Fertilized;
  public GameHashes ResourceDepletedEvent = GameHashes.Unfertilized;
  public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;
  public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasCorrectFertilizer;
  public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasIncorrectFertilizer;
  public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;
  public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;
  public FertilizationMonitor.ReplantedStates replanted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.wild;
    this.serializable = false;
    this.wild.ParamTransition<GameObject>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<GameObject>) this.fertilizerStorage, this.unfertilizable, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
    this.unfertilizable.Enter((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi =>
    {
      if (!smi.AcceptsFertilizer())
        return;
      smi.GoTo((StateMachine.BaseState) this.replanted.fertilized);
    }));
    this.replanted.Enter((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi =>
    {
      foreach (ManualDeliveryKG component in smi.gameObject.GetComponents<ManualDeliveryKG>())
        component.Pause(false, "replanted");
      smi.UpdateFertilization(0.03333334f);
    })).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.UpdateFertilization(0.2f))).Target(this.masterTarget);
    this.replanted.fertilized.DefaultState((GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted.fertilized.decaying).TriggerOnEnter(this.ResourceRecievedEvent, (Func<FertilizationMonitor.Instance, object>) null);
    this.replanted.fertilized.decaying.DefaultState(this.replanted.fertilized.decaying.normal).ToggleAttributeModifier("Consuming", (Func<FertilizationMonitor.Instance, AttributeModifier>) (smi => smi.consumptionRate), (Func<FertilizationMonitor.Instance, bool>) null).ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasCorrectFertilizer, (GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted.fertilized.absorbing, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue).Update("Decaying", (System.Action<FertilizationMonitor.Instance, float>) ((smi, dt) =>
    {
      if (!smi.Starved())
        return;
      smi.GoTo((StateMachine.BaseState) this.replanted.starved);
    }), UpdateRate.SIM_200ms, false);
    this.replanted.fertilized.decaying.normal.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
    this.replanted.fertilized.decaying.wrongFert.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
    this.replanted.fertilized.absorbing.DefaultState(this.replanted.fertilized.absorbing.normal).ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasCorrectFertilizer, (GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted.fertilized.decaying, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (Func<FertilizationMonitor.Instance, AttributeModifier>) (smi => smi.absorptionRate), (Func<FertilizationMonitor.Instance, bool>) null).Enter((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StartAbsorbing())).EventHandler(GameHashes.Wilt, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StopAbsorbing())).EventHandler(GameHashes.WiltRecover, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StartAbsorbing())).Exit((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StopAbsorbing()));
    this.replanted.fertilized.absorbing.normal.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
    this.replanted.fertilized.absorbing.wrongFert.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
    this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, (Func<FertilizationMonitor.Instance, object>) null).ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasCorrectFertilizer, (GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted.fertilized, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
    this.replanted.starved.normal.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.starved.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
    this.replanted.starved.wrongFert.ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.hasIncorrectFertilizer, this.replanted.starved.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Tag wrongFertilizerTestTag;
    public PlantElementAbsorber.ConsumeInfo[] consumedElements;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      if (this.consumedElements.Length <= 0)
        return (List<Descriptor>) null;
      List<Descriptor> descriptorList = new List<Descriptor>();
      foreach (PlantElementAbsorber.ConsumeInfo consumedElement in this.consumedElements)
        descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(-consumedElement.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(consumedElement.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
      return descriptorList;
    }
  }

  public class VariableFertilizerStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
  {
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State normal;
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wrongFert;
  }

  public class FertilizedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
  {
    public FertilizationMonitor.VariableFertilizerStates decaying;
    public FertilizationMonitor.VariableFertilizerStates absorbing;
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
  }

  public class ReplantedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
  {
    public FertilizationMonitor.FertilizedStates fertilized;
    public FertilizationMonitor.VariableFertilizerStates starved;
  }

  public class Instance : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance, IWiltCause
  {
    private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;
    public AttributeModifier consumptionRate;
    public AttributeModifier absorptionRate;
    protected AmountInstance fertilization;
    private Storage storage;
    private float total_available_mass;

    public Instance(IStateMachineTarget master, FertilizationMonitor.Def def)
      : base(master, def)
    {
      this.AddAmounts(this.gameObject);
      this.MakeModifiers();
      master.Subscribe(1309017699, new System.Action<object>(this.SetStorage));
    }

    public float total_fertilizer_available
    {
      get
      {
        return this.total_available_mass;
      }
    }

    public virtual StatusItem GetStarvedStatusItem()
    {
      return Db.Get().CreatureStatusItems.NeedsFertilizer;
    }

    public virtual StatusItem GetIncorrectFertStatusItem()
    {
      return Db.Get().CreatureStatusItems.WrongFertilizer;
    }

    public virtual StatusItem GetIncorrectFertStatusItemMajor()
    {
      return Db.Get().CreatureStatusItems.WrongFertilizerMajor;
    }

    protected virtual void AddAmounts(GameObject gameObject)
    {
      this.fertilization = gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
    }

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.Fertilized
        };
      }
    }

    public string WiltStateString
    {
      get
      {
        string str = string.Empty;
        if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.fertilized.decaying.wrongFert))
          str = this.GetIncorrectFertStatusItemMajor().resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, (object) this);
        else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.fertilized.absorbing.wrongFert))
          str = this.GetIncorrectFertStatusItem().resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGFERTILIZER.NAME, (object) this);
        else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.starved))
          str = this.GetStarvedStatusItem().resolveStringCallback((string) CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, (object) this);
        else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.starved.wrongFert))
          str = this.GetIncorrectFertStatusItemMajor().resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, (object) this);
        return str;
      }
    }

    protected virtual void MakeModifiers()
    {
      this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, -0.1666667f, (string) CREATURES.STATS.FERTILIZATION.CONSUME_MODIFIER, false, false, true);
      this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.666667f, (string) CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER, false, false, true);
    }

    public void SetStorage(object obj)
    {
      this.storage = (Storage) obj;
      this.sm.fertilizerStorage.Set((KMonoBehaviour) this.storage, this.smi);
      IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, this.smi.gameObject);
      foreach (ManualDeliveryKG component in this.smi.gameObject.GetComponents<ManualDeliveryKG>())
      {
        bool flag = false;
        foreach (PlantElementAbsorber.ConsumeInfo consumedElement in this.def.consumedElements)
        {
          if (component.requestedItemTag == consumedElement.tag)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          component.SetStorage(this.storage);
          component.enabled = true;
        }
      }
    }

    public virtual bool AcceptsFertilizer()
    {
      PlantablePlot component = this.sm.fertilizerStorage.Get(this).GetComponent<PlantablePlot>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component.AcceptsFertilizer;
      return false;
    }

    public bool Starved()
    {
      return (double) this.fertilization.value == 0.0;
    }

    public void UpdateFertilization(float dt)
    {
      if (this.def.consumedElements == null || (UnityEngine.Object) this.storage == (UnityEngine.Object) null)
        return;
      bool flag1 = true;
      bool flag2 = false;
      List<GameObject> items = this.storage.items;
      for (int index1 = 0; index1 < this.def.consumedElements.Length; ++index1)
      {
        PlantElementAbsorber.ConsumeInfo consumedElement = this.def.consumedElements[index1];
        float num = 0.0f;
        for (int index2 = 0; index2 < items.Count; ++index2)
        {
          GameObject go = items[index2];
          if (go.HasTag(consumedElement.tag))
            num += go.GetComponent<PrimaryElement>().Mass;
          else if (go.HasTag(this.def.wrongFertilizerTestTag))
            flag2 = true;
        }
        this.total_available_mass = num;
        if ((double) num < (double) consumedElement.massConsumptionRate * (double) dt)
        {
          flag1 = false;
          break;
        }
      }
      this.sm.hasCorrectFertilizer.Set(flag1, this.smi);
      this.sm.hasIncorrectFertilizer.Set(flag2, this.smi);
    }

    public void StartAbsorbing()
    {
      if (this.absorberHandle.IsValid() || this.def.consumedElements == null || this.def.consumedElements.Length == 0)
        return;
      GameObject gameObject = this.smi.gameObject;
      this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, this.def.consumedElements);
    }

    public void StopAbsorbing()
    {
      if (!this.absorberHandle.IsValid())
        return;
      this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
    }
  }
}
