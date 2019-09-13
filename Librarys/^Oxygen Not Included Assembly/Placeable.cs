// Decompiled with JetBrains decompiler
// Type: Placeable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Placeable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Placeable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Placeable>((System.Action<Placeable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [Serialize]
  private int targetCell = -1;
  [MyCmpReq]
  private KPrefabID prefabId;
  public Tag previewTag;
  public Tag spawnOnPlaceTag;
  private GameObject preview;
  private FetchChore chore;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Placeable>(493375141, Placeable.OnRefreshUserMenuDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.prefabId.AddTag(new Tag(this.prefabId.InstanceID.ToString()), false);
    if (this.targetCell == -1)
      return;
    this.QueuePlacement(this.targetCell);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.preview != (UnityEngine.Object) null)
      this.preview.DeleteObject();
    base.OnCleanUp();
  }

  public void QueuePlacement(int target)
  {
    this.targetCell = target;
    Vector3 posCbc = Grid.CellToPosCBC(this.targetCell, Grid.SceneLayer.Front);
    if ((UnityEngine.Object) this.preview == (UnityEngine.Object) null)
    {
      this.preview = GameUtil.KInstantiate(Assets.GetPrefab(this.previewTag), posCbc, Grid.SceneLayer.Front, (string) null, 0);
      this.preview.SetActive(true);
    }
    else
      this.preview.transform.SetPosition(posCbc);
    if (this.chore != null)
      this.chore.Cancel("new target");
    this.chore = new FetchChore(Db.Get().ChoreTypes.Fetch, this.preview.GetComponent<Storage>(), 1f, new Tag[1]
    {
      new Tag(this.prefabId.InstanceID.ToString())
    }, (Tag[]) null, (Tag[]) null, (ChoreProvider) null, true, new System.Action<Chore>(this.OnChoreComplete), (System.Action<Chore>) null, (System.Action<Chore>) null, FetchOrder2.OperationalRequirement.None, 0);
  }

  private void OnChoreComplete(Chore completed_chore)
  {
    this.Place(this.targetCell);
  }

  public void Place(int target)
  {
    GameUtil.KInstantiate(Assets.GetPrefab(this.spawnOnPlaceTag), Grid.CellToPosCBC(target, Grid.SceneLayer.Front), Grid.SceneLayer.Front, (string) null, 0).SetActive(true);
    this.DeleteObject();
  }

  private void OpenPlaceTool()
  {
    PlaceTool.Instance.Activate(this, this.previewTag);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, this.targetCell != -1 ? new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELOCATE.NAME_OFF, new System.Action(this.CancelRelocation), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.RELOCATE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELOCATE.NAME, new System.Action(this.OpenPlaceTool), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.RELOCATE.TOOLTIP, true), 1f);
  }

  private void CancelRelocation()
  {
    if ((UnityEngine.Object) this.preview != (UnityEngine.Object) null)
    {
      this.preview.DeleteObject();
      this.preview = (GameObject) null;
    }
    this.targetCell = -1;
  }
}
