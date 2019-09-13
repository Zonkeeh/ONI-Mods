// Decompiled with JetBrains decompiler
// Type: LoreBearer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LoreBearer : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<LoreBearer> RefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LoreBearer>((System.Action<LoreBearer, object>) ((component, data) => component.RefreshUserMenu(data)));
  public string BeenSearched = (string) UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;
  private bool BeenClicked;

  public string content
  {
    get
    {
      return (string) Strings.Get("STRINGS.LORE.BUILDINGS." + this.gameObject.name + ".ENTRY");
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<LoreBearer>(493375141, LoreBearer.RefreshUserMenuDelegate);
  }

  private void RefreshUserMenu(object data = null)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", (string) UI.USERMENUACTIONS.READLORE.NAME, new System.Action(this.OnClickRead), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.READLORE.TOOLTIP, true), 1f);
  }

  private System.Action<InfoDialogScreen> OpenCodex(string key)
  {
    return (System.Action<InfoDialogScreen>) (dialog =>
    {
      dialog.Deactivate();
      string entryForLock = CodexCache.GetEntryForLock(key);
      if (entryForLock == null)
        KCrashReporter.Assert(false, "Missing codex entry: " + key);
      else
        ManagementMenu.Instance.OpenCodexToEntry(entryForLock);
    });
  }

  private void OnClickRead()
  {
    InfoDialogScreen infoDialogScreen = (InfoDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
    infoDialogScreen.SetHeader(this.gameObject.GetComponent<KSelectable>().GetProperName());
    if (this.BeenClicked)
    {
      infoDialogScreen.AddPlainText(this.BeenSearched);
    }
    else
    {
      this.BeenClicked = true;
      if (this.gameObject.name == "GeneShuffler")
        Game.Instance.unlocks.Unlock("neuralvacillator");
      if (this.gameObject.name == "PropDesk")
      {
        string key = Game.Instance.unlocks.UnlockNext("emails");
        if (key != null)
        {
          string str = "SEARCH" + (object) UnityEngine.Random.Range(1, 6);
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_SUCCESS." + str));
          infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex(key));
        }
        else
        {
          string str = "SEARCH" + (object) UnityEngine.Random.Range(1, 8);
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str));
        }
      }
      else if (this.gameObject.name == "GeneShuffler" || this.gameObject.name == "MassiveHeatSink")
      {
        string key = Game.Instance.unlocks.UnlockNext("researchnotes");
        if (key != null)
        {
          string str = "SEARCH" + (object) UnityEngine.Random.Range(1, 3);
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_TECHNOLOGY_SUCCESS." + str));
          infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex(key));
        }
        else
        {
          string str = "SEARCH1";
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str));
        }
      }
      else if (this.gameObject.name == "PropReceptionDesk")
      {
        Game.Instance.unlocks.Unlock("email_pens");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_ELLIESDESK);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("email_pens"));
      }
      else if (this.gameObject.name == "PropFacilityDesk")
      {
        Game.Instance.unlocks.Unlock("journal_magazine");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_STERNSDESK);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("journal_magazine"));
      }
      else if (this.gameObject.name == "HeadquartersComplete")
      {
        Game.Instance.unlocks.Unlock("pod_evacuation");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_POD);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("pod_evacuation"));
      }
      else if (this.gameObject.name == "PropFacilityDisplay")
      {
        Game.Instance.unlocks.Unlock("display_prop1");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("display_prop1"));
      }
      else if (this.gameObject.name == "PropFacilityDisplay2")
      {
        Game.Instance.unlocks.Unlock("display_prop2");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("display_prop2"));
      }
      else if (this.gameObject.name == "PropFacilityDisplay3")
      {
        Game.Instance.unlocks.Unlock("display_prop3");
        infoDialogScreen.AddPlainText((string) UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY);
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("display_prop3"));
      }
      else if (this.gameObject.name == "PropFacilityGlobeDroors")
      {
        Game.Instance.unlocks.Unlock("journal_newspaper");
        infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_CABINET"));
        infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex("journal_newspaper"));
      }
      else
      {
        string key = Game.Instance.unlocks.UnlockNext("journals");
        if (key != null)
        {
          string str = "SEARCH" + (object) UnityEngine.Random.Range(1, 6);
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
          infoDialogScreen.AddOption((string) UI.USERMENUACTIONS.READLORE.GOTODATABASE, this.OpenCodex(key));
        }
        else
        {
          string str = "SEARCH1";
          infoDialogScreen.AddPlainText((string) Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str));
        }
      }
    }
  }
}
