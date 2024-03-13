using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HydraulicCalAPI.Service
{
    #region Chart and Graph Section
    public class PressureDistributionChartCollection
    {
        public string name { get; set; }
        public string value { get; set; }
        public string color { get; set; }
    }

    public class HydraproLineSeries
    {
        public double secondaryAxisValue { get; set; }
        public int primaryAxisValue { get; set; }
    }
    public class BhAchart
    {
        public List<HydraproLineSeries> HydraproLineSeries { get; set; }
    }
    public class HydraulicOutputBHAList
    {
        public object toolSourcePath { get; set; }
        public bool isToolInputDetailsVisible { get; set; }
        public object toolType { get; set; }
        public BhAchart bhAchart { get; set; }
        public object toolID { get; set; }
        public string workstring { get; set; }
        public int positionNo { get; set; }
        public int lengthBHA { get; set; }
        public double outerDiameter { get; set; }
        public double inputFlowRate { get; set; }
        public double averageVelocity { get; set; }
        public double criticalVelocity { get; set; }
        public string flowType { get; set; }
        public double bhaPressureDrop { get; set; }
        public string averageVelocityColor { get; set; }
        public string isPlusMinus { get; set; }
        public string bhaColor { get; set; }
        public bool isToolDetailsVisible { get; set; }
        public object actualToolDescription { get; set; }
        public bool showStandpipeVsFlowRateScaling { get; set; }
        public int pressureLowerDisplayValue { get; set; }
        public int pressureUpperDisplayValue { get; set; }
        public int flowRateLowerDisplayValue { get; set; }
        public int flowRateUpperDisplayValue { get; set; }
        public object pressureRangeMinAppliedValue { get; set; }
        public object pressureRangeMaxAppliedValue { get; set; }
        public object flowRateRangeMinAppliedValue { get; set; }
        public object flowRateRangeMaxAppliedValue { get; set; }
    }

    #endregion
    public class BhaTopToBottom
    {
        public string ID { get; set; }
        public string ToolDescription { get; set; }
        public string SerialNumber { get; set; }
        public string MeasuredOD { get; set; }
        public string InnerDiameter { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string UpperConnType { get; set; }
        public string LowerConnType { get; set; }
        public string FishNeckOD { get; set; }
        public string FishNeckLength { get; set; }
        public string HydraulicOD { get; set; }
        public string HydraulicID { get; set; }
    }
    public class CaseLinerTube
    {
        public string CLTID { get; set; }
        public string WellBoreSection { get; set; }
        public string OutDiameter { get; set; }
        public string InnDiameter { get; set; }
        public string WellBoreWeight { get; set; }
        public string Grade { get; set; }
        public string WellTop { get; set; }
        public string WellBottom { get; set; }

    }
    public class WorkStringData
    {
        public string wrkID { get; set; }
        public string wrkToolDescription { get; set; }
        public string wrkMeasuredOD { get; set; }
        public string wrkInnerDiameter { get; set; }
        public string wrkWeight { get; set; }
        public string wrkLength { get; set; }
        public string wrkUpperConnType { get; set; }
    }
    public class PdfReportService
    {
        public string ReportHeader { get; set; }
        public string Customer { get; set; }
        public string JobNumber { get; set; }
        public string WellNameNumber { get; set; }
        public string PreparedBy { get; set; }
        public string PreparedOn { get; set; }
        public string Comment { get; set; }
        public string JobID { get; set; }
        public string WPTSReportID { get; set; }
        public string AccuViewVersion { get; set; }
        public string Segment { get; set; }
        public string ProductService { get; set; }
        public string JobStartDate { get; set; }
        public string WellLocation { get; set; }
        public double WellDepth { get; set; }
        public string JobEndDate { get; set; }
        public string JDEDeliveryTicketNo { get; set; }
        public string Field { get; set; }
        public string Lease { get; set; }
        public string Rig { get; set; }
        public string legalAPIOCSG { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WellCountry { get; set; }
        public string CountyParish { get; set; }
        public string WFRDLocation { get; set; }
        public string Hemisphere { get; set; }
        public string Geozone { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string CustomerContactOffice { get; set; }
        public string CustomerPhoneNoOffice { get; set; }
        public string CustomerContactField { get; set; }
        public string CustomerPhoneNoField { get; set; }
        public string DrillingEngineer { get; set; }
        public string DrillingContractor { get; set; }
        public string WFRDSalesman { get; set; }
        public string WFRDFieldEngineer { get; set; }
        public string ProjectName { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string QuoteNumber { get; set; }
        public string RigElevation { get; set; }
        public string ReservoirType { get; set; }
        public string WaterDepth { get; set; }
        public string RigType { get; set; }
        public string WellClassification { get; set; }
        public string WorkString { get; set; }
        public string Inclination { get; set; }
        public string CustomerType { get; set; }
        public string H2SPresent { get; set; }
        public string CO2Present { get; set; }
        public string TotalMileageTFLocation { get; set; }
        public string TotalTravelTimeTFLlocation { get; set; }
        public string TotalOffDutyHrsAtLocation { get; set; }
        public string Status { get; set; }
        public string InputBy { get; set; }
        public string StatusPreparedBy { get; set; }
        public string AccuviewInputDate { get; set; }
        public string SubmittedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string AnnulusLength { get; set; }
        public string BHALength { get; set; }
        public string ToolDepth { get; set; }
        public string SurfaceEquipment { get; set; }
        public double TotalLength { get; set; }
        public string MaximumAllowablePressure { get; set; }
        public string MaximumAllowableFlowrate { get; set; }
        public string Comments { get; set; }
        public double Solids { get; set; }
        public string DrillingFluidType { get; set; }
        public double DrillingFluidWeight { get; set; }
        public double BuoyancyFactorl { get; set; }
        public double PlasticViscosity { get; set; }
        public double YieldPoint { get; set; }
        public double CuttingAverageSize { get; set; }
        public string CuttingType { get; set; }
        public List<CaseLinerTube> CasingLinerTubing { get; set; }
        public List<WorkStringData> WorkStringItem { get; set; } 
        public List<BhaTopToBottom> BhaTopToBottom { get; set; }
        public List<PressureDistributionChartCollection> PressureDistributionChartCollection { get; set; }

        public List<HydraulicOutputBHAList> HydraulicOutputBHAList { get; set; }
        public HydraulicCalculationService HydraulicCalculationService {  get; set; }

    }
}
