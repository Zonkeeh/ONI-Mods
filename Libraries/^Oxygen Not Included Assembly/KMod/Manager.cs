// Decompiled with JetBrains decompiler
// Type: KMod.Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KMod
{
  public class Manager
  {
    public List<IDistributionPlatform> distribution_platforms = new List<IDistributionPlatform>();
    public List<Mod> mods = new List<Mod>();
    public List<Event> events = new List<Event>();
    private bool dirty = true;
    private bool load_user_mod_loader_dll = true;
    private int current_version = 1;
    public const Content all_content = Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation;
    public const Content boot_content = Content.Strings | Content.DLL | Content.Translation | Content.Animation;
    public const Content install_content = Content.DLL;
    public const Content on_demand_content = Content.LayerableFiles;
    public Manager.OnUpdate on_update;
    private const int IO_OP_RETRY_COUNT = 5;

    public Manager()
    {
      Manager manager = this;
      string filename = this.GetFilename();
      try
      {
        FileUtil.DoIOAction((System.Action) (() =>
        {
          if (!File.Exists(filename))
            return;
          manager.mods = JsonConvert.DeserializeObject<Manager.PersistentData>(File.ReadAllText(filename)).mods;
        }), 5);
      }
      catch (Exception ex)
      {
        Debug.LogWarningFormat((string) UI.FRONTEND.MODS.DB_CORRUPT, (object) filename);
        this.mods = new List<Mod>();
      }
      List<Mod> modList = new List<Mod>();
      bool flag = false;
      foreach (Mod mod in this.mods)
      {
        switch (mod.status)
        {
          case Mod.Status.UninstallPending:
            Debug.LogFormat("Latent uninstall of mod {0} from {1}", (object) mod.title, (object) mod.label.install_path);
            if (mod.Uninstall())
            {
              modList.Add(mod);
            }
            else
            {
              DebugUtil.Assert(mod.status == Mod.Status.UninstallPending);
              Debug.LogFormat("\t...failed to uninstall mod {0}", (object) mod.title);
            }
            if (mod.status != Mod.Status.UninstallPending)
            {
              flag = true;
              break;
            }
            break;
          case Mod.Status.ReinstallPending:
            Debug.LogFormat("Latent reinstall of mod {0}", (object) mod.title);
            if (!string.IsNullOrEmpty(mod.reinstall_path) && File.Exists(mod.reinstall_path))
            {
              bool enabled = mod.enabled;
              mod.file_source = (IFileSource) new ZipFile(mod.reinstall_path);
              mod.enabled = false;
              if (mod.Uninstall())
              {
                mod.Install();
                if (mod.status == Mod.Status.Installed)
                  mod.enabled = enabled;
              }
              flag = true;
              break;
            }
            if (mod.enabled)
            {
              mod.enabled = false;
              flag = true;
              break;
            }
            break;
        }
        if (!string.IsNullOrEmpty(mod.reinstall_path))
        {
          mod.reinstall_path = (string) null;
          flag = true;
        }
      }
      foreach (Mod mod in modList)
        this.mods.Remove(mod);
      foreach (Mod mod in this.mods)
        mod.ScanContent();
      if (!flag)
        return;
      this.Save();
    }

    public static string GetDirectory()
    {
      return System.IO.Path.Combine(Util.RootFolder(), "mods/");
    }

    public void Shutdown()
    {
      foreach (Mod mod in this.mods)
        mod.Unload(Content.LayerableFiles);
    }

    public void Sanitize(GameObject parent)
    {
      ListPool<Label, Manager>.PooledList pooledList = ListPool<Label, Manager>.Allocate();
      foreach (Mod mod in this.mods)
      {
        if (!mod.is_subscribed)
          pooledList.Add(mod.label);
      }
      foreach (Label label in (List<Label>) pooledList)
        this.Unsubscribe(label, (object) this);
      pooledList.Recycle();
      this.Report(parent);
    }

    public bool HaveMods()
    {
      foreach (Mod mod in this.mods)
      {
        if (mod.status == Mod.Status.Installed && mod.HasContent())
          return true;
      }
      return false;
    }

    public bool HaveLoadedMods()
    {
      foreach (Mod mod in this.mods)
      {
        if (mod.status != Mod.Status.NotInstalled && mod.IsActive())
          return true;
      }
      return false;
    }

    private void Install(Mod mod)
    {
      if (mod.status != Mod.Status.NotInstalled)
        return;
      Debug.LogFormat("\tInstalling mod: {0}", (object) mod.title);
      mod.Install();
      if (mod.status == Mod.Status.Installed)
      {
        Debug.Log((object) "\tSuccessfully installed.");
        this.events.Add(new Event()
        {
          event_type = EventType.Installed,
          mod = mod.label
        });
      }
      else
      {
        Debug.Log((object) "\tFailed install. Will install on restart.");
        this.events.Add(new Event()
        {
          event_type = EventType.InstallFailed,
          mod = mod.label
        });
        this.events.Add(new Event()
        {
          event_type = EventType.RestartRequested,
          mod = mod.label
        });
      }
    }

    private void Uninstall(Mod mod)
    {
      if (mod.status == Mod.Status.NotInstalled)
        return;
      Debug.LogFormat("\tUninstalling mod {0}", (object) mod.title);
      mod.Uninstall();
      if (mod.status != Mod.Status.UninstallPending)
        return;
      Debug.Log((object) "\tFailed. Will re-install on restart.");
      mod.status = Mod.Status.ReinstallPending;
      this.events.Add(new Event()
      {
        event_type = EventType.RestartRequested,
        mod = mod.label
      });
    }

    public void Subscribe(Mod mod, object caller)
    {
      Debug.LogFormat("Subscribe to mod {0}", (object) mod.title);
      Mod mod1 = this.mods.Find((Predicate<Mod>) (candidate => mod.label.Match(candidate.label)));
      mod.is_subscribed = true;
      if (mod1 == null)
      {
        this.mods.Add(mod);
        this.Install(mod);
      }
      else
      {
        if (mod1.status == Mod.Status.UninstallPending)
        {
          mod1.status = Mod.Status.Installed;
          this.events.Add(new Event()
          {
            event_type = EventType.Installed,
            mod = mod1.label
          });
        }
        bool flag1 = mod1.label.version != mod.label.version;
        bool flag2 = mod1.available_content != mod.available_content;
        bool flag3 = flag1 || flag2 || mod1.status == Mod.Status.ReinstallPending;
        if (flag1)
          this.events.Add(new Event()
          {
            event_type = EventType.VersionUpdate,
            mod = mod.label
          });
        if (flag2)
          this.events.Add(new Event()
          {
            event_type = EventType.AvailableContentChanged,
            mod = mod.label
          });
        string root = mod.file_source.GetRoot();
        mod1.CopyPersistentDataTo(mod);
        int index = this.mods.IndexOf(mod1);
        this.mods.RemoveAt(index);
        this.mods.Insert(index, mod);
        if (flag3 || mod.status == Mod.Status.NotInstalled)
        {
          if (mod.enabled)
          {
            mod.reinstall_path = root;
            mod.status = Mod.Status.ReinstallPending;
            this.events.Add(new Event()
            {
              event_type = EventType.RestartRequested,
              mod = mod.label
            });
          }
          else
          {
            if (flag3)
              this.Uninstall(mod);
            this.Install(mod);
          }
        }
        else
          mod.file_source = mod1.file_source;
      }
      this.dirty = true;
      this.Update(caller);
    }

    public void Update(Mod mod, object caller)
    {
      Debug.LogFormat("Update mod {0}", (object) mod.title);
      Mod mod1 = this.mods.Find((Predicate<Mod>) (candidate => mod.label.Match(candidate.label)));
      DebugUtil.DevAssert(!string.IsNullOrEmpty(mod1.label.id), "Should be subscribed to a mod we are getting an Update notification for");
      if (mod1.status == Mod.Status.UninstallPending)
        return;
      this.events.Add(new Event()
      {
        event_type = EventType.VersionUpdate,
        mod = mod.label
      });
      string root = mod.file_source.GetRoot();
      mod1.CopyPersistentDataTo(mod);
      mod.is_subscribed = mod1.is_subscribed;
      int index = this.mods.IndexOf(mod1);
      this.mods.RemoveAt(index);
      this.mods.Insert(index, mod);
      if (mod.enabled)
      {
        mod.reinstall_path = root;
        mod.status = Mod.Status.ReinstallPending;
        this.events.Add(new Event()
        {
          event_type = EventType.RestartRequested,
          mod = mod.label
        });
      }
      else
      {
        this.Uninstall(mod);
        this.Install(mod);
      }
      this.dirty = true;
      this.Update(caller);
    }

    public void Unsubscribe(Label label, object caller)
    {
      Debug.LogFormat("Unsubscribe from mod {0}", (object) label.ToString());
      int index = 0;
      foreach (Mod mod in this.mods)
      {
        if (mod.label.Match(label))
        {
          Debug.LogFormat("\t...found it: {0}", (object) mod.title);
          break;
        }
        ++index;
      }
      if (index == this.mods.Count)
      {
        Debug.LogFormat("\t...not found");
      }
      else
      {
        Mod mod = this.mods[index];
        mod.enabled = false;
        mod.Unload(Content.LayerableFiles);
        this.events.Add(new Event()
        {
          event_type = EventType.Uninstalled,
          mod = mod.label
        });
        if (mod.IsActive())
        {
          Debug.LogFormat("\tCould not unload all content provided by mod {0} : {1}\nUninstall will likely fail", (object) mod.title, (object) mod.label.ToString());
          this.events.Add(new Event()
          {
            event_type = EventType.RestartRequested,
            mod = mod.label
          });
        }
        if (mod.status == Mod.Status.Installed)
        {
          Debug.LogFormat("\tUninstall mod {0} : {1}", (object) mod.title, (object) mod.label.ToString());
          mod.Uninstall();
        }
        if (mod.status == Mod.Status.NotInstalled)
        {
          Debug.LogFormat("\t...success. Removing from management list {0} : {1}", (object) mod.title, (object) mod.label.ToString());
          this.mods.RemoveAt(index);
        }
        this.dirty = true;
        this.Update(caller);
      }
    }

    public bool IsInDevMode()
    {
      return this.mods.Exists((Predicate<Mod>) (mod =>
      {
        if (mod.enabled)
          return mod.label.distribution_platform == Label.DistributionPlatform.Dev;
        return false;
      }));
    }

    public void Load(Content content)
    {
      if ((content & Content.DLL) != (Content) 0 && this.load_user_mod_loader_dll)
      {
        if (!DLLLoader.LoadUserModLoaderDLL())
          Debug.Log((object) "ModLoader.dll failed to load. Either it is not present or it encountered an error");
        this.load_user_mod_loader_dll = false;
      }
      foreach (Mod mod in this.mods)
      {
        if (mod.enabled)
          mod.Load(content);
      }
      bool flag1 = false;
      bool flag2 = this.IsInDevMode();
      foreach (Mod mod in this.mods)
      {
        Content content1 = mod.loaded_content & content;
        Content content2 = mod.available_content & content;
        if (mod.enabled && content1 != content2)
        {
          mod.Crash(!flag2);
          if (!mod.enabled)
          {
            flag1 = true;
            this.events.Add(new Event()
            {
              event_type = EventType.Deactivated,
              mod = mod.label
            });
          }
          Debug.LogFormat("Failed to load mod {0}...disabling", (object) mod.title);
          this.events.Add(new Event()
          {
            event_type = EventType.LoadError,
            mod = mod.label
          });
        }
      }
      if (!flag1)
        return;
      this.Save();
    }

    public void Unload(Content content)
    {
      foreach (Mod mod in this.mods)
        mod.Unload(content);
    }

    public void Update(object change_source)
    {
      if (!this.dirty)
        return;
      this.dirty = false;
      this.Save();
      if (this.on_update == null)
        return;
      this.on_update(change_source);
    }

    public bool MatchFootprint(List<Label> footprint, Content relevant_content)
    {
      if (footprint == null)
        return true;
      bool flag1 = true;
      bool flag2 = true;
      bool flag3 = false;
      int num = -1;
      Func<Label, Mod, bool> is_match = (Func<Label, Mod, bool>) ((label, mod) => mod.label.Match(label));
      foreach (Label label1 in footprint)
      {
        Label label = label1;
        bool flag4 = false;
        for (int index = num + 1; index != this.mods.Count; ++index)
        {
          Mod mod = this.mods[index];
          num = index;
          Content content = mod.available_content & relevant_content;
          bool flag5 = content != (Content) 0;
          if (!is_match(label, mod))
          {
            if (flag5 && mod.enabled)
            {
              this.events.Add(new Event()
              {
                event_type = EventType.ExpectedInactive,
                mod = mod.label
              });
              flag3 = true;
            }
          }
          else
          {
            if (flag5)
            {
              if (!mod.enabled)
              {
                this.events.Add(new Event()
                {
                  event_type = EventType.ExpectedActive,
                  mod = label
                });
                flag1 = false;
              }
              else if (!mod.AllActive(content))
                this.events.Add(new Event()
                {
                  event_type = EventType.LoadError,
                  mod = label
                });
            }
            flag4 = true;
            break;
          }
        }
        if (!flag4)
        {
          this.events.Add(new Event()
          {
            event_type = !this.mods.Exists((Predicate<Mod>) (candidate => is_match(label, candidate))) ? EventType.NotFound : EventType.OutOfOrder,
            mod = label
          });
          flag2 = false;
        }
      }
      for (int index = num + 1; index != this.mods.Count; ++index)
      {
        Mod mod = this.mods[index];
        if ((mod.available_content & relevant_content) != (Content) 0 && mod.enabled)
        {
          this.events.Add(new Event()
          {
            event_type = EventType.ExpectedInactive,
            mod = mod.label
          });
          flag3 = true;
        }
      }
      if (flag2 && flag1)
        return !flag3;
      return false;
    }

    private string GetFilename()
    {
      return FileSystem.Normalize(System.IO.Path.Combine(Manager.GetDirectory(), "mods.json"));
    }

    public static void Dialog(
      GameObject parent = null,
      string title = null,
      string text = null,
      string confirm_text = null,
      System.Action on_confirm = null,
      string cancel_text = null,
      System.Action on_cancel = null,
      string configurable_text = null,
      System.Action on_configurable_clicked = null,
      Sprite image_sprite = null,
      bool activateBlackBackground = true)
    {
      ((ConfirmDialogScreen) KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent ?? Global.Instance.globalCanvas)).PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, title, confirm_text, cancel_text, image_sprite, activateBlackBackground);
    }

    private static string MakeModList(List<Event> events, EventType event_type)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      foreach (Event @event in events)
      {
        if (@event.event_type == event_type)
          stringBuilder.AppendLine(@event.mod.title);
      }
      return stringBuilder.ToString();
    }

    private static string MakeEventList(List<Event> events)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      string title = (string) null;
      string title_tooltip = (string) null;
      foreach (Event @event in events)
      {
        Event.GetUIStrings(@event.event_type, out title, out title_tooltip);
        stringBuilder.AppendFormat("{0}: {1}", (object) title, (object) @event.mod.title);
        if (!string.IsNullOrEmpty(@event.details))
          stringBuilder.AppendFormat(" ({0})", (object) @event.details);
        stringBuilder.Append("\n");
      }
      return stringBuilder.ToString();
    }

    private static string MakeModList(List<Event> events)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      HashSetPool<string, Manager>.PooledHashSet pooledHashSet = HashSetPool<string, Manager>.Allocate();
      foreach (Event @event in events)
      {
        if (pooledHashSet.Add(@event.mod.title))
          stringBuilder.AppendLine(@event.mod.title);
      }
      pooledHashSet.Recycle();
      return stringBuilder.ToString();
    }

    private void LoadFailureDialog(GameObject parent)
    {
      if (this.events.Count == 0)
        return;
      foreach (Event @event in this.events)
      {
        if (@event.event_type == EventType.LoadError)
        {
          foreach (Mod mod in this.mods)
          {
            if (mod.label.distribution_platform != Label.DistributionPlatform.Local && mod.label.distribution_platform != Label.DistributionPlatform.Dev && mod.label.Match(@event.mod))
              mod.status = Mod.Status.ReinstallPending;
          }
        }
      }
      this.dirty = true;
      this.Update((object) this);
      GameObject parent1 = parent;
      string title = (string) UI.FRONTEND.MOD_DIALOGS.LOAD_FAILURE.TITLE;
      string text = string.Format((string) UI.FRONTEND.MOD_DIALOGS.LOAD_FAILURE.MESSAGE, (object) Manager.MakeModList(this.events, EventType.LoadError));
      string ok = (string) UI.FRONTEND.MOD_DIALOGS.RESTART.OK;
      string cancel = (string) UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL;
      Manager.Dialog(parent1, title, text, ok, new System.Action(App.instance.Restart), cancel, (System.Action) (() => {}), (string) null, (System.Action) null, (Sprite) null, true);
      this.events.Clear();
    }

    private void DevRestartDialog(GameObject parent, bool is_crash)
    {
      if (this.events.Count == 0)
        return;
      if (is_crash)
        Manager.Dialog(parent, (string) UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.TITLE, string.Format((string) UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.DEV_MESSAGE, (object) Manager.MakeEventList(this.events)), (string) UI.FRONTEND.MOD_DIALOGS.RESTART.OK, (System.Action) (() =>
        {
          foreach (Mod mod in this.mods)
            mod.enabled = false;
          this.dirty = true;
          this.Update((object) this);
          App.instance.Restart();
        }), (string) UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL, (System.Action) (() => {}), (string) null, (System.Action) null, (Sprite) null, true);
      else
        Manager.Dialog(parent, (string) UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, string.Format((string) UI.FRONTEND.MOD_DIALOGS.RESTART.DEV_MESSAGE, (object) Manager.MakeEventList(this.events)), (string) UI.FRONTEND.MOD_DIALOGS.RESTART.OK, (System.Action) (() => App.instance.Restart()), (string) UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL, (System.Action) (() => {}), (string) null, (System.Action) null, (Sprite) null, true);
      this.events.Clear();
    }

    public void RestartDialog(
      string title,
      string message_format,
      System.Action on_cancel,
      bool with_details,
      GameObject parent,
      string cancel_text = null)
    {
      if (this.events.Count == 0)
        return;
      GameObject parent1 = parent;
      string title1 = title;
      string text = string.Format(message_format, !with_details ? (object) Manager.MakeModList(this.events) : (object) Manager.MakeEventList(this.events));
      string ok = (string) UI.FRONTEND.MOD_DIALOGS.RESTART.OK;
      string cancel_text1 = cancel_text ?? (string) UI.FRONTEND.MOD_DIALOGS.RESTART.CANCEL;
      Manager.Dialog(parent1, title1, text, ok, new System.Action(App.instance.Restart), cancel_text1, on_cancel, (string) null, (System.Action) null, (Sprite) null, true);
      this.events.Clear();
    }

    public void NotifyDialog(string title, string message_format, GameObject parent)
    {
      if (this.events.Count == 0)
        return;
      Manager.Dialog(parent, title, string.Format(message_format, (object) Manager.MakeEventList(this.events)), (string) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (System.Action) null, (Sprite) null, true);
      this.events.Clear();
    }

    public void HandleCrash()
    {
      Debug.Log((object) "Error occurred with mods active. Disabling all mods (unless dev mods active).");
      bool flag = this.IsInDevMode();
      foreach (Mod mod in this.mods)
      {
        if (mod.enabled)
        {
          this.events.Add(new Event()
          {
            event_type = EventType.ActiveDuringCrash,
            mod = mod.label
          });
          mod.Crash(!flag);
          if (!flag)
            this.events.Add(new Event()
            {
              event_type = EventType.Deactivated,
              mod = mod.label
            });
        }
      }
      this.dirty = true;
      this.Update((object) this);
    }

    public void HandleErrors(List<YamlIO.Error> world_gen_errors)
    {
      string str1 = FileSystem.Normalize(Manager.GetDirectory());
      ListPool<Mod, Manager>.PooledList pooledList = ListPool<Mod, Manager>.Allocate();
      foreach (YamlIO.Error worldGenError in world_gen_errors)
      {
        string str2 = worldGenError.file.source == null ? string.Empty : FileSystem.Normalize(worldGenError.file.source.GetRoot());
        YamlIO.LogError(worldGenError, str2.Contains(str1));
        if (worldGenError.severity != YamlIO.Error.Severity.Recoverable && str2.Contains(str1))
        {
          foreach (Mod mod in this.mods)
          {
            if (mod.enabled && str2.Contains(mod.label.install_path))
            {
              this.events.Add(new Event()
              {
                event_type = EventType.BadWorldGen,
                mod = mod.label,
                details = System.IO.Path.GetFileName(worldGenError.file.full_path)
              });
              break;
            }
          }
        }
      }
      bool flag = this.IsInDevMode();
      foreach (Mod mod in (List<Mod>) pooledList)
      {
        mod.Crash(!flag);
        if (!flag)
          this.events.Add(new Event()
          {
            event_type = EventType.Deactivated,
            mod = mod.label
          });
        this.dirty = true;
      }
      pooledList.Recycle();
      this.Update((object) this);
    }

    public void Report(GameObject parent)
    {
      if (this.events.Count == 0)
        return;
      for (int index1 = 0; index1 < this.events.Count; ++index1)
      {
        Event @event = this.events[index1];
        for (int index2 = this.events.Count - 1; index2 != index1; --index2)
        {
          if (this.events[index2].event_type == @event.event_type && this.events[index2].mod.Match(@event.mod) && this.events[index2].details == @event.details)
            this.events.RemoveAt(index2);
        }
      }
      bool is_crash = false;
      bool flag1 = false;
      bool flag2 = false;
      foreach (Event @event in this.events)
      {
        EventType eventType = @event.event_type;
        switch (eventType)
        {
          case EventType.RestartRequested:
            flag2 = true;
            continue;
          case EventType.Deactivated:
            if ((this.FindMod(@event.mod).available_content & (Content.Strings | Content.DLL | Content.Translation | Content.Animation)) != (Content) 0)
            {
              flag2 = true;
              continue;
            }
            continue;
          default:
            if (eventType != EventType.LoadError)
            {
              if (eventType == EventType.ActiveDuringCrash)
              {
                is_crash = true;
                continue;
              }
              continue;
            }
            flag1 = true;
            continue;
        }
      }
      bool flag3 = is_crash || flag1 || flag2;
      bool flag4 = this.IsInDevMode();
      if (flag3 && flag4)
        this.DevRestartDialog(parent, is_crash);
      else if (flag1)
        this.LoadFailureDialog(parent);
      else if (is_crash)
        this.RestartDialog((string) UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.MOD_ERRORS_ON_BOOT.MESSAGE, (System.Action) null, false, parent, (string) null);
      else if (flag3)
        this.RestartDialog((string) UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.RESTART.MESSAGE, (System.Action) null, true, parent, (string) null);
      else
        this.NotifyDialog((string) UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.TITLE, (string) (!flag4 ? UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.MESSAGE : UI.FRONTEND.MOD_DIALOGS.MOD_EVENTS.DEV_MESSAGE), parent);
    }

    public bool Save()
    {
      if (!FileUtil.CreateDirectory(Manager.GetDirectory(), 5))
        return false;
      using (FileStream stream = FileUtil.Create(this.GetFilename(), 5))
      {
        if (stream == null)
          return false;
        using (StreamWriter streamWriter = FileUtil.DoIODialog<StreamWriter>((Func<StreamWriter>) (() => new StreamWriter((Stream) stream)), this.GetFilename(), (StreamWriter) null, 5))
        {
          if (streamWriter == null)
            return false;
          string str = JsonConvert.SerializeObject((object) new Manager.PersistentData(this.current_version, this.mods), Formatting.Indented);
          streamWriter.Write(str);
        }
      }
      return true;
    }

    public Mod FindMod(Label label)
    {
      foreach (Mod mod in this.mods)
      {
        if (mod.label.Equals((object) label))
          return mod;
      }
      return (Mod) null;
    }

    public bool IsModEnabled(Label id)
    {
      Mod mod = this.FindMod(id);
      if (mod != null)
        return mod.enabled;
      return false;
    }

    public bool EnableMod(Label id, bool enabled, object caller)
    {
      Mod mod = this.FindMod(id);
      if (mod == null || mod.enabled == enabled)
        return false;
      mod.enabled = enabled;
      if (enabled)
        mod.Load(Content.LayerableFiles);
      else
        mod.Unload(Content.LayerableFiles);
      this.dirty = true;
      this.Update(caller);
      return true;
    }

    public void Reinsert(int source_index, int target_index, object caller)
    {
      DebugUtil.Assert(source_index != target_index);
      if (source_index < -1 || this.mods.Count <= source_index || (target_index < -1 || this.mods.Count < target_index))
        return;
      Mod mod = this.mods[source_index];
      this.mods.RemoveAt(source_index);
      if (source_index < target_index)
        --target_index;
      if (target_index == this.mods.Count)
        this.mods.Add(mod);
      else
        this.mods.Insert(target_index, mod);
      this.dirty = true;
      this.Update(caller);
    }

    public void SendMetricsEvent()
    {
      ListPool<string, Manager>.PooledList pooledList = ListPool<string, Manager>.Allocate();
      foreach (Mod mod in this.mods)
      {
        if (mod.enabled)
          pooledList.Add(mod.title);
      }
      DictionaryPool<string, object, Manager>.PooledDictionary pooledDictionary = DictionaryPool<string, object, Manager>.Allocate();
      pooledDictionary["ModCount"] = (object) pooledList.Count;
      pooledDictionary["Mods"] = (object) pooledList;
      ThreadedHttps<KleiMetrics>.Instance.SendEvent((Dictionary<string, object>) pooledDictionary);
      pooledDictionary.Recycle();
      pooledList.Recycle();
      KCrashReporter.haveActiveMods = pooledList.Count > 0;
    }

    public delegate void OnUpdate(object change_source);

    private class PersistentData
    {
      public int version;
      public List<Mod> mods;

      public PersistentData()
      {
      }

      public PersistentData(int version, List<Mod> mods)
      {
        this.version = version;
        this.mods = mods;
      }
    }
  }
}
