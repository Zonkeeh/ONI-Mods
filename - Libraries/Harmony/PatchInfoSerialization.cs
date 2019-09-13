// Decompiled with JetBrains decompiler
// Type: Harmony.PatchInfoSerialization
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Harmony
{
  public static class PatchInfoSerialization
  {
    public static byte[] Serialize(this PatchInfo patchInfo)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) memoryStream, (object) patchInfo);
        return memoryStream.GetBuffer();
      }
    }

    public static PatchInfo Deserialize(byte[] bytes)
    {
      return (PatchInfo) new BinaryFormatter()
      {
        Binder = ((SerializationBinder) new PatchInfoSerialization.Binder())
      }.Deserialize((Stream) new MemoryStream(bytes));
    }

    public static int PriorityComparer(
      object obj,
      int index,
      int priority,
      string[] before,
      string[] after)
    {
      Traverse traverse = Traverse.Create(obj);
      string str = traverse.Field("owner").GetValue<string>();
      int num1 = traverse.Field(nameof (priority)).GetValue<int>();
      int num2 = traverse.Field(nameof (index)).GetValue<int>();
      if (before != null && Array.IndexOf<string>(before, str) > -1)
        return -1;
      if (after != null && Array.IndexOf<string>(after, str) > -1)
        return 1;
      if (priority != num1)
        return -priority.CompareTo(num1);
      return index.CompareTo(num2);
    }

    private class Binder : SerializationBinder
    {
      public override Type BindToType(string assemblyName, string typeName)
      {
        Type[] typeArray = new Type[3]
        {
          typeof (PatchInfo),
          typeof (Patch[]),
          typeof (Patch)
        };
        foreach (Type type in typeArray)
        {
          if (typeName == type.FullName)
            return type;
        }
        return Type.GetType(string.Format("{0}, {1}", (object) typeName, (object) assemblyName));
      }
    }
  }
}
