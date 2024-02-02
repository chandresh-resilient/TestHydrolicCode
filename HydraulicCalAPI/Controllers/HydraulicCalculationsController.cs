using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HydraulicEngine;
using HydraulicCalAPI.Service;

namespace HydraulicCalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HydraulicCalculationsController : ControllerBase
    {

        public static BHATool ConvertWorkstringToBHAForHydraulic(int positionNumber, string sectionName, double outerDiameter, double innerDiameter, double wrkstrLength)
        {
            BHAToolType1 bhaToolTyp1 = new BHAToolType1(positionNumber, sectionName != null ? sectionName : "", outerDiameter, wrkstrLength, innerDiameter, double.MinValue);
            bhaToolTyp1.ToolIdentifier = Guid.NewGuid();
            return bhaToolTyp1;
        }

        [HttpPost("getHydraulicCalculations")]
        public Dictionary<String, Object> getHydraulicCalculations([FromBody] HydraulicCalAPI.Service.HydraulicCalculationService objHcs)
        {
            List<HydraulicEngine.BHATool> bhatools = new List<HydraulicEngine.BHATool>();
            SurfaceEquipment equipment = new SurfaceEquipment(objHcs.surfaceEquipmentInput.CaseType);
            foreach (var item in objHcs.bhaInput)
            {
                var toolcasetype = item.bhatooltype;
                Console.WriteLine(toolcasetype);
                HydraulicEngine.BHATool bhaToolItem;
                switch (toolcasetype)
                {
                    case "type1":
                        {   // int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch, double toolDepth
                            bhaToolItem = new BHAToolType1
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                InsideDiameterInInch = item.InsideDiameterInInch,
                                Depth = item.Depth
                            };
                            break;
                        }
                    case "type2":
                        {   //int positionNumber, string toolDescription, Common.BHAType2ModelName modelName,double length,double outerDiameter
                            bhaToolItem = new BHAToolType2
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                LengthInFeet = item.LengthInFeet,
                                ModelName = item.ModelName,//Check with Shwetang
                                OutsideDiameterInInch = item.OutsideDiameterInInch
                            };
                            break;
                        }
                    case "type3":
                        {   //int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> nozzles, double insideDiameterInInch = 0
                            bhaToolItem = (new BHAToolType3
                            {
                                PositionNumber = item.PositionNumber,
                                Depth = item.Depth,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                NozzlesInfomation = item.NozzlesInfomation,//Check with Shwetang Nozzel Cofficient kahan se aayega
                                InsideDiameterInInch = item.InsideDiameterInInch
                            });
                            break;
                        }
                    case "type4":
                        {   //int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState, double actuationFlowrateInGPA,
                            //double valveInsideDiameterinInch, double minimumSidePortAreaInInch2, double maximumPortAreaInInch2, double gapNutInsideDiameterInInch, double gapWidthInInch
                            bhaToolItem = (new BHAToolType4
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                CurrentState = item.CurrentState,
                                ActuatingFlowRateInGallonsPerMinute = item.ActuatingFlowRateInGallonsPerMinute,
                                ValveInsertDiameterInInch = item.ValveInsertDiameterInInch,
                                MinimumSidePortAreaInInch2 = item.MinimumSidePortAreaInInch2,
                                MaximumSidePortAreaInInch2 = item.MaximumSidePortAreaInInch2,
                                GapNutInsideDiameterInInch = item.GapNutInsideDiameterInInch,
                                GapWidthInInch = item.GapWidthInInch
                            });
                            break;
                        }
                    case "type5":
                        {   //int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> annulusNozzles, double insideDiameterInInch = 0
                            bhaToolItem = (new BHAToolType5
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                AnnulusNozzleInformation = item.AnnulusNozzleInformation,
                                InsideDiameterInInches = item.InsideDiameterInInch
                            });
                            break;
                        }
                    case "type6":
                        {   //int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState,
                            //List<Nozzles> bhaNozzleInfo, List<Nozzles> annulusNozzleInfo, double insideDiameterInInches, double lengthBeforeAnnulusOpeningInFeet,
                            //double lengthAfterAnnnulusOpeningInFeet, Common.ToolState bhaToolState = Common.ToolState.OpenToAnnulus
                            bhaToolItem = (new BHAToolType6
                            {
                                PositionNumber = item.PositionNumber,
                                Depth = item.Depth,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                CurrentState = item.CurrentState,
                                BHANozzleInformation = item.BHANozzleInformation,
                                AnnulusNozzleInformation = item.AnnulusNozzleInformation,
                                InsideDiameterInInches = item.InsideDiameterInInch,
                                LengthBeforeAnnulusOpeningInFeet = item.LengthBeforeAnnulusOpeningInFeet,
                                LengthAfterAnnulusOpeningInFeet = item.LengthAfterAnnulusOpeningInFeet,
                                BHAOpeningState = item.BHAOpeningState
                            });
                            break;
                        }
                    case "type7":
                        {   //int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double pressureDropInPSI
                            bhaToolItem = (new BHAToolType7
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                PressureDropInPSI = item.PressureDropInPSI
                            });
                            break;
                        }
                    case "type8":
                        {   //int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch = 0
                            bhaToolItem = (new BHAToolType8
                            {
                                PositionNumber = item.PositionNumber,
                                Depth = item.Depth,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                InsideDiameterInInch = item.InsideDiameterInInch
                            });
                            break;
                        }
                    case "type9":
                        {   //int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch,
                            //double observedFlowRateInGallonsPerMinute, double observedPressureDropInPSI
                            bhaToolItem = (new BHAToolType9
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                InsideDiameterInInch = item.InsideDiameterInInch,
                                ObservedFlowRateInGallonsPerMinute = item.ObservedFlowRateInGallonsPerMinute,
                                ObservedPressureDropInPSI = item.ObservedPressureDropInPSI
                            });
                            break;
                        }
                    case "type10":
                        {   //int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> nozzles,
                            //double insideDiameterInInch = 0, Accuset toolAccuset=null
                            bhaToolItem = (new BHAToolType10
                            {
                                PositionNumber = item.PositionNumber,
                                Depth = item.Depth,
                                toolDescription = item.toolDescription,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                NozzlesInfomation = item.NozzlesInfomation,
                                InsideDiameterInInch = item.InsideDiameterInInch,
                                ToolAccuset = item.ToolAccuset
                            });
                            break;
                        }
                    default:
                        {   //int positionNumber,  string sectionName, double outsideDiameterInInch, double insideDiameterInInch, double lengthInFeet
                            // BHATOOLType1 // int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch, double toolDepth
                            // BHATool convertedResult = ConvertWorkstringToBHAForHydraulic(item.PositionNumber, item.SectionName, item.OutsideDiameterInInch,
                            //                                                           item.InsideDiameterInInch, item.LengthInFeet);
                            //Console.WriteLine(convertedResult);
                            bhaToolItem = (new BHAToolType1
                            {
                                PositionNumber = item.PositionNumber,
                                toolDescription = item.SectionName,
                                OutsideDiameterInInch = item.OutsideDiameterInInch,
                                LengthInFeet = item.LengthInFeet,
                                InsideDiameterInInch = item.InsideDiameterInInch,
                                Depth = item.Depth
                            });

                            break;
                        }
                }
                bhatools.Add(bhaToolItem);
            }

            if ((double.IsNaN(objHcs.toolDepthInFeet) || objHcs.toolDepthInFeet == 0))
            {
                for (int i = 0; i < objHcs.annulusInput.Count; i++)
                {
                    objHcs.toolDepthInFeet += objHcs.annulusInput[i].AnnulusBottomInFeet;
                }
            }

            HydraulicAnalysisOutput response = Main.CompleteHydraulicAnalysis(objHcs.fluidInput, objHcs.flowRateInGPMInput, objHcs.cuttingsInput, bhatools, objHcs.annulusInput, equipment, objHcs.torqueInFeetPound=0, objHcs.toolDepthInFeet, objHcs.blockPostionInFeet=double.MinValue);

            ChartAndGraphService objChartnGraph = new ChartAndGraphService();
            return objChartnGraph.GetDataPoints(response, objHcs.fluidInput,
                                                objHcs.flowRateInGPMInput,
                                                objHcs.cuttingsInput,
                                                bhatools,
                                                objHcs.annulusInput,
                                                equipment,
                                                objHcs.maxflowrate,
                                                objHcs.maxflowpressure, objHcs.toolDepthInFeet);

        }
    }
}