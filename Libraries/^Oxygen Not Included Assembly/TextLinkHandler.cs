// Decompiled with JetBrains decompiler
// Type: TextLinkHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private static TextLinkHandler hoveredText;
  [MyCmpGet]
  private LocText text;
  private bool hoverLink;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || !this.text.AllowLinks)
      return;
    int intersectingLink = TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null);
    if (intersectingLink == -1)
      return;
    string index = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[intersectingLink].GetLinkID());
    if (!CodexCache.entries.ContainsKey(index))
    {
      SubEntry subEntry = CodexCache.FindSubEntry(index);
      if (subEntry == null || subEntry.disabled)
        index = "PAGENOTFOUND";
    }
    else if (CodexCache.entries[index].disabled)
      index = "PAGENOTFOUND";
    if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
      ManagementMenu.Instance.ToggleCodex();
    ManagementMenu.Instance.codexScreen.ChangeArticle(index, true);
  }

  private void Update()
  {
    this.CheckMouseOver();
    if (!((Object) TextLinkHandler.hoveredText == (Object) this) || !this.text.AllowLinks)
      return;
    PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
  }

  private void OnEnable()
  {
    this.CheckMouseOver();
  }

  private void OnDisable()
  {
    this.ClearState();
  }

  private void Awake()
  {
    this.text = this.GetComponent<LocText>();
    if (!this.text.AllowLinks || this.text.raycastTarget)
      return;
    this.text.raycastTarget = true;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.SetMouseOver();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.ClearState();
  }

  private void ClearState()
  {
    if ((Object) this == (Object) null || this.Equals((object) null) || !((Object) TextLinkHandler.hoveredText == (Object) this))
      return;
    if (this.hoverLink && (Object) PlayerController.Instance != (Object) null && (Object) PlayerController.Instance.ActiveTool != (Object) null)
      PlayerController.Instance.ActiveTool.SetLinkCursor(false);
    TextLinkHandler.hoveredText = (TextLinkHandler) null;
    this.hoverLink = false;
  }

  public void CheckMouseOver()
  {
    if ((Object) this.text == (Object) null)
      return;
    if (TMP_TextUtilities.FindIntersectingLink((TMP_Text) this.text, KInputManager.GetMousePos(), (Camera) null) != -1)
    {
      this.SetMouseOver();
      this.hoverLink = true;
    }
    else
    {
      if (!((Object) TextLinkHandler.hoveredText == (Object) this))
        return;
      this.hoverLink = false;
    }
  }

  private void SetMouseOver()
  {
    if ((Object) TextLinkHandler.hoveredText != (Object) null && (Object) TextLinkHandler.hoveredText != (Object) this)
      TextLinkHandler.hoveredText.hoverLink = false;
    TextLinkHandler.hoveredText = this;
  }
}
