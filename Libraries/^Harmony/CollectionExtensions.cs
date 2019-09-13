// Decompiled with JetBrains decompiler
// Type: Harmony.CollectionExtensions
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Harmony
{
  public static class CollectionExtensions
  {
    public static void Do<T>(this IEnumerable<T> sequence, Action<T> action)
    {
      if (sequence == null)
        return;
      IEnumerator<T> enumerator = sequence.GetEnumerator();
      while (enumerator.MoveNext())
        action(enumerator.Current);
    }

    public static void DoIf<T>(
      this IEnumerable<T> sequence,
      Func<T, bool> condition,
      Action<T> action)
    {
      sequence.Where<T>(condition).Do<T>(action);
    }

    public static IEnumerable<T> Add<T>(this IEnumerable<T> sequence, T item)
    {
      return (sequence ?? Enumerable.Empty<T>()).Concat<T>((IEnumerable<T>) new T[1]
      {
        item
      });
    }

    public static T[] AddRangeToArray<T>(this T[] sequence, T[] items)
    {
      return ((IEnumerable<T>) sequence ?? Enumerable.Empty<T>()).Concat<T>((IEnumerable<T>) items).ToArray<T>();
    }

    public static T[] AddToArray<T>(this T[] sequence, T item)
    {
      return ((IEnumerable<T>) sequence).Add<T>(item).ToArray<T>();
    }
  }
}
