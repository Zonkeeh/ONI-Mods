// Decompiled with JetBrains decompiler
// Type: DiseaseContainers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI.DiseaseGrowthRules;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseContainers : KGameObjectSplitComponentManager<DiseaseHeader, DiseaseContainer>
{
  public HandleVector<int>.Handle Add(
    GameObject go,
    byte disease_idx,
    int disease_count)
  {
    DiseaseHeader header = new DiseaseHeader()
    {
      diseaseIdx = disease_idx,
      diseaseCount = disease_count,
      primaryElement = go.GetComponent<PrimaryElement>()
    };
    DiseaseContainer diseaseContainer = new DiseaseContainer(go, header.primaryElement.Element.idx);
    if (disease_idx != byte.MaxValue)
      this.EvaluateGrowthConstants(header, ref diseaseContainer);
    return this.Add(go, header, ref diseaseContainer);
  }

  protected override void OnCleanUp(HandleVector<int>.Handle h)
  {
    AutoDisinfectable autoDisinfectable = this.GetPayload(h).autoDisinfectable;
    if ((UnityEngine.Object) autoDisinfectable != (UnityEngine.Object) null)
      AutoDisinfectableManager.Instance.RemoveAutoDisinfectable(autoDisinfectable);
    base.OnCleanUp(h);
  }

  public override void Sim200ms(float dt)
  {
    ListPool<int, DiseaseContainers>.PooledList pooledList = ListPool<int, DiseaseContainers>.Allocate();
    pooledList.Capacity = Math.Max(pooledList.Capacity, this.headers.Count);
    for (int index = 0; index < this.headers.Count; ++index)
    {
      DiseaseHeader header = this.headers[index];
      if (header.diseaseIdx != byte.MaxValue && (UnityEngine.Object) header.primaryElement != (UnityEngine.Object) null)
        pooledList.Add(index);
    }
    foreach (int index in (List<int>) pooledList)
    {
      DiseaseContainer payload = this.payloads[index];
      DiseaseHeader header = this.headers[index];
      Klei.AI.Disease disease = Db.Get().Diseases[(int) header.diseaseIdx];
      float num1 = DiseaseContainers.CalculateDelta(header, ref payload, disease, dt) + payload.accumulatedError;
      int num2 = (int) num1;
      payload.accumulatedError = num1 - (float) num2;
      if (header.diseaseCount > payload.overpopulationCount != header.diseaseCount + num2 > payload.overpopulationCount)
        this.EvaluateGrowthConstants(header, ref payload);
      header.diseaseCount += num2;
      if (header.diseaseCount <= 0)
      {
        payload.accumulatedError = 0.0f;
        header.diseaseCount = 0;
        header.diseaseIdx = byte.MaxValue;
      }
      this.headers[index] = header;
      this.payloads[index] = payload;
    }
    pooledList.Recycle();
  }

  public static float CalculateDelta(
    DiseaseHeader header,
    ref DiseaseContainer container,
    Klei.AI.Disease disease,
    float dt)
  {
    return DiseaseContainers.CalculateDelta(header.diseaseCount, (int) container.elemIdx, header.primaryElement.Mass, Grid.PosToCell(header.primaryElement.transform.GetPosition()), header.primaryElement.Temperature, container.instanceGrowthRate, disease, dt);
  }

  public static float CalculateDelta(
    int disease_count,
    int element_idx,
    float mass,
    int environment_cell,
    float temperature,
    float tags_multiplier_base,
    Klei.AI.Disease disease,
    float dt)
  {
    float num1 = 0.0f + disease.elemGrowthInfo[element_idx].CalculateDiseaseCountDelta(disease_count, mass, dt);
    float growthRate = Klei.AI.Disease.HalfLifeToGrowthRate(Klei.AI.Disease.CalculateRangeHalfLife(temperature, ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
    float num2 = num1 + ((float) disease_count * growthRate - (float) disease_count);
    float num3 = Mathf.Pow(tags_multiplier_base, dt);
    float num4 = num2 + ((float) disease_count * num3 - (float) disease_count);
    if (Grid.IsValidCell(environment_cell))
    {
      byte num5 = Grid.ElementIdx[environment_cell];
      ElemExposureInfo elemExposureInfo = disease.elemExposureInfo[(int) num5];
      num4 += elemExposureInfo.CalculateExposureDiseaseCountDelta(disease_count, dt);
    }
    return num4;
  }

  public int ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
  {
    DiseaseHeader header = this.GetHeader(h);
    header.diseaseCount = Math.Max(0, header.diseaseCount + disease_count_delta);
    if (header.diseaseCount == 0)
    {
      header.diseaseIdx = byte.MaxValue;
      DiseaseContainer payload = this.GetPayload(h);
      payload.accumulatedError = 0.0f;
      this.SetPayload(h, ref payload);
    }
    this.SetHeader(h, header);
    return header.diseaseCount;
  }

  public int AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
  {
    DiseaseHeader header;
    DiseaseContainer payload;
    this.GetData(h, out header, out payload);
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, header.diseaseIdx, header.diseaseCount);
    bool flag = (int) header.diseaseIdx != (int) finalDiseaseInfo.idx;
    header.diseaseIdx = finalDiseaseInfo.idx;
    header.diseaseCount = finalDiseaseInfo.count;
    if (flag && finalDiseaseInfo.idx != byte.MaxValue)
    {
      this.EvaluateGrowthConstants(header, ref payload);
      this.SetData(h, header, ref payload);
    }
    else
      this.SetHeader(h, header);
    if (flag)
      header.primaryElement.Trigger(-283306403, (object) null);
    return header.diseaseCount;
  }

  private void GetVisualDiseaseIdxAndCount(
    DiseaseHeader header,
    ref DiseaseContainer payload,
    out int disease_idx,
    out int disease_count)
  {
    if ((UnityEngine.Object) payload.visualDiseaseProvider == (UnityEngine.Object) null)
    {
      disease_idx = (int) header.diseaseIdx;
      disease_count = header.diseaseCount;
    }
    else
    {
      disease_idx = (int) byte.MaxValue;
      disease_count = 0;
      HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(payload.visualDiseaseProvider);
      if (!(handle != HandleVector<int>.InvalidHandle))
        return;
      DiseaseHeader header1 = GameComps.DiseaseContainers.GetHeader(handle);
      disease_idx = (int) header1.diseaseIdx;
      disease_count = header1.diseaseCount;
    }
  }

  public void UpdateOverlayColours()
  {
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    Diseases diseases = Db.Get().Diseases;
    Color32 color32_1 = new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
    for (int index1 = 0; index1 < this.headers.Count; ++index1)
    {
      DiseaseContainer payload = this.payloads[index1];
      DiseaseHeader header1 = this.headers[index1];
      KBatchedAnimController controller = payload.controller;
      if ((UnityEngine.Object) controller != (UnityEngine.Object) null)
      {
        Color32 color32_2 = color32_1;
        Vector3 position = controller.transform.GetPosition();
        if (visibleArea.Min <= (Vector2) position && (Vector2) position <= visibleArea.Max)
        {
          int count = 0;
          int disease_idx = (int) byte.MaxValue;
          int disease_count = 0;
          this.GetVisualDiseaseIdxAndCount(header1, ref payload, out disease_idx, out disease_count);
          if (disease_idx != (int) byte.MaxValue)
          {
            color32_2 = diseases[disease_idx].overlayColour;
            count = disease_count;
          }
          if (payload.isContainer)
          {
            List<GameObject> items = header1.primaryElement.GetComponent<Storage>().items;
            for (int index2 = 0; index2 < items.Count; ++index2)
            {
              GameObject gameObject = items[index2];
              if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
              {
                HandleVector<int>.Handle handle = this.GetHandle(gameObject);
                if (handle.IsValid())
                {
                  DiseaseHeader header2 = this.GetHeader(handle);
                  if (header2.diseaseCount > count && header2.diseaseIdx != byte.MaxValue)
                  {
                    count = header2.diseaseCount;
                    color32_2 = diseases[(int) header2.diseaseIdx].overlayColour;
                  }
                }
              }
            }
          }
          color32_2.a = SimUtil.DiseaseCountToAlpha254(count);
          if (payload.conduitType != ConduitType.None)
          {
            ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(payload.conduitType).GetContents(Grid.PosToCell(position));
            if (contents.diseaseIdx != byte.MaxValue && contents.diseaseCount > count)
            {
              int diseaseCount = contents.diseaseCount;
              color32_2 = diseases[(int) contents.diseaseIdx].overlayColour;
              color32_2.a = byte.MaxValue;
            }
          }
        }
        controller.OverlayColour = (Color) color32_2;
      }
    }
  }

  private void EvaluateGrowthConstants(DiseaseHeader header, ref DiseaseContainer container)
  {
    Klei.AI.Disease disease = Db.Get().Diseases[(int) header.diseaseIdx];
    KPrefabID component = header.primaryElement.GetComponent<KPrefabID>();
    ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[(int) header.diseaseIdx];
    container.overpopulationCount = (int) ((double) elemGrowthInfo.maxCountPerKG * (double) header.primaryElement.Mass);
    container.instanceGrowthRate = disease.GetGrowthRateForTags(component.Tags, header.diseaseCount > container.overpopulationCount);
  }

  public override void Clear()
  {
    base.Clear();
    for (int index = 0; index < this.payloads.Count; ++index)
      this.payloads[index].Clear();
    this.headers.Clear();
    this.payloads.Clear();
    this.handles.Clear();
  }
}
