using HydraulicCalAPI.Service;
using HydraulicEngine;
using System;
using System.Collections.Generic;
using System.Linq;

internal static class HydraulicCalculationsControllerHelpers
{
    public enum NozzleTypes { Jet, Hole };
    public static BHATool ConvertWorkstringToBHAForHydraulic(int positionNumber, string sectionName, double outerDiameter, double innerDiameter, double wrkstrLength)
    {
        BHAToolType1 bhaToolTyp1 = new BHAToolType1(positionNumber, sectionName != null ? sectionName : "", outerDiameter, wrkstrLength, innerDiameter, double.MinValue);
        bhaToolTyp1.ToolIdentifier = Guid.NewGuid();
        return bhaToolTyp1;
    }

    public static List<BHATool> getBHATools(HydraulicCalculationService objHcs)
    {
        List<HydraulicEngine.BHATool> bhatools = new List<HydraulicEngine.BHATool>();

        foreach (var item in objHcs.bhaInput)
        {
            var toolcasetype = item.bhatooltype;
            Console.WriteLine(toolcasetype);
            HydraulicEngine.BHATool bhaToolItem;
            switch (toolcasetype)
            {
                case "type1":
                    {
                        bhaToolItem = new BHAToolType1
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            InsideDiameterInInch = item.InsideDiameterInInch,
                        };
                        break;
                    }
                case "type2":
                    {
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
                    {
                        bhaToolItem = (new BHAToolType3
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            NozzlesInfomation = GetNozzleList(item.NozzlesInfomation),
                            InsideDiameterInInch = item.InsideDiameterInInch
                        });
                        break;
                    }
                case "type4":
                    {
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
                    {
                        bhaToolItem = (new BHAToolType5
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            AnnulusNozzleInformation = GetNozzleList(item.AnnulusNozzleInformation),
                            InsideDiameterInInches = item.InsideDiameterInInch
                        });
                        break;
                    }
                case "type6":
                    {
                        bhaToolItem = (new BHAToolType6
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            CurrentState = item.CurrentState,
                            BHANozzleInformation = GetNozzleList(item.BHANozzleInformation),
                            AnnulusNozzleInformation = GetNozzleList(item.AnnulusNozzleInformation),
                            InsideDiameterInInches = item.InsideDiameterInInch,
                            LengthBeforeAnnulusOpeningInFeet = item.LengthBeforeAnnulusOpeningInFeet,
                            LengthAfterAnnulusOpeningInFeet = item.LengthAfterAnnulusOpeningInFeet,
                            BHAOpeningState = item.BHAOpeningState
                        });
                        break;
                    }
                case "type7":
                    {
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
                    {
                        bhaToolItem = (new BHAToolType8
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            InsideDiameterInInch = item.InsideDiameterInInch
                        });
                        break;
                    }
                case "type9":
                    {
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
                    {
                        bhaToolItem = (new BHAToolType10
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.toolDescription,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            NozzlesInfomation = GetNozzleList(item.NozzlesInfomation),
                            InsideDiameterInInch = item.InsideDiameterInInch,
                            ToolAccuset = new Accuset(item.ToolAccuset.AccusetSystemName,item.ToolAccuset.StandardNozzleSize, objHcs.fluidInput.DensityInPoundPerGallon)
                        });
                        break;
                    }
                default:
                    {
                        bhaToolItem = (new BHAToolType1
                        {
                            PositionNumber = item.PositionNumber,
                            toolDescription = item.SectionName,
                            OutsideDiameterInInch = item.OutsideDiameterInInch,
                            LengthInFeet = item.LengthInFeet,
                            InsideDiameterInInch = item.InsideDiameterInInch,
                        });

                        break;
                    }
            }
            bhatools.Add(bhaToolItem);
        }

        return bhatools;
    }

    private static List<Nozzles> GetNozzleList(List<HydraulicCalculationService.BHATool.Nozzles> nozzlesInfomation)
    {
        var objNozzle = nozzlesInfomation;
        List<Nozzles> lstNozzles = new List<Nozzles>();
        if (objNozzle != null)
        {
            foreach (HydraulicCalculationService.BHATool.Nozzles noxzzel in objNozzle)
            {
                Nozzles _nozzles = new Nozzles
                {
                    ncount = objNozzle[0].NozzleQuantity,
                    nozType = (Nozzles.NozzleTypes)objNozzle[0].NozzleType,
                    dia = objNozzle[0].NozzleDiameterInInch
                };
                lstNozzles.Add(_nozzles);
            }
        }
        
        return lstNozzles;

    }
}