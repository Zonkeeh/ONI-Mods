// Decompiled with JetBrains decompiler
// Type: KMod.DLLLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace KMod
{
  internal static class DLLLoader
  {
    private const string managed_path = "Managed";

    public static bool LoadUserModLoaderDLL()
    {
      try
      {
        string path = System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "Managed"), "ModLoader.dll");
        if (!File.Exists(path))
          return false;
        Assembly assembly = Assembly.LoadFile(path);
        if (assembly == null)
          return false;
        System.Type type = assembly.GetType("ModLoader.ModLoader");
        if (type == null)
          return false;
        MethodInfo method = type.GetMethod("Start");
        if (method == null)
          return false;
        method.Invoke((object) null, (object[]) null);
        Debug.Log((object) "Successfully started ModLoader.dll");
        return true;
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.ToString());
      }
      return false;
    }

    public static bool LoadDLLs(string path)
    {
      try
      {
        if (Testing.dll_loading == Testing.DLLLoading.Fail || Testing.dll_loading == Testing.DLLLoading.UseModLoaderDLLExclusively)
          return false;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
          return false;
        List<Assembly> assemblyList = new List<Assembly>();
        foreach (FileInfo file in directoryInfo.GetFiles())
        {
          if (file.Name.ToLower().EndsWith(".dll"))
          {
            Debug.Log((object) string.Format("Loading MOD dll: {0}", (object) file.Name));
            Assembly assembly = Assembly.LoadFrom(file.FullName);
            if (assembly != null)
              assemblyList.Add(assembly);
          }
        }
        if (assemblyList.Count == 0)
          return false;
        ListPool<MethodInfo, Manager>.PooledList pooledList1 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList2 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList3 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList4 = ListPool<MethodInfo, Manager>.Allocate();
        System.Type[] types1 = new System.Type[0];
        System.Type[] types2 = new System.Type[1]
        {
          typeof (string)
        };
        System.Type[] types3 = new System.Type[1]
        {
          typeof (HarmonyInstance)
        };
        foreach (Assembly assembly in assemblyList)
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (type != null)
            {
              MethodInfo method1 = type.GetMethod("OnLoad", types1);
              if (method1 != null)
                pooledList3.Add(method1);
              MethodInfo method2 = type.GetMethod("OnLoad", types2);
              if (method2 != null)
                pooledList4.Add(method2);
              MethodInfo method3 = type.GetMethod("PrePatch", types3);
              if (method3 != null)
                pooledList1.Add(method3);
              MethodInfo method4 = type.GetMethod("PostPatch", types3);
              if (method4 != null)
                pooledList2.Add(method4);
            }
          }
        }
        HarmonyInstance harmonyInstance = HarmonyInstance.Create(string.Format("OxygenNotIncluded_v{0}.{1}", (object) 0, (object) 1));
        if (harmonyInstance != null)
        {
          object[] parameters = new object[1]
          {
            (object) harmonyInstance
          };
          foreach (MethodBase methodBase in (List<MethodInfo>) pooledList1)
            methodBase.Invoke((object) null, parameters);
          foreach (Assembly assembly in assemblyList)
            harmonyInstance.PatchAll(assembly);
          foreach (MethodBase methodBase in (List<MethodInfo>) pooledList2)
            methodBase.Invoke((object) null, parameters);
        }
        pooledList1.Recycle();
        pooledList2.Recycle();
        foreach (MethodBase methodBase in (List<MethodInfo>) pooledList3)
          methodBase.Invoke((object) null, (object[]) null);
        object[] parameters1 = new object[1]
        {
          (object) path
        };
        foreach (MethodBase methodBase in (List<MethodInfo>) pooledList4)
          methodBase.Invoke((object) null, parameters1);
        pooledList3.Recycle();
        pooledList4.Recycle();
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
