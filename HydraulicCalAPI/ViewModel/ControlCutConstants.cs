using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WFT.Common;

namespace HydraulicCalAPI.ViewModel
{
    public static class ControlCutConstants
    {
        #region LookupConstants

        public const string WellboreSection = "WBORE";
        public const string WellCountry = "WCNTR";
        public const string CustomerType = "CSTYP";
        public const string WellClassification = "WLCLS";
        public const string Inclination = "INCLI";
        public const string WellPhysicalLocation = "WLPHY";
        public const string ReservoirType = "RSRVR";
        public const string WorkString = "WRKST";
        public const string RigType = "RGTYP";
        public const string WhipstockType = "WHSTK";
        public const string AnchorType = "ANCTY";
        public const string OrientationMethod = "ORNTN";
        public const string FormationTypeatExitPoint = "FRMTN";
        public const string HighLowSide = "HGLWS";
        public const string DrillingFluidType = "DRFLT";
        public const string WorkstringInfo = "WKSTF";
        public const string DrillCollarConnection = "DRCON";
        public const string CuttingsType = "CTTYP";
        public const string MillType = "MLTYP";
        public const string Size = "USIZE";
        public const string Grade = "GRADE";
        public const string WorkstringSection = "WKSSC";
        public const string ConnectionType = "CNCTN";
        public const string WorkstringGrade = "WKSGR";
        public const string TypeofRotarySystem = "TYRTR";
        public const string PostJobFSQ = "LHPRL";
        public const string JarManufacturer = "JRMFC";
        public const string TypeofJob = "JBTYP";
        public const string CompressiveStrengthofFormationAtExitPoint = "CPRSG";
        public const string Region = "WFRGN";
        public const string StateOrProvince = "STPRV";
        public const string Area = "WAREA";
        public const string ToolDescription = "TOODE";
        public const string Status = "STATU";
        public const string ToolsDescription = "TOOLD";
        public const string Country = "CNTRY";
        public const string MudType = "MUDTY";
        public const string MillODWear = "MIODW";
        public const string WeightOption = "WEIGHTOPT";
        public const string NozzleMaterialType = "ORFMTRL";
        public const string NozzleAre = "ORFTYPE";
        public const string OffBottomPressure = "OffBottomPressure";
        public const string OnBottomPressure = "OnBottomPressure";
        public const string OffBottomStringWeight = "OffBottomStringWeight";
        public const string OffBottomTorque = "OffBottomTorque";
        public const string UpWeight = "UpWeight";
        public const string DownWeight = "DownWeight";
        public const string ROPMinMax = "ROPMinMax";
        public const string RPMMinMax = "RPMMinMax";
        public const string MinMax = "MinMax";
        public const string MinMaxSeries = "Min/Max Series";
        public const string OverlayJob = "OverlayJob";
        //public const string DepthAxisField = "VerticalDepthHole";
        public const string TargetColorField = "LightGray";
        public const string DepthAxisField = "CalculatedDepth";
        //public const string RotationPerMinute = "AvgRotaryTorqueSurface";
        public const string RotationPerMinuteField = "AvgRotarySpeedSurface";
        public const string RotaryTorqueField = "AvgRotaryTorqueSurface";
        public const string QuickCut = "Quickcut";
        public const string ShallowAngleQuickCut = "Shallow Angle Quickcut";
        public const string RateOfPenetrationField = "RateOfPenetration";
        public const string RateOfPenetrationCalculatedField = "RateOfPenetrationCalculated";
        public const string RateOfPenetrationWithCompressionFactorField = "RateOfPenetrationWithCompressionFactor";
        public const string WeightOnBitField = "AvgWeightOnBitSurface";
        public const string StandpipePressureField = "StandpipePressure";
        public const string FlowRateField = "AvgMudFlowOut";
        public const string WeightOnMillField = "WeightOnMill";
        public const string RatholeFeetField = "RatholeFeet";
        public const string RatholeFeetCalculatedField = "RatholeFeetCalculated";
        public const string RatholeFeetWithCompressionFactorField = "RatholeFeetWithCompressionFactor";
        public const string MillingFeetField = "MillingFeet";
        public const string MillingFeetCalculatedField = "MillingFeetCalculated";
        public const string MillingFeetWithCompressionFactorField = "MillingFeetWithCompressionFactor";

        public const string DepthBitMeasuredField = "MeasuredDepthBit";
        public const string DepthBitVerticalField = "VerticalDepthBit";
        public const string DepthHoleMeasuredField = "MeasuredDepthHole";
        public const string DepthHoleVerticalField = "VerticalDepthHole";
        public const string BlockPositionField = "BlockPosition";
        public const string HookloadField = "AverageHookLoad";
        public const string MaxHookloadField = "MaximumHookLoad";
        public const string CasingPressureField = "CasingPressure";
        public const string PumpStrokeRate1Field = "PumpStrokeRate1";
        public const string PumpStrokeRate2Field = "PumpStrokeRate2";
        public const string PumpStrokeRate3Field = "PumpstrokeRate3";
        public const string MudFlowOutPercentField = "PercentMudFlowOut";
        public const string PumpStrokeCountField = "PumpStrokeCount";
        public const string MudFlowInField = "MudFlowIn";
        public const string KickOffPoint = "KickOffPoint";
        public const string TargetParametersSeries = "Target Parameters";
        public const string ActualSeries = "Actual ";
        public const string BingamPlastic = "Bingham Plastic";

        public const string CostUnderreamers = "Underreamers";
        public const string ConstSectionMill = "Section Mill";
        public const string OtherTool = "Other";
        public const string General = "General";
        public const string Mills = "Mills";
        public const string QuickCutLeadMill = "Mills - QuickCut Lead Mill";
        public const string Whipstock = "Whipstock";

        public const string New = "New";
        public const string NewJob = "New Job";
        public const string Success = "Success";
        public const string SuccessAsViewer = "Success as Viewer joining the job";
        public const string JobAlreadyRunning = "There is already a job running at the rig. Do you want to view live data?";
        public const string NoOneRequestedForData = "The data retrieval from the rig has not been started.";
        public const string JobRunningCannotStartANewJob = "There is already a job running at the rig. You cannot start/view another job now";
        public const string AlreadyListeningToTheJob = "You are already listening to the data from the rig for this job";
        public const string NotValidIPAddress = "Please enter a valid IP Address!";
        public const string NoDataOnIPAddress = "No Data available from this Address.";
        public const string MilledWindowLength = "MilledWindowLength";

        public const string WeightOptionNA = "NA";
        public const string JobResumeClassification = "JRCLS";
        public const string Segment = "SEGMENT";
        public const string ProductService = "PST";

        public const string OpenHoleAppln = "OHAPPLN";
        public const string ApplicationType = "APPLTYPE";
        public const string Application = "APPLN";
        public const string ServicePersonnelOnJobPosition = "SPOJP";
        public const string PerfromanceRating = "LHCDA";
        public const string PerfromanceRatingWithNumbers = "LHPRG";

