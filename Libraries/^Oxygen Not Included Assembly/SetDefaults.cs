// Decompiled with JetBrains decompiler
// Type: SetDefaults
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class SetDefaults
{
  public static void Initialize()
  {
    KSlider.DefaultSounds[0] = GlobalAssets.GetSound("Slider_Start", false);
    KSlider.DefaultSounds[1] = GlobalAssets.GetSound("Slider_Move", false);
    KSlider.DefaultSounds[2] = GlobalAssets.GetSound("Slider_End", false);
    KSlider.DefaultSounds[3] = GlobalAssets.GetSound("Slider_Boundary_Low", false);
    KSlider.DefaultSounds[4] = GlobalAssets.GetSound("Slider_Boundary_High", false);
    KScrollRect.DefaultSounds[KScrollRect.SoundType.OnMouseScroll] = GlobalAssets.GetSound("Mousewheel_Move", false);
    // ISSUE: reference to a compiler-generated field
    if (SetDefaults.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SetDefaults.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(SetDefaults.GetSoundPath);
    }
    // ISSUE: reference to a compiler-generated field
    WidgetSoundPlayer.getSoundPath = SetDefaults.\u003C\u003Ef__mg\u0024cache0;
  }

  private static string GetSoundPath(string sound_name)
  {
    return GlobalAssets.GetSound(sound_name, false);
  }
}
