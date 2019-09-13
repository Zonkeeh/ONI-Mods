// Decompiled with JetBrains decompiler
// Type: ReportManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;

public class ReportManager : KMonoBehaviour
{
  [MyCmpAdd]
  private Notifier notifier;
  private ReportManager.NoteStorage noteStorage;
  public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;
  [Serialize]
  private List<ReportManager.DailyReport> dailyReports;
  [Serialize]
  private ReportManager.DailyReport todaysReport;
  [Serialize]
  private byte[] noteStorageBytes;

  public ReportManager()
  {
    Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary1 = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
    dictionary1.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, true, 1, (string) UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, string.Empty, string.Empty, ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true)), true, 1, (string) UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, (string) UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None)), true, 1, (string) UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, (string) UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, (string) UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedDiseaseAmount((int) v)), true, 1, (string) UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, (string) UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.LEVEL_UP.NAME, (string) UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, (string) UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, true, 1, (string) UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, (string) UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, (string) UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, (string) UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, false, 1, (string) UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, (string) UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, true, 2, (string) UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, string.Empty, string.Empty, ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0), GameUtil.TimeSlice.None)), true, 2, (string) UI.ENDOFDAYREPORT.WORK_TIME.NAME, (string) UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) ((v, num_entries) => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0) / num_entries, GameUtil.TimeSlice.None))));
    dictionary1.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0), GameUtil.TimeSlice.None)), true, 2, (string) UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, (string) UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) ((v, num_entries) => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0) / num_entries, GameUtil.TimeSlice.None))));
    dictionary1.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0), GameUtil.TimeSlice.None)), true, 2, (string) UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, (string) UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) ((v, num_entries) => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0) / num_entries, GameUtil.TimeSlice.None))));
    dictionary1.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0), GameUtil.TimeSlice.None)), true, 2, (string) UI.ENDOFDAYREPORT.IDLE_TIME.NAME, (string) UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, string.Empty, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) ((v, num_entries) => GameUtil.GetFormattedPercent((float) ((double) v / 600.0 * 100.0) / num_entries, GameUtil.TimeSlice.None))));
    dictionary1.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup((ReportManager.FormattingFn) null, true, 3, (string) UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, string.Empty, string.Empty, ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), true, 3, (string) UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, (string) UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary2 = dictionary1;
    // ISSUE: reference to a compiler-generated field
    if (ReportManager.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ReportManager.\u003C\u003Ef__mg\u0024cache0 = new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules);
    }
    // ISSUE: reference to a compiler-generated field
    ReportManager.ReportGroup reportGroup1 = new ReportManager.ReportGroup(ReportManager.\u003C\u003Ef__mg\u0024cache0, true, 3, (string) UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, (string) UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null);
    dictionary2.Add(ReportManager.ReportType.EnergyCreated, reportGroup1);
    Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary3 = dictionary1;
    // ISSUE: reference to a compiler-generated field
    if (ReportManager.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ReportManager.\u003C\u003Ef__mg\u0024cache1 = new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules);
    }
    // ISSUE: reference to a compiler-generated field
    ReportManager.ReportGroup reportGroup2 = new ReportManager.ReportGroup(ReportManager.\u003C\u003Ef__mg\u0024cache1, true, 3, (string) UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, (string) UI.ENDOFDAYREPORT.ENERGY_WASTED.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null);
    dictionary3.Add(ReportManager.ReportType.EnergyWasted, reportGroup2);
    dictionary1.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), false, 3, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    dictionary1.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((ReportManager.FormattingFn) (v => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), false, 3, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, (string) UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (ReportManager.GroupFormattingFn) null));
    this.ReportGroups = dictionary1;
    this.dailyReports = new List<ReportManager.DailyReport>();
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public List<ReportManager.DailyReport> reports
  {
    get
    {
      return this.dailyReports;
    }
  }

  public static void DestroyInstance()
  {
    ReportManager.Instance = (ReportManager) null;
  }

  public static ReportManager Instance { get; private set; }

  public ReportManager.DailyReport TodaysReport
  {
    get
    {
      return this.todaysReport;
    }
  }

  public ReportManager.DailyReport YesterdaysReport
  {
    get
    {
      if (this.dailyReports.Count <= 1)
        return (ReportManager.DailyReport) null;
      return this.dailyReports[this.dailyReports.Count - 1];
    }
  }

  protected override void OnPrefabInit()
  {
    ReportManager.Instance = this;
    this.Subscribe(Game.Instance.gameObject, -1917495436, new System.Action<object>(this.OnSaveGameReady));
    this.noteStorage = new ReportManager.NoteStorage();
  }

  protected override void OnCleanUp()
  {
    ReportManager.Instance = (ReportManager) null;
  }

  [OnSerializing]
  private void OnSerializing()
  {
    MemoryStream memoryStream = new MemoryStream();
    this.noteStorage.Serialize(new BinaryWriter((Stream) memoryStream));
    this.noteStorageBytes = memoryStream.GetBuffer();
  }

  [OnSerialized]
  private void OnSerialized()
  {
    this.noteStorageBytes = (byte[]) null;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (this.noteStorageBytes == null)
      return;
    this.noteStorage.Deserialize(new BinaryReader((Stream) new MemoryStream(this.noteStorageBytes)));
    this.noteStorageBytes = (byte[]) null;
  }

  private void OnSaveGameReady(object data)
  {
    this.Subscribe(GameClock.Instance.gameObject, -722330267, new System.Action<object>(this.OnNightTime));
    if (this.todaysReport != null)
      return;
    this.todaysReport = new ReportManager.DailyReport(this);
    this.todaysReport.day = GameUtil.GetCurrentCycle();
  }

  public void ReportValue(
    ReportManager.ReportType reportType,
    float value,
    string note = null,
    string context = null)
  {
    this.TodaysReport.AddData(reportType, value, note, context);
  }

  private void OnNightTime(object data)
  {
    this.dailyReports.Add(this.todaysReport);
    int day = this.todaysReport.day;
    Notification notification = new Notification(string.Format((string) UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, (object) day), NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((n, d) => string.Format((string) UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, (object) day)), (object) null, true, 0.0f, (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenReports(day)), (object) null, (Transform) null);
    if ((UnityEngine.Object) this.notifier == (UnityEngine.Object) null)
      Debug.LogError((object) "Cant notify, null notifier");
    else
      this.notifier.Add(notification, string.Empty);
    this.todaysReport = new ReportManager.DailyReport(this);
    this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
  }

  public ReportManager.DailyReport FindReport(int day)
  {
    foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
    {
      if (dailyReport.day == day)
        return dailyReport;
    }
    if (this.todaysReport.day == day)
      return this.todaysReport;
    return (ReportManager.DailyReport) null;
  }

  public delegate string FormattingFn(float v);

  public delegate string GroupFormattingFn(float v, float numEntries);

  public enum ReportType
  {
    DuplicantHeader,
    CaloriesCreated,
    StressDelta,
    LevelUp,
    DiseaseStatus,
    DiseaseAdded,
    ToiletIncident,
    ChoreStatus,
    TimeSpentHeader,
    TimeSpent,
    WorkTime,
    TravelTime,
    PersonalTime,
    IdleTime,
    BaseHeader,
    ContaminatedOxygenFlatulence,
    ContaminatedOxygenToilet,
    ContaminatedOxygenSublimation,
    OxygenCreated,
    EnergyCreated,
    EnergyWasted,
    DomesticatedCritters,
    WildCritters,
    RocketsInFlight,
  }

  public struct ReportGroup
  {
    public ReportManager.FormattingFn formatfn;
    public ReportManager.GroupFormattingFn groupFormatfn;
    public string stringKey;
    public string positiveTooltip;
    public string negativeTooltip;
    public bool reportIfZero;
    public int group;
    public bool isHeader;
    public ReportManager.ReportEntry.Order posNoteOrder;
    public ReportManager.ReportEntry.Order negNoteOrder;

    public ReportGroup(
      ReportManager.FormattingFn formatfn,
      bool reportIfZero,
      int group,
      string stringKey,
      string positiveTooltip,
      string negativeTooltip,
      ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered,
      ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered,
      bool is_header = false,
      ReportManager.GroupFormattingFn group_format_fn = null)
    {
      this.formatfn = formatfn == null ? (ReportManager.FormattingFn) (v => v.ToString()) : formatfn;
      this.groupFormatfn = group_format_fn;
      this.stringKey = stringKey;
      this.positiveTooltip = positiveTooltip;
      this.negativeTooltip = negativeTooltip;
      this.reportIfZero = reportIfZero;
      this.group = group;
      this.posNoteOrder = pos_note_order;
      this.negNoteOrder = neg_note_order;
      this.isHeader = is_header;
    }
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class ReportEntry
  {
    [Serialize]
    public int gameHash = -1;
    [Serialize]
    public int noteStorageId;
    [Serialize]
    public ReportManager.ReportType reportType;
    [Serialize]
    public string context;
    [Serialize]
    public float accumulate;
    [Serialize]
    public float accPositive;
    [Serialize]
    public float accNegative;
    [Serialize]
    public ArrayRef<ReportManager.ReportEntry> contextEntries;
    public bool isChild;

    public ReportEntry(
      ReportManager.ReportType reportType,
      int note_storage_id,
      string context,
      bool is_child = false)
    {
      this.reportType = reportType;
      this.context = context;
      this.isChild = is_child;
      this.accumulate = 0.0f;
      this.accPositive = 0.0f;
      this.accNegative = 0.0f;
      this.noteStorageId = note_storage_id;
    }

    public float Positive
    {
      get
      {
        return this.accPositive;
      }
    }

    public float Negative
    {
      get
      {
        return this.accNegative;
      }
    }

    public float Net
    {
      get
      {
        return this.accPositive + this.accNegative;
      }
    }

    [OnDeserializing]
    private void OnDeserialize()
    {
      this.contextEntries.Clear();
    }

    public void IterateNotes(System.Action<ReportManager.ReportEntry.Note> callback)
    {
      ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
    }

    [OnDeserialized]
    private void OnDeserialized()
    {
      if (this.gameHash == -1)
        return;
      this.reportType = (ReportManager.ReportType) this.gameHash;
      this.gameHash = -1;
    }

    public void AddData(
      ReportManager.NoteStorage note_storage,
      float value,
      string note = null,
      string dataContext = null)
    {
      this.AddActualData(note_storage, value, note);
      if (dataContext == null)
        return;
      ReportManager.ReportEntry reportEntry = (ReportManager.ReportEntry) null;
      for (int index = 0; index < this.contextEntries.Count; ++index)
      {
        if (this.contextEntries[index].context == dataContext)
        {
          reportEntry = this.contextEntries[index];
          break;
        }
      }
      if (reportEntry == null)
      {
        reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
        this.contextEntries.Add(reportEntry);
      }
      reportEntry.AddActualData(note_storage, value, note);
    }

    private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
    {
      this.accumulate += value;
      if ((double) value > 0.0)
        this.accPositive += value;
      else
        this.accNegative += value;
      if (note == null)
        return;
      note_storage.Add(this.noteStorageId, value, note);
    }

    public bool HasContextEntries()
    {
      return this.contextEntries.Count > 0;
    }

    public struct Note
    {
      public float value;
      public string note;

      public Note(float value, string note)
      {
        this.value = value;
        this.note = note;
      }
    }

    public enum Order
    {
      Unordered,
      Ascending,
      Descending,
    }
  }

  public class DailyReport
  {
    [Serialize]
    public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
    [Serialize]
    public int day;

    public DailyReport(ReportManager manager)
    {
      foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> reportGroup in manager.ReportGroups)
        this.reportEntries.Add(new ReportManager.ReportEntry(reportGroup.Key, this.noteStorage.GetNewNoteId(), (string) null, false));
    }

    private ReportManager.NoteStorage noteStorage
    {
      get
      {
        return ReportManager.Instance.noteStorage;
      }
    }

    public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
    {
      for (int index = 0; index < this.reportEntries.Count; ++index)
      {
        ReportManager.ReportEntry reportEntry = this.reportEntries[index];
        if (reportEntry.reportType == reportType)
          return reportEntry;
      }
      ReportManager.ReportEntry reportEntry1 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), (string) null, false);
      this.reportEntries.Add(reportEntry1);
      return reportEntry1;
    }

    public void AddData(
      ReportManager.ReportType reportType,
      float value,
      string note = null,
      string context = null)
    {
      this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
    }
  }

  public class NoteStorage
  {
    private const int SERIALIZATION_VERSION = 5;
    private int nextNoteId;
    private ReportManager.NoteStorage.NoteEntries noteEntries;
    private ReportManager.NoteStorage.StringTable stringTable;

    public NoteStorage()
    {
      this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
      this.stringTable = new ReportManager.NoteStorage.StringTable();
    }

    public void Add(int report_entry_id, float value, string note)
    {
      int note_id = this.stringTable.AddString(note);
      this.noteEntries.Add(report_entry_id, value, note_id);
    }

    public int GetNewNoteId()
    {
      return ++this.nextNoteId;
    }

    public void IterateNotes(int report_entry_id, System.Action<ReportManager.ReportEntry.Note> callback)
    {
      this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
    }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write(5);
      writer.Write(this.nextNoteId);
      this.stringTable.Serialize(writer);
      this.noteEntries.Serialize(writer);
    }

    public void Deserialize(BinaryReader reader)
    {
      if (reader.ReadInt32() != 5)
        return;
      this.nextNoteId = reader.ReadInt32();
      this.stringTable.Deserialize(reader);
      this.noteEntries.Deserialize(reader);
    }

    private class StringTable
    {
      private Dictionary<int, string> strings = new Dictionary<int, string>();

      public int AddString(string str)
      {
        HashedString hashedString = new HashedString(str);
        this.strings[hashedString.HashValue] = str;
        return hashedString.HashValue;
      }

      public string GetStringByHash(int hash)
      {
        string empty = string.Empty;
        this.strings.TryGetValue(hash, out empty);
        return empty;
      }

      public void Serialize(BinaryWriter writer)
      {
        writer.Write(this.strings.Count);
        foreach (KeyValuePair<int, string> keyValuePair in this.strings)
          writer.Write(keyValuePair.Value);
      }

      public void Deserialize(BinaryReader reader)
      {
        int num = reader.ReadInt32();
        for (int index = 0; index < num; ++index)
          this.AddString(reader.ReadString());
      }
    }

    private class NoteEntries
    {
      private List<ReportManager.NoteStorage.NoteEntries.NoteStorageBlock> storageBlocks = new List<ReportManager.NoteStorage.NoteEntries.NoteStorageBlock>();
      private const int REPORT_IDS_PER_BLOCK = 100;

      public void Add(int report_entry_id, float value, int note_id)
      {
        int storageBlockIdx = this.ReportEntryIdToStorageBlockIdx(report_entry_id);
        while (storageBlockIdx >= this.storageBlocks.Count)
          this.storageBlocks.Add(new ReportManager.NoteStorage.NoteEntries.NoteStorageBlock(32));
        ReportManager.NoteStorage.NoteEntries.NoteStorageBlock storageBlock = this.storageBlocks[storageBlockIdx];
        storageBlock.Add(report_entry_id, value, note_id);
        this.storageBlocks[storageBlockIdx] = storageBlock;
      }

      public void Serialize(BinaryWriter writer)
      {
        writer.Write(this.storageBlocks.Count);
        foreach (ReportManager.NoteStorage.NoteEntries.NoteStorageBlock storageBlock in this.storageBlocks)
          storageBlock.Serialize(writer);
      }

      public void Deserialize(BinaryReader reader)
      {
        int num = reader.ReadInt32();
        for (int index = 0; index < num; ++index)
        {
          ReportManager.NoteStorage.NoteEntries.NoteStorageBlock noteStorageBlock = new ReportManager.NoteStorage.NoteEntries.NoteStorageBlock();
          noteStorageBlock.Deserialize(reader);
          this.storageBlocks.Add(noteStorageBlock);
        }
      }

      private int ReportEntryIdToStorageBlockIdx(int report_entry_id)
      {
        return report_entry_id / 100;
      }

      public void IterateNotes(
        ReportManager.NoteStorage.StringTable string_table,
        int report_entry_id,
        System.Action<ReportManager.ReportEntry.Note> callback)
      {
        int storageBlockIdx = this.ReportEntryIdToStorageBlockIdx(report_entry_id);
        if (storageBlockIdx >= this.storageBlocks.Count)
          return;
        this.storageBlocks[storageBlockIdx].IterateNotes(string_table, report_entry_id, callback);
      }

      [StructLayout(LayoutKind.Explicit)]
      public struct NoteEntry
      {
        [FieldOffset(0)]
        public int reportEntryId;
        [FieldOffset(4)]
        public int noteHash;
        [FieldOffset(8)]
        public float value;

        public NoteEntry(int report_entry_id, int note_hash, float value)
        {
          this.reportEntryId = report_entry_id;
          this.noteHash = note_hash;
          this.value = value;
        }

        public bool Matches(int report_entry_id, int note_hash, float value)
        {
          return report_entry_id == this.reportEntryId && note_hash == this.noteHash && (double) value > 0.0 == (double) this.value > 0.0;
        }
      }

      [StructLayout(LayoutKind.Explicit)]
      public struct NoteEntryArray
      {
        [FieldOffset(0)]
        public byte[] bytes;
        [FieldOffset(0)]
        public ReportManager.NoteStorage.NoteEntries.NoteEntry[] structs;

        public NoteEntryArray(int size_in_structs)
        {
          int length = size_in_structs * Marshal.SizeOf(typeof (ReportManager.NoteStorage.NoteEntries.NoteEntry));
          this.structs = (ReportManager.NoteStorage.NoteEntries.NoteEntry[]) null;
          this.bytes = new byte[length];
        }

        public int SizeInStructs
        {
          get
          {
            return this.bytes.Length / Marshal.SizeOf(typeof (ReportManager.NoteStorage.NoteEntries.NoteEntry));
          }
        }

        public int StructSizeInBytes
        {
          get
          {
            return Marshal.SizeOf(typeof (ReportManager.NoteStorage.NoteEntries.NoteEntry));
          }
        }

        public void Resize(int size_in_structs)
        {
          byte[] bytes = this.bytes;
          this.bytes = new byte[size_in_structs * Marshal.SizeOf(typeof (ReportManager.NoteStorage.NoteEntries.NoteEntry))];
          Buffer.BlockCopy((Array) bytes, 0, (Array) this.bytes, 0, bytes.Length);
        }
      }

      private struct NoteStorageBlock
      {
        private int entryCount;
        private ReportManager.NoteStorage.NoteEntries.NoteEntryArray entries;

        public NoteStorageBlock(int capacity)
        {
          this.entries = new ReportManager.NoteStorage.NoteEntries.NoteEntryArray(capacity);
          this.entryCount = 0;
        }

        public void Add(int report_entry_id, float value, int note_id)
        {
          bool flag = false;
          for (int index = 0; index < this.entryCount; ++index)
          {
            ReportManager.NoteStorage.NoteEntries.NoteEntry noteEntry = this.entries.structs[index];
            if (noteEntry.Matches(report_entry_id, note_id, value))
            {
              noteEntry.value += value;
              this.entries.structs[index] = noteEntry;
              flag = true;
              break;
            }
          }
          if (flag)
            return;
          if (this.entries.SizeInStructs <= this.entryCount)
            this.entries.Resize(this.entries.SizeInStructs * 2);
          this.entries.structs[this.entryCount++] = new ReportManager.NoteStorage.NoteEntries.NoteEntry(report_entry_id, note_id, value);
        }

        public void IterateNotes(
          ReportManager.NoteStorage.StringTable string_table,
          int report_entry_id,
          System.Action<ReportManager.ReportEntry.Note> callback)
        {
          for (int index = 0; index < this.entryCount; ++index)
          {
            ReportManager.NoteStorage.NoteEntries.NoteEntry noteEntry = this.entries.structs[index];
            if (noteEntry.reportEntryId == report_entry_id)
            {
              string stringByHash = string_table.GetStringByHash(noteEntry.noteHash);
              ReportManager.ReportEntry.Note note = new ReportManager.ReportEntry.Note(noteEntry.value, stringByHash);
              callback(note);
            }
          }
        }

        public void Serialize(BinaryWriter writer)
        {
          writer.Write(this.entryCount);
          writer.Write(this.entries.bytes, 0, this.entries.StructSizeInBytes * this.entryCount);
        }

        public void Deserialize(BinaryReader reader)
        {
          this.entryCount = reader.ReadInt32();
          this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
        }
      }
    }
  }
}
