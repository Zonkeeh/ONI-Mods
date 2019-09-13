// Decompiled with JetBrains decompiler
// Type: EggProtectionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProtectionMonitor : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
  public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;
  public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;
  public EggProtectionMonitor.GuardEggStates guard;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.find_egg;
    this.root.EventHandler(GameHashes.ObjectDestroyed, (GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.Cleanup(d)));
    GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State findEgg = this.find_egg;
    // ISSUE: reference to a compiler-generated field
    if (EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache0 = new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard);
    }
    // ISSUE: reference to a compiler-generated field
    UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate fMgCache0 = EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache0;
    findEgg.BatchUpdate(fMgCache0, UpdateRate.SIM_200ms).ParamTransition<bool>((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Parameter<bool>) this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
    this.guard.Enter((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) "pincher_kanim"), (string) null, "_heat", 0);
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
    })).Exit((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim((HashedString) "pincher_kanim").GetData(), 0);
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
    })).Update("evaulate_egg", (System.Action<EggProtectionMonitor.Instance, float>) ((smi, dt) => smi.CanProtectEgg()), UpdateRate.SIM_1000ms, true).ParamTransition<bool>((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Parameter<bool>) this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
    this.guard.safe.Enter((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi => smi.RefreshThreat((object) null))).Update("safe", (System.Action<EggProtectionMonitor.Instance, float>) ((smi, dt) => smi.RefreshThreat((object) null)), UpdateRate.SIM_200ms, true);
    GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State state = this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.MainThreat != (UnityEngine.Object) null), (System.Action<EggProtectionMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.guard.safe)));
    // ISSUE: reference to a compiler-generated field
    if (EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<EggProtectionMonitor.Instance, float> fMgCache1 = EggProtectionMonitor.\u003C\u003Ef__mg\u0024cache1;
    state.Update("Threatened", fMgCache1, UpdateRate.SIM_200ms, false);
  }

  private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
  {
    if (smi.isMasterNull || smi.CheckForThreats())
      return;
    smi.GoTo((StateMachine.BaseState) smi.sm.guard.safe);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag[] allyTags;
  }

  public class GuardEggStates : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
  {
    public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;
    public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
  }

  public class Instance : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
  {
    private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();
    private List<FactionAlignment> threats = new List<FactionAlignment>();
    private int maxThreatDistance = 12;
    public GameObject eggToProtect;
    public FactionAlignment alignment;
    private Navigator navigator;
    private GameObject mainThreat;
    private System.Action<object> refreshThreatDelegate;

    public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def)
      : base(master, def)
    {
      this.alignment = master.GetComponent<FactionAlignment>();
      this.navigator = master.GetComponent<Navigator>();
      this.refreshThreatDelegate = new System.Action<object>(this.RefreshThreat);
    }

    public GameObject MainThreat
    {
      get
      {
        return this.mainThreat;
      }
    }

    public void CanProtectEgg()
    {
      bool flag = true;
      if ((UnityEngine.Object) this.eggToProtect == (UnityEngine.Object) null)
        flag = false;
      if (flag)
      {
        int num = 150;
        int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
        if (navigationCost == -1 || navigationCost >= num)
          flag = false;
      }
      if (flag)
        return;
      this.SetEggToGuard((GameObject) null);
    }

    public static void FindEggToGuard(
      List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances,
      float time_delta)
    {
      ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList1 = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
      pooledList1.Capacity = Mathf.Max(pooledList1.Capacity, Components.Pickupables.Count);
      IEnumerator enumerator = Components.Pickupables.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Pickupable current = (Pickupable) enumerator.Current;
          pooledList1.Add(current.gameObject.GetComponent<KPrefabID>());
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList pooledList2 = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
      EggProtectionMonitor.Instance.find_eggs_job.Reset((List<KPrefabID>) pooledList1);
      for (int start = 0; start < pooledList1.Count; start += 256)
        EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(start, Mathf.Min(start + 256, pooledList1.Count)));
      GlobalJobManager.Run((IWorkItemCollection) EggProtectionMonitor.Instance.find_eggs_job);
      for (int idx = 0; idx != EggProtectionMonitor.Instance.find_eggs_job.Count; ++idx)
        EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(idx).Finish((List<KPrefabID>) pooledList1, (List<EggProtectionMonitor.Instance.Egg>) pooledList2);
      pooledList1.Recycle();
      foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry instance in instances)
      {
        GameObject egg1 = (GameObject) null;
        int num = 100;
        foreach (EggProtectionMonitor.Instance.Egg egg2 in (List<EggProtectionMonitor.Instance.Egg>) pooledList2)
        {
          int navigationCost = instance.data.navigator.GetNavigationCost(egg2.cell);
          if (navigationCost != -1 && navigationCost < num)
          {
            egg1 = egg2.game_object;
            num = navigationCost;
          }
        }
        instance.data.SetEggToGuard(egg1);
      }
      pooledList2.Recycle();
    }

    public void SetEggToGuard(GameObject egg)
    {
      this.eggToProtect = egg;
      this.sm.hasEggToGuard.Set((UnityEngine.Object) egg != (UnityEngine.Object) null, this.smi);
    }

    public void SetMainThreat(GameObject threat)
    {
      if ((UnityEngine.Object) threat == (UnityEngine.Object) this.mainThreat)
        return;
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
        if ((UnityEngine.Object) threat == (UnityEngine.Object) null)
          this.Trigger(2144432245, (object) null);
      }
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
      }
      this.mainThreat = threat;
      if (!((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null))
        return;
      this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
    }

    public void Cleanup(object data)
    {
      if (!(bool) ((UnityEngine.Object) this.mainThreat))
        return;
      this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
    }

    public void GoToThreatened()
    {
      this.smi.GoTo((StateMachine.BaseState) this.sm.guard.threatened);
    }

    public void RefreshThreat(object data)
    {
      if (!this.IsRunning() || (UnityEngine.Object) this.eggToProtect == (UnityEngine.Object) null)
        return;
      if (this.smi.CheckForThreats())
      {
        this.GoToThreatened();
      }
      else
      {
        if (this.smi.GetCurrentState() == this.sm.guard.safe)
          return;
        this.Trigger(-21431934, (object) null);
        this.smi.GoTo((StateMachine.BaseState) this.sm.guard.safe);
      }
    }

    public bool CheckForThreats()
    {
      if ((UnityEngine.Object) this.eggToProtect == (UnityEngine.Object) null)
        return false;
      GameObject threat = this.FindThreat();
      this.SetMainThreat(threat);
      return (UnityEngine.Object) threat != (UnityEngine.Object) null;
    }

    public GameObject FindThreat()
    {
      this.threats.Clear();
      ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(new Extents(Grid.PosToCell(this.eggToProtect), this.maxThreatDistance), GameScenePartitioner.Instance.attackableEntitiesLayer, (List<ScenePartitionerEntry>) pooledList);
      for (int index = 0; index < pooledList.Count; ++index)
      {
        FactionAlignment cmp = pooledList[index].obj as FactionAlignment;
        if (!((UnityEngine.Object) cmp.transform == (UnityEngine.Object) null) && !((UnityEngine.Object) cmp == (UnityEngine.Object) this.alignment) && (cmp.IsAlignmentActive() && this.navigator.CanReach((IApproachable) cmp.attackable)))
        {
          bool flag = false;
          foreach (Tag allyTag in this.def.allyTags)
          {
            if (cmp.HasTag(allyTag))
              flag = true;
          }
          if (!flag)
            this.threats.Add(cmp);
        }
      }
      pooledList.Recycle();
      return this.PickBestTarget(this.threats);
    }

    public GameObject PickBestTarget(List<FactionAlignment> threats)
    {
      float num1 = 1f;
      Vector2 position = (Vector2) this.gameObject.transform.GetPosition();
      GameObject gameObject = (GameObject) null;
      float num2 = float.PositiveInfinity;
      for (int index = threats.Count - 1; index >= 0; --index)
      {
        FactionAlignment threat = threats[index];
        float num3 = Vector2.Distance(position, (Vector2) threat.transform.GetPosition()) / num1;
        if ((double) num3 < (double) num2)
        {
          num2 = num3;
          gameObject = threat.gameObject;
        }
      }
      return gameObject;
    }

    private struct Egg
    {
      public GameObject game_object;
      public int cell;
    }

    private struct FindEggsTask : IWorkItem<List<KPrefabID>>
    {
      private static readonly Tag EGG_TAG = "CrabEgg".ToTag();
      private ListPool<int, EggProtectionMonitor>.PooledList eggs;
      private int start;
      private int end;

      public FindEggsTask(int start, int end)
      {
        this.start = start;
        this.end = end;
        this.eggs = ListPool<int, EggProtectionMonitor>.Allocate();
      }

      public void Run(List<KPrefabID> prefab_ids)
      {
        for (int start = this.start; start != this.end; ++start)
        {
          if (prefab_ids[start].HasTag(EggProtectionMonitor.Instance.FindEggsTask.EGG_TAG))
            this.eggs.Add(start);
        }
      }

      public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
      {
        foreach (int egg in (List<int>) this.eggs)
        {
          GameObject gameObject = prefab_ids[egg].gameObject;
          eggs.Add(new EggProtectionMonitor.Instance.Egg()
          {
            game_object = gameObject,
            cell = Grid.PosToCell(gameObject)
          });
        }
        this.eggs.Recycle();
      }
    }
  }
}
