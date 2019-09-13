// Decompiled with JetBrains decompiler
// Type: DragMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
  public bool dragOnSurfaces = true;
  private GameObject m_DraggingIcon;
  private RectTransform m_DraggingPlane;
  public DragMe.IDragListener listener;

  public void OnBeginDrag(PointerEventData eventData)
  {
    Canvas inParents = DragMe.FindInParents<Canvas>(this.gameObject);
    if ((Object) inParents == (Object) null)
      return;
    this.m_DraggingIcon = Object.Instantiate<GameObject>(this.gameObject);
    GraphicRaycaster component = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
    if ((Object) component != (Object) null)
      component.enabled = false;
    this.m_DraggingIcon.name = "dragObj";
    this.m_DraggingIcon.transform.SetParent(inParents.transform, false);
    this.m_DraggingIcon.transform.SetAsLastSibling();
    this.m_DraggingIcon.GetComponent<RectTransform>().pivot = Vector2.zero;
    this.m_DraggingPlane = !this.dragOnSurfaces ? inParents.transform as RectTransform : this.transform as RectTransform;
    this.SetDraggedPosition(eventData);
    this.listener.OnBeginDrag(eventData.position);
  }

  public void OnDrag(PointerEventData data)
  {
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    this.SetDraggedPosition(data);
  }

  private void SetDraggedPosition(PointerEventData data)
  {
    if (this.dragOnSurfaces && (Object) data.pointerEnter != (Object) null && (Object) (data.pointerEnter.transform as RectTransform) != (Object) null)
      this.m_DraggingPlane = data.pointerEnter.transform as RectTransform;
    RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
    Vector3 worldPoint;
    if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out worldPoint))
      return;
    component.position = worldPoint;
    component.rotation = this.m_DraggingPlane.rotation;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    this.listener.OnEndDrag(eventData.position);
    if (!((Object) this.m_DraggingIcon != (Object) null))
      return;
    Object.Destroy((Object) this.m_DraggingIcon);
  }

  public static T FindInParents<T>(GameObject go) where T : Component
  {
    if ((Object) go == (Object) null)
      return (T) null;
    T obj = (T) null;
    for (Transform parent = go.transform.parent; (Object) parent != (Object) null && (Object) obj == (Object) null; parent = parent.parent)
      obj = parent.gameObject.GetComponent<T>();
    return obj;
  }

  public interface IDragListener
  {
    void OnBeginDrag(Vector2 position);

    void OnEndDrag(Vector2 position);
  }
}
