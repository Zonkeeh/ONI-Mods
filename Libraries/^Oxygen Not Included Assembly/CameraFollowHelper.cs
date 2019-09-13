// Decompiled with JetBrains decompiler
// Type: CameraFollowHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraFollowHelper : KMonoBehaviour
{
  private void LateUpdate()
  {
    if (!((Object) CameraController.Instance != (Object) null))
      return;
    CameraController.Instance.UpdateFollowTarget();
  }
}
