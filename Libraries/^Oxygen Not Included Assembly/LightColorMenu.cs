// Decompiled with JetBrains decompiler
// Type: LightColorMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class LightColorMenu : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<LightColorMenu> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LightColorMenu>((System.Action<LightColorMenu, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  public LightColorMenu.LightColor[] lightColors;
  private int currentColor;

  protected override void OnPrefabInit()
  {
    this.Subscribe<LightColorMenu>(493375141, LightColorMenu.OnRefreshUserMenuDelegate);
    this.SetColor(0);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.lightColors.Length <= 0)
      return;
    int length = this.lightColors.Length;
    for (int index = 0; index < length; ++index)
    {
      if (index != this.currentColor)
      {
        int new_color = index;
        Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo(this.lightColors[index].name, this.lightColors[index].name, (System.Action) (() => this.SetColor(new_color)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, string.Empty, true), 1f);
      }
    }
  }

  private void SetColor(int color_index)
  {
    if (this.lightColors.Length > 0 && color_index < this.lightColors.Length)
    {
      foreach (Light2D componentsInChild in this.GetComponentsInChildren<Light2D>(true))
        componentsInChild.Color = this.lightColors[color_index].color;
      foreach (Renderer componentsInChild in this.GetComponentsInChildren<MeshRenderer>(true))
      {
        foreach (Material material in componentsInChild.materials)
        {
          if (material.name.StartsWith("matScriptedGlow01"))
            material.color = this.lightColors[color_index].color;
        }
      }
    }
    this.currentColor = color_index;
  }

  [Serializable]
  public struct LightColor
  {
    public string name;
    public Color color;

    public LightColor(string name, Color color)
    {
      this.name = name;
      this.color = color;
    }
  }
}
