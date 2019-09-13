// Decompiled with JetBrains decompiler
// Type: IPolluter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public interface IPolluter
{
  int GetRadius();

  int GetNoise();

  GameObject GetGameObject();

  void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

  string GetName();

  Vector2 GetPosition();

  void Clear();

  void SetSplat(NoiseSplat splat);
}
