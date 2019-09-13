// Decompiled with JetBrains decompiler
// Type: SolidConsumerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SolidConsumerMonitor : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
  private static TagBits plantMask = new TagBits(GameTags.GrowingPlant);
  private static TagBits creatureMask = new TagBits(new Tag[2]
  {
    GameTags.Creatures.ReservedByCreature,
    GameTags.CreatureBrain
  });
  private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State satisfied;
  private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State lookingforfood;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventHandler(GameHashes.EatSolidComplete, (GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnEatSolidComplete(data))).ToggleBehaviour(GameTags.Creatures.WantsToEat, (StateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.Transition.ConditionCallback) (smi =>
    {
      if ((UnityEngine.Object) smi.targetEdible != (UnityEngine.Object) null)
        return !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature);
      return false;
    }), (System.Action<SolidConsumerMonitor.Instance>) null);
    this.satisfied.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood, false);
    GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State state = this.lookingforfood.TagTransition(GameTags.Creatures.Hungry, this.satisfied, true);
    // ISSUE: reference to a compiler-generated field
    if (SolidConsumerMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SolidConsumerMonitor.\u003C\u003Ef__mg\u0024cache0 = new System.Action<SolidConsumerMonitor.Instance, float>(SolidConsumerMonitor.FindFood);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SolidConsumerMonitor.Instance, float> fMgCache0 = SolidConsumerMonitor.\u003C\u003Ef__mg\u0024cache0;
    state.Update(fMgCache0, UpdateRate.SIM_1000ms, true);
  }

  [Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
  private static void BeginDetailedSample(string region_name)
  {
  }

  [Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
  private static void EndDetailedSample()
  {
  }

  private static void FindFood(SolidConsumerMonitor.Instance smi, float dt)
  {
    ListPool<KMonoBehaviour, SolidConsumerMonitor>.PooledList pooledList1 = ListPool<KMonoBehaviour, SolidConsumerMonitor>.Allocate();
    ListPool<Storage, SolidConsumerMonitor>.PooledList pooledList2 = ListPool<Storage, SolidConsumerMonitor>.Allocate();
    foreach (Component component in Components.CreatureFeeders.Items)
    {
      component.GetComponents<Storage>((List<Storage>) pooledList2);
      foreach (Storage storage in (List<Storage>) pooledList2)
      {
        if (!((UnityEngine.Object) storage == (UnityEngine.Object) null))
        {
          foreach (GameObject gameObject in storage.items)
            pooledList1.Add(!((UnityEngine.Object) gameObject != (UnityEngine.Object) null) ? (KMonoBehaviour) null : gameObject.GetComponent<KMonoBehaviour>());
        }
      }
    }
    pooledList2.Recycle();
    int x = 0;
    int y = 0;
    Grid.PosToXY(smi.gameObject.transform.GetPosition(), out x, out y);
    int x_bottomLeft = x - 8;
    y -= 8;
    ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y, 16, 16, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList3);
    GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y, 16, 16, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) pooledList3);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList3)
      pooledList1.Add(partitionerEntry.obj as KMonoBehaviour);
    pooledList3.Recycle();
    Diet diet = smi.def.diet;
    for (int index = 0; index != pooledList1.Count; ++index)
    {
      KMonoBehaviour kmonoBehaviour = pooledList1[index];
      if (!((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null))
      {
        KPrefabID component1 = kmonoBehaviour.GetComponent<KPrefabID>();
        component1.UpdateTagBits();
        if (component1.HasAnyTags_AssumeLaundered(ref SolidConsumerMonitor.creatureMask) || diet.GetDietInfo(component1.PrefabTag) == null)
          pooledList1[index] = (KMonoBehaviour) null;
        else if (component1.HasAnyTags_AssumeLaundered(ref SolidConsumerMonitor.plantMask))
        {
          float num1 = 0.25f;
          float num2 = 0.0f;
          BuddingTrunk component2 = component1.GetComponent<BuddingTrunk>();
          if ((bool) ((UnityEngine.Object) component2))
          {
            num2 = component2.GetMaxBranchMaturity();
          }
          else
          {
            AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup((Component) component1);
            if (amountInstance != null)
              num2 = amountInstance.value / amountInstance.GetMax();
          }
          if ((double) num2 < (double) num1)
            pooledList1[index] = (KMonoBehaviour) null;
        }
      }
    }
    Navigator component3 = smi.GetComponent<Navigator>();
    smi.targetEdible = (GameObject) null;
    int num = -1;
    foreach (KMonoBehaviour kmonoBehaviour in (List<KMonoBehaviour>) pooledList1)
    {
      if (!((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null))
      {
        int navigationCost = component3.GetNavigationCost(Grid.PosToCell(kmonoBehaviour.gameObject.transform.GetPosition()));
        if (navigationCost != -1 && (navigationCost < num || num == -1))
        {
          num = navigationCost;
          smi.targetEdible = kmonoBehaviour.gameObject;
        }
      }
    }
    pooledList1.Recycle();
  }

  public class Def : StateMachine.BaseDef
  {
    public Diet diet;
  }

  public class Instance : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameInstance
  {
    public GameObject targetEdible;

    public Instance(IStateMachineTarget master, SolidConsumerMonitor.Def def)
      : base(master, def)
    {
    }

    public void OnEatSolidComplete(object data)
    {
      KPrefabID cmp = data as KPrefabID;
      if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
        return;
      PrimaryElement component1 = cmp.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
        return;
      Diet.Info dietInfo = this.def.diet.GetDietInfo(cmp.PrefabTag);
      if (dietInfo == null)
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.smi.gameObject);
      string properName = cmp.GetProperName();
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, properName, cmp.transform, 1.5f, false);
      float calories1 = amountInstance.GetMax() - amountInstance.value;
      float num = dietInfo.ConvertCaloriesToConsumptionMass(calories1);
      Growing component2 = cmp.GetComponent<Growing>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        BuddingTrunk component3 = cmp.GetComponent<BuddingTrunk>();
        if ((bool) ((UnityEngine.Object) component3))
          component3.ConsumeMass(num);
        else
          component2.ConsumeMass(num);
      }
      else
      {
        num = Mathf.Min(num, component1.Mass);
        component1.Mass -= num;
        Pickupable component3 = component1.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component3.storage != (UnityEngine.Object) null)
        {
          component3.storage.Trigger(-1452790913, (object) this.gameObject);
          component3.storage.Trigger(-1697596308, (object) this.gameObject);
        }
      }
      float calories2 = dietInfo.ConvertConsumptionMassToCalories(num);
      this.Trigger(-2038961714, (object) new CreatureCalorieMonitor.CaloriesConsumedEvent()
      {
        tag = cmp.PrefabTag,
        calories = calories2
      });
      this.targetEdible = (GameObject) null;
    }
  }
}
