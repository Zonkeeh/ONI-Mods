// Decompiled with JetBrains decompiler
// Type: BuildingCellVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[SkipSaveFileSerialization]
public class BuildingCellVisualizer : KMonoBehaviour
{
  [SerializeField]
  public static Color32 secondOutputColour = (Color32) new Color(0.9843137f, 0.6901961f, 0.2313726f);
  [SerializeField]
  public static Color32 secondInputColour = (Color32) new Color(0.9843137f, 0.6901961f, 0.2313726f);
  private BuildingCellVisualizerResources resources;
  [MyCmpReq]
  private Building building;
  private const BuildingCellVisualizer.Ports POWER_PORTS = BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut;
  private const BuildingCellVisualizer.Ports GAS_PORTS = BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut;
  private const BuildingCellVisualizer.Ports LIQUID_PORTS = BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut;
  private const BuildingCellVisualizer.Ports SOLID_PORTS = BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut;
  private const BuildingCellVisualizer.Ports MATTER_PORTS = BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut;
  private BuildingCellVisualizer.Ports ports;
  private BuildingCellVisualizer.Ports secondary_ports;
  private Sprite diseaseSourceSprite;
  private Color32 diseaseSourceColour;
  private GameObject inputVisualizer;
  private GameObject outputVisualizer;
  private GameObject secondaryInputVisualizer;
  private GameObject secondaryOutputVisualizer;
  private bool enableRaycast;
  private Dictionary<GameObject, Image> icons;

