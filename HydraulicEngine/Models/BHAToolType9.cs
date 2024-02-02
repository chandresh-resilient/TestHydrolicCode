using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Type 9 tools as similar to type 1 tools except that we do know the pressure drop at a flow rate
    // so we need to calculate the correction factor and use that in the calculations
    public interface IBHAToolType9HydraulicsOutput : IBHAHydraulicsOutput
    {

    }
    public class BHAToolType9 : BHATool, IBHAToolType9HydraulicsOutput
    {
         public IBHAToolType9HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType9HydraulicsOutput)this; }
        }

        #region Private Variables

        protected double iD;
        protected double observedFlowRate;
        protected double observedPressureDrop;
        #endregion

        #region Properties

        public double InsideDiameterInInch
        {
            get { return iD; }
            set { iD = value; }
        }

        public double ObservedFlowRateInGallonsPerMinute
        {
            get { return observedFlowRate; }
            set { observedFlowRate = value; }
        }

        public double ObservedPressureDropInPSI
        {
            get { return observedPressureDrop; }
            set { observedPressureDrop = value; }
        }
        

        #endregion

        public BHAToolType9() { }
        public BHAToolType9(int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch,double observedFlowRateInGallonsPerMinute, double observedPressureDropInPSI)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch =  outsideDiameterInInch;
            this.InsideDiameterInInch = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.ObservedFlowRateInGallonsPerMinute = observedFlowRateInGallonsPerMinute;
            this.ObservedPressureDropInPSI = observedPressureDropInPSI;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate , double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
       {
           
           Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
           Calculations.Type9Calculations calc = new Calculations.Type9Calculations();
          
           this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInch);
           pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.InsideDiameterInInch, this.LengthInFeet, observedFlowRate, observedPressureDrop);
           this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
           this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
       }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
