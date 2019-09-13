// Decompiled with JetBrains decompiler
// Type: MainCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MainCamera : MonoBehaviour
{
  private void Awake()
  {
    if ((Object) Camera.main != (Object) null)
      Object.Destroy((Object) Camera.main.gameObject);
    this.gameObject.tag = nameof (MainCamera);
  }
}
