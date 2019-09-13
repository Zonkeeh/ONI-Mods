// Decompiled with JetBrains decompiler
// Type: LoopingSoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
  private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();
  private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>(0);
  private static LoopingSoundManager instance;

  public static void DestroyInstance()
  {
    LoopingSoundManager.instance = (LoopingSoundManager) null;
  }

  protected override void OnPrefabInit()
  {
    LoopingSoundManager.instance = this;
    this.CollectParameterUpdaters();
  }

  protected override void OnSpawn()
  {
    if (!((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Subscribe(-1788536802, new System.Action<object>(LoopingSoundManager.instance.OnPauseChanged));
  }

  private void CollectParameterUpdaters()
  {
    foreach (System.Type currentDomainType in App.GetCurrentDomainTypes())
    {
      if (!currentDomainType.IsAbstract)
      {
        bool flag = false;
        for (System.Type baseType = currentDomainType.BaseType; baseType != null; baseType = baseType.BaseType)
        {
          if (baseType == typeof (LoopingSoundParameterUpdater))
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          LoopingSoundParameterUpdater instance = (LoopingSoundParameterUpdater) Activator.CreateInstance(currentDomainType);
          DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(instance.parameter));
          this.parameterUpdaters[instance.parameter] = instance;
        }
      }
    }
  }

  public void UpdateFirstParameter(
    HandleVector<int>.Handle handle,
    HashedString parameter,
    float value)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.firstParameterValue = value;
    data.firstParameter = parameter;
    if (data.IsPlaying)
    {
      int num = (int) data.ev.setParameterValueByIndex(this.GetSoundDescription(data.path).GetParameterIdx(parameter), value);
    }
    this.sounds.SetData(handle, data);
  }

  public void UpdateSecondParameter(
    HandleVector<int>.Handle handle,
    HashedString parameter,
    float value)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.secondParameterValue = value;
    data.secondParameter = parameter;
    if (data.IsPlaying)
    {
      int num = (int) data.ev.setParameterValueByIndex(this.GetSoundDescription(data.path).GetParameterIdx(parameter), value);
    }
    this.sounds.SetData(handle, data);
  }

  public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
  {
    LoopingSoundManager.Sound data = this.sounds.GetData(handle);
    data.velocity = velocity;
    this.sounds.SetData(handle, data);
  }

  public void RenderEveryTick(float dt)
  {
    ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList1 = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
    ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
    ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
    List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
    bool flag1 = (double) Time.timeScale == 0.0;
    SoundCuller soundCuller = CameraController.Instance.soundCuller;
    for (int index = 0; index < dataList.Count; ++index)
    {
      LoopingSoundManager.Sound sound = dataList[index];
      if ((UnityEngine.Object) sound.transform != (UnityEngine.Object) null)
      {
        sound.pos = (Vector2) sound.transform.GetPosition();
        if ((UnityEngine.Object) sound.animController != (UnityEngine.Object) null)
        {
          Vector3 offset = sound.animController.Offset;
          sound.pos.x += offset.x;
          sound.pos.y += offset.y;
        }
      }
      bool flag2 = !sound.IsCullingEnabled || sound.ShouldCameraScalePosition && soundCuller.IsAudible(sound.pos, sound.falloffDistanceSq) || soundCuller.IsAudibleNoCameraScaling(sound.pos, sound.falloffDistanceSq);
      bool isPlaying = sound.IsPlaying;
      if (flag2)
      {
        pooledList1.Add(sound);
        if (!isPlaying)
        {
          SoundDescription soundDescription = this.GetSoundDescription(sound.path);
          sound.ev = KFMOD.CreateInstance(soundDescription.path);
          dataList[index] = sound;
          pooledList2.Add(index);
        }
      }
      else if (isPlaying)
        pooledList3.Add(index);
    }
    foreach (int index in (List<int>) pooledList2)
    {
      LoopingSoundManager.Sound sound1 = dataList[index];
      SoundDescription soundDescription = this.GetSoundDescription(sound1.path);
      int num1 = (int) sound1.ev.setPaused(flag1 && sound1.ShouldPauseOnGamePaused);
      Vector2 vector2 = sound1.pos;
      if (sound1.ShouldCameraScalePosition)
        vector2 = (Vector2) SoundEvent.GetCameraScaledPosition((Vector3) vector2);
      int num2 = (int) sound1.ev.set3DAttributes((Vector3) vector2.To3DAttributes());
      int num3 = (int) sound1.ev.start();
      sound1.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
      if (sound1.firstParameter != HashedString.Invalid)
      {
        int num4 = (int) sound1.ev.setParameterValueByIndex(soundDescription.GetParameterIdx(sound1.firstParameter), sound1.firstParameterValue);
      }
      if (sound1.secondParameter != HashedString.Invalid)
      {
        int num5 = (int) sound1.ev.setParameterValueByIndex(soundDescription.GetParameterIdx(sound1.secondParameter), sound1.secondParameterValue);
      }
      LoopingSoundParameterUpdater.Sound sound2 = new LoopingSoundParameterUpdater.Sound()
      {
        ev = sound1.ev,
        path = sound1.path,
        description = soundDescription,
        transform = sound1.transform
      };
      foreach (SoundDescription.Parameter parameter in soundDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (this.parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
          parameterUpdater.Add(sound2);
      }
      dataList[index] = sound1;
    }
    pooledList2.Recycle();
    foreach (int index in (List<int>) pooledList3)
    {
      LoopingSoundManager.Sound sound1 = dataList[index];
      SoundDescription soundDescription = this.GetSoundDescription(sound1.path);
      LoopingSoundParameterUpdater.Sound sound2 = new LoopingSoundParameterUpdater.Sound()
      {
        ev = sound1.ev,
        path = sound1.path,
        description = soundDescription,
        transform = sound1.transform
      };
      foreach (SoundDescription.Parameter parameter in soundDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (this.parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
          parameterUpdater.Remove(sound2);
      }
      if (sound1.ShouldCameraScalePosition)
      {
        int num1 = (int) sound1.ev.stop(STOP_MODE.IMMEDIATE);
      }
      else
      {
        int num2 = (int) sound1.ev.stop(STOP_MODE.ALLOWFADEOUT);
      }
      sound1.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
      int num3 = (int) sound1.ev.release();
      dataList[index] = sound1;
    }
    pooledList3.Recycle();
    float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
    foreach (LoopingSoundManager.Sound sound in (List<LoopingSoundManager.Sound>) pooledList1)
    {
      ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition((Vector3) sound.pos).To3DAttributes();
      attributes.velocity = (Vector3) (sound.velocity * velocityScale).ToFMODVector();
      int num = (int) sound.ev.set3DAttributes(attributes);
    }
    foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> parameterUpdater in this.parameterUpdaters)
      parameterUpdater.Value.Update(dt);
    pooledList1.Recycle();
  }

  public static LoopingSoundManager Get()
  {
    return LoopingSoundManager.instance;
  }

  public void StopAllSounds()
  {
    foreach (LoopingSoundManager.Sound data in this.sounds.GetDataList())
    {
      if (data.IsPlaying)
      {
        int num1 = (int) data.ev.stop(STOP_MODE.IMMEDIATE);
        int num2 = (int) data.ev.release();
      }
    }
  }

  private SoundDescription GetSoundDescription(HashedString path)
  {
    return KFMOD.GetSoundEventDescription(path);
  }

  public HandleVector<int>.Handle Add(
    string path,
    Vector2 pos,
    Transform transform = null,
    bool pause_on_game_pause = true,
    bool enable_culling = true,
    bool enable_camera_scaled_position = true)
  {
    SoundDescription eventDescription = KFMOD.GetSoundEventDescription((HashedString) path);
    LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags) 0;
    if (pause_on_game_pause)
      flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
    if (enable_culling)
      flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
    if (enable_camera_scaled_position)
      flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
    KBatchedAnimController kbatchedAnimController = (KBatchedAnimController) null;
    if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
      kbatchedAnimController = transform.GetComponent<KBatchedAnimController>();
    return this.sounds.Allocate(new LoopingSoundManager.Sound()
    {
      transform = transform,
      animController = kbatchedAnimController,
      falloffDistanceSq = eventDescription.falloffDistanceSq,
      path = (HashedString) path,
      pos = pos,
      flags = flags,
      firstParameter = HashedString.Invalid,
      secondParameter = HashedString.Invalid
    });
  }

  public static HandleVector<int>.Handle StartSound(
    string path,
    Vector3 pos,
    bool pause_on_game_pause = true,
    bool enable_culling = true)
  {
    if (!string.IsNullOrEmpty(path))
      return LoopingSoundManager.Get().Add(path, (Vector2) pos, (Transform) null, pause_on_game_pause, enable_culling, true);
    Debug.LogWarning((object) "Missing sound");
    return HandleVector<int>.InvalidHandle;
  }

  public static void StopSound(HandleVector<int>.Handle handle)
  {
    if ((UnityEngine.Object) LoopingSoundManager.Get() == (UnityEngine.Object) null)
      return;
    LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
    if (data.IsPlaying)
    {
      int num1 = (int) data.ev.stop(STOP_MODE.ALLOWFADEOUT);
      int num2 = (int) data.ev.release();
      SoundDescription eventDescription = KFMOD.GetSoundEventDescription(data.path);
      foreach (SoundDescription.Parameter parameter in eventDescription.parameters)
      {
        LoopingSoundParameterUpdater parameterUpdater = (LoopingSoundParameterUpdater) null;
        if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out parameterUpdater))
        {
          LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound()
          {
            ev = data.ev,
            path = data.path,
            description = eventDescription,
            transform = data.transform
          };
          parameterUpdater.Remove(sound);
        }
      }
    }
    LoopingSoundManager.Get().sounds.Free(handle);
  }

  private void OnPauseChanged(object data)
  {
    bool flag = (bool) data;
    foreach (LoopingSoundManager.Sound data1 in this.sounds.GetDataList())
    {
      if (data1.IsPlaying)
      {
        int num = (int) data1.ev.setPaused(flag && data1.ShouldPauseOnGamePaused);
      }
    }
  }

  public class Tuning : TuningData<LoopingSoundManager.Tuning>
  {
    public float velocityScale;
  }

  public struct Sound
  {
    public EventInstance ev;
    public Transform transform;
    public KBatchedAnimController animController;
    public float falloffDistanceSq;
    public HashedString path;
    public Vector2 pos;
    public Vector2 velocity;
    public HashedString firstParameter;
    public HashedString secondParameter;
    public float firstParameterValue;
    public float secondParameterValue;
    public LoopingSoundManager.Sound.Flags flags;

    public bool IsPlaying
    {
      get
      {
        return (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) != (LoopingSoundManager.Sound.Flags) 0;
      }
    }

    public bool ShouldPauseOnGamePaused
    {
      get
      {
        return (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) != (LoopingSoundManager.Sound.Flags) 0;
      }
    }

    public bool IsCullingEnabled
    {
      get
      {
        return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) != (LoopingSoundManager.Sound.Flags) 0;
      }
    }

    public bool ShouldCameraScalePosition
    {
      get
      {
        return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) != (LoopingSoundManager.Sound.Flags) 0;
      }
    }

    [System.Flags]
    public enum Flags
    {
      PLAYING = 1,
      PAUSE_ON_GAME_PAUSED = 2,
      ENABLE_CULLING = 4,
      ENABLE_CAMERA_SCALED_POSITION = 8,
    }
  }
}
