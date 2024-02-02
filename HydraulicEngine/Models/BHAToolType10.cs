using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Output porpoerty interface for mills and bits (Type 3 tools)
    public interface IBHAToolType10HydraulicsOutput : IBHAHydraulicsOutput
    {
        //void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
        double NozzlePressureDropInPSI { get; set; }

        double AccusetPressureDropInPSI { get; set; }
        double HydraulicHorsePower { get; set; }

        double ImpactForceInPounds { get; set; }
        double NozzleVelocityInFeetPerSecond { get; set; }

    }


    // This class takes care of hydraulic calculations of all type 3 tools i.e. mills and bits
    public class BHAToolType10 : BHATool, IBHAToolType10HydraulicsOutput
    {
        public IBHAToolType10HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType10HydraulicsOutput)this; }
        }

        #region Private Variables
        protected double iD = 0;
        protected double hydraulicHP;
        protected double nozzleVelocity;
        protected double impactForce;
        protected double nozzlePressureDrop;
        protected double accusetPressureDrop = double.MinValue;
        protected List<Nozzles> nozz;
        protected Accuset toolAccu;
        protected double depth = double.MinValue;
        #endregion

        #region Properties

        public double InsideDiameterInInch
        {
            get { return iD; }
            set { iD = value; }
        }


        public List<Nozzles> NozzlesInfomation
        {
            get { return nozz; }
            set { nozz = value; }
        }

        public Accuset ToolAccuset
        {
            get { return toolAccu; }
            set { toolAccu = value; }
        }
        double IBHAToolType10HydraulicsOutput.HydraulicHorsePower
        {
            get { return hydraulicHP; }
            set { hydraulicHP = value; }
        }

        double IBHAToolType10HydraulicsOutput.ImpactForceInPounds
        {
            get { return impactForce; }
            set { impactForce = value; }
        }


        double IBHAToolType10HydraulicsOutput.AccusetPressureDropInPSI
        {
            get { return accusetPressureDrop; }
            set { accusetPressureDrop = value; }
        }

        double IBHAToolType10HydraulicsOutput.NozzlePressureDropInPSI
        {
            get { return nozzlePressureDrop; }
            set { nozzlePressureDrop = value; }
        }

        double IBHAToolType10HydraulicsOutput.NozzleVelocityInFeetPerSecond
        {
            get { return nozzleVelocity; }
            set { nozzleVelocity = value; }
        }

        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        #endregion
        public BHAToolType10() { }
        public BHAToolType10(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> nozzles, double insideDiameterInInch = 0)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch = outsideDiameterInInch;
            this.InsideDiameterInInch = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.NozzlesInfomation = nozzles;

        }
        public BHAToolType10(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> nozzles, double insideDiameterInInch = 0, Accuset toolAccuset=null)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch = outsideDiameterInInch;
            this.InsideDiameterInInch = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.NozzlesInfomation = nozzles;
            this.ToolAccuset = toolAccuset;
            this.Depth = toolDepth;
        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {

            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.Type3Calculations calc = new Calculations.Type3Calculations();

            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInch);
            this.BHAHydraulicsOutput.CriticalVelocityInFeetPerSecond = calc.CalculateCriticalVelocityInFeetPerSecond(fluid, this.InsideDiameterInInch);
            if (ToolAccuset != null)
            {
                this.accusetPressureDrop = CalculateAccusetPressureLoss(fluid, flowRate);
                this.BHAHydraulicsOutput.PressureDropInPSI = this.accusetPressureDrop;
            }
            else
            {
                pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.NozzlesInfomation, this.InsideDiameterInInch, this.LengthInFeet);
                this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
                this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
                this.BHAHydraulicsOutput.HydraulicHorsePower = calc.CalculateHydraulicHorsePower(fluid, flowRate, this.NozzlesInfomation);
                this.BHAHydraulicsOutput.ImpactForceInPounds = calc.CalculateImpactForceInPounds(fluid, flowRate, this.NozzlesInfomation);
                this.BHAHydraulicsOutput.NozzlePressureDropInPSI = calc.CalculateNozzlePressureDropInPSI(fluid, flowRate, this.NozzlesInfomation);
                this.BHAHydraulicsOutput.NozzleVelocityInFeetPerSecond = calc.CalculateNozzleVelocityInFeetPerSecond(fluid, flowRate, this.NozzlesInfomation);
            }

            this.BHAHydraulicsOutput.EquivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, BHAHydraulicsOutput.PressureDropInPSI, this.Depth);
        }

        private double CalculateAccusetPressureLoss( Fluid fluid, double flowRate)
        {
            double beta = toolAccu.StandardNozzleSize / toolAccu.BoreIdInInches;
            double nozzArea = Math.PI * Math.Pow(toolAccu.StandardNozzleSize, 2) / 4;
            double returnValue = ((Math.Pow(flowRate, 2) * fluid.DensityInPoundPerGallon * (1 - Math.Pow(beta, 4))) / (12032 * Math.Pow(toolAccu.NozzleCoefficient, 2) * Math.Pow(nozzArea, 2))) + ((Math.Pow(flowRate, 2) * fluid.DensityInPoundPerGallon * toolAccu.CorrectionFactor) / (12032 * Math.Pow(toolAccu.NozzleCoefficientMh, 2) * Math.Pow(toolAccu.TotalFlowAreaInSquareInches, 2)));
            return returnValue;
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }
    }
}
