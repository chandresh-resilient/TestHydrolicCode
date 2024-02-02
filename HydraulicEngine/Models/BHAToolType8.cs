using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public interface IBHAToolType8HydraulicsOutput : IBHAHydraulicsOutput
    {

    }

    // no fluid passes through type 8 tools so all hydraulic outputs are 0
    public class BHAToolType8 : BHATool, IBHAToolType8HydraulicsOutput
    {

        public IBHAToolType8HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType8HydraulicsOutput)this; }
        }

        #region Private Variables

        protected double iD;
        protected double depth = double.MinValue;
        #endregion

        #region Properties

        public double InsideDiameterInInch
        {
            get { return iD; }
            set { iD = value; }
        }

        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }


        #endregion

        public BHAToolType8() { }
        public BHAToolType8(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, double insideDiameterInInch = 0)
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
            Calculations.Type8Calculations calc = new Calculations.Type8Calculations();
            UpdateAnnulusBelowCurrentToolForZeroFlow(fluid, PositionNumber, segments);
           // no fluid passes through type 8 tools so all hydraulic outputs are 0
           this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = 0;
           this.BHAHydraulicsOutput.FlowType = "None";
           this.BHAHydraulicsOutput.PressureDropInPSI = 0;
           this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute = 0;
           this.BHAHydraulicsOutput.EquivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, this.BHAHydraulicsOutput.PressureDropInPSI, this.Depth);
        }

        private void UpdateAnnulusBelowCurrentToolForZeroFlow(Fluid fluid, int positionNumber, List<Segment> segments = null)
        {
            if (segments != null)
            {
                IEnumerable<Segment> selected = segments.Where(x => x.ToolPositionNumber >= positionNumber);
                foreach (Segment seg in selected)
                {
                    seg.UpdateHydraulicsWithZeroFlow();//Considering zero flow rate for all the annulus segment below the type 8 tool.
                }
            }
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
