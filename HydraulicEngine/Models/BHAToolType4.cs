using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Hydraulic output of Type 4 tools (Sequencing valve)
    public interface IBHAToolType4HydraulicsOutput : IBHAHydraulicsOutput
    {
        //void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
        Common.ToolState FinalState { get; }
        double DeActuatingFlowRateInGallonsPerMinute { get;  }

        //double OutputFlowInGallonsPerMinute { get; }
    }

    // This class takes care of hydraulic calculations of all Type 4 tools (Sequencing valve)
    public class BHAToolType4 : BHATool, IBHAToolType4HydraulicsOutput
    {
        public IBHAToolType4HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType4HydraulicsOutput)this; }
        }

        #region Private Variables
        double valveInsertDiameter;
        double minimumSidePortArea;
        double maximumSidePortArea;
        double gapNutInsideDiameter;
        double gapWidth;
        double outputFlowRate;
        double actuatingFlowRate;
        double deActuatingFlowRate = double.MinValue;
        Common.ToolState initialSt;
        Common.ToolState finalSt;
        #endregion

        #region Properties

        // since this is a state driven tool this property is set to true
        public override bool HasVariableFlow
        {
            get { return true; }
        }
        public double ValveInsertDiameterInInch
        {
            get { return valveInsertDiameter; }
            set { valveInsertDiameter = value; }
        }

        public double MinimumSidePortAreaInInch2
        {
            get { return minimumSidePortArea; }
            set { minimumSidePortArea = value; }
        }

        public double MaximumSidePortAreaInInch2
        {
            get { return maximumSidePortArea; }
            set { maximumSidePortArea = value; }
        }

        public double GapNutInsideDiameterInInch
        {
            get { return gapNutInsideDiameter; }
            set { gapNutInsideDiameter = value; }
        }

        public double GapWidthInInch
        {
            get { return gapWidth; }
            set { gapWidth = value; }
        }

        public double ActuatingFlowRateInGallonsPerMinute
        {
            get { return actuatingFlowRate; }
            set { actuatingFlowRate = value; }
        }

        public Common.ToolState CurrentState
        {
            get { return initialSt; }
            set { initialSt = value; }
        }

        Common.ToolState IBHAToolType4HydraulicsOutput.FinalState
        {
            get { return finalSt; }
        }

        double IBHAToolType4HydraulicsOutput.DeActuatingFlowRateInGallonsPerMinute
        {
            get 
            {
                if (deActuatingFlowRate == double.MinValue)
                {
                    Calculations.Type4Calculations calc = new Calculations.Type4Calculations();
                    deActuatingFlowRate = calc.CalculateDeActuatingFlowRate(this.ActuatingFlowRateInGallonsPerMinute, this.GapNutInsideDiameterInInch, this.GapWidthInInch, this.ValveInsertDiameterInInch, this.MinimumSidePortAreaInInch2, this.MaximumSidePortAreaInInch2);
                }
                return deActuatingFlowRate; 
            }
        }
        //double IBHAToolType4HydraulicsOutput.OutputFlowInGallonsPerMinute
        //{
        //    get { return outputFlowRate; }
        //}

        
        #endregion
    

        public BHAToolType4() { }
        public BHAToolType4(int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState, double actuationFlowrateInGPA, double valveInsideDiameterinInch, double minimumSidePortAreaInInch2, double maximumPortAreaInInch2, double gapNutInsideDiameterInInch, double gapWidthInInch)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch =  outsideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.CurrentState = currentState;
            this.ActuatingFlowRateInGallonsPerMinute = actuationFlowrateInGPA;
            this.ValveInsertDiameterInInch = valveInsideDiameterinInch;
            this.MinimumSidePortAreaInInch2 = minimumSidePortAreaInInch2;
            this.MaximumSidePortAreaInInch2 = maximumPortAreaInInch2;
            this.GapNutInsideDiameterInInch = gapNutInsideDiameterInInch;
            this.GapWidthInInch = gapWidthInInch;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate , double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {
            
            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.Type4Calculations calc = new Calculations.Type4Calculations();
            
            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate);
            //deActuatingFlowRate = calc.CalculateDeActuatingFlowRate(this.ActuatingFlowRateInGallonsPerMinute, this.GapNutInsideDiameterInInch, this.GapWidthInInch, this.ValveInsertDiameterInInch, this.MinimumSidePortAreaInInch2, this.MaximumSidePortAreaInInch2);
   
            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.ActuatingFlowRateInGallonsPerMinute, this.CurrentState, this.GapNutInsideDiameterInInch, this.GapWidthInInch,this.ValveInsertDiameterInInch, this.MinimumSidePortAreaInInch2, this.MaximumSidePortAreaInInch2);
            this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
            this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
            finalSt = calc.CalculateFinalState(flowRate, this.ActuatingFlowRateInGallonsPerMinute, this.CurrentState, this.GapNutInsideDiameterInInch, this.GapWidthInInch, this.ValveInsertDiameterInInch, this.MinimumSidePortAreaInInch2, this.MaximumSidePortAreaInInch2);
            this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute = calc.CalculateOutPutFlowRateInGallonsPerMinute(flowRate, this.ActuatingFlowRateInGallonsPerMinute, this.CurrentState, this.GapNutInsideDiameterInInch, this.GapWidthInInch, this.ValveInsertDiameterInInch, this.MinimumSidePortAreaInInch2, this.MaximumSidePortAreaInInch2);
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