        public const string TypeOfJob = "TOJOB";
        public const string Jarmanufacturer = "JAMAN";
        public const string TypeOfRotarySystem = "TOROS";
        public const string CertificationLevel = "CEFTL";
        public const string ChargeMethod = "CHRGE";
        public const string SupervisorStatus = "SUPST";
        public const string FishingOperation = "FISHO";
        public const string WellheadType = "FHWHT";
        public const string WellheadProfile = "FHWHP";
        public const string PdmType = "FHPDM";
        public const string MostToolType = "FHMTT";
        public const string CutAndPullSpearAssembly = "FHCPS";
        public const string AbandonmentType = "FHABT";
        public const string TgbRecovered = "FHTGB";
        public const string TypeOfMilling = "TOMIL";
        public const string CarbideType = "CARBT";
        public const string SectionMillType = "SMILT";
        public const string FishingSize = "FISIZ";
        public const string SubRegionMDMID = "SRMDM";
        public const string RegionMDMID = "REMDM";
        public const string GeozoneMDMID = "GZMDM";
        public const string HemisphereMDMID = "HSMDM";
        public const string SurfaceEquipmentType = "SURET";
        public const string PatchMaterial = "IPATMT";
        public const string PatchThickness = "IPATTH";
        public const string CylindersOnTool = "IPATCT";
        public const string TypeOfLeak = "IPATTL";
        public const string TypeOfPatch = "IPATTP";
        public const string PumpType = "PUMPT";
        public const string SwivelSize = "SWIVL";
        public const string ReverseJobType = "REVJT";
        public const string WellType = "REVWT";
        public const string CompressiveStrengthofFormation = "COMPST";
        public const string CentralizerType = "CENTTP";
        public const string WPTSReportIDLinkProduction = "http://wpts/trackingsystem/Default.aspx?ReportId=";
        public const string WPTSReportIDLinkTest = "http://usdcscaapppd001:5015/trackingsystem/Default.aspx?ReportId=";
        public const string WPTSMeetingLink = "http://wpts/TrackingSystem/Default.aspx";
        #endregion

        #region N/A
        public const string NotApplicable = "N/A";
        #endregion

        public class LookupWithUnitConversionConstants
        {

            public const string OD = "OUDIA";
            public const string WhipstockOD = "WHIOD";
            public const string UnitWeight = "UTWGT";
            public const string Weight = "WEGHT";
            public const string ConcaveAngle = "CNCVA";
            public const string FishingSize = "FISIZ";
            //public const string MillODWear = "MIODW";
            public static List<string> ConstantsList = new List<string>() { OD, WhipstockOD, UnitWeight, Weight, ConcaveAngle, FishingSize };
            public static string GetUnitSystemAttributeForConstants(string constantName)
            {
                string attributeType = "";
                switch (constantName)
                {
                    case OD:
                        attributeType = UnitSystemAttributes.Size;
                        break;
                    case FishingSize:
                        attributeType = UnitSystemAttributes.Size;
                        break;
                    case WhipstockOD:
                        attributeType = UnitSystemAttributes.Size;
                        break;
                    case UnitWeight:
                        attributeType = UnitSystemAttributes.WeightPerLength;
                        break;
                    case Weight:
                        attributeType = UnitSystemAttributes.Weight;
                        break;
                    case ConcaveAngle:
                        attributeType = UnitSystemAttributes.Angle;
                        break;
                    default:
                        break;
                }
                return attributeType;
            }
        }

        public class DrawingLocation
        {
            public const string Assembly1 = "Assembly 1";
            public const string Assembly2 = "Assembly 2";
        }


        public enum Weights
        {
            UpWeight,
            DownWeight
        }
        public enum SalesRental
        {
            Sales,
            Rental
        }
        public enum PrimaryBackUp
        {
            Primary,
            BackUp
        }
        public enum RiskCode
        {
            PNOJ,
            PSRP,
            PFT,
            PMQP,
            POQP,
            PCWD,
            PPTL,
            PRTR,
            PJT,
            RRR,
            RLCM,
            RMW,
            RT1,
            RT2,
            RLW,
            RMI,
            RTD,
            SLSO,
            CLC,
            CRC,
            CRC1,
            SPTS,
            SPWS,
            SPDTL,
            SPLTP
        }
        public enum JobState
        {
            View,
            Copy,
            None
        }

        public enum AssemblyReferencePointName
        {
            DistanceToExposePackerActuator = 1,
            DistanceToRemoveRSM,
            DistanceToRemovePBRPackoff,
            DistanceFromPackerSlipsToTopOfLiner,
            DistanceFromPackerElementToTopOfLiner,
            DistanceFromHangerSlipsToTOL,
            DistanceFromHolddownSubSlipsToTOL,
            PBRIDLengthStart,
            PBRIDLengthEnd,
            FJBCheckFreeDistanceStart,
            FJBCheckFreeDistanceEnd
        }
        public enum JobStatusType
        {
            PlayBackJobData,
            CurrentDepth,
            Security,
            UserRole,
            MasterData,
            MasterDataLoadingFailure,
            SuddenChangeInTorque,
            JobInformation,
            SavingJob,
            ApplyingJobChanges,
            RecordUpWeight,
            RecordDownWeight,
            BaseLineHookLoad,
            PipeMarking,
            MillingInformation,
            UpdatingMillingData,
            CentralJobsearch,
            ROPInterval,
            CustomerLogo,
            Overlay,
            SeviceNotAvailable,
            JobDocuments,
            DownloadJobOffline,
            UploadToWPTS,
            JobType,
            DepthTarget,
            RiskScore,
            UserAddMsg,
            WptsUploadRequiredFields,
            RealTimeRelatedInfo,
            FetchWellDetails,
            ManageProductLine,
            UploadToServer,
            OfflineMasterDataSync,
            PackageGeneration,
            SorGeneration
        }

        public class PSRPDocumentPhaseNames
        {
            public const string JobPlanning = "Job Planning";
            public const string EquipmentTesting = "Equipment Testing";
            public const string EquipmentDispatch = "Equipment Dispatch";
            public const string JobClosure = "Job Closure";
            public const string EquipmentSelection = "Equipment Selection";
            public const string EquipmentAssembly = "Equipment Assembly";
            public const string InstallationAndServicing = "Installation & Servicing";

        }

        public class PSRPDocumentType
        {
            public const string Documents = "Documents";
            public const string StandardProcedures = "Standard Procedures";
            public const string DynamicChecklist = "Dynamic Checklist";
            public const string Procedure = "Procedure";

        }
        public enum MudTypes
        {
            WaterBased,
            OilBased,
            Polymer,
            PureWater
        }

        public enum AssyConnection
        {
            Box,
            Pin
        }

        public enum PlayOptions
        {
            Play,
            Pause,
            Stop,
            Rewind,
            Forward

        }
        public enum ImpotJobFromCentralNotification
        {
            JobSaved,
            MillingDataSaved,
            JobSavingError,
            MillingDataSavedPartially,
            MillingDataSavingIsInProcess,
            MillingDataNotAvailable,
            JobDocumentSavingInProcess,
            JobDocumentSavingError,
            JobDocumentSaved,
            JobDocumentNotAvailable
        }

