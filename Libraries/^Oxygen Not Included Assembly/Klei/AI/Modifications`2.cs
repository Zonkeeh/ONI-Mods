// Decompiled with JetBrains decompiler
// Type: Klei.AI.Modifications`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails
    where ModifierType : Resource
    where InstanceType : ModifierInstance<ModifierType>
  {
    public List<InstanceType> ModifierList = new List<InstanceType>();
    private ResourceSet<ModifierType> resources;

    public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
    {
      this.resources = resources;
      this.gameObject = go;
    }

    public int Count
    {
      get
      {
        return this.ModifierList.Count;
      }
    }

    public IEnumerator<InstanceType> GetEnumerator()
    {
      return (IEnumerator<InstanceType>) this.ModifierList.GetEnumerator();
    }

    public GameObject gameObject { get; private set; }

    public InstanceType this[int idx]
    {
      get
      {
        return this.ModifierList[idx];
      }
    }

    public ComponentType GetComponent<ComponentType>()
    {
      return this.gameObject.GetComponent<ComponentType>();
    }

    public void Trigger(GameHashes hash, object data = null)
    {
      this.gameObject.GetComponent<KPrefabID>().Trigger((int) hash, data);
    }

    public virtual InstanceType CreateInstance(ModifierType modifier)
    {
      return (InstanceType) null;
    }

    public virtual InstanceType Add(InstanceType instance)
    {
      this.ModifierList.Add(instance);
      return instance;
    }

    public virtual void Remove(InstanceType instance)
    {
      for (int index = 0; index < this.ModifierList.Count; ++index)
      {
        if ((object) this.ModifierList[index] == (object) instance)
        {
          this.ModifierList.RemoveAt(index);
          instance.OnCleanUp();
          break;
        }
      }
    }

    public bool Has(ModifierType modifier)
    {
      return (object) this.Get(modifier) != null;
    }

    public InstanceType Get(ModifierType modifier)
    {
      foreach (InstanceType modifier1 in this.ModifierList)
      {
        if ((object) modifier1.modifier == (object) modifier)
          return modifier1;
      }
      return (InstanceType) null;
    }

    public InstanceType Get(string id)
    {
      foreach (InstanceType modifier in this.ModifierList)
      {
        if (modifier.modifier.Id == id)
          return modifier;
      }
      return (InstanceType) null;
    }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write(this.ModifierList.Count);
      foreach (InstanceType modifier in this.ModifierList)
      {
        writer.WriteKleiString(modifier.modifier.Id);
        long position1 = writer.BaseStream.Position;
        writer.Write(0);
        long position2 = writer.BaseStream.Position;
        Serializer.SerializeTypeless((object) modifier, writer);
        long position3 = writer.BaseStream.Position;
        long num = position3 - position2;
        writer.BaseStream.Position = position1;
        writer.Write((int) num);
        writer.BaseStream.Position = position3;
      }
    }

    public void Deserialize(IReader reader)
    {
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        string id = reader.ReadKleiString();
        int length = reader.ReadInt32();
        int position = reader.Position;
        InstanceType instance = this.Get(id);
        if ((object) instance == null && this.resources != null)
        {
          ModifierType modifier = this.resources.TryGet(id);
          if ((object) modifier != null)
            instance = this.CreateInstance(modifier);
        }
        if ((object) instance == null)
        {
          if (id != "Condition")
            DebugUtil.LogWarningArgs((object) this.gameObject.name, (object) ("Missing modifier: " + id));
          reader.SkipBytes(length);
        }
        else if (!((object) instance is ISaveLoadable))
        {
          reader.SkipBytes(length);
        }
        else
        {
          Deserializer.DeserializeTypeless((object) instance, reader);
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
