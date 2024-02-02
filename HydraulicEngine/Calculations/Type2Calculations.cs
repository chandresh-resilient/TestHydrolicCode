using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace HydraulicEngine.Calculations
{
    internal class Type2Calculations
    {
       


        internal double  GetFreeRunningLosses ( Common.BHAType2ModelName modelName, double flowRateInGPM)
        {
            if (flowRateInGPM <= 0)
                return 0;
            List<FreeRunningLoss> FreeRunningLossData = Motor.GetFreeRunningLossesData(modelName);
            
                FreeRunningLoss lowerFlow = FreeRunningLossData.Where(FreeRunningLoss => FreeRunningLoss.FlowRateInGPM <= flowRateInGPM).LastOrDefault();
                FreeRunningLoss upperFlow = FreeRunningLossData.Where(FreeRunningLoss => FreeRunningLoss.FlowRateInGPM >= flowRateInGPM).FirstOrDefault();
                if (lowerFlow == null && upperFlow == null)
                    return 0;
                if (lowerFlow == upperFlow)
                    return lowerFlow.PressureLossInPSI;
                if (lowerFlow != null && upperFlow != null)
                {
                    double flowRatio = (flowRateInGPM - lowerFlow.FlowRateInGPM) / (upperFlow.FlowRateInGPM - lowerFlow.FlowRateInGPM);
                    return lowerFlow.PressureLossInPSI + flowRatio * (upperFlow.PressureLossInPSI - lowerFlow.PressureLossInPSI);
                }
                if (lowerFlow == null)
                {
                    lowerFlow = upperFlow;
                    upperFlow = FreeRunningLossData.Where(FreeRunningLoss => FreeRunningLoss.FlowRateInGPM > lowerFlow.FlowRateInGPM).FirstOrDefault();
                    double pressurePerGPM = (upperFlow.PressureLossInPSI - lowerFlow.PressureLossInPSI) / (upperFlow.FlowRateInGPM - lowerFlow.FlowRateInGPM);
                    return lowerFlow.PressureLossInPSI - (pressurePerGPM * (lowerFlow.FlowRateInGPM - flowRateInGPM));
                }
                if (upperFlow == null)
                {
                    upperFlow = lowerFlow;
                    lowerFlow = FreeRunningLossData.Where(FreeRunningLoss => FreeRunningLoss.FlowRateInGPM < upperFlow.FlowRateInGPM).LastOrDefault();
                    if (lowerFlow != null)
                    {
                        double pressurePerGPM = (upperFlow.PressureLossInPSI - lowerFlow.PressureLossInPSI) / (upperFlow.FlowRateInGPM - lowerFlow.FlowRateInGPM);
                        return upperFlow.PressureLossInPSI + (pressurePerGPM * (flowRateInGPM - upperFlow.FlowRateInGPM));
                    }
                    else
                        return 0;
                }
            return 0;
        }

        internal double GetTorqueGeneratingPressureLosses(Common.BHAType2ModelName modelName, double torqueInFeetPounds, double flowRateInGPM)
        {
            if (torqueInFeetPounds == 0)
                return 0;
            if (flowRateInGPM <= 0)
                return 0;
            List<TorqueData> torqueData = Motor.GetTorqueData(modelName);
           
                TorqueData lowerTorque = torqueData.Where(TorqueData => TorqueData.TorqueInFeetPounds <= torqueInFeetPounds).LastOrDefault();
                TorqueData upperTorque = torqueData.Where(TorqueData => TorqueData.TorqueInFeetPounds >= torqueInFeetPounds).FirstOrDefault();
                if (lowerTorque == null && upperTorque == null)
                    return 0;
                if (lowerTorque == upperTorque)
                    return lowerTorque.PressureLossInPSI;
                if (lowerTorque != null && upperTorque != null)
                {
                    if (upperTorque.TorqueInFeetPounds - lowerTorque.TorqueInFeetPounds != 0)
                    {
                        double torqueRatio = (torqueInFeetPounds - lowerTorque.TorqueInFeetPounds) / (upperTorque.TorqueInFeetPounds - lowerTorque.TorqueInFeetPounds);
                        return lowerTorque.PressureLossInPSI + torqueRatio * (upperTorque.PressureLossInPSI - lowerTorque.PressureLossInPSI);
                    }
                    else
                        return 0;
                }
                if (lowerTorque == null)
                {
                    lowerTorque = upperTorque;
                    upperTorque = torqueData.Where(TorqueData => TorqueData.TorqueInFeetPounds > lowerTorque.TorqueInFeetPounds).FirstOrDefault();
                    double pressurePerGPM = (upperTorque.PressureLossInPSI - lowerTorque.PressureLossInPSI) / (upperTorque.TorqueInFeetPounds - lowerTorque.TorqueInFeetPounds);
                    return lowerTorque.PressureLossInPSI - (pressurePerGPM * (lowerTorque.TorqueInFeetPounds - torqueInFeetPounds));
                }
                if (upperTorque == null)
                {
                    upperTorque = lowerTorque;
                    lowerTorque = torqueData.Where(TorqueData => TorqueData.TorqueInFeetPounds < upperTorque.TorqueInFeetPounds).LastOrDefault();
                    if (lowerTorque != null)
                    {
                        double pressurePerGPM = (upperTorque.PressureLossInPSI - lowerTorque.PressureLossInPSI) / (upperTorque.TorqueInFeetPounds - lowerTorque.TorqueInFeetPounds);
                        return upperTorque.PressureLossInPSI + (pressurePerGPM * (torqueInFeetPounds - upperTorque.TorqueInFeetPounds));
                    }
                    else
                        return 0;
                }
           
            return 0;
        }

        
    }
}
