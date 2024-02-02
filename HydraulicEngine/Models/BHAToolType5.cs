using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Hydraulic output of Type 5 tools (Sequencing valve)
    public interface IBHAToolType5HydraulicsOutput : IBHAHydraulicsOutput
    {
        //void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
        double TotalPressureDropInPSI { get; }
        double ToolPressureDropInPSI { get; }
        double AnnulusOpeningPressureDropInPSI { get; }
        double BHAOpeningPressureDropInPSI { get; }
        double InputFlowrateInGPM { get; }
        double AnnulusOpeningFlowrateInGPM { get; }
        double BHAOpeningFlowrateInGPM { get; }
    }

    // This class takes care of hydraulic calculations of all Type 4 tools (Sequencing valve)
    public class BHAToolType5 : BHATool, IBHAToolType5HydraulicsOutput, IBHASplitFlowTool
    {
        public IBHAToolType5HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType5HydraulicsOutput)this; }
        }

         #region Private Variables
            private List<Nozzles> annulusNozzleInfo;
            private List<Nozzles> bhaNozzleInfo;
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
        #endregion

        #region Properties

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

       
        double IBHAToolType5HydraulicsOutput.InputFlowrateInGPM
        {
            get { return inputFlowrate; }
        }
        double IBHAToolType5HydraulicsOutput.AnnulusOpeningFlowrateInGPM
        {
            get { return annulusFlowrate; }
        }
        double IBHAToolType5HydraulicsOutput.BHAOpeningFlowrateInGPM
        {
            get { return bhaFlowrate; }
        }

        double IBHAToolType5HydraulicsOutput.TotalPressureDropInPSI
        {
            get { return totalPressureDrop; }
        }
        double IBHAToolType5HydraulicsOutput.ToolPressureDropInPSI
        {
            get { return toolPressureDrop; }
        }

        double IBHAToolType5HydraulicsOutput.AnnulusOpeningPressureDropInPSI
        {
            get { return annulusOpeningPressureDrop; }
        }

        double IBHAToolType5HydraulicsOutput.BHAOpeningPressureDropInPSI
        {
            get { return bhaOpeningPressureDrop; }
        }
        public List<Nozzles> AnnulusNozzleInformation
        {
            get { return annulusNozzleInfo; }
            set { annulusNozzleInfo = value; }
        }
        public List<Nozzles> BHANozzleInformation
        {
            get { return bhaNozzleInfo; }
            set { bhaNozzleInfo = value; }
        }

        #endregion
        public BHAToolType5() { }
        public BHAToolType5(int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> annulusNozzles, double insideDiameterInInch = 0)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch = outsideDiameterInInch;
            this.InsideDiameterInInches = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            //Get the BHA Info
            this.AnnulusNozzleInformation = annulusNozzles;
            //this.BHATools = bhaTools;


        }
       


        public override void CalculateHydraulics(Fluid fluid, double flowRate = double.MinValue, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {
            
            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.Type5Calculations calc = new Calculations.Type5Calculations();
            double bhaFlowRate = 0;
            double annulusFlowRate = 0;
            

            bhaFlowRate = Calculations.SplitFLowCalculations.CalculateFlowRateInGPM(fluid, flowRate, bhaTools, PositionNumber, annulusNozzleInfo, torqueInFeetPound,0,0, segments);//Send BHA Info
            annulusFlowRate = flowRate - bhaFlowRate;

            this.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute=bhaFlowRate;
            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(annulusFlowRate, this.InsideDiameterInInches);
            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, bhaFlowRate, this.InsideDiameterInInches, this.LengthInFeet);
            this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
            this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
            
        }

        public double GetToolPressureLoss(Fluid fluid, double inputFlowrateInGPM, double outputFlowRateInGPM)
        {
           
            Calculations.Type5Calculations calc = new Calculations.Type5Calculations();
            return calc.CalculateTotalPressureDropInPSI(fluid, outputFlowRateInGPM, this.InsideDiameterInInches, this.LengthInFeet).PressureDropInPSI;
         
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
