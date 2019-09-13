// Decompiled with JetBrains decompiler
// Type: UserNavigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserNavigation : KMonoBehaviour
{
  [Serialize]
  private List<UserNavigation.NavPoint> hotkeyNavPoints = new List<UserNavigation.NavPoint>();

  public UserNavigation()
  {
    for (Action action = Action.SetUserNav1; action <= Action.SetUserNav10; ++action)
      this.hotkeyNavPoints.Add(UserNavigation.NavPoint.Invalid);
  }

  private static int GetIndex(Action action)
  {
    int num = -1;
    if (Action.SetUserNav1 <= action && action <= Action.SetUserNav10)
      num = (int) (action - 15);
    else if (Action.GotoUserNav1 <= action && action <= Action.GotoUserNav10)
      num = (int) (action - 25);
    return num;
  }

  private void SetHotkeyNavPoint(Action action, Vector3 pos, float ortho_size)
  {
    int index = UserNavigation.GetIndex(action);
    if (index < 0)
      return;
    this.hotkeyNavPoints[index] = new UserNavigation.NavPoint()
    {
      pos = pos,
      orthoSize = ortho_size
    };
    EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_set", false), CameraController.Instance.GetVerticallyScaledPosition((Vector2) pos));
    int num = (int) instance.setParameterValue("userNavPoint_ID", (float) index);
    KFMOD.EndOneShot(instance);
  }

  private void GoToHotkeyNavPoint(Action action)
  {
    int index = UserNavigation.GetIndex(action);
    if (index < 0)
      return;
    UserNavigation.NavPoint hotkeyNavPoint = this.hotkeyNavPoints[index];
    if (!hotkeyNavPoint.IsValid())
      return;
    CameraController instance1 = CameraController.Instance;
    instance1.SetTargetPos(hotkeyNavPoint.pos, hotkeyNavPoint.orthoSize, true);
    EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound("UserNavPoint_recall", false), instance1.GetVerticallyScaledPosition((Vector2) instance1.transform.GetPosition()));
    int num = (int) instance2.setParameterValue("userNavPoint_ID", (float) index);
    KFMOD.EndOneShot(instance2);
  }

  public bool Handle(KButtonEvent e)
  {
    bool flag = false;
    for (Action action = Action.GotoUserNav1; action <= Action.GotoUserNav10; ++action)
    {
      if (e.TryConsume(action))
      {
        this.GoToHotkeyNavPoint(action);
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      for (Action action = Action.SetUserNav1; action <= Action.SetUserNav10; ++action)
      {
        if (e.TryConsume(action))
        {
          Camera baseCamera = CameraController.Instance.baseCamera;
          Vector3 position = baseCamera.transform.GetPosition();
          this.SetHotkeyNavPoint(action, position, baseCamera.orthographicSize);
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  [Serializable]
  private struct NavPoint
  {
    public static readonly UserNavigation.NavPoint Invalid = new UserNavigation.NavPoint()
    {
      pos = Vector3.zero,
      orthoSize = 0.0f
    };
    public Vector3 pos;
    public float orthoSize;

    public bool IsValid()
    {
      return (double) this.orthoSize != 0.0;
    }
  }
}
