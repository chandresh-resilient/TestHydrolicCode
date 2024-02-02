using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public class Accuset
    {
        #region Private Variable
        string name;
        double boreID;
        double nozzleCoeff;
        double nozzlecoeffMh;
        double tfa;
        string fluid;
        double nozzleSize;
        double corrFactor;
        #endregion

        #region Properties
        public string AccusetSystemName
        {
            get { return name; }
            set { name = value; }
        }
        public double BoreIdInInches
        {
            get { return boreID; }
            set { boreID = value; }
        }
        public double NozzleCoefficient
        {
            get { return nozzleCoeff; }
            set { nozzleCoeff = value; }
        }
        public double NozzleCoefficientMh
        {
            get { return nozzlecoeffMh; }
            set { nozzlecoeffMh = value; }
        }
        public double TotalFlowAreaInSquareInches
        {
            get { return tfa; }
            set { tfa = value; }
        }
        public string Fluid
        {
            get { return fluid; }
            set { fluid = value; }
        }
        public double StandardNozzleSize
        {
            get { return nozzleSize; }
            set { nozzleSize = value; }
        }
        public double CorrectionFactor
        {
            get { return corrFactor; }
            set { corrFactor = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// This constructor is for serialization purpose.
        /// </summary>
        public Accuset()
        {
            
        }

        internal Accuset(string accusetSystemName, double boreIdInInches, double nozzleCoefficient, double nozzleCoefficientMh, double totalFlowAreaInsquareInches, string standardFluid, double standardNozzleSize, double correctionFactor)
        {
            name = accusetSystemName;
            boreID = boreIdInInches;
            nozzleCoeff = nozzleCoefficient;
            nozzlecoeffMh = nozzleCoefficientMh;
            tfa = totalFlowAreaInsquareInches;
            fluid = standardFluid;
            nozzleSize = standardNozzleSize;
            corrFactor = correctionFactor;
        }

        public Accuset(string accusetSystemName, double nozzleSizeInInches, double mudDensityInPoundsPerGallons)
        {
            Accuset accuset = AccusetSystem.GetAccusetDetails(accusetSystemName, nozzleSizeInInches, mudDensityInPoundsPerGallons);
            name = accusetSystemName;
            if (accuset != null)
            {
                boreID = accuset.boreID;
                nozzleCoeff = accuset.NozzleCoefficient;
                nozzlecoeffMh = accuset.NozzleCoefficientMh;
                tfa = accuset.TotalFlowAreaInSquareInches;
                fluid = accuset.Fluid;
                nozzleSize = accuset.StandardNozzleSize;
                corrFactor = accuset.corrFactor;
            }
        }

        #endregion

       
    }

   

   
}
