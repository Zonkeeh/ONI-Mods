// Decompiled with JetBrains decompiler
// Type: BuildToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BuildToolHoverTextCard : HoverTextConfiguration
{
  public BuildingDef currentDef;

  public override void UpdateHoverElements(List<KSelectable> hoverObjects)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar(false);
    this.ActionName = (string) (!((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null) || !this.currentDef.DragBuild ? UI.TOOLS.BUILD.TOOLACTION : UI.TOOLS.BUILD.TOOLACTION_DRAG);
    if ((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null && this.currentDef.Name != null)
      this.ToolName = string.Format((string) UI.TOOLS.BUILD.NAME, (object) this.currentDef.Name);
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(instance, drawer);
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    int min_height = 26;
    int width = 8;
    if ((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) PlayerController.Instance.ActiveTool != (UnityEngine.Object) null)
      {
        System.Type type = PlayerController.Instance.ActiveTool.GetType();
        if (typeof (BuildTool).IsAssignableFrom(type) || typeof (BaseUtilityBuildTool).IsAssignableFrom(type))
        {
          if ((UnityEngine.Object) this.currentDef.BuildingComplete.GetComponent<Rotatable>() != (UnityEngine.Object) null)
          {
            drawer.NewLine(min_height);
            drawer.AddIndent(width);
            string text = UI.TOOLTIPS.HELP_ROTATE_KEY.ToString().Replace("{Key}", GameUtil.GetActionString(Action.RotateBuilding));
            drawer.DrawText(text, this.Styles_Instruction.Standard);
          }
          Orientation buildingOrientation = BuildTool.Instance.GetBuildingOrientation;
          string fail_reason = "Unknown reason";
          Vector3 posCcc = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
          if (!this.currentDef.IsValidPlaceLocation(BuildTool.Instance.visualizer, posCcc, buildingOrientation, out fail_reason))
          {
            drawer.NewLine(min_height);
            drawer.AddIndent(width);
            drawer.DrawText(fail_reason, this.HoverTextStyleSettings[1]);
          }
          RoomTracker component = this.currentDef.BuildingComplete.GetComponent<RoomTracker>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.SufficientBuildLocation(cell))
          {
            drawer.NewLine(min_height);
            drawer.AddIndent(width);
            drawer.DrawText((string) UI.TOOLTIPS.HELP_REQUIRES_ROOM, this.HoverTextStyleSettings[1]);
          }
        }
      }
      drawer.NewLine(min_height);
      drawer.AddIndent(width);
      drawer.DrawText(ResourceRemainingDisplayScreen.instance.GetString(), this.Styles_BodyText.Standard);
      drawer.EndShadowBar();
      HashedString mode = SimDebugView.Instance.GetMode();
      if (mode == OverlayModes.Logic.ID && hoverObjects != null)
      {
        SelectToolHoverTextCard component1 = SelectTool.Instance.GetComponent<SelectToolHoverTextCard>();
        foreach (KSelectable hoverObject in hoverObjects)
        {
          LogicPorts component2 = hoverObject.GetComponent<LogicPorts>();
          LogicPorts.Port port1;
          bool isInput;
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.TryGetPortAtCell(cell, out port1, out isInput))
          {
            bool flag = component2.IsPortConnected(port1.id);
            drawer.BeginShadowBar(false);
            int num;
            if (isInput)
            {
              string replacement = !port1.displayCustomName ? UI.LOGIC_PORTS.PORT_INPUT_DEFAULT_NAME.text : port1.description;
              num = component2.GetInputValue(port1.id);
              drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_INPUT_HOVER_FMT.Replace("{Port}", replacement).Replace("{Name}", hoverObject.GetProperName().ToUpper()), component1.Styles_Title.Standard);
            }
            else
            {
              string replacement = !port1.displayCustomName ? UI.LOGIC_PORTS.PORT_OUTPUT_DEFAULT_NAME.text : port1.description;
              num = component2.GetOutputValue(port1.id);
              drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_OUTPUT_HOVER_FMT.Replace("{Port}", replacement).Replace("{Name}", hoverObject.GetProperName().ToUpper()), component1.Styles_Title.Standard);
            }
            drawer.NewLine(26);
            TextStyleSetting style1 = !flag ? component1.Styles_LogicActive.Standard : (num != 1 ? component1.Styles_LogicSignalInactive : component1.Styles_LogicActive.Selected);
            drawer.DrawIcon(num != 1 || !flag ? component1.iconDash : component1.iconActiveAutomationPort, style1.textColor, 18, 2);
            drawer.DrawText(port1.activeDescription, style1);
            drawer.NewLine(26);
            TextStyleSetting style2 = !flag ? component1.Styles_LogicStandby.Standard : (num != 0 ? component1.Styles_LogicSignalInactive : component1.Styles_LogicStandby.Selected);
            drawer.DrawIcon(num != 0 || !flag ? component1.iconDash : component1.iconActiveAutomationPort, style2.textColor, 18, 2);
            drawer.DrawText(port1.inactiveDescription, style2);
            drawer.EndShadowBar();
          }
          LogicGate component3 = hoverObject.GetComponent<LogicGate>();
          LogicGateBase.PortId port2;
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.TryGetPortAtCell(cell, out port2))
          {
            int portValue = component3.GetPortValue(port2);
            bool portConnected = component3.GetPortConnected(port2);
            LogicGate.LogicGateDescriptions.Description portDescription = component3.GetPortDescription(port2);
            drawer.BeginShadowBar(false);
            if (port2 == LogicGateBase.PortId.Output)
              drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_OUTPUT_HOVER_FMT.Replace("{Port}", portDescription.name).Replace("{Name}", hoverObject.GetProperName().ToUpper()), component1.Styles_Title.Standard);
            else
              drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_INPUT_HOVER_FMT.Replace("{Port}", portDescription.name).Replace("{Name}", hoverObject.GetProperName().ToUpper()), component1.Styles_Title.Standard);
            drawer.NewLine(26);
            TextStyleSetting style1 = !portConnected ? component1.Styles_LogicActive.Standard : (portValue != 1 ? component1.Styles_LogicSignalInactive : component1.Styles_LogicActive.Selected);
            drawer.DrawIcon(portValue != 1 || !portConnected ? component1.iconDash : component1.iconActiveAutomationPort, style1.textColor, 18, 2);
            drawer.DrawText(portDescription.active, style1);
            drawer.NewLine(26);
            TextStyleSetting style2 = !portConnected ? component1.Styles_LogicStandby.Standard : (portValue != 0 ? component1.Styles_LogicSignalInactive : component1.Styles_LogicStandby.Selected);
            drawer.DrawIcon(portValue != 0 || !portConnected ? component1.iconDash : component1.iconActiveAutomationPort, style2.textColor, 18, 2);
            drawer.DrawText(portDescription.inactive, style2);
            drawer.EndShadowBar();
          }
        }
      }
      else if (mode == OverlayModes.Power.ID)
      {
        CircuitManager circuitManager = Game.Instance.circuitManager;
        ushort circuitId = circuitManager.GetCircuitID(cell);
        if (circuitId != ushort.MaxValue)
        {
          drawer.BeginShadowBar(false);
          float watts = circuitManager.GetWattsNeededWhenActive(circuitId) + this.currentDef.EnergyConsumptionWhenActive;
          float wattageForCircuit = circuitManager.GetMaxSafeWattageForCircuit(circuitId);
          Color color = (double) watts < (double) wattageForCircuit ? Color.white : Color.red;
          drawer.AddIndent(width);
          drawer.DrawText(string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, (object) GameUtil.GetFormattedWattage(watts, GameUtil.WattageFormatterUnit.Automatic)), this.Styles_BodyText.Standard, color, true);
          drawer.EndShadowBar();
        }
      }
    }
    drawer.EndDrawing();
  }
}
