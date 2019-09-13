// Decompiled with JetBrains decompiler
// Type: Database.Personalities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class Personalities : ResourceSet<Personality>
  {
    public Personalities()
    {
      foreach (Personalities.PersonalityInfo entry in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
        this.Add(new Personality(entry.Name.ToUpper(), (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", (object) entry.Name.ToUpper())), entry.Gender.ToUpper(), entry.PersonalityType, entry.StressTrait, entry.CongenitalTrait, entry.HeadShape, entry.Mouth, entry.Neck, entry.Eyes, entry.Hair, entry.Body, (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", (object) entry.Name.ToUpper()))));
    }

    private void AddTrait(Personality personality, string trait_name)
    {
      Trait trait = Db.Get().traits.TryGet(trait_name);
      if (trait == null)
        return;
      personality.AddTrait(trait);
    }

    private void SetAttribute(Personality personality, string attribute_name, int value)
    {
      Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
      if (attribute == null)
        Debug.LogWarning((object) ("Attribute does not exist: " + attribute_name));
      else
        personality.SetAttribute(attribute, value);
    }

    public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
    {
      public PersonalityLoader()
        : base(Assets.instance.personalitiesFile)
      {
      }

      public override void Run()
      {
        base.Run();
      }
    }

    public class PersonalityInfo : Resource
    {
      public int HeadShape;
      public int Mouth;
      public int Neck;
      public int Eyes;
      public int Hair;
      public int Body;
      public string Gender;
      public string PersonalityType;
      public string StressTrait;
      public string CongenitalTrait;
      public string Design;
    }
  }
}
