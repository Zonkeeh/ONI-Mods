// Decompiled with JetBrains decompiler
// Type: Database.Attributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class Attributes : ResourceSet<Attribute>
  {
    public Attribute Construction;
    public Attribute Digging;
    public Attribute Machinery;
    public Attribute Athletics;
    public Attribute Learning;
    public Attribute Cooking;
    public Attribute Caring;
    public Attribute Strength;
    public Attribute Art;
    public Attribute Botanist;
    public Attribute Ranching;
    public Attribute LifeSupport;
    public Attribute Toggle;
    public Attribute PowerTinker;
    public Attribute FarmTinker;
    public Attribute SpaceNavigation;
    public Attribute Immunity;
    public Attribute GermResistance;
    public Attribute Insulation;
    public Attribute ThermalConductivityBarrier;
    public Attribute Decor;
    public Attribute FoodQuality;
    public Attribute ScaldingThreshold;
    public Attribute GeneratorOutput;
    public Attribute MachinerySpeed;
    public Attribute DecorExpectation;
    public Attribute FoodExpectation;
    public Attribute RoomTemperaturePreference;
    public Attribute QualityOfLifeExpectation;
    public Attribute AirConsumptionRate;
    public Attribute MaxUnderwaterTravelCost;
    public Attribute ToiletEfficiency;
    public Attribute Sneezyness;
    public Attribute DiseaseCureSpeed;
    public Attribute DoctoredLevel;
    public Attribute CarryAmount;
    public Attribute QualityOfLife;

    public Attributes(ResourceSet parent)
      : base(nameof (Attributes), parent)
    {
      this.Construction = this.Add(new Attribute(nameof (Construction), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Construction.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Digging = this.Add(new Attribute(nameof (Digging), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Digging.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Machinery = this.Add(new Attribute(nameof (Machinery), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Machinery.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Athletics = this.Add(new Attribute(nameof (Athletics), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Athletics.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Learning = this.Add(new Attribute(nameof (Learning), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Learning.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Cooking = this.Add(new Attribute(nameof (Cooking), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Cooking.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Art = this.Add(new Attribute(nameof (Art), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Art.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Strength = this.Add(new Attribute(nameof (Strength), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Strength.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Caring = this.Add(new Attribute(nameof (Caring), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Caring.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Botanist = this.Add(new Attribute(nameof (Botanist), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Botanist.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Ranching = this.Add(new Attribute(nameof (Ranching), true, Attribute.Display.Skill, true, 0.0f, (string) null, (string) null));
      this.Ranching.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.PowerTinker = this.Add(new Attribute(nameof (PowerTinker), true, Attribute.Display.Normal, true, 0.0f, (string) null, (string) null));
      this.PowerTinker.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.FarmTinker = this.Add(new Attribute(nameof (FarmTinker), true, Attribute.Display.Normal, true, 0.0f, (string) null, (string) null));
      this.FarmTinker.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.SpaceNavigation = this.Add(new Attribute(nameof (SpaceNavigation), true, Attribute.Display.Normal, true, 0.0f, (string) null, (string) null));
      this.SpaceNavigation.SetFormatter((IAttributeFormatter) new PercentAttributeFormatter());
      this.Immunity = this.Add(new Attribute(nameof (Immunity), true, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.Immunity.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.ThermalConductivityBarrier = this.Add(new Attribute(nameof (ThermalConductivityBarrier), false, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.ThermalConductivityBarrier.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Distance, GameUtil.TimeSlice.None));
      this.Insulation = this.Add(new Attribute(nameof (Insulation), false, Attribute.Display.General, true, 0.0f, (string) null, (string) null));
      this.Decor = this.Add(new Attribute(nameof (Decor), false, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.Decor.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.FoodQuality = this.Add(new Attribute(nameof (FoodQuality), false, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.FoodQuality.SetFormatter((IAttributeFormatter) new FoodQualityAttributeFormatter());
      this.ScaldingThreshold = this.Add(new Attribute(nameof (ScaldingThreshold), false, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.ScaldingThreshold.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.None));
      this.GeneratorOutput = this.Add(new Attribute(nameof (GeneratorOutput), false, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.GeneratorOutput.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None));
      this.MachinerySpeed = this.Add(new Attribute(nameof (MachinerySpeed), false, Attribute.Display.General, false, 1f, (string) null, (string) null));
      this.MachinerySpeed.SetFormatter((IAttributeFormatter) new PercentAttributeFormatter());
      this.DecorExpectation = this.Add(new Attribute(nameof (DecorExpectation), false, Attribute.Display.Expectation, false, 0.0f, (string) null, (string) null));
      this.DecorExpectation.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.FoodExpectation = this.Add(new Attribute(nameof (FoodExpectation), false, Attribute.Display.Expectation, false, 0.0f, (string) null, (string) null));
      this.FoodExpectation.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.RoomTemperaturePreference = this.Add(new Attribute(nameof (RoomTemperaturePreference), false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null));
      this.RoomTemperaturePreference.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.None));
      this.QualityOfLifeExpectation = this.Add(new Attribute(nameof (QualityOfLifeExpectation), false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null));
      this.QualityOfLifeExpectation.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.AirConsumptionRate = this.Add(new Attribute(nameof (AirConsumptionRate), false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null));
      this.AirConsumptionRate.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
      this.MaxUnderwaterTravelCost = this.Add(new Attribute(nameof (MaxUnderwaterTravelCost), false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null));
      this.ToiletEfficiency = this.Add(new Attribute(nameof (ToiletEfficiency), false, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.ToiletEfficiency.SetFormatter((IAttributeFormatter) new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
      this.Sneezyness = this.Add(new Attribute(nameof (Sneezyness), false, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.DiseaseCureSpeed = this.Add(new Attribute(nameof (DiseaseCureSpeed), false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null));
      this.DiseaseCureSpeed.BaseValue = 1f;
      this.DiseaseCureSpeed.SetFormatter((IAttributeFormatter) new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
      this.DoctoredLevel = this.Add(new Attribute(nameof (DoctoredLevel), false, Attribute.Display.Never, false, 0.0f, (string) null, (string) null));
      this.CarryAmount = this.Add(new Attribute(nameof (CarryAmount), false, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.CarryAmount.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None));
      this.QualityOfLife = this.Add(new Attribute(nameof (QualityOfLife), false, Attribute.Display.Details, false, 0.0f, "ui_icon_qualityoflife", "attribute_qualityoflife"));
      this.QualityOfLife.SetFormatter((IAttributeFormatter) new QualityOfLifeAttributeFormatter());
      this.GermResistance = this.Add(new Attribute(nameof (GermResistance), false, Attribute.Display.Details, false, 0.0f, "ui_icon_immunelevel", "attribute_immunelevel"));
      this.GermResistance.SetFormatter((IAttributeFormatter) new GermResistanceAttributeFormatter());
      this.LifeSupport = this.Add(new Attribute(nameof (LifeSupport), true, Attribute.Display.Never, false, 0.0f, (string) null, (string) null));
      this.LifeSupport.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
      this.Toggle = this.Add(new Attribute(nameof (Toggle), true, Attribute.Display.Never, false, 0.0f, (string) null, (string) null));
      this.Toggle.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
    }
  }
}
