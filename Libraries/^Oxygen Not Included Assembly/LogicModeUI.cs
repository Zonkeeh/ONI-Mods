// Decompiled with JetBrains decompiler
// Type: LogicModeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LogicModeUI : ScriptableObject
{
  [Header("Colouring")]
  public Color32 colourOn = new Color32((byte) 0, byte.MaxValue, (byte) 0, (byte) 0);
  public Color32 colourOff = new Color32(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0);
  public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
  [Header("Base Assets")]
  public Sprite inputSprite;
  public Sprite outputSprite;
  public Sprite resetSprite;
  public GameObject prefab;
}
