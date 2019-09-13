// Decompiled with JetBrains decompiler
// Type: KMod.ZipFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Ionic.Zip;
using Klei;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KMod
{
  internal struct ZipFile : IFileSource
  {
    private string filename;
    private Ionic.Zip.ZipFile zipfile;
    private ZipFileDirectory file_system;

    public ZipFile(string filename)
    {
      this.filename = filename;
      this.zipfile = Ionic.Zip.ZipFile.Read(filename);
      this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath);
    }

    public string GetRoot()
    {
      return this.filename;
    }

    public bool Exists()
    {
      return File.Exists(this.GetRoot());
    }

    public void GetTopLevelItems(List<FileSystemItem> file_system_items)
    {
      HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
      foreach (ZipEntry zipEntry in this.zipfile)
      {
        string[] strArray = FileSystem.Normalize(zipEntry.FileName).Split('/');
        string str = strArray[0];
        if (pooledHashSet.Add(str))
          file_system_items.Add(new FileSystemItem()
          {
            name = str,
            type = 1 >= strArray.Length ? FileSystemItem.ItemType.File : FileSystemItem.ItemType.Directory
          });
      }
      pooledHashSet.Recycle();
    }

    public IFileDirectory GetFileSystem()
    {
      return (IFileDirectory) this.file_system;
    }

    public void CopyTo(string path, List<string> extensions = null)
    {
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) this.zipfile.Entries)
      {
        bool flag = extensions == null || extensions.Count == 0;
        if (extensions != null)
        {
          foreach (string extension in extensions)
          {
            if (entry.FileName.ToLower().EndsWith(extension))
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
        {
          string str = FileSystem.Normalize(System.IO.Path.Combine(path, entry.FileName));
          string directoryName = System.IO.Path.GetDirectoryName(str);
          if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
          {
            using (MemoryStream memoryStream = new MemoryStream((int) entry.UncompressedSize))
            {
              entry.Extract((Stream) memoryStream);
              using (FileStream fileStream = FileUtil.Create(str, 0))
                fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
            }
          }
        }
      }
    }

    public string Read(string relative_path)
    {
      ICollection<ZipEntry> zipEntries = this.zipfile.SelectEntries(relative_path);
      if (zipEntries.Count == 0)
        return string.Empty;
      using (IEnumerator<ZipEntry> enumerator = zipEntries.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          ZipEntry current = enumerator.Current;
          using (MemoryStream memoryStream = new MemoryStream((int) current.UncompressedSize))
          {
            current.Extract((Stream) memoryStream);
            return Encoding.UTF8.GetString(memoryStream.GetBuffer());
          }
        }
      }
      return string.Empty;
    }
  }
}
