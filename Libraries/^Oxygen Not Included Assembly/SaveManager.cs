// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : KMonoBehaviour
{
  private static readonly char[] SAVE_HEADER = new char[4]
  {
    'K',
    'S',
    'A',
    'V'
  };
  private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();
  private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();
  private List<Tag> orderedKeys = new List<Tag>();
  public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;
  public const int SAVE_MAJOR_VERSION = 7;
  public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;
  public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;
  public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;
  public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;
  public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;
  public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;
  public const int SAVE_MINOR_VERSION = 11;

  public event System.Action<SaveLoadRoot> onRegister;

  public event System.Action<SaveLoadRoot> onUnregister;

  protected override void OnPrefabInit()
  {
    Assets.RegisterOnAddPrefab(new System.Action<KPrefabID>(this.OnAddPrefab));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Assets.UnregisterOnAddPrefab(new System.Action<KPrefabID>(this.OnAddPrefab));
  }

  private void OnAddPrefab(KPrefabID prefab)
  {
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return;
    this.prefabMap[prefab.GetSaveLoadTag()] = prefab.gameObject;
  }

  public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
  {
    return this.sceneObjects;
  }

  private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
  {
    KPrefabID component = saver.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      DebugUtil.LogErrorArgs((UnityEngine.Object) saver.gameObject, (object) "All savers must also have a KPrefabID on them but", (object) saver.gameObject.name, (object) "does not have one.");
      return (List<SaveLoadRoot>) null;
    }
    List<SaveLoadRoot> saveLoadRootList;
    if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out saveLoadRootList))
    {
      saveLoadRootList = new List<SaveLoadRoot>();
      this.sceneObjects[component.GetSaveLoadTag()] = saveLoadRootList;
    }
    return saveLoadRootList;
  }

  public void Register(SaveLoadRoot root)
  {
    List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
    if (saveLoadRootList == null)
      return;
    saveLoadRootList.Add(root);
    if (this.onRegister == null)
      return;
    this.onRegister(root);
  }

  public void Unregister(SaveLoadRoot root)
  {
    if (this.onRegister != null)
      this.onUnregister(root);
    this.GetSaveLoadRootList(root)?.Remove(root);
  }

  public GameObject GetPrefab(Tag tag)
  {
    GameObject gameObject = (GameObject) null;
    if (this.prefabMap.TryGetValue(tag, out gameObject))
      return gameObject;
    DebugUtil.LogArgs((object) "Item not found in prefabMap", (object) ("[" + tag.Name + "]"));
    return (GameObject) null;
  }

  public void Save(BinaryWriter writer)
  {
    writer.Write(SaveManager.SAVE_HEADER);
    writer.Write(7);
    writer.Write(11);
    int num = 0;
    foreach (KeyValuePair<Tag, List<SaveLoadRoot>> sceneObject in this.sceneObjects)
    {
      if (sceneObject.Value.Count > 0)
        ++num;
    }
    writer.Write(num);
    this.orderedKeys.Clear();
    this.orderedKeys.AddRange((IEnumerable<Tag>) this.sceneObjects.Keys);
    this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
    this.orderedKeys = this.orderedKeys.OrderBy<Tag, bool>((Func<Tag, bool>) (a => a.Name.Contains("UnderConstruction"))).ToList<Tag>();
    this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>((IEnumerable<SaveLoadRoot>) new SaveLoadRoot[1]
    {
      SaveGame.Instance.GetComponent<SaveLoadRoot>()
    }), writer);
    foreach (Tag orderedKey in this.orderedKeys)
    {
      List<SaveLoadRoot> sceneObject = this.sceneObjects[orderedKey];
      if (sceneObject.Count > 0)
      {
        foreach (SaveLoadRoot saveLoadRoot in sceneObject)
        {
          if (!((UnityEngine.Object) saveLoadRoot == (UnityEngine.Object) null) && (UnityEngine.Object) saveLoadRoot.GetComponent<SimCellOccupier>() != (UnityEngine.Object) null)
          {
            this.Write(orderedKey, sceneObject, writer);
            break;
          }
        }
      }
    }
    foreach (Tag orderedKey in this.orderedKeys)
    {
      List<SaveLoadRoot> sceneObject = this.sceneObjects[orderedKey];
      if (sceneObject.Count > 0)
      {
        foreach (SaveLoadRoot saveLoadRoot in sceneObject)
        {
          if (!((UnityEngine.Object) saveLoadRoot == (UnityEngine.Object) null) && (UnityEngine.Object) saveLoadRoot.GetComponent<SimCellOccupier>() == (UnityEngine.Object) null)
          {
            this.Write(orderedKey, sceneObject, writer);
            break;
          }
        }
      }
    }
  }

  private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
  {
    int count = value.Count;
    Tag tag = key;
    writer.WriteKleiString(tag.Name);
    writer.Write(count);
    long position1 = writer.BaseStream.Position;
    int num1 = -1;
    writer.Write(num1);
    long position2 = writer.BaseStream.Position;
    foreach (SaveLoadRoot saveLoadRoot in value)
    {
      if ((UnityEngine.Object) saveLoadRoot != (UnityEngine.Object) null)
        saveLoadRoot.Save(writer);
      else
        DebugUtil.LogWarningArgs((object) "Null game object when saving");
    }
    long position3 = writer.BaseStream.Position;
    long num2 = position3 - position2;
    writer.BaseStream.Position = position1;
    writer.Write((int) num2);
    writer.BaseStream.Position = position3;
  }

  public bool Load(IReader reader)
  {
    char[] chArray = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
    if (chArray == null || chArray.Length != SaveManager.SAVE_HEADER.Length)
      return false;
    for (int index = 0; index < SaveManager.SAVE_HEADER.Length; ++index)
    {
      if ((int) chArray[index] != (int) SaveManager.SAVE_HEADER[index])
        return false;
    }
    int num1 = reader.ReadInt32();
    int num2 = reader.ReadInt32();
    if (num1 != 7 || num2 > 11)
    {
      DebugUtil.LogWarningArgs((object) string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", (object) 7, (object) 11, (object) num1, (object) num2));
      return false;
    }
    this.ClearScene();
    try
    {
      int num3 = reader.ReadInt32();
      for (int index1 = 0; index1 < num3; ++index1)
      {
        string tag_string = reader.ReadKleiString();
        int capacity = reader.ReadInt32();
        int length = reader.ReadInt32();
        Tag key = TagManager.Create(tag_string);
        GameObject prefab;
        if (!this.prefabMap.TryGetValue(key, out prefab))
        {
          DebugUtil.LogWarningArgs((object) ("Could not find prefab '" + tag_string + "'"));
          reader.SkipBytes(length);
        }
        else
        {
          List<SaveLoadRoot> saveLoadRootList = new List<SaveLoadRoot>(capacity);
          this.sceneObjects[key] = saveLoadRootList;
          for (int index2 = 0; index2 < capacity; ++index2)
          {
            if ((UnityEngine.Object) SaveLoadRoot.Load(prefab, reader) == (UnityEngine.Object) null)
            {
              Debug.LogError((object) ("Error loading data [" + tag_string + "]"));
              return false;
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((object) "Error deserializing prefabs\n\n", (object) ex.ToString());
      throw ex;
    }
    return true;
  }

  private void ClearScene()
  {
    foreach (KeyValuePair<Tag, List<SaveLoadRoot>> sceneObject in this.sceneObjects)
    {
      foreach (Component component in sceneObject.Value)
        UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    }
    this.sceneObjects.Clear();
  }

  private enum BoundaryTag : uint
  {
    Prefab = 3131961357, // 0xBAADF00D
    Component = 3735928559, // 0xDEADBEEF
    Complete = 3735929054, // 0xDEADC0DE
  }
}