        public enum JobStatusLevels
        {
            [StringValue("")]
            NotApplicable = 0,
            [StringValue("Job Planning")]
            JobPlanning = 1,
            [StringValue("Equipment Selection")]
            EquipmentSourcing = 2,
            [StringValue("Equipment Assembly")]
            EquipmentPreparation = 3,
            [StringValue("Equipment Dispatch")]
            EquipmentDispatch = 5,
            [StringValue("Installation and Servicing")]
            JobInstalltionExecution = 6,
            [StringValue("Job Closure")]
            JobClosure = 7

        }

        public enum MasterDataSyncFromCentralNotification
        {
            CustomerFetching,
            CustomerFetchingCompleted,
            CustomerFetchingError,
            LookUpFetching,
            LookUpFetchingCompleted,
            LookUpFetchingError,
            WftLocationFetching,
            WftLocationFetchingCompleted,
            WftLocationFetchingError,
            WptsUserFetching,
            WptsUserFetchingCompleted,
            WptsUserFetchingError,
            MasterDataSaving,
            MasterDataSavingCompleted,
            MasterDataSavingError,
            SecurityUsersFetching,
            SecurityUsersFetchingCompleted,
            SecurityUsersFetchingError,
            SecurityUsersDataSaving,
            SecurityUsersDataSavingCompleted,
            SecurityUsersDataSavingError,
            CustomerLogosDataFetching,
            CustomerLogosDataFetchingCompleted,
            CustomerLogosDataFetchingError,
            CustomerLogosDataSaving,
            CustomerLogosDataSavingCompleted,
            CustomerLogosDataSavingError,
            ProcessFetching,
            ProcessFetchingCompleted,
            ProcessFetchingError,
            CheckListFetching,
            CheckListFetchingCompleted,
            CheckListFetchingError,
            RiskFetching,
            RiskFetchingCompleted,
            RiskFetchingError,
            ProcedureTemplateFetching,
            ProcedureTemplateFetchingCompleted,
            ProcedureTemplateFetchingError,
            ChartOperationsDataFetching,
            ChartOperationsDataFetchingCompleted,
            ChartOperationsDataFetchingError,
            AMSFetching,
            AMSFetchingCompleted,
            AMSFetchingError,
            DocumentsSavingError,
        }

        public enum QFSDataSyncFromCentralNotification
        {
            QFSFetching,
            QFSFetchingCompleted,
            QFSFetchingError,

        }
        public enum AMSDataSyncFromSharePointNotification
        {
            AMSFetchingSP,
            AMSFetchingCompletedSP,
            AMSFetchingErrorSP,
            AMSDocumentFetching,
            AMSDocumentFetchingCompleted,
            AMSDocumentFetchingError,
            SPSaving,
            SPSavingCompleted,
            SPSavingError,
            DownloadDocument,
            DownloadDocumentCompleted,
            DownloadDocumentError
        }
        public enum ProductLines
        {
            CasingExits,
            Fishing,
            LinerHangers,

        }
        #region other constants
        public const string CommunicationChannelNameWell = "Well Communication Setup";
        public const string CommunicationChannelNameCementingTruck = "CementingTruck";
        public const string ProductLine = "Casing Exits";
        public const string Operation = "Whipstocks / Casing Exit - New";
        public const string WaterBased = "Water Based";
        public const string SelectOne = "<--Select One-->";
        #endregion
        #region AccusetC
        public const string Water_075 = "Water_0.75";
        public const string Water_0875 = "Water_0.875";
        public const string Water_1 = "Water_1";
        public const string Water_1125 = "Water_1.125";
        public const string Water_125 = "Water_1.25";
        public const string Water_0545 = "Water_0.545";
        public const string Water_0575 = "Water_0.575";
        public const string Water_0605 = "Water_0.605";
        public const string Mud_075 = "Mud_0.75";
        public const string Mud_0875 = "Mud_0.875";
        public const string Mud_1 = "Mud_1";
        public const string Mud_1125 = "Mud_1.125";
        public const string Mud_125 = "Mud_1.25";
        public const string Mud_0545 = "Mud_0.545";
        public const string Mud_0575 = "Mud_0.575";
        public const string Mud_0605 = "Mud_0.605";
        #endregion
        public class DocumentReferances
        {
            public const string CompletedWorkshopJobPackage = "Completed Workshop Job Package";
            public const string InspectionSheet = "Inspection Reports";
            public const string TestReports = "Test Reports";
            public const string PrimaryAssembly = "Primary-Assembly";
            public const string BackupAssembly = "Backup-Assembly";
            public const string Assembly = "Assembly";
            public const string SubAssembly1 = "Sub Assembly 1";
            public const string SubAssembly2 = "Sub Assembly 2";
            public const string SubAssembly3 = "Sub Assembly 3";
            public const string Loose = "Loose";
            public const string Other = "Other";
            public const string AssemblyMeasurementSheet = "Assembly Measurement Sheet";
            public const string WorkshopAssemblyProcedureSignOffCheckSheet = "Workshop Assembly Procedure/Sign-off Check sheet";
            public const string AssemblyPressureShearTest = "Assembly Pressure/Shear Test Charts";
            public const string AssemblyTorqueCharts = "Assembly Torque Charts";
            public const string LinerPreDispatchCheckList = "Liner Pre-Dispatch/  Loose Equipment Checklist";
            public const string DeliveryTicket = "Delivery Ticket";
            public const string LiftingEquipmentCertificates = "Lifting Equipment Certificates";
            public const string ThirdPartyInspectorCustomerReleaseNote = "3rd Party Inspector Customer Release Note";
            //public const string CompletedFieldJobPackageChecklist = "Field package Checklist";
            public const string StatementofRequirement = "Statement of Requirements";
            public const string JobBriefDebriefForm = "Pre Job Brief";
            public const string DeliveryTicketEquipmentList = "Delivery Ticket";
            public const string FieldArrivalChecklist = "Field Arrival Checklist";
            public const string SurfaceEquipmentChecklist = "Surface Equipment Pre-Dispatch Checklist";
            public const string TaskRiskAssessment = "Task Risk Assessment Worksheet";
            public const string RiskAssessment = "Risk Assessment";
            public const string FieldTaskRiskAssessment = "Field Task Risk Assessment (TRA)";
            public const string GeneralFieldTaskRiskAssessment = "General Field Task Risk Assessment (TRA)";
            public const string DispatchBackloadList = "Dispatch & Backload List";
            public const string JobResume = "Job Resume";
            public const string CustomrCounts = "Sign Off - Job Brief/Debrief";
            public const string GlobalFirstAlert = "Global First Alert";
            public const string RunningProceduresTechUnits = "Running Procedures / Tech Units";
            public const string StringHookUpDrawing = "String/Hook-Up Drawing";
            public const string TorqueDragSimulation = "Torque & Drag Simulation";
            public const string HydraulicsSimulation = "Hydraulics Simulation";
            public const string CentralizerPlacementSimulation = "Centralizer Placement Simulation";
            public const string RunnningProcedure = "Running Procedure";
            public const string BlankForms = "Blank Forms";
            public const string JobsSimulations = "Jobs Simulations";
            public const string DataSheet = "Data Sheet";
            public const string DimensionalInspectionSheets = "Dimensional Inspection Sheets Sales Equipment & Rental Tools";
        }
        public class TechnicalName
        {
            public const string LiftSubBP = "Lift Sub";
            public const string HandlingSub = "Handling  Sub";
            public const string RetrievablePackOff = "Retrievable Packoff";
            public const string RSM = "RSM";
            public const string Stinger = "Stinger";
            public const string ReamerShoe = "Reamer Shoe";
            public const string ReamerTShoe = "Reamer\t Shoe";
            public const string Surgemaster = "Surgemaster";
            public const string DiverterTool = "Diverter Tool";
            public const string CementedHead = "Cemented Head";
            public const string CementingHead = "Cementing Head";
            public const string MXBallSeat = "MX Ball Seat";
            public const string MechanicalBallSeat = "Mechanical Ball Seat";
            public const string MBS = "MBS";
            public const string BallSeat = "Ball Seat";
            public const string MX = "MX";
            public const string SubBP = "Sub, B-P";
            public const string Sub = "Sub";
        }
        public class ModleDesignation
        {
            public const string RSM = "RSM";
            public const string RKC = "RKC";
        }
        public class PDMSLICESDocumentPaths
        {
            public const string Datasheet = @"PDMSLICES\DataSheet\";
            public const string Images = @"PDMSLICES\Images\";
            public const string InspectionSheets = @"PDMSLICES\DimensionSheet\";
            public const string TechnicalSheets = @"PDMSLICES\TssSheet\";
            public const string SharpointFiles = @"SharePoint\";

        }
        public class Sections
        {

