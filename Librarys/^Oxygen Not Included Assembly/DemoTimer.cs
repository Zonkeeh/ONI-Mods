// Decompiled with JetBrains decompiler
// Type: DemoTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using UnityEngine;
using UnityEngine.UI;

public class DemoTimer : MonoBehaviour
{
  private float beginTime = -1f;
  private Color fadeOutColor = new Color();
  public static DemoTimer Instance;
  public LocText labelText;
  public Image clockImage;
  public GameObject Prefab_DemoOverScreen;
  public GameObject Prefab_FadeOutScreen;
  private float duration;
  private float elapsed;
  private bool demoOver;
  public bool CountdownActive;
  private GameObject fadeOutScreen;

  public static void DestroyInstance()
  {
    DemoTimer.Instance = (DemoTimer) null;
  }

  private void Start()
  {
    DemoTimer.Instance = this;
    if (GenericGameSettings.instance != null)
    {
      if (GenericGameSettings.instance.demoMode)
      {
        this.duration = (float) GenericGameSettings.instance.demoTime;
        this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
        this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
      }
      else
        this.gameObject.SetActive(false);
    }
    else
      this.gameObject.SetActive(false);
    this.duration = (float) GenericGameSettings.instance.demoTime;
    this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
    Image component = this.fadeOutScreen.GetComponent<Image>();
    component.raycastTarget = false;
    this.fadeOutColor = component.color;
    this.fadeOutColor.a = 0.0f;
    this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
  }

  private void Update()
  {
    if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
    {
      this.CountdownActive = !this.CountdownActive;
      this.UpdateLabel();
    }
    if (this.demoOver || !this.CountdownActive)
      return;
    if ((double) this.beginTime == -1.0)
      this.beginTime = Time.unscaledTime;
    this.elapsed = Mathf.Clamp(0.0f, Time.unscaledTime - this.beginTime, this.duration);
    if ((double) this.elapsed + 5.0 >= (double) this.duration)
    {
      this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt((float) (((double) this.duration - (double) this.elapsed) / 5.0)));
      this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
    }
    if ((double) this.elapsed >= (double) this.duration)
      this.EndDemo();
    this.UpdateLabel();
  }

  private void UpdateLabel()
  {
    int num1 = Mathf.RoundToInt(this.duration - this.elapsed);
    int num2 = Mathf.FloorToInt((float) (num1 / 60));
    int num3 = num1 % 60;
    this.labelText.text = (string) STRINGS.UI.DEMOOVERSCREEN.TIMEREMAINING + " " + num2.ToString("00") + ":" + num3.ToString("00");
    if (this.CountdownActive)
      return;
    this.labelText.text = (string) STRINGS.UI.DEMOOVERSCREEN.TIMERINACTIVE;
  }

  public void EndDemo()
  {
    if (this.demoOver)
      return;
    this.demoOver = true;
    Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
  }
}
