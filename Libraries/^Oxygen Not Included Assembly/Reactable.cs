// Decompiled with JetBrains decompiler
// Type: Reactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class Reactable
{
  public bool preventChoreInterruption = true;
  private int transformId = -1;
  public float maxTriggerTime = float.PositiveInfinity;
  private float lastTriggerTime = (float) int.MinValue;
  private HandleVector<int>.Handle partitionerEntry;
  protected GameObject gameObject;
  public HashedString id;
  public int sourceCell;
  private int rangeWidth;
  private int rangeHeight;
  public float minReactableTime;
  public float minReactorTime;
  private float creationTime;
  protected GameObject reactor;
  private ChoreType choreType;
  protected LoggerFSS log;
  private List<Reactable.ReactablePrecondition> additionalPreconditions;

  public Reactable(
    GameObject gameObject,
    HashedString id,
    ChoreType chore_type,
    int range_width = 15,
    int range_height = 8,
    bool follow_transform = false,
    float min_reactable_time = 0.0f,
    float min_reactor_time = 0.0f,
    float max_trigger_time = float.PositiveInfinity)
  {
    this.rangeHeight = range_height;
    this.rangeWidth = range_width;
    this.id = id;
    this.gameObject = gameObject;
    this.choreType = chore_type;
    this.minReactableTime = min_reactable_time;
    this.minReactorTime = min_reactor_time;
    this.maxTriggerTime = max_trigger_time;
    this.creationTime = GameClock.Instance.GetTime();
    this.UpdateLocation();
    if (!follow_transform)
      return;
    this.transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(gameObject.transform, new System.Action(this.UpdateLocation), "Reactable follow transform");
  }

  public bool IsReacting
  {
    get
    {
      return (UnityEngine.Object) this.reactor != (UnityEngine.Object) null;
    }
  }

  public void Begin(GameObject reactor)
  {
    this.reactor = reactor;
    this.lastTriggerTime = GameClock.Instance.GetTime();
    this.InternalBegin();
  }

  public void End()
  {
    this.InternalEnd();
    if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
      return;
    GameObject reactor = this.reactor;
    this.InternalEnd();
    this.reactor = (GameObject) null;
    if (!((UnityEngine.Object) reactor != (UnityEngine.Object) null))
      return;
    reactor.GetSMI<ReactionMonitor.Instance>()?.StopReaction();
  }

  public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
  {
    if ((double) GameClock.Instance.GetTime() - (double) this.lastTriggerTime < (double) this.minReactableTime)
      return false;
    ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    Chore currentChore = component.choreDriver.GetCurrentChore();
    if (currentChore == null || this.choreType.priority <= currentChore.choreType.priority)
      return false;
    if (this.additionalPreconditions != null)
    {
      foreach (Reactable.ReactablePrecondition additionalPrecondition in this.additionalPreconditions)
      {
        if (!additionalPrecondition(reactor, transition))
          return false;
      }
    }
    return this.InternalCanBegin(reactor, transition);
  }

  public bool IsExpired()
  {
    return (double) GameClock.Instance.GetTime() - (double) this.creationTime > (double) this.maxTriggerTime;
  }

  public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

  public abstract void Update(float dt);

  protected abstract void InternalBegin();

  protected abstract void InternalEnd();

  protected abstract void InternalCleanup();

  public void Cleanup()
  {
    this.End();
    this.InternalCleanup();
    if (this.transformId != -1)
    {
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transformId, new System.Action(this.UpdateLocation));
      this.transformId = -1;
    }
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }

  public void Sim1000ms(float dt)
  {
    this.UpdateLocation();
  }

  private void UpdateLocation()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    this.sourceCell = Grid.PosToCell(this.gameObject);
    this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (Reactable), (object) this, new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight), GameScenePartitioner.Instance.objectLayers[0], (System.Action<object>) null);
  }

  public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
  {
    if (this.additionalPreconditions == null)
      this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
    this.additionalPreconditions.Add(precondition);
    return this;
  }

  public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
