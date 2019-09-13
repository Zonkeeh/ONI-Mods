// Decompiled with JetBrains decompiler
// Type: AsteroidDescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidDescriptorPanel : KMonoBehaviour
{
  private List<GameObject> labels = new List<GameObject>();
  [SerializeField]
  private GameObject customLabelPrefab;

  public bool HasDescriptors()
  {
    return this.labels.Count > 0;
  }

  public void SetDescriptors(IList<AsteroidDescriptor> descriptors)
  {
    int index1;
    for (index1 = 0; index1 < descriptors.Count; ++index1)
    {
      GameObject gameObject;
      if (index1 >= this.labels.Count)
      {
        gameObject = Util.KInstantiate(!((Object) this.customLabelPrefab != (Object) null) ? ScreenPrefabs.Instance.DescriptionLabel : this.customLabelPrefab, this.gameObject, (string) null);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.labels.Add(gameObject);
      }
      else
        gameObject = this.labels[index1];
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      component1.GetReference<LocText>("Label").text = descriptors[index1].text;
      component1.GetReference<ToolTip>("ToolTip").toolTip = descriptors[index1].tooltip;
      if (descriptors[index1].bands != null)
      {
        Transform reference1 = component1.GetReference<Transform>("BandContainer");
        Transform reference2 = component1.GetReference<Transform>("BarBitPrefab");
        int index2;
        for (index2 = 0; index2 < descriptors[index1].bands.Count; ++index2)
        {
          Transform transform = index2 < reference1.childCount ? reference1.GetChild(index2) : Util.KInstantiateUI<Transform>(reference2.gameObject, reference1.gameObject, false);
          Image component2 = transform.GetComponent<Image>();
          LayoutElement component3 = transform.GetComponent<LayoutElement>();
          component2.color = descriptors[index1].bands[index2].second;
          component3.flexibleWidth = descriptors[index1].bands[index2].third;
          transform.GetComponent<ToolTip>().toolTip = descriptors[index1].bands[index2].first;
          transform.gameObject.SetActive(true);
        }
        for (; index2 < reference1.childCount; ++index2)
          reference1.GetChild(index2).gameObject.SetActive(false);
      }
      gameObject.SetActive(true);
    }
    for (; index1 < this.labels.Count; ++index1)
      this.labels[index1].SetActive(false);
  }
}
