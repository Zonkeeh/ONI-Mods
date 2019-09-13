// Decompiled with JetBrains decompiler
// Type: Database.TechItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
  public class TechItems : ResourceSet<TechItem>
  {
    public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";
    public TechItem automationOverlay;
    public const string SUITS_OVERLAY_ID = "SuitsOverlay";
    public TechItem suitsOverlay;
    public const string JET_SUIT_ID = "JetSuit";
    public TechItem jetSuit;
    public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";
    public TechItem betaResearchPoint;
    public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";
    public TechItem gammaResearchPoint;
    public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";
    public TechItem conveyorOverlay;

    public TechItems(ResourceSet parent)
      : base(nameof (TechItems), parent)
    {
      this.automationOverlay = this.AddTechItem("AutomationOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"));
      this.suitsOverlay = this.AddTechItem("SuitsOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"));
      this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"));
      this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"));
      this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"));
      this.jetSuit = this.AddTechItem("JetSuit", (string) RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetSpriteFnBuilder("overlay_suit"));
    }

    private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
    {
      return (Func<string, bool, Sprite>) ((anim, centered) => Assets.GetSprite((HashedString) spriteName));
    }

    public TechItem AddTechItem(
      string id,
      string name,
      string description,
      Func<string, bool, Sprite> getUISprite)
    {
      if (this.TryGet(id) != null)
      {
        DebugUtil.LogWarningArgs((object) "Tried adding a tech item called", (object) id, (object) name, (object) "but it was already added!");
        return this.Get(id);
      }
      Tech parentTech = this.LookupGroupForID(id);
      if (parentTech == null)
        return (TechItem) null;
      TechItem resource = new TechItem(id, (ResourceSet) this, name, description, getUISprite, parentTech);
      this.Add(resource);
      parentTech.unlockedItems.Add(resource);
      return resource;
    }

    public bool IsTechItemComplete(string id)
    {
      foreach (TechItem resource in this.resources)
      {
        if (resource.Id == id)
          return resource.IsComplete();
      }
      return true;
    }

    public Tech LookupGroupForID(string itemID)
    {
      foreach (KeyValuePair<string, string[]> keyValuePair in Techs.TECH_GROUPING)
      {
        if (Array.IndexOf<string>(keyValuePair.Value, itemID) != -1)
          return Db.Get().Techs.Get(keyValuePair.Key);
      }
      return (Tech) null;
    }
  }
}
