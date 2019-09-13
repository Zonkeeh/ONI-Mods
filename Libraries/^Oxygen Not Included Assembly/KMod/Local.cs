// Decompiled with JetBrains decompiler
// Type: KMod.Local
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KMod
{
  public class Local : IDistributionPlatform
  {
    public Local(string folder, Label.DistributionPlatform distribution_platform)
    {
      this.folder = folder;
      this.distribution_platform = distribution_platform;
      DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
      if (!directoryInfo.Exists)
        return;
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        this.Subscribe(directory.Name, directory.LastWriteTime.ToFileTime(), (IFileSource) new Directory(directory.FullName));
    }

    public string folder { get; private set; }

    public Label.DistributionPlatform distribution_platform { get; private set; }

    public string GetDirectory()
    {
      return FileSystem.Normalize(System.IO.Path.Combine(Manager.GetDirectory(), this.folder));
    }

    private void Subscribe(string id, long timestamp, IFileSource file_source)
    {
      string readText = file_source.Read("mod.yaml");
      Local.Header header = !string.IsNullOrEmpty(readText) ? YamlIO.Parse<Local.Header>(readText, file_source.GetRoot() + "\\mod.yaml", (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null) : (Local.Header) null;
      if (header == null)
        header = new Local.Header()
        {
          title = id,
          description = id
        };
      Mod mod = new Mod(new Label()
      {
        id = id,
        distribution_platform = this.distribution_platform,
        version = (long) id.GetHashCode(),
        title = header.title
      }, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, (System.Action) (() => Application.OpenURL("file://" + file_source.GetRoot())));
      if (file_source.GetType() == typeof (Directory))
        mod.status = Mod.Status.Installed;
      Global.Instance.modManager.Subscribe(mod, (object) this);
    }

    private class Header
    {
      public string title { get; set; }

      public string description { get; set; }
    }
  }
}
