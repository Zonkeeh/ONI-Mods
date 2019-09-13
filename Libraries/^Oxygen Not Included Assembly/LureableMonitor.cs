// Decompiled with JetBrains decompiler
// Type: LureableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
  public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.cooldown.ScheduleGoTo((Func<LureableMonitor.Instance, float>) (smi => smi.def.cooldown), (StateMachine.BaseState) this.nolure);
    this.nolure.Update("FindLure", (System.Action<LureableMonitor.Instance, float>) ((smi, dt) => smi.FindLure()), UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>((StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>) this.targetLure, this.haslure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
    this.haslure.ParamTransition<GameObject>((StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>) this.targetLure, this.haslure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null)).Update("FindLure", (System.Action<LureableMonitor.Instance, float>) ((smi, dt) => smi.FindLure()), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.MoveToLure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasLure()), (System.Action<LureableMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float cooldown = 20f;
    public Tag[] lures;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_LURE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_LURE, Descriptor.DescriptorType.Effect, false)
      };
    }
  }

  public class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, LureableMonitor.Def def)
      : base(master, def)
    {
    }

    public void FindLure()
    {
      LureableMonitor.Instance.LureIterator iterator = new LureableMonitor.Instance.LureIterator(this.GetComponent<Navigator>(), this.def.lures);
      GameScenePartitioner.Instance.Iterate<LureableMonitor.Instance.LureIterator>(Grid.PosToCell(this.smi.transform.GetPosition()), 1, GameScenePartitioner.Instance.lure, ref iterator);
      iterator.Cleanup();
      this.sm.targetLure.Set(iterator.result, this);
    }

    public bool HasLure()
    {
      return (UnityEngine.Object) this.sm.targetLure.Get(this) != (UnityEngine.Object) null;
    }

    public GameObject GetTargetLure()
    {
      return this.sm.targetLure.Get(this);
    }

    private struct LureIterator : GameScenePartitioner.Iterator
    {
      private Navigator navigator;
      private Tag[] lures;

      public LureIterator(Navigator navigator, Tag[] lures)
      {
        this.navigator = navigator;
        this.lures = lures;
        this.cost = -1;
        this.result = (GameObject) null;
      }

      public int cost { get; private set; }

      public GameObject result { get; private set; }

      public void Iterate(object target_obj)
      {
        Lure.Instance instance = target_obj as Lure.Instance;
        if (instance == null || !instance.IsActive() || !instance.HasAnyLure(this.lures))
          return;
        int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.def.lurePoints);
        if (navigationCost == -1 || this.cost != -1 && navigationCost >= this.cost)
          return;
        this.cost = navigationCost;
        this.result = instance.gameObject;
      }

      public void Cleanup()
      {
      }
    }
  }
}
