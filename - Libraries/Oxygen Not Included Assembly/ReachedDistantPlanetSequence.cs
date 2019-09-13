// Decompiled with JetBrains decompiler
// Type: ReachedDistantPlanetSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

public static class ReachedDistantPlanetSequence
{
  public static void Start(KMonoBehaviour controller)
  {
    controller.StartCoroutine(ReachedDistantPlanetSequence.Sequence());
  }

  [DebuggerHidden]
  private static IEnumerator Sequence()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ReachedDistantPlanetSequence.\u003CSequence\u003Ec__Iterator0 sequenceCIterator0 = new ReachedDistantPlanetSequence.\u003CSequence\u003Ec__Iterator0();
    return (IEnumerator) sequenceCIterator0;
  }
}
