using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{

    internal class PressureInformation
    {
        private double pressureDrop;
        private string flowType;
        private double outputFlowRate = Double.MinValue;
        internal string FlowType
        {
            get { return flowType; }
            set { flowType = value; }
        }
        internal double PressureDropInPSI
        {
            get { return pressureDrop; }
            set { pressureDrop = value; }
        }
        internal double OutputFlowRateInGPM
        {
            get { return outputFlowRate; }
            set { outputFlowRate = value; }
        }
    }
    internal static class PressureDropCalculations
    {

        internal static PressureInformation CalculateSegmentPressureDropInPSI(Fluid fluid, double flowRateInGPM, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches, double lengthInFeet)
        {
            PressureInformation pressureInfo = new PressureInformation();
            if (flowRateInGPM <= 0)
            {
                pressureInfo.FlowType = Common.LaminarFlowType;
                pressureInfo.PressureDropInPSI = 0;
                return pressureInfo;
            }
            double diameterDiff = annulusInsideDiameterInInches - toolOutsideDiameterInInches;
            double averageVelocity = VelocityCalculations.CalculateAnnulusAverageVelocityInFeetPerMinute(flowRateInGPM, annulusInsideDiameterInInches, toolOutsideDiameterInInches);
            double criticalVelocity = VelocityCalculations.CalculateAnnulusCriticalVelocityInFeetPerMinute(fluid, annulusInsideDiameterInInches, toolOutsideDiameterInInches);

            double laminarPressureDrop = 0;
            double turbulentPressureDrop = 0;
            double pressureDrop = 0;
            if (averageVelocity > criticalVelocity)
            {
                //Turbulent
                pressureInfo.FlowType = Common.TurbulentFlowType;
            }
            else
            {
                pressureInfo.FlowType = Common.LaminarFlowType;
            }

            laminarPressureDrop = CalculateSegmentLaminarPressureDrop(fluid, averageVelocity, diameterDiff, lengthInFeet);
            turbulentPressureDrop = CalculateSegmentTurbulentPressureDrop(fluid, flowRateInGPM, averageVelocity, annulusInsideDiameterInInches, toolOutsideDiameterInInches, lengthInFeet);
            pressureDrop = Common.SmoothingValue(averageVelocity, criticalVelocity, laminarPressureDrop, turbulentPressureDrop);
            pressureInfo.PressureDropInPSI = pressureDrop;
            return pressureInfo;
        }

        internal static PressureInformation CalculateType1PressureDropInPSI(Fluid fluid, double flowRateInGPM, double insideDiameterInInches, double lengthInFeet)
        {

            PressureInformation pressureInfo = new PressureInformation();
            if (flowRateInGPM <= 0)
            {
                pressureInfo.FlowType = Common.LaminarFlowType;
                pressureInfo.PressureDropInPSI = 0;
                return pressureInfo;
            }
            double averageVelocity = VelocityCalculations.CalculateToolAverageVelocityInFeetPerSecond(flowRateInGPM, insideDiameterInInches);
            double criticalVelocity = VelocityCalculations.CalculateToolCriticalVelocityInFeetPerSecond(fluid, insideDiameterInInches);
            // PressureInformation pressureInfo = new PressureInformation();
            double laminarPressureDrop = 0;
            double turbulentPressureDrop = 0;
            double pressureDrop = 0;
            if (averageVelocity > criticalVelocity)
            {
                //Turbulent
                pressureInfo.FlowType = Common.TurbulentFlowType;
            }
            else
            {
                pressureInfo.FlowType = Common.LaminarFlowType;
            }

            laminarPressureDrop = CalculateType1ToolLaminarPressureDrop(fluid, insideDiameterInInches, lengthInFeet, averageVelocity);
            turbulentPressureDrop = CalculateType1ToolTurbulentPressureDrop(fluid, insideDiameterInInches, lengthInFeet, averageVelocity);
            pressureDrop = Common.SmoothingValue(averageVelocity, criticalVelocity, laminarPressureDrop, turbulentPressureDrop);
            pressureInfo.PressureDropInPSI = pressureDrop;
            return pressureInfo;
        }


        internal static double CalculateNozzlePressureDrop(Fluid fluid, double flowRateInGPM, List<Nozzles> nozzles = null, double nozzleCoeff = double.MinValue, double nozzleTFAData = double.MinValue)
        {
            double pressureDrop = 0;
            double temp = 0;
            double nozzleCoefficient;
            double nozzleTFA;
            double totalTFA = 0;
            if (nozzles != null && nozzles.Count > 0)
            {
                //foreach (Nozzles nozzle in nozzles)
                //{
                //    totalTFA += CalculateNozzleArea(nozzle.NozzleDiameterInInch, nozzle.NozzleQuantity);
                //}

                foreach (Nozzles nozzle in nozzles)
                {
                    if (nozzle.NozzleCoefficient != double.MinValue)
                        nozzleCoefficient = nozzle.NozzleCoefficient;
                    else if (nozzle.NozzleType == Nozzles.NozzleTypes.Jet)
                        nozzleCoefficient = Common.JetCoeeficient;
                    else
                        nozzleCoefficient = Common.HoleCoeeficient;

                    nozzleTFA = CalculateNozzleArea(nozzle.NozzleDiameterInInch, nozzle.NozzleQuantity);
                    totalTFA += nozzleCoefficient * nozzleTFA;
                    
                }
                if (nozzles[0].NozzleCoefficient != double.MinValue)
                    nozzleCoefficient = nozzles[0].NozzleCoefficient;
                else if (nozzles[0].NozzleType == Nozzles.NozzleTypes.Jet)
                    nozzleCoefficient = Common.JetCoeeficient;
                else
                    nozzleCoefficient = Common.HoleCoeeficient;

                nozzleTFA = CalculateNozzleArea(nozzles[0].NozzleDiameterInInch, nozzles[0].NozzleQuantity);
                temp = nozzleCoefficient * nozzleTFA / totalTFA;

                pressureDrop +=  fluid.DensityInPoundPerGallon * Math.Pow(temp * flowRateInGPM, 2) * 8.311 * 0.00001 / (Math.Pow(nozzleCoefficient, 2) * Math.Pow(nozzleTFA, 2));

            }
            else if ((nozzleCoeff != double.MinValue) && (nozzleTFAData != double.MinValue))
            {
                nozzleCoefficient = nozzleCoeff;
                nozzleTFA = nozzleTFAData;
                pressureDrop += fluid.DensityInPoundPerGallon * Math.Pow(flowRateInGPM, 2) * 8.311 * 0.00001 / (Math.Pow(nozzleCoefficient, 2) * Math.Pow(nozzleTFA, 2));
            }

            return pressureDrop;
        }


        private static double CalculateType1ToolTurbulentPressureDrop(Fluid fluid, double insideDiameter, double length, double averageVelocityInFeetPerSecond)
        {
            double pressureDrop = 0;
            double reynoldsNumber = CalculateReynoldsNumber(fluid, insideDiameter, averageVelocityInFeetPerSecond);
            if ((reynoldsNumber > 0) && (insideDiameter > 0))
            {
                double frictionFactor = 0.0791 / Math.Pow(reynoldsNumber, 0.25);
                pressureDrop = length * frictionFactor * fluid.DensityInPoundPerGallon * Math.Pow(averageVelocityInFeetPerSecond, 2) / (25.8 * insideDiameter);
            }
            return pressureDrop;
        }

        private static double CalculateType1ToolLaminarPressureDrop(Fluid fluid, double insideDiameter, double length, double averageVelocityInFeetPerSecond)
        {
            double pressureDrop = 0;
            if (insideDiameter > 0)
            {
                pressureDrop = length * ((fluid.PlasticViscosityInCentiPoise * averageVelocityInFeetPerSecond / (1500 * Math.Pow(insideDiameter, 2))) + (fluid.YieldPointInPoundPerFeetSquare / (225 * insideDiameter)));
            }
            return pressureDrop;
        }


        private static double CalculateReynoldsNumber(Fluid fluid, double insideDiameter, double averageVelocityInFeetPerSecond)
        {
            double reynoldsNumber = 0;
            if (fluid.PlasticViscosityInCentiPoise != 0)
            {
                reynoldsNumber = 928 * insideDiameter * fluid.DensityInPoundPerGallon * averageVelocityInFeetPerSecond / fluid.PlasticViscosityInCentiPoise;
            }
            return reynoldsNumber;
        }

        public static double CalculateNozzleArea(double nozzleDiameter, double nozzleQuantity)
        {
            double totalFlowArea = 0;
            totalFlowArea += nozzleQuantity * Math.PI * Math.Pow(nozzleDiameter, 2) / 4;
            return totalFlowArea;
        }

        private static double CalculateSegmentLaminarPressureDrop(Fluid fluid, double averageVelocityInFeetPerMinute, double diameterDifferenceInInches, double lengthInFeet)
        {
            double averageVelocityInFeetPerSecond = averageVelocityInFeetPerMinute / 60;
            double returnValue = 0;
            if (diameterDifferenceInInches > 0)
                returnValue = lengthInFeet * (fluid.PlasticViscosityInCentiPoise * averageVelocityInFeetPerSecond / (1000 * Math.Pow(diameterDifferenceInInches, 2)) + (fluid.YieldPointInPoundPerFeetSquare / (267 * diameterDifferenceInInches)));
            return returnValue;
        }

        private static double CalculateSegmentTurbulentPressureDrop(Fluid fluid, double flowRateInGPM, double averageVelocityInFeetPerMinute, double annulusInsideDiameterInInches, double toolOutsideDiameteInInches, double lengthInFeet)
        {
            double averageVelocityInFeetPerSecond = averageVelocityInFeetPerMinute / 60;
            double returnValue = 0;
            double diameterDifference = annulusInsideDiameterInInches - toolOutsideDiameteInInches;
            double reynoldsNumber = CalculateSegmentReynoldsNumber(fluid, averageVelocityInFeetPerSecond, diameterDifference);
            if ((reynoldsNumber != 0) && (diameterDifference > 0))
            {
                double frictionFactor = 0.0791 / Math.Pow(reynoldsNumber, 0.25);
                returnValue = lengthInFeet * frictionFactor * fluid.DensityInPoundPerGallon * Math.Pow(averageVelocityInFeetPerSecond, 2) / (25.8 * diameterDifference);
            }
            return returnValue;
        }

        private static double CalculateSegmentReynoldsNumber(Fluid fluid, double averageVelocityinFeetPerSecond, double DiameterDifferenceInInches)
        {
            double returnValue = 0;
            if (fluid.PlasticViscosityInCentiPoise != 0)
                returnValue = 928 * fluid.DensityInPoundPerGallon * averageVelocityinFeetPerSecond * DiameterDifferenceInInches / (fluid.PlasticViscosityInCentiPoise);
            return returnValue;
        }

        private static double CalculateSegmentHedstromsNumber(Fluid fluid, double DiameterDifferenceInInches)
        {
            double returnValue = 0;
            if (fluid.PlasticViscosityInCentiPoise != 0)
                returnValue = 24700 * (fluid.DensityInPoundPerGallon * fluid.YieldPointInPoundPerFeetSquare * Math.Pow(DiameterDifferenceInInches, 2) / Math.Pow(fluid.PlasticViscosityInCentiPoise, 2));
            return returnValue;
        }


        internal static double CalculateSurfaceEquipmentPressureDropInPSI(Fluid fluid, double flowRateInGPM, double insideDiameterInInches, double lengthInFeet)
        {

            double returnValue;

            if (flowRateInGPM <= 0)
            {
                returnValue = 0;
                return returnValue;
            }
            double averageVelocity = VelocityCalculations.CalculateToolAverageVelocityInFeetPerSecond(flowRateInGPM, insideDiameterInInches);
            double criticalVelocity = VelocityCalculations.CalculateToolCriticalVelocityInFeetPerSecond(fluid, insideDiameterInInches);
            // PressureInformation pressureInfo = new PressureInformation();
            double laminarPressureDrop = 0;
            double turbulentPressureDrop = 0;
            double pressureDrop = 0;

            if (fluid != null)
            {
                laminarPressureDrop = CalculateSurfaceEquipmentLaminarPressureDrop(fluid, insideDiameterInInches, lengthInFeet, averageVelocity);
                turbulentPressureDrop = CalculateSurfaceEquipmentTurbulentPressureDrop(fluid, insideDiameterInInches, lengthInFeet, averageVelocity);
            }
            pressureDrop = Common.SmoothingValue(averageVelocity, criticalVelocity, laminarPressureDrop, turbulentPressureDrop);
            return pressureDrop;
        }

        private static double CalculateSurfaceEquipmentTurbulentPressureDrop(Fluid fluid, double flowRateInGPM, double insideDiameterInInches, double lengthInFeet)
        {
            if (insideDiameterInInches > 0)
                return (0.0000765 * Math.Pow(fluid.PlasticViscosityInCentiPoise, 0.18) * Math.Pow(fluid.DensityInPoundPerGallon, 0.82) * Math.Pow(flowRateInGPM, 1.82) * lengthInFeet) / Math.Pow(insideDiameterInInches, 4.82);
            else
                return 0;
        }
        private static double CalculateSurfaceEquipmentLaminarPressureDrop(Fluid fluid, double insideDiameterInInches, double lengthInFeet, double averageVelocityInFeetPerSecond)
        {
            return CalculateType1ToolLaminarPressureDrop(fluid, insideDiameterInInches, lengthInFeet, averageVelocityInFeetPerSecond);
        }

    }
}
