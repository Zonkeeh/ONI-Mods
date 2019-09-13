// Decompiled with JetBrains decompiler
// Type: HelmetController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HelmetController : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>((System.Action<HelmetController, object>) ((component, data) => component.OnEquipped(data)));
  private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>((System.Action<HelmetController, object>) ((component, data) => component.OnUnequipped(data)));
  public string anim_file;
  public bool has_jets;
  private bool is_shown;
  private bool in_tube;
  private bool is_flying;
  private Navigator owner_navigator;
  private GameObject jet_go;
  private GameObject glow_go;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
    this.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
  }

  private KBatchedAnimController GetAssigneeController()
  {
    Equippable component = this.GetComponent<Equippable>();
    if (component.assignee != null)
    {
      GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
      if ((bool) ((UnityEngine.Object) assigneeGameObject))
        return assigneeGameObject.GetComponent<KBatchedAnimController>();
    }
    return (KBatchedAnimController) null;
  }

  private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
  {
    GameObject gameObject = (GameObject) null;
    MinionAssignablesProxy assignablesProxy = ass_id as MinionAssignablesProxy;
    if ((bool) ((UnityEngine.Object) assignablesProxy))
    {
      gameObject = assignablesProxy.GetTargetGameObject();
    }
    else
    {
      MinionIdentity minionIdentity = ass_id as MinionIdentity;
      if ((bool) ((UnityEngine.Object) minionIdentity))
        gameObject = minionIdentity.gameObject;
    }
    return gameObject;
  }

  private void OnEquipped(object data)
  {
    Equippable component = this.GetComponent<Equippable>();
    this.ShowHelmet();
    GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
    assigneeGameObject.Subscribe(961737054, new System.Action<object>(this.OnBeginRecoverBreath));
    assigneeGameObject.Subscribe(-2037519664, new System.Action<object>(this.OnEndRecoverBreath));
    assigneeGameObject.Subscribe(1347184327, new System.Action<object>(this.OnPathAdvanced));
    this.in_tube = false;
    this.is_flying = false;
    this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
  }

  private void OnUnequipped(object data)
  {
    this.owner_navigator = (Navigator) null;
    Equippable component = this.GetComponent<Equippable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.HideHelmet();
    if (component.assignee == null)
      return;
    GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
    if (!(bool) ((UnityEngine.Object) assigneeGameObject))
      return;
    assigneeGameObject.Unsubscribe(961737054, new System.Action<object>(this.OnBeginRecoverBreath));
    assigneeGameObject.Unsubscribe(-2037519664, new System.Action<object>(this.OnEndRecoverBreath));
    assigneeGameObject.Unsubscribe(1347184327, new System.Action<object>(this.OnPathAdvanced));
  }

  private void ShowHelmet()
  {
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return;
    KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
    if (!string.IsNullOrEmpty(this.anim_file))
    {
      KAnimFile anim = Assets.GetAnim((HashedString) this.anim_file);
      assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
    }
    assigneeController.SetSymbolVisiblity(kanimHashedString, true);
    this.is_shown = true;
    this.UpdateJets();
  }

  private void HideHelmet()
  {
    this.is_shown = false;
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return;
    KAnimHashedString symbol = (KAnimHashedString) "snapTo_neck";
    if (!string.IsNullOrEmpty(this.anim_file))
    {
      SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      component.RemoveSymbolOverride((HashedString) symbol, 6);
    }
    assigneeController.SetSymbolVisiblity(symbol, false);
    this.UpdateJets();
  }

  private void UpdateJets()
  {
    if (this.is_shown && this.is_flying)
      this.EnableJets();
    else
      this.DisableJets();
  }

  private void EnableJets()
  {
    if (!this.has_jets || (bool) ((UnityEngine.Object) this.jet_go))
      return;
    this.jet_go = this.AddTrackedAnim("jet", Assets.GetAnim((HashedString) "jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck");
    this.glow_go = this.AddTrackedAnim("glow", Assets.GetAnim((HashedString) "jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck");
  }

  private void DisableJets()
  {
    if (!this.has_jets)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.jet_go);
    this.jet_go = (GameObject) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.glow_go);
    this.glow_go = (GameObject) null;
  }

  private GameObject AddTrackedAnim(
    string name,
    KAnimFile tracked_anim_file,
    string anim_clip,
    Grid.SceneLayer layer,
    string symbol_name)
  {
    KBatchedAnimController assigneeController = this.GetAssigneeController();
    if ((UnityEngine.Object) assigneeController == (UnityEngine.Object) null)
      return (GameObject) null;
    string name1 = assigneeController.name + "." + name;
    GameObject gameObject = new GameObject(name1);
    gameObject.SetActive(false);
    gameObject.transform.parent = assigneeController.transform;
    gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name1);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      tracked_anim_file
    };
    kbatchedAnimController.initialAnim = anim_clip;
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.sceneLayer = layer;
    gameObject.AddComponent<KBatchedAnimTracker>().symbol = (HashedString) symbol_name;
    bool symbolVisible;
    Vector3 column = (Vector3) assigneeController.GetSymbolTransform((HashedString) symbol_name, out symbolVisible).GetColumn(3);
    column.z = Grid.GetLayerZ(layer);
    gameObject.transform.SetPosition(column);
    gameObject.SetActive(true);
    kbatchedAnimController.Play((HashedString) anim_clip, KAnim.PlayMode.Loop, 1f, 0.0f);
    return gameObject;
  }

  private void OnBeginRecoverBreath(object data)
  {
    this.HideHelmet();
  }

  private void OnEndRecoverBreath(object data)
  {
    this.ShowHelmet();
  }

  private void OnPathAdvanced(object data)
  {
    if ((UnityEngine.Object) this.owner_navigator == (UnityEngine.Object) null)
      return;
    bool flag1 = this.owner_navigator.CurrentNavType == NavType.Hover;
    bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
    if (flag2 != this.in_tube)
    {
      this.in_tube = flag2;
      if (this.in_tube)
        this.HideHelmet();
      else
        this.ShowHelmet();
    }
    if (flag1 == this.is_flying)
      return;
    this.is_flying = flag1;
    this.UpdateJets();
  }
}
