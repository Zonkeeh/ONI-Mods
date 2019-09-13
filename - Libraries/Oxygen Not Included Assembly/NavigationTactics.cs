// Decompiled with JetBrains decompiler
// Type: NavigationTactics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class NavigationTactics
{
  public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);
  public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);
  public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);
}
