// Decompiled with JetBrains decompiler
// Type: SymbolOverrideController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SymbolOverrideController : KMonoBehaviour
{
  [SerializeField]
  private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();
  public bool applySymbolOverridesEveryFrame;
  private KAnimBatch.AtlasList atlases;
  private KBatchedAnimController animController;
  private FaceGraph faceGraph;
  private bool requiresSorting;

  public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
  {
    get
    {
      return this.symbolOverrides.ToArray();
    }
  }

  public int version { get; private set; }

  protected override void OnPrefabInit()
  {
    this.animController = this.GetComponent<KBatchedAnimController>();
    DebugUtil.Assert((UnityEngine.Object) this.GetComponent<KBatchedAnimController>() != (UnityEngine.Object) null, "SymbolOverrideController requires KBatchedAnimController");
    DebugUtil.Assert(this.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      symbolOverride.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolOverride.sourceSymbolBatchTag).GetSymbol((KAnimHashedString) symbolOverride.sourceSymbolId);
      this.symbolOverrides[index] = symbolOverride;
    }
    this.atlases = new KAnimBatch.AtlasList(0);
    this.faceGraph = this.GetComponent<FaceGraph>();
  }

  public void AddSymbolOverride(
    HashedString target_symbol,
    KAnim.Build.Symbol source_symbol,
    int priority = 0)
  {
    if (source_symbol == null)
      throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
    SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry()
    {
      targetSymbol = target_symbol,
      sourceSymbol = source_symbol,
      sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
      sourceSymbolBatchTag = source_symbol.build.batchTag,
      priority = priority
    };
    int symbolOverrideIdx = this.GetSymbolOverrideIdx(target_symbol, priority);
    if (symbolOverrideIdx >= 0)
      this.symbolOverrides[symbolOverrideIdx] = symbolEntry;
    else
      this.symbolOverrides.Add(symbolEntry);
    this.MarkDirty();
  }

  public void RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
  {
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      if (symbolOverride.targetSymbol == target_symbol && symbolOverride.priority == priority)
      {
        this.symbolOverrides.RemoveAt(index);
        break;
      }
    }
    this.MarkDirty();
  }

  public void RemoveAllSymbolOverrides(int priority = 0)
  {
    this.symbolOverrides.RemoveAll((Predicate<SymbolOverrideController.SymbolEntry>) (x => x.priority >= priority));
    this.MarkDirty();
  }

  public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
  {
    for (int index = 0; index < this.symbolOverrides.Count; ++index)
    {
      SymbolOverrideController.SymbolEntry symbolOverride = this.symbolOverrides[index];
      if (symbolOverride.targetSymbol == target_symbol && symbolOverride.priority == priority)
        return index;
    }
    return -1;
  }

  public int GetAtlasIdx(Texture2D atlas)
  {
    return this.atlases.GetAtlasIdx(atlas);
  }

  public void ApplyOverrides()
  {
    if (this.requiresSorting)
    {
      this.symbolOverrides.Sort((Comparison<SymbolOverrideController.SymbolEntry>) ((x, y) => x.priority - y.priority));
      this.requiresSorting = false;
    }
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    KAnimBatch batch = component.GetBatch();
    DebugUtil.Assert(batch != null);
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID);
    this.atlases.Clear(batch.atlases.Count);
    ListPool<SymbolOverrideController.SymbolToOverride, SymbolOverrideController>.PooledList pooledList1 = ListPool<SymbolOverrideController.SymbolToOverride, SymbolOverrideController>.Allocate();
    ListPool<SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledList pooledList2 = ListPool<SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
    foreach (SymbolOverrideController.SymbolEntry symbolOverride in this.symbolOverrides)
    {
      SymbolOverrideController.BatchGroupInfo batchGroupInfo1 = new SymbolOverrideController.BatchGroupInfo();
      foreach (SymbolOverrideController.BatchGroupInfo batchGroupInfo2 in (List<SymbolOverrideController.BatchGroupInfo>) pooledList2)
      {
        if (symbolOverride.sourceSymbol.build == batchGroupInfo2.build)
          batchGroupInfo1 = batchGroupInfo2;
      }
      if (batchGroupInfo1.build == null)
      {
        batchGroupInfo1 = new SymbolOverrideController.BatchGroupInfo()
        {
          build = symbolOverride.sourceSymbol.build,
          data = KAnimBatchManager.Instance().GetBatchGroupData(symbolOverride.sourceSymbol.build.batchTag)
        };
        batchGroupInfo1.atlasIdx = this.atlases.Add(symbolOverride.sourceSymbol.build.GetTexture(0));
        pooledList2.Add(batchGroupInfo1);
      }
      pooledList1.Add(new SymbolOverrideController.SymbolToOverride()
      {
        sourceSymbol = symbolOverride.sourceSymbol,
        targetSymbol = symbolOverride.targetSymbol,
        data = batchGroupInfo1.data,
        atlasIdx = batchGroupInfo1.atlasIdx
      });
    }
    pooledList2.Recycle();
    foreach (SymbolOverrideController.SymbolToOverride symbolToOverride in (List<SymbolOverrideController.SymbolToOverride>) pooledList1)
    {
      KAnim.Build.Symbol symbol = batchGroupData.GetSymbol((KAnimHashedString) symbolToOverride.targetSymbol);
      if (symbol != null)
      {
        KAnim.Build.Symbol sourceSymbol = symbolToOverride.sourceSymbol;
        for (int val2 = 0; val2 < symbol.numFrames; ++val2)
        {
          int num = Math.Min(sourceSymbol.numFrames - 1, val2);
          KAnim.Build.SymbolFrameInstance symbolFrameInstance = symbolToOverride.data.symbolFrameInstances[sourceSymbol.firstFrameIdx + num];
          symbolFrameInstance.buildImageIdx = symbolToOverride.atlasIdx;
          component.SetSymbolOverride(symbol.firstFrameIdx + val2, symbolFrameInstance);
        }
      }
    }
    pooledList1.Recycle();
    if (!((UnityEngine.Object) this.faceGraph != (UnityEngine.Object) null))
      return;
    this.faceGraph.ApplyShape();
  }

  public void ApplyAtlases()
  {
    this.atlases.Apply(this.animController.GetBatch().matProperties);
  }

  public void MarkDirty()
  {
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
      this.animController.SetDirty();
    ++this.version;
    this.requiresSorting = true;
  }

  [Serializable]
  public struct SymbolEntry
  {
    public HashedString targetSymbol;
    [NonSerialized]
    public KAnim.Build.Symbol sourceSymbol;
    public HashedString sourceSymbolId;
    public HashedString sourceSymbolBatchTag;
    public int priority;
  }

  private struct SymbolToOverride
  {
    public KAnim.Build.Symbol sourceSymbol;
    public HashedString targetSymbol;
    public KBatchGroupData data;
    public int atlasIdx;
  }

  private struct BatchGroupInfo
  {
    public KAnim.Build build;
    public int atlasIdx;
    public KBatchGroupData data;
  }
}
