// Decompiled with JetBrains decompiler
// Type: ObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
  private List<GameObject> unused;
  private Func<GameObject> instantiator;

  public ObjectPool(Func<GameObject> instantiator, int initial_count = 0)
  {
    this.instantiator = instantiator;
    this.unused = new List<GameObject>();
    for (int index = 0; index < initial_count; ++index)
      this.unused.Add(instantiator());
  }

  public GameObject GetInstance()
  {
    GameObject gameObject;
    if (this.unused.Count > 0)
    {
      gameObject = this.unused[this.unused.Count - 1];
      this.unused.RemoveAt(this.unused.Count - 1);
    }
    else
      gameObject = this.instantiator();
    return gameObject;
  }

  public void ReleaseInstance(GameObject go)
  {
    this.unused.Add(go);
  }

  public void Destroy()
  {
    for (int index = 0; index < this.unused.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.unused[index]);
    this.unused.Clear();
  }
}
