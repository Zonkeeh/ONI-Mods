// Decompiled with JetBrains decompiler
// Type: DestinationAsteroid2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class DestinationAsteroid2 : KMonoBehaviour
{
  [SerializeField]
  private Image asteroidImage;
  [SerializeField]
  private KButton button;
  private ColonyDestinationAsteroidData asteroidData;

  public event System.Action<ColonyDestinationAsteroidData> OnClicked;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.OnClickInternal);
  }

  public void SetAsteroid(ColonyDestinationAsteroidData newAsteroidData)
  {
    if (newAsteroidData == this.asteroidData)
      return;
    this.asteroidData = newAsteroidData;
    this.asteroidImage.sprite = Assets.GetSprite((HashedString) this.asteroidData.sprite);
  }

  private void OnClickInternal()
  {
    DebugUtil.LogArgs((object) "Clicked asteroid", (object) this.asteroidData.worldPath);
    this.OnClicked(this.asteroidData);
  }
}
