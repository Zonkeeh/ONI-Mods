// Decompiled with JetBrains decompiler
// Type: Achievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Achievements : KMonoBehaviour
{
  public void Unlock(string id)
  {
    if (!(bool) ((Object) SteamAchievementService.Instance))
      return;
    SteamAchievementService.Instance.Unlock(id);
  }
}
