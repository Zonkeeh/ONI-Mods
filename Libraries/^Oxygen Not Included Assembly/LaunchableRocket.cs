// Decompiled with JetBrains decompiler
// Type: LaunchableRocket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocket : StateMachineComponent<LaunchableRocket.StatesInstance>, IEffectDescriptor
{
  public List<GameObject> parts = new List<GameObject>();
  [Serialize]
  private int takeOffLocation;
  [Serialize]
  private float flightAnimOffset;
  private bool isLanding;
  private float rocketSpeed;
  private GameObject soundSpeakerObject;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.master.parts = AttachableBuilding.GetAttachedNetwork(this.smi.master.GetComponent<AttachableBuilding>());
    if (SpacecraftManager.instance.GetSpacecraftID(this) == -1)
    {
      Spacecraft craft = new Spacecraft(this.GetComponent<LaunchConditionManager>());
      craft.GenerateName();
      SpacecraftManager.instance.RegisterSpacecraft(craft);
    }
    this.smi.StartSM();
  }

  public List<GameObject> GetEngines()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (GameObject part in this.parts)
    {
      if ((bool) ((UnityEngine.Object) part.GetComponent<RocketEngine>()))
        gameObjectList.Add(part);
    }
    return gameObjectList;
  }

  protected override void OnCleanUp()
  {
    SpacecraftManager.instance.UnregisterSpacecraft(this.GetComponent<LaunchConditionManager>());
    base.OnCleanUp();
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }

  public class StatesInstance : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.GameInstance
  {
    public StatesInstance(LaunchableRocket master)
      : base(master)
    {
    }

    public bool IsMissionState(Spacecraft.MissionState state)
    {
      return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.master.GetComponent<LaunchConditionManager>()).state == state;
    }

    public void SetMissionState(Spacecraft.MissionState state)
    {
      SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.master.GetComponent<LaunchConditionManager>()).SetState(state);
    }
  }

  public class States : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket>
  {
    public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State grounded;
    public LaunchableRocket.States.NotGroundedStates not_grounded;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grounded;
      this.serializable = true;
      this.grounded.EventTransition(GameHashes.LaunchRocket, this.not_grounded.launch_pre, (StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.Transition.ConditionCallback) null).Enter((StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State.Callback) (smi =>
      {
        smi.master.rocketSpeed = 0.0f;
        foreach (GameObject part in smi.master.parts)
        {
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
            part.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
        }
        smi.SetMissionState(Spacecraft.MissionState.Grounded);
      }));
      this.not_grounded.ToggleTag(GameTags.RocketNotOnGround);
      this.not_grounded.launch_pre.Enter((StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State.Callback) (smi =>
      {
        smi.master.isLanding = false;
        smi.master.rocketSpeed = 0.0f;
        smi.master.parts = AttachableBuilding.GetAttachedNetwork(smi.master.GetComponent<AttachableBuilding>());
        if ((UnityEngine.Object) smi.master.soundSpeakerObject == (UnityEngine.Object) null)
        {
          smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
          smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
        }
        foreach (GameObject engine in smi.master.GetEngines())
          engine.Trigger(-1358394196, (object) null);
        Game.Instance.Trigger(-1056989049, (object) this);
        foreach (GameObject part in smi.master.parts)
        {
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
          {
            smi.master.takeOffLocation = Grid.PosToCell(smi.master.gameObject);
            part.Trigger(-1056989049, (object) null);
          }
        }
        smi.SetMissionState(Spacecraft.MissionState.Launching);
      })).ScheduleGoTo(5f, (StateMachine.BaseState) this.not_grounded.launch_loop);
      this.not_grounded.launch_loop.EventTransition(GameHashes.ReturnRocket, this.not_grounded.returning, (StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.Transition.ConditionCallback) null).Update((System.Action<LaunchableRocket.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.isLanding = false;
        bool flag = true;
        float num = Mathf.Clamp(Mathf.Pow(smi.timeinstate / 5f, 4f), 0.0f, 10f);
        smi.master.rocketSpeed = num;
        smi.master.flightAnimOffset += dt * num;
        foreach (GameObject part in smi.master.parts)
        {
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
          {
            KBatchedAnimController component = part.GetComponent<KBatchedAnimController>();
            component.Offset = Vector3.up * smi.master.flightAnimOffset;
            Vector3 positionIncludingOffset = component.PositionIncludingOffset;
            if ((UnityEngine.Object) smi.master.soundSpeakerObject == (UnityEngine.Object) null)
            {
              smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
              smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
            }
            smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
            if (Grid.PosToXY(positionIncludingOffset).y > Singleton<KBatchedAnimUpdater>.Instance.GetVisibleSize().y)
            {
              part.GetComponent<RocketModule>().OnSuspend((object) null);
              part.GetComponent<KBatchedAnimController>().enabled = false;
            }
            else
            {
              flag = false;
              LaunchableRocket.States.DoWorldDamage(part, positionIncludingOffset);
            }
          }
        }
        if (!flag)
          return;
        smi.GoTo((StateMachine.BaseState) this.not_grounded.space);
      }), UpdateRate.SIM_33ms, false);
      this.not_grounded.space.Enter((StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State.Callback) (smi =>
      {
        smi.master.rocketSpeed = 0.0f;
        foreach (GameObject part in smi.master.parts)
        {
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
          {
            part.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
            part.GetComponent<KBatchedAnimController>().enabled = false;
          }
        }
        smi.SetMissionState(Spacecraft.MissionState.Underway);
      })).EventTransition(GameHashes.ReturnRocket, this.not_grounded.returning, (StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.Transition.ConditionCallback) (smi => smi.IsMissionState(Spacecraft.MissionState.WaitingToLand)));
      this.not_grounded.returning.Enter((StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State.Callback) (smi =>
      {
        smi.master.isLanding = true;
        smi.master.rocketSpeed = 0.0f;
        smi.SetMissionState(Spacecraft.MissionState.Landing);
      })).Update((System.Action<LaunchableRocket.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.isLanding = true;
        KBatchedAnimController component1 = smi.master.gameObject.GetComponent<KBatchedAnimController>();
        component1.Offset = Vector3.up * smi.master.flightAnimOffset;
        float num = Mathf.Clamp(0.5f * Mathf.Abs(smi.master.gameObject.transform.position.y + component1.Offset.y - (Grid.CellToPos(smi.master.takeOffLocation) + Vector3.down * (Grid.CellSizeInMeters / 2f)).y), 0.0f, 10f) * dt;
        smi.master.rocketSpeed = num;
        smi.master.flightAnimOffset -= num;
        bool flag = true;
        if ((UnityEngine.Object) smi.master.soundSpeakerObject == (UnityEngine.Object) null)
        {
          smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
          smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
        }
        smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
        foreach (GameObject part in smi.master.parts)
        {
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
          {
            KBatchedAnimController component2 = part.GetComponent<KBatchedAnimController>();
            component2.Offset = Vector3.up * smi.master.flightAnimOffset;
            Vector3 positionIncludingOffset = component2.PositionIncludingOffset;
            if (Grid.IsValidCell(Grid.PosToCell(part)))
              part.GetComponent<KBatchedAnimController>().enabled = true;
            else
              flag = false;
            LaunchableRocket.States.DoWorldDamage(part, positionIncludingOffset);
          }
        }
        if (!flag)
          return;
        smi.GoTo((StateMachine.BaseState) this.not_grounded.landing_loop);
      }), UpdateRate.SIM_33ms, false);
      this.not_grounded.landing_loop.Enter((StateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State.Callback) (smi =>
      {
        smi.master.isLanding = true;
        int index1 = -1;
        for (int index2 = 0; index2 < smi.master.parts.Count; ++index2)
        {
          GameObject part = smi.master.parts[index2];
          if (!((UnityEngine.Object) part == (UnityEngine.Object) null) && (UnityEngine.Object) part != (UnityEngine.Object) smi.master.gameObject && (UnityEngine.Object) part.GetComponent<RocketEngine>() != (UnityEngine.Object) null)
            index1 = index2;
        }
        if (index1 == -1)
          return;
        smi.master.parts[index1].Trigger(-1358394196, (object) null);
      })).Update((System.Action<LaunchableRocket.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
        float flightAnimOffset = smi.master.flightAnimOffset;
        float num1 = 0.0f;
        float num2 = Mathf.Clamp(0.5f * flightAnimOffset, 0.0f, 10f);
        smi.master.rocketSpeed = num2;
        smi.master.flightAnimOffset -= num2 * dt;
        if ((UnityEngine.Object) smi.master.soundSpeakerObject == (UnityEngine.Object) null)
        {
          smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
          smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
        }
        smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
        if ((double) num2 <= 1.0 / 400.0 && (double) dt != 0.0)
        {
          smi.master.GetComponent<KSelectable>().IsSelectable = true;
          num1 = 0.0f;
          foreach (GameObject part in smi.master.parts)
          {
            if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
              part.Trigger(238242047, (object) null);
          }
          smi.GoTo((StateMachine.BaseState) this.grounded);
        }
        else
        {
          foreach (GameObject part in smi.master.parts)
          {
            if (!((UnityEngine.Object) part == (UnityEngine.Object) null))
            {
              KBatchedAnimController component = part.GetComponent<KBatchedAnimController>();
              component.Offset = Vector3.up * smi.master.flightAnimOffset;
              Vector3 positionIncludingOffset = component.PositionIncludingOffset;
              LaunchableRocket.States.DoWorldDamage(part, positionIncludingOffset);
            }
          }
        }
      }), UpdateRate.SIM_33ms, false);
    }

    private static void DoWorldDamage(GameObject part, Vector3 apparentPosition)
    {
      OccupyArea component1 = part.GetComponent<OccupyArea>();
      component1.UpdateOccupiedArea();
      foreach (CellOffset occupiedCellsOffset in component1.OccupiedCellsOffsets)
      {
        int cell1 = Grid.OffsetCell(Grid.PosToCell(apparentPosition), occupiedCellsOffset);
        if (Grid.IsValidCell(cell1))
        {
          if (Grid.Solid[cell1])
          {
            WorldDamage instance = WorldDamage.Instance;
            int num1 = cell1;
            float num2 = 10000f;
            int num3 = cell1;
            string rocket1 = (string) BUILDINGS.DAMAGESOURCES.ROCKET;
            int cell2 = num1;
            double num4 = (double) num2;
            int src_cell = num3;
            string source_name = rocket1;
            string rocket2 = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET;
            double num5 = (double) instance.ApplyDamage(cell2, (float) num4, src_cell, -1, source_name, rocket2);
          }
          else if (Grid.FakeFloor[cell1])
          {
            GameObject go = Grid.Objects[cell1, 38];
            if ((UnityEngine.Object) go != (UnityEngine.Object) null)
            {
              BuildingHP component2 = go.GetComponent<BuildingHP>();
              if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
                go.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
                {
                  damage = component2.MaxHitPoints,
                  source = (string) BUILDINGS.DAMAGESOURCES.ROCKET,
                  popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
                });
            }
          }
        }
      }
    }

    public class NotGroundedStates : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State
    {
      public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_pre;
      public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State space;
      public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_loop;
      public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State returning;
      public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State landing_loop;
    }
  }

  private class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
  {
    private List<LaunchableRocket.UpdateRocketLandingParameter.Entry> entries = new List<LaunchableRocket.UpdateRocketLandingParameter.Entry>();

    public UpdateRocketLandingParameter()
      : base((HashedString) "rocketLanding")
    {
    }

    public override void Add(LoopingSoundParameterUpdater.Sound sound)
    {
      this.entries.Add(new LaunchableRocket.UpdateRocketLandingParameter.Entry()
      {
        rocketModule = sound.transform.GetComponent<RocketModule>(),
        ev = sound.ev,
        parameterIdx = sound.description.GetParameterIdx(this.parameter)
      });
    }

    public override void Update(float dt)
    {
      foreach (LaunchableRocket.UpdateRocketLandingParameter.Entry entry in this.entries)
      {
        if (!((UnityEngine.Object) entry.rocketModule == (UnityEngine.Object) null))
        {
          LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
          if (!((UnityEngine.Object) conditionManager == (UnityEngine.Object) null))
          {
            LaunchableRocket component = conditionManager.GetComponent<LaunchableRocket>();
            if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            {
              if (component.isLanding)
              {
                int num1 = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, 1f);
              }
              else
              {
                int num2 = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, 0.0f);
              }
            }
          }
        }
      }
    }

    public override void Remove(LoopingSoundParameterUpdater.Sound sound)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ev.handle == sound.ev.handle)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
    }

    private struct Entry
    {
      public RocketModule rocketModule;
      public EventInstance ev;
      public int parameterIdx;
    }
  }

  private class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
  {
    private List<LaunchableRocket.UpdateRocketSpeedParameter.Entry> entries = new List<LaunchableRocket.UpdateRocketSpeedParameter.Entry>();

    public UpdateRocketSpeedParameter()
      : base((HashedString) "rocketSpeed")
    {
    }

    public override void Add(LoopingSoundParameterUpdater.Sound sound)
    {
      this.entries.Add(new LaunchableRocket.UpdateRocketSpeedParameter.Entry()
      {
        rocketModule = sound.transform.GetComponent<RocketModule>(),
        ev = sound.ev,
        parameterIdx = sound.description.GetParameterIdx(this.parameter)
      });
    }

    public override void Update(float dt)
    {
      foreach (LaunchableRocket.UpdateRocketSpeedParameter.Entry entry in this.entries)
      {
        if (!((UnityEngine.Object) entry.rocketModule == (UnityEngine.Object) null))
        {
          LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
          if (!((UnityEngine.Object) conditionManager == (UnityEngine.Object) null))
          {
            LaunchableRocket component = conditionManager.GetComponent<LaunchableRocket>();
            if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            {
              int num = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, component.rocketSpeed);
            }
          }
        }
      }
    }

    public override void Remove(LoopingSoundParameterUpdater.Sound sound)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ev.handle == sound.ev.handle)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
    }

    private struct Entry
    {
      public RocketModule rocketModule;
      public EventInstance ev;
      public int parameterIdx;
    }
  }
}
