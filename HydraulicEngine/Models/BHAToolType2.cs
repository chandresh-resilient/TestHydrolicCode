using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public interface IBHAToolType2HydraulicsOutput : IBHAHydraulicsOutput
    {
        double FreeRunningLossesInPSI { get; set; }
        double TorqueGeneratingPressureInPSI { get; set; }
    }
    public class BHAToolType2 : BHATool, IBHAToolType2HydraulicsOutput
    {
        public IBHAToolType2HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType2HydraulicsOutput)this; }
        }

        #region Private Variables

        protected Common.BHAType2ModelName mdlName;

        #endregion

        #region Properties

        public Common.BHAType2ModelName ModelName
        {
            get { return mdlName; }
            set { mdlName = value; }
        }





        #endregion
        public BHAToolType2() { }
        public BHAToolType2(int positionNumber, string toolDescription, Common.BHAType2ModelName modelName,double length,double outerDiameter)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.ModelName = modelName;
            this.LengthInFeet = length;
            this.OutsideDiameterInInch = outerDiameter;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {
            
            Calculations.Type2Calculations calc = new Calculations.Type2Calculations();
            

            this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute = flowRate;
            this.BHAHydraulicsOutput.FlowType = Common.TurbulentFlowType;
            this.BHAHydraulicsOutput.FreeRunningLossesInPSI = calc.GetFreeRunningLosses(mdlName, flowRate);
            this.BHAHydraulicsOutput.TorqueGeneratingPressureInPSI = calc.GetTorqueGeneratingPressureLosses(mdlName, torqueInFeetPound,flowRate);
            this.BHAHydraulicsOutput.PressureDropInPSI = this.BHAHydraulicsOutput.FreeRunningLossesInPSI + this.BHAHydraulicsOutput.TorqueGeneratingPressureInPSI;
        }

        public double FreeRunningLossesInPSI
        {
            get;
            set;
        }

        public double TorqueGeneratingPressureInPSI
        {
            get;
            set;
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
