// Decompiled with JetBrains decompiler
// Type: Trap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class Trap : StateMachineComponent<Trap.StatesInstance>
{
  public Vector2 trappedOffset = Vector2.zero;
  public TagSet captureTags = new TagSet();
  public Tag[] trappableCreatures;
  [Serialize]
  private Ref<KPrefabID> contents;
  private static StatusItem statusReady;
  private static StatusItem statusSprung;

  private void SetStoredPosition(GameObject go)
  {
    Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell(this.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
    posCbc.x += this.trappedOffset.x;
    posCbc.y += this.trappedOffset.y;
    go.transform.SetPosition(posCbc);
    go.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingBack);
  }

  private static void CreateStatusItems()
  {
    if (Trap.statusSprung != null)
      return;
    Trap.statusReady = new StatusItem("Ready", (string) BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, (string) BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
    Trap.statusSprung = new StatusItem("Sprung", (string) BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, (string) BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
    Trap.statusSprung.resolveTooltipCallback = (Func<string, object, string>) ((str, obj) =>
    {
      Trap.StatesInstance statesInstance = (Trap.StatesInstance) obj;
      return string.Format(str, (object) statesInstance.master.contents.Get().GetProperName());
    });
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.contents = new Ref<KPrefabID>();
    Trap.CreateStatusItems();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Storage component1 = this.GetComponent<Storage>();
    foreach (GameObject go in component1.items)
    {
      this.SetStoredPosition(go);
      KBoxCollider2D component2 = go.GetComponent<KBoxCollider2D>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.enabled = true;
    }
    this.smi.StartSM();
    if (component1.IsEmpty())
      return;
    KPrefabID component3 = component1.items[0].GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      this.contents.Set(component3);
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.occupied);
    }
    else
      component1.DropAll(false, false, new Vector3(), true);
  }

  public KPrefabID GetContents()
  {
    return this.contents.Get();
  }

  public class StatesInstance : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance
  {
    private HandleVector<int>.Handle partitionerEntry;

    public StatesInstance(Trap master)
      : base(master)
    {
      this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (Trap), (object) this.gameObject, Grid.PosToCell(this.gameObject), GameScenePartitioner.Instance.trapsLayer, new System.Action<object>(this.OnCreatureOnTrap));
    }

    public void OnCreatureOnTrap(object data)
    {
      Storage component = this.master.GetComponent<Storage>();
      if (!component.IsEmpty())
        return;
      Trappable cmp = (Trappable) data;
      if (cmp.HasTag(GameTags.Stored) || cmp.HasTag(GameTags.Trapped) || cmp.HasTag(GameTags.Creatures.Bagged))
        return;
      bool flag = false;
      foreach (Tag trappableCreature in this.master.trappableCreatures)
      {
        if (cmp.HasTag(trappableCreature))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      this.master.contents.Set(cmp.GetComponent<KPrefabID>());
      component.Store(cmp.gameObject, true, false, true, false);
      this.master.SetStoredPosition(cmp.gameObject);
      this.smi.sm.trapTriggered.Trigger(this.smi);
    }

    public override void StopSM(string reason)
    {
      this.DisableEvents();
      base.StopSM(reason);
    }

    public void DisableEvents()
    {
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    }
  }

  public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
  {
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;
    public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;
    public Trap.States.OccupiedStates occupied;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.ready;
      this.serializable = false;
      Trap.CreateStatusItems();
      this.ready.OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady, (object) null).Exit((StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State.Callback) (smi => smi.DisableEvents()));
      this.trapping.PlayAnim("working_pre").OnAnimQueueComplete((GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State) this.occupied);
      this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Func<Trap.StatesInstance, object>) (smi => (object) smi)).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Storage>().IsEmpty()));
      this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
      this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
      this.destroySelf.Enter((StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State.Callback) (smi => Util.KDestroyGameObject(smi.master.gameObject)));
    }

    public class OccupiedStates : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
    {
      public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
    }
  }
}
