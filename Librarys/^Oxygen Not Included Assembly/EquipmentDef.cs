// Decompiled with JetBrains decompiler
// Type: EquipmentDef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

public class EquipmentDef : Def
{
  public List<Effect> EffectImmunites = new List<Effect>();
  public float height = 0.325f;
  public List<Descriptor> additionalDescriptors = new List<Descriptor>();
  public string Id;
  public string Slot;
  public string FabricatorId;
  public float FabricationTime;
  public string RecipeTechUnlock;
  public SimHashes OutputElement;
  public Dictionary<string, float> InputElementMassMap;
  public float Mass;
  public KAnimFile Anim;
  public string SnapOn;
  public string SnapOn1;
  public KAnimFile BuildOverride;
  public int BuildOverridePriority;
  public bool IsBody;
  public List<AttributeModifier> AttributeModifiers;
  public string RecipeDescription;
  public System.Action<Equippable> OnEquipCallBack;
  public System.Action<Equippable> OnUnequipCallBack;
  public EntityTemplates.CollisionShape CollisionShape;
  public float width;
  public Tag[] AdditionalTags;

  public override string Name
  {
    get
    {
      return (string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
    }
  }

  public string GenericName
  {
    get
    {
      return (string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
    }
  }
}
