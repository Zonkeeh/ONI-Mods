// Decompiled with JetBrains decompiler
// Type: KMod.IFileSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;

namespace KMod
{
  public interface IFileSource
  {
    string GetRoot();

    bool Exists();

    void GetTopLevelItems(List<FileSystemItem> file_system_items);

    IFileDirectory GetFileSystem();

    void CopyTo(string path, List<string> extensions = null);

    string Read(string relative_path);
  }
}
