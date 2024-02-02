using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{

    internal static class SplitFLowCalculations
    {
        internal static double CalculateFlowRateInGPM(Fluid fluid, double flowRateInGPM, List<BHATool> bhaTools, double positionNumber, List<Nozzles> nozzels, double torqueInFeetPound = 0, double nozzelTFA = 0, double nozzleCoeffecient = 0, List<Segment> segments = null)
        {
            
            double belowFlowRate = 0;
            double tempFlowRate = -100;
            double pressureLossAcrossToolsBelow = 0;
            double nozzelFlowRate = 0;
            double flowRate = 0;
            double totalFlowRate = flowRateInGPM;
            double nozzleData;
            int iterations = 0;
            if (flowRateInGPM == 0)
                return 0;
            while (((Math.Abs( Math.Round(tempFlowRate,0) - Math.Round(belowFlowRate, 0)) > 1) && (tempFlowRate != -1)) || tempFlowRate == 0)
            {
                iterations += iterations + 1;
                pressureLossAcrossToolsBelow = 0;
                nozzelFlowRate = 0;
                flowRateInGPM = totalFlowRate;
                if (tempFlowRate == -100)
                {
                    tempFlowRate = 0.5 * flowRateInGPM;
                }
                else if (belowFlowRate > tempFlowRate)
                {
                    tempFlowRate = tempFlowRate + Math.Abs (belowFlowRate - tempFlowRate) / 2;
                }
                else
                {
                    tempFlowRate = tempFlowRate - Math.Abs (belowFlowRate - tempFlowRate) / 2;

                }

                //if (tempFlowRate < 0)
                //   return 0;
                //else if (tempFlowRate > totalFlowRate)
                //    return totalFlowRate;


                flowRate = tempFlowRate;
                
                foreach (BHATool bhatool in bhaTools)
                {
                    if (bhatool.PositionNumber == positionNumber)
                    {
                        IBHASplitFlowTool splitFlowTool = (IBHASplitFlowTool)bhatool;
                        pressureLossAcrossToolsBelow += splitFlowTool.GetToolPressureLoss(fluid, totalFlowRate, flowRate);
                    }
                    if (bhatool.PositionNumber > positionNumber)
                    {
                        bhatool.CalculateHydraulics(fluid, flowRate,torqueInFeetPound,bhaTools);

                        if (bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute != double.MinValue)
                            flowRate = bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute;

                        pressureLossAcrossToolsBelow += bhatool.BHAHydraulicsOutput.PressureDropInPSI;
                        pressureLossAcrossToolsBelow += GetAnnulusPressureDrop(bhatool.toolDescription, bhatool.OutsideDiameterInInch, bhatool.PositionNumber, fluid, flowRate, segments);
                    }
                }

                if (nozzels != null && nozzels.Count > 0)
                {
                    double tfa = 0;
                    double nozzleCoefficient;
                    nozzelFlowRate = 0;
                    foreach (Nozzles nozzle in nozzels)
                    {
                        if (nozzle.NozzleCoefficient != double.MinValue)
                            nozzleCoefficient = nozzle.NozzleCoefficient;
                        else if (nozzle.NozzleType == Nozzles.NozzleTypes.Jet)
                            nozzleCoefficient = Common.JetCoeeficient;
                        else
                            nozzleCoefficient = Common.HoleCoeeficient;
                        tfa = PressureDropCalculations.CalculateNozzleArea(nozzle.NozzleDiameterInInch, nozzle.NozzleQuantity);
                        if (tfa != 0)
                        {
                            nozzelFlowRate += Math.Sqrt(pressureLossAcrossToolsBelow * (Math.Pow(nozzleCoefficient, 2) * Math.Pow(tfa, 2)) / (8.312 * .00001 * fluid.DensityInPoundPerGallon ));
                        }
                    }

                    //nozzleData = CalculateNozzleMultiplier (nozzels);
                    //nozzelFlowRate = Math.Sqrt(pressureLossAcrossToolsBelow/(8.312 * .00001 * fluid.DensityInPoundPerGallon*nozzleData));
                }
                else
                {
                    nozzelFlowRate = Math.Sqrt( Math.Pow(nozzelTFA, 2) *  Math.Pow(nozzleCoeffecient, 2) * pressureLossAcrossToolsBelow / (8.311 * .00001 * fluid.DensityInPoundPerGallon));
                }
                belowFlowRate = totalFlowRate - nozzelFlowRate;
                if (belowFlowRate <= 0)
                    belowFlowRate = tempFlowRate / 2;
                else if (iterations >= 200)
                    return ((belowFlowRate+ tempFlowRate)/2);
                else if (belowFlowRate > totalFlowRate)
                    return totalFlowRate;
            }
            if (iterations >= 200)
            {
                double previousBHAFlowrate= 0;
                double previousPressureDifference = double.MaxValue;
                double currentPressureDifference = double.MaxValue;
                double nozzlePressureDrop;
                belowFlowRate = 0;
                while ( Math.Abs( previousPressureDifference) >= Math.Abs(currentPressureDifference))
                {
                    pressureLossAcrossToolsBelow = 0;
                    previousPressureDifference = currentPressureDifference;
                    previousBHAFlowrate = belowFlowRate;
                    belowFlowRate += 1;
                    nozzelFlowRate = totalFlowRate - belowFlowRate;
                    flowRate = belowFlowRate;
                    foreach (BHATool bhatool in bhaTools)
                    {
                        if (bhatool.PositionNumber == positionNumber)
                        {
                            IBHASplitFlowTool splitFlowTool = (IBHASplitFlowTool)bhatool;
                            pressureLossAcrossToolsBelow += splitFlowTool.GetToolPressureLoss(fluid, totalFlowRate, flowRate);
                        }
                        if (bhatool.PositionNumber > positionNumber)
                        {
                            bhatool.CalculateHydraulics(fluid, flowRate, torqueInFeetPound, bhaTools);

                            if (bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute != double.MinValue)
                                flowRate = bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute;

                            pressureLossAcrossToolsBelow += bhatool.BHAHydraulicsOutput.PressureDropInPSI;
                            pressureLossAcrossToolsBelow += GetAnnulusPressureDrop(bhatool.toolDescription, bhatool.OutsideDiameterInInch, bhatool.PositionNumber, fluid, flowRate, segments);
                        }
                    }
                    if (nozzels != null && nozzels.Count > 0)
                    {
                        nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, nozzelFlowRate, nozzels);
                    }
                    else
                    {
                        nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, nozzelFlowRate, nozzels);
                    }
                    currentPressureDifference =  pressureLossAcrossToolsBelow - nozzlePressureDrop;
                    if (Math.Round(currentPressureDifference, 0) == 0)
                        break;
                    if (belowFlowRate == totalFlowRate)
                        break;
                }
                if (previousPressureDifference <= currentPressureDifference)
                    return previousBHAFlowrate;
            }

            return belowFlowRate;
        }

        internal static void UpdateAnnulusBelowCurrentToolForZeroFlow(Fluid fluid, int positionNumber, List<Segment> segments = null)
        {
            if (segments != null)
            {
                IEnumerable<Segment> selected = segments.Where(x => x.ToolPositionNumber > positionNumber);
                foreach (Segment seg in selected)
                {
                    seg.ReCalculateHydraulics(fluid, 0);//Considering zero flow rate for all the annulus segment below the split tool for which the flow is closed to BHA.
                }
            }
        }

        private static double CalculateNozzleMultiplier(List<Nozzles> nozzles)
        {
            double multiplier = 0;
            double nozzleCoefficient;
            double tfa;
            double value;
            if (nozzles != null)
            {
                foreach (Nozzles nozzle in nozzles)
                {
                    if (nozzle.NozzleCoefficient != double.MinValue)
                        nozzleCoefficient = nozzle.NozzleCoefficient;
                    else if (nozzle.NozzleType == Nozzles.NozzleTypes.Jet)
                        nozzleCoefficient = Common.JetCoeeficient;
                    else
                        nozzleCoefficient = Common.HoleCoeeficient;

                    tfa =  PressureDropCalculations.CalculateNozzleArea(nozzle.NozzleDiameterInInch, nozzle.NozzleQuantity);
                    if (tfa != 0)
                    {
                        value = 1 / (Math.Pow(nozzleCoefficient, 2) * Math.Pow(tfa, 2));
                        multiplier += value;
                    }
                }
            }
           return multiplier;
        }

        private static double GetAnnulusPressureDrop (string toolDescription, double toolOD, int toolPositionNumber, Fluid fluid, double predictedFlowRate, List<Segment> segments = null)
        {
            double returnValue = 0;
            if (segments == null)
                return returnValue;
            IEnumerable<Segment> selected = segments.Where(x => x.ToolODInInch == toolOD && x.ToolDescription == toolDescription && x.ToolPositionNumber == toolPositionNumber);
            foreach (Segment seg in selected)
            {
                seg.ReCalculateHydraulics(fluid, predictedFlowRate);
                returnValue += seg.SegmentHydraulicsOutput.PressureDropInPSI;
            }
            return returnValue;
        }
    }

}
