// Decompiled with JetBrains decompiler
// Type: MeterController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class MeterController
{
  public GameObject gameObject;
  private KAnimLink link;

  public MeterController(
    KMonoBehaviour target,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    params string[] symbols_to_hide)
  {
    string[] strArray = new string[symbols_to_hide.Length + 1];
    Array.Copy((Array) symbols_to_hide, (Array) strArray, symbols_to_hide.Length);
    strArray[strArray.Length - 1] = "meter_target";
    this.Initialize((KAnimControllerBase) target.GetComponent<KBatchedAnimController>(), "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, strArray);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    params string[] symbols_to_hide)
  {
    this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    Vector3 tracker_offset,
    params string[] symbols_to_hide)
  {
    this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    KBatchedAnimController meter_controller,
    params string[] symbol_names)
  {
    if ((UnityEngine.Object) meter_controller == (UnityEngine.Object) null)
      return;
    this.meterController = meter_controller;
    this.link = new KAnimLink(building_controller, (KAnimControllerBase) meter_controller);
    for (int index = 0; index < symbol_names.Length; ++index)
      building_controller.SetSymbolVisiblity((KAnimHashedString) symbol_names[index], false);
    this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
  }

  public KBatchedAnimController meterController { get; private set; }

  private void Initialize(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    Vector3 tracker_offset,
    params string[] symbols_to_hide)
  {
    string name = building_controller.name + "." + meter_animation;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab((Tag) MeterConfig.ID));
    gameObject.name = name;
    gameObject.SetActive(false);
    gameObject.transform.parent = building_controller.transform;
    this.gameObject = gameObject;
    gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
    Vector3 position = building_controller.transform.GetPosition();
    switch (front_back)
    {
      case Meter.Offset.Infront:
        position.z = building_controller.transform.GetPosition().z - 0.1f;
        break;
      case Meter.Offset.Behind:
        position.z = building_controller.transform.GetPosition().z + 0.1f;
        break;
      case Meter.Offset.UserSpecified:
        position.z = Grid.GetLayerZ(user_specified_render_layer);
        break;
    }
    gameObject.transform.SetPosition(position);
    KBatchedAnimController component1 = gameObject.GetComponent<KBatchedAnimController>();
    component1.AnimFiles = new KAnimFile[1]
    {
      building_controller.AnimFiles[0]
    };
    component1.initialAnim = meter_animation;
    component1.fgLayer = Grid.SceneLayer.NoLayer;
    component1.initialMode = KAnim.PlayMode.Paused;
    component1.isMovable = true;
    component1.FlipX = building_controller.FlipX;
    component1.FlipY = building_controller.FlipY;
    if (front_back == Meter.Offset.UserSpecified)
      component1.sceneLayer = user_specified_render_layer;
    this.meterController = component1;
    KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
    component2.offset = tracker_offset;
    component2.symbol = new HashedString(meter_target);
    gameObject.SetActive(true);
    building_controller.SetSymbolVisiblity((KAnimHashedString) meter_target, false);
    if (symbols_to_hide != null)
    {
      for (int index = 0; index < symbols_to_hide.Length; ++index)
        building_controller.SetSymbolVisiblity((KAnimHashedString) symbols_to_hide[index], false);
    }
    this.link = new KAnimLink(building_controller, (KAnimControllerBase) component1);
  }

  public void SetPositionPercent(float percent_full)
  {
    if ((UnityEngine.Object) this.meterController == (UnityEngine.Object) null)
      return;
    this.meterController.SetPositionPercent(percent_full);
  }

  public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
  {
    if (!((UnityEngine.Object) this.meterController != (UnityEngine.Object) null))
      return;
    this.meterController.SetSymbolTint(symbol, (Color) colour);
  }

  public void SetRotation(float rot)
  {
    if ((UnityEngine.Object) this.meterController == (UnityEngine.Object) null)
      return;
    this.meterController.Rotation = rot;
  }
}
