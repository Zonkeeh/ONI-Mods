// Decompiled with JetBrains decompiler
// Type: Reservable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Reservable : KMonoBehaviour
{
  private GameObject reservedBy;

  public GameObject ReservedBy
  {
    get
    {
      return this.reservedBy;
    }
  }

  public bool isReserved
  {
    get
    {
      return !((Object) this.reservedBy == (Object) null);
    }
  }

  public bool Reserve(GameObject reserver)
  {
    if (!((Object) this.reservedBy == (Object) null))
      return false;
    this.reservedBy = reserver;
    return true;
  }

  public void ClearReservation(GameObject reserver)
  {
    if (!((Object) this.reservedBy == (Object) reserver))
      return;
    this.reservedBy = (GameObject) null;
  }
}
