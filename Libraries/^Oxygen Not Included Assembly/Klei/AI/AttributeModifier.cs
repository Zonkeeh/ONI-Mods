// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
  [DebuggerDisplay("{AttributeId}")]
  public class AttributeModifier
  {
    public string Description;
    public Func<string> DescriptionCB;

    public AttributeModifier(
      string attribute_id,
      float value,
      string description = null,
      bool is_multiplier = false,
      bool uiOnly = false,
      bool is_readonly = true)
    {
      this.AttributeId = attribute_id;
      this.Value = value;
      this.Description = description != null ? description : attribute_id;
      this.DescriptionCB = (Func<string>) null;
      this.IsMultiplier = is_multiplier;
      this.UIOnly = uiOnly;
      this.IsReadonly = is_readonly;
    }

    public AttributeModifier(
      string attribute_id,
      float value,
      Func<string> description_cb,
      bool is_multiplier = false,
      bool uiOnly = false)
    {
      this.AttributeId = attribute_id;
      this.Value = value;
      this.DescriptionCB = description_cb;
      this.Description = (string) null;
      this.IsMultiplier = is_multiplier;
      this.UIOnly = uiOnly;
      if (description_cb != null)
        return;
      Debug.LogWarning((object) ("AttributeModifier being constructed without a description callback: " + attribute_id));
    }

    public string AttributeId { get; private set; }

    public float Value { get; private set; }

    public bool IsMultiplier { get; private set; }

    public bool UIOnly { get; private set; }

    public bool IsReadonly { get; private set; }

    public void SetValue(float value)
    {
      this.Value = value;
    }

    public string GetDescription()
    {
      if (this.DescriptionCB != null)
        return this.DescriptionCB();
      return this.Description;
    }

    public string GetFormattedString(GameObject parent_instance)
    {
      IAttributeFormatter attributeFormatter = (IAttributeFormatter) null;
      Attribute attribute1 = Db.Get().Attributes.TryGet(this.AttributeId);
      if (!this.IsMultiplier)
      {
        if (attribute1 != null)
        {
          attributeFormatter = attribute1.formatter;
        }
        else
        {
          Attribute attribute2 = Db.Get().BuildingAttributes.TryGet(this.AttributeId);
          if (attribute2 != null)
            attributeFormatter = attribute2.formatter;
        }
      }
      string empty = string.Empty;
      string text = attributeFormatter == null ? (!this.IsMultiplier ? empty + GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, (string) null) : empty + GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None)) : attributeFormatter.GetFormattedModifier(this, parent_instance);
      if (text != null && text.Length > 0 && text[0] != '-')
        text = GameUtil.AddPositiveSign(text, (double) this.Value > 0.0);
      return text;
    }

    public AttributeModifier Clone()
    {
      return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
    }
  }
}
