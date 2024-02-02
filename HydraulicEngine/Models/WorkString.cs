using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public interface IWorkStringHydraulicsOutput 
    {
        string FlowType { get; set; }
        double PressureDropinPSI { get; set; }
        double AverageVelocityInFtPerSecond { get; set; }
        void CalculateHydraulics(Fluid fluid, double flowRate = Double.MinValue);
    }
    public class WorkString : IWorkStringHydraulicsOutput
    {
        public IWorkStringHydraulicsOutput WorkStringHydraulicsOutput
        {
            get { return (IWorkStringHydraulicsOutput)this; }
        }

        #region Private Variables
        private int pN;
        private string sN;
        private double oD;
        private double iD;
        private double len;
        protected double averageVelocity = double.MinValue;
        protected double pressureDrop = double.MinValue;
        protected string flowType;
        #endregion

        #region Properties
        public int PositionNumber
        {
            get{return pN;}
            set{pN = value;}
        }
        public string SectionName
        {   
            get{return sN;}
            set{sN = value;}
        }
        public double OutsideDiameterInInch
        {
            get{return oD;}
            set{oD = value;}
        }

        public double InsideDiameterInInch
        {
            get{return iD;}
            set{iD = value;}
        }

        public double LengthInFeet
        {
            get{return len;}
            set{len = value;}
        }

        double IWorkStringHydraulicsOutput.AverageVelocityInFtPerSecond
        {
            get { return averageVelocity; }
            set { averageVelocity = value; }
        }

        double IWorkStringHydraulicsOutput.PressureDropinPSI
        {
            get { return pressureDrop; }
            set { pressureDrop = value; }
        }

        string IWorkStringHydraulicsOutput.FlowType
        {
            get { return flowType; }
            set { flowType = value; }
        }
        #endregion
        public WorkString() { }
        public WorkString(int positionNumber,  string sectionName, double outsideDiameterInInch, double insideDiameterInInch, double lengthInFeet)
        {
            pN = positionNumber;
            sN = sectionName;
            oD = outsideDiameterInInch;
            iD = insideDiameterInInch;
            len = lengthInFeet;
        }

        void IWorkStringHydraulicsOutput.CalculateHydraulics(Fluid fluid, double flowRate )
        {
            Calculations.PressureInformation pressureInfo = new Calculations.PressureInformation();
            Calculations.Type1Calculations calc = new Calculations.Type1Calculations();
            
            this.WorkStringHydraulicsOutput.AverageVelocityInFtPerSecond = calc.CalculateAverageVelocityInFeetPerSecond(flowRate, this.InsideDiameterInInch);
            pressureInfo = calc.CalculateTotalPressureDropInPSI(fluid, flowRate, this.InsideDiameterInInch, this.LengthInFeet);
            this.WorkStringHydraulicsOutput.FlowType = pressureInfo.FlowType;
            this.WorkStringHydraulicsOutput.PressureDropinPSI = pressureInfo.PressureDropInPSI;
        }
    }
}
