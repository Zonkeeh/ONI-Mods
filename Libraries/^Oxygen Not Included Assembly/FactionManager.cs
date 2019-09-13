// Decompiled with JetBrains decompiler
// Type: FactionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class FactionManager : KMonoBehaviour
{
  public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);
  public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);
  public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);
  public Faction Predator = new Faction(FactionManager.FactionID.Predator);
  public Faction Prey = new Faction(FactionManager.FactionID.Prey);
  public Faction Pest = new Faction(FactionManager.FactionID.Pest);
  public static FactionManager Instance;

  public static void DestroyInstance()
  {
    FactionManager.Instance = (FactionManager) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FactionManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  public Faction GetFaction(FactionManager.FactionID faction)
  {
    switch (faction)
    {
      case FactionManager.FactionID.Duplicant:
        return this.Duplicant;
      case FactionManager.FactionID.Friendly:
        return this.Friendly;
      case FactionManager.FactionID.Hostile:
        return this.Hostile;
      case FactionManager.FactionID.Prey:
        return this.Prey;
      case FactionManager.FactionID.Predator:
        return this.Predator;
      case FactionManager.FactionID.Pest:
        return this.Pest;
      default:
        return (Faction) null;
    }
  }

  public FactionManager.Disposition GetDisposition(
    FactionManager.FactionID of_faction,
    FactionManager.FactionID to_faction)
  {
    if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
      return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
    return FactionManager.Disposition.Neutral;
  }

  public enum FactionID
  {
    Duplicant,
    Friendly,
    Hostile,
    Prey,
    Predator,
    Pest,
    NumberOfFactions,
  }

  public enum Disposition
  {
    Assist,
    Neutral,
    Attack,
  }
}
