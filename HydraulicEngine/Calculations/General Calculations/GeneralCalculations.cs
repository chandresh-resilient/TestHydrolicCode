using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    public static class GeneralCalculations
    {
        internal static double CalculateTotalNozzleTFA(List<Nozzles> nozzles)
        {
            double totalFlowArea = 0;
            foreach (Nozzles nozz in nozzles)
            {
                
                    totalFlowArea +=   PressureDropCalculations.CalculateNozzleArea(nozz.NozzleDiameterInInch, nozz.NozzleQuantity);
               
            }
            return totalFlowArea;
        }
    }
}
