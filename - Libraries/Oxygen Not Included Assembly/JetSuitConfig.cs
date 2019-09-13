﻿// Decompiled with JetBrains decompiler
// Type: JetSuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class JetSuitConfig : IEquipmentConfig
{
  public const string ID = "Jet_Suit";
  public static ComplexRecipe recipe;
  private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasJetPack;
  private AttributeModifier expertAthleticsModifier;

  public EquipmentDef CreateEquipmentDef()
  {
    Dictionary<string, float> dictionary = new Dictionary<string, float>()
    {
      {
        SimHashes.Steel.ToString(),
        200f
      },
      {
        SimHashes.Petroleum.ToString(),
        25f
      }
    };
    List<AttributeModifier> attributeModifierList = new List<AttributeModifier>();
    attributeModifierList.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
    attributeModifierList.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
    attributeModifierList.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
    attributeModifierList.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_DIGGING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
    attributeModifierList.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME, false, false, true));
    this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) -TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, Db.Get().Skills.Suits1.Name, false, false, true);
    string Id = "Jet_Suit";
    string slot = TUNING.EQUIPMENT.SUITS.SLOT;
    SimHashes OutputElement = SimHashes.Steel;
    float atmosuitMass = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS;
    List<AttributeModifier> AttributeModifiers = attributeModifierList;
    Tag[] additional_tags = new Tag[1]{ GameTags.Suit };
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef(Id, slot, OutputElement, atmosuitMass, "suit_jetpack_kanim", string.Empty, "body_jetpack_kanim", 6, AttributeModifiers, (string) null, true, EntityTemplates.CollisionShape.CIRCLE, 0.325f, 0.325f, additional_tags, "JetSuit");
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC;
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("PoppedEarDrums"));
    equipmentDef.OnEquipCallBack = (System.Action<Equippable>) (eq =>
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      Navigator component1 = targetGameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetFlags(PathFinder.PotentialPath.Flags.HasJetPack);
      MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasPerk((HashedString) Db.Get().SkillPerks.ExosuitExpertise.Id))
        targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
      KAnimControllerBase component3 = targetGameObject.GetComponent<KAnimControllerBase>();
      if (!(bool) ((UnityEngine.Object) component3))
        return;
      component3.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_hover_kanim"), 0.0f);
    });
    equipmentDef.OnUnequipCallBack = (System.Action<Equippable>) (eq =>
    {
      if (eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!(bool) ((UnityEngine.Object) soleOwner))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if ((bool) ((UnityEngine.Object) targetGameObject))
      {
        targetGameObject.GetAttributes()?.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
        Navigator component1 = targetGameObject.GetComponent<Navigator>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.ClearFlags(PathFinder.PotentialPath.Flags.HasJetPack);
        KAnimControllerBase component2 = targetGameObject.GetComponent<KAnimControllerBase>();
        if ((bool) ((UnityEngine.Object) component2))
          component2.RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_hover_kanim"));
        Effects component3 = targetGameObject.GetComponent<Effects>();
        if (component3.HasEffect("SoiledSuit"))
          component3.Remove("SoiledSuit");
      }
      eq.GetComponent<Storage>().DropAll(eq.transform.GetPosition(), true, true, new Vector3(), false);
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Jet_Suit");
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.element = "Oxygen";
    suitTank.capacity = 75f;
    go.AddComponent<JetSuitTank>();
    go.AddComponent<HelmetController>().has_jets = true;
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Clothes, false);
    component.AddTag(GameTags.PedestalDisplayable, false);
    component.AddTag(GameTags.AirtightSuit, false);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.showInUI = true;
    go.AddOrGet<AtmoSuit>();
    go.AddComponent<SuitDiseaseHandler>();
  }
}