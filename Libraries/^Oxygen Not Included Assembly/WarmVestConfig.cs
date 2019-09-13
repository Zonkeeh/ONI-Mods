// Decompiled with JetBrains decompiler
// Type: WarmVestConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class WarmVestConfig : IEquipmentConfig
{
  public const string ID = "Warm_Vest";
  public static ComplexRecipe recipe;

  public EquipmentDef CreateEquipmentDef()
  {
    new Dictionary<string, float>()
    {
      {
        "BasicFabric",
        (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS
      }
    };
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.WARM_CLOTHING;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("Warm_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS, TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.WARM_VEST_ANIM0, 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[]) null, (string) null);
    Descriptor descriptor1 = new Descriptor(string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)), string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
    Descriptor descriptor2 = new Descriptor(string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object) ClothingWearer.ClothingInfo.WARM_CLOTHING.decorMod), string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object) ClothingWearer.ClothingInfo.WARM_CLOTHING.decorMod), Descriptor.DescriptorType.Effect, false);
    equipmentDef1.additionalDescriptors.Add(descriptor1);
    equipmentDef1.additionalDescriptors.Add(descriptor2);
    equipmentDef1.OnEquipCallBack = (System.Action<Equippable>) (eq => CoolVestConfig.OnEquipVest(eq, clothingInfo));
    EquipmentDef equipmentDef2 = equipmentDef1;
    // ISSUE: reference to a compiler-generated field
    if (WarmVestConfig.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WarmVestConfig.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Equippable>(CoolVestConfig.OnUnequipVest);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<Equippable> fMgCache0 = WarmVestConfig.\u003C\u003Ef__mg\u0024cache0;
    equipmentDef2.OnUnequipCallBack = fMgCache0;
    equipmentDef1.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC;
    return equipmentDef1;
  }

  public static void SetupVest(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
    Equippable equippable = go.GetComponent<Equippable>();
    if ((UnityEngine.Object) equippable == (UnityEngine.Object) null)
      equippable = go.AddComponent<Equippable>();
    equippable.SetQuality(QualityLevel.Poor);
    go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
  }

  public void DoPostConfigure(GameObject go)
  {
    WarmVestConfig.SetupVest(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
  }
}
