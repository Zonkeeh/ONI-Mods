// Decompiled with JetBrains decompiler
// Type: ResearchScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchScreen : KModalScreen
{
  private float targetZoom = 1f;
  private float currentZoom = 1f;
  private Vector2 keyPanDelta = (Vector2) Vector3.zero;
  [SerializeField]
  private float effectiveZoomSpeed = 5f;
  [SerializeField]
  private float zoomAmountPerScroll = 0.05f;
  [SerializeField]
  private float zoomAmountPerButton = 0.5f;
  [SerializeField]
  private float minZoom = 0.15f;
  [SerializeField]
  private float maxZoom = 1f;
  [SerializeField]
  private float keyboardScrollSpeed = 200f;
  [SerializeField]
  private float keyPanEasing = 1f;
  [SerializeField]
  private float edgeClampFactor = 0.5f;
  [SerializeField]
  private Image BG;
  public ResearchEntry entryPrefab;
  public ResearchTreeTitle researchTreeTitlePrefab;
  public GameObject foreground;
  public GameObject scrollContent;
  public GameObject treeTitles;
  public GameObject pointDisplayCountPrefab;
  public GameObject pointDisplayContainer;
  private Dictionary<string, LocText> pointDisplayMap;
  private Dictionary<Tech, ResearchEntry> entryMap;
  [SerializeField]
  private TMP_InputField filterField;
  [SerializeField]
  private KButton filterClearButton;
  [SerializeField]
  private KButton zoomOutButton;
  [SerializeField]
  private KButton zoomInButton;
  private Tech currentResearch;
  public KButton CloseButton;
  private GraphicRaycaster m_Raycaster;
  private PointerEventData m_PointerEventData;
  private Vector3 currentScrollPosition;
  private bool panUp;
  private bool panDown;
  private bool panLeft;
  private bool panRight;
  private bool rightMouseDown;
  private bool leftMouseDown;
  private bool isDragging;
  private Vector3 dragStartPosition;
  private Vector3 dragLastPosition;
  private bool zoomCenterLock;

  public bool IsBeingResearched(Tech tech)
  {
    return Research.Instance.IsBeingResearched(tech);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    Transform transform = this.transform;
    while ((UnityEngine.Object) this.m_Raycaster == (UnityEngine.Object) null)
    {
      this.m_Raycaster = transform.GetComponent<GraphicRaycaster>();
      if ((UnityEngine.Object) this.m_Raycaster == (UnityEngine.Object) null)
        transform = transform.parent;
    }
  }

  private void ZoomOut()
  {
    this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
    this.zoomCenterLock = true;
  }

  private void ZoomIn()
  {
    this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
    this.zoomCenterLock = true;
  }

  private void Update()
  {
    RectTransform component = this.scrollContent.GetComponent<RectTransform>();
    if (!this.isDragging && (this.rightMouseDown || this.leftMouseDown) && (double) Vector2.Distance((Vector2) this.dragStartPosition, (Vector2) KInputManager.GetMousePos()) > 1.0)
      this.isDragging = true;
    Vector2 anchoredPosition = component.anchoredPosition;
    this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f));
    Vector2 zero1 = Vector2.zero;
    Vector2 mousePos = (Vector2) KInputManager.GetMousePos();
    Vector2 vector2_1 = (Vector2) (!this.zoomCenterLock ? component.InverseTransformPoint((Vector3) mousePos) * this.currentZoom : component.InverseTransformPoint((Vector3) new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2))) * this.currentZoom);
    component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
    Vector2 vector2_2 = (Vector2) (!this.zoomCenterLock ? component.InverseTransformPoint((Vector3) mousePos) * this.currentZoom : component.InverseTransformPoint((Vector3) new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2))) * this.currentZoom) - vector2_1;
    float keyboardScrollSpeed = this.keyboardScrollSpeed;
    if (this.panUp)
      this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * keyboardScrollSpeed;
    else if (this.panDown)
      this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * keyboardScrollSpeed;
    if (this.panLeft)
      this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * keyboardScrollSpeed;
    else if (this.panRight)
      this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * keyboardScrollSpeed;
    this.keyPanDelta -= new Vector2(Mathf.Lerp(0.0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0.0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
    Vector2 zero2 = Vector2.zero;
    if (this.isDragging)
    {
      Vector2 vector2_3 = (Vector2) (KInputManager.GetMousePos() - this.dragLastPosition);
      zero2 += vector2_3;
      this.dragLastPosition = KInputManager.GetMousePos();
    }
    Vector2 vector2_4 = anchoredPosition + vector2_2 + this.keyPanDelta + zero2;
    if (!this.isDragging)
    {
      Vector2 vector2_3 = component.rect.size * -0.5f * this.currentZoom;
      Vector2 vector2_5 = component.rect.size * 0.5f * this.currentZoom;
      Vector2 vector2_6 = new Vector2(Mathf.Clamp(vector2_4.x, vector2_3.x, vector2_5.x), Mathf.Clamp(vector2_4.y, vector2_3.y, vector2_5.y)) - vector2_4;
      if (!this.panLeft && !this.panRight && (!this.panUp && !this.panDown))
      {
        vector2_4 += vector2_6 * this.edgeClampFactor * Time.unscaledDeltaTime;
      }
      else
      {
        vector2_4 += vector2_6;
        if ((double) vector2_6.x < 0.0)
          this.keyPanDelta.x = Mathf.Min(0.0f, this.keyPanDelta.x);
        if ((double) vector2_6.x > 0.0)
          this.keyPanDelta.x = Mathf.Max(0.0f, this.keyPanDelta.x);
        if ((double) vector2_6.y < 0.0)
          this.keyPanDelta.y = Mathf.Min(0.0f, this.keyPanDelta.y);
        if ((double) vector2_6.y > 0.0)
          this.keyPanDelta.y = Mathf.Max(0.0f, this.keyPanDelta.y);
      }
    }
    component.anchoredPosition = vector2_4;
  }

  protected override void OnSpawn()
  {
    this.Subscribe(Research.Instance.gameObject, -1914338957, new System.Action<object>(this.OnActiveResearchChanged));
    this.Subscribe(Game.Instance.gameObject, -107300940, new System.Action<object>(this.OnResearchComplete));
    this.Subscribe(Game.Instance.gameObject, -1974454597, (System.Action<object>) (o => this.Show(false)));
    this.filterField.placeholder.GetComponent<TextMeshProUGUI>().text = (string) STRINGS.UI.FILTER;
    this.filterField.onValueChanged.AddListener(new UnityAction<string>(this.OnFilterChanged));
    this.filterClearButton.onClick += (System.Action) (() =>
    {
      this.filterField.text = string.Empty;
      this.OnFilterChanged(string.Empty);
    });
    this.pointDisplayMap = new Dictionary<string, LocText>();
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
    {
      this.pointDisplayMap[type.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
      this.pointDisplayMap[type.id].text = Research.Instance.globalPointInventory.PointsByTypeID[type.id].ToString();
      this.pointDisplayMap[type.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(type.description);
      this.pointDisplayMap[type.id].transform.parent.GetComponentInChildren<Image>().sprite = type.sprite;
    }
    this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
    this.entryMap = new Dictionary<Tech, ResearchEntry>();
    List<Tech> resources1 = Db.Get().Techs.resources;
    resources1.Sort((Comparison<Tech>) ((x, y) => y.center.y.CompareTo(x.center.y)));
    List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
    resources2.Sort((Comparison<TechTreeTitle>) ((x, y) => y.center.y.CompareTo(x.center.y)));
    Vector2 vector2_1 = new Vector2(0.0f, 125f);
    for (int id = 0; id < resources2.Count; ++id)
    {
      ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
      TechTreeTitle techTreeTitle1 = resources2[id];
      researchTreeTitle.name = techTreeTitle1.Name + " Title";
      Vector3 vector3_1 = (Vector3) (techTreeTitle1.center + vector2_1);
      researchTreeTitle.transform.rectTransform().anchoredPosition = (Vector2) vector3_1;
      float height = techTreeTitle1.height;
      float y;
      if (id + 1 < resources2.Count)
      {
        TechTreeTitle techTreeTitle2 = resources2[id + 1];
        Vector3 vector3_2 = (Vector3) (techTreeTitle2.center + vector2_1);
        y = height + (vector3_1.y - (vector3_2.y + techTreeTitle2.height));
      }
      else
        y = height + 600f;
      researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle1.width, y);
      researchTreeTitle.SetLabel(techTreeTitle1.Name);
      researchTreeTitle.SetColor(id);
    }
    List<Vector2> vector2List = new List<Vector2>();
    Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
    for (int index1 = 0; index1 < resources1.Count; ++index1)
    {
      ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
      Tech key = resources1[index1];
      researchEntry.name = key.Name + " Panel";
      Vector3 vector3 = (Vector3) (key.center + vector2_2);
      researchEntry.transform.rectTransform().anchoredPosition = (Vector2) vector3;
      researchEntry.transform.rectTransform().sizeDelta = new Vector2(key.width, key.height);
      this.entryMap.Add(key, researchEntry);
      if (key.edges.Count > 0)
      {
        for (int index2 = 0; index2 < key.edges.Count; ++index2)
        {
          ResourceTreeNode.Edge edge = key.edges[index2];
          if (edge.path == null)
          {
            vector2List.AddRange((IEnumerable<Vector2>) edge.SrcTarget);
          }
          else
          {
            switch (edge.edgeType)
            {
              case ResourceTreeNode.Edge.EdgeType.PolyLineEdge:
              case ResourceTreeNode.Edge.EdgeType.QuadCurveEdge:
              case ResourceTreeNode.Edge.EdgeType.BezierEdge:
              case ResourceTreeNode.Edge.EdgeType.GenericEdge:
                vector2List.Add(edge.SrcTarget[0]);
                vector2List.Add(edge.path[0]);
                for (int index3 = 1; index3 < edge.path.Count; ++index3)
                {
                  vector2List.Add(edge.path[index3 - 1]);
                  vector2List.Add(edge.path[index3]);
                }
                vector2List.Add(edge.path[edge.path.Count - 1]);
                vector2List.Add(edge.SrcTarget[1]);
                continue;
              default:
                vector2List.AddRange((IEnumerable<Vector2>) edge.path);
                continue;
            }
          }
        }
      }
    }
    for (int index = 0; index < vector2List.Count; ++index)
      vector2List[index] = new Vector2(vector2List[index].x, vector2List[index].y + this.foreground.transform.rectTransform().rect.height);
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetTech(entry.Key);
    this.CloseButton.soundPlayer.Enabled = false;
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.StartCoroutine(this.WaitAndSetActiveResearch());
    ManagementMenu.Instance.AddResearchScreen(this);
    base.OnSpawn();
    this.Show(false);
    this.zoomOutButton.onClick += (System.Action) (() => this.ZoomOut());
    this.zoomInButton.onClick += (System.Action) (() => this.ZoomIn());
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unsubscribe(Game.Instance.gameObject, -1974454597, (System.Action<object>) (o => this.Deactivate()));
  }

  [DebuggerHidden]
  private IEnumerator WaitAndSetActiveResearch()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ResearchScreen.\u003CWaitAndSetActiveResearch\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public Vector3 GetEntryPosition(Tech tech)
  {
    if (this.entryMap.ContainsKey(tech))
      return this.entryMap[tech].transform.GetPosition();
    Debug.LogError((object) "The Tech provided was not present in the dictionary");
    return Vector3.zero;
  }

  public ResearchEntry GetEntry(Tech tech)
  {
    if (this.entryMap == null)
      return (ResearchEntry) null;
    if (this.entryMap.ContainsKey(tech))
      return this.entryMap[tech];
    Debug.LogError((object) "The Tech provided was not present in the dictionary");
    return (ResearchEntry) null;
  }

  public void SetEntryPercentage(Tech tech, float percent)
  {
    ResearchEntry entry = this.GetEntry(tech);
    if (!((UnityEngine.Object) entry != (UnityEngine.Object) null))
      return;
    entry.SetPercentage(percent);
  }

  public void TurnEverythingOff()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetEverythingOff();
  }

  public void TurnEverythingOn()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetEverythingOn();
  }

  private void SelectAllEntries(Tech tech, bool isSelected)
  {
    ResearchEntry entry = this.GetEntry(tech);
    if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
      entry.QueueStateChanged(isSelected);
    foreach (Tech tech1 in tech.requiredTech)
      this.SelectAllEntries(tech1, isSelected);
  }

  private void OnResearchComplete(object data)
  {
    ResearchEntry entry = this.GetEntry((Tech) data);
    if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
      entry.ResearchCompleted(true);
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
  }

  private void UpdatePointDisplay()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
      this.pointDisplayMap[type.id].text = string.Format("{0}: {1}", (object) Research.Instance.researchTypes.GetResearchType(type.id).name, (object) Research.Instance.globalPointInventory.PointsByTypeID[type.id].ToString());
  }

  private void OnActiveResearchChanged(object data)
  {
    List<TechInstance> techInstanceList = (List<TechInstance>) data;
    foreach (TechInstance techInstance in techInstanceList)
    {
      ResearchEntry entry = this.GetEntry(techInstance.tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.QueueStateChanged(true);
    }
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
    if (techInstanceList.Count <= 0)
      return;
    this.currentResearch = techInstanceList[techInstanceList.Count - 1].tech;
  }

  private void UpdateProgressBars()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.UpdateProgressBars();
  }

  public void CancelResearch()
  {
    List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
    foreach (TechInstance techInstance in researchQueue)
    {
      ResearchEntry entry = this.GetEntry(techInstance.tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.QueueStateChanged(false);
    }
    researchQueue.Clear();
  }

  private void SetActiveResearch(Tech newResearch)
  {
    if (newResearch != this.currentResearch && this.currentResearch != null)
      this.SelectAllEntries(this.currentResearch, false);
    this.currentResearch = newResearch;
    if (this.currentResearch == null)
      return;
    this.SelectAllEntries(this.currentResearch, true);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      DetailsScreen.Instance.gameObject.SetActive(false);
    else if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
    {
      DetailsScreen.Instance.gameObject.SetActive(true);
      DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
    }
    this.filterField.text = string.Empty;
    this.OnFilterChanged(string.Empty);
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if (e.IsAction(Action.MouseRight) || e.IsAction(Action.MouseLeft))
      {
        if (!this.isDragging && e.TryConsume(Action.MouseRight))
        {
          this.isDragging = false;
          this.rightMouseDown = false;
          this.leftMouseDown = false;
          ManagementMenu.Instance.CloseAll();
          return;
        }
        this.isDragging = false;
        this.rightMouseDown = false;
        this.leftMouseDown = false;
      }
      if (this.panUp && e.TryConsume(Action.PanUp))
      {
        this.panUp = false;
        return;
      }
      if (this.panDown && e.TryConsume(Action.PanDown))
      {
        this.panDown = false;
        return;
      }
      if (this.panRight && e.TryConsume(Action.PanRight))
      {
        this.panRight = false;
        return;
      }
      if (this.panLeft && e.TryConsume(Action.PanLeft))
      {
        this.panLeft = false;
        return;
      }
    }
    base.OnKeyUp(e);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if (e.TryConsume(Action.MouseRight))
      {
        this.dragStartPosition = KInputManager.GetMousePos();
        this.dragLastPosition = KInputManager.GetMousePos();
        this.rightMouseDown = true;
        return;
      }
      if (e.TryConsume(Action.MouseLeft))
      {
        this.dragStartPosition = KInputManager.GetMousePos();
        this.dragLastPosition = KInputManager.GetMousePos();
        this.leftMouseDown = true;
        return;
      }
      if (e.TryConsume(Action.ZoomIn))
      {
        this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
        this.zoomCenterLock = false;
        return;
      }
      if (e.TryConsume(Action.ZoomOut))
      {
        this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
        this.zoomCenterLock = false;
        return;
      }
      if (e.TryConsume(Action.Escape))
      {
        ManagementMenu.Instance.CloseAll();
        return;
      }
      if (e.TryConsume(Action.PanLeft))
      {
        this.panLeft = true;
        return;
      }
      if (e.TryConsume(Action.PanRight))
      {
        this.panRight = true;
        return;
      }
      if (e.TryConsume(Action.PanUp))
      {
        this.panUp = true;
        return;
      }
      if (e.TryConsume(Action.PanDown))
      {
        this.panDown = true;
        return;
      }
    }
    base.OnKeyDown(e);
  }

  private void OnFilterChanged(string filter_text)
  {
    filter_text = filter_text.ToLower();
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.UpdateFilterState(filter_text);
  }

  public enum ResearchState
  {
    Available,
    ActiveResearch,
    ResearchComplete,
    MissingPrerequisites,
    StateCount,
  }
}
