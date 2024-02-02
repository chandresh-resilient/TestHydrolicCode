using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public class SplitFlowPressureInformation
    {
        private double inputFlowrate;
        private double annulusFlowrate;
        private double bhaFlowrate;
        private double totalPressureDrop;
        private double toolPressureDrop;
        private double annulusOpeningPressureDrop;
        private double bhaOpeningPressureDrop;
        private Common.ToolState finalState;
        private double annulusNozzleVelocity;
        private double bhaNozzleVelocity;

        internal double InputFlowrateInGPM
        {
            get { return inputFlowrate; }
            set { inputFlowrate = value; }
        }
        internal double AnnulusFlowrateInGPM
        {
            get { return annulusFlowrate; }
            set { annulusFlowrate = value; }
        }
        internal double BHAFlowrateInGPM
        {
            get { return bhaFlowrate; }
            set { bhaFlowrate = value; }
        }
        internal double TotalPressureDropInPSI
        {
            get { return totalPressureDrop; }
            set { totalPressureDrop = value; }
        }
        internal double ToolPressureDropInPSI
        {
            get { return toolPressureDrop; }
            set { toolPressureDrop = value; }
        }
        internal double AnnulusOpeningPressureDropInPSI
        {
            get { return annulusOpeningPressureDrop; }
            set { annulusOpeningPressureDrop = value; }
        }
        internal double BHAOpeningPressureDropInPSI
        {
            get { return bhaOpeningPressureDrop; }
            set { bhaOpeningPressureDrop = value; }
        }
        internal Common.ToolState FinalState
        {
            get { return finalState; }
            set { finalState = value; }
        }
        internal double AnnulusNozzleVelocityInFeetPerSecond
        {
            get { return annulusNozzleVelocity; }
            set { annulusNozzleVelocity = value; }
        }
        internal double BHANozzleVelocityInFeetPerSecond
        {
            get { return bhaNozzleVelocity; }
            set { bhaNozzleVelocity = value; }
        }
    }
    public interface IBHASplitFlowTool
    {
        double GetToolPressureLoss(Fluid fluid, double inputFlowrateInGPM, double outputFlowRateInGPM);
    }
}
