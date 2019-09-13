// Decompiled with JetBrains decompiler
// Type: EntityThreatMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EntityThreatMonitor : GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>
{
  public GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.State safe;
  public GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.State threatened;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.safe;
    this.root.EventHandler(GameHashes.ObjectDestroyed, (GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.Cleanup(d)));
    this.safe.Enter((StateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.State.Callback) (smi => smi.RefreshThreat((object) null))).Update("safe", (System.Action<EntityThreatMonitor.Instance, float>) ((smi, dt) => smi.RefreshThreat((object) null)), UpdateRate.SIM_1000ms, true);
    GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.State state = this.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (StateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.MainThreat != (UnityEngine.Object) null), (System.Action<EntityThreatMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.safe)));
    // ISSUE: reference to a compiler-generated field
    if (EntityThreatMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EntityThreatMonitor.\u003C\u003Ef__mg\u0024cache0 = new System.Action<EntityThreatMonitor.Instance, float>(EntityThreatMonitor.CritterUpdateThreats);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<EntityThreatMonitor.Instance, float> fMgCache0 = EntityThreatMonitor.\u003C\u003Ef__mg\u0024cache0;
    state.Update("Threatened", fMgCache0, UpdateRate.SIM_200ms, false);
  }

  private static void CritterUpdateThreats(EntityThreatMonitor.Instance smi, float dt)
  {
    if (smi.isMasterNull || smi.CheckForThreats())
      return;
    smi.GoTo((StateMachine.BaseState) smi.sm.safe);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<EntityThreatMonitor, EntityThreatMonitor.Instance, IStateMachineTarget, EntityThreatMonitor.Def>.GameInstance
  {
    private List<FactionAlignment> threats = new List<FactionAlignment>();
    private int maxThreatDistance = 6;
    public GameObject entityToProtect;
    public FactionAlignment alignment;
    private Navigator navigator;
    public ChoreDriver choreDriver;
    public Tag allyTag;
    private GameObject mainThreat;
    private System.Action<object> refreshThreatDelegate;

    public Instance(IStateMachineTarget master, EntityThreatMonitor.Def def)
      : base(master, def)
    {
      this.alignment = master.GetComponent<FactionAlignment>();
      this.navigator = master.GetComponent<Navigator>();
      this.choreDriver = master.GetComponent<ChoreDriver>();
      this.refreshThreatDelegate = new System.Action<object>(this.RefreshThreat);
    }

    public GameObject MainThreat
    {
      get
      {
        return this.mainThreat;
      }
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
      this.smi.GoTo((StateMachine.BaseState) this.sm.threatened);
    }

    public void RefreshThreat(object data)
    {
      if (!this.IsRunning() || (UnityEngine.Object) this.entityToProtect == (UnityEngine.Object) null)
        return;
      if (this.smi.CheckForThreats())
      {
        this.GoToThreatened();
      }
      else
      {
        if (this.smi.GetCurrentState() == this.sm.safe)
          return;
        this.Trigger(-21431934, (object) null);
        this.smi.GoTo((StateMachine.BaseState) this.sm.safe);
      }
    }

    public bool CheckForThreats()
    {
      if ((UnityEngine.Object) this.entityToProtect == (UnityEngine.Object) null)
        return false;
      GameObject threat = this.FindThreat();
      this.SetMainThreat(threat);
      return (UnityEngine.Object) threat != (UnityEngine.Object) null;
    }

    public GameObject FindThreat()
    {
      this.threats.Clear();
      ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(new Extents(Grid.PosToCell(this.entityToProtect), this.maxThreatDistance), GameScenePartitioner.Instance.attackableEntitiesLayer, (List<ScenePartitionerEntry>) pooledList);
      for (int index = 0; index < pooledList.Count; ++index)
      {
        FactionAlignment cmp = pooledList[index].obj as FactionAlignment;
        if (!((UnityEngine.Object) cmp.transform == (UnityEngine.Object) null) && !((UnityEngine.Object) cmp == (UnityEngine.Object) this.alignment) && (cmp.IsAlignmentActive() && this.navigator.CanReach((IApproachable) cmp.attackable)) && (!(this.allyTag != (Tag) ((string) null)) || !cmp.HasTag(this.allyTag)))
          this.threats.Add(cmp);
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
  }
}
