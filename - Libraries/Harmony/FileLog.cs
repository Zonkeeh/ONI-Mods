// Decompiled with JetBrains decompiler
// Type: Harmony.FileLog
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Harmony
{
  public static class FileLog
  {
    public static char indentChar = '\t';
    public static int indentLevel = 0;
    private static List<string> buffer = new List<string>();
    public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar.ToString() + "harmony.log.txt";

    [UpgradeToLatestVersion(1)]
    static FileLog()
    {
    }

    private static string IndentString()
    {
      return new string(FileLog.indentChar, FileLog.indentLevel);
    }

    public static void ChangeIndent(int delta)
    {
      FileLog.indentLevel = Math.Max(0, FileLog.indentLevel + delta);
    }

    public static void LogBuffered(string str)
    {
      lock (FileLog.logPath)
        FileLog.buffer.Add(FileLog.IndentString() + str);
    }

    public static void FlushBuffer()
    {
      lock (FileLog.logPath)
      {
        if (FileLog.buffer.Count <= 0)
          return;
        using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
        {
          foreach (string str in FileLog.buffer)
            streamWriter.WriteLine(str);
        }
        FileLog.buffer.Clear();
      }
    }

    public static void Log(string str)
    {
      lock (FileLog.logPath)
      {
        using (StreamWriter streamWriter = File.AppendText(FileLog.logPath))
          streamWriter.WriteLine(FileLog.IndentString() + str);
      }
    }

    public static void Reset()
    {
      lock (FileLog.logPath)
        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar.ToString() + "harmony.log.txt");
    }

    public static unsafe void LogBytes(long ptr, int len)
    {
      lock (FileLog.logPath)
      {
        byte* numPtr = (byte*) ptr;
        string str = "";
        for (int index = 1; index <= len; ++index)
        {
          if (str == "")
            str = "#  ";
          str = str + numPtr->ToString("X2") + " ";
          if (index > 1 || len == 1)
          {
            if (index % 8 == 0 || index == len)
            {
              FileLog.Log(str);
              str = "";
            }
            else if (index % 4 == 0)
              str += " ";
          }
          ++numPtr;
        }
        byte[] numArray = new byte[len];
        Marshal.Copy((IntPtr) ptr, numArray, 0, len);
        byte[] hash = MD5.Create().ComputeHash(numArray);
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("X2"));
        FileLog.Log("HASH: " + (object) stringBuilder);
      }
    }
  }
}
