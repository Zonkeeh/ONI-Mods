// Decompiled with JetBrains decompiler
// Type: CO2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class CO2 : KMonoBehaviour
{
  [Serialize]
  [NonSerialized]
  public Vector3 velocity = Vector3.zero;
  [Serialize]
  [NonSerialized]
  public float mass;
  [Serialize]
  [NonSerialized]
  public float temperature;
  [Serialize]
  [NonSerialized]
  public float lifetimeRemaining;

  public void StartLoop()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) "exhale_pre", KAnim.PlayMode.Once, 1f, 0.0f);
    component.Play((HashedString) "exhale_loop", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public void TriggerDestroy()
  {
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "exhale_pst", KAnim.PlayMode.Once, 1f, 0.0f);
  }
}