            public const string Section1 = "Section 1";
            public const string Section2 = "Section 2";
            public const string Section3 = "Section 3";
            public const string Section4 = "Section 4";
            public const string Section5 = "Section 5";
            public const string Section6 = "Section 6";
            public const string Section7 = "Section 7";
            public const string Section8 = "Section 8";
            public const string Section9 = "Section 9";
            public const string Section10 = "Section 10";
            public const string Section11 = "Section 11";
            public const string Section12 = "Section 12";
            public const string Section13 = "Section 13";


        }
        public class PackageTypes
        {
            public const string Workshop = "Workshop";
            public const string Field = "Field";

        }
        public class SectionTypes
        {
            public const string Other = "Other";
            public const string Loose = "Loose";
            public const string Assembly = "Assembly";
            public const string Review = "Review";
            public const string SubAssembly = "Sub Assembly";
        }
        public class SubSectionName
        {
            public const string AssemblyMeasurementSheet = "- (As Built) Assembly Measurement Sheet";
            public const string WorkshopAssemblyProcedure = "- Workshop Assembly Procedure /Sign-Off Check Sheet";
            public const string DimensionalInspectionSheets = "- Dimensional Inspection Sheets Sales Equipment & Rental Tools";
            public const string AssemblyTorqueCharts = "- Assembly Torque Charts";
            public const string AssemblyPressureShearTestCharts = "- Assembly Pressure/Shear Test Charts (if applicable)";
            public const string LoosePressureShearTestCharts = "- Pressure/Shear Test Charts (if applicable)";
            public const string DataSheet = "- DataSheet";
        }
        public class SectionNames
        {
            public const string CoverPage = "Cover Page";
            public const string ReviewPage = "Review Page";
            public const string WorkshopJobOrder = "Workshop Job Order";
            public const string Section3 = "Primary Assembly Information";
            public const string Section4 = "Backup Assembly Information";
            public const string Section5 = "Sub Assembly 1 Information";
            public const string Section6 = "Sub Assembly 2 Information";
            public const string Section7 = "Sub Assembly 3 Information";
            public const string Section8 = "Loose Equipment Information";
            public const string PreDispatchLooseChecklist = "Liner Pre-Dispatch/  Loose Equipment Checklist";
            public const string SurfaceEquipmentChecklist = "SURFACE EQUIPMENT PRE-DISPATCH CHECKLIST";
            public const string PreDispatchLooseChecklistDetails = "PreDispatchChecklist";
            public const string DeliveryTicket = "Delivery Ticket";
            public const string LiftingEquipmentCertificates = "Lifting Equipment Certificates";
            public const string ThirdPartyInspectorCustomerReleaseNote = "3rd Party Inspector Customer Release Note";
            public const string AdditionalInformation = "Additional Information";
            public const string DownloadChecklist = "Download Template";
            public const string GenerateChecklist = "Generate Template";
            public const string CompletedFieldJobPackageChecklist = "Completed Field Job Package Checklist";
            public const string StatementofRequirement = "Statement of Requirements";
            public const string JobBriefDebriefForm = "Job Brief/Debrief Form";
            public const string DeliveryTicketEquipmentList = "Delivery Ticket/Equipment List";
            public const string CompletedWorkshopJobPackage = "Completed Workshop Job Package";
            public const string RunningProceduresTechUnits = "Running Procedures / Tech Units";
            public const string StringHookUpDrawing = "String/Hook-Up Drawing";
            public const string TorqueDragSimulation = "Torque & Drag Simulation";
            public const string HydraulicsSimulation = "Hydraulics Simulation";
            public const string CentralizerPlacementSimulation = "Centralizer Placement Simulation";
            public const string FieldArrivalChecklist = "Field Arrival Checklist";
            public const string TaskRiskAssessment = "Task Risk Assessment Worksheet";
            public const string RiskAssessment = "Risk Assessment";
            public const string RunnningProcedure = "Running Procedure";
            public const string BlankForms = "Blank Forms";
            public const string JobsSimulations = "Jobs Simulations";
            public const string Index = "Index Page";


        }
        public class EntityConstants
        {
            public const string Job = "Job";
            public const string WITSData = "WITS Data";
            public const string Chat = "Chat";
            public const string QuickfactData = "Quickfact Data";
            public const string Notifications = "Notifications";
            public const string WPTSMasterData = "WPTS Master Data";
            public const string Comments = "Comments";
            public const string Overlay = "Overlay";
            public const string MarkPipe = "Mark Pipe";
            public const string OfOnBottomParameters = "Off On Bottom Parameters";
            public const string PlayJob = "Replay";
            public const string WPTSJob = "WPTS Job";
            public const string ChartTargetParameters = "Chart Target Parameters";
            public const string ChartTemplates = "Chart Templates";
            public const string ProcessCategory = "Process Category";

