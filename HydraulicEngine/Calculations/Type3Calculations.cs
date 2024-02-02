using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    
    internal class Type3Calculations
    {
        double nozzlePressureDrop = double.MinValue;
        double nozzleVelocity = double.MinValue;
        double nozzleTFA = double.MinValue;
    
       
        
        internal double CalculateHydraulicHorsePower(Fluid fluid, double flowRateInGPM,  List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA = CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzlePressureDrop == double.MinValue)
                    nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, flowRateInGPM, nozzles);
                return nozzlePressureDrop * flowRateInGPM / 1714;
            }
            return double.MinValue;
        }

        internal double CalculateNozzleVelocityInFeetPerSecond(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            
            if (nozzleVelocity == double.MinValue)
                nozzleVelocity = VelocityCalculations.CalculateNozzleVelocityInFeetPerSecond (fluid, flowRateInGPM,nozzles);
            return nozzleVelocity;
            
        }

        internal double CalculateImpactForceInPounds(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA = CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzleVelocity == double.MinValue)
                    nozzleVelocity = flowRateInGPM / (3.117 * CalculateTotalNozzleTFA(nozzles));
                return fluid.DensityInPoundPerGallon * flowRateInGPM * nozzleVelocity / (32.2 * 60);
            }
            return double.MinValue;
        }

        internal double CalculateNozzlePressureDropInPSI(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA = CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzlePressureDrop == double.MinValue)
                    nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, flowRateInGPM,  nozzles);
                return nozzlePressureDrop;
            }
            return double.MinValue;
        }

        internal PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles, double insideDiameterInInches = 0, double lengthInFeet = 0)
        {
            PressureInformation pressureInfo = new PressureInformation();
            pressureInfo = PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, flowRateInGPM, insideDiameterInInches, lengthInFeet);
            
            
            if (nozzleTFA == double.MinValue)
                nozzleTFA = CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzlePressureDrop == double.MinValue)
                    nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, flowRateInGPM, nozzles);
                pressureInfo.PressureDropInPSI += nozzlePressureDrop;
            }
            return pressureInfo;
        }


        private double CalculateTotalNozzleTFA ( List<Nozzles> nozzles)
        {
            double totalFlowArea = 0;
            foreach (Nozzles nozz in nozzles)
            {
                totalFlowArea += PressureDropCalculations.CalculateNozzleArea(nozz.NozzleDiameterInInch, nozz.NozzleQuantity);
            }
            return totalFlowArea;
        }

        internal double CalculateCriticalVelocityInFeetPerSecond(Fluid fluid, double insideDiameterInInches)
        {
            return Calculations.VelocityCalculations.CalculateToolCriticalVelocityInFeetPerSecond(fluid, insideDiameterInInches);
        }
        internal double CalculateAverageVelocityInFeetPerSecond(double flowRateInGPM, double insideDiameterInInches)
        {
            return Calculations.VelocityCalculations.CalculateToolAverageVelocityInFeetPerSecond(flowRateInGPM, insideDiameterInInches);
        }
        internal double CalculateEquivalentCirculatingDensity(Fluid fluid, double pressureDropInPSI, double depth)
        {
            return Calculations.EquivalentCirculatingDensityCalculations.CalculateEquivalentCirculatingDensity(fluid, pressureDropInPSI, depth);
        }
    }
}
