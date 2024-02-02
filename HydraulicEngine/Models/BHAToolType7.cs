using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public interface IBHAToolType7HydraulicsOutput : IBHAHydraulicsOutput
    {

    }

    // no fluid passes through type 8 tools so all hydraulic outputs are 0
    public class BHAToolType7 : BHATool, IBHAToolType7HydraulicsOutput
    {

        public IBHAToolType7HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType7HydraulicsOutput)this; }
        }

        #region Private Variables

        protected double pressureDrop;
        
        #endregion

        #region Properties

        public double PressureDropInPSI
        {
            get { return pressureDrop; }
            set { pressureDrop = value; }
        }

        

        #endregion

        public BHAToolType7() { }
        public BHAToolType7(int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double pressureDropInPSI)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch =  outsideDiameterInInch;
            this.PressureDropInPSI = pressureDropInPSI;
            this.LengthInFeet = lengthInFeet;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate , double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
       {
           
           // Pressure Drop is entered by user
           this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = 0;
           this.BHAHydraulicsOutput.FlowType = "None";
           this.BHAHydraulicsOutput.PressureDropInPSI = this.PressureDropInPSI;
           this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute = double.MinValue;
       }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }

    }
}
