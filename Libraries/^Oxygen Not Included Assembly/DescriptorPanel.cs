// Decompiled with JetBrains decompiler
// Type: DescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DescriptorPanel : KMonoBehaviour
{
  private List<GameObject> labels = new List<GameObject>();
  [SerializeField]
  private GameObject customLabelPrefab;

  public bool HasDescriptors()
  {
    return this.labels.Count > 0;
  }

  public void SetDescriptors(IList<Descriptor> descriptors)
  {
    int index;
    for (index = 0; index < descriptors.Count; ++index)
    {
      GameObject gameObject;
      if (index >= this.labels.Count)
      {
        gameObject = Util.KInstantiate(!((Object) this.customLabelPrefab != (Object) null) ? ScreenPrefabs.Instance.DescriptionLabel : this.customLabelPrefab, this.gameObject, (string) null);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.labels.Add(gameObject);
      }
      else
        gameObject = this.labels[index];
      gameObject.GetComponent<LocText>().text = descriptors[index].IndentedText();
      gameObject.GetComponent<ToolTip>().toolTip = descriptors[index].tooltipText;
      gameObject.SetActive(true);
    }
    for (; index < this.labels.Count; ++index)
      this.labels[index].SetActive(false);
  }
}
