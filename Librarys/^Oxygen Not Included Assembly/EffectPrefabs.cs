// Decompiled with JetBrains decompiler
// Type: EffectPrefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EffectPrefabs : MonoBehaviour
{
  public GameObject ThoughtBubble;
  public GameObject ThoughtBubbleConvo;
  public GameObject MeteorBackground;

  public static EffectPrefabs Instance { get; private set; }

  private void Awake()
  {
    EffectPrefabs.Instance = this;
  }
}
