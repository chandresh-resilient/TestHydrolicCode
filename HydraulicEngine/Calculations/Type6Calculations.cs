using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type6Calculations
    {
        
        internal SplitFlowPressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM, Common.ToolState currentAnnulusState, List<BHATool> bhaTools, int positionNumber, double torqueInFeetPound, List<Nozzles> bhaNozzelInfo, List<Nozzles> annulusNozzelInfo, double lengthInFeet, double insideDiameterInInches, double lengthBeforeAnnulusOpeningInFeet, double lengthAfterAnnulusOpeningInFeet, Common.ToolState bhaState, List<Segment> segments = null)
        {
            SplitFlowPressureInformation pressureInfo = new SplitFlowPressureInformation();
            double annulusFlowRate = 0;
            double bhaFlowRate = 0;
            pressureInfo.InputFlowrateInGPM = flowRateInGPM;
            if ((currentAnnulusState == Common.ToolState.OpenToAnnulus) && (bhaState == Common.ToolState.OpenToAnnulus))
            {
                bhaFlowRate = Calculations.SplitFLowCalculations.CalculateFlowRateInGPM(fluid, flowRateInGPM, bhaTools, positionNumber, annulusNozzelInfo, torqueInFeetPound, 0, 0, segments);//Send TFA info
                annulusFlowRate = flowRateInGPM - bhaFlowRate;
                //flowRateInGPM = bhaFlowRate;
            }
            else if ((bhaState == Common.ToolState.CloseToAnnulus) && (currentAnnulusState == Common.ToolState.OpenToAnnulus))
            {
                annulusFlowRate = flowRateInGPM;
                Calculations.SplitFLowCalculations.UpdateAnnulusBelowCurrentToolForZeroFlow(fluid, positionNumber, segments);
            }
            else if ((bhaState == Common.ToolState.OpenToAnnulus) && (currentAnnulusState == Common.ToolState.CloseToAnnulus))
                bhaFlowRate = flowRateInGPM;
            pressureInfo = GetPressureDrop (fluid, pressureInfo.InputFlowrateInGPM, bhaFlowRate, insideDiameterInInches, bhaNozzelInfo, lengthInFeet, lengthBeforeAnnulusOpeningInFeet, lengthAfterAnnulusOpeningInFeet, bhaState);
            pressureInfo.AnnulusFlowrateInGPM = annulusFlowRate;
            pressureInfo.BHAFlowrateInGPM = bhaFlowRate;
            pressureInfo.AnnulusOpeningPressureDropInPSI= PressureDropCalculations.CalculateNozzlePressureDrop(fluid, annulusFlowRate,annulusNozzelInfo);
            if (bhaState == Common.ToolState.CloseToAnnulus)
                pressureInfo.TotalPressureDropInPSI = pressureInfo.AnnulusOpeningPressureDropInPSI;
            pressureInfo.AnnulusNozzleVelocityInFeetPerSecond = VelocityCalculations.CalculateNozzleVelocityInFeetPerSecond(fluid, annulusFlowRate, annulusNozzelInfo);
            pressureInfo.BHANozzleVelocityInFeetPerSecond = VelocityCalculations.CalculateNozzleVelocityInFeetPerSecond(fluid, bhaFlowRate, bhaNozzelInfo);
            return pressureInfo;
        }

        internal double CalculateAverageVelocityInFeetPerSecond(double flowRateInGPM, double insideDiameterInInches)
        {
            return Calculations.VelocityCalculations.CalculateToolAverageVelocityInFeetPerSecond(flowRateInGPM, insideDiameterInInches);
        }

        internal double CalculateCriticalVelocityInFeetPerSecond(Fluid fluid, double insideDiameterInInches)
        {
            return Calculations.VelocityCalculations.CalculateToolCriticalVelocityInFeetPerSecond(fluid, insideDiameterInInches);
        }
        internal double CalculateEquivalentCirculatingDensity(Fluid fluid, double pressureDropInPSI, double depth)
        {
            return Calculations.EquivalentCirculatingDensityCalculations.CalculateEquivalentCirculatingDensity(fluid, pressureDropInPSI, depth);
        }
        internal SplitFlowPressureInformation GetPressureDrop(Fluid fluid, double inputFlowrateInGPM,  double outputFlowrateInGPM, double IDInInches, List<Nozzles> BHANozzle, double lengthInFeet, double lengthBeforeAnnulusOpeningInFeet, double lengthAfterAnnulusOpeningInFeet, Common.ToolState bhaState)
        {
            SplitFlowPressureInformation returnValue = new SplitFlowPressureInformation();
            if ( bhaState == Common.ToolState.CloseToAnnulus)
            {
                returnValue.BHAOpeningPressureDropInPSI = 0;
                returnValue.TotalPressureDropInPSI = 0;
                return returnValue;
            }
            
            returnValue.InputFlowrateInGPM = inputFlowrateInGPM;
            if (((lengthAfterAnnulusOpeningInFeet == double.MinValue) || (lengthBeforeAnnulusOpeningInFeet == double.MinValue)) || ((lengthAfterAnnulusOpeningInFeet == 0) || (lengthBeforeAnnulusOpeningInFeet == 0)))
            {
                if (lengthInFeet != double.MinValue)
                    returnValue.ToolPressureDropInPSI = PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, inputFlowrateInGPM, IDInInches, lengthInFeet).PressureDropInPSI;
            }
            else
            {
                returnValue.ToolPressureDropInPSI = PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, inputFlowrateInGPM, IDInInches, lengthBeforeAnnulusOpeningInFeet).PressureDropInPSI;
            }
            returnValue.BHAOpeningPressureDropInPSI = GetBHAOpeningPressureDrop(fluid, outputFlowrateInGPM, IDInInches, BHANozzle, lengthAfterAnnulusOpeningInFeet, bhaState);
            returnValue.TotalPressureDropInPSI = returnValue.ToolPressureDropInPSI + returnValue.BHAOpeningPressureDropInPSI;
            return returnValue;
        }

        internal double GetBHAOpeningPressureDrop(Fluid fluid, double outputFlowrateInGPM, double IDInInches, List<Nozzles> BHANozzle,double lengthAfterAnnulusOpeningInFeet, Common.ToolState bhaState)
        {
            double returnValue = 0;
            if (bhaState == Common.ToolState.CloseToAnnulus)
            {
                return returnValue;
            }
            returnValue = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, outputFlowrateInGPM, BHANozzle);
            
            if ((lengthAfterAnnulusOpeningInFeet != 0) && (lengthAfterAnnulusOpeningInFeet != double.MinValue))
            {
                returnValue += PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, outputFlowrateInGPM, IDInInches, lengthAfterAnnulusOpeningInFeet).PressureDropInPSI;
            }
            return returnValue;
        }

        internal double CalculateNozzleVelocityInFeetPerSecond(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            return VelocityCalculations.CalculateNozzleVelocityInFeetPerSecond(fluid, flowRateInGPM, nozzles);
        }


    }
}
