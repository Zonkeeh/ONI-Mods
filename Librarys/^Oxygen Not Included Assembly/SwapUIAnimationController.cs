// Decompiled with JetBrains decompiler
// Type: SwapUIAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SwapUIAnimationController : MonoBehaviour
{
  public GameObject AnimationControllerObject_Primary;
  public GameObject AnimationControllerObject_Alternate;

  public void SetState(bool Primary)
  {
    this.AnimationControllerObject_Primary.SetActive(Primary);
    if (!Primary)
    {
      this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = (Color32) new Color(1f, 1f, 1f, 0.5f);
      this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = (Color32) Color.clear;
    }
    this.AnimationControllerObject_Alternate.SetActive(!Primary);
    if (!Primary)
      return;
    this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = (Color32) Color.white;
    this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = (Color32) Color.clear;
  }
}
