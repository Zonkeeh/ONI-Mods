// Decompiled with JetBrains decompiler
// Type: Unlocks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

public class Unlocks : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnLaunchRocketDelegate = new EventSystem.IntraObjectHandler<Unlocks>((System.Action<Unlocks, object>) ((component, data) => component.OnLaunchRocket(data)));
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDuplicantDiedDelegate = new EventSystem.IntraObjectHandler<Unlocks>((System.Action<Unlocks, object>) ((component, data) => component.OnDuplicantDied(data)));
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDiscoveredSpaceDelegate = new EventSystem.IntraObjectHandler<Unlocks>((System.Action<Unlocks, object>) ((component, data) => component.OnDiscoveredSpace(data)));
  private List<string> unlocked = new List<string>();
  public Dictionary<string, string[]> lockCollections = new Dictionary<string, string[]>()
  {
    {
      "emails",
      new string[21]
      {
        "email_thermodynamiclaws",
        "email_security2",
        "email_pens2",
        "email_atomiconrecruitment",
        "email_devonsblog",
        "email_researchgiant",
        "email_thejanitor",
        "email_newemployee",
        "email_timeoffapproved",
        "email_security3",
        "email_preliminarycalculations",
        "email_hollandsdog",
        "email_temporalbowupdate",
        "email_retemporalbowupdate",
        "email_memorychip",
        "email_arthistoryrequest",
        "email_AIcontrol",
        "email_AIcontrol2",
        "email_friendlyemail",
        "email_AIcontrol3",
        "email_AIcontrol4"
      }
    },
    {
      "journals",
      new string[29]
      {
        "journal_timesarrowthoughts",
        "journal_A046_1",
        "journal_B835_1",
        "journal_sunflowerseeds",
        "journal_B327_1",
        "journal_B556_1",
        "journal_employeeprocessing",
        "journal_B327_2",
        "journal_A046_2",
        "journal_elliesbirthday1",
        "journal_B835_2",
        "journal_ants",
        "journal_pipedream",
        "journal_B556_2",
        "journal_movedrats",
        "journal_B835_3",
        "journal_A046_3",
        "journal_B556_3",
        "journal_B327_3",
        "journal_B835_4",
        "journal_cleanup",
        "journal_A046_4",
        "journal_B327_4",
        "journal_revisitednumbers",
        "journal_B556_4",
        "journal_B835_5",
        "journal_elliesbirthday2",
        "journal_revisitednumbers2",
        "journal_timemusings"
      }
    },
    {
      "researchnotes",
      new string[15]
      {
        "notes_clonedrats",
        "notes_agriculture1",
        "notes_husbandry1",
        "notes_hibiscus3",
        "notes_husbandry2",
        "notes_agriculture2",
        "notes_geneticooze",
        "notes_agriculture3",
        "notes_husbandry3",
        "notes_memoryimplantation",
        "notes_husbandry4",
        "notes_agriculture4",
        "notes_neutronium",
        "notes_firstsuccess",
        "notes_neutroniumapplications"
      }
    },
    {
      "misc",
      new string[6]
      {
        "misc_newsecurity",
        "misc_mailroometiquette",
        "misc_unattendedcultures",
        "misc_politerequest",
        "misc_casualfriday",
        "misc_dishbot"
      }
    }
  };
  public Dictionary<int, string> cycleLocked = new Dictionary<int, string>()
  {
    {
      0,
      "log1"
    },
    {
      3,
      "log2"
    },
    {
      15,
      "log3"
    },
    {
      1000,
      "log4"
    },
    {
      1500,
      "log4b"
    },
    {
      2000,
      "log5"
    },
    {
      2500,
      "log5b"
    },
    {
      3000,
      "log6"
    },
    {
      3500,
      "log6b"
    },
    {
      4000,
      "log7"
    },
    {
      4001,
      "log8"
    }
  };
  private const int FILE_IO_RETRY_ATTEMPTS = 5;

  private static string UnlocksFilename
  {
    get
    {
      return System.IO.Path.Combine(Util.RootFolder(), "unlocks.json");
    }
  }

  protected override void OnPrefabInit()
  {
    this.LoadUnlocks();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UnlockCycleCodexes();
    GameClock.Instance.Subscribe(631075836, new System.Action<object>(this.OnNewDay));
    this.Subscribe<Unlocks>(-1056989049, Unlocks.OnLaunchRocketDelegate);
    this.Subscribe<Unlocks>(282337316, Unlocks.OnDuplicantDiedDelegate);
    this.Subscribe<Unlocks>(-818188514, Unlocks.OnDiscoveredSpaceDelegate);
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnNewDupe);
  }

  public bool IsUnlocked(string unlockID)
  {
    if (string.IsNullOrEmpty(unlockID))
      return false;
    if (DebugHandler.InstantBuildMode)
      return true;
    return this.unlocked.Contains(unlockID);
  }

  public void Unlock(string unlockID)
  {
    if (string.IsNullOrEmpty(unlockID))
    {
      DebugUtil.DevAssert(false, "Unlock called with null or empty string");
    }
    else
    {
      if (this.unlocked.Contains(unlockID))
        return;
      this.unlocked.Add(unlockID);
      this.SaveUnlocks();
      Game.Instance.Trigger(1594320620, (object) unlockID);
      MessageNotification unlockNotification = this.GenerateCodexUnlockNotification(unlockID);
      if (unlockNotification == null)
        return;
      this.GetComponent<Notifier>().Add((Notification) unlockNotification, string.Empty);
    }
  }

  private void SaveUnlocks()
  {
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string s = JsonConvert.SerializeObject((object) this.unlocked);
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (num >= 5)
        break;
      try
      {
        Thread.Sleep(num * 100);
        using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
          flag = true;
          byte[] bytes = new ASCIIEncoding().GetBytes(s);
          fileStream.Write(bytes, 0, bytes.Length);
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarningFormat("Failed to save Unlocks attempt {0}: {1}", (object) (num + 1), (object) ex.ToString());
      }
      ++num;
    }
  }

  public void LoadUnlocks()
  {
    this.unlocked.Clear();
    if (!File.Exists(Unlocks.UnlocksFilename))
      return;
    string empty = string.Empty;
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (num < 5)
      {
        try
        {
          Thread.Sleep(num * 100);
          using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            flag = true;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] numArray = new byte[fileStream.Length];
            if ((long) fileStream.Read(numArray, 0, numArray.Length) == fileStream.Length)
              empty += asciiEncoding.GetString(numArray);
          }
        }
        catch (Exception ex)
        {
          Debug.LogWarningFormat("Failed to load Unlocks attempt {0}: {1}", (object) (num + 1), (object) ex.ToString());
        }
        ++num;
      }
      else
        break;
    }
    if (string.IsNullOrEmpty(empty))
      return;
    try
    {
      foreach (string str in JsonConvert.DeserializeObject<string[]>(empty))
      {
        if (!string.IsNullOrEmpty(str) && !this.unlocked.Contains(str))
          this.unlocked.Add(str);
      }
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("Error parsing unlocks file [{0}]: {1}", (object) Unlocks.UnlocksFilename, (object) ex.ToString());
    }
  }

  public string UnlockNext(string collectionID)
  {
    foreach (string unlockID in this.lockCollections[collectionID])
    {
      if (string.IsNullOrEmpty(unlockID))
        DebugUtil.DevAssertArgs(false, (object) "Found null/empty string in Unlocks collection: ", (object) collectionID);
      else if (!this.IsUnlocked(unlockID))
      {
        this.Unlock(unlockID);
        return unlockID;
      }
    }
    return (string) null;
  }

  private MessageNotification GenerateCodexUnlockNotification(string lockID)
  {
    string entryForLock = CodexCache.GetEntryForLock(lockID);
    string key = (string) null;
    if (CodexCache.FindSubEntry(lockID) != null)
      key = CodexCache.FindSubEntry(lockID).title;
    else if (CodexCache.FindSubEntry(entryForLock) != null)
      key = CodexCache.FindSubEntry(entryForLock).title;
    else if (CodexCache.FindEntry(entryForLock) != null)
      key = CodexCache.FindEntry(entryForLock).title;
    string unlock_message = UI.FormatAsLink((string) Strings.Get(key), entryForLock);
    if (string.IsNullOrEmpty(key))
      return (MessageNotification) null;
    ContentContainer contentContainer = CodexCache.FindEntry(entryForLock).contentContainers.Find((Predicate<ContentContainer>) (match => match.lockID == lockID));
    if (contentContainer != null)
    {
      foreach (ICodexWidget codexWidget in contentContainer.content)
      {
        CodexText codexText = codexWidget as CodexText;
        if (codexText != null)
          unlock_message = unlock_message + "\n\n" + codexText.text;
      }
    }
    return new MessageNotification((Message) new CodexUnlockedMessage(lockID, unlock_message));
  }

  private void UnlockCycleCodexes()
  {
    foreach (KeyValuePair<int, string> keyValuePair in this.cycleLocked)
    {
      if (GameClock.Instance.GetCycle() + 1 >= keyValuePair.Key)
        this.Unlock(keyValuePair.Value);
    }
  }

  private void OnNewDay(object data)
  {
    this.UnlockCycleCodexes();
  }

  private void OnLaunchRocket(object data)
  {
    this.Unlock("surfacebreach");
    this.Unlock("firstrocketlaunch");
  }

  private void OnDuplicantDied(object data)
  {
    this.Unlock("duplicantdeath");
    if (Components.LiveMinionIdentities.Count != 1)
      return;
    this.Unlock("onedupeleft");
  }

  private void OnNewDupe(MinionIdentity minion_identity)
  {
    if (Components.LiveMinionIdentities.Count < 35)
      return;
    this.Unlock("fulldupecolony");
  }

  private void OnDiscoveredSpace(object data)
  {
    this.Unlock("surfacebreach");
  }

  public void Sim4000ms(float dt)
  {
    int x1 = int.MinValue;
    int num1 = int.MinValue;
    int x2 = int.MaxValue;
    int num2 = int.MaxValue;
    foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
    {
      if (!((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null))
      {
        int cell = Grid.PosToCell((KMonoBehaviour) minionIdentity);
        if (Grid.IsValidCell(cell))
        {
          int x3;
          int y;
          Grid.CellToXY(cell, out x3, out y);
          if (y > num1)
          {
            num1 = y;
            x1 = x3;
          }
          if (y < num2)
          {
            x2 = x3;
            num2 = y;
          }
        }
      }
    }
    if (num1 != int.MinValue)
    {
      int y = num1;
      for (int index = 0; index < 30; ++index)
      {
        ++y;
        int cell = Grid.XYToCell(x1, y);
        if (Grid.IsValidCell(cell))
        {
          if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
          {
            this.Unlock("nearingsurface");
            break;
          }
        }
        else
          break;
      }
    }
    if (num2 == int.MaxValue)
      return;
    int y1 = num2;
    for (int index = 0; index < 30; ++index)
    {
      --y1;
      int cell = Grid.XYToCell(x2, y1);
      if (!Grid.IsValidCell(cell))
        break;
      if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.ToxicJungle && Grid.Element[cell].id == SimHashes.Magma)
      {
        this.Unlock("nearingmagma");
        break;
      }
    }
  }
}
