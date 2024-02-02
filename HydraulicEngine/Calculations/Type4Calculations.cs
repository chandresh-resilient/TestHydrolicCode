using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine.Calculations
{
    internal class Type4Calculations
    {
        internal double dischargeCoefficientThroughTool = 0.5;
        internal double dischargeCoefficientToAnnulus = 0.38;

        double sidePortArea = double.MinValue;
        double valveInsertArea = double.MinValue;
        double deActuatingFlowRate = double.MinValue;
        Common.ToolState finalState;
        internal PressureInformation CalculateTotalPressureDropInPSI(Fluid fluid, double flowRateInGPM, double actuatingFlowRateInGallonsPerMinute, Common.ToolState currentState, double gapNutInsideDiameterinInch, double gapWidthInInch, double valveInsertDiameterInInch, double minimumSidePortAreaInInch2, double maximumSidePortAreaInInch2)
        {
            PressureInformation pressureInfo = new PressureInformation();
            pressureInfo.FlowType = Common.TurbulentFlowType;
            double pressureDrop = double.MinValue;
            CalculateSidePortArea(gapNutInsideDiameterinInch, gapWidthInInch, minimumSidePortAreaInInch2, maximumSidePortAreaInInch2);
            CalculateValveInsertArea(valveInsertDiameterInInch);
            finalState = CalculateFinalState(flowRateInGPM, actuatingFlowRateInGallonsPerMinute, currentState, gapNutInsideDiameterinInch, gapWidthInInch, valveInsertDiameterInInch, minimumSidePortAreaInInch2, maximumSidePortAreaInInch2);
            if ((finalState == Common.ToolState.OpenToAnnulus) && ( sidePortArea !=0))
                pressureDrop = (Math.Pow(flowRateInGPM, 2) * fluid.DensityInPoundPerGallon) / (12042.8 * Math.Pow(dischargeCoefficientToAnnulus, 2) * Math.Pow(sidePortArea, 2));
            else if ((finalState == Common.ToolState.CloseToAnnulus) && (valveInsertArea != 0))
                pressureDrop = (Math.Pow(flowRateInGPM, 2) * fluid.DensityInPoundPerGallon) / (12042.8 * Math.Pow(dischargeCoefficientThroughTool, 2) * Math.Pow(valveInsertArea, 2));
            pressureInfo.PressureDropInPSI = pressureDrop;
            return pressureInfo;
        }

        internal double CalculateAverageVelocityInFeetPerSecond(double flowRateInGPM)
        {
            return double.MinValue;
        }

        internal double CalculateDeActuatingFlowRate(double actuatingFlowRateinGallonsPerMinute, double gapNutInsideDiameterinInch, double gapWidthInInch, double valveInsertDiameterInInch, double minimumSidePortAreaInInch2, double maximumSidePortAreaInInch2)
        {
            if (deActuatingFlowRate == double.MinValue)
            {
                if (valveInsertDiameterInInch != 0)
                {
                    CalculateSidePortArea(gapNutInsideDiameterinInch, gapWidthInInch, minimumSidePortAreaInInch2, maximumSidePortAreaInInch2);
                    CalculateValveInsertArea(valveInsertDiameterInInch);
                    deActuatingFlowRate = actuatingFlowRateinGallonsPerMinute * dischargeCoefficientToAnnulus * sidePortArea / (dischargeCoefficientThroughTool * valveInsertArea);
                }
            }
            return deActuatingFlowRate;
        }

        internal Common.ToolState CalculateFinalState (double flowRateInGallonsPerMinute, double actuatingFlowRateInGallonsPerMinute, Common.ToolState currentState, double gapNutInsideDiameterinInch, double gapWidthInInch, double valveInsertDiameterInInch, double minimumSidePortAreaInInch2, double maximumSidePortAreaInInch2 )
        {
            
            if (currentState == Common.ToolState.OpenToAnnulus)
            {
                deActuatingFlowRate = CalculateDeActuatingFlowRate(actuatingFlowRateInGallonsPerMinute, gapNutInsideDiameterinInch, gapWidthInInch, valveInsertDiameterInInch, minimumSidePortAreaInInch2, maximumSidePortAreaInInch2);
                if (flowRateInGallonsPerMinute > deActuatingFlowRate)
                    finalState = Common.ToolState.OpenToAnnulus;
                else
                    finalState = Common.ToolState.CloseToAnnulus;
            }
            else
            {
                if (flowRateInGallonsPerMinute >= actuatingFlowRateInGallonsPerMinute)
                    finalState = Common.ToolState.OpenToAnnulus;
                else
                    finalState = Common.ToolState.CloseToAnnulus;
            }
           
            return finalState;
        }

        internal double CalculateOutPutFlowRateInGallonsPerMinute(double flowRateInGallonsPerMinute, double actuatingFlowRateInGallonsPerMinute, Common.ToolState currentState, double gapNutInsideDiameterinInch, double gapWidthInInch, double valveInsertDiameterInInch, double minimumSidePortAreaInInch2, double maximumSidePortAreaInInch2)
        {
            finalState = CalculateFinalState(flowRateInGallonsPerMinute, actuatingFlowRateInGallonsPerMinute, currentState, gapNutInsideDiameterinInch, gapWidthInInch, valveInsertDiameterInInch, minimumSidePortAreaInInch2, maximumSidePortAreaInInch2);
            if (finalState == Common.ToolState.OpenToAnnulus)
                return 0;
            else
                return flowRateInGallonsPerMinute;
        }

        private void CalculateSidePortArea(double gapNutInsideDiameterinInch, double gapWidthInInch, double minimumSidePortAreaInInch2, double maximumSidePortAreaInInch2)
        {
            if (sidePortArea == double.MinValue)
            {
                sidePortArea = (Math.PI * gapNutInsideDiameterinInch * gapWidthInInch) + minimumSidePortAreaInInch2;
                if (sidePortArea < minimumSidePortAreaInInch2)
                    sidePortArea = minimumSidePortAreaInInch2;
                else if (sidePortArea > maximumSidePortAreaInInch2)
                    sidePortArea = maximumSidePortAreaInInch2;
            }
        }
        private void CalculateValveInsertArea(double valveInsertDiameterInInch)
        {
            if (valveInsertArea == double.MinValue)
                valveInsertArea = Math.PI * valveInsertDiameterInInch * valveInsertDiameterInInch / 4;
        }

    }
}
