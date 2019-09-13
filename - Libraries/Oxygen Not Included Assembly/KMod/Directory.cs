// Decompiled with JetBrains decompiler
// Type: KMod.Directory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KMod
{
  internal struct Directory : IFileSource
  {
    private AliasDirectory file_system;
    private string root;

    public Directory(string root)
    {
      this.root = root;
      this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath);
    }

    public string GetRoot()
    {
      return this.root;
    }

    public bool Exists()
    {
      return System.IO.Directory.Exists(this.GetRoot());
    }

    public void GetTopLevelItems(List<FileSystemItem> file_system_items)
    {
      foreach (FileSystemInfo fileSystemInfo in new DirectoryInfo(this.root).GetFileSystemInfos())
        file_system_items.Add(new FileSystemItem()
        {
          name = fileSystemInfo.Name,
          type = !(fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.File : FileSystemItem.ItemType.Directory
        });
    }

    public IFileDirectory GetFileSystem()
    {
      return (IFileDirectory) this.file_system;
    }

    public void CopyTo(string path, List<string> extensions = null)
    {
      try
      {
        Directory.CopyDirectory(this.root, path, extensions);
      }
      catch (UnauthorizedAccessException ex)
      {
        FileUtil.ErrorDialog(string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, (object) path));
      }
      catch (IOException ex)
      {
        FileUtil.ErrorDialog(string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, (object) path));
      }
      catch (Exception ex)
      {
        FileUtil.ErrorDialog(string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, (object) ex.Message));
      }
    }

    public string Read(string relative_path)
    {
      try
      {
        using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
        {
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, (int) fileStream.Length);
          return Encoding.UTF8.GetString(numArray);
        }
      }
      catch
      {
        return string.Empty;
      }
    }

    private static int CopyDirectory(
      string sourceDirName,
      string destDirName,
      List<string> extensions)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
      if (!directoryInfo.Exists || !FileUtil.CreateDirectory(destDirName, 0))
        return 0;
      FileInfo[] files = directoryInfo.GetFiles();
      int num = 0;
      foreach (FileInfo fileInfo in files)
      {
        bool flag = extensions == null || extensions.Count == 0;
        if (extensions != null)
        {
          foreach (string extension in extensions)
          {
            if (extension == Path.GetExtension(fileInfo.Name).ToLower())
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
        {
          string destFileName = Path.Combine(destDirName, fileInfo.Name);
          fileInfo.CopyTo(destFileName, false);
          ++num;
        }
      }
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
      {
        string destDirName1 = Path.Combine(destDirName, directory.Name);
        num += Directory.CopyDirectory(directory.FullName, destDirName1, extensions);
      }
      if (num == 0)
        FileUtil.DeleteDirectory(destDirName, 0);
      return num;
    }
  }
}
