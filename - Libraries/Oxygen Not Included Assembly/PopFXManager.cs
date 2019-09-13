// Decompiled with JetBrains decompiler
// Type: PopFXManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;
using UnityEngine;

public class PopFXManager : KScreen
{
  public List<PopFX> Pool = new List<PopFX>();
  public static PopFXManager Instance;
  public GameObject Prefab_PopFX;
  public Sprite sprite_Plus;
  public Sprite sprite_Negative;
  public Sprite sprite_Resource;
  public Sprite sprite_Building;
  public Sprite sprite_Research;
  private bool ready;

  public static void DestroyInstance()
  {
    PopFXManager.Instance = (PopFXManager) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PopFXManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ready = true;
    if (GenericGameSettings.instance.disablePopFx)
      return;
    for (int index = 0; index < 20; ++index)
      this.Pool.Add(this.CreatePopFX());
  }

  public bool Ready()
  {
    return this.ready;
  }

  public PopFX SpawnFX(
    Sprite icon,
    string text,
    Transform target_transform,
    Vector3 offset,
    float lifetime = 1.5f,
    bool track_target = false,
    bool force_spawn = false)
  {
    if (GenericGameSettings.instance.disablePopFx)
      return (PopFX) null;
    if (Game.IsQuitting())
      return (PopFX) null;
    Vector3 pos = offset;
    if ((Object) target_transform != (Object) null)
      pos += target_transform.GetPosition();
    if (!force_spawn)
    {
      int cell = Grid.PosToCell(pos);
      if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
        return (PopFX) null;
    }
    PopFX popFx;
    if (this.Pool.Count > 0)
    {
      popFx = this.Pool[0];
      this.Pool[0].gameObject.SetActive(true);
      this.Pool[0].Spawn(icon, text, target_transform, offset, lifetime, track_target);
      this.Pool.RemoveAt(0);
    }
    else
    {
      popFx = this.CreatePopFX();
      popFx.gameObject.SetActive(true);
      popFx.Spawn(icon, text, target_transform, offset, lifetime, track_target);
    }
    return popFx;
  }

  public PopFX SpawnFX(
    Sprite icon,
    string text,
    Transform target_transform,
    float lifetime = 1.5f,
    bool track_target = false)
  {
    return this.SpawnFX(icon, text, target_transform, Vector3.zero, lifetime, track_target, false);
  }

  private PopFX CreatePopFX()
  {
    GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, this.gameObject, "Pooled_PopFX");
    gameObject.transform.localScale = Vector3.one;
    return gameObject.GetComponent<PopFX>();
  }

  public void RecycleFX(PopFX fx)
  {
    this.Pool.Add(fx);
  }
}
