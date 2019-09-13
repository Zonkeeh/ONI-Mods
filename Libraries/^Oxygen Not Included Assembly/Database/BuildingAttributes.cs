// Decompiled with JetBrains decompiler
// Type: Database.BuildingAttributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class BuildingAttributes : ResourceSet<Attribute>
  {
    public Attribute Decor;
    public Attribute DecorRadius;
    public Attribute NoisePollution;
    public Attribute NoisePollutionRadius;
    public Attribute Hygiene;
    public Attribute Comfort;
    public Attribute OverheatTemperature;
    public Attribute FatalTemperature;

    public BuildingAttributes(ResourceSet parent)
      : base(nameof (BuildingAttributes), parent)
    {
      this.Decor = this.Add(new Attribute(nameof (Decor), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.DecorRadius = this.Add(new Attribute(nameof (DecorRadius), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.NoisePollution = this.Add(new Attribute(nameof (NoisePollution), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.NoisePollutionRadius = this.Add(new Attribute(nameof (NoisePollutionRadius), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.Hygiene = this.Add(new Attribute(nameof (Hygiene), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.Comfort = this.Add(new Attribute(nameof (Comfort), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.OverheatTemperature = this.Add(new Attribute(nameof (OverheatTemperature), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.OverheatTemperature.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
      this.FatalTemperature = this.Add(new Attribute(nameof (FatalTemperature), true, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.FatalTemperature.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
    }
  }
}
