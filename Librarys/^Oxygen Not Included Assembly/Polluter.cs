// Decompiled with JetBrains decompiler
// Type: Polluter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Polluter : IPolluter
{
  private int _radius;
  private int decibels;
  private Vector2 position;
  private string sourceName;
  private GameObject gameObject;
  private NoiseSplat splat;

  public Polluter(int radius)
  {
    this.radius = radius;
  }

  public int radius
  {
    get
    {
      return this._radius;
    }
    private set
    {
      this._radius = value;
      if (this._radius != 0)
        return;
      Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", (object) this.GetName());
    }
  }

  public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
  {
    this.position = pos;
    this.sourceName = name;
    this.decibels = dB;
    this.gameObject = go;
  }

  public string GetName()
  {
    return this.sourceName;
  }

  public int GetRadius()
  {
    return this.radius;
  }

  public int GetNoise()
  {
    return this.decibels;
  }

  public GameObject GetGameObject()
  {
    return this.gameObject;
  }

  public void SetSplat(NoiseSplat new_splat)
  {
    if (new_splat == null && this.splat != null)
      this.Clear();
    this.splat = new_splat;
    if (this.splat == null)
      return;
    AudioEventManager.Get().AddSplat(this.splat);
  }

  public void Clear()
  {
    if (this.splat == null)
      return;
    AudioEventManager.Get().ClearNoiseSplat(this.splat);
    this.splat.Clear();
    this.splat = (NoiseSplat) null;
  }

  public Vector2 GetPosition()
  {
    return this.position;
  }
}
