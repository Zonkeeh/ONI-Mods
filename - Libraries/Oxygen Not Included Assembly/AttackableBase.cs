// Decompiled with JetBrains decompiler
// Type: AttackableBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

public class AttackableBase : Workable, IApproachable
{
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => component.SetupScenePartitioner(data)));
  private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>((System.Action<AttackableBase, object>) ((component, data) => GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject))));
  private HandleVector<int>.Handle scenePartitionerEntry;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
    this.SetupScenePartitioner((object) null);
    this.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
    this.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
    this.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
    this.Subscribe<AttackableBase>(1623392196, AttackableBase.OnDefeatedDelegate);
  }

  public float GetDamageMultiplier()
  {
    if (this.attributeConverter != null && (UnityEngine.Object) this.worker != (UnityEngine.Object) null)
      return Mathf.Max(1f + this.worker.GetComponent<AttributeConverters>().GetConverter(this.attributeConverter.Id).Evaluate(), 0.1f);
    return 1f;
  }

  private void SetupScenePartitioner(object data = null)
  {
    this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(this.gameObject.name, (object) this.GetComponent<FactionAlignment>(), new Extents(Grid.PosToXY(this.transform.GetPosition()).x, Grid.PosToXY(this.transform.GetPosition()).y, 1, 1), GameScenePartitioner.Instance.attackableEntitiesLayer, (System.Action<object>) null);
  }

  private void OnDefeated(object data = null)
  {
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
  }

  public override float GetEfficiencyMultiplier(Worker worker)
  {
    return 1f;
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate, false);
    this.Unsubscribe<AttackableBase>(1623392196, AttackableBase.OnDefeatedDelegate, false);
    this.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate, false);
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
    base.OnCleanUp();
  }
}
