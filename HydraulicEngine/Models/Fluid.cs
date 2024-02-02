using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public class Fluid
    {
        #region Private variable
        private double dens;
        private double yP;
        private double pV;
        //private double flowRate;
        #endregion

        #region Properties
        public double DensityInPoundPerGallon
        {
            get {return dens;}
            set {dens = value;}
        }
        public double YieldPointInPoundPerFeetSquare
        {
            get { return yP; }
            set { yP = value; }
        }
        public double PlasticViscosityInCentiPoise
        {
            get { return pV; }
            set { pV = value; }
        }
        //public double FlowRateInGPM
        //{
        //    get { return flowRate; }
        //    set { flowRate = value; }
        //}

        public Fluid() { }
        public Fluid(double densityInPoundPerGallon, double yieldPointInPoundsPerFeetSquare, double plasticViscosityInCentipoise) 
        {
            dens = densityInPoundPerGallon;
            yP = yieldPointInPoundsPerFeetSquare;
            pV = plasticViscosityInCentipoise;
            //flowRate = flowRateInGPM;
        }
       
        #endregion
    }
}
