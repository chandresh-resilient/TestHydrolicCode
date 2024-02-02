using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type5Calculations
    {
        double nozzlePressureDrop = double.MinValue;
        double nozzleVelocity = double.MinValue;
        double nozzleTFA = double.MinValue;



        internal double CalculateHydraulicHorsePower(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA =Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles);
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
            if (nozzleTFA == double.MinValue)
                nozzleTFA = Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzleVelocity == double.MinValue)
                    nozzleVelocity = flowRateInGPM / (3.117 * nozzleTFA);
                return nozzleVelocity;
            }
            return double.MinValue;
        }

        internal double CalculateImpactForceInPounds(Fluid fluid, double flowRateInGPM,  List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA = Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzleVelocity == double.MinValue)
                    nozzleVelocity = flowRateInGPM / (3.117 * Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles));
                return fluid.DensityInPoundPerGallon * flowRateInGPM * nozzleVelocity / (32.2 * 60);
            }
            return double.MinValue;
        }
        internal double CalculateNozzlePressureDropInPSI(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles)
        {
            if (nozzleTFA == double.MinValue)
                nozzleTFA = Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles);
            if (nozzleTFA != 0)
            {
                if (nozzlePressureDrop == double.MinValue)
                    nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, flowRateInGPM, nozzles);
                return nozzlePressureDrop;
            }
            return double.MinValue;
        }


        internal PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM,  double insideDiameterInInches = 0, double lengthInFeet = 0)
        {
            //if (nozzleTFA == double.MinValue)
            //    nozzleTFA = Calculations.GeneralCalculations.CalculateTotalNozzleTFA(nozzles);
            //if (nozzleTFA != 0)
            //{
                //if (nozzlePressureDrop == double.MinValue)
                //    nozzlePressureDrop = PressureDropCalculations.CalculateNozzlePressureDrop(fluid, nozzles);
                PressureInformation pressureInfo = new PressureInformation();
                pressureInfo = PressureDropCalculations.CalculateType1PressureDropInPSI(fluid, flowRateInGPM, insideDiameterInInches, lengthInFeet);
                //pressureInfo.PressureDropInPSI += nozzlePressureDrop;
                return pressureInfo;
            //}
            //return new PressureInformation();
        }

        internal double CalculateAverageVelocityInFeetPerSecond(double flowRateInGPM, double insideDiameterInInches)
        {
            return double.MinValue;
        }


      

       
    }
}
