// Decompiled with JetBrains decompiler
// Type: SolidConduitSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitSerializer : KMonoBehaviour, ISaveLoadableDetails
{
  protected override void OnPrefabInit()
  {
  }

  protected override void OnSpawn()
  {
  }

  public void Serialize(BinaryWriter writer)
  {
    SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
    List<int> cells = solidConduitFlow.GetSOAInfo().Cells;
    int num = 0;
    for (int index = 0; index < cells.Count; ++index)
    {
      int cell = cells[index];
      SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
      if (contents.pickupableHandle.IsValid() && (bool) ((Object) solidConduitFlow.GetPickupable(contents.pickupableHandle)))
        ++num;
    }
    writer.Write(num);
    for (int index = 0; index < cells.Count; ++index)
    {
      int cell = cells[index];
      SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
      if (contents.pickupableHandle.IsValid())
      {
        Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
        if ((bool) ((Object) pickupable))
        {
          writer.Write(cell);
          SaveLoadRoot component = pickupable.GetComponent<SaveLoadRoot>();
          if ((Object) component != (Object) null)
          {
            string name = pickupable.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
            writer.WriteKleiString(name);
            component.Save(writer);
          }
          else
            Debug.Log((object) "Tried to save obj in solid conduit but obj has no SaveLoadRoot", (Object) pickupable.gameObject);
        }
      }
    }
  }

  public void Deserialize(IReader reader)
  {
    SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      int cell = reader.ReadInt32();
      Tag tag = TagManager.Create(reader.ReadKleiString());
      SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
      if ((Object) saveLoadRoot != (Object) null)
      {
        Pickupable component = saveLoadRoot.GetComponent<Pickupable>();
        if ((Object) component != (Object) null)
          solidConduitFlow.SetContents(cell, component);
      }
      else
        Debug.Log((object) ("Tried to deserialize " + tag.ToString() + " into storage but failed"), (Object) this.gameObject);
    }
  }
}
