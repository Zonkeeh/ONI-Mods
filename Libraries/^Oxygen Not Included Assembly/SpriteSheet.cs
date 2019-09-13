// Decompiled with JetBrains decompiler
// Type: SpriteSheet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct SpriteSheet
{
  public string name;
  public int numFrames;
  public int numXFrames;
  public Vector2 uvFrameSize;
  public int renderLayer;
  public Material material;
  public Texture2D texture;
}
