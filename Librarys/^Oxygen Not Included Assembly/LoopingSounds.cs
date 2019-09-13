// Decompiled with JetBrains decompiler
// Type: LoopingSounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class LoopingSounds : KMonoBehaviour
{
  private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();
  private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();
  [SerializeField]
  public bool updatePosition;

  public bool IsSoundPlaying(string path)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == path)
        return true;
    }
    return false;
  }

  public bool StartSound(
    string asset,
    AnimEventManager.EventPlayerData behaviour,
    EffectorValues noiseValues,
    bool ignore_pause = false,
    bool enable_camera_scaled_position = true)
  {
    if (asset == null || asset == string.Empty)
    {
      Debug.LogWarning((object) "Missing sound");
      return false;
    }
    if (!this.IsSoundPlaying(asset))
    {
      LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
      {
        asset = asset
      };
      Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
      loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, (Vector2) position, this.transform, !ignore_pause, true, enable_camera_scaled_position);
      this.loopingSounds.Add(loopingSoundEvent);
    }
    return true;
  }

  public bool StartSound(string asset)
  {
    if (asset == null || asset == string.Empty)
    {
      Debug.LogWarning((object) "Missing sound");
      return false;
    }
    if (!this.IsSoundPlaying(asset))
    {
      LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
      {
        asset = asset
      };
      loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, (Vector2) this.transform.GetPosition(), this.transform, true, true, true);
      this.loopingSounds.Add(loopingSoundEvent);
    }
    return true;
  }

  public bool StartSound(
    string asset,
    bool pause_on_game_pause = true,
    bool enable_culling = true,
    bool enable_camera_scaled_position = true)
  {
    if (asset == null || asset == string.Empty)
    {
      Debug.LogWarning((object) "Missing sound");
      return false;
    }
    if (!this.IsSoundPlaying(asset))
    {
      LoopingSounds.LoopingSoundEvent loopingSoundEvent = new LoopingSounds.LoopingSoundEvent()
      {
        asset = asset
      };
      loopingSoundEvent.handle = LoopingSoundManager.Get().Add(asset, (Vector2) this.transform.GetPosition(), this.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position);
      this.loopingSounds.Add(loopingSoundEvent);
    }
    return true;
  }

  public void UpdateVelocity(string asset, Vector2 value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateVelocity(loopingSound.handle, value);
        break;
      }
    }
  }

  public void UpdateFirstParameter(string asset, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateFirstParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  public void UpdateSecondParameter(string asset, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == asset)
      {
        LoopingSoundManager.Get().UpdateSecondParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  private void StopSoundAtIndex(int i)
  {
    LoopingSoundManager.StopSound(this.loopingSounds[i].handle);
  }

  public void StopSound(string asset)
  {
    for (int index = 0; index < this.loopingSounds.Count; ++index)
    {
      if (this.loopingSounds[index].asset == asset)
      {
        this.StopSoundAtIndex(index);
        this.loopingSounds.RemoveAt(index);
        break;
      }
    }
  }

  public void StopAllSounds()
  {
    for (int i = 0; i < this.loopingSounds.Count; ++i)
      this.StopSoundAtIndex(i);
    this.loopingSounds.Clear();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.StopAllSounds();
  }

  public void SetParameter(string path, HashedString parameter, float value)
  {
    foreach (LoopingSounds.LoopingSoundEvent loopingSound in this.loopingSounds)
    {
      if (loopingSound.asset == path)
      {
        LoopingSoundManager.Get().UpdateFirstParameter(loopingSound.handle, parameter, value);
        break;
      }
    }
  }

  public void PlayEvent(GameSoundEvents.Event ev)
  {
    if (AudioDebug.Get().debugGameEventSounds)
      Debug.Log((object) ("GameSoundEvent: " + (object) ev.Name));
    List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
    if (events == null)
      return;
    Vector2 position = (Vector2) this.transform.GetPosition();
    for (int index = 0; index < events.Count; ++index)
    {
      SoundEvent soundEvent = events[index] as SoundEvent;
      if (soundEvent == null || soundEvent.sound == null)
        break;
      if (CameraController.Instance.IsAudibleSound((Vector3) position, soundEvent.sound))
      {
        if (AudioDebug.Get().debugGameEventSounds)
          Debug.Log((object) ("GameSound: " + soundEvent.sound));
        float num = 0.0f;
        if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
        {
          if ((double) Time.time - (double) num > (double) soundEvent.minInterval)
            SoundEvent.PlayOneShot(soundEvent.sound, (Vector3) position);
        }
        else
          SoundEvent.PlayOneShot(soundEvent.sound, (Vector3) position);
        this.lastTimePlayed[soundEvent.soundHash] = Time.time;
      }
    }
  }

  private struct LoopingSoundEvent
  {
    public string asset;
    public HandleVector<int>.Handle handle;
  }
}
