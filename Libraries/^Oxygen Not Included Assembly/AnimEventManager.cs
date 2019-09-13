// Decompiled with JetBrains decompiler
// Type: AnimEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AnimEventManager
{
  private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();
  private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);
  private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);
  private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);
  private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);
  private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);
  private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();
  private const int INITIAL_VECTOR_SIZE = 256;

  public void FreeResources()
  {
  }

  public HandleVector<int>.Handle PlayAnim(
    KAnimControllerBase controller,
    KAnim.Anim anim,
    KAnim.PlayMode mode,
    float time,
    bool use_unscaled_time)
  {
    AnimEventManager.AnimData initial_data1 = new AnimEventManager.AnimData();
    initial_data1.frameRate = anim.frameRate;
    initial_data1.totalTime = anim.totalTime;
    initial_data1.numFrames = anim.numFrames;
    initial_data1.useUnscaledTime = use_unscaled_time;
    AnimEventManager.EventPlayerData initial_data2 = new AnimEventManager.EventPlayerData()
    {
      elapsedTime = time,
      mode = mode,
      controller = controller as KBatchedAnimController
    };
    initial_data2.currentFrame = initial_data2.controller.GetFrameIdx(initial_data2.elapsedTime, false);
    initial_data2.previousFrame = -1;
    initial_data2.events = (List<AnimEvent>) null;
    initial_data2.updatingEvents = (List<AnimEvent>) null;
    initial_data2.events = GameAudioSheets.Get().GetEvents(anim.id);
    if (initial_data2.events == null)
      initial_data2.events = AnimEventManager.emptyEventList;
    return !initial_data1.useUnscaledTime ? this.indirectionData.Allocate(new AnimEventManager.IndirectionData(this.animData.Allocate(initial_data1), this.eventData.Allocate(initial_data2), false)) : this.indirectionData.Allocate(new AnimEventManager.IndirectionData(this.uiAnimData.Allocate(initial_data1), this.uiEventData.Allocate(initial_data2), true));
  }

  public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
  {
    if (!handle.IsValid())
      return;
    AnimEventManager.IndirectionData data1 = this.indirectionData.GetData(handle);
    KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = !data1.isUIData ? this.eventData : this.uiEventData;
    AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data1.eventDataHandle);
    data2.mode = mode;
    kcompactedVector.SetData(data1.eventDataHandle, data2);
  }

  public void StopAnim(HandleVector<int>.Handle handle)
  {
    if (!handle.IsValid())
      return;
    AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
    KCompactedVector<AnimEventManager.AnimData> kcompactedVector1 = !data.isUIData ? this.animData : this.uiAnimData;
    KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = !data.isUIData ? this.eventData : this.uiEventData;
    this.StopEvents(kcompactedVector2.GetData(data.eventDataHandle));
    data.animDataHandle = kcompactedVector1.Free(data.animDataHandle);
    data.eventDataHandle = kcompactedVector2.Free(data.eventDataHandle);
    this.indirectionData.SetData(handle, data);
  }

  public float GetElapsedTime(HandleVector<int>.Handle handle)
  {
    AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
    return (!data.isUIData ? this.eventData : this.uiEventData).GetData(data.eventDataHandle).elapsedTime;
  }

  public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
  {
    AnimEventManager.IndirectionData data1 = this.indirectionData.GetData(handle);
    KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = !data1.isUIData ? this.eventData : this.uiEventData;
    AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data1.eventDataHandle);
    data2.elapsedTime = elapsed_time;
    kcompactedVector.SetData(data1.eventDataHandle, data2);
  }

  public void Update()
  {
    float deltaTime = Time.deltaTime;
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
    this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
    for (int index = 0; index < this.finishedCalls.Count; ++index)
      this.finishedCalls[index].TriggerStop();
    this.finishedCalls.Clear();
  }

  private void Update(
    float dt,
    List<AnimEventManager.AnimData> anim_data,
    List<AnimEventManager.EventPlayerData> event_data)
  {
    if ((double) dt <= 0.0)
      return;
    for (int index1 = 0; index1 < event_data.Count; ++index1)
    {
      AnimEventManager.EventPlayerData eventPlayerData = event_data[index1];
      if (!((Object) eventPlayerData.controller == (Object) null))
      {
        eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
        event_data[index1] = eventPlayerData;
        this.PlayEvents(eventPlayerData);
        eventPlayerData.previousFrame = eventPlayerData.currentFrame;
        eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
        event_data[index1] = eventPlayerData;
        if (eventPlayerData.mode != KAnim.PlayMode.Paused)
        {
          if (eventPlayerData.updatingEvents != null)
          {
            for (int index2 = 0; index2 < eventPlayerData.updatingEvents.Count; ++index2)
              eventPlayerData.updatingEvents[index2].OnUpdate(eventPlayerData);
          }
          event_data[index1] = eventPlayerData;
          if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[index1].numFrames - 1)
          {
            this.StopEvents(eventPlayerData);
            this.finishedCalls.Add(eventPlayerData.controller);
          }
        }
      }
    }
  }

  private void PlayEvents(AnimEventManager.EventPlayerData data)
  {
    for (int index = 0; index < data.events.Count; ++index)
      data.events[index].Play(data);
  }

  private void StopEvents(AnimEventManager.EventPlayerData data)
  {
    for (int index = 0; index < data.events.Count; ++index)
      data.events[index].Stop(data);
    if (data.updatingEvents == null)
      return;
    data.updatingEvents.Clear();
  }

  private struct AnimData
  {
    public float frameRate;
    public float totalTime;
    public int numFrames;
    public bool useUnscaledTime;
  }

  [DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
  public struct EventPlayerData
  {
    public float elapsedTime;
    public KAnim.PlayMode mode;
    public List<AnimEvent> events;
    public List<AnimEvent> updatingEvents;
    public KBatchedAnimController controller;

    public int currentFrame { get; set; }

    public int previousFrame { get; set; }

    public ComponentType GetComponent<ComponentType>()
    {
      return this.controller.GetComponent<ComponentType>();
    }

    public string name
    {
      get
      {
        return this.controller.name;
      }
    }

    public float normalizedTime
    {
      get
      {
        return this.elapsedTime / this.controller.CurrentAnim.totalTime;
      }
    }

    public string currentAnimFile
    {
      get
      {
        return this.controller.currentAnimFile;
      }
    }

    public KAnimHashedString currentAnimFileHash
    {
      get
      {
        return this.controller.currentAnimFileHash;
      }
    }

    public string currentAnim
    {
      get
      {
        return this.controller.currentAnim;
      }
    }

    public Vector3 position
    {
      get
      {
        return this.controller.transform.GetPosition();
      }
    }

    public void AddUpdatingEvent(AnimEvent ev)
    {
      if (this.updatingEvents == null)
        this.updatingEvents = new List<AnimEvent>();
      this.updatingEvents.Add(ev);
    }

    public void SetElapsedTime(float elapsedTime)
    {
      this.elapsedTime = elapsedTime;
    }

    public void FreeResources()
    {
      this.elapsedTime = 0.0f;
      this.mode = KAnim.PlayMode.Once;
      this.currentFrame = 0;
      this.previousFrame = 0;
      this.events = (List<AnimEvent>) null;
      this.updatingEvents = (List<AnimEvent>) null;
      this.controller = (KBatchedAnimController) null;
    }
  }

  private struct IndirectionData
  {
    public bool isUIData;
    public HandleVector<int>.Handle animDataHandle;
    public HandleVector<int>.Handle eventDataHandle;

    public IndirectionData(
      HandleVector<int>.Handle anim_data_handle,
      HandleVector<int>.Handle event_data_handle,
      bool is_ui_data)
    {
      this.isUIData = is_ui_data;
      this.animDataHandle = anim_data_handle;
      this.eventDataHandle = event_data_handle;
    }
  }
}
