using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type9Calculations
    {
        internal PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM, double insideDiameterInInches, double lengthInFeet, double observedFlowRateInGallonsPerMinute, double observedPressureDropInPSI)
        {
            double correctionFactor;
            PressureInformation output;
            // define a new fluid that has same properties as the fluid assed as parameter except for Flow rate. Flow rate should be observed flow rate passed
            //Fluid newFluid = new Fluid();
            //newFluid.DensityInPoundPerGallon = fluid.DensityInPoundPerGallon;
            //newFluid.FlowRateInGPM = observedFlowRateInGallonsPerMinute;
            //newFluid.PlasticViscosityInCentiPoise = fluid.PlasticViscosityInCentiPoise;
            //newFluid.YieldPointInPoundPerFeetSquare = fluid.YieldPointInPoundPerFeetSquare;
            //Calculate Pressure drop at the pbserved flow rate
            output = Calculations.PressureDropCalculations.CalculateType1PressureDropInPSI(fluid,observedFlowRateInGallonsPerMinute, insideDiameterInInches, lengthInFeet);
            //Calculate correction factor on the basis of observed pressure drop and calculated pressure drop
            correctionFactor = 1 + ((observedPressureDropInPSI - output.PressureDropInPSI) / output.PressureDropInPSI);
            //Calculate Pressure drop for the orginal fuid
            output =  Calculations.PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, flowRateInGPM, insideDiameterInInches, lengthInFeet);
            // apply correction factor to the pressure drop calculated
            output.PressureDropInPSI = correctionFactor * output.PressureDropInPSI;
            return output;
        }

        internal double CalculateAverageVelocityInFeetPerSecond(double flowRateInGPM, double insideDiameterInInches)
        {
            return Calculations.VelocityCalculations.CalculateToolAverageVelocityInFeetPerSecond(flowRateInGPM, insideDiameterInInches);
        }
    }
}
