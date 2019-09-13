// Decompiled with JetBrains decompiler
// Type: ClimbableTreeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
  private const int MAX_NAV_COST = 2147483647;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (StateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.Transition.ConditionCallback) (smi => smi.UpdateHasClimbable()), (System.Action<ClimbableTreeMonitor.Instance>) (smi => smi.OnClimbComplete()));
  }

  public class Def : StateMachine.BaseDef
  {
    public float searchMinInterval = 60f;
    public float searchMaxInterval = 120f;
  }

  public class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
  {
    public GameObject climbTarget;
    public float nextSearchTime;

    public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def)
      : base(master, def)
    {
      this.RefreshSearchTime();
    }

    private void RefreshSearchTime()
    {
      this.nextSearchTime = Time.time + Mathf.Lerp(this.def.searchMinInterval, this.def.searchMaxInterval, UnityEngine.Random.value);
    }

    public bool UpdateHasClimbable()
    {
      if ((UnityEngine.Object) this.climbTarget == (UnityEngine.Object) null)
      {
        if ((double) Time.time < (double) this.nextSearchTime)
          return false;
        this.FindClimbableTree();
        this.RefreshSearchTime();
      }
      return (UnityEngine.Object) this.climbTarget != (UnityEngine.Object) null;
    }

    private void FindClimbableTree()
    {
      this.climbTarget = (GameObject) null;
      ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList1 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
      ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList pooledList2 = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
      Extents extents = new Extents(Grid.PosToCell(this.master.transform.GetPosition()), 10);
      GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) pooledList1);
      GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, (List<ScenePartitionerEntry>) pooledList1);
      Navigator component1 = this.GetComponent<Navigator>();
      foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList1)
      {
        KMonoBehaviour cmp = partitionerEntry.obj as KMonoBehaviour;
        if (!cmp.HasTag(GameTags.Creatures.ReservedByCreature))
        {
          int cell = Grid.PosToCell(cmp);
          if (component1.CanReach(cell))
          {
            BuddingTrunk component2 = cmp.GetComponent<BuddingTrunk>();
            StorageLocker component3 = cmp.GetComponent<StorageLocker>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            {
              if (!component2.ExtraSeedAvailable)
                continue;
            }
            else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
            {
              Storage component4 = component3.GetComponent<Storage>();
              if (!component4.allowItemRemoval || component4.IsEmpty())
                continue;
            }
            else
              continue;
            pooledList2.Add(cmp);
          }
        }
      }
      if (pooledList2.Count > 0)
      {
        int index = UnityEngine.Random.Range(0, pooledList2.Count);
        this.climbTarget = pooledList2[index].gameObject;
      }
      pooledList1.Recycle();
      pooledList2.Recycle();
    }

    public void OnClimbComplete()
    {
      this.climbTarget = (GameObject) null;
    }
  }
}
