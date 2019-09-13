// Decompiled with JetBrains decompiler
// Type: ToggleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct ToggleState
{
  public string Name;
  public string on_click_override_sound_path;
  public string on_release_override_sound_path;
  public Sprite sprite;
  public Color color;
  public Color color_on_hover;
  public bool use_color_on_hover;
  public bool use_rect_margins;
  public Vector2 rect_margins;
  public StatePresentationSetting[] additional_display_settings;
}
