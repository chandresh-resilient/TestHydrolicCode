using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Type 1 tools output interface. It does not declare any propertiess but it is defined for consistancy sake
    public interface IBHAToolType1HydraulicsOutput : IBHAHydraulicsOutput
    {
      
    }

    // Class for all standard tools for which hydraulic calculations are done with OD, ID & Length dimensions
    public class BHAToolType1 : BHATool, IBHAToolType1HydraulicsOutput
    {
        public IBHAToolType1HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType1HydraulicsOutput)this; }
        }
        
        #region Private Variables
        
        protected double iD;
        protected double criticalVelocity = double.MinValue;
        protected double depth = double.MinValue;
        #endregion

        #region Properties

        public double InsideDiameterInInch
        {
            get{return iD;}
            set{iD = value;}
        }

        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }



        #endregion
        public BHAToolType1() { }
        public BHAToolType1(int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch, double toolDepth)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch =  outsideDiameterInInch;
            this.InsideDiameterInInch = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.Depth = toolDepth;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
       {
           
           Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
           Calculations.Type1Calculations calc = new Calculations.Type1Calculations();

           this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInch);
           pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.InsideDiameterInInch, this.LengthInFeet);
           this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
           this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
           this.BHAHydraulicsOutput.CriticalVelocityInFeetPerSecond = calc.CalculateCriticalVelocityInFeetPerSecond(fluid, this.InsideDiameterInInch);
           this.BHAHydraulicsOutput.EquivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, pressureInfo.PressureDropInPSI, this.Depth);
       }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
