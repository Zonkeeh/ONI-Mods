// Decompiled with JetBrains decompiler
// Type: Database.Diseases
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class Diseases : ResourceSet<Klei.AI.Disease>
  {
    public Klei.AI.Disease FoodGerms;
    public Klei.AI.Disease SlimeGerms;
    public Klei.AI.Disease PollenGerms;
    public Klei.AI.Disease ZombieSpores;

    public Diseases(ResourceSet parent)
      : base(nameof (Diseases), parent)
    {
      this.FoodGerms = this.Add((Klei.AI.Disease) new Klei.AI.FoodGerms());
      this.SlimeGerms = this.Add((Klei.AI.Disease) new Klei.AI.SlimeGerms());
      this.PollenGerms = this.Add((Klei.AI.Disease) new Klei.AI.PollenGerms());
      this.ZombieSpores = this.Add((Klei.AI.Disease) new Klei.AI.ZombieSpores());
    }

    public static bool IsValidID(string id)
    {
      bool flag = false;
      foreach (Resource resource in Db.Get().Diseases.resources)
      {
        if (resource.Id == id)
          flag = true;
      }
      return flag;
    }

    public byte GetIndex(int hash)
    {
      Diseases diseases = Db.Get().Diseases;
      for (byte index = 0; (int) index < diseases.Count; ++index)
      {
        Klei.AI.Disease disease = diseases[(int) index];
        if (hash == disease.id.GetHashCode())
          return index;
      }
      return byte.MaxValue;
    }

    public byte GetIndex(HashedString id)
    {
      return this.GetIndex(id.GetHashCode());
    }
  }
}
