// Decompiled with JetBrains decompiler
// Type: SaveLoadRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[SkipSaveFileSerialization]
public class SaveLoadRoot : KMonoBehaviour
{
  private bool registered = true;
  private bool hasOnSpawnRun;
  private static Dictionary<string, ISerializableComponentManager> serializableComponentManagers;

  public static void DestroyStatics()
  {
    SaveLoadRoot.serializableComponentManagers = (Dictionary<string, ISerializableComponentManager>) null;
  }

  protected override void OnPrefabInit()
  {
    if (SaveLoadRoot.serializableComponentManagers != null)
      return;
    SaveLoadRoot.serializableComponentManagers = new Dictionary<string, ISerializableComponentManager>();
    foreach (FieldInfo field in typeof (GameComps).GetFields())
    {
      IComponentManager componentManager = (IComponentManager) field.GetValue((object) null);
      if (typeof (ISerializableComponentManager).IsAssignableFrom(componentManager.GetType()))
      {
        System.Type type = componentManager.GetType();
        SaveLoadRoot.serializableComponentManagers[type.ToString()] = (ISerializableComponentManager) componentManager;
      }
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.registered)
      SaveLoader.Instance.saveManager.Register(this);
    this.hasOnSpawnRun = true;
  }

  public void SetRegistered(bool registered)
  {
    if (this.registered == registered)
      return;
    this.registered = registered;
    if (!this.hasOnSpawnRun)
      return;
    if (registered)
      SaveLoader.Instance.saveManager.Register(this);
    else
      SaveLoader.Instance.saveManager.Unregister(this);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SaveLoader.Instance.saveManager != (UnityEngine.Object) null)
      SaveLoader.Instance.saveManager.Unregister(this);
    if (!GameComps.WhiteBoards.Has((object) this.gameObject))
      return;
    GameComps.WhiteBoards.Remove(this.gameObject);
  }

  public void Save(BinaryWriter writer)
  {
    Transform transform = this.transform;
    writer.Write(transform.GetPosition());
    writer.Write(transform.rotation);
    writer.Write(transform.localScale);
    byte num = 0;
    writer.Write(num);
    this.SaveWithoutTransform(writer);
  }

  public void SaveWithoutTransform(BinaryWriter writer)
  {
    KMonoBehaviour[] components = this.GetComponents<KMonoBehaviour>();
    if (components == null)
      return;
    int num1 = 0;
    foreach (KMonoBehaviour kmonoBehaviour in components)
    {
      if ((kmonoBehaviour is ISaveLoadableDetails || kmonoBehaviour != null) && !kmonoBehaviour.GetType().IsDefined(typeof (SkipSaveFileSerialization), false))
        ++num1;
    }
    foreach (KeyValuePair<string, ISerializableComponentManager> componentManager in SaveLoadRoot.serializableComponentManagers)
    {
      if (componentManager.Value.Has((object) this.gameObject))
        ++num1;
    }
    writer.Write(num1);
    foreach (KMonoBehaviour kmonoBehaviour in components)
    {
      if ((kmonoBehaviour is ISaveLoadableDetails || kmonoBehaviour != null) && !kmonoBehaviour.GetType().IsDefined(typeof (SkipSaveFileSerialization), false))
      {
        writer.WriteKleiString(kmonoBehaviour.GetType().ToString());
        long position1 = writer.BaseStream.Position;
        writer.Write(0);
        long position2 = writer.BaseStream.Position;
        if (kmonoBehaviour is ISaveLoadableDetails)
        {
          ISaveLoadableDetails saveLoadableDetails = (ISaveLoadableDetails) kmonoBehaviour;
          Serializer.SerializeTypeless((object) kmonoBehaviour, writer);
          saveLoadableDetails.Serialize(writer);
        }
        else if (kmonoBehaviour != null)
          Serializer.SerializeTypeless((object) kmonoBehaviour, writer);
        long position3 = writer.BaseStream.Position;
        long num2 = position3 - position2;
        writer.BaseStream.Position = position1;
        writer.Write((int) num2);
        writer.BaseStream.Position = position3;
      }
    }
    foreach (KeyValuePair<string, ISerializableComponentManager> componentManager1 in SaveLoadRoot.serializableComponentManagers)
    {
      ISerializableComponentManager componentManager2 = componentManager1.Value;
      if (componentManager2.Has((object) this.gameObject))
      {
        string key = componentManager1.Key;
        writer.WriteKleiString(key);
        componentManager2.Serialize(this.gameObject, writer);
      }
    }
  }

  public static SaveLoadRoot Load(Tag tag, IReader reader)
  {
    return SaveLoadRoot.Load(SaveLoader.Instance.saveManager.GetPrefab(tag), reader);
  }

  public static SaveLoadRoot Load(GameObject prefab, IReader reader)
  {
    Vector3 position = reader.ReadVector3();
    Quaternion rotation = reader.ReadQuaternion();
    Vector3 scale = reader.ReadVector3();
    int num = (int) reader.ReadByte();
    return SaveLoadRoot.Load(prefab, position, rotation, scale, reader);
  }

  public static SaveLoadRoot Load(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation,
    Vector3 scale,
    IReader reader)
  {
    SaveLoadRoot saveLoadRoot = (SaveLoadRoot) null;
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      GameObject gameObject = Util.KInstantiate(prefab, position, rotation, (GameObject) null, (string) null, false, 0);
      gameObject.transform.localScale = scale;
      gameObject.SetActive(true);
      saveLoadRoot = gameObject.GetComponent<SaveLoadRoot>();
      if ((UnityEngine.Object) saveLoadRoot != (UnityEngine.Object) null)
      {
        try
        {
          SaveLoadRoot.LoadInternal(gameObject, reader);
        }
        catch (ArgumentException ex)
        {
          DebugUtil.LogErrorArgs((UnityEngine.Object) gameObject, (object) "Failed to load SaveLoadRoot ", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        }
      }
      else
        Debug.Log((object) "missing SaveLoadRoot", (UnityEngine.Object) gameObject);
    }
    else
      SaveLoadRoot.LoadInternal((GameObject) null, reader);
    return saveLoadRoot;
  }

  private static void LoadInternal(GameObject gameObject, IReader reader)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    KMonoBehaviour[] kmonoBehaviourArray = !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) ? (KMonoBehaviour[]) null : gameObject.GetComponents<KMonoBehaviour>();
    int num1 = reader.ReadInt32();
    for (int index1 = 0; index1 < num1; ++index1)
    {
      string key = reader.ReadKleiString();
      int length = reader.ReadInt32();
      int position = reader.Position;
      ISerializableComponentManager componentManager;
      if (SaveLoadRoot.serializableComponentManagers.TryGetValue(key, out componentManager))
      {
        componentManager.Deserialize(gameObject, reader);
      }
      else
      {
        int num2 = 0;
        dictionary.TryGetValue(key, out num2);
        KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) null;
        int num3 = 0;
        if (kmonoBehaviourArray != null)
        {
          for (int index2 = 0; index2 < kmonoBehaviourArray.Length; ++index2)
          {
            if (kmonoBehaviourArray[index2].GetType().ToString() == key)
            {
              if (num3 == num2)
              {
                kmonoBehaviour = kmonoBehaviourArray[index2];
                break;
              }
              ++num3;
            }
          }
        }
        if ((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null)
          reader.SkipBytes(length);
        else if (kmonoBehaviour == null && !(kmonoBehaviour is ISaveLoadableDetails))
        {
          DebugUtil.LogErrorArgs((object) "Component", (object) key, (object) "is not ISaveLoadable");
          reader.SkipBytes(length);
        }
        else
        {
          dictionary[key] = num3 + 1;
          if (kmonoBehaviour is ISaveLoadableDetails)
          {
            ISaveLoadableDetails saveLoadableDetails = (ISaveLoadableDetails) kmonoBehaviour;
            Deserializer.DeserializeTypeless((object) kmonoBehaviour, reader);
            saveLoadableDetails.Deserialize(reader);
          }
          else
            Deserializer.DeserializeTypeless((object) kmonoBehaviour, reader);
          if (reader.Position != position + length)
          {
            DebugUtil.LogWarningArgs((object) "Expected to be at offset", (object) (position + length), (object) "but was only at offset", (object) reader.Position, (object) ". Skipping to catch up.");
            reader.SkipBytes(position + length - reader.Position);
          }
        }
      }
    }
  }
}
