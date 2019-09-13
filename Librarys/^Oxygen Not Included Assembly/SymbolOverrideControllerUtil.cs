// Decompiled with JetBrains decompiler
// Type: SymbolOverrideControllerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class SymbolOverrideControllerUtil
{
  public static SymbolOverrideController AddToPrefab(GameObject prefab)
  {
    SymbolOverrideController overrideController = prefab.AddComponent<SymbolOverrideController>();
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    DebugUtil.Assert((Object) overrideController != (Object) null, "SymbolOverrideController must be added after a KBatchedAnimController component.");
    component.usingNewSymbolOverrideSystem = true;
    return overrideController;
  }

  public static void AddBuildOverride(
    this SymbolOverrideController symbol_override_controller,
    KAnimFileData anim_file_data,
    int priority = 0)
  {
    for (int index = 0; index < anim_file_data.build.symbols.Length; ++index)
    {
      KAnim.Build.Symbol symbol = anim_file_data.build.symbols[index];
      symbol_override_controller.AddSymbolOverride(new HashedString(symbol.hash.HashValue), symbol, priority);
    }
  }

  public static void RemoveBuildOverride(
    this SymbolOverrideController symbol_override_controller,
    KAnimFileData anim_file_data,
    int priority = 0)
  {
    for (int index = 0; index < anim_file_data.build.symbols.Length; ++index)
    {
      KAnim.Build.Symbol symbol = anim_file_data.build.symbols[index];
      symbol_override_controller.RemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
    }
  }

  public static void TryRemoveBuildOverride(
    this SymbolOverrideController symbol_override_controller,
    KAnimFileData anim_file_data,
    int priority = 0)
  {
    for (int index = 0; index < anim_file_data.build.symbols.Length; ++index)
    {
      KAnim.Build.Symbol symbol = anim_file_data.build.symbols[index];
      symbol_override_controller.TryRemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
    }
  }

  public static void TryRemoveSymbolOverride(
    this SymbolOverrideController symbol_override_controller,
    HashedString target_symbol,
    int priority = 0)
  {
    if (symbol_override_controller.GetSymbolOverrideIdx(target_symbol, priority) < 0)
      return;
    symbol_override_controller.RemoveSymbolOverride(target_symbol, priority);
  }

  public static void ApplySymbolOverridesByAffix(
    this SymbolOverrideController symbol_override_controller,
    KAnimFile anim_file,
    string prefix = null,
    string postfix = null,
    int priority = 0)
  {
    for (int index = 0; index < anim_file.GetData().build.symbols.Length; ++index)
    {
      KAnim.Build.Symbol symbol = anim_file.GetData().build.symbols[index];
      string str = HashCache.Get().Get(symbol.hash);
      if (prefix != null && str.StartsWith(prefix))
        symbol_override_controller.AddSymbolOverride((HashedString) str.Substring(prefix.Length, str.Length - prefix.Length), symbol, priority);
      else if (postfix != null && str.EndsWith(postfix))
        symbol_override_controller.AddSymbolOverride((HashedString) str.Substring(0, str.Length - postfix.Length), symbol, priority);
    }
  }
}