            public const string ManageProductLine = "Product Line";
            public const string JobPlanningPhase = "Job Planning Phase";
            public const string EquipmentSelectionPhase = "Equipment Selection Phase";
            public const string EquipmentAssemblyPhase = "Equipment Assembly Phase";
            public const string EquipmentTestingPhase = "Equipment Testing Phase";
            public const string EquipmentDispatchPhase = "Equipment Dispatch Phase";
            public const string InstallationandServicePhase = "Installation and Service Phase";
            public const string JobClosurePhase = "Job Closure Phase";
            public const string JobPlanningTasksTab = "Job Planning Tasks Tab";
            public const string WorkshopTasksTab = "Workshop Tasks Tab";
            public const string InstallationandServicingTaskTab = "Installation and Servicing Task Tab";

            public const string Equipment = "Equipment";
            public const string FieldPackage = "Field Package";
            public const string QRCode = "QRCode";
            public const string EOWReport = "EOW Report";
            public const string JobGeneralInfo = "Job General Info";
            public const string WellInfo = "Well Info";
            public const string Risk = "Risk";
            public const string Responsibilities = "Responsibilities";
            public const string DocumentTemplate = "Document Template";
            public const string WorkshopPackage = "Workshop Package";
            public const string PSRPDocument = "PSRP Document";
            public const string AdditionalDocument = "Additional Document";
            public const string PostJob = "Post Job";
            public const string RealTime = "RealTime";
            public const string TorqueDrag = "Torque Drag";
            public const string JobResume = "Job Resume";
            public const string EquipmentSerialNumber = "Equipment Serial Number";
            public const string RunningProcedure = "Running Procedure";
            public const string AssemblyProcedure = "Assembly Procedure";
            public const string SurfaceEquipmentChecklist = "Surface Equipment Pre-Dispatch Checklist";
            public const string FieldArivalChecklist = "Field Arival Checklist";
            public const string LinerPredispatchChecklist = "Liner Predispatch Checklist";
            public const string PSRPMaster = "PSRPMaster";

        }

        public class OverrideFrequencyUnit
        {
            public const string Never = "Never";
            public const string Hour = "Hours";
            public const string Minute = "Mins";
        }

        public class WitsConfiguratorOperations
        {
            public const string Nothing = "";
            public const string Offset = "Offset";
            public const string Override = "Override";
        }
        public enum JobCriticalityStatus
        {
            [StringValue("")]
            NotApplicable = 0,
            [StringValue("Low")]
            Low = 1,
            [StringValue("Medium")]
            Medium = 2,
            [StringValue("High")]
            High = 3,
            [StringValue("Severe")]
            Severe = 4
        }

        public class RiskConsequence
        {
            public int ID { get; set; }
            public string Value { get; set; }

        }
        public static System.Collections.ObjectModel.ObservableCollection<RiskConsequence> GetRiskConsequence()
        {
            System.Collections.ObjectModel.ObservableCollection<RiskConsequence> reutrnList = new System.Collections.ObjectModel.ObservableCollection<RiskConsequence>();
            reutrnList.Add(new RiskConsequence() { ID = 0, Value = "N/A" });
            reutrnList.Add(new RiskConsequence() { ID = 1, Value = "C1 - Slight" });
            reutrnList.Add(new RiskConsequence() { ID = 2, Value = "C2 - Minor" });
            reutrnList.Add(new RiskConsequence() { ID = 3, Value = "C3 - Serious" });
            reutrnList.Add(new RiskConsequence() { ID = 4, Value = "C4 - Major" });
            reutrnList.Add(new RiskConsequence() { ID = 5, Value = "C5 - Catastrophic" });
            return reutrnList;
        }
        public class RiskProbability
        {
            public int ID { get; set; }
            public string Value { get; set; }

        }
        public static System.Collections.ObjectModel.ObservableCollection<RiskProbability> GetRiskProbability()
        {
            System.Collections.ObjectModel.ObservableCollection<RiskProbability> reutrnList = new System.Collections.ObjectModel.ObservableCollection<RiskProbability>();
            reutrnList.Add(new RiskProbability() { ID = 0, Value = "N/A" });
            reutrnList.Add(new RiskProbability() { ID = 1, Value = "P1-Remote" });
            reutrnList.Add(new RiskProbability() { ID = 2, Value = "P2- Unlikely" });
            reutrnList.Add(new RiskProbability() { ID = 3, Value = "P3-Possible" });
            reutrnList.Add(new RiskProbability() { ID = 4, Value = "P4-Likely" });
            reutrnList.Add(new RiskProbability() { ID = 5, Value = "C5 - Catastrophic" });
            return reutrnList;
        }
        public class ActionConstants
        {
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
            public const string View = "View";
            public const string Assign = "Assign";
            public const string SignOff = "SignOff";
            public const string ApprovalLevel1 = "ApproveLevel 1";
            public const string ApprovalLevel2 = "ApproveLevel 2";
            public const string ApprovalLevel3 = "ApproveLevel 3";
            public const string ApprovalLevel4 = "ApproveLevel 4";
            public const string ApprovalLevel5 = "ApproveLevel 5";
            public const string Lock = "Lock";
            public const string Download = "Download";
            public const string Upload = "Upload";
        }

        public static string GetApproveLevel(int? JobApproveLevel)
        {
            if (JobApproveLevel == null)
            {
                return ActionConstants.ApprovalLevel1;
            }
            else
            {
                var tempLevel = JobApproveLevel.Value + 1;
                return "ApproveLevel " + tempLevel;
            }
        }
        public static List<string> GetApproveLevelList()
        {
            List<string> ApproveList = new List<string>();
            ApproveList.Add(ActionConstants.ApprovalLevel1);
            ApproveList.Add(ActionConstants.ApprovalLevel2);
            ApproveList.Add(ActionConstants.ApprovalLevel3);
            ApproveList.Add(ActionConstants.ApprovalLevel4);
            ApproveList.Add(ActionConstants.ApprovalLevel5);
            return ApproveList;
        }
        public class RoleConstants
        {
            public const string Administrator = "Administrator";
            public const string Editor = "Editor";
            public const string Observer = "Observer";
            public const string SME = "SME";
            public const string SuperUser = "SuperUser";
            public const string Supervisor = "Supervisor";
            public const string JobPlanner = "Job Planner";
        }
        public class PrivilegeConstants
        {
            public const string Notmapped = "Not mapped";
            public const string Noaccess = "None";
            public const string User = "User";
            public const string Unit = "Unit";
            public const string Fullaccess = "All";
        }

