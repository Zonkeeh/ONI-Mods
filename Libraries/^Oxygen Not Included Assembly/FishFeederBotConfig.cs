// Decompiled with JetBrains decompiler
// Type: FishFeederBotConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FishFeederBotConfig : IEntityConfig
{
  public const string ID = "FishFeederBot";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("FishFeederBot", "FishFeederBot", true);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "fishfeeder_kanim")
    };
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
    SymbolOverrideControllerUtil.AddToPrefab(kbatchedAnimController.gameObject);
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
