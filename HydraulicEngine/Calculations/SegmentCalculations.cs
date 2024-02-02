using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class SegmentCalculations
    {
        double averageVelocity = double.MinValue;
        double criticalVelocity = double.MinValue;
        internal PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches, double lengthInFeet)
        {
            return Calculations.PressureDropCalculations.CalculateSegmentPressureDropInPSI(fluid, flowRateInGPM, annulusInsideDiameterInInches, toolOutsideDiameterInInches, lengthInFeet);
        }

        internal double CalculateAverageVelocityInFeetPerMinute(double flowRateInGPM,  double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            CalculateAverageVelocity (flowRateInGPM,annulusInsideDiameterInInches,toolOutsideDiameterInInches);
            return averageVelocity;
        }

        internal double CalculateCriticalVelocityInFeetPerMinute(Fluid fluid, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            CalculateCriticalVelocity(fluid, annulusInsideDiameterInInches, toolOutsideDiameterInInches);
            return criticalVelocity;
        }

        internal ChipRateInformation CalculateChipRateInFeetPerInch(Fluid fluid, double flowRateInGPM, Cuttings cuttings, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            return Calculations.ChipRateCalculations.CalculateSegmentChipRateInFeetperMinute(fluid, flowRateInGPM, cuttings, annulusInsideDiameterInInches, toolOutsideDiameterInInches);
        }

        private void CalculateAverageVelocity (double flowRateInGPM,  double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            if (averageVelocity == double.MinValue)
                averageVelocity = Calculations.VelocityCalculations.CalculateAnnulusAverageVelocityInFeetPerMinute (flowRateInGPM, annulusInsideDiameterInInches, toolOutsideDiameterInInches);

        }

        private void CalculateCriticalVelocity(Fluid fluid, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            if (criticalVelocity == double.MinValue)
                criticalVelocity = Calculations.VelocityCalculations.CalculateAnnulusCriticalVelocityInFeetPerMinute(fluid, annulusInsideDiameterInInches, toolOutsideDiameterInInches);

        }
        internal double CalculateEquivalentCirculatingDensity(Fluid fluid, double pressureDropInPSI, double depth)
        {
            return Calculations.EquivalentCirculatingDensityCalculations.CalculateEquivalentCirculatingDensity(fluid, pressureDropInPSI, depth);
        }
    }
}
