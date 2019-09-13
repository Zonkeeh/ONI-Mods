// Decompiled with JetBrains decompiler
// Type: Database.Sicknesses
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class Sicknesses : ResourceSet<Sickness>
  {
    public Sickness FoodSickness;
    public Sickness SlimeSickness;
    public Sickness ZombieSickness;
    public Sickness Allergies;
    public Sickness ColdBrain;
    public Sickness HeatRash;
    public Sickness Sunburn;

    public Sicknesses(ResourceSet parent)
      : base(nameof (Sicknesses), parent)
    {
      this.FoodSickness = this.Add((Sickness) new Klei.AI.FoodSickness());
      this.SlimeSickness = this.Add((Sickness) new Klei.AI.SlimeSickness());
      this.ZombieSickness = this.Add((Sickness) new Klei.AI.ZombieSickness());
      this.Allergies = this.Add((Sickness) new Klei.AI.Allergies());
      this.ColdBrain = this.Add((Sickness) new Klei.AI.ColdBrain());
      this.HeatRash = this.Add((Sickness) new Klei.AI.HeatRash());
      this.Sunburn = this.Add((Sickness) new Klei.AI.Sunburn());
    }

    public static bool IsValidID(string id)
    {
      bool flag = false;
      foreach (Resource resource in Db.Get().Sicknesses.resources)
      {
        if (resource.Id == id)
          flag = true;
      }
      return flag;
    }
  }
}
