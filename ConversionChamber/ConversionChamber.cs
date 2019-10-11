using Klei;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConversionChambers
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ConversionChamber : StateMachineComponent<Electrolyzer.StatesInstance>
    {
        [SerializeField]
        public float maxMass = 1f;
        [SerializeField]
        public float workSpeedMultiplier = 1f;
        [SerializeField]
        public Element.State output_state = Element.State.Vacuum;
        [SerializeField]
        public bool hasMeter = true;
        [MyCmpAdd]
        private Storage storage;
        [MyCmpGet]
        private ElementConverter emitter;
        [MyCmpReq]
        private Operational operational;
        private MeterController meter;

        protected override void OnSpawn()
        {
            KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
            if (this.hasMeter)
                this.meter = new MeterController(
                    building_controller: component, 
                    meter_target: "U2H_meter_target", 
                    meter_animation: "meter", 
                    front_back: Meter.Offset.Behind, 
                    user_specified_render_layer: Grid.SceneLayer.NoLayer, 
                    tracker_offset: new Vector3(-0.4f, 0.5f, -0.1f), 
                    symbols_to_hide: new string[4]{
                        "U2H_meter_target",
                        "U2H_meter_tank",
                        "U2H_meter_waterbody",
                        "U2H_meter_level"
                    });
            this.smi.StartSM();
            this.UpdateMeter();
        }

        protected override void OnCleanUp() => base.OnCleanUp();

        protected bool RoomForPressure() => !GameUtil.FloodFillCheck(null, this, Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition())), 3, true, true);

        public void UpdateMeter()
        {
            if (!this.hasMeter)
                return;
            this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
        }

        private static bool OverPressure(int cell, ConversionChamber chamber)
        {
            return Grid.Mass[cell] > chamber.maxMass;
        }

        public bool CanConvertAtAll()
        {
            bool return_state = true;
            List<GameObject> items = this.storage.items;
            foreach (GameObject go in items)
            {
                if (!(go == null) && go.GetComponent<PrimaryElement>().Mass > 0.0)
                {
                    return_state = true;
                    break;
                }
            }
            return return_state;
        }

        private bool HasEnoughMass()
        {
            float work_speed = 1f * this.workSpeedMultiplier;
            List<GameObject> items = this.storage.items;
            float cum_mass = 0.0f;

            foreach (GameObject go in items)
                if (go != null)
                    cum_mass += go.GetComponent<PrimaryElement>().Mass;

            if (cum_mass < work_speed)
                return false;
            else
                return true;
        }

        private void ConvertMass()
        {
            float work_speed = 1f * this.workSpeedMultiplier;
            List<GameObject> items = this.storage.items;
            SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
            diseaseInfo.idx = byte.MaxValue;
            diseaseInfo.count = 0;
            float num3 = 0.0f;
            float num4 = 0.0f;
            float num5 = 0.0f;

                foreach (GameObject go in items)
                {
                    if (go != null)
                    {

                            PrimaryElement component = go.GetComponent<PrimaryElement>();
                            component.KeepZeroMassObject = true;
                            float num6 = Mathf.Min(component.Mass);
                            int src2_count = (int)((double)(num6 / component.Mass) * (double)component.DiseaseCount);
                            float num7 = num6 * component.Element.specificHeatCapacity;
                            num5 += num7;
                            num4 += num7 * component.Temperature;
                            component.Mass -= num6;
                            component.ModifyDiseaseCount(-src2_count, "ElementConverter.ConvertMass");
                            num3 += num6;
                            diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component.DiseaseIdx, src2_count);
                            num2 -= num6;

                            if ((double)num2 <= 0.0)
                                break;
                    }
                }
            float b1 = (double)num5 <= 0.0 ? 0.0f : num4 / num5;
            if (this.onConvertMass != null && (double)num3 > 0.0)
                this.onConvertMass(num3);
            if (this.outputElements != null && this.outputElements.Length > 0)
            {
                for (int index = 0; index < this.outputElements.Length; ++index)
                {
                    ElementConverter.OutputElement outputElement = this.outputElements[index];
                    SimUtil.DiseaseInfo a2 = diseaseInfo;
                    if ((double)this.totalDiseaseWeight <= 0.0)
                    {
                        a2.idx = byte.MaxValue;
                        a2.count = 0;
                    }
                    else
                    {
                        float num2 = outputElement.diseaseWeight / this.totalDiseaseWeight;
                        a2.count = (int)((double)a2.count * (double)num2);
                    }
                    if (outputElement.addedDiseaseIdx != byte.MaxValue)
                        a2 = SimUtil.CalculateFinalDiseaseInfo(a2, new SimUtil.DiseaseInfo()
                        {
                            idx = outputElement.addedDiseaseIdx,
                            count = outputElement.addedDiseaseCount
                        });
                    float num6 = outputElement.massGenerationRate * this.OutputMultiplier * work_speed * a1;
                    Game.Instance.accumulators.Accumulate(outputElement.accumulator, num6);
                    float temperature = outputElement.useEntityTemperature || (double)b1 == 0.0 && (double)outputElement.minOutputTemperature == 0.0 ? this.GetComponent<PrimaryElement>().Temperature : Mathf.Max(outputElement.minOutputTemperature, b1);
                    Element elementByHash = ElementLoader.FindElementByHash(outputElement.elementHash);
                    if (outputElement.storeOutput)
                    {
                        PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num6, temperature);
                        if ((UnityEngine.Object)primaryElement == (UnityEngine.Object)null)
                        {
                            if (elementByHash.IsGas)
                                this.storage.AddGasChunk(outputElement.elementHash, num6, temperature, a2.idx, a2.count, true, true);
                            else if (elementByHash.IsLiquid)
                                this.storage.AddLiquid(outputElement.elementHash, num6, temperature, a2.idx, a2.count, true, true);
                            else
                                this.storage.Store(elementByHash.substance.SpawnResource(this.transform.GetPosition(), num6, temperature, a2.idx, a2.count, true, false, false), true, false, true, false);
                        }
                        else
                            primaryElement.AddDisease(a2.idx, a2.count, "ElementConverter.ConvertMass");
                    }
                    else
                    {
                        Vector3 vector3 = new Vector3(this.transform.GetPosition().x + outputElement.outputElementOffset.x, this.transform.GetPosition().y + outputElement.outputElementOffset.y, 0.0f);
                        int cell = Grid.PosToCell(vector3);
                        if (elementByHash.IsLiquid)
                        {
                            int idx = (int)elementByHash.idx;
                            FallingWater.instance.AddParticle(cell, (byte)idx, num6, temperature, a2.idx, a2.count, true, false, false, false);
                        }
                        else if (elementByHash.IsSolid)
                            elementByHash.substance.SpawnResource(vector3, num6, temperature, a2.idx, a2.count, false, false, false);
                        else
                            SimMessages.AddRemoveSubstance(cell, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num6, temperature, a2.idx, a2.count, true, -1);
                    }
                    if (outputElement.elementHash == SimHashes.Oxygen)
                        ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num6, this.gameObject.GetProperName(), (string)null);
                }
            }
            this.storage.Trigger(-1697596308, (object)this.gameObject);
        }

        public class StatesInstance : GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber, object>.GameInstance
        {
            public StatesInstance(ConversionChamber smi)
              : base(smi)
            {
            }
        }

        public class States : GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber>
        {
            public GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber, object>.State disabled;
            public GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber, object>.State waiting;
            public GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber, object>.State converting;
            public GameStateMachine<ConversionChamber.States, ConversionChamber.StatesInstance, ConversionChamber, object>.State overpressure;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.disabled;
                this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (smi => !smi.master.operational.IsOperational)).EventHandler(GameHashes.OnStorageChange, (smi => smi.master.UpdateMeter()));
                this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (smi => smi.master.operational.IsOperational));
                this.waiting.Enter("Waiting", (smi => smi.master.operational.SetActive(false, false))).EventTransition(GameHashes.OnStorageChange, this.converting, (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
                this.converting.Enter("Ready", (smi => smi.master.operational.SetActive(true, false))).Transition(this.waiting, (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()), UpdateRate.SIM_200ms).Transition(this.overpressure, (smi => !smi.master.RoomForPressure()), UpdateRate.SIM_200ms).Update("Convert", ((smi, dt) => smi.master.ConvertMass()), UpdateRate.SIM_1000ms, true); ;
                this.overpressure.Enter("OverPressure", (smi => smi.master.operational.SetActive(false, false))).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, (object)null).Transition(this.converting, (smi => smi.master.RoomForPressure()), UpdateRate.SIM_200ms);
            }
        }
    }
}