  public bool RequiresPowerInput
  {
    get
    {
      return (this.ports & BuildingCellVisualizer.Ports.PowerIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresPowerOutput
  {
    get
    {
      return (this.ports & BuildingCellVisualizer.Ports.PowerOut) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresPower
  {
    get
    {
      return (this.ports & (BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresGas
  {
    get
    {
      return (this.ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresLiquid
  {
    get
    {
      return (this.ports & (BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresSolid
  {
    get
    {
      return (this.ports & (BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public bool RequiresUtilityConnection
  {
    get
    {
      return (this.ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut);
    }
  }

  public void ConnectedEventWithDelay(
    float delay,
    int connectionCount,
    int cell,
    string soundName)
  {
    this.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
  }

  [DebuggerHidden]
  private IEnumerator ConnectedDelay(
    float delay,
    int connectionCount,
    int cell,
    string soundName)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BuildingCellVisualizer.\u003CConnectedDelay\u003Ec__Iterator0()
    {
      delay = delay,
      cell = cell,
      soundName = soundName,
      connectionCount = connectionCount,
      \u0024this = this
    };
  }

  public void ConnectedEvent(int cell)
  {
    GameObject gameObject = (GameObject) null;
    if ((UnityEngine.Object) this.inputVisualizer != (UnityEngine.Object) null && Grid.PosToCell(this.inputVisualizer) == cell)
      gameObject = this.inputVisualizer;
    else if ((UnityEngine.Object) this.outputVisualizer != (UnityEngine.Object) null && Grid.PosToCell(this.outputVisualizer) == cell)
      gameObject = this.outputVisualizer;
    else if ((UnityEngine.Object) this.secondaryInputVisualizer != (UnityEngine.Object) null && Grid.PosToCell(this.secondaryInputVisualizer) == cell)
      gameObject = this.secondaryInputVisualizer;
    else if ((UnityEngine.Object) this.secondaryOutputVisualizer != (UnityEngine.Object) null && Grid.PosToCell(this.secondaryOutputVisualizer) == cell)
      gameObject = this.secondaryOutputVisualizer;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    SizePulse pulse = gameObject.gameObject.AddComponent<SizePulse>();
    pulse.speed = 20f;
    pulse.multiplier = 0.75f;
    pulse.updateWhenPaused = true;
    pulse.onComplete += (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) pulse));
  }

  protected override void OnSpawn()
  {
    this.resources = BuildingCellVisualizerResources.Instance();
    this.enableRaycast = (UnityEngine.Object) (this.building as BuildingComplete) != (UnityEngine.Object) null;
    this.icons = new Dictionary<GameObject, Image>();
  }

  private void MapBuilding()
  {
    BuildingDef def = this.building.Def;
    if (def.CheckRequiresPowerInput())
      this.ports |= BuildingCellVisualizer.Ports.PowerIn;
    if (def.CheckRequiresPowerOutput())
      this.ports |= BuildingCellVisualizer.Ports.PowerOut;
    if (def.CheckRequiresGasInput())
      this.ports |= BuildingCellVisualizer.Ports.GasIn;
    if (def.CheckRequiresGasOutput())
      this.ports |= BuildingCellVisualizer.Ports.GasOut;
    if (def.CheckRequiresLiquidInput())
      this.ports |= BuildingCellVisualizer.Ports.LiquidIn;
    if (def.CheckRequiresLiquidOutput())
      this.ports |= BuildingCellVisualizer.Ports.LiquidOut;
    if (def.CheckRequiresSolidInput())
      this.ports |= BuildingCellVisualizer.Ports.SolidIn;
    if (def.CheckRequiresSolidOutput())
      this.ports |= BuildingCellVisualizer.Ports.SolidOut;
    DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo((HashedString) def.DiseaseCellVisName);
    if (info.name != null)
    {
      this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
      this.diseaseSourceColour = info.overlayColour;
    }
    ISecondaryInput component1 = def.BuildingComplete.GetComponent<ISecondaryInput>();
    if (component1 != null)
    {
      switch (component1.GetSecondaryConduitType())
      {
        case ConduitType.Gas:
          this.secondary_ports |= BuildingCellVisualizer.Ports.GasIn;
          break;
        case ConduitType.Liquid:
          this.secondary_ports |= BuildingCellVisualizer.Ports.LiquidIn;
          break;
      }
    }
    ISecondaryOutput component2 = def.BuildingComplete.GetComponent<ISecondaryOutput>();
    if (component2 == null)
      return;
    switch (component2.GetSecondaryConduitType())
    {
      case ConduitType.Gas:
        this.secondary_ports |= BuildingCellVisualizer.Ports.GasOut;
        break;
      case ConduitType.Liquid:
        this.secondary_ports |= BuildingCellVisualizer.Ports.LiquidOut;
        break;
    }
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((UnityEngine.Object) this.inputVisualizer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.inputVisualizer);
    if ((UnityEngine.Object) this.outputVisualizer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.outputVisualizer);
    if ((UnityEngine.Object) this.secondaryInputVisualizer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.secondaryInputVisualizer);
    if (!((UnityEngine.Object) this.secondaryOutputVisualizer != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.secondaryOutputVisualizer);
  }

  private Color GetWireColor(int cell)
  {
    GameObject gameObject = Grid.Objects[cell, 26];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return Color.white;
    KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return (Color) component.TintColour;
    return Color.white;
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.MapBuilding();
    Components.BuildingCellVisualizers.Add(this);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    Components.BuildingCellVisualizers.Remove(this);
  }

  public void DrawIcons(HashedString mode)
  {
    if (mode == OverlayModes.Power.ID)
    {
      if (this.RequiresPower)
      {
        bool flag = (UnityEngine.Object) (this.building as BuildingPreview) != (UnityEngine.Object) null;
        BuildingEnabledButton component = this.building.GetComponent<BuildingEnabledButton>();
        int powerInputCell = this.building.GetPowerInputCell();
        if (this.RequiresPowerInput)
        {
          int circuitId = (int) Game.Instance.circuitManager.GetCircuitID(powerInputCell);
          Color tint = !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsEnabled ? Color.white : Color.gray;
          Sprite icon_img = flag || circuitId == (int) ushort.MaxValue ? this.resources.electricityInputIcon : this.resources.electricityConnectedIcon;
          this.DrawUtilityIcon(powerInputCell, icon_img, ref this.inputVisualizer, tint, this.GetWireColor(powerInputCell), 1f, false);
        }
        if (!this.RequiresPowerOutput)
          return;
        int powerOutputCell = this.building.GetPowerOutputCell();
        int circuitId1 = (int) Game.Instance.circuitManager.GetCircuitID(powerOutputCell);
        Color color = !this.building.Def.UseWhitePowerOutputConnectorColour ? this.resources.electricityOutputColor : Color.white;
        Color32 color32 = (Color32) (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsEnabled ? color : Color.gray);
        Sprite icon_img1 = flag || circuitId1 == (int) ushort.MaxValue ? this.resources.electricityInputIcon : this.resources.electricityConnectedIcon;
        this.DrawUtilityIcon(powerOutputCell, icon_img1, ref this.outputVisualizer, (Color) color32, this.GetWireColor(powerOutputCell), 1f, false);
      }
      else
      {
        bool flag = true;
        Switch component1 = this.GetComponent<Switch>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          this.DrawUtilityIcon(Grid.PosToCell(this.transform.GetPosition()), this.resources.switchIcon, ref this.outputVisualizer, (Color) (!component1.IsHandlerOn() ? this.resources.switchOffColor : this.resources.switchColor), Color.white, 1f, false);
          flag = false;
        }
        else
        {
          WireUtilityNetworkLink component2 = this.GetComponent<WireUtilityNetworkLink>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            int linked_cell1;
            int linked_cell2;
            component2.GetCells(out linked_cell1, out linked_cell2);
            this.DrawUtilityIcon(linked_cell1, Game.Instance.circuitManager.GetCircuitID(linked_cell1) != ushort.MaxValue ? this.resources.electricityConnectedIcon : this.resources.electricityBridgeIcon, ref this.inputVisualizer, this.resources.electricityInputColor, Color.white, 1f, false);
            this.DrawUtilityIcon(linked_cell2, Game.Instance.circuitManager.GetCircuitID(linked_cell2) != ushort.MaxValue ? this.resources.electricityConnectedIcon : this.resources.electricityBridgeIcon, ref this.outputVisualizer, this.resources.electricityInputColor, Color.white, 1f, false);
            flag = false;
          }
        }
        if (!flag)
          return;
        this.DisableIcons();
      }
    }
    else if (mode == OverlayModes.GasConduits.ID)
    {
      if (this.RequiresGas || (this.secondary_ports & (BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
      {
        if ((this.ports & BuildingCellVisualizer.Ports.GasIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
        {
          bool flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityInputCell(), 12];
          BuildingCellVisualizerResources.ConnectedDisconnectedColours input = this.resources.gasIOColours.input;
          this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.gasInputIcon, ref this.inputVisualizer, (Color) (!flag ? input.disconnected : input.connected));
        }
        if ((this.ports & BuildingCellVisualizer.Ports.GasOut) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
        {
          bool flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityOutputCell(), 12];
          BuildingCellVisualizerResources.ConnectedDisconnectedColours output = this.resources.gasIOColours.output;
          this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.gasOutputIcon, ref this.outputVisualizer, (Color) (!flag ? output.disconnected : output.connected));
        }
        if ((this.secondary_ports & BuildingCellVisualizer.Ports.GasIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
          this.DrawUtilityIcon(this.GetVisualizerCell(this.building, this.building.GetComponent<ISecondaryInput>().GetSecondaryConduitOffset()), this.resources.gasInputIcon, ref this.secondaryInputVisualizer, (Color) BuildingCellVisualizer.secondInputColour, Color.white, 1.5f, false);
        if ((this.secondary_ports & BuildingCellVisualizer.Ports.GasOut) == ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
          return;
        this.DrawUtilityIcon(this.GetVisualizerCell(this.building, this.building.GetComponent<ISecondaryOutput>().GetSecondaryConduitOffset()), this.resources.gasOutputIcon, ref this.secondaryOutputVisualizer, (Color) BuildingCellVisualizer.secondOutputColour, Color.white, 1.5f, false);
      }
      else
        this.DisableIcons();
    }
    else if (mode == OverlayModes.LiquidConduits.ID)
    {
      if (this.RequiresLiquid || (this.secondary_ports & (BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut)) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
      {
        if ((this.ports & BuildingCellVisualizer.Ports.LiquidIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
        {
          bool flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityInputCell(), 16];
          BuildingCellVisualizerResources.ConnectedDisconnectedColours input = this.resources.liquidIOColours.input;
          this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.liquidInputIcon, ref this.inputVisualizer, (Color) (!flag ? input.disconnected : input.connected));
        }
        if ((this.ports & BuildingCellVisualizer.Ports.LiquidOut) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
        {
          bool flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityOutputCell(), 16];
          BuildingCellVisualizerResources.ConnectedDisconnectedColours output = this.resources.liquidIOColours.output;
          this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.liquidOutputIcon, ref this.outputVisualizer, (Color) (!flag ? output.disconnected : output.connected));
        }
        if ((this.secondary_ports & BuildingCellVisualizer.Ports.LiquidIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
          this.DrawUtilityIcon(this.GetVisualizerCell(this.building, this.building.GetComponent<ISecondaryInput>().GetSecondaryConduitOffset()), this.resources.liquidInputIcon, ref this.secondaryInputVisualizer, (Color) BuildingCellVisualizer.secondInputColour, Color.white, 1.5f, false);
        if ((this.secondary_ports & BuildingCellVisualizer.Ports.LiquidOut) == ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
          return;
        this.DrawUtilityIcon(this.GetVisualizerCell(this.building, this.building.GetComponent<ISecondaryOutput>().GetSecondaryConduitOffset()), this.resources.liquidOutputIcon, ref this.secondaryOutputVisualizer, (Color) BuildingCellVisualizer.secondOutputColour, Color.white, 1.5f, false);
      }
      else
        this.DisableIcons();
    }
    else if (mode == OverlayModes.SolidConveyor.ID)
    {
      if (this.RequiresSolid)
      {
        if ((this.ports & BuildingCellVisualizer.Ports.SolidIn) != ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
        {
          bool flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityInputCell(), 20];
          BuildingCellVisualizerResources.ConnectedDisconnectedColours input = this.resources.liquidIOColours.input;
          this.DrawUtilityIcon(this.building.GetUtilityInputCell(), this.resources.liquidInputIcon, ref this.inputVisualizer, (Color) (!flag ? input.disconnected : input.connected));
        }
        if ((this.ports & BuildingCellVisualizer.Ports.SolidOut) == ~(BuildingCellVisualizer.Ports.PowerIn | BuildingCellVisualizer.Ports.PowerOut | BuildingCellVisualizer.Ports.GasIn | BuildingCellVisualizer.Ports.GasOut | BuildingCellVisualizer.Ports.LiquidIn | BuildingCellVisualizer.Ports.LiquidOut | BuildingCellVisualizer.Ports.SolidIn | BuildingCellVisualizer.Ports.SolidOut))
          return;
        bool flag1 = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[this.building.GetUtilityOutputCell(), 20];
        BuildingCellVisualizerResources.ConnectedDisconnectedColours output = this.resources.liquidIOColours.output;
        this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.resources.liquidOutputIcon, ref this.outputVisualizer, (Color) (!flag1 ? output.disconnected : output.connected));
      }
      else
        this.DisableIcons();
    }
    else
    {
      if (!(mode == OverlayModes.Disease.ID) || !((UnityEngine.Object) this.diseaseSourceSprite != (UnityEngine.Object) null))
        return;
      this.DrawUtilityIcon(this.building.GetUtilityOutputCell(), this.diseaseSourceSprite, ref this.inputVisualizer, (Color) this.diseaseSourceColour);
    }
  }

  private int GetVisualizerCell(Building building, CellOffset offset)
  {
    CellOffset rotatedOffset = building.GetRotatedOffset(offset);
    return Grid.OffsetCell(building.GetCell(), rotatedOffset);
  }

  public void DisableIcons()
  {
    if ((UnityEngine.Object) this.inputVisualizer != (UnityEngine.Object) null && this.inputVisualizer.activeInHierarchy)
      this.inputVisualizer.SetActive(false);
    if ((UnityEngine.Object) this.outputVisualizer != (UnityEngine.Object) null && this.outputVisualizer.activeInHierarchy)
      this.outputVisualizer.SetActive(false);
    if ((UnityEngine.Object) this.secondaryInputVisualizer != (UnityEngine.Object) null && this.secondaryInputVisualizer.activeInHierarchy)
      this.secondaryInputVisualizer.SetActive(false);
    if (!((UnityEngine.Object) this.secondaryOutputVisualizer != (UnityEngine.Object) null) || !this.secondaryOutputVisualizer.activeInHierarchy)
      return;
    this.secondaryOutputVisualizer.SetActive(false);
  }

  private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj)
  {
    this.DrawUtilityIcon(cell, icon_img, ref visualizerObj, Color.white, Color.white, 1.5f, false);
  }

  private void DrawUtilityIcon(
    int cell,
    Sprite icon_img,
    ref GameObject visualizerObj,
    Color tint)
  {
    this.DrawUtilityIcon(cell, icon_img, ref visualizerObj, tint, Color.white, 1.5f, false);
  }

  private void DrawUtilityIcon(
    int cell,
    Sprite icon_img,
    ref GameObject visualizerObj,
    Color tint,
    Color connectorColor,
    float scaleMultiplier = 1.5f,
    bool hideBG = false)
  {
    Vector3 posCcc = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
    if ((UnityEngine.Object) visualizerObj == (UnityEngine.Object) null)
    {
      visualizerObj = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, (string) null);
      visualizerObj.transform.SetAsFirstSibling();
      this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
    }
    if (!visualizerObj.gameObject.activeInHierarchy)
      visualizerObj.gameObject.SetActive(true);
    visualizerObj.GetComponent<Image>().enabled = !hideBG;
    this.icons[visualizerObj].raycastTarget = this.enableRaycast;
    this.icons[visualizerObj].sprite = icon_img;
    visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
    visualizerObj.transform.SetPosition(posCcc);
    if (!((UnityEngine.Object) visualizerObj.GetComponent<SizePulse>() == (UnityEngine.Object) null))
      return;
    visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
  }

  public Image GetOutputIcon()
  {
    if ((UnityEngine.Object) this.outputVisualizer == (UnityEngine.Object) null)
      return (Image) null;
    return this.outputVisualizer.transform.GetChild(0).GetComponent<Image>();
  }

  public Image GetInputIcon()
  {
    if ((UnityEngine.Object) this.inputVisualizer == (UnityEngine.Object) null)
      return (Image) null;
    return this.inputVisualizer.transform.GetChild(0).GetComponent<Image>();
  }

  [System.Flags]
  private enum Ports : byte
  {
    PowerIn = 1,
    PowerOut = 2,
    GasIn = 4,
    GasOut = 8,
    LiquidIn = 16, // 0x10
    LiquidOut = 32, // 0x20
    SolidIn = 64, // 0x40
    SolidOut = 128, // 0x80
  }
}
