using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HydraulicEngine.Calculations
{
    internal static class VelocityCalculations
    {
        internal static double CalculateToolAverageVelocityInFeetPerSecond(double flowRateInGPM, double insideDiameterInInches)
        {
            if (insideDiameterInInches != 0)
            {
                return flowRateInGPM / (2.448 * Math.Pow(insideDiameterInInches, 2));
            }
            else
            {
                return 0;
            }
        }
        internal static double CalculateToolCriticalVelocityInFeetPerSecond(Fluid fluid, double insideDiameterInInches)
        {
            if ((insideDiameterInInches != double.MinValue) && (insideDiameterInInches != 0) && (fluid != null) && (fluid.DensityInPoundPerGallon != 0))
            {
                return (1.08 * fluid.PlasticViscosityInCentiPoise + 1.08 * Math.Sqrt(Math.Pow(fluid.PlasticViscosityInCentiPoise, 2) + 9.3 * Math.Pow(insideDiameterInInches, 2) * fluid.YieldPointInPoundPerFeetSquare * fluid.DensityInPoundPerGallon)) / (fluid.DensityInPoundPerGallon * insideDiameterInInches);
            }
            else
            {
                return 0;
            }
        }

        internal static double CalculateAnnulusAverageVelocityInFeetPerMinute(double flowRateInGPM, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            if ((annulusInsideDiameterInInches > 0) && (annulusInsideDiameterInInches > toolOutsideDiameterInInches))
            {
                return 60 * flowRateInGPM / (2.448 * (Math.Pow(annulusInsideDiameterInInches, 2) - Math.Pow(toolOutsideDiameterInInches, 2)));
            }
            else
            {
                return 0;
            }
        }

        internal static double CalculateAnnulusCriticalVelocityInFeetPerMinute(Fluid fluid, double annulusInsideDiameterInInches, double toolOutsideDiameter)
        {
            double difference;
            difference = annulusInsideDiameterInInches - toolOutsideDiameter;
            if ((difference != 0) && (fluid.DensityInPoundPerGallon != 0))
            {
                return 60 * (1.08 * fluid.PlasticViscosityInCentiPoise + 1.08 * Math.Sqrt(Math.Pow(fluid.PlasticViscosityInCentiPoise, 2) + 9.3 * Math.Pow(difference, 2) * fluid.YieldPointInPoundPerFeetSquare * fluid.DensityInPoundPerGallon)) / (fluid.DensityInPoundPerGallon * difference);
            }
            else
            {
                return 0;
            }
        }
        internal static double CalculateNozzleVelocityInFeetPerSecond(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            double nozzleVelocity = double.MinValue;
            double nozzleTFA = CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                nozzleVelocity = flowRateInGPM / (3.117 * nozzleTFA);
            }
            return nozzleVelocity;
        }

        private static double CalculateTotalNozzleTFA(List<Nozzles> nozzles)
        {
            double totalFlowArea = 0;
            foreach (Nozzles nozz in nozzles)
            {
                totalFlowArea += PressureDropCalculations.CalculateNozzleArea(nozz.NozzleDiameterInInch, nozz.NozzleQuantity);
            }
            return totalFlowArea;
        }

    }
}
