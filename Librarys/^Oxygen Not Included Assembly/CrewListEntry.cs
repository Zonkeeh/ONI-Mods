// Decompiled with JetBrains decompiler
// Type: CrewListEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
{
  protected MinionIdentity identity;
  protected CrewPortrait portrait;
  public CrewPortrait PortraitPrefab;
  public GameObject crewPortraitParent;
  protected bool mouseOver;
  public Image BorderHighlight;
  public Image BGImage;
  public float lastClickTime;

  public MinionIdentity Identity
  {
    get
    {
      return this.identity;
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.mouseOver = true;
    this.BGImage.enabled = true;
    this.BorderHighlight.color = new Color(0.6588235f, 0.2901961f, 0.4745098f);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.mouseOver = false;
    this.BGImage.enabled = false;
    this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    this.SelectCrewMember((double) Time.unscaledTime - (double) this.lastClickTime < 0.300000011920929);
    this.lastClickTime = Time.unscaledTime;
  }

  public virtual void Populate(MinionIdentity _identity)
  {
    this.identity = _identity;
    if ((Object) this.portrait == (Object) null)
    {
      this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, !((Object) this.crewPortraitParent != (Object) null) ? this.gameObject : this.crewPortraitParent, false);
      if ((Object) this.crewPortraitParent == (Object) null)
        this.portrait.transform.SetSiblingIndex(2);
    }
    this.portrait.SetIdentityObject((IAssignableIdentity) _identity, true);
  }

  public virtual void Refresh()
  {
  }

  public void RefreshCrewPortraitContent()
  {
    if (!((Object) this.portrait != (Object) null))
      return;
    this.portrait.ForceRefresh();
  }

  private string seniorityString()
  {
    return this.identity.GetAttributes().GetProfessionString(true);
  }

  public void SelectCrewMember(bool focus)
  {
    if (focus)
      SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0.0f, 0.0f));
    else
      SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
  }
}
