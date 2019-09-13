// Decompiled with JetBrains decompiler
// Type: ReportScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ReportScreen : KScreen
{
  private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();
  [SerializeField]
  private LocText title;
  [SerializeField]
  private KButton prevButton;
  [SerializeField]
  private KButton nextButton;
  [SerializeField]
  private KButton summaryButton;
  [SerializeField]
  private GameObject lineItem;
  [SerializeField]
  private GameObject lineItemSpacer;
  [SerializeField]
  private GameObject lineItemHeader;
  [SerializeField]
  private GameObject contentFolder;
  private ReportManager.DailyReport currentReport;

  public static ReportScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    ReportScreen.Instance = (ReportScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ReportScreen.Instance = this;
    this.prevButton.onClick += (System.Action) (() => this.ShowReport(this.currentReport.day - 1));
    this.nextButton.onClick += (System.Action) (() => this.ShowReport(this.currentReport.day + 1));
    this.summaryButton.onClick += (System.Action) (() => MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData()));
    this.ConsumeMouseScroll = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  protected override void OnShow(bool bShow)
  {
    base.OnShow(bShow);
    if (!((UnityEngine.Object) ReportManager.Instance != (UnityEngine.Object) null))
      return;
    this.currentReport = ReportManager.Instance.TodaysReport;
  }

  public void SetTitle(string title)
  {
    this.title.text = title;
  }

  public override void ScreenUpdate(bool b)
  {
    base.ScreenUpdate(b);
    this.Refresh();
  }

  private void Refresh()
  {
    Debug.Assert(this.currentReport != null);
    if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
      this.SetTitle(string.Format((string) UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, (object) this.currentReport.day));
    else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
      this.SetTitle(string.Format((string) UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, (object) this.currentReport.day));
    else
      this.SetTitle(string.Format((string) UI.ENDOFDAYREPORT.DAY_TITLE, (object) this.currentReport.day));
    bool flag1 = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
    this.nextButton.isInteractable = flag1;
    if (flag1)
    {
      this.nextButton.GetComponent<ToolTip>().toolTip = string.Format((string) UI.ENDOFDAYREPORT.DAY_TITLE, (object) (this.currentReport.day + 1));
      this.nextButton.GetComponent<ToolTip>().enabled = true;
    }
    else
      this.nextButton.GetComponent<ToolTip>().enabled = false;
    bool flag2 = this.currentReport.day > 1;
    this.prevButton.isInteractable = flag2;
    if (flag2)
    {
      this.prevButton.GetComponent<ToolTip>().toolTip = string.Format((string) UI.ENDOFDAYREPORT.DAY_TITLE, (object) (this.currentReport.day - 1));
      this.prevButton.GetComponent<ToolTip>().enabled = true;
    }
    else
      this.prevButton.GetComponent<ToolTip>().enabled = false;
    this.AddSpacer(0);
    int group = 1;
    foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> reportGroup in ReportManager.Instance.ReportGroups)
    {
      ReportManager.ReportEntry entry = this.currentReport.GetEntry(reportGroup.Key);
      if (group != reportGroup.Value.group)
      {
        group = reportGroup.Value.group;
        this.AddSpacer(group);
      }
      bool is_line_active = (double) entry.accumulate != 0.0 || reportGroup.Value.reportIfZero;
      if (reportGroup.Value.isHeader)
        this.CreateHeader(reportGroup.Value);
      else if (is_line_active)
        this.CreateOrUpdateLine(entry, reportGroup.Value, is_line_active);
    }
  }

  public void ShowReport(int day)
  {
    this.currentReport = ReportManager.Instance.FindReport(day);
    Debug.Assert(this.currentReport != null, (object) ("Can't find report for day: " + day.ToString()));
    this.Refresh();
  }

  private GameObject AddSpacer(int group)
  {
    GameObject gameObject;
    if (this.lineItems.ContainsKey(group.ToString()))
    {
      gameObject = this.lineItems[group.ToString()];
    }
    else
    {
      gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
      gameObject.name = "Spacer" + group.ToString();
      this.lineItems[group.ToString()] = gameObject;
    }
    gameObject.SetActive(true);
    return gameObject;
  }

  private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
  {
    GameObject gameObject = (GameObject) null;
    this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
      gameObject.name = "LineItemHeader" + (object) this.lineItems.Count;
      this.lineItems[reportGroup.stringKey] = gameObject;
    }
    gameObject.SetActive(true);
    gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
    return gameObject;
  }

  private GameObject CreateOrUpdateLine(
    ReportManager.ReportEntry entry,
    ReportManager.ReportGroup reportGroup,
    bool is_line_active)
  {
    GameObject gameObject = (GameObject) null;
    this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
    if (!is_line_active)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.activeSelf)
        gameObject.SetActive(false);
    }
    else
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
        gameObject.name = "LineItem" + (object) this.lineItems.Count;
        this.lineItems[reportGroup.stringKey] = gameObject;
      }
      gameObject.SetActive(true);
      gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
    }
    return gameObject;
  }

  private void OnClickClose()
  {
    this.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
    this.Show(false);
  }
}
