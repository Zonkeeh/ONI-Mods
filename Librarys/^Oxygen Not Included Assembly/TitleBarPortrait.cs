// Decompiled with JetBrains decompiler
// Type: TitleBarPortrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class TitleBarPortrait : KMonoBehaviour
{
  public GameObject FaceObject;
  public GameObject ImageObject;
  public GameObject PortraitShadow;
  public GameObject AnimControllerObject;
  public Material DefaultMaterial;
  public Material DesatMaterial;

  public void SetSaturation(bool saturated)
  {
    this.ImageObject.GetComponent<Image>().material = !saturated ? this.DesatMaterial : this.DefaultMaterial;
  }

  public void SetPortrait(GameObject selectedTarget)
  {
    MinionIdentity component1 = selectedTarget.GetComponent<MinionIdentity>();
    if ((Object) component1 != (Object) null)
    {
      this.SetPortrait(component1);
    }
    else
    {
      Building component2 = selectedTarget.GetComponent<Building>();
      if ((Object) component2 != (Object) null)
      {
        this.SetPortrait(component2.Def.GetUISprite("ui", false));
      }
      else
      {
        MeshRenderer componentInChildren = selectedTarget.GetComponentInChildren<MeshRenderer>();
        if (!(bool) ((Object) componentInChildren))
          return;
        this.SetPortrait(Sprite.Create((Texture2D) componentInChildren.material.mainTexture, new Rect(0.0f, 0.0f, (float) componentInChildren.material.mainTexture.width, (float) componentInChildren.material.mainTexture.height), new Vector2(0.5f, 0.5f)));
      }
    }
  }

  public void SetPortrait(Sprite image)
  {
    if ((bool) ((Object) this.PortraitShadow))
      this.PortraitShadow.SetActive(true);
    if ((bool) ((Object) this.FaceObject))
      this.FaceObject.SetActive(false);
    if ((bool) ((Object) this.ImageObject))
      this.ImageObject.SetActive(true);
    if ((bool) ((Object) this.AnimControllerObject))
      this.AnimControllerObject.SetActive(false);
    if ((Object) image == (Object) null)
      this.ClearPortrait();
    else
      this.ImageObject.GetComponent<Image>().sprite = image;
  }

  private void SetPortrait(MinionIdentity identity)
  {
    if ((bool) ((Object) this.PortraitShadow))
      this.PortraitShadow.SetActive(true);
    if ((bool) ((Object) this.FaceObject))
      this.FaceObject.SetActive(false);
    if ((bool) ((Object) this.ImageObject))
      this.ImageObject.SetActive(false);
    CrewPortrait component = this.GetComponent<CrewPortrait>();
    if ((Object) component != (Object) null)
    {
      component.SetIdentityObject((IAssignableIdentity) identity, true);
    }
    else
    {
      if (!(bool) ((Object) this.AnimControllerObject))
        return;
      this.AnimControllerObject.SetActive(true);
      CrewPortrait.SetPortraitData((IAssignableIdentity) identity, this.AnimControllerObject.GetComponent<KBatchedAnimController>(), true);
    }
  }

  public void ClearPortrait()
  {
    if ((bool) ((Object) this.PortraitShadow))
      this.PortraitShadow.SetActive(false);
    if ((bool) ((Object) this.FaceObject))
      this.FaceObject.SetActive(false);
    if ((bool) ((Object) this.ImageObject))
      this.ImageObject.SetActive(false);
    if (!(bool) ((Object) this.AnimControllerObject))
      return;
    this.AnimControllerObject.SetActive(false);
  }
}
