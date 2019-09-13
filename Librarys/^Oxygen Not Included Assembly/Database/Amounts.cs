// Decompiled with JetBrains decompiler
// Type: Database.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class Amounts : ResourceSet<Amount>
  {
    public Amount Stamina;
    public Amount Calories;
    public Amount ImmuneLevel;
    public Amount ExternalTemperature;
    public Amount Breath;
    public Amount Stress;
    public Amount Toxicity;
    public Amount Bladder;
    public Amount Decor;
    public Amount Temperature;
    public Amount HitPoints;
    public Amount AirPressure;
    public Amount Maturity;
    public Amount OldAge;
    public Amount Age;
    public Amount Fertilization;
    public Amount Illumination;
    public Amount Irrigation;
    public Amount CreatureCalories;
    public Amount Fertility;
    public Amount Viability;
    public Amount Wildness;
    public Amount Incubation;
    public Amount ScaleGrowth;
    public Amount Rot;

    public void Load()
    {
      this.Stamina = this.CreateAmount("Stamina", 0.0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_stamina", "attribute_stamina");
      this.Stamina.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null));
      this.Calories = this.CreateAmount("Calories", 0.0f, 0.0f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_calories", "attribute_calories");
      this.Calories.SetDisplayer((IAmountDisplayer) new CaloriesDisplayer());
      this.Temperature = this.CreateAmount("Temperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_temperature", (string) null);
      this.Temperature.SetDisplayer((IAmountDisplayer) new DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
      this.ExternalTemperature = this.CreateAmount("ExternalTemperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS.STATS", (string) null, (string) null);
      this.ExternalTemperature.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond, (StandardAttributeFormatter) null));
      this.Breath = this.CreateAmount("Breath", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_breath", (string) null);
      this.Breath.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, (StandardAttributeFormatter) null));
      this.Stress = this.CreateAmount("Stress", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_stress", "attribute_stress");
      this.Stress.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Toxicity = this.CreateAmount("Toxicity", 0.0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", (string) null, (string) null);
      this.Toxicity.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null));
      this.Bladder = this.CreateAmount("Bladder", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_bladder", (string) null);
      this.Bladder.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null));
      this.Decor = this.CreateAmount("Decor", -1000f, 1000f, false, Units.Flat, 0.01666667f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_decor", (string) null);
      this.Decor.SetDisplayer((IAmountDisplayer) new DecorDisplayer());
      this.Maturity = this.CreateAmount("Maturity", 0.0f, 0.0f, true, Units.Flat, 0.0009166667f, true, "STRINGS.CREATURES.STATS", "ui_icon_maturity", (string) null);
      this.Maturity.SetDisplayer((IAmountDisplayer) new MaturityDisplayer());
      this.OldAge = this.CreateAmount("OldAge", 0.0f, 0.0f, false, Units.Flat, 0.0f, false, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Fertilization = this.CreateAmount("Fertilization", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Fertilization.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, (StandardAttributeFormatter) null));
      this.Fertility = this.CreateAmount("Fertility", 0.0f, 100f, true, Units.Flat, 0.008375f, true, "STRINGS.CREATURES.STATS", "ui_icon_fertility", (string) null);
      this.Fertility.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Wildness = this.CreateAmount("Wildness", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_wildness", (string) null);
      this.Wildness.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Incubation = this.CreateAmount("Incubation", 0.0f, 100f, true, Units.Flat, 0.01675f, true, "STRINGS.CREATURES.STATS", "ui_icon_incubation", (string) null);
      this.Incubation.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Viability = this.CreateAmount("Viability", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Viability.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Age = this.CreateAmount("Age", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_age", (string) null);
      this.Age.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null));
      this.Irrigation = this.CreateAmount("Irrigation", 0.0f, 1f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Irrigation.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, (StandardAttributeFormatter) null));
      this.HitPoints = this.CreateAmount("HitPoints", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_hitpoints", "attribute_hitpoints");
      this.HitPoints.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null));
      this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_immunelevel", "attribute_immunelevel");
      this.ImmuneLevel.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Rot = this.CreateAmount("Rot", 0.0f, 0.0f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Rot.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.AirPressure = this.CreateAmount("AirPressure", 0.0f, 1E+09f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.AirPressure.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond, (StandardAttributeFormatter) null));
      this.Illumination = this.CreateAmount("Illumination", 0.0f, 1f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS", (string) null, (string) null);
      this.Illumination.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None, (StandardAttributeFormatter) null));
      this.ScaleGrowth = this.CreateAmount("ScaleGrowth", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_scale_growth", (string) null);
      this.ScaleGrowth.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    }

    public Amount CreateAmount(
      string id,
      float min,
      float max,
      bool show_max,
      Units units,
      float delta_threshold,
      bool show_in_ui,
      string string_root,
      string uiSprite = null,
      string thoughtSprite = null)
    {
      string name1 = (string) Strings.Get(string.Format("{1}.{0}.NAME", (object) id.ToUpper(), (object) string_root.ToUpper()));
      string description = (string) Strings.Get(string.Format("{1}.{0}.TOOLTIP", (object) id.ToUpper(), (object) string_root.ToUpper()));
      Attribute attribute1 = new Attribute(id + "Min", "Minimum" + name1, string.Empty, string.Empty, min, Attribute.Display.Normal, false, (string) null, (string) null);
      Attribute attribute2 = new Attribute(id + "Max", "Maximum" + name1, string.Empty, string.Empty, max, Attribute.Display.Normal, false, (string) null, (string) null);
      string id1 = id + "Delta";
      string name2 = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.ATTRIBUTES.{0}.NAME", (object) id1.ToUpper()));
      string attribute_description = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.ATTRIBUTES.{0}.DESC", (object) id1.ToUpper()));
      Attribute attribute3 = new Attribute(id1, name2, string.Empty, attribute_description, 0.0f, Attribute.Display.Normal, false, (string) null, (string) null);
      Amount resource = new Amount(id, name1, description, attribute1, attribute2, attribute3, show_max, units, delta_threshold, show_in_ui, uiSprite, thoughtSprite);
      Db.Get().Attributes.Add(attribute1);
      Db.Get().Attributes.Add(attribute2);
      Db.Get().Attributes.Add(attribute3);
      this.Add(resource);
      return resource;
    }
  }
}
