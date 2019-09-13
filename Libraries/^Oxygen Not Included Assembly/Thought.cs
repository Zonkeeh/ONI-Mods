// Decompiled with JetBrains decompiler
// Type: Thought
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Thought : Resource
{
  public int priority;
  public Sprite sprite;
  public Sprite modeSprite;
  public string sound;
  public Sprite bubbleSprite;
  public string speechPrefix;
  public LocString hoverText;
  public bool showImmediately;
  public float showTime;

  public Thought(
    string id,
    ResourceSet parent,
    Sprite icon,
    string mode_icon,
    string sound_name,
    string bubble,
    string speech_prefix,
    LocString hover_text,
    bool show_immediately = false,
    float show_time = 4f)
    : base(id, parent, (string) null)
  {
    this.sprite = icon;
    if (mode_icon != null)
      this.modeSprite = Assets.GetSprite((HashedString) mode_icon);
    this.bubbleSprite = Assets.GetSprite((HashedString) bubble);
    this.sound = sound_name;
    this.speechPrefix = speech_prefix;
    this.hoverText = hover_text;
    this.showImmediately = show_immediately;
    this.showTime = show_time;
  }

  public Thought(
    string id,
    ResourceSet parent,
    string icon,
    string mode_icon,
    string sound_name,
    string bubble,
    string speech_prefix,
    LocString hover_text,
    bool show_immediately = false,
    float show_time = 4f)
    : base(id, parent, (string) null)
  {
    this.sprite = Assets.GetSprite((HashedString) icon);
    if (mode_icon != null)
      this.modeSprite = Assets.GetSprite((HashedString) mode_icon);
    this.bubbleSprite = Assets.GetSprite((HashedString) bubble);
    this.sound = sound_name;
    this.speechPrefix = speech_prefix;
    this.hoverText = hover_text;
    this.showImmediately = show_immediately;
    this.showTime = show_time;
  }
}
