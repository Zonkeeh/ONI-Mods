// Decompiled with JetBrains decompiler
// Type: TimeOfDayPositioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TimeOfDayPositioner : KMonoBehaviour
{
  [SerializeField]
  private RectTransform targetRect;

  private void Update()
  {
    (this.transform as RectTransform).anchoredPosition = this.targetRect.anchoredPosition + new Vector2(Mathf.Round(GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width), 0.0f);
  }
}
