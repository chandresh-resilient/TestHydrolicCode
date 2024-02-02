using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HydraulicEngine
{
    public class Cuttings
    {
        #region Private Variables
        Common.CuttingType cuttingType;
        double averageCuttingSize;
        #endregion

        #region Properties
        public Common.CuttingType CuttingsType
        {
            get { return cuttingType; }
            set { cuttingType = value; }
        }

        public double AverageCuttingSizeInInch
        {
            get { return averageCuttingSize; }
            set { averageCuttingSize = value; }
        }
        #endregion


        #region Constructors
        public Cuttings() { }

        public Cuttings (Common.CuttingType cuttingsType, double averageCuttingSizeInInch)
        {
            cuttingType = cuttingsType;
            averageCuttingSize = averageCuttingSizeInInch;
        }
        #endregion
    }
}
