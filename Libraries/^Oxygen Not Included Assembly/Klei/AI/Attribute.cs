// Decompiled with JetBrains decompiler
// Type: Klei.AI.Attribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  public class Attribute : Resource
  {
    private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);
    public List<AttributeConverter> converters = new List<AttributeConverter>();
    public string Description;
    public float BaseValue;
    public Attribute.Display ShowInUI;
    public bool IsTrainable;
    public bool IsProfession;
    public string ProfessionName;
    public string uiSprite;
    public string thoughtSprite;
    public IAttributeFormatter formatter;

    public Attribute(
      string id,
      bool is_trainable,
      Attribute.Display show_in_ui,
      bool is_profession,
      float base_value = 0.0f,
      string uiSprite = null,
      string thoughtSprite = null)
      : base(id, (ResourceSet) null, (string) null)
    {
      string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
      this.Name = (string) Strings.Get(new StringKey(str + ".NAME"));
      this.ProfessionName = (string) Strings.Get(new StringKey(str + ".NAME"));
      this.Description = (string) Strings.Get(new StringKey(str + ".DESC"));
      this.IsTrainable = is_trainable;
      this.IsProfession = is_profession;
      this.ShowInUI = show_in_ui;
      this.BaseValue = base_value;
      this.formatter = (IAttributeFormatter) Attribute.defaultFormatter;
      this.uiSprite = uiSprite;
      this.thoughtSprite = thoughtSprite;
    }

    public Attribute(
      string id,
      string name,
      string profession_name,
      string attribute_description,
      float base_value,
      Attribute.Display show_in_ui,
      bool is_trainable,
      string uiSprite = null,
      string thoughtSprite = null)
      : base(id, name)
    {
      this.Description = attribute_description;
      this.ProfessionName = profession_name;
      this.BaseValue = base_value;
      this.ShowInUI = show_in_ui;
      this.IsTrainable = is_trainable;
      this.uiSprite = uiSprite;
      this.thoughtSprite = thoughtSprite;
      if (!(this.ProfessionName == string.Empty))
        return;
      this.ProfessionName = (string) null;
    }

    public void SetFormatter(IAttributeFormatter formatter)
    {
      this.formatter = formatter;
    }

    public AttributeInstance Lookup(Component cmp)
    {
      return this.Lookup(cmp.gameObject);
    }

    public AttributeInstance Lookup(GameObject go)
    {
      return go.GetAttributes()?.Get(this);
    }

    public string GetDescription(AttributeInstance instance)
    {
      return instance.GetDescription();
    }

    public string GetTooltip(AttributeInstance instance)
    {
      return this.formatter.GetTooltip(this, instance);
    }

    public enum Display
    {
      Normal,
      Skill,
      Expectation,
      General,
      Details,
      Never,
    }
  }
}