        public class ImageTypeConstants
        {
            public const string Mills = "Mills";
            public const string Whipstock = "Whipstock";
            public const string Others = "Others";
        }
        public class UnitSystemAttributes
        {
            public const string Size = "Size";
            public const string Depth = "Depth";
            public const string Pressure = "Pressure";
            public const string Temperature = "Temperature";
            public const string Weight = "Weight";
            public const string Capacity = "Capacity";
            public const string Volume = "Volume";
            public const string FluidDensity = "Fluid Density";
            public const string FlowRate = "Flow Rate";
            public const string Area = "Area";
            public const string RateOfPenetration = "RateOfPenetration";
            public const string Force = "Force";
            public const string RotationalSpeed = "Rotational Speed";
            public const string Torque = "Torque";
            public const string StrokeRate = "StrokeRate";
            public const string WeightPerLength = "Weight/Length";
            public const string Viscosity = "Viscosity";
            public const string YieldPoint = "YieldPoint";
            public const string Angle = "Angle";
            public const string MaxDogLegPassedThru = "MaxDogLegPassedThru";
            public const string AnnularVelocity = "AnnularVelocity";
            public const string TubularVelocity = "TubularVelocity";
            public const string Power = "Power";
            public const string Conductivity = "Conductivity";
            public const string WITSForce = "WITSForce";
            public const string Other = "Other";
            public const string GelStrength = "GelStrength";
            public const string DrillingFluids = "Drilling Fluids";
            public const string GasPressure = "Gas Pressure";
            public const string StandpipePressure = "Standpipe Pressure";
            public const string WeightOnMill = "Weight On Mill";
        }

        public class QFSStageConstants
        {
            public const string ODBladeStartsToCut = "ODBladeStartsToCut";
            public const string ODBladeStartsToCutOut = "ODBladeStartsToCutOut";
            public const string MillStartsToCut = "MillStartsToCut";
            public const string PilotBladeStartsToCut = "PilotBladeStartsToCut";
            public const string MillStartsToCutOut = "MillStartsToCutOut";
            public const string PilotBladeStartsToCutOut = "PilotBladeStartsToCutOut";
            public const string DistanceToMaxDeflection = "DistanceToMaxDeflection";
            public const string StartOfRetrievalSlot = "StartOfRetrievalSlot";
            public const string EndOfLug = "EndOfLug";
            public const string EndOfRetrievalSlot = "EndOfRetrievalSlot";
            public const string StartOfCorePoint = "StartOfCorePoint";
            public const string MiddleOfCorePoint = "MiddleOfCorePoint";
            public const string EndOfCorePoint = "EndOfCorePoint";
            public const string KickOffPoint = "KickOffPoint";
            public const string MilledWindowLength = "MilledWindowLength";

        }

        public class MasterDataConstants
        {
            public const string CustomerDetails = "CustomerDetails";
            public const string UserDetails = "UserDetails";
            public const string LocationDetails = "LocationDetails";
            public const string LookupDetails = "LookupDetails";
            public const string SurfaceEquipmentDetails = "SurfaceEquipmentDetails";
            public const string NozzleMasterDataDetails = "NozzleMasterDataDetails";
            public const string AccusetSystemDetails = "AccusetSystemDetails";
            public const string UnderreamerDetails = "UnderreamerDetails";
            public const string MillsDetails = "MillsDetails";
            public const string RolesModelDetails = "RolesModelDetails";
            public const string CasingSizeAndWeight = "CasingSizeANdWeight";
            public const string MasterToolList = "MasterToolList";
            public const string ProcessDocProductLineList = "ProcessDocProductLineList";
            public const string ChecklistList = "ChecklistList";
            public const string AMSList = "AMSList";
            public const string ProcedureTemplateList = "ProcedureTemplate";
            public const string ChartOperationList = "ChartOperation";
            public const string RiskList = "RiskList";
        }

        //public class RoleTypeConstants
        //{
        //    public const string CompanyMan = "Company Man";
        //    public const string Engineer = "Engineer";
        //    public const string EDRContact = "EDR Contact";
        //    public const string DrillingEngineer = "Drilling Engineer";
        //    public const string Supervisor = "Supervisor";
        //    public const string SME = "SME";
        //    public const string AccuviewSupport = "Accuview Support";

