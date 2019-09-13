// Decompiled with JetBrains decompiler
// Type: KAnimSequencer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];
  [Serialize]
  public bool autoRun;
  private int currentIndex;
  private KBatchedAnimController kbac;
  private MinionBrain mb;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.kbac = this.GetComponent<KBatchedAnimController>();
    this.mb = this.GetComponent<MinionBrain>();
    if (!this.autoRun)
      return;
    this.PlaySequence();
  }

  public void Reset()
  {
    this.currentIndex = 0;
  }

  public void PlaySequence()
  {
    if (this.sequence == null || this.sequence.Length <= 0)
      return;
    if ((UnityEngine.Object) this.mb != (UnityEngine.Object) null)
      this.mb.Suspend("AnimSequencer");
    this.kbac.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.PlayNext);
    this.PlayNext((HashedString) ((string) null));
  }

  private void PlayNext(HashedString name)
  {
    if (this.sequence.Length > this.currentIndex)
    {
      this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0.0f);
      ++this.currentIndex;
    }
    else
    {
      this.kbac.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.PlayNext);
      if (!((UnityEngine.Object) this.mb != (UnityEngine.Object) null))
        return;
      this.mb.Resume("AnimSequencer");
    }
  }

  [SerializationConfig(MemberSerialization.OptOut)]
  [Serializable]
  public class KAnimSequence
  {
    public float speed = 1f;
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;
    public string anim;
  }
}
