// Decompiled with JetBrains decompiler
// Type: SnapOn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class SnapOn : KMonoBehaviour
{
  public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();
  private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();
  private KAnimControllerBase kanimController;

  protected override void OnPrefabInit()
  {
    this.kanimController = this.GetComponent<KAnimControllerBase>();
  }

  protected override void OnSpawn()
  {
    foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
    {
      if (snapPoint.automatic)
        this.DoAttachSnapOn(snapPoint);
    }
  }

  public void AttachSnapOnByName(string name)
  {
    foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
    {
      if (snapPoint.pointName == name)
      {
        HashedString context = this.GetComponent<AnimEventHandler>().GetContext();
        if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
          this.DoAttachSnapOn(snapPoint);
      }
    }
  }

  public void DetachSnapOnByName(string name)
  {
    foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
    {
      if (snapPoint.pointName == name)
      {
        HashedString context = this.GetComponent<AnimEventHandler>().GetContext();
        if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
        {
          this.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
          this.kanimController.SetSymbolVisiblity((KAnimHashedString) snapPoint.overrideSymbol, false);
          break;
        }
      }
    }
  }

  private void DoAttachSnapOn(SnapOn.SnapPoint point)
  {
    SnapOn.OverrideEntry overrideEntry = (SnapOn.OverrideEntry) null;
    KAnimFile buildFile = point.buildFile;
    string symbol_name = string.Empty;
    if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
    {
      buildFile = overrideEntry.buildFile;
      symbol_name = overrideEntry.symbolName;
    }
    KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
    this.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
    this.kanimController.SetSymbolVisiblity((KAnimHashedString) point.overrideSymbol, true);
  }

  private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
  {
    KAnim.Build.Symbol symbol1 = anim_file.GetData().build.symbols[0];
    KAnimHashedString kanimHashedString = new KAnimHashedString(symbol_name);
    foreach (KAnim.Build.Symbol symbol2 in anim_file.GetData().build.symbols)
    {
      if (symbol2.hash == kanimHashedString)
      {
        symbol1 = symbol2;
        break;
      }
    }
    return symbol1;
  }

  public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
  {
    this.overrideMap[point_name] = new SnapOn.OverrideEntry()
    {
      buildFile = build_override,
      symbolName = symbol_name
    };
  }

  public void RemoveOverride(string point_name)
  {
    this.overrideMap.Remove(point_name);
  }

  [Serializable]
  public class SnapPoint
  {
    public bool automatic = true;
    public string pointName;
    public HashedString context;
    public KAnimFile buildFile;
    public HashedString overrideSymbol;
  }

  public class OverrideEntry
  {
    public KAnimFile buildFile;
    public string symbolName;
  }
}
