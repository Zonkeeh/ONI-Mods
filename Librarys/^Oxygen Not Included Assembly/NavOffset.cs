// Decompiled with JetBrains decompiler
// Type: NavOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct NavOffset
{
  public NavType navType;
  public CellOffset offset;

  public NavOffset(NavType nav_type, int x, int y)
  {
    this.navType = nav_type;
    this.offset.x = x;
    this.offset.y = y;
  }
}
