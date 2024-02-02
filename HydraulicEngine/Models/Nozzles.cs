using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public class Nozzles
    {
        #region variable
        public bool hasNozz;
        public enum NozzleTypes { Jet, Hole };
        public NozzleTypes nozType; 
        public int ncount;
        public double dia;
        private double nozzleCoeff = double.MinValue;
        #endregion

        #region Properties
        public NozzleTypes NozzleType
        {
            get{return nozType;}
            set{nozType = value;}
        }

        public int NozzleQuantity
        {
            get{return ncount;}
            set{ncount = value;}
        }

        public double NozzleDiameterInInch
        {
            get{return dia;}
            set{dia = value;}
        }

        public double NozzleCoefficient
        {
            get { return nozzleCoeff; }
            set { nozzleCoeff = value; }
        }

        #endregion

        /// <summary>
        /// This constructor is used for the purpose of Serialization.
        /// </summary>
        public Nozzles()
        {
                
        }

        public Nozzles(NozzleTypes nozzType, int nozzleQuantity, double nozzleDiameterinInch)
        {
            nozType = nozzType;
            ncount = nozzleQuantity;
            dia = nozzleDiameterinInch;
        }
        public Nozzles(double nozzleCoefficient, int nozzleQuantity, double nozzleDiameterinInch)
        {
            nozzleCoeff = nozzleCoefficient;
            ncount = nozzleQuantity;
            dia = nozzleDiameterinInch;
        }

        public Nozzles( int nozzleQuantity, double nozzleDiameterinInch)
        {
            ncount = nozzleQuantity;
            dia = nozzleDiameterinInch;
        }
    }
}
