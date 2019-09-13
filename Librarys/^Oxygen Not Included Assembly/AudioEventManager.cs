// Decompiled with JetBrains decompiler
// Type: AudioEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventManager : KMonoBehaviour
{
  private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();
  private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();
  private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();
  private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();
  private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();
  private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();
  public const float NO_NOISE_EFFECTORS = 0.0f;
  public const float MIN_LOUDNESS_THRESHOLD = 1f;
  private static AudioEventManager instance;

  public static AudioEventManager Get()
  {
    if ((UnityEngine.Object) AudioEventManager.instance == (UnityEngine.Object) null)
    {
      if (App.IsExiting)
        return (AudioEventManager) null;
      GameObject gameObject = GameObject.Find("/AudioEventManager");
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        gameObject = new GameObject();
        gameObject.name = nameof (AudioEventManager);
      }
      AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
      if ((UnityEngine.Object) AudioEventManager.instance == (UnityEngine.Object) null)
        AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
    }
    return AudioEventManager.instance;
  }

  protected override void OnSpawn()
  {
    this.OnPrefabInit();
    this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
  }

  public static float LoudnessToDB(float loudness)
  {
    if ((double) loudness > 0.0)
      return 10f * Mathf.Log10(loudness);
    return 0.0f;
  }

  public static float DBToLoudness(float src_db)
  {
    return Mathf.Pow(10f, src_db / 10f);
  }

  public float GetDecibelsAtCell(int cell)
  {
    return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
  }

  public static string GetLoudestNoisePolluterAtCell(int cell)
  {
    float num = float.NegativeInfinity;
    string str = (string) null;
    AudioEventManager audioEventManager = AudioEventManager.Get();
    Vector2I xy = Grid.CellToXY(cell);
    Vector2 pos = new Vector2((float) xy.x, (float) xy.y);
    IEnumerator enumerator = audioEventManager.spatialSplats.GetAllIntersecting(pos).GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        NoiseSplat current = (NoiseSplat) enumerator.Current;
        if ((double) current.GetLoudness(cell) > (double) num)
          str = current.GetProvider().GetName();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return str;
  }

  public void ClearNoiseSplat(NoiseSplat splat)
  {
    if (!this.splats.Contains(splat))
      return;
    this.splats.Remove(splat);
    this.spatialSplats.Remove(splat);
  }

  public void AddSplat(NoiseSplat splat)
  {
    this.splats.Add(splat);
    this.spatialSplats.Add(splat);
  }

  public NoiseSplat CreateNoiseSplat(
    Vector2 pos,
    int dB,
    int radius,
    string name,
    GameObject go)
  {
    Polluter polluter = this.GetPolluter(radius);
    polluter.SetAttributes(pos, dB, go, name);
    NoiseSplat new_splat = new NoiseSplat((IPolluter) polluter, 0.0f);
    polluter.SetSplat(new_splat);
    return new_splat;
  }

  public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
  {
    this.polluters.Clear();
    Vector2I xy = Grid.CellToXY(cell);
    IEnumerator enumerator = this.spatialSplats.GetAllIntersecting(new Vector2((float) xy.x, (float) xy.y)).GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        NoiseSplat current = (NoiseSplat) enumerator.Current;
        float loudness = current.GetLoudness(cell);
        if ((double) loudness > 0.0)
          this.polluters.Add(new AudioEventManager.PolluterDisplay()
          {
            name = current.GetName(),
            value = AudioEventManager.LoudnessToDB(loudness),
            provider = current.GetProvider()
          });
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return this.polluters;
  }

  private void RemoveExpiredSplats()
  {
    if (this.removeTime.Count > 1)
      this.removeTime.Sort((Comparison<Pair<float, NoiseSplat>>) ((a, b) => a.first.CompareTo(b.first)));
    int num = -1;
    for (int index = 0; index < this.removeTime.Count && (double) this.removeTime[index].first <= (double) Time.time; ++index)
    {
      NoiseSplat second = this.removeTime[index].second;
      if (second != null)
        this.FreePolluter(second.GetProvider() as Polluter);
      num = index;
    }
    for (int index = num; index >= 0; --index)
      this.removeTime.RemoveAt(index);
  }

  private void Update()
  {
    this.RemoveExpiredSplats();
  }

  private Polluter GetPolluter(int radius)
  {
    if (!this.freePool.ContainsKey(radius))
      this.freePool.Add(radius, new List<Polluter>());
    Polluter polluter;
    if (this.freePool[radius].Count > 0)
    {
      polluter = this.freePool[radius][0];
      this.freePool[radius].RemoveAt(0);
    }
    else
      polluter = new Polluter(radius);
    if (!this.inusePool.ContainsKey(radius))
      this.inusePool.Add(radius, new List<Polluter>());
    this.inusePool[radius].Add(polluter);
    return polluter;
  }

  private void FreePolluter(Polluter pol)
  {
    if (pol == null)
      return;
    pol.Clear();
    Debug.Assert(this.inusePool[pol.radius].Contains(pol));
    this.inusePool[pol.radius].Remove(pol);
    this.freePool[pol.radius].Add(pol);
  }

  public void PlayTimedOnceOff(
    Vector2 pos,
    int dB,
    int radius,
    string name,
    GameObject go,
    float time = 1f)
  {
    if (dB <= 0 || radius <= 0 || (double) time <= 0.0)
      return;
    Polluter polluter = this.GetPolluter(radius);
    polluter.SetAttributes(pos, dB, go, name);
    this.AddTimedInstance(polluter, time);
  }

  private void AddTimedInstance(Polluter p, float time)
  {
    NoiseSplat noiseSplat = new NoiseSplat((IPolluter) p, time + Time.time);
    p.SetSplat(noiseSplat);
    this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
  }

  private static void SoundLog(long itemId, string message)
  {
    Debug.Log((object) (" [" + (object) itemId + "] \t" + message));
  }

  public enum NoiseEffect
  {
    Peaceful = 0,
    Quiet = 36, // 0x00000024
    TossAndTurn = 45, // 0x0000002D
    WakeUp = 60, // 0x0000003C
    Passive = 80, // 0x00000050
    Active = 106, // 0x0000006A
  }

  public struct PolluterDisplay
  {
    public string name;
    public float value;
    public IPolluter provider;
  }
}
