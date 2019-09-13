// Decompiled with JetBrains decompiler
// Type: ReportScreenEntryRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportScreenEntryRow : KMonoBehaviour
{
  private static List<ReportManager.ReportEntry.Note> notes = new List<ReportManager.ReportEntry.Note>();
  private float addedValue = float.NegativeInfinity;
  private float removedValue = float.NegativeInfinity;
  private float netValue = float.NegativeInfinity;
  private float nameWidth = 164f;
  private float indentWidth = 6f;
  [SerializeField]
  public LocText name;
  [SerializeField]
  public LocText added;
  [SerializeField]
  public LocText removed;
  [SerializeField]
  public LocText net;
  [SerializeField]
  public MultiToggle toggle;
  [SerializeField]
  private LayoutElement spacer;
  [SerializeField]
  private Image bgImage;
  public float groupSpacerWidth;
  public float contextSpacerWidth;
  [SerializeField]
  private Color oddRowColor;
  private ReportManager.ReportEntry entry;
  private ReportManager.ReportGroup reportGroup;

  private List<ReportManager.ReportEntry.Note> Sort(
    List<ReportManager.ReportEntry.Note> notes,
    ReportManager.ReportEntry.Order order)
  {
    switch (order)
    {
      case ReportManager.ReportEntry.Order.Ascending:
        notes.Sort((Comparison<ReportManager.ReportEntry.Note>) ((x, y) => x.value.CompareTo(y.value)));
        break;
      case ReportManager.ReportEntry.Order.Descending:
        notes.Sort((Comparison<ReportManager.ReportEntry.Note>) ((x, y) => y.value.CompareTo(x.value)));
        break;
    }
    return notes;
  }

  public static void DestroyStatics()
  {
    ReportScreenEntryRow.notes = (List<ReportManager.ReportEntry.Note>) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.added.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnPositiveNoteTooltip);
    this.removed.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNegativeNoteTooltip);
    this.net.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
    this.name.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
  }

  private string OnNoteTooltip(
    float total_accumulation,
    string tooltip_text,
    ReportManager.ReportEntry.Order order,
    ReportManager.FormattingFn format_fn,
    Func<ReportManager.ReportEntry.Note, bool> is_note_applicable_cb,
    ReportManager.GroupFormattingFn group_format_fn = null)
  {
    ReportScreenEntryRow.notes.Clear();
    this.entry.IterateNotes((System.Action<ReportManager.ReportEntry.Note>) (note =>
    {
      if (!is_note_applicable_cb(note))
        return;
      ReportScreenEntryRow.notes.Add(note);
    }));
    string str1 = string.Empty;
    float numEntries = Mathf.Max(this.entry.contextEntries.Count <= 0 ? (float) ReportScreenEntryRow.notes.Count : (float) this.entry.contextEntries.Count, 1f);
    foreach (ReportManager.ReportEntry.Note note in this.Sort(ReportScreenEntryRow.notes, this.reportGroup.posNoteOrder))
    {
      string str2 = format_fn(note.value);
      if (this.toggle.gameObject.activeInHierarchy && group_format_fn != null)
        str2 = group_format_fn(note.value, numEntries);
      str1 = string.Format((string) STRINGS.UI.ENDOFDAYREPORT.NOTES.NOTE_ENTRY_LINE_ITEM, (object) str1, (object) note.note, (object) str2);
    }
    string str3 = format_fn(total_accumulation);
    if (group_format_fn != null && this.entry.context == null)
      str3 = group_format_fn(total_accumulation, numEntries);
    return string.Format(tooltip_text + "\n" + str1, (object) str3);
  }

  private string OnNegativeNoteTooltip()
  {
    return this.OnNoteTooltip(this.entry.Negative, this.reportGroup.negativeTooltip, this.reportGroup.negNoteOrder, this.reportGroup.formatfn, (Func<ReportManager.ReportEntry.Note, bool>) (note => this.IsNegativeNote(note)), this.reportGroup.groupFormatfn);
  }

  private string OnPositiveNoteTooltip()
  {
    return this.OnNoteTooltip(this.entry.Positive, this.reportGroup.positiveTooltip, this.reportGroup.posNoteOrder, this.reportGroup.formatfn, (Func<ReportManager.ReportEntry.Note, bool>) (note => this.IsPositiveNote(note)), this.reportGroup.groupFormatfn);
  }

  private string OnNetNoteTooltip()
  {
    if ((double) this.entry.Net > 0.0)
      return this.OnPositiveNoteTooltip();
    return this.OnNegativeNoteTooltip();
  }

  private bool IsPositiveNote(ReportManager.ReportEntry.Note note)
  {
    return (double) note.value > 0.0;
  }

  private bool IsNegativeNote(ReportManager.ReportEntry.Note note)
  {
    return (double) note.value < 0.0;
  }

  public void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
  {
    this.entry = entry;
    this.reportGroup = reportGroup;
    ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList pos_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
    entry.IterateNotes((System.Action<ReportManager.ReportEntry.Note>) (note =>
    {
      if (!this.IsPositiveNote(note))
        return;
      pos_notes.Add(note);
    }));
    ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList neg_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
    entry.IterateNotes((System.Action<ReportManager.ReportEntry.Note>) (note =>
    {
      if (!this.IsNegativeNote(note))
        return;
      neg_notes.Add(note);
    }));
    LayoutElement component = this.name.GetComponent<LayoutElement>();
    if (entry.context == null)
    {
      LayoutElement layoutElement = component;
      float nameWidth = this.nameWidth;
      component.preferredWidth = nameWidth;
      double num = (double) nameWidth;
      layoutElement.minWidth = (float) num;
      if (entry.HasContextEntries())
      {
        this.toggle.gameObject.SetActive(true);
        this.spacer.minWidth = this.groupSpacerWidth;
      }
      else
      {
        this.toggle.gameObject.SetActive(false);
        this.spacer.minWidth = this.groupSpacerWidth + this.toggle.GetComponent<LayoutElement>().minWidth;
      }
      this.name.text = reportGroup.stringKey;
    }
    else
    {
      this.toggle.gameObject.SetActive(false);
      this.spacer.minWidth = this.contextSpacerWidth;
      this.name.text = entry.context;
      LayoutElement layoutElement = component;
      float num1 = this.nameWidth - this.indentWidth;
      component.preferredWidth = num1;
      double num2 = (double) num1;
      layoutElement.minWidth = (float) num2;
      if (this.transform.GetSiblingIndex() % 2 != 0)
        this.bgImage.color = this.oddRowColor;
    }
    if ((double) this.addedValue != (double) entry.Positive)
    {
      string str = reportGroup.formatfn(entry.Positive);
      if (reportGroup.groupFormatfn != null && entry.context == null)
      {
        float numEntries = Mathf.Max(entry.contextEntries.Count <= 0 ? (float) pos_notes.Count : (float) entry.contextEntries.Count, 1f);
        str = reportGroup.groupFormatfn(entry.Positive, numEntries);
      }
      this.added.text = str;
      this.addedValue = entry.Positive;
    }
    if ((double) this.removedValue != (double) entry.Negative)
    {
      string str = reportGroup.formatfn(entry.Negative);
      if (reportGroup.groupFormatfn != null && entry.context == null)
      {
        float numEntries = Mathf.Max(entry.contextEntries.Count <= 0 ? (float) neg_notes.Count : (float) entry.contextEntries.Count, 1f);
        str = reportGroup.groupFormatfn(entry.Negative, numEntries);
      }
      this.removed.text = str;
      this.removedValue = entry.Negative;
    }
    if ((double) this.netValue != (double) entry.Net)
    {
      string str = reportGroup.formatfn != null ? reportGroup.formatfn(entry.Net) : entry.Net.ToString();
      if (reportGroup.groupFormatfn != null && entry.context == null)
      {
        float numEntries = Mathf.Max(entry.contextEntries.Count <= 0 ? (float) (pos_notes.Count + neg_notes.Count) : (float) entry.contextEntries.Count, 1f);
        str = reportGroup.groupFormatfn(entry.Net, numEntries);
      }
      this.net.text = str;
      this.netValue = entry.Net;
    }
    pos_notes.Recycle();
    neg_notes.Recycle();
  }
}
