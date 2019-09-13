// Decompiled with JetBrains decompiler
// Type: MinionBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using UnityEngine;

public class MinionBrain : Brain
{
  private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>((System.Action<MinionBrain, object>) ((component, data) => component.AnimTrackStoredItem(data)));
  private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>((System.Action<MinionBrain, object>) ((component, data) => component.OnUnstableGroundImpact(data)));
  [MyCmpReq]
  public Navigator Navigator;
  [MyCmpGet]
  public OxygenBreather OxygenBreather;
  private float lastResearchCompleteEmoteTime;

  public bool IsCellClear(int cell)
  {
    if (Grid.Reserved[cell])
      return false;
    GameObject gameObject = Grid.Objects[cell, 0];
    return !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) gameObject) || gameObject.GetComponent<Navigator>().IsMoving();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Navigator.SetAbilities((PathFinderAbilities) new MinionPathFinderAbilities(this.Navigator));
    this.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
    this.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (GameObject go in this.GetComponent<Storage>().items)
      this.AddAnimTracker(go);
    Game.Instance.Subscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
  }

  private void AnimTrackStoredItem(object data)
  {
    Storage component = this.GetComponent<Storage>();
    GameObject go = (GameObject) data;
    this.RemoveTracker(go);
    if (!component.items.Contains(go))
      return;
    this.AddAnimTracker(go);
  }

  private void AddAnimTracker(GameObject go)
  {
    KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.AnimFiles == null || (component.AnimFiles.Length <= 0 || !((UnityEngine.Object) component.AnimFiles[0] != (UnityEngine.Object) null)) || !component.GetComponent<Pickupable>().trackOnPickup)
      return;
    KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.useTargetPoint = false;
    kbatchedAnimTracker.fadeOut = false;
    kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
    kbatchedAnimTracker.forceAlwaysVisible = true;
  }

  private void RemoveTracker(GameObject go)
  {
    KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  public override void UpdateBrain()
  {
    base.UpdateBrain();
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return;
    if (!Game.Instance.savedInfo.discoveredSurface && World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(this.gameObject)) == SubWorld.ZoneType.Space)
    {
      Game.Instance.savedInfo.discoveredSurface = true;
      DiscoveredSpaceMessage discoveredSpaceMessage = new DiscoveredSpaceMessage(this.gameObject.transform.GetPosition());
      Messenger.Instance.QueueMessage((Message) discoveredSpaceMessage);
      Game.Instance.Trigger(-818188514, (object) this.gameObject);
    }
    if (Game.Instance.savedInfo.discoveredOilField || World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(this.gameObject)) != SubWorld.ZoneType.OilField)
      return;
    Game.Instance.savedInfo.discoveredOilField = true;
  }

  private void RegisterReactEmotePair(
    string reactable_id,
    string kanim_file_name,
    float max_trigger_time)
  {
    if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
    if (smi == null)
      return;
    EmoteChore emote = new EmoteChore((IStateMachineTarget) this.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, (HashedString) kanim_file_name, new HashedString[1]
    {
      (HashedString) "react"
    }, (Func<StatusItem>) null);
    SelfEmoteReactable reactable = new SelfEmoteReactable(this.gameObject, (HashedString) reactable_id, Db.Get().ChoreTypes.Cough, (HashedString) kanim_file_name, max_trigger_time, 20f, float.PositiveInfinity);
    emote.PairReactable(reactable);
    reactable.AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "react"
    });
    reactable.PairEmote(emote);
    smi.AddOneshotReactable(reactable);
  }

  private void OnResearchComplete(object data)
  {
    if ((double) Time.time - (double) this.lastResearchCompleteEmoteTime <= 1.0)
      return;
    this.RegisterReactEmotePair("ResearchComplete", "anim_react_research_complete_kanim", 3f);
    this.lastResearchCompleteEmoteTime = Time.time;
  }

  private void OnUnstableGroundImpact(object data)
  {
    this.RegisterReactEmotePair("UnstableGroundShock", "anim_react_shock_kanim", 1f);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.Unsubscribe(-107300940, new System.Action<object>(this.OnResearchComplete));
  }
}
