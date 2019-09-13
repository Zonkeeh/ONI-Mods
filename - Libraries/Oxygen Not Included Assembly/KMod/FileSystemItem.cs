// Decompiled with JetBrains decompiler
// Type: KMod.FileSystemItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace KMod
{
  public struct FileSystemItem
  {
    public string name;
    public FileSystemItem.ItemType type;

    public enum ItemType
    {
      Directory,
      File,
    }
  }
}
