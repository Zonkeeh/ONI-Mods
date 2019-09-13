// Decompiled with JetBrains decompiler
// Type: ResearchPointObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ResearchPointObject : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public string TypeID = string.Empty;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Research.Instance.AddResearchPoints(this.TypeID, 1f);
    ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, this.transform, 1.5f, false);
    Util.KDestroyGameObject(this.gameObject);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    ResearchType researchType = Research.Instance.GetResearchType(this.TypeID);
    descriptorList.Add(new Descriptor(string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, (object) researchType.name), string.Format((string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.RESEARCHPOINT, (object) researchType.description), Descriptor.DescriptorType.Effect, false));
    return descriptorList;
  }
}
