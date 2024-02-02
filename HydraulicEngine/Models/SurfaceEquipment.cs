using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    public interface ISurfaceEquipmentHydraulicsOutput
    {
        double PressureDropInPSI { get; }
        
    }
    public class SurfaceEquipment : ISurfaceEquipmentHydraulicsOutput
    {
        #region Private Variables
        double standardPipeIdInInch;
        double standardPipelengthInFeet;
        double rotaryHoseIdInInch;
        double rotaryHoselengthInFeet;
        double swivelIdInInch;
        double swivellengthInFeet;
        double kellyIdInInch;
        double kellylengthInFeet;
        Common.SurfaceEquipmentCaseType caset;
        double pressureDrop;
        #endregion

        #region Properties

        public Common.SurfaceEquipmentCaseType CaseType
        {
            get { return caset; }
            set { caset = value; }
        }

        double ISurfaceEquipmentHydraulicsOutput.PressureDropInPSI
        {
            get { return pressureDrop; }
            
        }
        #endregion

        #region Constructors
        public SurfaceEquipment() { }

        public SurfaceEquipment(Common.SurfaceEquipmentCaseType caseType)
        {
            caset = caseType;
            SetProperties(caset);
        }
        #endregion


        public virtual void CalculateHydraulics(Fluid fluid, double flowRateInGPM)
        {
            pressureDrop = Calculations.PressureDropCalculations.CalculateSurfaceEquipmentPressureDropInPSI(fluid, flowRateInGPM, standardPipeIdInInch, standardPipelengthInFeet);
            pressureDrop += Calculations.PressureDropCalculations.CalculateSurfaceEquipmentPressureDropInPSI(fluid, flowRateInGPM, rotaryHoseIdInInch, rotaryHoselengthInFeet);
            pressureDrop += Calculations.PressureDropCalculations.CalculateSurfaceEquipmentPressureDropInPSI(fluid, flowRateInGPM, swivelIdInInch, swivellengthInFeet);
            pressureDrop += Calculations.PressureDropCalculations.CalculateSurfaceEquipmentPressureDropInPSI(fluid, flowRateInGPM, kellyIdInInch, kellylengthInFeet);
        }

        private void SetProperties (Common.SurfaceEquipmentCaseType caseType)
        {
            switch (caseType)
            {
                case Common.SurfaceEquipmentCaseType.Case1:
                    standardPipeIdInInch = 3;
                    standardPipelengthInFeet = 40;
                    rotaryHoseIdInInch = 2;
                    rotaryHoselengthInFeet = 40;
                    swivelIdInInch = 2;
                    swivellengthInFeet = 4;
                    kellyIdInInch = 2.25;
                    kellylengthInFeet = 40;
                    break;
                case Common.SurfaceEquipmentCaseType.Case2:
                    standardPipeIdInInch = 3.5;
                    standardPipelengthInFeet = 40;
                    rotaryHoseIdInInch = 2.5;
                    rotaryHoselengthInFeet = 55;
                    swivelIdInInch = 2.5;
                    swivellengthInFeet = 5;
                    kellyIdInInch = 3.25;
                    kellylengthInFeet = 40;
                    break;
                case Common.SurfaceEquipmentCaseType.Case3:
                    standardPipeIdInInch = 4;
                    standardPipelengthInFeet = 45;
                    rotaryHoseIdInInch = 3;
                    rotaryHoselengthInFeet = 55;
                    swivelIdInInch = 2.5;
                    swivellengthInFeet = 5;
                    kellyIdInInch = 3.25;
                    kellylengthInFeet = 40;
                    break;
                case Common.SurfaceEquipmentCaseType.Case4:
                    standardPipeIdInInch = 4;
                    standardPipelengthInFeet = 45;
                    rotaryHoseIdInInch = 3;
                    rotaryHoselengthInFeet = 55;
                    swivelIdInInch = 3;
                    swivellengthInFeet = 6;
                    kellyIdInInch = 4;
                    kellylengthInFeet = 40;
                    break;
                case Common.SurfaceEquipmentCaseType.TopDrive:
                    standardPipeIdInInch = 3.5;
                    standardPipelengthInFeet = 40;
                    rotaryHoseIdInInch = 2.5;
                    rotaryHoselengthInFeet = 60;
                    swivelIdInInch = 2.5;
                    swivellengthInFeet = 5;
                    kellyIdInInch = 3.25;
                    kellylengthInFeet = 1;
                    break;
                default:
                    standardPipeIdInInch = 0;
                    standardPipelengthInFeet = 0;
                    rotaryHoseIdInInch = 0;
                    rotaryHoselengthInFeet = 0;
                    swivelIdInInch = 0;
                    swivellengthInFeet = 0;
                    kellyIdInInch = 0;
                    kellylengthInFeet = 0;
                    break;
            }

        }
    }
}
