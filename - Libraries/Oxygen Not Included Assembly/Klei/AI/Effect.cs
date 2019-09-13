// Decompiled with JetBrains decompiler
// Type: Klei.AI.Effect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
  [DebuggerDisplay("{Id}")]
  public class Effect : Modifier
  {
    public float duration;
    public bool showInUI;
    public bool triggerFloatingText;
    public bool isBad;
    public string emoteAnim;
    public float emoteCooldown;
    public List<Reactable.ReactablePrecondition> emotePreconditions;
    public string stompGroup;

    public Effect(
      string id,
      string name,
      string description,
      float duration,
      bool show_in_ui,
      bool trigger_floating_text,
      bool is_bad,
      string emote_anim = null,
      float emote_cooldown = 0.0f,
      string stompGroup = null)
      : base(id, name, description)
    {
      this.duration = duration;
      this.showInUI = show_in_ui;
      this.triggerFloatingText = trigger_floating_text;
      this.isBad = is_bad;
      this.emoteAnim = emote_anim;
      this.emoteCooldown = emote_cooldown;
      this.stompGroup = stompGroup;
    }

    public override void AddTo(Attributes attributes)
    {
      base.AddTo(attributes);
    }

    public override void RemoveFrom(Attributes attributes)
    {
      base.RemoveFrom(attributes);
    }

    public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
    {
      if (this.emotePreconditions == null)
        this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
      this.emotePreconditions.Add(precon);
    }

    public static string CreateTooltip(Effect effect, bool showDuration, string linePrefix = "\n")
    {
      string str = string.Empty;
      foreach (AttributeModifier selfModifier in effect.SelfModifiers)
      {
        Attribute attribute = Db.Get().Attributes.TryGet(selfModifier.AttributeId) ?? Db.Get().CritterAttributes.TryGet(selfModifier.AttributeId);
        if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
          str = str + linePrefix + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) attribute.Name, (object) selfModifier.GetFormattedString((GameObject) null));
      }
      StringEntry result;
      if (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".ADDITIONAL_EFFECTS", out result))
        str = str + linePrefix + (string) result;
      if (showDuration && (double) effect.duration > 0.0)
        str = str + linePrefix + string.Format((string) DUPLICANTS.MODIFIERS.TIME_TOTAL, (object) GameUtil.GetFormattedCycles(effect.duration, "F1"));
      return str;
    }

    public static void AddModifierDescriptions(
      GameObject parent,
      List<Descriptor> descs,
      string effect_id,
      bool increase_indent = false)
    {
      foreach (AttributeModifier selfModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
      {
        Descriptor descriptor = new Descriptor((string) Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + selfModifier.AttributeId.ToUpper() + ".NAME") + ": " + selfModifier.GetFormattedString(parent), string.Empty, Descriptor.DescriptorType.Effect, false);
        if (increase_indent)
          descriptor.IncreaseIndent();
        descs.Add(descriptor);
      }
    }
  }
}
