using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal static class EquivalentCirculatingDensityCalculations
    {
        internal static double CalculateEquivalentCirculatingDensity(Fluid fluid, double pressureDropInPSI, double depth)
        {
            if ((fluid != null))
            {
                return ((fluid.DensityInPoundPerGallon + pressureDropInPSI) / (0.052 * depth));
            }
            else
            {
                return 0;
            }
        }
    }
}
