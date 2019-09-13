// Decompiled with JetBrains decompiler
// Type: TemplateCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class TemplateCache
{
  private static string baseTemplatePath;
  private static Dictionary<string, TemplateContainer> templates;
  private const string defaultAssetFolder = "bases";

  public static void Init()
  {
    TemplateCache.templates = new Dictionary<string, TemplateContainer>();
    TemplateCache.baseTemplatePath = FileSystem.Normalize(System.IO.Path.Combine(Application.streamingAssetsPath, "templates"));
  }

  public static void Clear()
  {
    TemplateCache.templates = (Dictionary<string, TemplateContainer>) null;
    TemplateCache.baseTemplatePath = (string) null;
  }

  public static string GetTemplatePath()
  {
    return TemplateCache.baseTemplatePath;
  }

  public static TemplateContainer GetStartingBaseTemplate(
    string startingTemplateName)
  {
    DebugUtil.Assert(startingTemplateName != null, "Tried loading a starting template named ", startingTemplateName);
    if (TemplateCache.baseTemplatePath == null)
      TemplateCache.Init();
    return TemplateCache.GetTemplate(System.IO.Path.Combine("bases", startingTemplateName));
  }

  public static TemplateContainer GetTemplate(string templatePath)
  {
    if (!TemplateCache.templates.ContainsKey(templatePath))
      TemplateCache.templates.Add(templatePath, (TemplateContainer) null);
    if (TemplateCache.templates[templatePath] == null)
    {
      string str = FileSystem.Normalize(System.IO.Path.Combine(TemplateCache.baseTemplatePath, templatePath));
      TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(str + ".yaml", (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null);
      if (templateContainer == null)
        Debug.LogWarning((object) ("Missing template [" + str + ".yaml]"));
      TemplateCache.templates[templatePath] = templateContainer;
    }
    return TemplateCache.templates[templatePath];
  }

  private static void GetAssetPaths(string folder, List<string> paths)
  {
    FileSystem.GetFiles(FileSystem.Normalize(System.IO.Path.Combine(TemplateCache.baseTemplatePath, folder)), "*.yaml", (ICollection<string>) paths);
  }

  public static List<string> CollectBaseTemplateNames(string folder = "bases")
  {
    List<string> stringList = new List<string>();
    ListPool<string, TemplateContainer>.PooledList pooledList = ListPool<string, TemplateContainer>.Allocate();
    TemplateCache.GetAssetPaths(folder, (List<string>) pooledList);
    foreach (string path in (List<string>) pooledList)
    {
      string key = FileSystem.Normalize(System.IO.Path.Combine(folder, System.IO.Path.GetFileNameWithoutExtension(path)));
      stringList.Add(key);
      if (!TemplateCache.templates.ContainsKey(key))
        TemplateCache.templates.Add(key, (TemplateContainer) null);
    }
    pooledList.Recycle();
    stringList.Sort((Comparison<string>) ((x, y) => x.CompareTo(y)));
    return stringList;
  }

  public static List<TemplateContainer> CollectBaseTemplateAssets(string folder = "bases")
  {
    List<TemplateContainer> templateContainerList = new List<TemplateContainer>();
    ListPool<string, TemplateContainer>.PooledList pooledList = ListPool<string, TemplateContainer>.Allocate();
    TemplateCache.GetAssetPaths(folder, (List<string>) pooledList);
    foreach (string filename in (List<string>) pooledList)
      templateContainerList.Add(YamlIO.LoadFile<TemplateContainer>(filename, (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null));
    pooledList.Recycle();
    templateContainerList.Sort((Comparison<TemplateContainer>) ((x, y) =>
    {
      if (y.priority - x.priority == 0)
        return x.name.CompareTo(y.name);
      return y.priority - x.priority;
    }));
    return templateContainerList;
  }
}
