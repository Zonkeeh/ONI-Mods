// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.Memory
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Harmony.ILCopying
{
  public static class Memory
  {
    private static readonly HashSet<PlatformID> WindowsPlatformIDSet = new HashSet<PlatformID>()
    {
      PlatformID.Win32NT,
      PlatformID.Win32S,
      PlatformID.Win32Windows,
      PlatformID.WinCE
    };

    public static bool IsWindows
    {
      get
      {
        return Memory.WindowsPlatformIDSet.Contains(Environment.OSVersion.Platform);
      }
    }

    [DllImport("kernel32.dll")]
    public static extern bool VirtualProtect(
      IntPtr lpAddress,
      UIntPtr dwSize,
      Protection flNewProtect,
      out Protection lpflOldProtect);

    public static void UnprotectMemoryPage(long memory)
    {
      Protection lpflOldProtect;
      if (Memory.IsWindows && !Memory.VirtualProtect(new IntPtr(memory), new UIntPtr(1U), Protection.PAGE_EXECUTE_READWRITE, out lpflOldProtect))
        throw new Win32Exception();
    }

    public static string DetourMethod(MethodBase original, MethodBase replacement)
    {
      Exception exception;
      long methodStart1 = Memory.GetMethodStart(original, out exception);
      if (methodStart1 == 0L)
        return exception.Message;
      long methodStart2 = Memory.GetMethodStart(replacement, out exception);
      if (methodStart2 == 0L)
        return exception.Message;
      return Memory.WriteJump(methodStart1, methodStart2);
    }

    public static string WriteJump(long memory, long destination)
    {
      Memory.UnprotectMemoryPage(memory);
      if (IntPtr.Size == 8)
      {
        if (Memory.CompareBytes(memory, new byte[1]
        {
          (byte) 233
        }))
        {
          int num = Memory.ReadInt(memory + 1L);
          memory += (long) (5 + num);
        }
        memory = Memory.WriteBytes(memory, new byte[2]
        {
          (byte) 72,
          (byte) 184
        });
        memory = Memory.WriteLong(memory, destination);
        memory = Memory.WriteBytes(memory, new byte[2]
        {
          byte.MaxValue,
          (byte) 224
        });
      }
      else
      {
        memory = Memory.WriteByte(memory, (byte) 104);
        memory = Memory.WriteInt(memory, (int) destination);
        memory = Memory.WriteByte(memory, (byte) 195);
      }
      return (string) null;
    }

    private static RuntimeMethodHandle GetRuntimeMethodHandle(MethodBase method)
    {
      if (!(method is DynamicMethod))
        return method.MethodHandle;
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
      MethodInfo method1 = typeof (DynamicMethod).GetMethod("GetMethodDescriptor", bindingAttr);
      if (method1 != null)
        return (RuntimeMethodHandle) method1.Invoke((object) method, new object[0]);
      FieldInfo field = typeof (DynamicMethod).GetField("m_method", bindingAttr);
      if (field != null)
        return (RuntimeMethodHandle) field.GetValue((object) method);
      return (RuntimeMethodHandle) typeof (DynamicMethod).GetField("mhandle", bindingAttr).GetValue((object) method);
    }

    public static long GetMethodStart(MethodBase method, out Exception exception)
    {
      RuntimeMethodHandle runtimeMethodHandle = Memory.GetRuntimeMethodHandle(method);
      try
      {
        RuntimeHelpers.PrepareMethod(runtimeMethodHandle);
      }
      catch (Exception ex)
      {
      }
      try
      {
        exception = (Exception) null;
        return runtimeMethodHandle.GetFunctionPointer().ToInt64();
      }
      catch (Exception ex)
      {
        exception = ex;
        return 0;
      }
    }

    public static unsafe bool CompareBytes(long memory, byte[] values)
    {
      byte* numPtr = (byte*) memory;
      foreach (int num in values)
      {
        if (num != (int) *numPtr)
          return false;
        ++numPtr;
      }
      return true;
    }

    public static unsafe byte ReadByte(long memory)
    {
      return *(byte*) memory;
    }

    public static unsafe int ReadInt(long memory)
    {
      return *(int*) memory;
    }

    public static unsafe long ReadLong(long memory)
    {
      return *(long*) memory;
    }

    public static unsafe long WriteByte(long memory, byte value)
    {
      *(byte*) memory = value;
      return memory + 1L;
    }

    public static long WriteBytes(long memory, byte[] values)
    {
      foreach (byte num in values)
        memory = Memory.WriteByte(memory, num);
      return memory;
    }

    public static unsafe long WriteInt(long memory, int value)
    {
      *(int*) memory = value;
      return memory + 4L;
    }

    public static unsafe long WriteLong(long memory, long value)
    {
      *(long*) memory = value;
      return memory + 8L;
    }
  }
}
