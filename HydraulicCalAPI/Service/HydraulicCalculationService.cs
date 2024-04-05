using System;
using System.Linq;
using System.Collections.Generic;
using HydraulicEngine;

namespace HydraulicCalAPI.Service
{
    public enum ResultType { Good, Caution, Problem };
    public enum ToolTypes { Type1, Type2, Type3, Type4, Type5, Type6, Type7, Type8, Type9 };
  //  public enum SurfaceEquipmentCaseType { Case1, Case2, Case3, Case4, TopDrive };
   // public enum CuttingType { Steel, Rock, CastIron, Granite, Sandstone, Concrete, WetSand };

    public class HydraulicCalculationService
    {
       
        public Fluid fluidInput { get; set; }
        public List<BHATool> bhaInput { get; set; }
        public double flowRateInGPMInput { get; set; }
        public List<WorkString> _workstringList { get; set; }
        public List<Annulus> annulusInput { get; set; }
        public Cuttings cuttingsInput { get; set; }
        public SurfaceEquipment surfaceEquipmentInput { get; set; }

        public double maxflowpressure { get; set; }
        public double maxflowrate { get; set; }
        public double torqueInFeetPound { get; set; }
        public double toolDepthInFeet { get; set; }
        public double blockPostionInFeet { get; set; }

        /// <summary>
        /// Inputs Points for the BHATool
        /// </summary>
        public class BHATool
        {
            public enum NozzleTypes { Jet, Hole };
            public class Nozzles
            {
                public NozzleTypes NozzleType { get; set; }
                public int NozzleQuantity { get; set; }
                public double NozzleDiameterInInch { get; set; }
                public double NozzleCoefficient { get; set; }
            }
            // public Guid? ToolIdentifier { get; set; }
            public int PositionNumber { get; set; }
            public string toolDescription { get; set; }
            public double OutsideDiameterInInch { get; set; }
            public double LengthInFeet { get; set; }

            /// <summary>
            /// Input point to get workstring data
            /// </summary>

            public string SectionName { get; set; }
            public double InsideDiameterInInch { get; set; }

            /// <summary>
            /// Input Points for BHAToolType1
            /// </summary>
            public double Depth { get; set; }

            /// <summary>
            /// Input Points for BHAToolType2
            /// </summary>
            public Common.BHAType2ModelName ModelName { get; set; }

            /// <summary>
            /// Input Points for BHATootType3
            /// </summary>
            public List<Nozzles> NozzlesInfomation { get; set; }
          
            /// <summary>
            /// Input Points for BHAToolType4
            /// </summary>
            public double ValveInsertDiameterInInch { get; set; }
            public double MinimumSidePortAreaInInch2 { get; set; }
            public double MaximumSidePortAreaInInch2 { get; set; }
            public double GapNutInsideDiameterInInch { get; set; }
            public double GapWidthInInch { get; set; }
            public double ActuatingFlowRateInGallonsPerMinute { get; set; }
            public Common.ToolState CurrentState { get; set; }

            /// <summary>
            /// Input Points for BHATootType5
            /// </summary>
            public List<Nozzles> AnnulusNozzleInformation { get; set; }
            public List<Nozzles> BHANozzleInformation { get; set; }

            /// <summary>
            /// Input Points for BHAToolType6
            /// </summary>
            public double LengthBeforeAnnulusOpeningInFeet { get; set; }
            public double LengthAfterAnnulusOpeningInFeet { get; set; }

            public Common.ToolState BHAOpeningState { get; set; }

            /// <summary>
            /// Input Points for BHATootType7
            /// </summary>
            public double PressureDropInPSI { get; set; }

            /// <summary>
            /// Input Points for BHAToolType8
            /// </summary>

            /// <summary>
            /// Input Points for BHATootType9
            /// </summary>
            public double ObservedFlowRateInGallonsPerMinute { get; set; }
            public double ObservedPressureDropInPSI { get; set; }

            /// <summary>
            /// Input Points for BHAToolType10
            /// </summary>
            public Accuset ToolAccuset { get; set; }

            /// <summary>
            /// Point to hold the type value of a tool
            /// </summary>
            public string bhatooltype { get; set; }
        }

    }
}
