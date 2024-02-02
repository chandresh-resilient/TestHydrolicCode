using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
    
    internal class FreeRunningLoss
    {
        #region Private Variables
        double flowRate;
        double pressureLoss;
        #endregion

        #region Properties

        public double FlowRateInGPM
        {
            get { return flowRate; }
            set { flowRate = value; }
        }

        public double PressureLossInPSI
        {
            get { return pressureLoss; }
            set { pressureLoss = value; }
        }

        #endregion

        internal FreeRunningLoss() { }

        internal FreeRunningLoss(double flowRateInGPM, double pressureLossInPSI)
        {
            flowRate = flowRateInGPM;
            pressureLoss = pressureLossInPSI;
        }

    }

    internal class TorqueData
    {
        #region Private Variables
        double flowRate;
        double pressureLoss;
        double speed;
        double torque;
        #endregion

        #region Properties

        public double FlowRateInGPM
        {
            get { return flowRate; }
            set { flowRate = value; }
        }

        public double PressureLossInPSI
        {
            get { return pressureLoss; }
            set { pressureLoss = value; }
        }

        public double SpeedInRPM
        {
            get { return speed; }
            set { speed = value; }
        }

        public double TorqueInFeetPounds
        {
            get { return torque; }
            set { torque = value; }
        }

        #endregion

        internal TorqueData() { }

        internal TorqueData(double pressureLossInPSI, double flowRateInGPM, double speedInRPM, double torqueInFeetPounds)
        {
            flowRate = flowRateInGPM;
            pressureLoss = pressureLossInPSI;
            speed = speedInRPM;
            torque = torqueInFeetPounds;
        }

    }


    internal static  class Motor
    {
        #region FreeRunningLosses
        internal static  List<FreeRunningLoss> GetFreeRunningLossesData (Common.BHAType2ModelName modelName)
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            switch (modelName)
            {
                case Common.BHAType2ModelName.NullMotor:
                    returnValue = GetFreeRunningLossesNullMotor();
                    break;
                case Common.BHAType2ModelName.MacDrill168:
                    returnValue = GetFreeRunningLossesMacDrill168();
                    break;
                case Common.BHAType2ModelName.MacDrill212:
                    returnValue = GetFreeRunningLossesMacDrill212();
                    break;
                case Common.BHAType2ModelName.MacDrill287:
                    returnValue = GetFreeRunningLossesMacDrill287();
                    break;
                case Common.BHAType2ModelName.MacDrill287GB:
                    returnValue = GetFreeRunningLossesMacDrill287GB();
                    break;
                case Common.BHAType2ModelName.CTDPDM168:
                    returnValue = GetFreeRunningLossesCTDPDM168();
                    break;
                case Common.BHAType2ModelName.CTDPDM212:
                    returnValue = GetFreeRunningLossesCTDPDM212();
                    break;
                case Common.BHAType2ModelName.CTDPDM237:
                    returnValue = GetFreeRunningLossesCTDPDM237();
                    break;
                case Common.BHAType2ModelName.CTDPDM287:
                    returnValue = GetFreeRunningLossesCTDPDM287();
                    break;
                case Common.BHAType2ModelName.CTDPDM287TwoStage:
                    returnValue = GetFreeRunningLossesCTDPDM287TwoStage();
                    break;
                case Common.BHAType2ModelName.CTDPDM312:
                    returnValue = GetFreeRunningLossesCTDPDM312();
                    break;
                case Common.BHAType2ModelName.CTDPDM475:
                    returnValue = GetFreeRunningLossesCTDPDM475();
                    break;
                case Common.BHAType2ModelName.eCTD168:
                    returnValue = GetFreeRunningLosseseCTD168();
                    break;
                case Common.BHAType2ModelName.eCTD212:
                    returnValue = GetFreeRunningLosseseCTD212();
                    break;
                case Common.BHAType2ModelName.eCTD287:
                    returnValue = GetFreeRunningLosseseCTD287();
                    break;
               
            }
            returnValue.Where(FreeRunningLoss => FreeRunningLoss.FlowRateInGPM <= 100).LastOrDefault();
            return returnValue;
        }

       

        private static List<FreeRunningLoss> GetFreeRunningLosseseCTD287()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(60, 261.5));
            returnValue.Add(new FreeRunningLoss(80, 340.7));
            returnValue.Add(new FreeRunningLoss(100, 505.5));
            returnValue.Add(new FreeRunningLoss(120, 648.3));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLosseseCTD212()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(25, 150));
            returnValue.Add(new FreeRunningLoss(40, 301));
            returnValue.Add(new FreeRunningLoss(50, 478));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLosseseCTD168()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(20, 340));
            returnValue.Add(new FreeRunningLoss(30, 450));
            returnValue.Add(new FreeRunningLoss(35, 522));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM475()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(100, 87.6));
            returnValue.Add(new FreeRunningLoss(175, 153.3));
            returnValue.Add(new FreeRunningLoss(250, 219));            
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM312()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();

            returnValue.Add(new FreeRunningLoss(80,  175));
            returnValue.Add(new FreeRunningLoss(100, 217));
            returnValue.Add(new FreeRunningLoss(120, 276));
            returnValue.Add(new FreeRunningLoss(160, 370));            

            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM287TwoStage()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();

            returnValue.Add(new FreeRunningLoss(90,  525));
            returnValue.Add(new FreeRunningLoss(110, 641));
            returnValue.Add(new FreeRunningLoss(130, 758));
            returnValue.Add(new FreeRunningLoss(150, 874));
            returnValue.Add(new FreeRunningLoss(160, 932));
            
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM287()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();

            returnValue.Add(new FreeRunningLoss(40, 150));
            returnValue.Add(new FreeRunningLoss(50, 180));
            returnValue.Add(new FreeRunningLoss(60, 220));
            returnValue.Add(new FreeRunningLoss(70, 255));
            returnValue.Add(new FreeRunningLoss(80, 290));
            returnValue.Add(new FreeRunningLoss(90, 335));
            returnValue.Add(new FreeRunningLoss(100,380));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM237()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();

            returnValue.Add(new FreeRunningLoss(20, 130));
            returnValue.Add(new FreeRunningLoss(30, 207));
            returnValue.Add(new FreeRunningLoss(40, 295));
            returnValue.Add(new FreeRunningLoss(50, 384));
            returnValue.Add(new FreeRunningLoss(60, 480));
            returnValue.Add(new FreeRunningLoss(70, 571));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM212()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
          
            returnValue.Add(new FreeRunningLoss(20, 220));
            returnValue.Add(new FreeRunningLoss(30, 350));
            returnValue.Add(new FreeRunningLoss(40, 480));
            returnValue.Add(new FreeRunningLoss(50, 586));
            returnValue.Add(new FreeRunningLoss(60, 680));
            returnValue.Add(new FreeRunningLoss(65, 737));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesCTDPDM168()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(10, 241));
            returnValue.Add(new FreeRunningLoss(20, 403));
            returnValue.Add(new FreeRunningLoss(30, 557));
            returnValue.Add(new FreeRunningLoss(40, 737));
            returnValue.Add(new FreeRunningLoss(50, 1000));            
            return returnValue;
        }

      
    
        private static List<FreeRunningLoss> GetFreeRunningLossesNullMotor()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(0, 0));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesMacDrill168()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(13, 275));
            returnValue.Add(new FreeRunningLoss(16, 318));
            returnValue.Add(new FreeRunningLoss(19, 427));
            returnValue.Add(new FreeRunningLoss(21, 497));
            returnValue.Add(new FreeRunningLoss(22, 590));
            returnValue.Add(new FreeRunningLoss(23, 585));
            returnValue.Add(new FreeRunningLoss(24, 661));
            returnValue.Add(new FreeRunningLoss(25, 668));
            returnValue.Add(new FreeRunningLoss(26, 717));
            returnValue.Add(new FreeRunningLoss(27, 787));
            returnValue.Add(new FreeRunningLoss(28, 792));
            returnValue.Add(new FreeRunningLoss(29, 895));
            returnValue.Add(new FreeRunningLoss(30, 911));
            returnValue.Add(new FreeRunningLoss(32, 1069));
            returnValue.Add(new FreeRunningLoss(33, 1091));
            returnValue.Add(new FreeRunningLoss(34, 1220));
            returnValue.Add(new FreeRunningLoss(35, 1223));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesMacDrill212()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(20, 275));
            returnValue.Add(new FreeRunningLoss(30, 450));
            returnValue.Add(new FreeRunningLoss(40, 750));
            returnValue.Add(new FreeRunningLoss(50, 950));
            return returnValue;
        }

        private static List<FreeRunningLoss> GetFreeRunningLossesMacDrill287()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(40, 100));
            returnValue.Add(new FreeRunningLoss(70, 350));
            returnValue.Add(new FreeRunningLoss(90, 550));
            return returnValue;
        }
        private static List<FreeRunningLoss> GetFreeRunningLossesMacDrill287GB()
        {
            List<FreeRunningLoss> returnValue = new List<FreeRunningLoss>();
            returnValue.Add(new FreeRunningLoss(50, 300));
            returnValue.Add(new FreeRunningLoss(70, 375));
            returnValue.Add(new FreeRunningLoss(90, 630));
            return returnValue;
       
        }     
        #endregion
        #region Set Torque Data
        internal static List<TorqueData> GetTorqueData(Common.BHAType2ModelName modelName)
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            switch (modelName)
            {
                case Common.BHAType2ModelName.NullMotor:
                    returnValue = GetTorqueDataNullMotor();
                    break;
                case Common.BHAType2ModelName.MacDrill168:
                    returnValue = GetTorqueDataMacDrill168();
                    break;
                case Common.BHAType2ModelName.MacDrill212:
                    returnValue = GetTorqueDataMacDrill212();
                    break;
                case Common.BHAType2ModelName.MacDrill287:
                    returnValue = GetTorqueDataMacDrill287();
                    break;
                case Common.BHAType2ModelName.MacDrill287GB:
                    returnValue = GetTorqueDataMacDrill287GB();
                    break;

                case Common.BHAType2ModelName.CTDPDM168:
                    returnValue = GetTorqueDataCTDPDM168();
                    break;
                case Common.BHAType2ModelName.CTDPDM212:
                    returnValue = GetTorqueDataCTDPDM212();
                    break;
                case Common.BHAType2ModelName.CTDPDM237:
                    returnValue = GetTorqueDataCTDPDM237();
                    break;
                case Common.BHAType2ModelName.CTDPDM287:
                    returnValue = GetTorqueDataCTDPDM287();
                    break;
                case Common.BHAType2ModelName.CTDPDM287TwoStage:
                    returnValue = GetTorqueDataCTDPDM287TwoStage();
                    break;
                case Common.BHAType2ModelName.CTDPDM312:
                    returnValue = GetTorqueDataCTDPDM312();
                    break;
                case Common.BHAType2ModelName.CTDPDM475:
                    returnValue = GetTorqueDataCTDPDM475();
                    break;
                case Common.BHAType2ModelName.eCTD168:
                    returnValue = GetTorqueDataeCTD168();
                    break;
                case Common.BHAType2ModelName.eCTD212:
                    returnValue = GetTorqueDataeCTD212();
                    break;
                case Common.BHAType2ModelName.eCTD287:
                    returnValue = GetTorqueDataeCTD287();
                    break;
            }

            return returnValue;
        }

      
        private static List<TorqueData> GetTorqueDataNullMotor()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(1, 1, 5, 1));
            returnValue.Add(new TorqueData(1, 1000, 5, 1));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataMacDrill168()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 15, 825, 0));
            returnValue.Add(new TorqueData(0, 35, 1925, 0));
            returnValue.Add(new TorqueData(0, 40, 220, 0));
            returnValue.Add(new TorqueData(200, 15, 669, 16));
            returnValue.Add(new TorqueData(200, 35, 1769, 16));
            returnValue.Add(new TorqueData(200, 40, 2044, 16));
            returnValue.Add(new TorqueData(400, 15, 515, 20));
            returnValue.Add(new TorqueData(400, 35, 1615, 20));
            returnValue.Add(new TorqueData(400, 40, 1890, 20));
            returnValue.Add(new TorqueData(600, 15, 120, 28));
            returnValue.Add(new TorqueData(600, 35, 1220, 28));
            returnValue.Add(new TorqueData(600, 40, 1495, 28));
            returnValue.Add(new TorqueData(800, 15, -125, 36));
            returnValue.Add(new TorqueData(800, 34, 975, 36));
            returnValue.Add(new TorqueData(800, 40, 1250, 36));
            returnValue.Add(new TorqueData(1000, 15, -388, 41));
            returnValue.Add(new TorqueData(1000, 35, 712, 41));
            returnValue.Add(new TorqueData(1000, 40, 987, 41));
            returnValue.Add(new TorqueData(1200, 15, -719, 60));
            returnValue.Add(new TorqueData(1200, 35, 381, 60));
            returnValue.Add(new TorqueData(1200, 40, 656, 60));
            returnValue.Add(new TorqueData(1400, 15, -1019, 75));
            returnValue.Add(new TorqueData(1400, 35, 81, 75));
            returnValue.Add(new TorqueData(1400, 40, 356, 75));
            returnValue.Add(new TorqueData(1600, 15, -1100, 86));
            returnValue.Add(new TorqueData(1600, 35, 0, 86));
            returnValue.Add(new TorqueData(1600, 40, 155, 86));

            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataMacDrill212()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 40, 1167, 0));
            returnValue.Add(new TorqueData(0, 50, 1458, 0));
            returnValue.Add(new TorqueData(0, 60, 1750, 0));
            returnValue.Add(new TorqueData(200, 40, 617, 18));
            returnValue.Add(new TorqueData(200, 50, 908, 18));
            returnValue.Add(new TorqueData(200, 60, 1200, 18));
            returnValue.Add(new TorqueData(400, 40, 392, 30));
            returnValue.Add(new TorqueData(400, 50, 683, 30));
            returnValue.Add(new TorqueData(400, 60, 975, 30));
            returnValue.Add(new TorqueData(600, 40, 267, 46));
            returnValue.Add(new TorqueData(600, 50, 558, 46));
            returnValue.Add(new TorqueData(600, 60, 850, 46));
            returnValue.Add(new TorqueData(800, 40, 192, 58));
            returnValue.Add(new TorqueData(800, 50, 483, 58));
            returnValue.Add(new TorqueData(800, 60, 775, 58));
            returnValue.Add(new TorqueData(1000, 40, 132, 64));
            returnValue.Add(new TorqueData(1000, 50, 423, 64));
            returnValue.Add(new TorqueData(1000, 60, 715, 64));
            returnValue.Add(new TorqueData(1200, 40, 79, 78));
            returnValue.Add(new TorqueData(1200, 50, 370, 78));
            returnValue.Add(new TorqueData(1200, 60, 662, 78));
            returnValue.Add(new TorqueData(1400, 40, 17, 91));
            returnValue.Add(new TorqueData(1400, 50, 308, 91));
            returnValue.Add(new TorqueData(1400, 60, 600, 91));
            returnValue.Add(new TorqueData(1600, 40, 0, 106));
            returnValue.Add(new TorqueData(1600, 50, 231, 106));
            returnValue.Add(new TorqueData(1600, 60, 523, 106));
            returnValue.Add(new TorqueData(1800, 40, 0, 121));
            returnValue.Add(new TorqueData(1800, 50, 148, 121));
            returnValue.Add(new TorqueData(1800, 60, 440, 121));
            return returnValue;
        }
        private static List<TorqueData> GetTorqueDataMacDrill287()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 40, 240, 0));
            returnValue.Add(new TorqueData(0, 60, 360, 0));
            returnValue.Add(new TorqueData(0, 80, 480, 0));
            returnValue.Add(new TorqueData(0, 100, 600, 0));
            returnValue.Add(new TorqueData(200, 40, 240, 62));
            returnValue.Add(new TorqueData(200, 60, 360, 62));
            returnValue.Add(new TorqueData(200, 80, 480, 62));
            returnValue.Add(new TorqueData(200, 100, 600, 62));
            returnValue.Add(new TorqueData(400, 40, 159, 128));
            returnValue.Add(new TorqueData(400, 60, 279, 128));
            returnValue.Add(new TorqueData(400, 80, 399, 128));
            returnValue.Add(new TorqueData(400, 100, 519, 128));
            returnValue.Add(new TorqueData(600, 40, 138, 183));
            returnValue.Add(new TorqueData(600, 60, 258, 183));
            returnValue.Add(new TorqueData(600, 80, 378, 183));
            returnValue.Add(new TorqueData(600, 100, 498, 183));
            returnValue.Add(new TorqueData(800, 40, 128, 225));
            returnValue.Add(new TorqueData(800, 60, 248, 225));
            returnValue.Add(new TorqueData(800, 80, 368, 225));
            returnValue.Add(new TorqueData(800, 100, 488, 225));
            returnValue.Add(new TorqueData(1000, 40, 119, 299));
            returnValue.Add(new TorqueData(1000, 60, 239, 299));
            returnValue.Add(new TorqueData(1000, 80, 359, 299));
            returnValue.Add(new TorqueData(1000, 100, 479, 299));
            returnValue.Add(new TorqueData(1200, 40, 110, 347));
            returnValue.Add(new TorqueData(1200, 60, 230, 347));
            returnValue.Add(new TorqueData(1200, 80, 350, 347));
            returnValue.Add(new TorqueData(1200, 100, 470, 347));
            returnValue.Add(new TorqueData(1400, 40, 90, 400));
            returnValue.Add(new TorqueData(1400, 60, 210, 400));
            returnValue.Add(new TorqueData(1400, 80, 330, 400));
            returnValue.Add(new TorqueData(1400, 100, 450, 400));
            returnValue.Add(new TorqueData(1600, 40, 79, 440));
            returnValue.Add(new TorqueData(1600, 60, 199, 440));
            returnValue.Add(new TorqueData(1600, 80, 319, 440));
            returnValue.Add(new TorqueData(1600, 100, 439, 440));
            returnValue.Add(new TorqueData(1800, 40, 48, 480));
            returnValue.Add(new TorqueData(1800, 60, 168, 480));
            returnValue.Add(new TorqueData(1800, 80, 288, 480));
            returnValue.Add(new TorqueData(1800, 100, 408, 480));
            return returnValue;
        }
        private static List<TorqueData> GetTorqueDataMacDrill287GB()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 50, 249, 0));
            returnValue.Add(new TorqueData(0, 70, 383, 0));
            returnValue.Add(new TorqueData(0, 90, 479, 0));
            returnValue.Add(new TorqueData(100, 50, 227, 44));
            returnValue.Add(new TorqueData(100, 70, 351, 44));
            returnValue.Add(new TorqueData(100, 90, 445, 46));
            returnValue.Add(new TorqueData(300, 50, 213, 98));
            returnValue.Add(new TorqueData(300, 70, 313, 103));
            returnValue.Add(new TorqueData(300, 90, 408, 108));
            returnValue.Add(new TorqueData(500, 50, 204, 144));
            returnValue.Add(new TorqueData(500, 70, 294, 144));
            returnValue.Add(new TorqueData(500, 90, 389, 146));
            returnValue.Add(new TorqueData(1000, 50, 192, 227));
            returnValue.Add(new TorqueData(1000, 70, 272, 227));
            returnValue.Add(new TorqueData(1000, 90, 365, 238));
            returnValue.Add(new TorqueData(1500, 50, 185, 308));
            returnValue.Add(new TorqueData(1500, 70, 258, 310));
            returnValue.Add(new TorqueData(1500, 90, 350, 312));
            returnValue.Add(new TorqueData(2000, 50, 177, 392));
            returnValue.Add(new TorqueData(2000, 70, 251, 399));
            returnValue.Add(new TorqueData(2000, 90, 329, 404));
            returnValue.Add(new TorqueData(2500, 50, 158, 456));
            returnValue.Add(new TorqueData(2500, 70, 233, 456));
            returnValue.Add(new TorqueData(2500, 90, 310, 454));
            returnValue.Add(new TorqueData(3000, 50, 114, 506));
            returnValue.Add(new TorqueData(3000, 70, 188, 504));
            returnValue.Add(new TorqueData(3000, 90, 267, 510));
            returnValue.Add(new TorqueData(3350, 50, 66, 554));
            returnValue.Add(new TorqueData(3350, 70, 120, 565));
            returnValue.Add(new TorqueData(3350, 90, 182, 573));
   
            return returnValue;
            
        }
        private static List<TorqueData> GetTorqueDataCTDPDM168()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 20, 280, 0));
            returnValue.Add(new TorqueData(0, 30, 420, 0));
            returnValue.Add(new TorqueData(0, 42, 588, 0));
            returnValue.Add(new TorqueData(0, 50, 700, 0));
            returnValue.Add(new TorqueData(200, 20, 262, 57.3));
            returnValue.Add(new TorqueData(200, 30, 402, 57.3));
            returnValue.Add(new TorqueData(200, 42, 570, 57.3));
            returnValue.Add(new TorqueData(200, 50, 682, 57.3));
            returnValue.Add(new TorqueData(400, 20, 191, 104.3));
            returnValue.Add(new TorqueData(400, 30, 331, 104.3));
            returnValue.Add(new TorqueData(400, 42, 499, 104.3));
            returnValue.Add(new TorqueData(400, 50, 611, 104.3));
            returnValue.Add(new TorqueData(600, 20, 104, 139.7));
            returnValue.Add(new TorqueData(600, 30, 244, 139.7));
            returnValue.Add(new TorqueData(600, 42, 412, 139.7));
            returnValue.Add(new TorqueData(600, 50, 524, 139.7));
            returnValue.Add(new TorqueData(800, 20, 8, 162.2));
            returnValue.Add(new TorqueData(800, 30, 148, 162.2));
            returnValue.Add(new TorqueData(800, 42, 316, 162.2));
            returnValue.Add(new TorqueData(800, 50, 428, 162.2));
            returnValue.Add(new TorqueData(1000, 20, -120, 170.4));
            returnValue.Add(new TorqueData(1000, 30, 20, 170.4));
            returnValue.Add(new TorqueData(1000, 42, 188, 170.4));
            returnValue.Add(new TorqueData(1000, 50, 300, 170.4));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM212()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 20, 182, 0));
            returnValue.Add(new TorqueData(0, 40, 363, 0));
            returnValue.Add(new TorqueData(0, 65, 590, 0));
            returnValue.Add(new TorqueData(200, 20, 167, 72));
            returnValue.Add(new TorqueData(200, 40, 348, 72));
            returnValue.Add(new TorqueData(200, 65, 575, 72));
            returnValue.Add(new TorqueData(400, 20, 150, 132));
            returnValue.Add(new TorqueData(400, 40, 331, 132));
            returnValue.Add(new TorqueData(400, 65, 558, 132));
            returnValue.Add(new TorqueData(600, 20, 122, 192));
            returnValue.Add(new TorqueData(600, 40, 303, 192));
            returnValue.Add(new TorqueData(600, 65, 530, 192));
            returnValue.Add(new TorqueData(800, 20, 82, 245));
            returnValue.Add(new TorqueData(800, 40, 263, 245));
            returnValue.Add(new TorqueData(800, 65, 490, 245));
            returnValue.Add(new TorqueData(1000, 20, 47, 290));
            returnValue.Add(new TorqueData(1000, 40, 228, 290));
            returnValue.Add(new TorqueData(1000, 65, 455, 290));
            returnValue.Add(new TorqueData(1200, 20, 2, 338));
            returnValue.Add(new TorqueData(1200, 40, 183, 338));
            returnValue.Add(new TorqueData(1200, 65, 410, 338));
            returnValue.Add(new TorqueData(1400, 20, -58, 370));
            returnValue.Add(new TorqueData(1400, 40, 123, 370));
            returnValue.Add(new TorqueData(1400, 65, 350, 370));
            returnValue.Add(new TorqueData(1450, 20, -85, 400));
            returnValue.Add(new TorqueData(1450, 40, 96, 400));
            returnValue.Add(new TorqueData(1450, 65, 323, 400));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM237()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 40, 288, 0));
            returnValue.Add(new TorqueData(0, 60, 431, 0));
            returnValue.Add(new TorqueData(0, 80, 575, 0));
            returnValue.Add(new TorqueData(200, 40, 283, 80));
            returnValue.Add(new TorqueData(200, 60, 426, 80));
            returnValue.Add(new TorqueData(200, 80, 570, 80));
            returnValue.Add(new TorqueData(400, 40, 278, 152));
            returnValue.Add(new TorqueData(400, 60, 421, 152));
            returnValue.Add(new TorqueData(400, 80, 565, 152));
            returnValue.Add(new TorqueData(600, 40, 258, 217));
            returnValue.Add(new TorqueData(600, 60, 401, 217));
            returnValue.Add(new TorqueData(600, 80, 545, 217));
            returnValue.Add(new TorqueData(800, 40, 223, 275));
            returnValue.Add(new TorqueData(800, 60, 366, 275));
            returnValue.Add(new TorqueData(800, 80, 510, 275));
            returnValue.Add(new TorqueData(1000, 40, 193, 330));
            returnValue.Add(new TorqueData(1000, 60, 336, 330));
            returnValue.Add(new TorqueData(1000, 80, 480, 330));
            returnValue.Add(new TorqueData(1200, 40, 153, 365));
            returnValue.Add(new TorqueData(1200, 60, 296, 365));
            returnValue.Add(new TorqueData(1200, 80, 440, 365));
            returnValue.Add(new TorqueData(1400, 40, 48, 410));
            returnValue.Add(new TorqueData(1400, 60, 191, 410));
            returnValue.Add(new TorqueData(1400, 80, 335, 410));
            returnValue.Add(new TorqueData(1450, 40, 25, 435));
            returnValue.Add(new TorqueData(1450, 60, 168, 435));
            returnValue.Add(new TorqueData(1450, 80, 312, 435));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM287()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 60, 230, 0));
            returnValue.Add(new TorqueData(0, 90, 345, 0));
            returnValue.Add(new TorqueData(0, 120, 460, 0));
            returnValue.Add(new TorqueData(200, 60, 200, 175));
            returnValue.Add(new TorqueData(200, 90, 315, 175));
            returnValue.Add(new TorqueData(200, 120, 430, 175));
            returnValue.Add(new TorqueData(400, 60, 155, 314));
            returnValue.Add(new TorqueData(400, 90, 270, 314));
            returnValue.Add(new TorqueData(400, 120, 385, 314));
            returnValue.Add(new TorqueData(600, 60, 110, 440));
            returnValue.Add(new TorqueData(600, 90, 225, 440));
            returnValue.Add(new TorqueData(600, 120, 340, 440));
            returnValue.Add(new TorqueData(800, 60, 48, 555));
            returnValue.Add(new TorqueData(800, 90, 163, 555));
            returnValue.Add(new TorqueData(800, 120, 278, 555));
            returnValue.Add(new TorqueData(1000, 60, -21, 636));
            returnValue.Add(new TorqueData(1000, 90, 94, 636));
            returnValue.Add(new TorqueData(1000, 120, 209, 636));
            returnValue.Add(new TorqueData(1200, 60, -108, 720));
            returnValue.Add(new TorqueData(1200, 90, 7, 720));
            returnValue.Add(new TorqueData(1200, 120, 122, 720));

            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM287TwoStage()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 90, 155.7, 0.0));
            returnValue.Add(new TorqueData(0, 125, 216.2, 0.0));
            returnValue.Add(new TorqueData(0, 160, 276.8, 0.0));
            returnValue.Add(new TorqueData(50, 90, 144.8, 118.7));
            returnValue.Add(new TorqueData(50, 125, 205.4, 118.7));
            returnValue.Add(new TorqueData(50, 160, 265.9, 118.7));
            returnValue.Add(new TorqueData(100, 90, 133.9, 227.8));
            returnValue.Add(new TorqueData(100, 125, 194.5, 227.8));
            returnValue.Add(new TorqueData(100, 160, 255.0, 227.8));
            returnValue.Add(new TorqueData(150, 90, 123.1, 327.1));
            returnValue.Add(new TorqueData(150, 125, 183.6, 327.1));
            returnValue.Add(new TorqueData(150, 160, 244.1, 327.1));
            returnValue.Add(new TorqueData(200, 90, 112.2, 416.7));
            returnValue.Add(new TorqueData(200, 125, 172.7, 416.7));
            returnValue.Add(new TorqueData(200, 160, 233.3, 416.7));
            returnValue.Add(new TorqueData(250, 90, 101.3, 496.6));
            returnValue.Add(new TorqueData(250, 125, 161.9, 496.6));
            returnValue.Add(new TorqueData(250, 160, 222.4, 496.6));
            returnValue.Add(new TorqueData(300, 90, 90.4, 566.8));
            returnValue.Add(new TorqueData(300, 125, 151.0, 566.8));
            returnValue.Add(new TorqueData(300, 160, 211.5, 566.8));
            returnValue.Add(new TorqueData(350, 90, 79.6, 627.4));
            returnValue.Add(new TorqueData(350, 125, 140.1, 627.4));
            returnValue.Add(new TorqueData(350, 160, 200.6, 627.4));
            returnValue.Add(new TorqueData(420, 90, 64.3, 695.8));
            returnValue.Add(new TorqueData(420, 125, 124.9, 695.8));
            returnValue.Add(new TorqueData(420, 160, 185.4, 695.8));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM312()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 80, 198, 0));
            returnValue.Add(new TorqueData(0, 120, 296, 0));
            returnValue.Add(new TorqueData(0, 160, 395, 0));
            returnValue.Add(new TorqueData(200, 80, 194, 114));
            returnValue.Add(new TorqueData(200, 120, 292, 114));
            returnValue.Add(new TorqueData(200, 160, 391, 114));
            returnValue.Add(new TorqueData(400, 80, 181, 338));
            returnValue.Add(new TorqueData(400, 120, 279, 338));
            returnValue.Add(new TorqueData(400, 160, 378, 338));
            returnValue.Add(new TorqueData(600, 80, 162, 595));
            returnValue.Add(new TorqueData(600, 120, 260, 595));
            returnValue.Add(new TorqueData(600, 160, 359, 595));
            returnValue.Add(new TorqueData(800, 80, 134, 762));
            returnValue.Add(new TorqueData(800, 120, 232, 762));
            returnValue.Add(new TorqueData(800, 160, 331, 762));
            returnValue.Add(new TorqueData(1000, 80, 105, 946));
            returnValue.Add(new TorqueData(1000, 120, 203, 946));
            returnValue.Add(new TorqueData(1000, 160, 302, 946));
            returnValue.Add(new TorqueData(1200, 80, 50, 1110));
            returnValue.Add(new TorqueData(1200, 120, 148, 1110));
            returnValue.Add(new TorqueData(1200, 160, 247, 1110));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataCTDPDM475()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 100, 106.08, 0));
            returnValue.Add(new TorqueData(0, 175, 183.82, 0));
            returnValue.Add(new TorqueData(0, 250, 260, 0));
            returnValue.Add(new TorqueData(100, 100, 101.3, 273.3));
            returnValue.Add(new TorqueData(100, 175, 179.0, 273.3));
            returnValue.Add(new TorqueData(100, 250, 255.2, 273.3));
            returnValue.Add(new TorqueData(200, 100, 95.9, 546.7));
            returnValue.Add(new TorqueData(200, 175, 173.6, 546.7));
            returnValue.Add(new TorqueData(200, 250, 249.8, 546.7));
            returnValue.Add(new TorqueData(300, 100, 85.8, 820.0));
            returnValue.Add(new TorqueData(300, 175, 163.6, 820.0));
            returnValue.Add(new TorqueData(300, 250, 239.8, 820.0));
            returnValue.Add(new TorqueData(400, 100, 67.0, 1093.3));
            returnValue.Add(new TorqueData(400, 175, 144.7, 1093.3));
            returnValue.Add(new TorqueData(400, 250, 220.9, 1093.3));
            returnValue.Add(new TorqueData(500, 100, 35.2, 1366.7));
            returnValue.Add(new TorqueData(500, 175, 112.9, 1366.7));
            returnValue.Add(new TorqueData(500, 250, 189.1, 1366.7));
            returnValue.Add(new TorqueData(600, 100, -13.7, 1640.0));
            returnValue.Add(new TorqueData(600, 175, 64.0, 1640.0));
            returnValue.Add(new TorqueData(600, 250, 140.2, 1640.0));

            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataeCTD168()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 20, 217.1, 0.0));
            returnValue.Add(new TorqueData(0, 30, 325.7, 0.0));
            returnValue.Add(new TorqueData(0, 35, 380.0, 0.0));
            returnValue.Add(new TorqueData(200, 20, 197.1, 66.0));
            returnValue.Add(new TorqueData(200, 30, 305.7, 66.0));
            returnValue.Add(new TorqueData(200, 35, 360.0, 66.0));
            returnValue.Add(new TorqueData(400, 20, 187.1, 117.0));
            returnValue.Add(new TorqueData(400, 30, 295.7, 117.0));
            returnValue.Add(new TorqueData(400, 35, 350.0, 117.0));
            returnValue.Add(new TorqueData(600, 20, 167.1, 160.0));
            returnValue.Add(new TorqueData(600, 30, 275.7, 160.0));
            returnValue.Add(new TorqueData(600, 35, 330.0, 160.0));
            returnValue.Add(new TorqueData(800, 20, 142.1, 195.0));
            returnValue.Add(new TorqueData(800, 30, 250.7, 195.0));
            returnValue.Add(new TorqueData(800, 35, 305.0, 195.0));
            returnValue.Add(new TorqueData(1000, 20, 117.1, 225.0));
            returnValue.Add(new TorqueData(1000, 30, 225.7, 225.0));
            returnValue.Add(new TorqueData(1000, 35, 280.0, 225.0));
            returnValue.Add(new TorqueData(1200, 20, 85.1, 250.0));
            returnValue.Add(new TorqueData(1200, 30, 193.7, 250.0));
            returnValue.Add(new TorqueData(1200, 35, 248.0, 250.0));
            return returnValue;
        }

        private static List<TorqueData> GetTorqueDataeCTD212()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 40, 485, 0));
            returnValue.Add(new TorqueData(0, 50, 613, 0));
            returnValue.Add(new TorqueData(400, 25, 288, 80));
            returnValue.Add(new TorqueData(400, 40, 479, 84));
            returnValue.Add(new TorqueData(400, 50, 599, 84));
            returnValue.Add(new TorqueData(800, 25, 255, 160));
            returnValue.Add(new TorqueData(800, 40, 446, 165));
            returnValue.Add(new TorqueData(800, 50, 569, 162));
            returnValue.Add(new TorqueData(1200, 25, 195, 238));
            returnValue.Add(new TorqueData(1200, 40, 390, 240));
            returnValue.Add(new TorqueData(1200, 50, 522, 233));
            returnValue.Add(new TorqueData(1600, 25, 120, 294));
            returnValue.Add(new TorqueData(1600, 40, 315, 303));
            returnValue.Add(new TorqueData(1600, 50, 458, 299));
            returnValue.Add(new TorqueData(2000, 25, 25, 312));
            returnValue.Add(new TorqueData(2000, 40, 225, 353));
            returnValue.Add(new TorqueData(2000, 50, 378, 359));
            returnValue.Add(new TorqueData(2400, 25, -66, 322));
            returnValue.Add(new TorqueData(2400, 40, 124, 384));
            returnValue.Add(new TorqueData(2400, 50, 281, 413));

            return returnValue;
           
        }

        private static List<TorqueData> GetTorqueDataeCTD287()
        {
            List<TorqueData> returnValue = new List<TorqueData>();
            returnValue.Add(new TorqueData(0, 60, 222, 0));
            returnValue.Add(new TorqueData(0, 80, 284, 0));
            returnValue.Add(new TorqueData(0, 100, 364, 0));
            returnValue.Add(new TorqueData(0, 120, 443, 0));
            returnValue.Add(new TorqueData(200, 60, 214, 139));
            returnValue.Add(new TorqueData(200, 80, 283, 142));
            returnValue.Add(new TorqueData(200, 100, 360, 145));
            returnValue.Add(new TorqueData(200, 120, 438, 137));
            returnValue.Add(new TorqueData(400, 60, 202, 269));
            returnValue.Add(new TorqueData(400, 80, 276, 274));
            returnValue.Add(new TorqueData(400, 100, 350, 280));
            returnValue.Add(new TorqueData(400, 120, 430, 265));
            returnValue.Add(new TorqueData(600, 60, 184, 389));
            returnValue.Add(new TorqueData(600, 80, 262, 396));
            returnValue.Add(new TorqueData(600, 100, 335, 405));
            returnValue.Add(new TorqueData(600, 120, 416, 386));
            returnValue.Add(new TorqueData(800, 60, 161, 498));
            returnValue.Add(new TorqueData(800, 80, 242, 508));
            returnValue.Add(new TorqueData(800, 100, 315, 519));
            returnValue.Add(new TorqueData(800, 120, 399, 500));
            returnValue.Add(new TorqueData(1000, 60, 133, 598));
            returnValue.Add(new TorqueData(1000, 80, 216, 611));
            returnValue.Add(new TorqueData(1000, 100, 290, 624));
            returnValue.Add(new TorqueData(1000, 120, 378, 605));
            returnValue.Add(new TorqueData(1200, 60, 101, 689));
            returnValue.Add(new TorqueData(1200, 80, 184, 703));
            returnValue.Add(new TorqueData(1200, 100, 260, 718));
            returnValue.Add(new TorqueData(1200, 120, 352, 703));
            returnValue.Add(new TorqueData(1400, 60, 63, 769));
            returnValue.Add(new TorqueData(1400, 80, 146, 786));
            returnValue.Add(new TorqueData(1400, 100, 225, 802));
            returnValue.Add(new TorqueData(1400, 120, 322, 793));
            returnValue.Add(new TorqueData(1600, 60, 20, 839));
            returnValue.Add(new TorqueData(1600, 80, 101, 859));
            returnValue.Add(new TorqueData(1600, 100, 185, 876));
            returnValue.Add(new TorqueData(1600, 120, 288, 876));
            returnValue.Add(new TorqueData(1800, 60, -29, 900));
            returnValue.Add(new TorqueData(1800, 80, 50, 922));
            returnValue.Add(new TorqueData(1800, 100, 140, 940));
            returnValue.Add(new TorqueData(1800, 120, 250, 951));
            returnValue.Add(new TorqueData(2000, 60, -82, 951));
            returnValue.Add(new TorqueData(2000, 80, -6, 975));
            returnValue.Add(new TorqueData(2000, 100, 89, 993));
            returnValue.Add(new TorqueData(2000, 120, 208, 1018));

            return returnValue;
        }
   
        #endregion
    }


    
}
