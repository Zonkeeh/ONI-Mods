// Decompiled with JetBrains decompiler
// Type: DestroyAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DestroyAfter : KMonoBehaviour
{
  private ParticleSystem[] particleSystems;

  protected override void OnSpawn()
  {
    this.particleSystems = this.gameObject.GetComponentsInChildren<ParticleSystem>(true);
  }

  private bool IsAlive()
  {
    for (int index = 0; index < this.particleSystems.Length; ++index)
    {
      if (this.particleSystems[index].IsAlive(false))
        return true;
    }
    return false;
  }

  private void Update()
  {
    if (this.particleSystems == null || this.IsAlive())
      return;
    this.DeleteObject();
  }
}
