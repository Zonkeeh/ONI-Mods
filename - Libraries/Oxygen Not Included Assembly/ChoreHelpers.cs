// Decompiled with JetBrains decompiler
// Type: ChoreHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class ChoreHelpers
{
  public static GameObject CreateLocator(string name, Vector3 pos)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) ApproachableLocator.ID), (GameObject) null, (string) null);
    gameObject.name = name;
    gameObject.transform.SetPosition(pos);
    gameObject.gameObject.SetActive(true);
    return gameObject;
  }

  public static GameObject CreateSleepLocator(Vector3 pos)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) SleepLocator.ID), (GameObject) null, (string) null);
    gameObject.name = "SLeepLocator";
    gameObject.transform.SetPosition(pos);
    gameObject.gameObject.SetActive(true);
    return gameObject;
  }

  public static void DestroyLocator(GameObject locator)
  {
    if (!((Object) locator != (Object) null))
      return;
    locator.gameObject.DeleteObject();
  }
}
