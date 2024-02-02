using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class ChipRateInformation
    {
        private double chipRate;
        private Common.ResultType result;

        internal Common.ResultType ResultType
        {
            get { return result; }
            set { result = value; }
        }
        internal double ChipRateInFeetPerMinute
        {
            get { return chipRate; }
            set { chipRate = value; }
        }

    }
    internal static class ChipRateCalculations
    {
        internal static ChipRateInformation CalculateSegmentChipRateInFeetperMinute(Fluid fluid, double flowRateInGPM, Cuttings cuttings, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches)
        {
            double diameterDiff = annulusInsideDiameterInInches - toolOutsideDiameterInInches;
            double averageVelocity = VelocityCalculations.CalculateAnnulusAverageVelocityInFeetPerMinute(flowRateInGPM, annulusInsideDiameterInInches, toolOutsideDiameterInInches);
            double criticalVelocity = VelocityCalculations.CalculateAnnulusCriticalVelocityInFeetPerMinute(fluid, annulusInsideDiameterInInches, toolOutsideDiameterInInches);
            ChipRateInformation chipRateInfo = new ChipRateInformation();
            double laminarChipRate = 0;
            double turbulentChipRate = 0;
            double chipRate = 0;


            laminarChipRate = CalculateLaminarChipRate(fluid, cuttings, annulusInsideDiameterInInches, toolOutsideDiameterInInches, averageVelocity);
            turbulentChipRate = CalculateTurbulentChipRate(fluid, cuttings, averageVelocity);
            chipRate = Common.SmoothingValue(averageVelocity, criticalVelocity, laminarChipRate, turbulentChipRate);
            chipRateInfo.ChipRateInFeetPerMinute = chipRate;
            chipRateInfo.ResultType = ChipRateResult(averageVelocity, chipRate);
            return chipRateInfo;
        }

        private static double CalculateTurbulentChipRate(Fluid fluid, Cuttings cuttings, double averageVelocityInFeetPerMinute)
        {
            double returnValue = 0;
            if (fluid.DensityInPoundPerGallon != 0)
            {
                double slipVelocity = CalculateTurbulentSlipVelocityinFeetPerMinute (cuttings, fluid);
                returnValue = averageVelocityInFeetPerMinute - slipVelocity;
            }
            return returnValue;
        }

        private static double CalculateLaminarChipRate(Fluid fluid, Cuttings cuttings, double annulusInsideDiameterInInches, double toolOutsideDiameterInInches, double averageVelocityInFeetPerMinute)
        {
            double returnValue = 0;
            double diameterDifference = annulusInsideDiameterInInches - toolOutsideDiameterInInches;
            if (fluid.DensityInPoundPerGallon != 0)
            {
                double slipVelocity = CalculateLaminarSlipVelocityinFeetPerMinute(fluid, cuttings, diameterDifference, averageVelocityInFeetPerMinute);
                returnValue = averageVelocityInFeetPerMinute - slipVelocity;
            }
            return returnValue;
        }

        private static double CalculateTurbulentSlipVelocityinFeetPerMinute (Cuttings cuttings, Fluid fluid)
        {
            double returnValue = 0;
            double cuttingDensity;
            if ((fluid.DensityInPoundPerGallon > 0) && (cuttings.AverageCuttingSizeInInch > 0))
            {
                if (cuttings.CuttingsType == Common.CuttingType.Steel)
                    cuttingDensity = Common.SteelCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.Rock)
                    cuttingDensity = Common.RockCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.CastIron)
                    cuttingDensity = Common.CastIronCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.Granite)
                    cuttingDensity = Common.GraniteCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.Sandstone)
                    cuttingDensity = Common.SandstoneCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.Concrete)
                    cuttingDensity = Common.ConcreteCuttingDensityinPoundPerGallon;
                else if (cuttings.CuttingsType == Common.CuttingType.WetSand)
                    cuttingDensity = Common.WetSandCuttingDensityinPoundPerGallon;
                else
                    cuttingDensity = Common.RockCuttingDensityinPoundPerGallon;

                double factor1 = fluid.PlasticViscosityInCentiPoise / (fluid.DensityInPoundPerGallon * cuttings.AverageCuttingSizeInInch);
                double factor2 = (cuttingDensity - fluid.DensityInPoundPerGallon) / fluid.DensityInPoundPerGallon;

                returnValue = 0.45 * factor1 * (Math.Sqrt(((36800 * cuttings.AverageCuttingSizeInInch * factor2) / Math.Pow(factor1, 2)) + 1) - 1);

                //if (cuttings.CuttingsType == Common.CuttingType.Steel)
                //    returnValue = 60.6 * Math.Sqrt(cuttings.AverageCuttingSizeInInch * Math.Abs(Common.SteelCuttingDensityinPoundPerGallon - mudDensity)/mudDensity);
                //else
                //    returnValue = 155.9 * Math.Sqrt(cuttings.AverageCuttingSizeInInch * Math.Abs(Common.RockCuttingDensityinPoundPerGallon - mudDensity)/mudDensity);
            }

            return returnValue;
        }

        private static double CalculateLaminarSlipVelocityinFeetPerMinute(Fluid fluid, Cuttings cuttings, double diameterDifference, double averageVelovityFeetPerMinute)
        {
            double returnValue = 0;
            double effectiveViscosity ;
            double cuttingDensity;
            if (averageVelovityFeetPerMinute != 0)
                effectiveViscosity = fluid.PlasticViscosityInCentiPoise + (399 * fluid.YieldPointInPoundPerFeetSquare * diameterDifference / averageVelovityFeetPerMinute);
            else
                effectiveViscosity = fluid.PlasticViscosityInCentiPoise;

            if (cuttings.CuttingsType == Common.CuttingType.Steel)
                cuttingDensity = Common.SteelCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.Rock)
                cuttingDensity = Common.RockCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.CastIron)
                cuttingDensity = Common.CastIronCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.Granite)
                cuttingDensity = Common.GraniteCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.Sandstone)
                cuttingDensity = Common.SandstoneCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.Concrete)
                cuttingDensity = Common.ConcreteCuttingDensityinPoundPerGallon;
            else if (cuttings.CuttingsType == Common.CuttingType.WetSand)
                cuttingDensity = Common.WetSandCuttingDensityinPoundPerGallon;
            else 
                cuttingDensity = Common.RockCuttingDensityinPoundPerGallon;

            returnValue = 3226 * Math.Pow (cuttings.AverageCuttingSizeInInch, 2) * (cuttingDensity - fluid.DensityInPoundPerGallon)/effectiveViscosity;
            
            return returnValue;
        }

        private static Common.ResultType ChipRateResult(double averageVelocity, double chipRate)
        {
            Common.ResultType returnValue = Common.ResultType.Good;

            if (chipRate <= .5 * averageVelocity)
                returnValue = Common.ResultType.Problem;
            if (chipRate <= .75 * averageVelocity)
                returnValue = Common.ResultType.Caution;

            return returnValue;
        }
    }
}
