using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Hydraulic output of Type 6 tools (Sequencing valve)
    public interface IBHAToolType6HydraulicsOutput : IBHAHydraulicsOutput
    {
        //void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
        Common.ToolState FinalState { get; }
        double TotalPressureDropInPSI { get; }
        double ToolPressureDropInPSI { get; }
        double AnnulusOpeningPressureDropInPSI { get; }
        double BHAOpeningPressureDropInPSI { get; }
        double InputFlowrateInGPM { get; }
        double AnnulusOpeningFlowrateInGPM { get; }
        double BHAOpeningFlowrateInGPM { get; }
        double AnnulusNozzleVelocityInFeetPerSecond { get;  }
        double BHANozzleVelocityInFeetPerSecond { get;  }
    }

    // This class takes care of hydraulic calculations of all Type 6 tools (Sequencing valve)
    public class BHAToolType6 : BHATool, IBHAToolType6HydraulicsOutput, IBHASplitFlowTool
    {
        public IBHAToolType6HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType6HydraulicsOutput)this; }
        }

        #region Private Variables
        Common.ToolState initialSt;
        Common.ToolState finalSt;
        List<BHATool> bhaTools;
        private List<Nozzles> annulusInfo;
        private List<Nozzles> bhaInfo;
        private double lengthBeforeAnnulus = double.MinValue;
        private double lengthAfterAnnulus = double.MinValue;
        private double ID = double.MinValue;
        private double totalPressureDrop = double.MinValue;
        private double toolPressureDrop = double.MinValue;
        private double annulusOpeningPressureDrop = double.MinValue;
        private double bhaOpeningPressureDrop = double.MinValue;
        private double inputFlowrate;
        private double annulusFlowrate;
        private double bhaFlowrate;
        private Common.ToolState bhaState = Common.ToolState.OpenToAnnulus;
        private double annulusVelocity;
        private double bhaVelocity;
        protected double depth = double.MinValue;
        #endregion

        #region Properties

        public Common.ToolState CurrentState
        {
            get { return initialSt; }
            set { initialSt = value; }
        }

        //public double LengthInFeet
        //{
        //    get { return length; }
        //    set { length = value; }
        //}
        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public double LengthBeforeAnnulusOpeningInFeet
        {
            get { return lengthBeforeAnnulus; }
            set { lengthBeforeAnnulus = value; }
        }

        public double LengthAfterAnnulusOpeningInFeet
        {
            get { return lengthAfterAnnulus; }
            set { lengthAfterAnnulus = value; }
        }

        public double InsideDiameterInInches
        {
            get { return ID; }
            set { ID = value; }
        }

        Common.ToolState IBHAToolType6HydraulicsOutput.FinalState
        {
            get { return finalSt; }
        }

        double IBHAToolType6HydraulicsOutput.InputFlowrateInGPM
        {
            get { return inputFlowrate; }
        }
        double IBHAToolType6HydraulicsOutput.AnnulusOpeningFlowrateInGPM
        {
            get { return annulusFlowrate; }
        }
        double IBHAToolType6HydraulicsOutput.BHAOpeningFlowrateInGPM
        {
            get { return bhaFlowrate; }
        }

        double IBHAToolType6HydraulicsOutput.TotalPressureDropInPSI
        {
            get { return totalPressureDrop; }
        }
        double IBHAToolType6HydraulicsOutput.ToolPressureDropInPSI
        {
            get { return toolPressureDrop; }
        }

        double IBHAToolType6HydraulicsOutput.AnnulusOpeningPressureDropInPSI
        {
            get { return annulusOpeningPressureDrop; }
        }

        double IBHAToolType6HydraulicsOutput.BHAOpeningPressureDropInPSI
        {
            get { return bhaOpeningPressureDrop; }
        }
        double IBHAToolType6HydraulicsOutput.AnnulusNozzleVelocityInFeetPerSecond
        {
            get { return annulusVelocity; }
        }
        double IBHAToolType6HydraulicsOutput.BHANozzleVelocityInFeetPerSecond
        {
            get { return bhaVelocity; }
        }
        public List<BHATool> BHATools
        {
            get { return bhaTools; }
            set { bhaTools = value; }
        }
        public List<Nozzles> AnnulusNozzleInformation
        {
            get { return annulusInfo; }
                set { annulusInfo = value;  }
            }
        public List<Nozzles> BHANozzleInformation
        {
            get { return bhaInfo; }
            set { bhaInfo = value;  }
        }

        public Common.ToolState BHAOpeningState
        {
            get { return bhaState; }
            set { bhaState = value; }
        }
        
        #endregion


        public BHAToolType6() { }
        public BHAToolType6(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState, List<BHATool> bhaTools,List<Nozzles > bhaInfo,List<Nozzles> annulusInfo)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch =  outsideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.CurrentState = currentState;
            this.BHATools = bhaTools;
            this.BHANozzleInformation = bhaInfo;
            this.AnnulusNozzleInformation = annulusInfo;
            this.Depth = toolDepth;
        }

        
        public BHAToolType6(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState,  List<Nozzles> bhaInfo, List<Nozzles> annulausInfo)
        {
           this.PositionNumber = positionNumber;
           this.toolDescription = toolDescription;
           this.OutsideDiameterInInch = outsideDiameterInInch;
           this.LengthInFeet = lengthInFeet;
           this.CurrentState = currentState;
           
           this.BHANozzleInformation = bhaInfo;
           this.AnnulusNozzleInformation = annulausInfo;
            this.Depth = toolDepth;
        }

        public BHAToolType6(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, Common.ToolState currentState, List<Nozzles> bhaNozzleInfo, List<Nozzles> annulusNozzleInfo, double insideDiameterInInches, double lengthBeforeAnnulusOpeningInFeet, double lengthAfterAnnnulusOpeningInFeet, Common.ToolState bhaToolState = Common.ToolState.OpenToAnnulus)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch = outsideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.CurrentState = currentState;
            this.BHANozzleInformation = bhaNozzleInfo;
            this.AnnulusNozzleInformation = annulusNozzleInfo;
            this.InsideDiameterInInches = insideDiameterInInches;
            this.LengthAfterAnnulusOpeningInFeet = lengthAfterAnnnulusOpeningInFeet;
            this.LengthBeforeAnnulusOpeningInFeet = lengthBeforeAnnulusOpeningInFeet;
            this.BHAOpeningState = bhaToolState;
            this.Depth = toolDepth;

        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate = double.MinValue, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {
            SplitFlowPressureInformation pressureInfo = new SplitFlowPressureInformation();
            Calculations.Type6Calculations calc = new Calculations.Type6Calculations();
            
            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInches);
            this.BHAHydraulicsOutput.CriticalVelocityInFeetPerSecond = calc.CalculateCriticalVelocityInFeetPerSecond(fluid, this.InsideDiameterInInches);

            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.CurrentState,bhaTools,PositionNumber,torqueInFeetPound, BHANozzleInformation,AnnulusNozzleInformation, this.LengthInFeet, ID, lengthBeforeAnnulus, lengthAfterAnnulus, this.BHAOpeningState, segments);

            this.BHAHydraulicsOutput.FlowType = Common.TurbulentFlowType;
            this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.TotalPressureDropInPSI;
            this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute = pressureInfo.BHAFlowrateInGPM;
            
            totalPressureDrop = pressureInfo.TotalPressureDropInPSI;
            toolPressureDrop = pressureInfo.ToolPressureDropInPSI;
            inputFlowrate = pressureInfo.InputFlowrateInGPM;
            annulusOpeningPressureDrop = pressureInfo.AnnulusOpeningPressureDropInPSI;
            annulusFlowrate = pressureInfo.AnnulusFlowrateInGPM;
            bhaOpeningPressureDrop = pressureInfo.BHAOpeningPressureDropInPSI;
            bhaFlowrate = pressureInfo.BHAFlowrateInGPM;
            annulusVelocity = calc.CalculateNozzleVelocityInFeetPerSecond(fluid, annulusFlowrate, AnnulusNozzleInformation);
            bhaVelocity = calc.CalculateNozzleVelocityInFeetPerSecond(fluid, bhaFlowrate, BHANozzleInformation);
            this.BHAHydraulicsOutput.EquivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, pressureInfo.TotalPressureDropInPSI, this.depth);
        }

        public double GetToolPressureLoss(Fluid fluid, double inputFlowrateInGPM, double outputFlowrateInGPM)
        {
            //Calculations.Type6Calculations calc = new Calculations.Type6Calculations();
            //return calc.GetPressureDrop (fluid, inputFlowrateInGPM, outputFlowrateInGPM,ID,bhaInfo,this.LengthInFeet, lengthBeforeAnnulus, lengthAfterAnnulus, this.BHAOpeningState);
            Calculations.Type6Calculations calc = new Calculations.Type6Calculations();
            return calc.GetBHAOpeningPressureDrop(fluid, outputFlowrateInGPM, ID, bhaInfo, lengthAfterAnnulus, this.BHAOpeningState);
        }

        public double GetBHAOpeningPressureLoss(Fluid fluid, double outputFlowrateInGPM)
        {
            Calculations.Type6Calculations calc = new Calculations.Type6Calculations();
            return calc.GetBHAOpeningPressureDrop(fluid,  outputFlowrateInGPM, ID, bhaInfo, lengthAfterAnnulus, this.BHAOpeningState);
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
