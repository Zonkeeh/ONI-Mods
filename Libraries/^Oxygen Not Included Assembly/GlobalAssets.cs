// Decompiled with JetBrains decompiler
// Type: GlobalAssets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAssets : MonoBehaviour
{
  private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();
  private static HashSet<string> LowPrioritySounds = new HashSet<string>();
  private static HashSet<string> HighPrioritySounds = new HashSet<string>();

  private void Awake()
  {
    if (GlobalAssets.SoundTable.Count == 0)
    {
      Bank[] array1 = (Bank[]) null;
      try
      {
        if (RuntimeManager.StudioSystem.getBankList(out array1) != RESULT.OK)
          array1 = (Bank[]) null;
      }
      catch
      {
        array1 = (Bank[]) null;
      }
      if (array1 != null)
      {
        foreach (Bank bank in array1)
        {
          EventDescription[] array2;
          RESULT eventList = bank.getEventList(out array2);
          string path1;
          if (eventList != RESULT.OK)
          {
            int path2 = (int) bank.getPath(out path1);
            Debug.LogError((object) string.Format("ERROR [{0}] loading FMOD events for bank [{1}]", (object) eventList, (object) path1));
          }
          else
          {
            for (int index = 0; index < array2.Length; ++index)
            {
              int path2 = (int) array2[index].getPath(out path1);
              string lowerInvariant = Assets.GetSimpleSoundEventName(path1).ToLowerInvariant();
              if (lowerInvariant.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(lowerInvariant))
              {
                GlobalAssets.SoundTable[lowerInvariant] = path1;
                if (path1.ToLower().Contains("lowpriority") || lowerInvariant.Contains("lowpriority"))
                  GlobalAssets.LowPrioritySounds.Add(path1);
                else if (path1.ToLower().Contains("highpriority") || lowerInvariant.Contains("highpriority"))
                  GlobalAssets.HighPrioritySounds.Add(path1);
              }
            }
          }
        }
      }
    }
    SetDefaults.Initialize();
    LocString.CreateLocStringKeys(typeof (DUPLICANTS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (MISC), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (UI), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (ELEMENTS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (CREATURES), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (SETITEMS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (RESEARCH), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (ITEMS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (INPUT), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (INPUT_BINDINGS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (BUILDING.DETAILS), "STRINGS.BUILDING.");
    LocString.CreateLocStringKeys(typeof (LORE), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (CODEX), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (WORLDS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (WORLD_TRAITS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (COLONY_ACHIEVEMENTS), "STRINGS.");
    LocString.CreateLocStringKeys(typeof (VIDEOS), "STRINGS.");
  }

  public static string GetSound(string name, bool force_no_warning = false)
  {
    if (name == null)
      return (string) null;
    name = name.ToLowerInvariant();
    string str = (string) null;
    GlobalAssets.SoundTable.TryGetValue(name, out str);
    return str;
  }

  public static bool IsLowPriority(string path)
  {
    return GlobalAssets.LowPrioritySounds.Contains(path);
  }

  public static bool IsHighPriority(string path)
  {
    return GlobalAssets.HighPrioritySounds.Contains(path);
  }
}
