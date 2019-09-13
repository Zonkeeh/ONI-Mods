// Decompiled with JetBrains decompiler
// Type: SubstanceTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
  [SerializeField]
  private List<Substance> list;
  public Material solidMaterial;
  public Material liquidMaterial;

  public List<Substance> GetList()
  {
    return this.list;
  }

  public Substance GetSubstance(SimHashes substance)
  {
    int count = this.list.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.list[index].elementID == substance)
        return this.list[index];
    }
    return (Substance) null;
  }

  public void OnBeforeSerialize()
  {
    this.BindAnimList();
  }

  public void OnAfterDeserialize()
  {
    this.BindAnimList();
  }

  private void BindAnimList()
  {
    foreach (Substance substance in this.list)
    {
      if ((Object) substance.anim != (Object) null && (substance.anims == null || substance.anims.Length == 0))
      {
        substance.anims = new KAnimFile[1];
        substance.anims[0] = substance.anim;
      }
    }
  }
}
