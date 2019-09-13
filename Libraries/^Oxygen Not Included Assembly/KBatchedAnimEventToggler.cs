// Decompiled with JetBrains decompiler
// Type: KBatchedAnimEventToggler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KBatchedAnimEventToggler : KMonoBehaviour
{
  [SerializeField]
  public GameObject eventSource;
  [SerializeField]
  public string enableEvent;
  [SerializeField]
  public string disableEvent;
  [SerializeField]
  public List<KBatchedAnimEventToggler.Entry> entries;
  private AnimEventHandler animEventHandler;

  protected override void OnPrefabInit()
  {
    Vector3 position = this.eventSource.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    int layer = LayerMask.NameToLayer("Default");
    foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
    {
      entry.controller.transform.SetPosition(position);
      entry.controller.SetLayer(layer);
      entry.controller.gameObject.SetActive(false);
    }
    int hash1 = Hash.SDBMLower(this.enableEvent);
    int hash2 = Hash.SDBMLower(this.disableEvent);
    this.Subscribe(this.eventSource, hash1, new System.Action<object>(this.Enable));
    this.Subscribe(this.eventSource, hash2, new System.Action<object>(this.Disable));
  }

  protected override void OnSpawn()
  {
    this.animEventHandler = this.GetComponentInParent<AnimEventHandler>();
  }

  private void Enable(object data)
  {
    this.StopAll();
    HashedString context = this.animEventHandler.GetContext();
    if (!context.IsValid)
      return;
    foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
    {
      if (entry.context == context)
      {
        entry.controller.gameObject.SetActive(true);
        entry.controller.Play((HashedString) entry.anim, KAnim.PlayMode.Loop, 1f, 0.0f);
      }
    }
  }

  private void Disable(object data)
  {
    this.StopAll();
  }

  private void StopAll()
  {
    foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
    {
      entry.controller.StopAndClear();
      entry.controller.gameObject.SetActive(false);
    }
  }

  [Serializable]
  public struct Entry
  {
    public string anim;
    public HashedString context;
    public KBatchedAnimController controller;
  }
}
