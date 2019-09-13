// Decompiled with JetBrains decompiler
// Type: PathFinderQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PathFinderQuery
{
  protected int resultCell;
  private NavType resultNavType;

  public virtual bool IsMatch(int cell, int parent_cell, int cost)
  {
    return true;
  }

  public void SetResult(int cell, int cost, NavType nav_type)
  {
    this.resultCell = cell;
    this.resultNavType = nav_type;
  }

  public void ClearResult()
  {
    this.resultCell = -1;
  }

  public virtual int GetResultCell()
  {
    return this.resultCell;
  }

  public NavType GetResultNavType()
  {
    return this.resultNavType;
  }
}
