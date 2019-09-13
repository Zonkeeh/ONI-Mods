// Decompiled with JetBrains decompiler
// Type: DiseaseOverlayWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiseaseOverlayWidget : KMonoBehaviour
{
  private List<Image> displayedDiseases = new List<Image>();
  [SerializeField]
  private Image progressFill;
  [SerializeField]
  private ToolTip progressToolTip;
  [SerializeField]
  private Image germsImage;
  [SerializeField]
  private Vector3 offset;
  [SerializeField]
  private Image diseasedImage;

  public void Refresh(AmountInstance value_src)
  {
    GameObject gameObject = value_src.gameObject;
    if ((Object) gameObject == (Object) null)
      return;
    KAnimControllerBase component = gameObject.GetComponent<KAnimControllerBase>();
    this.transform.SetPosition((!((Object) component != (Object) null) ? gameObject.transform.GetPosition() + Vector3.down : component.GetWorldPivot()) + this.offset);
    AmountInstance amountInstance = value_src;
    if (amountInstance != null)
    {
      this.progressFill.transform.parent.gameObject.SetActive(true);
      float num = amountInstance.value / amountInstance.GetMax();
      Vector3 localScale = this.progressFill.rectTransform.localScale;
      localScale.y = num;
      this.progressFill.rectTransform.localScale = localScale;
      this.progressToolTip.toolTip = (string) DUPLICANTS.ATTRIBUTES.IMMUNITY.NAME + " " + GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
    }
    else
      this.progressFill.transform.parent.gameObject.SetActive(false);
    int index1 = 0;
    Amounts amounts = gameObject.GetComponent<Modifiers>().GetAmounts();
    foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
    {
      float num = amounts.Get(resource.amount).value;
      if ((double) num > 0.0)
      {
        Image image;
        if (index1 < this.displayedDiseases.Count)
        {
          image = this.displayedDiseases[index1];
        }
        else
        {
          image = Util.KInstantiateUI(this.germsImage.gameObject, this.germsImage.transform.parent.gameObject, true).GetComponent<Image>();
          this.displayedDiseases.Add(image);
        }
        image.color = (Color) resource.overlayColour;
        image.GetComponent<ToolTip>().toolTip = resource.Name + " " + GameUtil.GetFormattedDiseaseAmount((int) num);
        ++index1;
      }
    }
    for (int index2 = this.displayedDiseases.Count - 1; index2 >= index1; --index2)
    {
      Util.KDestroyGameObject(this.displayedDiseases[index2].gameObject);
      this.displayedDiseases.RemoveAt(index2);
    }
    this.diseasedImage.enabled = false;
    this.progressFill.transform.parent.gameObject.SetActive(this.displayedDiseases.Count > 0);
    this.germsImage.transform.parent.gameObject.SetActive(this.displayedDiseases.Count > 0);
  }
}
