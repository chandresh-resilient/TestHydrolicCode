using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type1Calculations
    {
        internal  PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM,  double insideDiameterInInches, double lengthInFeet)
        {
            return Calculations.PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, flowRateInGPM, insideDiameterInInches, lengthInFeet);
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
    }
}
