// Decompiled with JetBrains decompiler
// Type: ConversationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : KMonoBehaviour, ISim200ms
{
  private static readonly List<Tag> invalidConvoTags = new List<Tag>()
  {
    GameTags.Asleep,
    GameTags.HoldingBreath,
    GameTags.Dead
  };
  private Dictionary<MinionIdentity, Conversation> setupsByMinion = new Dictionary<MinionIdentity, Conversation>();
  private List<System.Type> convoTypes = new List<System.Type>()
  {
    typeof (RecentThingConversation),
    typeof (AmountStateConversation),
    typeof (CurrentJobConversation)
  };
  private List<Conversation> activeSetups;
  private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;

  protected override void OnPrefabInit()
  {
    this.activeSetups = new List<Conversation>();
    this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
  }

  public void Sim200ms(float dt)
  {
    for (int index1 = this.activeSetups.Count - 1; index1 >= 0; --index1)
    {
      Conversation activeSetup = this.activeSetups[index1];
      for (int index2 = activeSetup.minions.Count - 1; index2 >= 0; --index2)
      {
        if (!this.ValidMinionTags(activeSetup.minions[index2]) || !this.MinionCloseEnoughToConvo(activeSetup.minions[index2], activeSetup))
          activeSetup.minions.RemoveAt(index2);
        else
          this.setupsByMinion[activeSetup.minions[index2]] = activeSetup;
      }
      if (activeSetup.minions.Count <= 1)
      {
        this.activeSetups.RemoveAt(index1);
      }
      else
      {
        bool flag = true;
        if (activeSetup.numUtterances == 0 && (double) GameClock.Instance.GetTime() > (double) activeSetup.lastTalkedTime + (double) TuningData<ConversationManager.Tuning>.Get().delayBeforeStart)
        {
          MinionIdentity minion = activeSetup.minions[UnityEngine.Random.Range(0, activeSetup.minions.Count)];
          activeSetup.conversationType.NewTarget(minion);
          flag = this.DoTalking(activeSetup, minion);
        }
        else if (activeSetup.numUtterances > 0 && activeSetup.numUtterances < TuningData<ConversationManager.Tuning>.Get().maxUtterances && (double) GameClock.Instance.GetTime() > (double) activeSetup.lastTalkedTime + (double) TuningData<ConversationManager.Tuning>.Get().speakTime + (double) TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances)
        {
          int index2 = (activeSetup.minions.IndexOf(activeSetup.lastTalked) + UnityEngine.Random.Range(1, activeSetup.minions.Count)) % activeSetup.minions.Count;
          MinionIdentity minion = activeSetup.minions[index2];
          flag = this.DoTalking(activeSetup, minion);
        }
        else if (activeSetup.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
          flag = false;
        if (!flag)
          this.activeSetups.RemoveAt(index1);
      }
    }
    foreach (MinionIdentity index1 in Components.LiveMinionIdentities.Items)
    {
      if (this.ValidMinionTags(index1) && !this.setupsByMinion.ContainsKey(index1) && !this.MinionOnCooldown(index1))
      {
        foreach (MinionIdentity index2 in Components.LiveMinionIdentities.Items)
        {
          if (!((UnityEngine.Object) index2 == (UnityEngine.Object) index1) && this.ValidMinionTags(index2))
          {
            if (this.setupsByMinion.ContainsKey(index2))
            {
              Conversation setup = this.setupsByMinion[index2];
              if (setup.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (double) (this.GetCentroid(setup) - index1.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5)
              {
                setup.minions.Add(index1);
                this.setupsByMinion[index1] = setup;
                break;
              }
            }
            else if (!this.MinionOnCooldown(index2) && (double) (index2.transform.GetPosition() - index1.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance)
            {
              Conversation conversation = new Conversation();
              conversation.minions.Add(index1);
              conversation.minions.Add(index2);
              System.Type convoType = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
              conversation.conversationType = (ConversationType) Activator.CreateInstance(convoType);
              conversation.lastTalkedTime = GameClock.Instance.GetTime();
              this.activeSetups.Add(conversation);
              this.setupsByMinion[index1] = conversation;
              this.setupsByMinion[index2] = conversation;
              break;
            }
          }
        }
      }
    }
    this.setupsByMinion.Clear();
  }

  private bool DoTalking(Conversation setup, MinionIdentity new_speaker)
  {
    DebugUtil.Assert(setup != null, "setup was null");
    DebugUtil.Assert((UnityEngine.Object) new_speaker != (UnityEngine.Object) null, "new_speaker was null");
    if ((UnityEngine.Object) setup.lastTalked != (UnityEngine.Object) null)
      setup.lastTalked.Trigger(25860745, (object) setup.lastTalked.gameObject);
    DebugUtil.Assert(setup.conversationType != null, "setup.conversationType was null");
    Conversation.Topic nextTopic = setup.conversationType.GetNextTopic(new_speaker, setup.lastTopic);
    if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End || nextTopic.mode == Conversation.ModeType.Segue)
      return false;
    Thought thoughtForTopic = this.GetThoughtForTopic(setup, nextTopic);
    if (thoughtForTopic == null)
      return false;
    ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
    if (smi == null)
      return false;
    smi.AddThought(thoughtForTopic);
    setup.lastTopic = nextTopic;
    setup.lastTalked = new_speaker;
    setup.lastTalkedTime = GameClock.Instance.GetTime();
    DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
    this.lastConvoTimeByMinion[setup.lastTalked] = GameClock.Instance.GetTime();
    Effects component = setup.lastTalked.GetComponent<Effects>();
    DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "effects was null");
    component.Add("GoodConversation", true);
    Conversation.Mode mode = Conversation.Topic.Modes[(int) nextTopic.mode];
    DebugUtil.Assert(mode != null, "mode was null");
    ConversationManager.StartedTalkingEvent startedTalkingEvent = new ConversationManager.StartedTalkingEvent()
    {
      talker = new_speaker.gameObject,
      anim = mode.anim
    };
    foreach (MinionIdentity minion in setup.minions)
    {
      if (!(bool) ((UnityEngine.Object) minion))
        DebugUtil.DevAssert(false, "minion in setup.minions was null");
      else
        minion.Trigger(-594200555, (object) startedTalkingEvent);
    }
    ++setup.numUtterances;
    return true;
  }

  private Vector3 GetCentroid(Conversation setup)
  {
    Vector3 zero = Vector3.zero;
    foreach (MinionIdentity minion in setup.minions)
    {
      if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
        zero += minion.transform.GetPosition();
    }
    return zero / (float) setup.minions.Count;
  }

  private Thought GetThoughtForTopic(Conversation setup, Conversation.Topic topic)
  {
    if (string.IsNullOrEmpty(topic.topic))
    {
      DebugUtil.DevAssert(false, "topic.topic was null");
      return (Thought) null;
    }
    Sprite sprite = setup.conversationType.GetSprite(topic.topic);
    if (!((UnityEngine.Object) sprite != (UnityEngine.Object) null))
      return (Thought) null;
    Conversation.Mode mode = Conversation.Topic.Modes[(int) topic.mode];
    return new Thought("Topic_" + topic.topic, (ResourceSet) null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
  }

  private bool ValidMinionTags(MinionIdentity minion)
  {
    if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      return false;
    return !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
  }

  private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation setup)
  {
    return (double) (this.GetCentroid(setup) - minion.transform.GetPosition()).magnitude < (double) TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5;
  }

  private bool MinionOnCooldown(MinionIdentity minion)
  {
    if (minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse))
      return false;
    if (!this.lastConvoTimeByMinion.ContainsKey(minion) || (double) GameClock.Instance.GetTime() >= (double) this.lastConvoTimeByMinion[minion] + (double) TuningData<ConversationManager.Tuning>.Get().minionCooldownTime)
      return (double) GameClock.Instance.GetTime() / 600.0 < (double) TuningData<ConversationManager.Tuning>.Get().cyclesBeforeFirstConversation;
    return true;
  }

  public class Tuning : TuningData<ConversationManager.Tuning>
  {
    public float cyclesBeforeFirstConversation;
    public float maxDistance;
    public int maxDupesPerConvo;
    public float minionCooldownTime;
    public float speakTime;
    public float delayBetweenUtterances;
    public float delayBeforeStart;
    public int maxUtterances;
  }

  public class StartedTalkingEvent
  {
    public GameObject talker;
    public string anim;
  }
}