        //}
        public enum CommonLookupEnum
        {
            WCNTR = 0,
            CSTYP = 1,
            WLCLS = 2,
            INCLI = 3,
            WLPHY = 4,
            RSRVR = 5,
            WRKST = 6,
            RGTYP = 7,
            WBORE = 8,
            WHSTK = 9,
            CNCVA = 10,
            ANCTY = 11,
            ORNTN = 12,
            FRMTN = 13,
            HGLWS = 14,
            DRFLT = 15,
            WKSTF = 16,
            DRCON = 17,
            CTTYP = 18,
            MLTYP = 19,
            USIZE = 20,
            UTWGT = 21,
            GRADE = 22,
            WKSSC = 23,
            OUDIA = 24,
            WEGHT = 25,
            CNCTN = 26,
            TYRTR = 27,
            JRMFC = 28,
            JBTYP = 29,
            CPRSG = 30,
            WFRGN = 31,
            STPRV = 32,
            WAREA = 33,
            WHIOD = 34,
            MIODW = 35,
            TOODE = 36,
            STATU = 37,
            CNTRY = 38,
            MUDTY = 39,
            WEIGHTOPT = 40,
            ORFTYPE = 41,
            ORFMTRL = 42,
            JRCLS = 43,
            SEGMENT = 44,
            PST = 45,
            LHWRN = 46,
            LHPRG = 47,
            LHLTE = 48,
            LHBSL = 49,
            LHSBM = 50,
            LHCPT = 51,
            LHTPI = 52,
            LHCPC = 53,
            LHKPM = 54,
            LHTMU = 55,
            LHPWT = 56,
            LHPLH = 57,
            LHPID = 58,
            LHPSS = 59,
            LHPTE = 60,
            LHHTE = 61,
            LHSST = 62,
            LHPPT = 63,
            LHLCT = 64,
            LHFCL = 65,
            LHFCM = 66,
            LHSTE = 67,
            LHSNT = 68,
            LHITE = 69,
            LHOHM = 70,
            LHCTT = 71,
            LHCTM = 72,
            LHSES = 73,
            LHBSM = 74,
            LHRTE = 75,
            LHMFD = 76,
            LHFMU = 77,
            LHRWL = 78,
            LHLRH = 79,
            LHSBO = 80,
            LHSBG = 81,
            LHBPD = 82,
            LHHSM = 83,
            LHRTT = 84,
            LHPOT = 85,
            LHCCO = 86,
            LHCDB = 87,
            LHDFD = 88,
            LHCAA = 89,
            LHMTW = 90,
            LHPST = 91,
            LHCBA = 92,
            LHDOC = 93,
            LHCCW = 94,
            LHSTT = 95,
            LHCHT = 96,
            LHLCP = 97,
            LHOHZ = 98,
            LHPRL = 99,
            LHWAR = 100,
            LHPBY = 101,
            LHLRT = 102,
            LHDPC = 103,
            LHLRR = 104,
            LHLHS = 105,
            LHSPC = 106,
            LHRTR = 107,
            LHRTP = 108,
            LHBSF = 109,
            LHBSP = 110,
            LHCWP = 111,
            LHCWC = 112,
            LHCWD = 113,
            LHCDP = 114,
            LHLRP = 115,
            LHCLP = 116,
            LHTPP = 117,
            LHTPC = 118,
            LHLPT = 119,
            LHLPP = 120,
            LHACR = 121,
            LHACP = 122,
            LHTDP = 123,
            LHCTY = 124,
            LHPAS = 125,
            LHSET = 126,
            LHLMT = 127,
            LHLCM = 128,
            LHHML = 129,
            LHCRR = 130,
            LHWCI = 131,
            LHTOG = 132,
            LHTPA = 133,
            LHDFL = 134,
            LHCDA = 135,
            ELHDC = 136,
            ELHBL = 137,
            ELHEB = 138,
            ELHSB = 139,
            ELHSM = 140,
            ELHSG = 141,
            ELHSP = 142,
            ELHWC = 143,
            ELHRR = 144,
            ELHTPC = 145,
            ELHKTD = 146,
            ELHKLR = 147,
            ELHLRPC = 148,
            ELHKCW = 149,
            ELHCWPC = 150,
            ELHWCPC = 151,
            ELHKHS = 152,
            ELHHS = 153,
            ELHRTR = 154,
            ELHKRT = 155,
            ELHKLT = 156,
            ELHPTS = 157,
            ELHKRA = 158,
            ELHSKP1 = 159,
            ELHSPC1 = 160,
            ELHSKP2 = 161,
            ELHSPC2 = 162,
            LHTOT = 163,
            LHSEN = 164,
            LHREN = 165,
            LHTHN = 166,
            LHPCS = 167,
            LHPCW = 168,
            LHPCG = 169,
            LHDPS = 170,
            LHDPW = 171,
            LHDPG = 172,
            LHDPT = 173,
            LHCRW = 174,
            LHEFP = 175,
            LHEDSR = 176,
            OHAPPLN = 177,
            APPLTYPE = 178,
            APPLN = 179,
            AMSJT = 180,
            AMSRT = 181,
            AMSBST = 182,
            AMSWP = 183,
            AMSDB = 184,
            AMSPA = 185,
            AMSCMM = 186,
            AMSHM = 187,
            SPOJP = 188,
            TOOLD = 189,
            TOJOB = 190,
            JAMAN = 191,
            TOROS = 192,
            CEFTL = 193,
            CHRGE = 194,
            SUPST = 195,
            FISHO = 196,
            FHWHT = 197,
            FHWHP = 198,
            FHPDM = 199,
            FHMTT = 200,
            FHCPS = 201,
            FHABT = 202,
            FHTGB = 203,
            TOMIL = 204,
            CARBT = 205,
            SMILT = 206,
            FISIZ = 207,
            LHWODETP = 208,
            LHWOTPI = 209,
            LHWOSET = 210,
            LHWORET = 211,
            HSMDM = 212,
            GZMDM = 213,
            REMDM = 214,
            SRMDM = 215,
            DRWLC = 216,
            SALTHR = 217,
            RENTHR = 218,
            SURET = 219,
            IPATMT = 220,
            IPATTH = 221,
            IPATCT = 222,
            IPATTL = 223,
            IPATTP = 224,
            PUMPT = 225,
            SWIVL = 226,
            REVJT = 227,
            REVWT = 228,
            COMPST = 229,
            CENTTP = 230
        }
        public enum JobType
        {
            [StringValue("All")]
            All = 0,
            [StringValue("FRE Casing Exits")]
            FRECasingExits = 1,
            [StringValue("FRE OH Fishing")]
            FREOHFishing = 2,
            [StringValue("Standard Liner Hanger")]
            StandardLinerHanger = 3,
            [StringValue("Expandable Liner Hanger")]
            ExpandableLinerHanger = 4,
            [StringValue("Second Run Isolation")]
            SecondRunIsolation = 5,
            [StringValue("Tieback Liner Hanger")]
            TiebackLinerHanger = 6,
            [StringValue("FRE CH Fishing")]
            FRECHFishing = 7,
            [StringValue("FRE Milling")]
            FREMilling = 8,
            [StringValue("FRE Impact Tools")]
            FREImpactTools = 9,
            [StringValue("Well Abandonment")]
            WellAbandonment = 10,
            [StringValue("FRE Fishing")]
            FREFishing = 11,
        }

        public enum RunType
        {
            [StringValue("All")]
            All = 0,
            [StringValue("Fishing")]
            Fishing = 1,
            [StringValue("CE_Gauge")]
            CE_Gauge = 2,
            [StringValue("CE_SettingMilling")]
            CE_SettingMilling = 3,
            [StringValue("CE_WhipstockRetrieval")]
            CE_WhipstockRetrieval = 4
        }

        public enum Risk
        {

        }

        public enum ColorStrength
        {
            Transparent,
            Green,
            Yellow,
            Red
        }
        public enum RigData
        {
            MeasuredDepthBit = 108,
            VerticalDepthBit = 109,
            MeasuredDepthHole = 110,
            VerticalDepthHole = 111,
            BlockPosition = 112,
            RateOfPenetration = 113,
            AverageHookLoad = 114,
            MaxHookload = 115,
            AvgWeightOnBitSurface = 116,
            MaxWeightOnBitSurface = 117,
            AvgRotaryTorqueSurface = 118,
            MaxRotaryTorqueSurface = 119,
            AvgRotarySpeedSurface = 120,
            StandpipePressure = 121,
            CasingPressure = 122,
            PumpStrokeRate1 = 123,
            PumpStrokeRate2 = 124,
            PumpstrokeRate3 = 125,
            ActiveTankVolume = 126,
            TankVolChange = 127,
            PercentMudFlowOut = 128,
            AvgMudFlowOut = 129,
            AvgMudFlowIn = 130,
            AvgMudDensityOut = 131,
            AvgMudDensityIn = 132,
            AvgMudTempOut = 133,
            AvgMudTempIn = 134,
            AvgMudConductivityOut = 135,
            AvgMudConductivityIn = 136,
            PumpStrokeCount = 137,
            LagStrokes = 138,
            MeasuredDepthReturns = 139,
            AvgGas = 140


        }

        public enum SORSectionSequence
        {
            GeneralInformation = 1,
            WellAndRigInformation = 2,
            Depths = 3,
            MudInformation = 4,
            Casing = 5,
            Liner = 6,
            WorkString = 7,
            InnerString = 8,
            EquipmentSpecifications = 9,
            EquipmentSpecificationsTiebackPacker = 10,
            EquipmentSpecificationsTieback = 11,
            Execution = 12,
            Planning = 13,
            Workshop = 14,
            AccessoriesOHIsolationCementedOneStage = 15,
            AccessoriesOHIsolationMultipleStagesDrilloutAllow = 16,
            AccessoriesOHIsolationMultipleStagesNoDrilloutAllow = 17,
            AccessoriesOHIsolationBarefootDrillOut = 18,
            AccessoriesOHIsolationBarefootNoDrilloutAllow = 19,
            AccessoriesOHIsolationUncementedWashDown = 20,
            AccessoriesOHIsolationUncementedNoWashDown = 21,
            AccessoriesRemediationStubLinerCemented = 22,
            AccessoriesRemediationStubLinerUncemented = 23,
            AccessoriesRemediationReLiner = 24,
            Supplemental = 25,
            AdditionalInformation = 26

        }


