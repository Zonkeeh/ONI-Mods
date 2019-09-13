// Decompiled with JetBrains decompiler
// Type: MonumentPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MonumentPart : KMonoBehaviour
{
  public List<Tuple<string, string>> selectableStatesAndSymbols = new List<Tuple<string, string>>();
  public MonumentPart.Part part;
  public string stateUISymbol;
  [Serialize]
  private string chosenState;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.MonumentParts.Add(this);
    if (!string.IsNullOrEmpty(this.chosenState))
      this.SetState(this.chosenState);
    this.UpdateMonumentDecor();
  }

  protected override void OnCleanUp()
  {
    Components.MonumentParts.Remove(this);
    this.RemoveMonumentPiece();
    base.OnCleanUp();
  }

  public void SetState(string state)
  {
    this.GetComponent<KBatchedAnimController>().Play((HashedString) state, KAnim.PlayMode.Once, 1f, 0.0f);
    this.chosenState = state;
  }

  public bool IsMonumentCompleted()
  {
    bool flag1 = (Object) this.GetMonumentPart(MonumentPart.Part.Top) != (Object) null;
    bool flag2 = (Object) this.GetMonumentPart(MonumentPart.Part.Middle) != (Object) null;
    bool flag3 = (Object) this.GetMonumentPart(MonumentPart.Part.Bottom) != (Object) null;
    if (flag1 && flag3)
      return flag2;
    return false;
  }

  public void UpdateMonumentDecor()
  {
    GameObject monumentPart = this.GetMonumentPart(MonumentPart.Part.Middle);
    if (!this.IsMonumentCompleted())
      return;
    monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      if ((Object) gameObject != (Object) monumentPart)
        gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
    }
  }

  public void RemoveMonumentPiece()
  {
    if (!this.IsMonumentCompleted())
      return;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      if ((Object) gameObject.GetComponent<MonumentPart>() != (Object) this)
        gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
    }
  }

  private GameObject GetMonumentPart(MonumentPart.Part requestPart)
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      MonumentPart component = gameObject.GetComponent<MonumentPart>();
      if (!((Object) component == (Object) null) && component.part == requestPart)
        return gameObject;
    }
    return (GameObject) null;
  }

  public enum Part
  {
    Bottom,
    Middle,
    Top,
  }
}
