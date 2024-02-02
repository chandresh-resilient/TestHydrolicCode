using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    // Output porpoerty interface for mills and bits (Type 3 tools)
    public interface IBHAToolType3HydraulicsOutput : IBHAHydraulicsOutput
    {
        //void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
        double NozzlePressureDropInPSI { get; set; }
        double HydraulicHorsePower { get; set; }

        double ImpactForceInPounds { get; set; }
        double NozzleVelocityInFeetPerSecond { get; set; }

    }


    // This class takes care of hydraulic calculations of all type 3 tools i.e. mills and bits
    public class BHAToolType3 : BHATool, IBHAToolType3HydraulicsOutput
    {
        public IBHAToolType3HydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAToolType3HydraulicsOutput)this; }
        }

        #region Private Variables
        protected double iD = 0;
        protected double hydraulicHP;
        protected double nozzleVelocity;
        protected double impactForce;
        protected double nozzlePressureDrop;
        protected List<Nozzles> nozz;
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

        double IBHAToolType3HydraulicsOutput.HydraulicHorsePower
        {
            get { return hydraulicHP; }
            set { hydraulicHP = value; }
        }

        double IBHAToolType3HydraulicsOutput.ImpactForceInPounds
        {
            get { return impactForce; }
            set { impactForce = value; }
        }


        double IBHAToolType3HydraulicsOutput.NozzlePressureDropInPSI
        {
            get { return nozzlePressureDrop; }
            set { nozzlePressureDrop = value; }
        }

        double IBHAToolType3HydraulicsOutput.NozzleVelocityInFeetPerSecond
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
        public BHAToolType3() { }
        public BHAToolType3(int positionNumber, double toolDepth, string toolDescription, double outsideDiameterInInch, double lengthInFeet, List<Nozzles> nozzles, double insideDiameterInInch = 0)
        {
            this.PositionNumber = positionNumber;
            this.toolDescription = toolDescription;
            this.OutsideDiameterInInch = outsideDiameterInInch;
            this.InsideDiameterInInch = insideDiameterInInch;
            this.LengthInFeet = lengthInFeet;
            this.NozzlesInfomation = nozzles;
            this.Depth = toolDepth;

        }

        public override void CalculateHydraulics(Fluid fluid, double flowRate, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {

            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.Type3Calculations calc = new Calculations.Type3Calculations();

            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInch);
            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.NozzlesInfomation, this.InsideDiameterInInch, this.LengthInFeet);
            this.BHAHydraulicsOutput.FlowType = pressureInfo.FlowType;
            this.BHAHydraulicsOutput.PressureDropInPSI = pressureInfo.PressureDropInPSI;
            this.BHAHydraulicsOutput.HydraulicHorsePower = calc.CalculateHydraulicHorsePower(fluid, flowRate, this.NozzlesInfomation);
            this.BHAHydraulicsOutput.ImpactForceInPounds = calc.CalculateImpactForceInPounds(fluid, flowRate, this.NozzlesInfomation);
            this.BHAHydraulicsOutput.NozzlePressureDropInPSI = calc.CalculateNozzlePressureDropInPSI(fluid, flowRate, this.NozzlesInfomation);
            this.BHAHydraulicsOutput.NozzleVelocityInFeetPerSecond = calc.CalculateNozzleVelocityInFeetPerSecond(fluid, flowRate, this.NozzlesInfomation);
            this.BHAHydraulicsOutput.CriticalVelocityInFeetPerSecond = calc.CalculateCriticalVelocityInFeetPerSecond(fluid, this.InsideDiameterInInch);
            this.BHAHydraulicsOutput.EquivalentCirculatingDensity = calc.CalculateEquivalentCirculatingDensity(fluid, pressureInfo.PressureDropInPSI, this.Depth); 
        }

        public override BHATool GetDeepCopy()
        {
            return base.DeepCopyHelper(this);
        }

    }
}
