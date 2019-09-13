// Decompiled with JetBrains decompiler
// Type: DiggerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System;
using UnityEngine;

public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
  public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;
  public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (Func<DiggerMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.dig, (StateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.Transition.ConditionCallback) (smi => smi.CanTunnel()));
    this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (StateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<DiggerMonitor.Instance>) null).GoTo(this.loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public int depthToDig { get; set; }
  }

  public class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
  {
    [Serialize]
    public int lastDigCell = -1;

    public Instance(IStateMachineTarget master, DiggerMonitor.Def def)
      : base(master, def)
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
      master.Subscribe(387220196, new System.Action<object>(this.OnDestinationReached));
      master.Subscribe(-766531887, new System.Action<object>(this.OnDestinationReached));
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
      this.master.Unsubscribe(387220196, new System.Action<object>(this.OnDestinationReached));
      this.master.Unsubscribe(-766531887, new System.Action<object>(this.OnDestinationReached));
    }

    private void OnDestinationReached(object data)
    {
      this.CheckInSolid();
    }

    private void CheckInSolid()
    {
      Navigator component = this.gameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      int cell = Grid.PosToCell(this.gameObject);
      if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
      {
        component.SetCurrentNavType(NavType.Solid);
      }
      else
      {
        if (component.CurrentNavType != NavType.Solid || Grid.IsSolidCell(cell))
          return;
        component.SetCurrentNavType(NavType.Floor);
        this.gameObject.AddTag(GameTags.Creatures.Falling);
      }
    }

    private void OnSolidChanged(int cell)
    {
      this.CheckInSolid();
    }

    public bool CanTunnel()
    {
      int cell1 = Grid.PosToCell((StateMachine.Instance) this);
      if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell1) == SubWorld.ZoneType.Space)
      {
        int cell2 = cell1;
        while (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
          cell2 = Grid.CellAbove(cell2);
        if (!Grid.IsValidCell(cell2))
          return this.FoundValidDigCell();
      }
      return false;
    }

    private bool FoundValidDigCell()
    {
      int depthToDig = this.smi.def.depthToDig;
      int cell1 = Grid.PosToCell(this.smi.master.gameObject);
      this.lastDigCell = cell1;
      int cell2;
      for (cell2 = Grid.CellBelow(cell1); this.IsValidDigCell(cell2, (object) null) && depthToDig > 0; --depthToDig)
        cell2 = Grid.CellBelow(cell2);
      if (depthToDig > 0)
        cell2 = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), (object) null, cell1, this.smi.def.depthToDig, false, true);
      this.lastDigCell = cell2;
      return this.lastDigCell != -1;
    }

    private bool IsValidDigCell(int cell, object arg = null)
    {
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
      {
        if (Grid.HasDoor[cell] || Grid.Foundation[cell])
        {
          GameObject gameObject = Grid.Objects[cell, 1];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            if (Grid.Element[cell].hardness < (byte) 150)
              return !component.Element.HasTag(GameTags.RefinedMetal);
            return false;
          }
        }
        else
        {
          byte num = Grid.ElementIdx[cell];
          Element element = ElementLoader.elements[(int) num];
          if (Grid.Element[cell].hardness < (byte) 150)
            return !element.HasTag(GameTags.RefinedMetal);
          return false;
        }
      }
      return false;
    }
  }
}
