using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public static class Common
    {
        public const string TurbulentFlowType = "Turbulent";
        public const string LaminarFlowType = "Laminar";
        public const double JetCoeeficient = 0.95;
        public const double HoleCoeeficient = 0.82;

        public enum ToolTypes { Type1, Type2, Type3, Type4, Type5, Type6, Type7, Type8, Type9 };
        public enum SurfaceEquipmentCaseType { Case1, Case2, Case3, Case4, TopDrive };
        public enum ToolState { OpenToAnnulus, CloseToAnnulus };

        public enum BHAType2ModelName
        {
            NullMotor, MacDrill168, MacDrill212, MacDrill287, MacDrill287GB, CTDPDM168, CTDPDM212, CTDPDM237, CTDPDM287TwoStage, CTDPDM287, CTDPDM312, CTDPDM475, eCTDNull, eCTD168, eCTD212, eCTD287
        }

        public enum CuttingType
        {
            Steel, Rock, CastIron, Granite, Sandstone, Concrete, WetSand
        };

        public const double SteelCuttingDensityinPoundPerGallon = 65.5;
        public const double RockCuttingDensityinPoundPerGallon = 21.7;
        public const double CastIronCuttingDensityinPoundPerGallon = 60.15;
        public const double GraniteCuttingDensityinPoundPerGallon = 22.05;
        public const double SandstoneCuttingDensityinPoundPerGallon = 19.11;
        public const double ConcreteCuttingDensityinPoundPerGallon = 26.20;
        public const double WetSandCuttingDensityinPoundPerGallon = 16.84;

        public enum ResultType { Good, Caution, Problem };

        public static double SmoothingValue(double averageVelocity, double criticalVelocity, double laminarValue, double turbulentValue)
        {
            double returnValue;
            if (averageVelocity <= 0.6 * criticalVelocity)
            {
                returnValue = laminarValue;
            }
            else if (averageVelocity >= criticalVelocity)
            {
                returnValue = turbulentValue;
            }
            else
            {
                double turbulentFraction = (((averageVelocity / criticalVelocity) - 0.6) * 100) / 40;
                returnValue = ((turbulentFraction * turbulentValue) + ((1 - turbulentFraction) * laminarValue));
            }
            return returnValue;
        }

    }
}
