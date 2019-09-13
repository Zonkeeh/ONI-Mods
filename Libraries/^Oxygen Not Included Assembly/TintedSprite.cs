// Decompiled with JetBrains decompiler
// Type: TintedSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
  [ReadOnly]
  public string name;
  public Sprite sprite;
  public Color color;

  public void OnAfterDeserialize()
  {
  }

  public void OnBeforeSerialize()
  {
    if (!((UnityEngine.Object) this.sprite != (UnityEngine.Object) null))
      return;
    this.name = this.sprite.name;
  }
}
