// Decompiled with JetBrains decompiler
// Type: SpaceArtifact
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class SpaceArtifact : KMonoBehaviour, IEffectDescriptor, IGameObjectEffectDescriptor
{
  public const string ID = "SpaceArtifact";
  [SerializeField]
  private string ui_anim;
  [SerializeField]
  private ArtifactTier artifactTier;

  public void SetArtifactTier(ArtifactTier tier)
  {
    this.artifactTier = tier;
  }

  public ArtifactTier GetArtifactTier()
  {
    return this.artifactTier;
  }

  public void SetUIAnim(string anim)
  {
    this.ui_anim = anim;
  }

  public string GetUIAnim()
  {
    return this.ui_anim;
  }

  public List<Descriptor> GetEffectDescriptions()
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format("This is an artifact from space"), string.Format("This is the tooltip string"), Descriptor.DescriptorType.Information, false)
    };
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetEffectDescriptions();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return this.GetEffectDescriptions();
  }
}
