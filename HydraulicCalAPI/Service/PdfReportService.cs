using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HydraulicCalAPI.Service
{
    public class FluidData
    {
        public double Solids { get; set; }
        public string DrillingFluidType { get; set; }
        public double BuoyancyFactor { get; set; }
        public string CuttingType { get; set; }
    }
    public class BhaTopToBottom
    {
        public int SerialNumber { get; set; }
        public string Weight { get; set; }
        public double Length { get; set; }
        public string UpperConnType { get; set; }
        public string LowerConnType { get; set; }
        public string FishNeckOD { get; set; }
        public string FishNeckLength { get; set; }
     }
    public class WorkStringData
    {
        public string wrkSectionName { get; set; }
        public string wrkWeight { get; set; }
        public double wrkLength { get; set; }
        public string wrkUpperConnType { get; set; }
    }
    public class CaseLinerTube
    {
        public string WellBoreSection { get; set; }
        public double WellTop { get; set; }
        public string WellBoreWeight { get; set; }
        public string Grade { get; set; }
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
        public string ProductLine { get; set; }
        public string SubProductLine { get; set; }
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
        public string Comments { get; set; }
       
        public HydraulicCalculationService HydraCalcService { get; set; }
        public List<CaseLinerTube> CasingLinerTubeData { get; set; }
        public List<WorkStringData> WorkStringItems { get; set; }
        public List<BhaTopToBottom> BHAToolItemData { get; set; }
        public List<FluidData> FluidItemData { get; set; }
    }
}
