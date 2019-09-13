// Decompiled with JetBrains decompiler
// Type: KMod.Mod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KMod
{
  [JsonObject(MemberSerialization.OptIn)]
  [DebuggerDisplay("{title}")]
  public class Mod
  {
    [JsonProperty]
    public Label label;
    [JsonProperty]
    public Mod.Status status;
    [JsonProperty]
    public bool enabled;
    [JsonProperty]
    public int crash_count;
    [JsonProperty]
    public string reinstall_path;
    public IFileSource file_source;
    public bool is_subscribed;
    public const int MAX_CRASH_COUNT = 3;

    [JsonConstructor]
    public Mod()
    {
    }

    public Mod(
      Label label,
      string description,
      IFileSource file_source,
      LocString manage_tooltip,
      System.Action on_managed)
    {
      this.enabled = false;
      this.label = label;
      this.status = Mod.Status.NotInstalled;
      this.description = description;
      this.file_source = file_source;
      this.manage_tooltip = manage_tooltip;
      this.on_managed = on_managed;
      this.loaded_content = (Content) 0;
      this.available_content = (Content) 0;
      this.ScanContent();
    }

    public Content available_content { get; private set; }

    public LocString manage_tooltip { get; private set; }

    public System.Action on_managed { get; private set; }

    public bool is_managed
    {
      get
      {
        return this.manage_tooltip != null;
      }
    }

    public string title
    {
      get
      {
        return this.label.title;
      }
    }

    public string description { get; private set; }

    public Content loaded_content { get; private set; }

    public void CopyPersistentDataTo(Mod other_mod)
    {
      other_mod.status = this.status;
      other_mod.enabled = this.enabled;
      other_mod.crash_count = this.crash_count;
      other_mod.loaded_content = this.loaded_content;
      other_mod.reinstall_path = this.reinstall_path;
    }

    public void ScanContent()
    {
      this.available_content = (Content) 0;
      if (this.file_source == null)
        this.file_source = (IFileSource) new Directory(this.label.install_path);
      if (!this.file_source.Exists())
        return;
      List<FileSystemItem> file_system_items = new List<FileSystemItem>();
      this.file_source.GetTopLevelItems(file_system_items);
      foreach (FileSystemItem fileSystemItem in file_system_items)
      {
        if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
          this.AddDirectory(fileSystemItem.name.ToLower());
        else
          this.AddFile(fileSystemItem.name.ToLower());
      }
    }

    public bool IsEmpty()
    {
      return this.available_content == (Content) 0;
    }

    private void AddDirectory(string directory)
    {
      switch (directory.TrimEnd('/'))
      {
        case "strings":
          this.available_content |= Content.Strings;
          break;
        case "codex":
          this.available_content |= Content.LayerableFiles;
          break;
        case "elements":
          this.available_content |= Content.LayerableFiles;
          break;
        case "templates":
          this.available_content |= Content.LayerableFiles;
          break;
        case "worldgen":
          this.available_content |= Content.LayerableFiles;
          break;
        case "anim":
          this.available_content |= Content.Animation;
          break;
      }
    }

    private void AddFile(string file)
    {
      if (file.EndsWith(".dll"))
        this.available_content |= Content.DLL;
      if (!file.EndsWith(".po"))
        return;
      this.available_content |= Content.Translation;
    }

    private static void AccumulateExtensions(Content content, List<string> extensions)
    {
      if ((content & Content.DLL) != (Content) 0)
        extensions.Add(".dll");
      if ((content & (Content.Strings | Content.Translation)) == (Content) 0)
        return;
      extensions.Add(".po");
    }

    [Conditional("DEBUG")]
    private void Assert(bool condition, string failure_message)
    {
      if (string.IsNullOrEmpty(this.title))
        DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", (object) this.title, (object) this.label.ToString(), (object) failure_message));
      else
        DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", (object) this.label.ToString(), (object) failure_message));
    }

    public void Install()
    {
      if (this.label.distribution_platform == Label.DistributionPlatform.Local || this.label.distribution_platform == Label.DistributionPlatform.Dev)
      {
        this.status = Mod.Status.Installed;
      }
      else
      {
        this.status = Mod.Status.ReinstallPending;
        if (this.file_source == null || !FileUtil.DeleteDirectory(this.label.install_path, 0) || !FileUtil.CreateDirectory(this.label.install_path, 0))
          return;
        this.file_source.CopyTo(this.label.install_path, (List<string>) null);
        this.file_source = (IFileSource) new Directory(this.label.install_path);
        this.status = Mod.Status.Installed;
      }
    }

    public bool Uninstall()
    {
      this.enabled = false;
      if (this.loaded_content != (Content) 0)
      {
        Debug.Log((object) string.Format("Can't uninstall {0}: still has loaded content: {1}", (object) this.label.ToString(), (object) this.loaded_content.ToString()));
        this.status = Mod.Status.UninstallPending;
        return false;
      }
      if (this.label.distribution_platform != Label.DistributionPlatform.Local && this.label.distribution_platform != Label.DistributionPlatform.Dev && !FileUtil.DeleteDirectory(this.label.install_path, 0))
      {
        Debug.Log((object) string.Format("Can't uninstall {0}: directory deletion failed", (object) this.label.ToString()));
        this.status = Mod.Status.UninstallPending;
        return false;
      }
      this.status = Mod.Status.NotInstalled;
      return true;
    }

    private bool LoadStrings()
    {
      string path = FileSystem.Normalize(System.IO.Path.Combine(this.label.install_path, "strings"));
      if (!System.IO.Directory.Exists(path))
        return false;
      int num = 0;
      foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
      {
        if (!(file.Extension.ToLower() != ".po"))
        {
          ++num;
          Localization.OverloadStrings(Localization.LoadStringsFile(file.FullName, false));
        }
      }
      return true;
    }

    private bool LoadTranslations()
    {
      string path = FileSystem.Normalize(this.label.install_path);
      if (!System.IO.Directory.Exists(path))
        return false;
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      HashSetPool<Localization.Locale, Mod>.PooledHashSet source = HashSetPool<Localization.Locale, Mod>.Allocate();
      foreach (FileInfo file in directoryInfo.GetFiles())
      {
        if (!(file.Extension.ToLower() != ".po"))
        {
          string[] lines = File.ReadAllLines(file.FullName, Encoding.UTF8);
          source.Add(Localization.GetLocale(lines));
          Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, false));
        }
      }
      if (source.Count == 0)
        return false;
      Localization.Locale new_locale = source.First<Localization.Locale>();
      if (!source.All<Localization.Locale>((Func<Localization.Locale, bool>) (locale => locale == new_locale)))
        return false;
      Localization.SetLocale(new_locale);
      Localization.SwapToLocalizedFont(new_locale.FontName);
      KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.UGC.ToString());
      KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, new_locale.Code);
      return true;
    }

    private bool LoadAnimation()
    {
      string path = FileSystem.Normalize(System.IO.Path.Combine(this.label.install_path, "anim"));
      if (!System.IO.Directory.Exists(path))
        return false;
      int num = 0;
      foreach (DirectoryInfo directory1 in new DirectoryInfo(path).GetDirectories())
      {
        foreach (DirectoryInfo directory2 in directory1.GetDirectories())
        {
          KAnimFile.Mod anim_mod = new KAnimFile.Mod();
          foreach (FileInfo file in directory2.GetFiles())
          {
            if (file.Extension == ".png")
            {
              byte[] data = File.ReadAllBytes(file.FullName);
              Texture2D tex = new Texture2D(2, 2);
              tex.LoadImage(data);
              anim_mod.textures.Add(tex);
            }
            else if (file.Extension == ".bytes")
            {
              string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(file.Name);
              byte[] numArray = File.ReadAllBytes(file.FullName);
              if (withoutExtension.EndsWith("_anim"))
                anim_mod.anim = numArray;
              else if (withoutExtension.EndsWith("_build"))
                anim_mod.build = numArray;
              else
                DebugUtil.LogWarningArgs((object) string.Format("Unhandled TextAsset ({0})...ignoring", (object) file.FullName));
            }
            else
              DebugUtil.LogWarningArgs((object) string.Format("Unhandled asset ({0})...ignoring", (object) file.FullName));
          }
          string name = directory2.Name + "_kanim";
          if (anim_mod.IsValid() && (bool) ((UnityEngine.Object) ModUtil.AddKAnimMod(name, anim_mod)))
            ++num;
        }
      }
      return true;
    }

    public void Load(Content content)
    {
      content &= this.available_content & ~this.loaded_content;
      if ((content & Content.Strings) != (Content) 0 && this.LoadStrings())
        this.loaded_content |= Content.Strings;
      if ((content & Content.Translation) != (Content) 0 && this.LoadTranslations())
        this.loaded_content |= Content.Translation;
      if ((content & Content.DLL) != (Content) 0 && DLLLoader.LoadDLLs(this.label.install_path))
        this.loaded_content |= Content.DLL;
      if ((content & Content.LayerableFiles) != (Content) 0)
      {
        FileSystem.file_sources.Insert(0, this.file_source.GetFileSystem());
        this.loaded_content |= Content.LayerableFiles;
      }
      if ((content & Content.Animation) == (Content) 0 || !this.LoadAnimation())
        return;
      this.loaded_content |= Content.Animation;
    }

    public void Unload(Content content)
    {
      content &= this.loaded_content;
      if ((content & Content.LayerableFiles) == (Content) 0)
        return;
      FileSystem.file_sources.Remove(this.file_source.GetFileSystem());
      this.loaded_content &= ~Content.LayerableFiles;
    }

    private void SetCrashCount(int new_crash_count)
    {
      this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
    }

    public void Crash(bool do_disable)
    {
      this.SetCrashCount(this.crash_count + 1);
      if (!do_disable)
        return;
      this.enabled = false;
    }

    public void Uncrash()
    {
      this.SetCrashCount(this.label.distribution_platform != Label.DistributionPlatform.Dev ? 0 : this.crash_count - 1);
    }

    public bool IsActive()
    {
      return this.loaded_content != (Content) 0;
    }

    public bool AllActive(Content content)
    {
      return (this.loaded_content & content) == content;
    }

    public bool AllActive()
    {
      return (this.loaded_content & this.available_content) == this.available_content;
    }

    public bool AnyActive(Content content)
    {
      return (this.loaded_content & content) != (Content) 0;
    }

    public bool HasContent()
    {
      return this.available_content != (Content) 0;
    }

    public bool HasAnyContent(Content content)
    {
      return (this.available_content & content) != (Content) 0;
    }

    public enum Status
    {
      NotInstalled,
      Installed,
      UninstallPending,
      ReinstallPending,
    }
  }
}
