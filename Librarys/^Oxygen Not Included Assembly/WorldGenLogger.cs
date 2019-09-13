// Decompiled with JetBrains decompiler
// Type: WorldGenLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class WorldGenLogger
{
  public static void LogException(string message, string stack)
  {
    Debug.LogError((object) (message + "\n" + stack));
  }
}
