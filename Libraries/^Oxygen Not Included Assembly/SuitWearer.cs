// Decompiled with JetBrains decompiler
// Type: SuitWearer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SuitWearer : GameStateMachine<SuitWearer, SuitWearer.Instance>
{
  public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State suit;
  public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State nosuit;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.PathAdvanced, (GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, data) => smi.OnPathAdvanced(data))).DoNothing();
    this.suit.DoNothing();
    this.nosuit.DoNothing();
  }

  public class Instance : GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.GameInstance
  {
    private List<int> suitReservations = new List<int>();
    private List<int> emptyLockerReservations = new List<int>();
    private Navigator navigator;
    private int prefabInstanceID;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.navigator = master.GetComponent<Navigator>();
      this.navigator.SetFlags(PathFinder.PotentialPath.Flags.PerformSuitChecks);
      this.prefabInstanceID = this.navigator.GetComponent<KPrefabID>().InstanceID;
      master.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_neck", false);
    }

    public void OnPathAdvanced(object data)
    {
      if (this.navigator.CurrentNavType == NavType.Hover && (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) == PathFinder.PotentialPath.Flags.None)
        this.navigator.SetCurrentNavType(NavType.Floor);
      this.UnreserveSuits();
      this.ReserveSuits();
    }

    public void ReserveSuits()
    {
      PathFinder.Path path = this.navigator.path;
      if (path.nodes == null)
        return;
      bool flag1 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) != PathFinder.PotentialPath.Flags.None;
      bool flag2 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) != PathFinder.PotentialPath.Flags.None;
      for (int index = 0; index < path.nodes.Count - 1; ++index)
      {
        int cell = path.nodes[index].cell;
        Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags) 0;
        PathFinder.PotentialPath.Flags pathFlags = PathFinder.PotentialPath.Flags.None;
        if (Grid.TryGetSuitMarkerFlags(cell, out flags, out pathFlags))
        {
          bool flag3 = (pathFlags & PathFinder.PotentialPath.Flags.HasAtmoSuit) != PathFinder.PotentialPath.Flags.None;
          bool flag4 = (pathFlags & PathFinder.PotentialPath.Flags.HasJetPack) != PathFinder.PotentialPath.Flags.None;
          bool flag5 = flag2 || flag1;
          bool flag6 = flag3 == flag1 && flag4 == flag2;
          bool flag7 = SuitMarker.DoesTraversalDirectionRequireSuit(cell, path.nodes[index + 1].cell, flags);
          if (flag7 && !flag5)
          {
            Grid.ReserveSuit(cell, this.prefabInstanceID, true);
            this.suitReservations.Add(cell);
            if (flag3)
              flag1 = true;
            if (flag4)
              flag2 = true;
          }
          else if (!flag7 && flag6 && Grid.HasEmptyLocker(cell, this.prefabInstanceID))
          {
            Grid.ReserveEmptyLocker(cell, this.prefabInstanceID, true);
            this.emptyLockerReservations.Add(cell);
            if (flag3)
              flag1 = false;
            if (flag4)
              flag2 = false;
          }
        }
      }
    }

    public void UnreserveSuits()
    {
      foreach (int suitReservation in this.suitReservations)
      {
        if (Grid.HasSuitMarker[suitReservation])
          Grid.ReserveSuit(suitReservation, this.prefabInstanceID, false);
      }
      this.suitReservations.Clear();
      foreach (int lockerReservation in this.emptyLockerReservations)
      {
        if (Grid.HasSuitMarker[lockerReservation])
          Grid.ReserveEmptyLocker(lockerReservation, this.prefabInstanceID, false);
      }
      this.emptyLockerReservations.Clear();
    }
  }
}
