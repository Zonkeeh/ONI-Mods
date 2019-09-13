// Decompiled with JetBrains decompiler
// Type: ResearchTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTypes
{
  public List<ResearchType> Types = new List<ResearchType>();

  public ResearchTypes()
  {
    this.Types.Add(new ResearchType("alpha", (string) RESEARCH.TYPES.ALPHA.NAME, (string) RESEARCH.TYPES.ALPHA.DESC, Assets.GetSprite((HashedString) "research_type_alpha_icon"), new Color(0.5960785f, 0.6666667f, 0.9137255f), new Recipe.Ingredient[1]
    {
      new Recipe.Ingredient("Dirt".ToTag(), 100f)
    }, 600f, (HashedString) "research_center_kanim", new string[1]
    {
      "ResearchCenter"
    }, (string) RESEARCH.TYPES.ALPHA.RECIPEDESC));
    this.Types.Add(new ResearchType("beta", (string) RESEARCH.TYPES.BETA.NAME, (string) RESEARCH.TYPES.BETA.DESC, Assets.GetSprite((HashedString) "research_type_beta_icon"), new Color(0.6f, 0.3843137f, 0.5686275f), new Recipe.Ingredient[1]
    {
      new Recipe.Ingredient("Water".ToTag(), 25f)
    }, 1200f, (HashedString) "research_center_kanim", new string[1]
    {
      "AdvancedResearchCenter"
    }, (string) RESEARCH.TYPES.BETA.RECIPEDESC));
    this.Types.Add(new ResearchType("gamma", (string) RESEARCH.TYPES.GAMMA.NAME, (string) RESEARCH.TYPES.GAMMA.DESC, Assets.GetSprite((HashedString) "research_type_gamma_icon"), (Color) new Color32((byte) 240, (byte) 141, (byte) 44, byte.MaxValue), (Recipe.Ingredient[]) null, 2400f, (HashedString) "research_center_kanim", new string[1]
    {
      "CosmicResearchCenter"
    }, (string) RESEARCH.TYPES.GAMMA.RECIPEDESC));
  }

  public ResearchType GetResearchType(string id)
  {
    foreach (ResearchType type in this.Types)
    {
      if (id == type.id)
        return type;
    }
    Debug.LogWarning((object) string.Format("No research with type id {0} found", (object) id));
    return (ResearchType) null;
  }

  public class ID
  {
    public const string ALPHA = "alpha";
    public const string BETA = "beta";
    public const string GAMMA = "gamma";
  }
}
