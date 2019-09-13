// Decompiled with JetBrains decompiler
// Type: Klei.AI.EffectInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
  [DebuggerDisplay("{effect.Id}")]
  public class EffectInstance : ModifierInstance<Effect>
  {
    public Effect effect;
    public bool shouldSave;
    public StatusItem statusItem;
    public float timeRemaining;
    public Reactable reactable;

    public EffectInstance(GameObject game_object, Effect effect, bool should_save)
      : base(game_object, effect)
    {
      this.effect = effect;
      this.shouldSave = should_save;
      this.ConfigureStatusItem();
      if (effect.showInUI)
      {
        KSelectable component = this.gameObject.GetComponent<KSelectable>();
        if (!component.GetStatusItemGroup().HasStatusItemID(this.statusItem))
          component.AddStatusItem(this.statusItem, (object) this);
      }
      if (effect.triggerFloatingText && (UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
      if (string.IsNullOrEmpty(effect.emoteAnim))
        return;
      ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
      if (smi == null)
        return;
      if ((double) effect.emoteCooldown < 0.0)
      {
        SelfEmoteReactable reactable = (SelfEmoteReactable) new SelfEmoteReactable(game_object, (HashedString) (effect.Name + "_Emote"), Db.Get().ChoreTypes.Emote, (HashedString) effect.emoteAnim, 100000f, 20f, float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
        {
          anim = (HashedString) "react"
        });
        reactable.AddPrecondition(new Reactable.ReactablePrecondition(this.NotInATube));
        if (effect.emotePreconditions != null)
        {
          foreach (Reactable.ReactablePrecondition emotePrecondition in effect.emotePreconditions)
            reactable.AddPrecondition(emotePrecondition);
        }
        smi.AddOneshotReactable(reactable);
      }
      else
      {
        this.reactable = (Reactable) new SelfEmoteReactable(game_object, (HashedString) (effect.Name + "_Emote"), Db.Get().ChoreTypes.Emote, (HashedString) effect.emoteAnim, effect.emoteCooldown, 20f, float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
        {
          anim = (HashedString) "react"
        });
        this.reactable.AddPrecondition(new Reactable.ReactablePrecondition(this.NotInATube));
        if (effect.emotePreconditions == null)
          return;
        foreach (Reactable.ReactablePrecondition emotePrecondition in effect.emotePreconditions)
          this.reactable.AddPrecondition(emotePrecondition);
      }
    }

    private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
    {
      if (transition.navGridTransition.start != NavType.Tube)
        return transition.navGridTransition.end != NavType.Tube;
      return false;
    }

    public override void OnCleanUp()
    {
      if (this.statusItem != null)
      {
        this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
        this.statusItem = (StatusItem) null;
      }
      if (this.reactable == null)
        return;
      this.reactable.Cleanup();
      this.reactable = (Reactable) null;
    }

    public float GetTimeRemaining()
    {
      return this.timeRemaining;
    }

    public bool IsExpired()
    {
      if ((double) this.effect.duration > 0.0)
        return (double) this.timeRemaining <= 0.0;
      return false;
    }

    private void ConfigureStatusItem()
    {
      this.statusItem = new StatusItem(this.effect.Id, this.effect.Name, this.effect.description, string.Empty, !this.effect.isBad ? StatusItem.IconType.Info : StatusItem.IconType.Exclamation, !this.effect.isBad ? NotificationType.Neutral : NotificationType.Bad, false, OverlayModes.None.ID, 2);
      this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
      this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveTooltip);
    }

    private string ResolveString(string str, object data)
    {
      return str;
    }

    private string ResolveTooltip(string str, object data)
    {
      string str1 = str;
      EffectInstance effectInstance = (EffectInstance) data;
      string tooltip = Effect.CreateTooltip(effectInstance.effect, false, "\n");
      if (!string.IsNullOrEmpty(tooltip))
        str1 = str1 + "\n" + tooltip;
      if ((double) effectInstance.effect.duration > 0.0)
        str1 = str1 + "\n" + string.Format((string) DUPLICANTS.MODIFIERS.TIME_REMAINING, (object) GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1"));
      return str1;
    }
  }
}
