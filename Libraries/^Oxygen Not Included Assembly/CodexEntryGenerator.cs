// Decompiled with JetBrains decompiler
// Type: CodexEntryGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class CodexEntryGenerator
{
  public static Dictionary<string, CodexEntry> GenerateBuildingEntries()
  {
    string str1 = "BUILD_CATEGORY_";
    Dictionary<string, CodexEntry> categoryEntries = new Dictionary<string, CodexEntry>();
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      string str2 = HashCache.Get().Get(planInfo.category);
      string str3 = CodexCache.FormatLinkID(str1 + str2);
      Dictionary<string, CodexEntry> entries = new Dictionary<string, CodexEntry>();
      for (int index = 0; index < (planInfo.data as IList<string>).Count; ++index)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef((planInfo.data as IList<string>)[index]);
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(buildingDef.Name, contentContainerList);
        CodexEntryGenerator.GenerateImageContainers(buildingDef.GetUISprite("ui", false), contentContainerList);
        CodexEntryGenerator.GenerateBuildingDescriptionContainers(buildingDef, contentContainerList);
        CodexEntryGenerator.GenerateFabricatorContainers(buildingDef.BuildingComplete, contentContainerList);
        CodexEntryGenerator.GenerateReceptacleContainers(buildingDef.BuildingComplete, contentContainerList);
        CodexEntry entry = new CodexEntry(str3, contentContainerList, (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + (planInfo.data as IList<string>)[index].ToUpper() + ".NAME"));
        entry.icon = buildingDef.GetUISprite("ui", false);
        entry.parentId = str3;
        CodexCache.AddEntry((planInfo.data as IList<string>)[index], entry, (List<CategoryEntry>) null);
        entries.Add(entry.id, entry);
      }
      CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(str3), (string) Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str2.ToUpper() + ".NAME"), entries, (Sprite) null, true, true, (string) null);
      categoryEntry.parentId = "BUILDINGS";
      categoryEntry.category = "BUILDINGS";
      categoryEntry.icon = Assets.GetSprite((HashedString) PlanScreen.IconNameMap[(HashedString) str2]);
      categoryEntries.Add(str3, (CodexEntry) categoryEntry);
    }
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries);
    return categoryEntries;
  }

  public static void GeneratePageNotFound()
  {
    CodexCache.AddEntry("PageNotFound", new CodexEntry("ROOT", new List<ContentContainer>()
    {
      new ContentContainer()
      {
        content = {
          (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.TITLE, CodexTextStyle.Title),
          (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.SUBTITLE, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexDividerLine(),
          (ICodexWidget) new CodexImage(312, 312, Assets.GetSprite((HashedString) "outhouseMessage"))
        }
      }
    }, (string) CODEX.PAGENOTFOUND.TITLE)
    {
      searchOnly = true
    }, (List<CategoryEntry>) null);
  }

  public static Dictionary<string, CodexEntry> GenerateCreatureEntries()
  {
    Dictionary<string, CodexEntry> results = new Dictionary<string, CodexEntry>();
    List<GameObject> brains = Assets.GetPrefabsWithComponent<CreatureBrain>();
    System.Action<Tag, string> action = (System.Action<Tag, string>) ((speciesTag, name) =>
    {
      CodexEntry entry = new CodexEntry("CREATURES", new List<ContentContainer>()
      {
        new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexSpacer()
        }, ContentContainer.ContentLayout.Vertical)
      }, name);
      entry.parentId = "CREATURES";
      CodexCache.AddEntry(speciesTag.ToString(), entry, (List<CategoryEntry>) null);
      results.Add(speciesTag.ToString(), entry);
      foreach (GameObject gameObject in brains)
      {
        if (gameObject.GetDef<BabyMonitor.Def>() == null)
        {
          Sprite sprite = (Sprite) null;
          GameObject prefab = Assets.TryGetPrefab((Tag) (gameObject.PrefabID().ToString() + "Baby"));
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
            sprite = Def.GetUISprite((object) prefab, "ui", false).first;
          CreatureBrain component = gameObject.GetComponent<CreatureBrain>();
          if (!(component.species != speciesTag))
          {
            List<ContentContainer> contentContainerList = new List<ContentContainer>();
            string symbolPrefix = component.symbolPrefix;
            Sprite first = Def.GetUISprite((object) gameObject, symbolPrefix + "ui", false).first;
            if ((bool) ((UnityEngine.Object) sprite))
              CodexEntryGenerator.GenerateImageContainers(new Sprite[2]
              {
                first,
                sprite
              }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
            else
              CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
            CodexEntryGenerator.GenerateCreatureDescriptionContainers(gameObject, contentContainerList);
            entry.subEntries.Add(new SubEntry(component.PrefabID().ToString(), speciesTag.ToString(), contentContainerList, component.GetProperName())
            {
              icon = first,
              iconColor = Color.white
            });
          }
        }
      }
    });
    action(GameTags.Creatures.Species.PuftSpecies, (string) STRINGS.CREATURES.FAMILY.PUFT);
    action(GameTags.Creatures.Species.PacuSpecies, (string) STRINGS.CREATURES.FAMILY.PACU);
    action(GameTags.Creatures.Species.OilFloaterSpecies, (string) STRINGS.CREATURES.FAMILY.OILFLOATER);
    action(GameTags.Creatures.Species.LightBugSpecies, (string) STRINGS.CREATURES.FAMILY.LIGHTBUG);
    action(GameTags.Creatures.Species.HatchSpecies, (string) STRINGS.CREATURES.FAMILY.HATCH);
    action(GameTags.Creatures.Species.GlomSpecies, (string) STRINGS.CREATURES.FAMILY.GLOM);
    action(GameTags.Creatures.Species.DreckoSpecies, (string) STRINGS.CREATURES.FAMILY.DRECKO);
    action(GameTags.Creatures.Species.MooSpecies, (string) STRINGS.CREATURES.FAMILY.MOO);
    action(GameTags.Creatures.Species.MoleSpecies, (string) STRINGS.CREATURES.FAMILY.MOLE);
    action(GameTags.Creatures.Species.SquirrelSpecies, (string) STRINGS.CREATURES.FAMILY.SQUIRREL);
    action(GameTags.Creatures.Species.CrabSpecies, (string) STRINGS.CREATURES.FAMILY.CRAB);
    return results;
  }

  public static Dictionary<string, CodexEntry> GeneratePlantEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Harvestable>();
    prefabsWithComponent.AddRange((IEnumerable<GameObject>) Assets.GetPrefabsWithComponent<WiltCondition>());
    foreach (GameObject gameObject in prefabsWithComponent)
    {
      if (!dictionary.ContainsKey(gameObject.PrefabID().ToString()) && !((UnityEngine.Object) gameObject.GetComponent<BudUprootedMonitor>() != (UnityEngine.Object) null))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        Sprite first = Def.GetUISprite((object) gameObject, "ui", false).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        CodexEntryGenerator.GeneratePlantDescriptionContainers(gameObject, contentContainerList);
        CodexEntry entry = new CodexEntry("PLANTS", contentContainerList, gameObject.GetProperName());
        entry.parentId = "PLANTS";
        entry.icon = first;
        CodexCache.AddEntry(gameObject.PrefabID().ToString(), entry, (List<CategoryEntry>) null);
        dictionary.Add(gameObject.PrefabID().ToString(), entry);
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateFoodEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (EdiblesManager.FoodInfo foodTypes in TUNING.FOOD.FOOD_TYPES_LIST)
    {
      if (!Assets.GetPrefab((Tag) foodTypes.Id).HasTag(GameTags.IncubatableEgg))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(foodTypes.Name, contentContainerList);
        Sprite first = Def.GetUISprite((object) foodTypes.ConsumableId, "ui", false).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        CodexEntryGenerator.GenerateFoodDescriptionContainers(foodTypes, contentContainerList);
        CodexEntryGenerator.GenerateRecipeContainers(foodTypes.ConsumableId.ToTag(), contentContainerList);
        CodexEntryGenerator.GenerateUsedInRecipeContainers(foodTypes.ConsumableId.ToTag(), contentContainerList);
        CodexEntry entry = new CodexEntry("FOOD", contentContainerList, foodTypes.Name);
        entry.icon = first;
        entry.parentId = "FOOD";
        CodexCache.AddEntry(foodTypes.Id, entry, (List<CategoryEntry>) null);
        dictionary.Add(foodTypes.Id, entry);
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateTechEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Tech resource in Db.Get().Techs.resources)
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
      CodexEntryGenerator.GenerateTechDescriptionContainers(resource, contentContainerList);
      CodexEntryGenerator.GeneratePrerequisiteTechContainers(resource, contentContainerList);
      CodexEntryGenerator.GenerateUnlockContainers(resource, contentContainerList);
      CodexEntry entry = new CodexEntry("TECH", contentContainerList, resource.Name);
      TechItem unlockedItem = resource.unlockedItems[0];
      if (unlockedItem == null)
        DebugUtil.LogErrorArgs((object) "Unknown tech:", (object) resource.Name);
      entry.icon = unlockedItem.getUISprite("ui", false);
      entry.parentId = "TECH";
      CodexCache.AddEntry(resource.Id, entry, (List<CategoryEntry>) null);
      dictionary.Add(resource.Id, entry);
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateRoleEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      Sprite sprite = Assets.GetSprite((HashedString) resource.hat);
      CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
      CodexEntryGenerator.GenerateImageContainers(sprite, contentContainerList);
      CodexEntryGenerator.GenerateGenericDescriptionContainers(resource.description, contentContainerList);
      CodexEntryGenerator.GenerateSkillRequirementsAndPerksContainers(resource, contentContainerList);
      CodexEntryGenerator.GenerateRelatedSkillContainers(resource, contentContainerList);
      CodexEntry entry = new CodexEntry("ROLES", contentContainerList, resource.Name);
      entry.parentId = "ROLES";
      entry.icon = sprite;
      CodexCache.AddEntry(resource.Id, entry, (List<CategoryEntry>) null);
      dictionary.Add(resource.Id, entry);
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateGeyserEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Geyser>();
    if (prefabsWithComponent != null)
    {
      foreach (GameObject go in prefabsWithComponent)
      {
        if (!go.GetComponent<KPrefabID>().HasTag(GameTags.DeprecatedContent))
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
          Sprite first = Def.GetUISprite((object) go, "ui", false).first;
          CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          List<ICodexWidget> content = new List<ICodexWidget>();
          string upper = go.PrefabID().ToString().Remove(0, 14).ToUpper();
          content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CREATURES.SPECIES.GEYSER." + upper + ".DESC"), CodexTextStyle.Body));
          content.Add((ICodexWidget) new CodexText((string) UI.CODEX.GEYSERS.DESC, CodexTextStyle.Body));
          ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
          contentContainerList.Add(contentContainer);
          CodexEntry entry = new CodexEntry("GEYSERS", contentContainerList, go.GetProperName());
          entry.icon = first;
          entry.parentId = "GEYSERS";
          entry.id = go.PrefabID().ToString();
          CodexCache.AddEntry(entry.id, entry, (List<CategoryEntry>) null);
          dictionary.Add(entry.id, entry);
        }
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateEquipmentEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Equippable>();
    if (prefabsWithComponent != null)
    {
      foreach (GameObject go in prefabsWithComponent)
      {
        bool flag = false;
        Equippable component = go.GetComponent<Equippable>();
        if (component.def.AdditionalTags != null)
        {
          foreach (Tag additionalTag in component.def.AdditionalTags)
          {
            if (additionalTag == GameTags.DeprecatedContent)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
          Sprite first = Def.GetUISprite((object) go, "ui", false).first;
          CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          List<ICodexWidget> content = new List<ICodexWidget>();
          string str = go.PrefabID().ToString();
          content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + str.ToUpper() + ".DESC"), CodexTextStyle.Body));
          ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
          contentContainerList.Add(contentContainer);
          CodexEntry entry = new CodexEntry("EQUIPMENT", contentContainerList, go.GetProperName());
          entry.icon = first;
          entry.parentId = "EQUIPMENT";
          entry.id = go.PrefabID().ToString();
          CodexCache.AddEntry(entry.id, entry, (List<CategoryEntry>) null);
          dictionary.Add(entry.id, entry);
        }
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateElementEntries()
  {
    Dictionary<string, CodexEntry> categoryEntries = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries1 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries2 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries3 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries4 = new Dictionary<string, CodexEntry>();
    string str1 = CodexCache.FormatLinkID("ELEMENTS");
    string str2 = CodexCache.FormatLinkID("ELEMENTS_SOLID");
    string str3 = CodexCache.FormatLinkID("ELEMENTS_LIQUID");
    string str4 = CodexCache.FormatLinkID("ELEMENTS_GAS");
    string str5 = CodexCache.FormatLinkID("ELEMENTS_OTHER");
    System.Action<Element, List<ContentContainer>> action = (System.Action<Element, List<ContentContainer>>) ((element, containers) =>
    {
      if (element.highTempTransition != null || element.lowTempTransition != null)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.ELEMENTTRANSITIONS, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexDividerLine()
        }, ContentContainer.ContentLayout.Vertical));
      if (element.highTempTransition != null)
      {
        List<ContentContainer> contentContainerList = containers;
        List<ICodexWidget> content = new List<ICodexWidget>();
        content.Add((ICodexWidget) new CodexImage(32, 32, Def.GetUISprite((object) element.highTempTransition, "ui", false)));
        List<ICodexWidget> codexWidgetList = content;
        string text;
        if (element.highTempTransition != null)
          text = element.highTempTransition.name + " (" + element.highTempTransition.GetStateString() + ")  (" + GameUtil.GetFormattedTemperature(element.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + ")";
        else
          text = string.Empty;
        CodexText codexText = new CodexText(text, CodexTextStyle.Body);
        codexWidgetList.Add((ICodexWidget) codexText);
        ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Horizontal);
        contentContainerList.Add(contentContainer);
      }
      if (element.lowTempTransition != null)
      {
        List<ContentContainer> contentContainerList = containers;
        List<ICodexWidget> content = new List<ICodexWidget>();
        content.Add((ICodexWidget) new CodexImage(32, 32, Def.GetUISprite((object) element.lowTempTransition, "ui", false)));
        List<ICodexWidget> codexWidgetList = content;
        string text;
        if (element.lowTempTransition != null)
          text = element.lowTempTransition.name + " (" + element.lowTempTransition.GetStateString() + ")  (" + GameUtil.GetFormattedTemperature(element.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + ")";
        else
          text = string.Empty;
        CodexText codexText = new CodexText(text, CodexTextStyle.Body);
        codexWidgetList.Add((ICodexWidget) codexText);
        ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Horizontal);
        contentContainerList.Add(contentContainer);
      }
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText(element.FullDescription(true), CodexTextStyle.Body),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical));
    });
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        string name = element.name + " (" + element.GetStateString() + ")";
        CodexEntryGenerator.GenerateTitleContainers(name, contentContainerList);
        Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) element, "ui", false);
        CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1]
        {
          Def.GetUISprite((object) element, "ui", false)
        }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
        action(element, contentContainerList);
        string str6 = element.id.ToString();
        string category;
        Dictionary<string, CodexEntry> dictionary;
        if (element.IsSolid)
        {
          category = str2;
          dictionary = entries1;
        }
        else if (element.IsLiquid)
        {
          category = str3;
          dictionary = entries2;
        }
        else if (element.IsGas)
        {
          category = str4;
          dictionary = entries3;
        }
        else
        {
          category = str5;
          dictionary = entries4;
        }
        CodexEntry entry = new CodexEntry(category, contentContainerList, name);
        entry.parentId = category;
        entry.icon = uiSprite.first;
        entry.iconColor = uiSprite.second;
        CodexCache.AddEntry(str6, entry, (List<CategoryEntry>) null);
        dictionary.Add(str6, entry);
      }
    }
    string str7 = str2;
    CodexEntry categoryEntry1 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str7, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, entries1, (Sprite) null, true, true, (string) null);
    categoryEntry1.parentId = str1;
    categoryEntry1.category = str1;
    categoryEntries.Add(str7, categoryEntry1);
    string str8 = str3;
    CodexEntry categoryEntry2 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str8, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, entries2, (Sprite) null, true, true, (string) null);
    categoryEntry2.parentId = str1;
    categoryEntry2.category = str1;
    categoryEntries.Add(str8, categoryEntry2);
    string str9 = str4;
    CodexEntry categoryEntry3 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str9, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, entries3, (Sprite) null, true, true, (string) null);
    categoryEntry3.parentId = str1;
    categoryEntry3.category = str1;
    categoryEntries.Add(str9, categoryEntry3);
    string str10 = str5;
    CodexEntry categoryEntry4 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str10, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, entries4, Assets.GetSprite((HashedString) "overlay_heatflow"), true, true, (string) null);
    categoryEntry4.parentId = str1;
    categoryEntry4.category = str1;
    categoryEntries.Add(str10, categoryEntry4);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries);
    return categoryEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateDiseaseEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
    {
      if (!resource.Disabled)
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
        CodexEntryGenerator.GenerateDiseaseDescriptionContainers(resource, contentContainerList);
        CodexEntry entry = new CodexEntry("DISEASE", contentContainerList, resource.Name);
        entry.parentId = "DISEASE";
        dictionary.Add(resource.Id, entry);
        entry.icon = Assets.GetSprite((HashedString) "overlay_disease");
        CodexCache.AddEntry(resource.Id, entry, (List<CategoryEntry>) null);
      }
    }
    return dictionary;
  }

  public static CategoryEntry GenerateCategoryEntry(
    string id,
    string name,
    Dictionary<string, CodexEntry> entries,
    Sprite icon = null,
    bool largeFormat = true,
    bool sort = true,
    string overrideHeader = null)
  {
    List<ContentContainer> contentContainerList = new List<ContentContainer>();
    CodexEntryGenerator.GenerateTitleContainers(overrideHeader != null ? overrideHeader : name, contentContainerList);
    List<CodexEntry> entriesInCategory = new List<CodexEntry>();
    foreach (KeyValuePair<string, CodexEntry> entry in entries)
    {
      entriesInCategory.Add(entry.Value);
      if ((UnityEngine.Object) icon == (UnityEngine.Object) null)
        icon = entry.Value.icon;
    }
    CategoryEntry categoryEntry = new CategoryEntry("Root", contentContainerList, name, entriesInCategory, largeFormat, sort);
    categoryEntry.icon = icon;
    CodexCache.AddEntry(id, (CodexEntry) categoryEntry, (List<CategoryEntry>) null);
    return categoryEntry;
  }

  public static Dictionary<string, CodexEntry> GenerateTutorialNotificationEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    for (int index = 0; index < 20; ++index)
    {
      TutorialMessage tutorialMessage = (TutorialMessage) Tutorial.Instance.TutorialMessage((Tutorial.TutorialMessages) index, false);
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), contentContainerList);
      if (!string.IsNullOrEmpty(tutorialMessage.videoClipId))
        contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexVideo()
          {
            videoName = tutorialMessage.videoClipId,
            overlayName = tutorialMessage.videoOverlayName,
            overlayTexts = new List<string>()
            {
              tutorialMessage.videoTitleText,
              (string) VIDEOS.TUTORIAL_HEADER
            }
          }
        }, ContentContainer.ContentLayout.Vertical));
      contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText(tutorialMessage.GetMessageBody(), CodexTextStyle.Body)
      }, ContentContainer.ContentLayout.Vertical));
      CodexEntry entry = new CodexEntry("Tips", contentContainerList, UI.FormatAsLink(tutorialMessage.GetTitle(), "tutorial_tips_" + (object) index));
      CodexCache.AddEntry("tutorial_tips_" + (object) index, entry, (List<CategoryEntry>) null);
      dictionary.Add(entry.id, entry);
    }
    return dictionary;
  }

  public static void PopulateCategoryEntries(Dictionary<string, CodexEntry> categoryEntries)
  {
    List<CategoryEntry> categoryEntries1 = new List<CategoryEntry>();
    foreach (KeyValuePair<string, CodexEntry> categoryEntry in categoryEntries)
      categoryEntries1.Add(categoryEntry.Value as CategoryEntry);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries1, (Comparison<CodexEntry>) null);
  }

  public static void PopulateCategoryEntries(
    List<CategoryEntry> categoryEntries,
    Comparison<CodexEntry> comparison = null)
  {
    foreach (CategoryEntry categoryEntry in categoryEntries)
    {
      List<ContentContainer> contentContainers = categoryEntry.contentContainers;
      List<CodexEntry> codexEntryList = new List<CodexEntry>();
      foreach (CodexEntry codexEntry in categoryEntry.entriesInCategory)
        codexEntryList.Add(codexEntry);
      if (categoryEntry.sort)
      {
        if (comparison == null)
          codexEntryList.Sort((Comparison<CodexEntry>) ((a, b) => UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name))));
        else
          codexEntryList.Sort(comparison);
      }
      if (categoryEntry.largeFormat)
      {
        ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);
        foreach (CodexEntry codexEntry in codexEntryList)
          contentContainer.content.Add((ICodexWidget) new CodexLabelWithLargeIcon(codexEntry.name, CodexTextStyle.BodyWhite, new Tuple<Sprite, Color>(!((UnityEngine.Object) codexEntry.icon != (UnityEngine.Object) null) ? Assets.GetSprite((HashedString) "unknown") : codexEntry.icon, codexEntry.iconColor), codexEntry.id));
        contentContainers.Add(contentContainer);
      }
      else
      {
        ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Vertical);
        foreach (CodexEntry codexEntry in codexEntryList)
        {
          if ((UnityEngine.Object) codexEntry.icon == (UnityEngine.Object) null)
            contentContainer.content.Add((ICodexWidget) new CodexText(codexEntry.name, CodexTextStyle.Body));
          else
            contentContainer.content.Add((ICodexWidget) new CodexLabelWithIcon(codexEntry.name, CodexTextStyle.Body, new Tuple<Sprite, Color>(codexEntry.icon, codexEntry.iconColor), 64, 48));
        }
        contentContainers.Add(contentContainer);
      }
    }
  }

  private static void GenerateTitleContainers(string name, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(name, CodexTextStyle.Title),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GeneratePrerequisiteTechContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    if (tech.requiredTech == null || tech.requiredTech.Count == 0)
      return;
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_TECH, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Tech tech1 in tech.requiredTech)
      content.Add((ICodexWidget) new CodexText(tech1.Name, CodexTextStyle.Body));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateSkillRequirementsAndPerksContainers(
    Skill skill,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    CodexText codexText1 = new CodexText((string) CODEX.HEADERS.ROLE_PERKS, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText((string) CODEX.HEADERS.ROLE_PERKS_DESC, CodexTextStyle.Body);
    content.Add((ICodexWidget) codexText1);
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) codexText2);
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Resource perk in skill.perks)
    {
      CodexText codexText3 = new CodexText(perk.Name, CodexTextStyle.Body);
      content.Add((ICodexWidget) codexText3);
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
    content.Add((ICodexWidget) new CodexSpacer());
  }

  private static void GenerateRelatedSkillContainers(Skill skill, List<ContentContainer> containers)
  {
    bool flag1 = false;
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    content1.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_ROLES, CodexTextStyle.Subtitle));
    content1.Add((ICodexWidget) new CodexDividerLine());
    content1.Add((ICodexWidget) new CodexSpacer());
    foreach (string priorSkill in skill.priorSkills)
    {
      CodexText codexText = new CodexText(Db.Get().Skills.Get(priorSkill).Name, CodexTextStyle.Body);
      content1.Add((ICodexWidget) codexText);
      flag1 = true;
    }
    if (flag1)
    {
      content1.Add((ICodexWidget) new CodexSpacer());
      containers.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    }
    bool flag2 = false;
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    CodexText codexText1 = new CodexText((string) CODEX.HEADERS.UNLOCK_ROLES, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText((string) CODEX.HEADERS.UNLOCK_ROLES_DESC, CodexTextStyle.Body);
    content2.Add((ICodexWidget) codexText1);
    content2.Add((ICodexWidget) new CodexDividerLine());
    content2.Add((ICodexWidget) codexText2);
    content2.Add((ICodexWidget) new CodexSpacer());
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      foreach (string priorSkill in resource.priorSkills)
      {
        if (priorSkill == skill.Id)
        {
          CodexText codexText3 = new CodexText(resource.Name, CodexTextStyle.Body);
          content2.Add((ICodexWidget) codexText3);
          flag2 = true;
        }
      }
    }
    if (!flag2)
      return;
    content2.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateUnlockContainers(Tech tech, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.TECH_UNLOCKS, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine(),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (TechItem unlockedItem in tech.unlockedItems)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64, 64, unlockedItem.getUISprite("ui", false)),
        (ICodexWidget) new CodexText(unlockedItem.Name, CodexTextStyle.Body)
      }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateRecipeContainers(Tag prefabID, List<ContentContainer> containers)
  {
    Recipe recipe1 = (Recipe) null;
    foreach (Recipe recipe2 in RecipeManager.Get().recipes)
    {
      if (recipe2.Result == prefabID)
      {
        recipe1 = recipe2;
        break;
      }
    }
    if (recipe1 == null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.RECIPE, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    Func<Recipe, List<ContentContainer>> func = (Func<Recipe, List<ContentContainer>>) (rec =>
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      foreach (Recipe.Ingredient ingredient in rec.Ingredients)
      {
        GameObject prefab = Assets.GetPrefab(ingredient.tag);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) prefab, "ui", false)),
            (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.RECIPE_ITEM, (object) Assets.GetPrefab(ingredient.tag).GetProperName(), (object) ingredient.amount, ElementLoader.GetElement(ingredient.tag) != null ? (object) UI.UNITSUFFIXES.MASS.KILOGRAM.text : (object) string.Empty), CodexTextStyle.Body)
          }, ContentContainer.ContentLayout.Horizontal));
      }
      return contentContainerList;
    });
    containers.AddRange((IEnumerable<ContentContainer>) func(recipe1));
    GameObject go = recipe1.fabricators != null ? Assets.GetPrefab((Tag) recipe1.fabricators[0]) : (GameObject) null;
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) UI.CODEX.RECIPE_FABRICATOR_HEADER, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexImage(64, 64, Def.GetUISpriteFromMultiObjectAnim(go.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, string.Empty)),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.RECIPE_FABRICATOR, (object) recipe1.FabricationTime, (object) go.GetProperName()), CodexTextStyle.Body)
    }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateUsedInRecipeContainers(
    Tag prefabID,
    List<ContentContainer> containers)
  {
    List<Recipe> recipeList = new List<Recipe>();
    foreach (Recipe recipe in RecipeManager.Get().recipes)
    {
      foreach (Recipe.Ingredient ingredient in recipe.Ingredients)
      {
        if (ingredient.tag == prefabID)
          recipeList.Add(recipe);
      }
    }
    if (recipeList.Count == 0)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.USED_IN_RECIPES, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (Recipe recipe in recipeList)
    {
      GameObject prefab = Assets.GetPrefab(recipe.Result);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) prefab, "ui", false)),
        (ICodexWidget) new CodexText(prefab.GetProperName(), CodexTextStyle.Body)
      }, ContentContainer.ContentLayout.Horizontal));
    }
  }

  private static void GeneratePlantDescriptionContainers(
    GameObject plant,
    List<ContentContainer> containers)
  {
    SeedProducer component1 = plant.GetComponent<SeedProducer>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      GameObject prefab = Assets.GetPrefab((Tag) component1.seedInfo.seedId);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.GROWNFROMSEED, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine()
      }, ContentContainer.ContentLayout.Vertical));
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(48, 48, Def.GetUISprite((object) prefab, "ui", false)),
        (ICodexWidget) new CodexText(prefab.GetProperName(), CodexTextStyle.Body)
      }, ContentContainer.ContentLayout.Horizontal));
    }
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    content.Add((ICodexWidget) new CodexText((string) UI.CODEX.DETAILS, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    InfoDescription component2 = Assets.GetPrefab(plant.PrefabID()).GetComponent<InfoDescription>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      content.Add((ICodexWidget) new CodexText(component2.description, CodexTextStyle.Body));
    string empty1 = string.Empty;
    List<Descriptor> requirementDescriptors = GameUtil.GetPlantRequirementDescriptors(plant);
    if (requirementDescriptors.Count > 0)
    {
      string text = empty1 + requirementDescriptors[0].text;
      for (int index = 1; index < requirementDescriptors.Count; ++index)
        text = text + "\n    • " + requirementDescriptors[index].text;
      content.Add((ICodexWidget) new CodexText(text, CodexTextStyle.Body));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    string empty2 = string.Empty;
    List<Descriptor> effectDescriptors = GameUtil.GetPlantEffectDescriptors(plant);
    if (effectDescriptors.Count > 0)
    {
      string text = empty2 + effectDescriptors[0].text;
      for (int index = 1; index < effectDescriptors.Count; ++index)
        text = text + "\n    • " + effectDescriptors[index].text;
      CodexText codexText = new CodexText(text, CodexTextStyle.Body);
      content.Add((ICodexWidget) codexText);
      content.Add((ICodexWidget) new CodexSpacer());
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static ICodexWidget GetIconWidget(object entity)
  {
    return (ICodexWidget) new CodexImage(32, 32, Def.GetUISprite(entity, "ui", false));
  }

  private static void GenerateCreatureDescriptionContainers(
    GameObject creature,
    List<ContentContainer> containers)
  {
    CreatureCalorieMonitor.Def def = creature.GetDef<CreatureCalorieMonitor.Def>();
    if (def == null)
      return;
    List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag((creature.PrefabID().ToString() + "Egg").ToTag());
    if (prefabsWithTag != null && prefabsWithTag.Count > 0)
    {
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.HATCHESFROMEGG, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine()
      }, ContentContainer.ContentLayout.Vertical));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          CodexEntryGenerator.GetIconWidget((object) go),
          (ICodexWidget) new CodexText(go.GetProperName(), CodexTextStyle.Body)
        }, ContentContainer.ContentLayout.Horizontal));
    }
    TemperatureVulnerable component = creature.GetComponent<TemperatureVulnerable>();
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.COMFORTRANGE, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine(),
      (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.COMFORT_RANGE, (object) (GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + " - " + GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))), CodexTextStyle.Body),
      (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.NON_LETHAL_RANGE, (object) (GameUtil.GetFormattedTemperature(component.internalTemperatureLethal_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + " - " + GameUtil.GetFormattedTemperature(component.internalTemperatureLethal_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))), CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
    List<Tag> tagList1 = new List<Tag>();
    if (def.diet.infos.Length > 0)
    {
      if (tagList1.Count == 0)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.DIET, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexDividerLine()
        }, ContentContainer.ContentLayout.Vertical));
      ContentContainer contentContainer = new ContentContainer();
      contentContainer.contentLayout = ContentContainer.ContentLayout.Vertical;
      contentContainer.content = new List<ICodexWidget>();
      foreach (Diet.Info info in def.diet.infos)
      {
        if (info.consumedTags.Count != 0)
        {
          foreach (Tag consumedTag in info.consumedTags)
          {
            Element elementByHash = ElementLoader.FindElementByHash(ElementLoader.GetElementID(consumedTag));
            GameObject go = (GameObject) null;
            if (elementByHash.id == SimHashes.Vacuum || elementByHash.id == SimHashes.Void)
            {
              go = Assets.GetPrefab(consumedTag);
              if ((UnityEngine.Object) go == (UnityEngine.Object) null)
                continue;
            }
            if (elementByHash != null && (UnityEngine.Object) go == (UnityEngine.Object) null)
            {
              if (!tagList1.Contains(elementByHash.tag))
              {
                tagList1.Add(elementByHash.tag);
                contentContainer.content.Add((ICodexWidget) new CodexLabelWithIcon("    " + elementByHash.name, CodexTextStyle.Body, Def.GetUISprite((object) elementByHash.substance, "ui", false)));
              }
            }
            else if ((UnityEngine.Object) go != (UnityEngine.Object) null && !tagList1.Contains(go.PrefabID()))
            {
              tagList1.Add(go.PrefabID());
              contentContainer.content.Add((ICodexWidget) new CodexLabelWithIcon("    " + go.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) go, "ui", false)));
            }
          }
        }
      }
      containers.Add(contentContainer);
    }
    bool flag = false;
    if (def.diet == null)
      return;
    foreach (Diet.Info info in def.diet.infos)
    {
      if (info.producedElement != (Tag) ((string) null))
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    ContentContainer contentContainer1 = new ContentContainer();
    contentContainer1.contentLayout = ContentContainer.ContentLayout.Vertical;
    contentContainer1.content = new List<ICodexWidget>();
    ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.PRODUCES, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical);
    containers.Add(contentContainer2);
    List<Tag> tagList2 = new List<Tag>();
    for (int index = 0; index < def.diet.infos.Length; ++index)
    {
      if (def.diet.infos[index].producedElement != Tag.Invalid && !tagList2.Contains(def.diet.infos[index].producedElement))
      {
        tagList2.Add(def.diet.infos[index].producedElement);
        contentContainer1.content.Add((ICodexWidget) new CodexLabelWithIcon("• " + def.diet.infos[index].producedElement.ProperName(), CodexTextStyle.Body, Def.GetUISprite((object) def.diet.infos[index].producedElement, "ui", false)));
      }
    }
    containers.Add(contentContainer1);
  }

  private static void GenerateDiseaseDescriptionContainers(
    Klei.AI.Disease disease,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Descriptor quantitativeDescriptor in disease.GetQuantitativeDescriptors())
      content.Add((ICodexWidget) new CodexText(quantitativeDescriptor.text, CodexTextStyle.Body));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateFoodDescriptionContainers(
    EdiblesManager.FoodInfo food,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(food.Description, CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.FOOD.QUALITY, (object) GameUtil.GetFormattedFoodQuality(food.Quality)), CodexTextStyle.Body),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.FOOD.CALORIES, (object) GameUtil.GetFormattedCalories(food.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText(!food.CanRot ? UI.CODEX.FOOD.NON_PERISHABLE.ToString() : string.Format((string) UI.CODEX.FOOD.SPOILPROPERTIES, (object) GameUtil.GetFormattedTemperature(food.PreserveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedCycles(food.SpoilTime, "F1")), CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateTechDescriptionContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.RESEARCH.TECHS." + tech.Id.ToUpper() + ".DESC"), CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateGenericDescriptionContainers(
    string description,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(description, CodexTextStyle.Body),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateBuildingDescriptionContainers(
    BuildingDef def,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".EFFECT"), CodexTextStyle.Body));
    content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".DESC"), CodexTextStyle.Body));
    Tech tech = Db.Get().TechItems.LookupGroupForID(def.PrefabID);
    if (tech != null)
      content.Add((ICodexWidget) new CodexText(string.Format((string) UI.PRODUCTINFO_REQUIRESRESEARCHDESC, (object) tech.Name), CodexTextStyle.Body));
    content.Add((ICodexWidget) new CodexSpacer());
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def);
    List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
    if (effectDescriptors.Count > 0)
    {
      content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGEFFECTS, CodexTextStyle.Subtitle));
      content.Add((ICodexWidget) new CodexDividerLine());
      foreach (Descriptor descriptor in effectDescriptors)
        content.Add((ICodexWidget) new CodexText(descriptor.text, CodexTextStyle.Body));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
    if (requirementDescriptors.Count > 0)
    {
      content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGREQUIREMENTS, CodexTextStyle.Subtitle));
      content.Add((ICodexWidget) new CodexDividerLine());
      foreach (Descriptor descriptor in requirementDescriptors)
        content.Add((ICodexWidget) new CodexText(descriptor.text, CodexTextStyle.Body));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateImageContainers(
    Sprite[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Sprite sprite in sprites)
    {
      if (!((UnityEngine.Object) sprite == (UnityEngine.Object) null))
      {
        CodexImage codexImage = new CodexImage(128, 128, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  private static void GenerateImageContainers(
    Tuple<Sprite, Color>[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Tuple<Sprite, Color> sprite in sprites)
    {
      if (sprite != null)
      {
        CodexImage codexImage = new CodexImage(128, 128, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  private static void GenerateImageContainers(Sprite sprite, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexImage(128, 128, sprite)
    }, ContentContainer.ContentLayout.Vertical));
  }

  public static void CreateUnlockablesContentContainer(SubEntry subentry)
  {
    subentry.lockedContentContainer = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.SECTION_UNLOCKABLES, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical)
    {
      showBeforeGeneratedContent = false
    };
  }

  private static void GenerateFabricatorContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    ComplexFabricator component = entity.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (ComplexRecipe recipe in component.GetRecipes())
    {
      GameObject prefab = Assets.GetPrefab(recipe.results[0].material);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) prefab, "ui", false)),
        (ICodexWidget) new CodexText(prefab.GetProperName(), CodexTextStyle.Body)
      }, ContentContainer.ContentLayout.Horizontal));
    }
  }

  private static void GenerateReceptacleContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    SingleEntityReceptacle plot = entity.GetComponent<SingleEntityReceptacle>();
    if ((UnityEngine.Object) plot == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.RECEPTACLE"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (Tag depositObjectTag in plot.possibleDepositObjectTags)
    {
      List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(depositObjectTag);
      if ((UnityEngine.Object) plot.rotatable == (UnityEngine.Object) null)
        prefabsWithTag.RemoveAll((Predicate<GameObject>) (go =>
        {
          IReceptacleDirection component = go.GetComponent<IReceptacleDirection>();
          if (component != null)
            return component.Direction != plot.Direction;
          return false;
        }));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) go, "ui", false).first),
          (ICodexWidget) new CodexText(go.GetProperName(), CodexTextStyle.Body)
        }, ContentContainer.ContentLayout.Horizontal));
    }
  }
}
