using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public class Annulus
    {
        #region Private Variables
        private double annulusOD;
        private double annulusID;
        private double annulusTop = double.MinValue;
        private double annulusBottom = double.MinValue;
        private string wellboreSectionName;

        #endregion

        #region Properties

        public double AnnulusODInInch {
            get{return annulusOD;}
            set{annulusOD = value;}
        }

        public double AnnulusIDInInch
        {
            get{return annulusID;}
            set{annulusID = value;}
        }

        public double AnnulusTopInFeet
        {
            get{return annulusTop;}
            set{annulusTop = value;}
        }

        public double AnnulusBottomInFeet
        {
            get{return annulusBottom;}
            set{annulusBottom = value;}
        }

        public string WellboreSectionName
        {
            get{return wellboreSectionName;}
            set{wellboreSectionName = value;}
        }

        public double AnnulusLengthInFeet
        {
            get
            {
                if (annulusTop != double.MinValue && annulusBottom != double.MinValue)
                    return annulusTop - annulusBottom;
                else
                    return 0;
            }
  
        }

        #endregion

        #region Constructor

        public Annulus() { }

        public Annulus (string sectionName, double ODInInch, double IDInInch, double topInFeet, double bottomInFeet)
        {
            wellboreSectionName = sectionName;
            annulusOD = ODInInch;
            annulusID = IDInInch;
            annulusTop = topInFeet ;
            annulusBottom = bottomInFeet;
        }

        #endregion
    }
}