        public class SORSectionNamesConstants
        {
            public const string GeneralInformation = "GeneralInformation";
            public const string WellAndRigInformation = "WellAndRigInformation";
            public const string Depths = "Depths";
            public const string MudInformation = "MudInformation";
            public const string Casing = "Casing";
            public const string Liner = "Liner";
            public const string WorkString = "WorkString";
            public const string InnerString = "InnerString";
            public const string EquipmentSpecifications = "EquipmentSpecifications";
            public const string EquipmentSpecificationsTiebackPacker = "EquipmentSpecificationsTiebackPacker";
            public const string EquipmentSpecificationsTieback = "EquipmentSpecificationsTieback";
            public const string Execution = "Execution";
            public const string Planning = "Planning";
            public const string Workshop = "Workshop";
            public const string AccessoriesOHIsolationCementedOneStage = "AccessoriesOHIsolationCementedOneStage";
            public const string AccessoriesOHIsolationUncemented = "AccessoriesOHIsolationUncemented";
            public const string AccessoriesOHIsolationMultipleStagesDrilloutAllow = "AccessoriesOHIsolationMultipleStagesDrilloutAllow";
            public const string AccessoriesOHIsolationMultipleStagesNoDrilloutAllow = "AccessoriesOHIsolationMultipleStagesNoDrilloutAllow";
            public const string AccessoriesOHIsolationBarefootDrillOut = "AccessoriesOHIsolationBarefootDrillOut";
            public const string AccessoriesOHIsolationBarefootNoDrilloutAllow = "AccessoriesOHIsolationBarefootNoDrilloutAllow";
            public const string AccessoriesOHIsolationUncementedWashDown = "AccessoriesOHIsolationUncementedWashDown";
            public const string AccessoriesOHIsolationUncementedNoWashDown = "AccessoriesOHIsolationUncementedNoWashDown";
            public const string AccessoriesRemediationStubLinerCemented = "AccessoriesRemediationStubLinerCemented";
            public const string AccessoriesRemediationStubLinerUncemented = "AccessoriesRemediationStubLinerUncemented";
            public const string AccessoriesRemediationReLiner = "AccessoriesRemediationReLiner";
            public const string Supplemental = "Supplemental";
            public const string AdditionalInformation = "AdditionalInformation";
            public const string CementJobPumpingProgram = "CementJobPumpingProgram";
        }



        public class SORMasterDataConstants
        {
            public const string OpenHoleApplication = "Open Hole Isolation";
            public const string RemediationCasedHole = "Remediation (Cased Hole)";
            public const string Cemented = "Cemented";
            public const string Uncemented = "Uncemented";
            public const string TiebackUncemented = "Tie back (Uncemented)";
            public const string TiebackCemented = "Tie back (Cemented)";
            public const string TiebackPacker = "Tie back Packer";
            public const string StubLinerCemented = "Stub Liner (Cemented)";
            public const string StubLinerUncemented = "Stub Liner (Uncemented)";
            public const string Reliner = "Reliner";
            public const string OneStage = "One Stage";
            public const string MultipleStagesDrilloutAllow = "Multiple Stages (Drillout Allow)";
            public const string MultipleStagesNoDrilloutAllow = "Multiple Stages (No Drillout Allow)";
            public const string BarefootDrilloutAllow = "Barefoot (Drillout Allow)";
            public const string BarefootNoDrilloutAllow = "Barefoot (No Drillout Allow)";
            public const string WashDown = "Wash Down";
            public const string NoWashDown = "No Wash Down";
        }

        public class SORDBColumnNamesConstants
        {
            public const string OHCBarefootDrillingOutAllowed = "OHCBarefootDrillingOutAllowed";
            public const string OHCBarefootNoDrillingOutAllowed = "OHCBarefootNoDrillingOutAllowed";
            public const string OHCMultipleStagesDrillingOutAllowed = "OHCMultipleStagesDrillingOutAllowed";
            public const string Uncemented = "Uncemented";
            public const string OHCMultipleStagesNoDrillingOutAllowed = "OHCMultipleStagesNoDrillingOutAllowed";
            public const string OHCOneStage = "OHCOneStage";
            public const string OHUWashdown = "OHUWashdown";
            public const string OHUNoWashdown = "OHUNoWashdown";
            public const string REMUTieback = "REMUTieback";
            public const string REMCTieback = "REMCTieback";
            public const string REMGTiebackPacker = "REMGTiebackPacker";
            public const string REMCStubLiner = "REMCStubLiner";
            public const string REMUStubLiner = "REMUStubLiner";
            public const string REMGReLiner = "REMGReLiner";
        }
        public enum RoleLevelsForJobApproval
        {
            Level1 = 1,
            Level2 = 2,
            Level3 = 3,
            Level4 = 4,
            Level5 = 5,
            Level6 = 6
        }

        public enum LHPartIdentifierMasterDataEnum
        {
            PBR = 1,// Sales
            Packer,
            SettingSleeve,
            Coupling,
            Hanger,
            SealStem,
            HoldDownSub,
            PBRLower,
            SubOrCrossOver,
            DrillPipeDart,
            WiperPlug,
            LandingCollar,
            FloatCollar,
            FloatShoe,
            ReamerShoe,


            HandlingSub,//Rentals
            FJB,
            WRTE,
            PackerActuator,
            Spacer,
            RunningTool,
            SettingTool,
            Stinger,
            PickUpSub,
            LiftSub,
            Other = 100
        }
        public class ProductLineId
        {
            public const string OHFishing = "267";
            public const string CHFishing = "268";
            public const string Milling = "270";
            public const string Impact = "374";
            public const string PJU = "666";
            public const string InternalExternalCasingPatch = "266";
            public const string ReverseRun = "274";
        }
        public class BlanckMnemonic
        {
            public const string Select = "--Select--";
        }

        public enum ReportSections
        {
            Parent,
            PSRP,
            Equipment,
            SharePoint
        }
        public enum CheckListType
        {
            LinerPreDispatchLooseEquipmentChecklist = 1,
            SurfaceEquipmentChecklist,
        }

        public enum ConceptType
        {
            [StringValue("WPF")]
            WPF = 0,
            [StringValue("Text")]
            Text = 1,
            [StringValue("HTML")]
            HTML = 2,
        }

        public enum JobStatus
        {
            Active,
            Draft,
            Cancelled,
            Test,
            Closed
        }

        public enum DateSearchOptions
        {
            [StringValue("All")]
            All,
            [StringValue("Last Month")]
            LastMonth,
            [StringValue("Last Quater")]
            LastQuater,
            [StringValue("Last Six Month")]
            LastSixMonth,
            [StringValue("Last One Year")]
            LastOneYear,
            [StringValue("Current Year")]
            CurrentYear,
            [StringValue("Custom Date Range")]
            CustomDateRange
        }

        public enum GapOptionMethod
        {
            [StringValue("Current Time")]
            CurrentTime,
            [StringValue("By Hour")]
            ByHour,
            [StringValue("By DateTime")]
            ByTime,
            [StringValue("Everything")]
            Everything,
           
        }
    }
}
