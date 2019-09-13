// Decompiled with JetBrains decompiler
// Type: Database.AttributeConverters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;

namespace Database
{
  public class AttributeConverters : ResourceSet<AttributeConverter>
  {
    public AttributeConverter MovementSpeed;
    public AttributeConverter ConstructionSpeed;
    public AttributeConverter DiggingSpeed;
    public AttributeConverter MachinerySpeed;
    public AttributeConverter HarvestSpeed;
    public AttributeConverter PlantTendSpeed;
    public AttributeConverter CompoundingSpeed;
    public AttributeConverter ResearchSpeed;
    public AttributeConverter TrainingSpeed;
    public AttributeConverter CookingSpeed;
    public AttributeConverter ArtSpeed;
    public AttributeConverter DoctorSpeed;
    public AttributeConverter TidyingSpeed;
    public AttributeConverter AttackDamage;
    public AttributeConverter ImmuneLevelBoost;
    public AttributeConverter ToiletSpeed;
    public AttributeConverter CarryAmountFromStrength;
    public AttributeConverter TemperatureInsulation;
    public AttributeConverter SeedHarvestChance;
    public AttributeConverter RanchingEffectDuration;

    public AttributeConverters()
    {
      ToPercentAttributeFormatter attributeFormatter1 = new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None);
      StandardAttributeFormatter attributeFormatter2 = new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None);
      this.MovementSpeed = this.Create(nameof (MovementSpeed), "Movement Speed", (string) DUPLICANTS.ATTRIBUTES.ATHLETICS.SPEEDMODIFIER, Db.Get().Attributes.Athletics, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.ConstructionSpeed = this.Create(nameof (ConstructionSpeed), "Construction Speed", (string) DUPLICANTS.ATTRIBUTES.CONSTRUCTION.SPEEDMODIFIER, Db.Get().Attributes.Construction, 0.25f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.DiggingSpeed = this.Create(nameof (DiggingSpeed), "Digging Speed", (string) DUPLICANTS.ATTRIBUTES.DIGGING.SPEEDMODIFIER, Db.Get().Attributes.Digging, 0.25f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.MachinerySpeed = this.Create(nameof (MachinerySpeed), "Machinery Speed", (string) DUPLICANTS.ATTRIBUTES.MACHINERY.SPEEDMODIFIER, Db.Get().Attributes.Machinery, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.HarvestSpeed = this.Create(nameof (HarvestSpeed), "Harvest Speed", (string) DUPLICANTS.ATTRIBUTES.BOTANIST.HARVEST_SPEED_MODIFIER, Db.Get().Attributes.Botanist, 0.05f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.PlantTendSpeed = this.Create(nameof (PlantTendSpeed), "Plant Tend Speed", (string) DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_MODIFIER, Db.Get().Attributes.Botanist, 0.025f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.CompoundingSpeed = this.Create(nameof (CompoundingSpeed), "Compounding Speed", (string) DUPLICANTS.ATTRIBUTES.CARING.FABRICATE_SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.ResearchSpeed = this.Create(nameof (ResearchSpeed), "Research Speed", (string) DUPLICANTS.ATTRIBUTES.LEARNING.RESEARCHSPEED, Db.Get().Attributes.Learning, 0.4f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.TrainingSpeed = this.Create(nameof (TrainingSpeed), "Training Speed", (string) DUPLICANTS.ATTRIBUTES.LEARNING.SPEEDMODIFIER, Db.Get().Attributes.Learning, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.CookingSpeed = this.Create(nameof (CookingSpeed), "Cooking Speed", (string) DUPLICANTS.ATTRIBUTES.COOKING.SPEEDMODIFIER, Db.Get().Attributes.Cooking, 0.05f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.ArtSpeed = this.Create(nameof (ArtSpeed), "Art Speed", (string) DUPLICANTS.ATTRIBUTES.ART.SPEEDMODIFIER, Db.Get().Attributes.Art, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.DoctorSpeed = this.Create(nameof (DoctorSpeed), "Doctor Speed", (string) DUPLICANTS.ATTRIBUTES.CARING.SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.2f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.TidyingSpeed = this.Create(nameof (TidyingSpeed), "Tidying Speed", (string) DUPLICANTS.ATTRIBUTES.STRENGTH.SPEEDMODIFIER, Db.Get().Attributes.Strength, 0.25f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.AttackDamage = this.Create(nameof (AttackDamage), "Attack Damage", (string) DUPLICANTS.ATTRIBUTES.DIGGING.ATTACK_MODIFIER, Db.Get().Attributes.Digging, 0.05f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.ImmuneLevelBoost = this.Create(nameof (ImmuneLevelBoost), "Immune Level Boost", (string) DUPLICANTS.ATTRIBUTES.IMMUNITY.BOOST_MODIFIER, Db.Get().Attributes.Immunity, 1f / 600f, 0.0f, (IAttributeFormatter) new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.PerCycle));
      this.ToiletSpeed = this.Create(nameof (ToiletSpeed), "Toilet Speed", string.Empty, Db.Get().Attributes.ToiletEfficiency, 1f, -1f, (IAttributeFormatter) attributeFormatter1);
      this.CarryAmountFromStrength = this.Create(nameof (CarryAmountFromStrength), "Carry Amount", (string) DUPLICANTS.ATTRIBUTES.STRENGTH.CARRYMODIFIER, Db.Get().Attributes.Strength, 40f, 0.0f, (IAttributeFormatter) attributeFormatter2);
      this.TemperatureInsulation = this.Create(nameof (TemperatureInsulation), "Temperature Insulation", (string) DUPLICANTS.ATTRIBUTES.INSULATION.SPEEDMODIFIER, Db.Get().Attributes.Insulation, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.SeedHarvestChance = this.Create(nameof (SeedHarvestChance), "Seed Harvest Chance", (string) DUPLICANTS.ATTRIBUTES.BOTANIST.BONUS_SEEDS, Db.Get().Attributes.Botanist, 0.033f, 0.0f, (IAttributeFormatter) attributeFormatter1);
      this.RanchingEffectDuration = this.Create(nameof (RanchingEffectDuration), "Ranching Effect Duration", (string) DUPLICANTS.ATTRIBUTES.RANCHING.EFFECTMODIFIER, Db.Get().Attributes.Ranching, 0.1f, 0.0f, (IAttributeFormatter) attributeFormatter1);
    }

    public AttributeConverter Create(
      string id,
      string name,
      string description,
      Attribute attribute,
      float multiplier,
      float base_value,
      IAttributeFormatter formatter)
    {
      AttributeConverter resource = new AttributeConverter(id, name, description, multiplier, base_value, attribute, formatter);
      this.Add(resource);
      attribute.converters.Add(resource);
      return resource;
    }

    public List<AttributeConverter> GetConvertersForAttribute(Attribute attrib)
    {
      List<AttributeConverter> attributeConverterList = new List<AttributeConverter>();
      foreach (AttributeConverter resource in this.resources)
      {
        if (resource.attribute == attrib)
          attributeConverterList.Add(resource);
      }
      return attributeConverterList;
    }
  }
}
