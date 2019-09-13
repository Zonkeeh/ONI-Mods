// Decompiled with JetBrains decompiler
// Type: AttachableBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableBuilding : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<AttachableBuilding> AttachmentNetworkChangedDelegate = new EventSystem.IntraObjectHandler<AttachableBuilding>((System.Action<AttachableBuilding, object>) ((component, data) => component.AttachmentNetworkChanged(data)));
  public Tag attachableToTag;
  public System.Action<AttachableBuilding> onAttachmentNetworkChanged;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.RegisterWithAttachPoint(true);
    Components.AttachableBuildings.Add(this);
    this.Subscribe<AttachableBuilding>(486707561, AttachableBuilding.AttachmentNetworkChangedDelegate);
    foreach (GameObject go in AttachableBuilding.GetAttachedNetwork(this))
      go.Trigger(486707561, (object) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  private void AttachmentNetworkChanged(object attachableBuilding)
  {
    if (this.onAttachmentNetworkChanged == null)
      return;
    this.onAttachmentNetworkChanged((AttachableBuilding) attachableBuilding);
  }

  public void RegisterWithAttachPoint(bool register)
  {
    int num = Grid.OffsetCell(Grid.PosToCell(this.gameObject), Assets.GetBuildingDef(this.GetComponent<KPrefabID>().PrefabID().Name).attachablePosition);
    bool flag = false;
    for (int index1 = 0; !flag && index1 < Components.BuildingAttachPoints.Count; ++index1)
    {
      for (int index2 = 0; index2 < Components.BuildingAttachPoints[index1].points.Length; ++index2)
      {
        if (num == Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) Components.BuildingAttachPoints[index1]), Components.BuildingAttachPoints[index1].points[index2].position))
        {
          Components.BuildingAttachPoints[index1].points[index2].attachedBuilding = !register ? (AttachableBuilding) null : this;
          flag = true;
          break;
        }
      }
    }
  }

  public static List<GameObject> GetAttachedNetwork(AttachableBuilding tip)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    gameObjectList.Add(tip.gameObject);
    AttachableBuilding attachableBuilding = tip;
    while ((UnityEngine.Object) attachableBuilding != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
      attachableBuilding = (AttachableBuilding) null;
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        gameObjectList.Add(attachedTo.gameObject);
        attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
      }
    }
    BuildingAttachPoint buildingAttachPoint = tip.GetComponent<BuildingAttachPoint>();
    while ((UnityEngine.Object) buildingAttachPoint != (UnityEngine.Object) null)
    {
      bool flag = false;
      foreach (BuildingAttachPoint.HardPoint point in buildingAttachPoint.points)
      {
        if (!flag)
        {
          if ((UnityEngine.Object) point.attachedBuilding != (UnityEngine.Object) null)
          {
            IEnumerator enumerator = Components.AttachableBuildings.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                AttachableBuilding current = (AttachableBuilding) enumerator.Current;
                if ((UnityEngine.Object) current == (UnityEngine.Object) point.attachedBuilding)
                {
                  gameObjectList.Add(current.gameObject);
                  buildingAttachPoint = current.GetComponent<BuildingAttachPoint>();
                  flag = true;
                }
              }
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
        }
        else
          break;
      }
      if (!flag)
        buildingAttachPoint = (BuildingAttachPoint) null;
    }
    return gameObjectList;
  }

  public BuildingAttachPoint GetAttachedTo()
  {
    for (int index1 = 0; index1 < Components.BuildingAttachPoints.Count; ++index1)
    {
      for (int index2 = 0; index2 < Components.BuildingAttachPoints[index1].points.Length; ++index2)
      {
        if ((UnityEngine.Object) Components.BuildingAttachPoints[index1].points[index2].attachedBuilding == (UnityEngine.Object) this)
          return Components.BuildingAttachPoints[index1];
      }
    }
    return (BuildingAttachPoint) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.RegisterWithAttachPoint(false);
    Components.AttachableBuildings.Remove(this);
  }
}
