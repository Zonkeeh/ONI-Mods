// Decompiled with JetBrains decompiler
// Type: LightSymbolTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
  public HashedString targetSymbol;

  public void RenderEveryTick(float dt)
  {
    Vector3 zero = Vector3.zero;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    bool symbolVisible;
    this.GetComponent<Light2D>().Offset = (Vector2) ((component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out symbolVisible)).MultiplyPoint(Vector3.zero) - this.transform.GetPosition());
  }
}
