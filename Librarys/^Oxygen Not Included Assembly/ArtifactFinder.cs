// Decompiled with JetBrains decompiler
// Type: ArtifactFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using TUNING;

[SkipSaveFileSerialization]
public class ArtifactFinder : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ArtifactFinder> OnLandDelegate = new EventSystem.IntraObjectHandler<ArtifactFinder>((System.Action<ArtifactFinder, object>) ((component, data) => component.OnLand(data)));
  public const string ID = "ArtifactFinder";
  [MyCmpReq]
  private MinionStorage minionStorage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<ArtifactFinder>(238242047, ArtifactFinder.OnLandDelegate);
  }

  public ArtifactTier GetArtifactDropTier(
    StoredMinionIdentity minionID,
    SpaceDestination destination)
  {
    ArtifactDropRate artifactDropTable = destination.GetDestinationType().artifactDropTable;
    bool flag = minionID.traitIDs.Contains("Archaeologist");
    if (artifactDropTable != null)
    {
      float totalWeight = artifactDropTable.totalWeight;
      if (flag)
        totalWeight -= artifactDropTable.GetTierWeight(DECOR.SPACEARTIFACT.TIER_NONE);
      float num = UnityEngine.Random.value * totalWeight;
      foreach (Tuple<ArtifactTier, float> rate in artifactDropTable.rates)
      {
        if (!flag || flag && rate.first != DECOR.SPACEARTIFACT.TIER_NONE)
          num -= rate.second;
        if ((double) num <= 0.0)
          return rate.first;
      }
    }
    return DECOR.SPACEARTIFACT.TIER0;
  }

  public List<string> GetArtifactsOfTier(ArtifactTier tier)
  {
    List<string> stringList = new List<string>();
    foreach (string artifactItem in ArtifactConfig.artifactItems)
    {
      if (Assets.GetPrefab(artifactItem.ToTag()).GetComponent<SpaceArtifact>().GetArtifactTier() == tier)
        stringList.Add(artifactItem);
    }
    return stringList;
  }

  public string SearchForArtifact(StoredMinionIdentity minionID, SpaceDestination destination)
  {
    ArtifactTier artifactDropTier = this.GetArtifactDropTier(minionID, destination);
    if (artifactDropTier == DECOR.SPACEARTIFACT.TIER_NONE)
      return (string) null;
    List<string> artifactsOfTier = this.GetArtifactsOfTier(artifactDropTier);
    return artifactsOfTier[UnityEngine.Random.Range(0, artifactsOfTier.Count - 1)];
  }

  public void OnLand(object data)
  {
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(this.GetComponent<RocketModule>().conditionManager.GetComponent<LaunchableRocket>()));
    foreach (MinionStorage.Info info in this.minionStorage.GetStoredMinionInfo())
    {
      string str = this.SearchForArtifact(info.serializedMinion.Get<StoredMinionIdentity>(), spacecraftDestination);
      if (str != null)
        GameUtil.KInstantiate(Assets.GetPrefab(str.ToTag()), this.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, (string) null, 0).SetActive(true);
    }
  }
}
