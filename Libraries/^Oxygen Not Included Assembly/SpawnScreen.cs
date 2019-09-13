// Decompiled with JetBrains decompiler
// Type: SpawnScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnScreen : KMonoBehaviour
{
  public GameObject Screen;

  protected override void OnPrefabInit()
  {
    Util.KInstantiateUI(this.Screen, this.gameObject, false);
  }
}
