// Decompiled with JetBrains decompiler
// Type: Harmony.SymbolExtensions
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Harmony
{
  public static class SymbolExtensions
  {
    public static MethodInfo GetMethodInfo(Expression<Action> expression)
    {
      return SymbolExtensions.GetMethodInfo((LambdaExpression) expression);
    }

    public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
    {
      return SymbolExtensions.GetMethodInfo((LambdaExpression) expression);
    }

    public static MethodInfo GetMethodInfo<T, TResult>(
      Expression<Func<T, TResult>> expression)
    {
      return SymbolExtensions.GetMethodInfo((LambdaExpression) expression);
    }

    public static MethodInfo GetMethodInfo(LambdaExpression expression)
    {
      MethodCallExpression body = expression.Body as MethodCallExpression;
      if (body == null)
        throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
      MethodInfo method = body.Method;
      if (method == null)
        throw new Exception("Cannot find method for expression " + (object) expression);
      return method;
    }
  }
}
