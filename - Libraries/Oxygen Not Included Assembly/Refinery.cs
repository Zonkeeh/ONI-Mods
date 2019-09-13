// Decompiled with JetBrains decompiler
// Type: Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class Refinery : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  [Serializable]
  public struct OrderSaveData
  {
    public string id;
    public bool infinite;

    public OrderSaveData(string id, bool infinite)
    {
      this.id = id;
      this.infinite = infinite;
    }
  }
}
