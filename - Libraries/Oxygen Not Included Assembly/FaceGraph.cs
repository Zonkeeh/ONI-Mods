// Decompiled with JetBrains decompiler
// Type: FaceGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class FaceGraph : KMonoBehaviour
{
  private static KAnimHashedString HASH_SNAPTO_EYES = (KAnimHashedString) "snapto_eyes";
  private static KAnimHashedString HASH_NEUTRAL = (KAnimHashedString) "neutral";
  private static int FIRST_SIDEWAYS_FRAME = 29;
  private List<Expression> expressions = new List<Expression>();

  public IEnumerator<Expression> GetEnumerator()
  {
    return (IEnumerator<Expression>) this.expressions.GetEnumerator();
  }

  public Expression overrideExpression { get; private set; }

  public Expression currentExpression { get; private set; }

  public void AddExpression(Expression expression)
  {
    if (this.expressions.Contains(expression))
      return;
    this.expressions.Add(expression);
    this.UpdateFace();
  }

  public void RemoveExpression(Expression expression)
  {
    if (!this.expressions.Remove(expression))
      return;
    this.UpdateFace();
  }

  public void SetOverrideExpression(Expression expression)
  {
    if (expression == this.overrideExpression)
      return;
    this.overrideExpression = expression;
    this.UpdateFace();
  }

  public void ApplyShape()
  {
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    Accessorizer component2 = this.GetComponent<Accessorizer>();
    KAnimFile anim = Assets.GetAnim((HashedString) "head_master_swap_kanim");
    bool should_use_sideways_symbol = this.ShouldUseSidewaysSymbol(component1);
    BlinkMonitor.Instance smi1 = component2.GetSMI<BlinkMonitor.Instance>();
    if (smi1.IsNullOrStopped() || !smi1.IsBlinking())
      this.ApplyShape(component2.GetAccessory(Db.Get().AccessorySlots.Eyes).symbol, component1, anim, (HashedString) "snapto_eyes", should_use_sideways_symbol);
    SpeechMonitor.Instance smi2 = component2.GetSMI<SpeechMonitor.Instance>();
    if (smi2.IsNullOrStopped() || !smi2.IsPlayingSpeech())
      this.ApplyShape(component2.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol, component1, anim, (HashedString) "snapto_mouth", should_use_sideways_symbol);
    else
      smi2.DrawMouth();
  }

  private bool ShouldUseSidewaysSymbol(KBatchedAnimController controller)
  {
    KAnim.Anim currentAnim = controller.GetCurrentAnim();
    if (currentAnim == null)
      return false;
    int currentFrameIndex = controller.GetCurrentFrameIndex();
    if (currentFrameIndex <= 0)
      return false;
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag);
    KAnim.Anim.Frame frame = batchGroupData.GetFrame(currentFrameIndex);
    for (int index = 0; index < frame.numElements; ++index)
    {
      KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + index);
      if (frameElement.symbol == FaceGraph.HASH_SNAPTO_EYES && frameElement.frame >= FaceGraph.FIRST_SIDEWAYS_FRAME)
        return true;
    }
    return false;
  }

  private void ApplyShape(
    KAnim.Build.Symbol variation_symbol,
    KBatchedAnimController controller,
    KAnimFile shapes_file,
    HashedString symbol_name_in_shape_file,
    bool should_use_sideways_symbol)
  {
    HashedString hash = (HashedString) FaceGraph.HASH_NEUTRAL;
    if (this.currentExpression != null)
      hash = this.currentExpression.face.hash;
    KAnim.Anim anim1 = (KAnim.Anim) null;
    KAnim.Anim.FrameElement frameElement = new KAnim.Anim.FrameElement();
    bool flag1 = false;
    bool flag2 = false;
    for (int index1 = 0; index1 < shapes_file.GetData().animCount && !flag1; ++index1)
    {
      KAnim.Anim anim2 = shapes_file.GetData().GetAnim(index1);
      if (anim2.hash == hash)
      {
        anim1 = anim2;
        KAnim.Anim.Frame frame = anim1.GetFrame(shapes_file.GetData().build.batchTag, 0);
        for (int index2 = 0; index2 < frame.numElements; ++index2)
        {
          frameElement = KAnimBatchManager.Instance().GetBatchGroupData(shapes_file.GetData().animBatchTag).GetFrameElement(frame.firstElementIdx + index2);
          if (!(frameElement.symbol != symbol_name_in_shape_file))
          {
            if (flag2 || !should_use_sideways_symbol)
              flag1 = true;
            flag2 = true;
            break;
          }
        }
      }
    }
    if (anim1 == null)
      DebugUtil.Assert(false, "Could not find shape for expression: " + HashCache.Get().Get(hash));
    if (!flag2)
      DebugUtil.Assert(false, "Could not find shape element for shape:" + HashCache.Get().Get(variation_symbol.hash));
    KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(controller.batchGroupID).GetSymbol((KAnimHashedString) symbol_name_in_shape_file);
    KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(variation_symbol.build.batchTag).symbolFrameInstances[variation_symbol.firstFrameIdx + frameElement.frame];
    symbolFrameInstance.buildImageIdx = this.GetComponent<SymbolOverrideController>().GetAtlasIdx(variation_symbol.build.GetTexture(0));
    controller.SetSymbolOverride(symbol.firstFrameIdx, symbolFrameInstance);
  }

  private void UpdateFace()
  {
    Expression expression = (Expression) null;
    if (this.overrideExpression != null)
      expression = this.overrideExpression;
    else if (this.expressions.Count > 0)
    {
      this.expressions.Sort((Comparison<Expression>) ((a, b) => b.priority.CompareTo(a.priority)));
      expression = this.expressions[0];
    }
    if (expression == this.currentExpression && expression != null)
      return;
    this.currentExpression = expression;
    this.GetComponent<SymbolOverrideController>().MarkDirty();
  }

  public Expression GetCurrentExpression()
  {
    return this.currentExpression;
  }
}
