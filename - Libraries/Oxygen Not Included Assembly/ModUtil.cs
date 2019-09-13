// Decompiled with JetBrains decompiler
// Type: ModUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KMod;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class ModUtil
{
  public static void AddBuildingToPlanScreen(HashedString category, string building_id)
  {
    int index = BUILDINGS.PLANORDER.FindIndex((Predicate<PlanScreen.PlanInfo>) (x => x.category == category));
    if (index < 0)
      return;
    (BUILDINGS.PLANORDER[index].data as IList<string>).Add(building_id);
  }

  public static void AddBuildingToHotkeyBuildMenu(
    HashedString category,
    string building_id,
    Action hotkey)
  {
    BuildMenu.DisplayInfo info = BuildMenu.OrderedBuildings.GetInfo(category);
    if (info.category != category)
      return;
    (info.data as IList<BuildMenu.BuildingInfo>).Add(new BuildMenu.BuildingInfo(building_id, hotkey));
  }

  public static KAnimFile AddKAnimMod(string name, KAnimFile.Mod anim_mod)
  {
    KAnimFile instance = ScriptableObject.CreateInstance<KAnimFile>();
    instance.mod = anim_mod;
    instance.name = name;
    AnimCommandFile akf = new AnimCommandFile();
    KAnimGroupFile.GroupFile gf = new KAnimGroupFile.GroupFile();
    gf.groupID = akf.GetGroupName(instance);
    gf.commandDirectory = "assets/" + name;
    akf.AddGroupFile(gf);
    if (KAnimGroupFile.GetGroupFile().AddAnimMod(gf, akf, instance) == KAnimGroupFile.AddModResult.Added)
      Assets.ModLoadedKAnims.Add(instance);
    return instance;
  }

  public static KAnimFile AddKAnim(
    string name,
    TextAsset anim_file,
    TextAsset build_file,
    IList<Texture2D> textures)
  {
    KAnimFile instance = ScriptableObject.CreateInstance<KAnimFile>();
    instance.Initialize(anim_file, build_file, textures);
    instance.name = name;
    AnimCommandFile akf = new AnimCommandFile();
    KAnimGroupFile.GroupFile gf = new KAnimGroupFile.GroupFile();
    gf.groupID = akf.GetGroupName(instance);
    gf.commandDirectory = "assets/" + name;
    akf.AddGroupFile(gf);
    KAnimGroupFile.GetGroupFile().AddAnimFile(gf, akf, instance);
    Assets.ModLoadedKAnims.Add(instance);
    return instance;
  }

  public static KAnimFile AddKAnim(
    string name,
    TextAsset anim_file,
    TextAsset build_file,
    Texture2D texture)
  {
    return ModUtil.AddKAnim(name, anim_file, build_file, (IList<Texture2D>) new List<Texture2D>()
    {
      texture
    });
  }

  public static Substance CreateSubstance(
    string name,
    Element.State state,
    KAnimFile kanim,
    Material material,
    Color32 colour,
    Color32 ui_colour,
    Color32 conduit_colour)
  {
    return new Substance()
    {
      name = name,
      nameTag = TagManager.Create(name),
      elementID = (SimHashes) Hash.SDBMLower(name),
      anim = kanim,
      colour = colour,
      uiColour = ui_colour,
      conduitColour = conduit_colour,
      material = material,
      renderedByWorld = (state & Element.State.Solid) == Element.State.Solid
    };
  }

  public static void RegisterForTranslation(System.Type locstring_tree_root)
  {
    Localization.RegisterForTranslation(locstring_tree_root);
    Localization.GenerateStringsTemplate(locstring_tree_root, System.IO.Path.Combine(Manager.GetDirectory(), "strings_templates"));
  }
}
