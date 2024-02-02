using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // A seprate interface is made to define output properties. This is done to keep it consistent with BHA  tool definition
    public interface ISegmentHydraulicsOutput
    {
        string FlowType { get;  }
        double PressureDropInPSI { get;  }
        double AverageVelocityInFeetPerMinute { get;  }

        double CriticalVelocityInFeetPerMinute { get;  }

        // This property by default will return double.MinValue. 
        //But the tool types which have either split flow or various states need to implement this property accordingly
        double ChipRateInFeetPerMinute { get; }

        Common.ResultType ChipRateRange { get; }

        double EquivalentCirculatingDensity { get; set; }

    }

    public class Segment : ISegmentHydraulicsOutput
    {
        // type cast the current class as Hydraulic output class so that user can browse through output properties
        public ISegmentHydraulicsOutput SegmentHydraulicsOutput
        {
            get { return (ISegmentHydraulicsOutput)this; }
        }


        #region Private Variables
        private int posNum;
        private double toolOD;
        private double annulusID;
        private double segmentTop = double.MinValue;
        private double segmentBottom = double.MinValue;
        private string wellboreSectionName;
        private string toolDesc;
        protected double averageVelocity = double.MinValue;
        protected double criticalVelocity = double.MinValue;
        protected double chipRate = double.MinValue;
        protected double pressureDrop = double.MinValue;
        protected string flowType;
        protected Common.ResultType chipRateResult;
        private Cuttings cuttingsObjectForReuseInCalculations;
        protected double equivalentCirculatingDensity = double.MinValue;
        protected double depth = double.MinValue;
        #endregion

        #region Properties

        public int PositionNumber
        {
            get { return posNum; }
            set { posNum = value; }
        }

        public double ToolODInInch {
            get{ return toolOD;}
            set{toolOD = value;}
        }

        public double AnnulusIDInInch
        {
            get{return annulusID;}
            set{annulusID = value;}
        }

        public double SegmentTopInFeet
        {
            get{return segmentTop;}
            set{segmentTop = value;}
        }

        public double SegmentBottomInFeet
        {
            get{return segmentBottom;}
            set{segmentBottom = value;}
        }

        public string WellboreSectionName
        {
            get{return wellboreSectionName;}
            set{wellboreSectionName = value;}
        }

        public string ToolDescription
        {
            get { return toolDesc; }
            set { toolDesc = value; }
        }

        public int ToolPositionNumber
        {
            get;
            set;
        }

        public double SegmentLengthInFeet
        {
            get
            {
                if (segmentTop != double.MinValue && segmentBottom!= double.MinValue)
                    return segmentBottom - segmentTop;
                else
                    return 0;
            }
  
        }

        double ISegmentHydraulicsOutput.AverageVelocityInFeetPerMinute
        {
            get { return averageVelocity; }
            
        }

        double ISegmentHydraulicsOutput.CriticalVelocityInFeetPerMinute
        {
            get { return criticalVelocity; }
         
        }

        double ISegmentHydraulicsOutput.ChipRateInFeetPerMinute
        {
            get { return chipRate; }
         
        }

        double ISegmentHydraulicsOutput.PressureDropInPSI
        {
            get { return pressureDrop; }
         
        }

        string ISegmentHydraulicsOutput.FlowType
        {
            get { return flowType; }
         
        }

        Common.ResultType ISegmentHydraulicsOutput.ChipRateRange
        {
            get { return chipRateResult; }
        }

        double ISegmentHydraulicsOutput.EquivalentCirculatingDensity
        {
            get { return equivalentCirculatingDensity; }
            set { equivalentCirculatingDensity = value; }
        }

        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        #endregion

        #region Constructor

        public Segment() { }

        public Segment (int positionNumber, string sectionName, string toolDescription, double annulusIDInInch, double toolODInInch, double topInFeet, double bottomInFeet)
        {
            posNum = positionNumber;
            wellboreSectionName = sectionName;
            toolDesc = toolDescription;
            annulusID = annulusIDInInch;
            toolOD = toolODInInch;
            segmentTop = topInFeet ;
            segmentBottom = bottomInFeet;
        }

        #endregion

        public virtual void CalculateHydraulics(Fluid fluid, double flowRateInGPM, Cuttings cuttings, double toolDepth)
        {
            Calculations.SegmentCalculations calc = new Calculations.SegmentCalculations();
            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.ChipRateInformation chipRateInfo = new Calculations.ChipRateInformation();
            //Depth = toolDepth;
            
                //cuttings = new Cuttings(Common.CuttingType.Rock, 0);
            averageVelocity = calc.CalculateAverageVelocityInFeetPerMinute(flowRateInGPM, annulusID,toolOD);
            criticalVelocity = calc.CalculateCriticalVelocityInFeetPerMinute (fluid, annulusID, toolOD);
            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid,flowRateInGPM, annulusID, toolOD, SegmentLengthInFeet);
            pressureDrop = pressureInfo.PressureDropInPSI;
            flowType = pressureInfo.FlowType;
            equivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, pressureDrop, Depth);
            if (cuttings != null)
            {
                chipRateInfo = calc.CalculateChipRateInFeetPerInch(fluid, flowRateInGPM, cuttings, annulusID, toolOD);
                chipRate = chipRateInfo.ChipRateInFeetPerMinute;
                chipRateResult = chipRateInfo.ResultType;
            }
            else
            {
                chipRate = double.MinValue;
                chipRateResult = Common.ResultType.Good;
            }
            this.cuttingsObjectForReuseInCalculations = cuttings;
        }

        public virtual void ReCalculateHydraulics(Fluid fluid, double flowRateInGPM)
        {
            CalculateHydraulics(fluid, flowRateInGPM, this.cuttingsObjectForReuseInCalculations, this.Depth);
        }

        public virtual void UpdateHydraulicsWithZeroFlow()
        {
            averageVelocity = 0;
            criticalVelocity = double.MinValue;
            pressureDrop = 0;
            flowType = "None";
            chipRate = 0;
            chipRateResult = Common.ResultType.Good;
        }

    }

}
