using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type8Calculations
    {
        internal double CalculateEquivalentCirculatingDensity(Fluid fluid, double pressureDropInPSI, double depth)
        {
            return Calculations.EquivalentCirculatingDensityCalculations.CalculateEquivalentCirculatingDensity(fluid, pressureDropInPSI, depth);
        }
    }
}
