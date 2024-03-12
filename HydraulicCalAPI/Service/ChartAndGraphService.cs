using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using WFT.Utility;
using WFT.UI.Common.Base;
using WFT.UI.Common.Charts;
using WFT.UnitConversion.UI;
using WFT.UI.Common.Validation;

using HydraulicEngine;
using HydraulicCalAPI.ViewModel;
using static HydraulicCalAPI.Service.HydraulicCalculationService;



namespace HydraulicCalAPI.Service
{
    public class ChartAndGraphService
    {
        public Dictionary<string, object> ChartNGraphDataPoints = new Dictionary<string, object>();

        public Fluid _fluidInput;
        public Cuttings _cuttingsInput;
        public List<HydraulicEngine.BHATool> _bhaInput;
        public List<Annulus> _annulusInput;
        public SurfaceEquipment _surfaceEquipmentInput;
        public HydraulicAnalysisOutput _hydraulicAnalysisOutput;
        public double _toolDepth;
        public double _flowRateOfChartedData;
        public bool IsUnitSystemChangesGettingApplied = false;
        public double BhaToolLength;
        public double ToolDepth;

        public enum ColorStrength { Transparent, Green, Yellow, Red }
        public ObservableCollection<HydraulicOutputAnnulusViewModel> _hydraulicOutputAnnulusList = new ObservableCollection<HydraulicOutputAnnulusViewModel>();
        public ChartViewModel<double> _standpipeVsFlowRateChart = new ChartViewModel<double>(new List<string>() { "OperatingPoint", "CalculatedPoint", "ObservedPoint" });
        public ObservableCollection<PieChartViewModel<double>> _pressureDistributionChartCollection = new ObservableCollection<PieChartViewModel<double>>();
        public ObservableCollection<HydraulicOutputBHAViewModel> _hydraulicOutputBHAList = new ObservableCollection<HydraulicOutputBHAViewModel>();
        public List<XYValueModelForLineData<double>> _hydraulicMainSeriesWholeData = new List<XYValueModelForLineData<double>>();

        public bool IsTotalPressureInWarningRegion = false;
        public bool IsTotalPressureInCriticalRegion = false;

        public double? _correctionFactorForObserved = 0;
        public double MaxPressure;
        public double MaxFlowRate;
        public double flowRateChartedData = double.MinValue;
        public double _minimumFlowRate;
        public double _maximumFlowRate;
        public double _lowerOperatingPoint;
        public double _upperOperatingPoint;
        public double _lowerCriticalPoint;
        public double _upperCriticalPoint;

        public double _observedFlowRate;
        public double _observedPressure;
        public double _calculatedFlowRate;
        public double _calculatedPressure;

        public bool _isObjectDisposedOrNotInUse;
        public bool _isDataInitializationHappening = false;
        private bool _isCalculationForCalculatedFieldInProgress = false;

        public const string flowRateField = "FlowRateOfChartedData";
        public const string toolDepthField = "ToolDepth";
        public const string standpipeVsFlowRateChartField = "StandpipeVsFlowRateChart";
        public const string observedFlowRateField = "ObservedFlowRate";
        public const string observedPressureField = "ObservedPressure";
        public const string calculatedFlowRateField = "CalculatedFlowRate";
        public const string calculatedPressureField = "CalculatedPressure";
        public const string correctionFactorForObservedField = "CorrectionFactorForObserved";
        public const string minimumFlowRateField = "MinimumFlowRate";
        public const string maximumFlowRateField = "MaximumFlowRate";
        public const string lowerOperatingPointField = "LowerOperatingPoint";
        public const string upperOperatingPointField = "UpperOperatingPoint";
        public const string lowerCriticalPointField = "LowerCriticalPoint";
        public const string upperCriticalPointField = "UpperCriticalPoint";
        public double TotalPressureDrop { get; set; }

        readonly ViewModelBase objvmb;

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public bool SetProperty<T>(string propertyName, ref T oldValue, ref T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }
            if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue))
            {
                oldValue = newValue;
                //objvmb.ValidateStateAndNotify(propertyName);
                //OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        public double ObservedFlowRate
        {
            get { return _observedFlowRate; }
            set { SetProperty(observedFlowRateField, ref _observedFlowRate, ref value); }
        }
        public double ObservedPressure
        {
            get
            {
                return _observedPressure;
            }
            set
            {
                SetProperty(observedPressureField, ref _observedPressure, ref value);
            }
        }
        public double CalculatedFlowRate
        {
            get
            {
                return _calculatedFlowRate;
            }
            set
            {
                SetProperty(calculatedFlowRateField, ref _calculatedFlowRate, ref value);
            }
        }
        public double CalculatedPressure
        {
            get
            {
                return _calculatedPressure;
            }
            set
            {
                SetProperty(calculatedPressureField, ref _calculatedPressure, ref value);
            }
        }
        public double? CorrectionFactorForObserved
        {
            get
            {
                return _correctionFactorForObserved;
            }
            set
            {
                SetProperty(correctionFactorForObservedField, ref _correctionFactorForObserved, ref value);
            }
        }

        public ObservableCollection<HydraulicOutputBHAViewModel> HydraulicOutputBHAList
        {
            get { return _hydraulicOutputBHAList; }
        }

        public ObservableCollection<PieChartViewModel<double>> PressureDistributionChartCollection
        {
            get { return _pressureDistributionChartCollection; }
        }

        public ObservableCollection<HydraulicOutputAnnulusViewModel> HydraulicOutputAnnulusList
        {
            get { return _hydraulicOutputAnnulusList; }
        }
        public ChartViewModel<double> StandpipeVsFlowRateChart
        {
            get { return _standpipeVsFlowRateChart; }
            set
            {
                SetProperty<ChartViewModel<double>>(standpipeVsFlowRateChartField, ref _standpipeVsFlowRateChart, ref value);
            }
        }
        public ErrorMessageDictionary _errorMessages = new ErrorMessageDictionary();
        public ErrorMessageDictionary ErrorMessages
        {
            get { return _errorMessages; }
        }

        /// <summary>
        /// Gets or sets MinimumFlowRate
        /// </summary>
        /// <value>
        /// MinimumFlowRate
        /// </value>
        public double MinimumFlowRate
        {
            get { return _minimumFlowRate; }
            set
            {
                SetProperty<double>(minimumFlowRateField, ref _minimumFlowRate, ref value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumFlowRate
        /// </summary>
        /// <value>
        /// MaximumFlowRate
        /// </value>
        public double MaximumFlowRate
        {
            get { return _maximumFlowRate; }
            set
            {
                SetProperty<double>(maximumFlowRateField, ref _maximumFlowRate, ref value);
            }
        }

        /// <summary>
        /// Gets or sets LowerOperatingPoint
        /// </summary>
        /// <value>
        /// LowerOperatingPoint
        /// </value>
        public double LowerOperatingPoint
        {
            get { return _lowerOperatingPoint; }
            set
            {
                SetProperty<double>(lowerOperatingPointField, ref _lowerOperatingPoint, ref value);
            }
        }

        /// <summary>
        /// Gets or sets UpperOperatingPoint
        /// </summary>
        /// <value>
        /// UpperOperatingPoint
        /// </value>
        public double UpperOperatingPoint
        {
            get { return _upperOperatingPoint; }
            set
            {
                SetProperty<double>(upperOperatingPointField, ref _upperOperatingPoint, ref value);
            }
        }

        /// <summary>
        /// Gets or sets LowerCriticalPoint
        /// </summary>
        /// <value>
        /// LowerCriticalPoint
        /// </value>
        public double LowerCriticalPoint
        {
            get { return _lowerCriticalPoint; }
            set
            {
                SetProperty<double>(lowerCriticalPointField, ref _lowerCriticalPoint, ref value);
            }
        }

        /// <summary>
        /// Gets or sets UpperCriticalPoint
        /// </summary>
        /// <value>
        /// UpperCriticalPoint
        /// </value>
        public double UpperCriticalPoint
        {
            get { return _upperCriticalPoint; }
            set
            {
                SetProperty<double>(upperCriticalPointField, ref _upperCriticalPoint, ref value);
            }
        }

        /// <summary>
        /// View Model List for HydraulicOutputAnnulusViewModel properties
        /// </summary>
        public class HydraulicOutputAnnulusViewModel
        {
            public string Annulus { get; set; }
            public string Workstring { get; set; }
            public double FromAnnulus { get; set; }
            public double ToAnnulus { get; set; }
            public double Length { get; set; }
            public double InnerDiameter { get; set; }
            public double ToolOuterDiameter { get; set; }
            public double AverageVelocity { get; set; }
            public double CriticalVelocity { get; set; }
            public string FlowType { get; set; }
            public double ChipRate { get; set; }
            public double AnnulusPressureDrop { get; set; }
            public ColorStrength AverageVelocityColor { get; set; }
            public ColorStrength ChipRateColor { get; set; }
            public string AnnulusColor { get; set; }
            public HydraulicOutputAnnulusViewModel(Segment segment, Color? bhaToolColor)
            {
                SetHydraulicAnnulusOutput(segment, bhaToolColor);
            }

            public void SetHydraulicAnnulusOutput(Segment segment, Color? bhaToolColor)
            {
                Annulus = segment.WellboreSectionName;
                Workstring = segment.ToolDescription;
                FromAnnulus = segment.SegmentTopInFeet;
                ToAnnulus = segment.SegmentBottomInFeet;
                AverageVelocity = segment.SegmentHydraulicsOutput.AverageVelocityInFeetPerMinute;
                CriticalVelocity = segment.SegmentHydraulicsOutput.CriticalVelocityInFeetPerMinute;
                FlowType = segment.SegmentHydraulicsOutput.FlowType;
                ChipRate = segment.SegmentHydraulicsOutput.ChipRateInFeetPerMinute;
                AnnulusPressureDrop = segment.SegmentHydraulicsOutput.PressureDropInPSI;
                SetAverageVelocityColor();
                SetChipRateColor(segment);
                if (bhaToolColor.HasValue)
                    AnnulusColor = bhaToolColor.Value.Name.ToString();
                Length = segment.SegmentLengthInFeet;
                ToolOuterDiameter = segment.ToolODInInch;
                InnerDiameter = segment.AnnulusIDInInch;
            }
            private void SetAverageVelocityColor()
            {
                if (AverageVelocity >= 0.00)
                {
                    if (AverageVelocity == 0)
                    {
                        AverageVelocityColor = ColorStrength.Transparent;
                    }
                    else if (AverageVelocity >= 140)
                    {
                        AverageVelocityColor = ColorStrength.Green;
                    }
                    else if (AverageVelocity >= 120 && AverageVelocity < 140)
                    {
                        AverageVelocityColor = ColorStrength.Yellow;
                    }
                    else if (AverageVelocity < 120)
                    {
                        AverageVelocityColor = ColorStrength.Red;
                    }
                }
            }

            private void SetChipRateColor(Segment segment)
            {
                if (segment.SegmentHydraulicsOutput.ChipRateRange == Common.ResultType.Good)
                {
                    if (ChipRate == 0)
                    {
                        ChipRateColor = ColorStrength.Transparent;
                    }
                    else
                    {
                        ChipRateColor = ColorStrength.Green;
                    }
                }
                else if (segment.SegmentHydraulicsOutput.ChipRateRange == Common.ResultType.Caution)
                {
                    ChipRateColor = ColorStrength.Yellow;
                }
                else
                {
                    ChipRateColor = ColorStrength.Red;
                }
            }
        }

        #region "Calculate Hydraulics"
        public Dictionary<string, object> GetDataPoints(Fluid fluidInput,
            double __flowRateOfChartedData, Cuttings cuttingInput, List<HydraulicEngine.BHATool> bhaInput, List<Annulus> annulusInput, SurfaceEquipment surfaceEquipment, double maxflow, double maxpres, double toolDepthInFeet)
        {
            MaxFlowRate = maxflow;
            MaxPressure = maxpres;
            int torqueInFeetPound;
            _fluidInput = fluidInput;
            _bhaInput = bhaInput;
            _cuttingsInput = cuttingInput;
            _annulusInput = annulusInput;
            _surfaceEquipmentInput = surfaceEquipment;
            flowRateChartedData = __flowRateOfChartedData;
            ToolDepth = toolDepthInFeet;
            double blockPostionInFeet;
            _hydraulicAnalysisOutput = Main.CompleteHydraulicAnalysis(fluidInput, flowRateChartedData, cuttingInput, _bhaInput, annulusInput, surfaceEquipment, torqueInFeetPound = 0, toolDepthInFeet, blockPostionInFeet = double.MinValue);
           // PlotChart();
            double totalPressureDrop = CalculateTotalPressureDropFROMHydraulicAnalysisOutput(_hydraulicAnalysisOutput);
            CalculateHydraulics();

            ChartNGraphDataPoints.Add("hydraulicOutput", _hydraulicAnalysisOutput);
            ChartNGraphDataPoints.Add("TotalPressureDrop", totalPressureDrop);

            return ChartNGraphDataPoints;
        }

        public void CalculateHydraulics()
        {
           
            ColorCodeGenerator colorSelector = new ColorCodeGenerator();
            Color annulusColor = colorSelector.GetColor();
            string annulusToolNameForPieChart = "Annulus";
            Nullable<double> totalPressureDrolAtAnnulus = null;
            if (_hydraulicAnalysisOutput.Segment != null)
                foreach (var item in _hydraulicAnalysisOutput.Segment)
                {
                    HydraulicOutputAnnulusList.Add(new HydraulicOutputAnnulusViewModel(item, annulusColor));
                    if (totalPressureDrolAtAnnulus == null)
                        totalPressureDrolAtAnnulus = 0;
                    totalPressureDrolAtAnnulus = totalPressureDrolAtAnnulus + item.SegmentHydraulicsOutput.PressureDropInPSI;
                }
            // Get the lis and colour code for Annulus Table
            ChartNGraphDataPoints.Add("HydraulicOutputAnnulusList", HydraulicOutputAnnulusList.ToArray());
            ChartNGraphDataPoints.Add("ToolDepthInFeet", ToolDepth);

            PressureDistributionChartCollection.Clear();
            if (totalPressureDrolAtAnnulus != null)
            {
                var tempPressureField = Convert.ToDouble(totalPressureDrolAtAnnulus);
                PressureDistributionChartCollection.Add(new PieChartViewModel<double>() { Name = annulusToolNameForPieChart, Value = tempPressureField, Color = annulusColor.Name.ToString() });
            }

            TotalPressureDrop = 0.00;

            if (_hydraulicAnalysisOutput.Segment != null)
                TotalPressureDrop = _hydraulicAnalysisOutput.Segment.Where(o => o.SegmentHydraulicsOutput != null && !double.IsNaN(o.SegmentHydraulicsOutput.PressureDropInPSI)).Select(o => o.SegmentHydraulicsOutput.PressureDropInPSI).Sum();
            if (_hydraulicAnalysisOutput.BHATool != null)
                TotalPressureDrop += _hydraulicAnalysisOutput.BHATool.Where(o => o.BHAHydraulicsOutput != null && !double.IsNaN(o.BHAHydraulicsOutput.PressureDropInPSI)).Select(o => o.BHAHydraulicsOutput.PressureDropInPSI).Sum();

            if (double.IsNaN(((ISurfaceEquipmentHydraulicsOutput)_hydraulicAnalysisOutput.surfaceEquipment).PressureDropInPSI))
            {
                TotalPressureDrop = TotalPressureDrop + ((ISurfaceEquipmentHydraulicsOutput)_hydraulicAnalysisOutput.surfaceEquipment).PressureDropInPSI;
            }

            if (TotalPressureDrop >= 0.00 && MaxPressure >= 0.00)
            {
                if (TotalPressureDrop > MaxPressure)
                {
                    IsTotalPressureInCriticalRegion = true;
                    IsTotalPressureInWarningRegion = false;
                }
                else if (TotalPressureDrop > 0.9 * MaxPressure)
                {
                    IsTotalPressureInCriticalRegion = false;
                    IsTotalPressureInWarningRegion = true;
                }
                else
                {
                    IsTotalPressureInCriticalRegion = false;
                    IsTotalPressureInWarningRegion = false;
                }
            }
            else
            {
                IsTotalPressureInCriticalRegion = false;
                IsTotalPressureInWarningRegion = false;
            }

            List<HydraulicOutputBHAViewModel> tempListForIsToolDetailsVisible = new List<HydraulicOutputBHAViewModel>();
            tempListForIsToolDetailsVisible = HydraulicOutputBHAList.Where(x => x.IsToolDetailsVisible).ToList();

            double? inputFlowRate = flowRateChartedData;
            Dictionary<Guid, Tuple<double?, double?, double?, double?>> scalingCache = new Dictionary<Guid, Tuple<double?, double?, double?, double?>>();
            foreach (var item in HydraulicOutputBHAList)
            {
                if (item.ToolID.HasValue)
                {
                    scalingCache.Add(item.ToolID.Value, new Tuple<double?, double?, double?, double?>
                        (item.PressureRangeMinAppliedValue, item.PressureRangeMaxAppliedValue, item.FlowRateRangeMinAppliedValue, item.FlowRateRangeMaxAppliedValue));
                }
            }

            HydraulicOutputBHAList.Clear();
            if (_hydraulicAnalysisOutput.BHATool != null)
                foreach (var item in _hydraulicAnalysisOutput.BHATool)
                {
                    System.Drawing.Color color = colorSelector.GetColor();
                    //var tempPressureValue = Convert.ToDouble((new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure) { BaseValue = item.BHAHydraulicsOutput.PressureDropInPSI }));
                    var tempPressureValue = (double)item.BHAHydraulicsOutput.PressureDropInPSI;
                    PressureDistributionChartCollection.Add(new PieChartViewModel<double>() { Name = item.toolDescription, Value = tempPressureValue, Color = color.Name.ToString() });
                    HydraulicOutputBHAViewModel bhaObject = HydraulicToolTypeObjectManager.GetToolTypeObject(item, color, _fluidInput, MaxFlowRate, MaxPressure, _bhaInput, inputFlowRate);
                    bhaObject.positionNo = item.PositionNumber;
                    //if (_bhaVMList.Exists(o => o.BHAID == bhaObject.ToolID.Value))
                    //{
                    //    bhaObject.IsToolInputDetailsVisible = true;
                    //}

                    BhaToolLength += item.LengthInFeet;
                    HydraulicOutputBHAList.Add(bhaObject);
                    if (!double.IsNaN(item.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute))
                        inputFlowRate = item.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute;
                }
            DateTime startOfCalculations = DateTime.UtcNow;
            foreach (HydraulicOutputBHAViewModel item in HydraulicOutputBHAList)
            {
                item.PlotChart();
                item.BHAchart = item.GetSPvsFRChartData();


                if (item.ToolID.HasValue && scalingCache.ContainsKey(item.ToolID.Value))
                {
                    var scalingValues = scalingCache[item.ToolID.Value];
                    item.PressureLowerDisplayValue = (double)scalingValues.Item1;
                    item.PressureUpperDisplayValue = (double)scalingValues.Item2;
                    item.FlowRateLowerDisplayValue = (double)scalingValues.Item3;
                    item.FlowRateUpperDisplayValue = (double)scalingValues.Item4;
                    //item.SetSPVsFRScalingCommand.Execute(null);
                }
            }
            //ChartNGraphDataPoints.Add("B".ToArray());
            ChartNGraphDataPoints.Add("HydraulicOutputBHAList", HydraulicOutputBHAList.ToArray());
            ChartNGraphDataPoints.Add("PressureDistributionChartCollection", PressureDistributionChartCollection.ToArray());

            //ChartNGraphDataPoints.Add(innStandardVSFlowrateDp.ToArray());


            System.Diagnostics.Debug.WriteLine("End of all plot chart : " + (DateTime.UtcNow - startOfCalculations).ToString());
            startOfCalculations = DateTime.UtcNow;
            PlotChart();
            #endregion "Calculate Hydraulics"
        }

        private void PlotChart()
        {
            double flowrate = 0;
            double lastRecordedStandpipePressure = 0.00;

            List<XYValueModelForLineData<double>> standpipePressureListRL = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> standpipePressureListYL = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> standpipePressureListG = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> standpipePressureListYH = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> standpipePressureListRH = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> estimatedStandpipePressureList = new List<XYValueModelForLineData<double>>();
            List<XYValueModelForLineData<double>> standpipePressureRangeData = new List<XYValueModelForLineData<double>>();
            SeriesModel<XYValueModel<double>, double> estimatedStandpipeSeries = new SeriesModel<XYValueModel<double>, double>();
            SeriesModel<XYValueModel<double>, double> hydraproRangeSeries = new SeriesModel<XYValueModel<double>, double>();
            int exitLoopCounter = 0;
            double currentSecondaryAxisValue = 0;
            double lastFlowRate = 0;
            StandpipeVsFlowRateChart.RemoveSeries("HydraproRangeSeries");
            hydraproRangeSeries.SeriesType = "Band";
            hydraproRangeSeries.Color = "LightBlue";
            hydraproRangeSeries.SeriesName = "HydraproRangeSeries";
            StandpipeVsFlowRateChart.AddSeries(hydraproRangeSeries);

            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeriesRedLower");
            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeriesYellowLower");
            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeriesGreen");
            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeriesYellowHigher");
            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeriesRedHigher");

            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Red",
                SeriesName = "HydraproLineSeriesRedLower",

            });
            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Yellow",
                SeriesName = "HydraproLineSeriesYellowLower",

            });
            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Green",
                SeriesName = "HydraproLineSeriesGreen",

            });
            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Yellow",
                SeriesName = "HydraproLineSeriesYellowHigher",

            });
            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Red",
                SeriesName = "HydraproLineSeriesRedHigher",

            });


            StandpipeVsFlowRateChart.RemoveSeries("EstimatedStandpipeLineSeries");
            estimatedStandpipeSeries.SeriesType = "Line";
            estimatedStandpipeSeries.Color = "Blue";
            estimatedStandpipeSeries.SeriesName = "EstimatedStandpipeLineSeries";
            StandpipeVsFlowRateChart.AddSeries(estimatedStandpipeSeries);

            double pressureValueMax = 0.00;
            if (MaxFlowRate >= 0.00)
            {
                double? annularVelocity;
                pressureValueMax = (double)CalculateStandPipePressure(MaxFlowRate, ToolDepth, out annularVelocity);

                if (MaxPressure >= 0.00 && MaxPressure < pressureValueMax)
                {
                    pressureValueMax = MaxPressure;
                }
            }
            else
            {
                pressureValueMax = MaxPressure;
            }

            flowrate = 0;
            do
            {
                XYValueModelForLineData<double> valuemodelForLineData = new XYValueModelForLineData<double>();
                XYValueModelForLineData<double> dataPointsForEstimatedStandpipePressure = new XYValueModelForLineData<double>();

                double? annularVelocity;
                lastRecordedStandpipePressure = (double)CalculateStandPipePressure(flowrate, ToolDepth, out annularVelocity);


                valuemodelForLineData.PrimaryAxisValue = Convert.ToDouble(flowrate);
                valuemodelForLineData.SecondaryAxisValue = currentSecondaryAxisValue = Convert.ToDouble(lastRecordedStandpipePressure);

                dataPointsForEstimatedStandpipePressure.PrimaryAxisValue = Convert.ToDouble(flowrate);
                dataPointsForEstimatedStandpipePressure.SecondaryAxisValue = currentSecondaryAxisValue * _correctionFactorForObserved ?? 0;

                if (pressureValueMax >= 0)
                {

                    if (lastRecordedStandpipePressure <= pressureValueMax)
                    {
                        XYValueModelForRangeData<double> valueModelForRangeData = new XYValueModelForRangeData<double>();
                        valueModelForRangeData.LowerBoundValue = currentSecondaryAxisValue;
                        valueModelForRangeData.UpperBoundValue = Convert.ToDouble(pressureValueMax);
                        valueModelForRangeData.PrimaryAxisValue = Convert.ToDouble(flowrate);
                        //  standpipePressureRangeData.Add(valueModelForRangeData);
                    }
                }

                if (annularVelocity.HasValue && annularVelocity < 120)
                {
                    standpipePressureListRL.Add(valuemodelForLineData);
                }
                else if (annularVelocity.HasValue && annularVelocity >= 120 && annularVelocity <= 140)
                {
                    standpipePressureListYL.Add(valuemodelForLineData);
                }
                else if ((MaxFlowRate >= 0.00 && valuemodelForLineData.PrimaryAxisValue > MaxFlowRate) ||
                    (MaxPressure >= 0.00 && valuemodelForLineData.SecondaryAxisValue > MaxPressure))
                {
                    standpipePressureListRH.Add(valuemodelForLineData);
                }
                else if ((MaxFlowRate >= 0.00 && valuemodelForLineData.PrimaryAxisValue >= MaxFlowRate * 0.9 && valuemodelForLineData.PrimaryAxisValue <= MaxFlowRate) ||
                    (MaxPressure >= 0.00 && valuemodelForLineData.SecondaryAxisValue >= MaxPressure * 0.9 && valuemodelForLineData.SecondaryAxisValue <= MaxPressure))
                {
                    standpipePressureListYH.Add(valuemodelForLineData);
                }
                else
                {
                    standpipePressureListG.Add(valuemodelForLineData);
                }

                estimatedStandpipePressureList.Add(dataPointsForEstimatedStandpipePressure);
                if (currentSecondaryAxisValue == 0)
                {
                    exitLoopCounter++;
                }
                flowrate += 1;
                Console.WriteLine("flowrate : " + flowrate + " get laststandpie : " + lastRecordedStandpipePressure);
                if (exitLoopCounter == 1000)
                {
                    exitLoopCounter = 0;
                    break;
                }
                lastFlowRate = valuemodelForLineData.PrimaryAxisValue;

                _hydraulicMainSeriesWholeData.Add(new XYValueModelForLineData<double>() { PrimaryAxisValue = valuemodelForLineData.PrimaryAxisValue, SecondaryAxisValue = valuemodelForLineData.SecondaryAxisValue });
            }
            while (lastRecordedStandpipePressure < 1.2 * this.MaxPressure);


            var firstValueOfYL = (standpipePressureListYL.FirstOrDefault() as XYValueModelForLineData<double>);
            var firstValueOfG = (standpipePressureListG.FirstOrDefault() as XYValueModelForLineData<double>);
            var firstValueOfYH = (standpipePressureListYH.FirstOrDefault() as XYValueModelForLineData<double>);
            var firstValueOfRH = (standpipePressureListRH.FirstOrDefault() as XYValueModelForLineData<double>);

            if (standpipePressureListRL.Count > 0)
            {
                XYValueModelForLineData<double> valueToAdd = firstValueOfYL ?? firstValueOfG ?? firstValueOfYH ?? firstValueOfRH;
                if (valueToAdd != null)
                {
                    standpipePressureListRL.Add(new XYValueModelForLineData<double>() { PrimaryAxisValue = valueToAdd.PrimaryAxisValue, SecondaryAxisValue = valueToAdd.SecondaryAxisValue });
                }
            }
            if (standpipePressureListYL.Count > 0)
            {
                XYValueModelForLineData<double> valueToAdd = firstValueOfG ?? firstValueOfYH ?? firstValueOfRH;
                if (valueToAdd != null)
                {
                    standpipePressureListYL.Add(new XYValueModelForLineData<double>() { PrimaryAxisValue = valueToAdd.PrimaryAxisValue, SecondaryAxisValue = valueToAdd.SecondaryAxisValue });
                }
            }
            if (standpipePressureListG.Count > 0)
            {
                XYValueModelForLineData<double> valueToAdd = firstValueOfYH ?? firstValueOfRH;
                if (valueToAdd != null)
                {
                    standpipePressureListG.Add(new XYValueModelForLineData<double>() { PrimaryAxisValue = valueToAdd.PrimaryAxisValue, SecondaryAxisValue = valueToAdd.SecondaryAxisValue });
                }
            }

            if (standpipePressureListYH.Count > 0)
            {
                XYValueModelForLineData<double> valueToAdd = firstValueOfRH;
                if (firstValueOfRH != null)
                {
                    standpipePressureListYH.Add(new XYValueModelForLineData<double>() { PrimaryAxisValue = valueToAdd.PrimaryAxisValue, SecondaryAxisValue = valueToAdd.SecondaryAxisValue });
                }
            }

            /*tandpipeVsFlowRateChart.AddBulkValue("HydraproRangeSeries", standpipePressureRangeData);

            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeriesRedLower", standpipePressureListRL);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeriesYellowLower", standpipePressureListYL);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeriesGreen", standpipePressureListG);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeriesYellowHigher", standpipePressureListYH);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeriesRedHigher", standpipePressureListRH);
            StandpipeVsFlowRateChart.AddBulkValue("EstimatedStandpipeLineSeries", estimatedStandpipePressureList);
*/
            MinimumFlowRate = 0;
            MaximumFlowRate = lastFlowRate;
            LowerCriticalPoint = standpipePressureListRL.LastOrDefault() == null ? 0 : standpipePressureListRL.LastOrDefault().PrimaryAxisValue;
            LowerOperatingPoint = standpipePressureListYL.LastOrDefault() == null ? LowerCriticalPoint : standpipePressureListYL.LastOrDefault().PrimaryAxisValue;
            UpperOperatingPoint = standpipePressureListG.LastOrDefault() == null ? LowerOperatingPoint : standpipePressureListG.LastOrDefault().PrimaryAxisValue;
            UpperCriticalPoint = standpipePressureListYH.LastOrDefault() == null ? UpperOperatingPoint : standpipePressureListYH.LastOrDefault().PrimaryAxisValue;

            ChartNGraphDataPoints.Add("standpipePressureRangeData", standpipePressureRangeData);

            ChartNGraphDataPoints.Add("standpipePressureListRL", standpipePressureListRL.ToArray());
            ChartNGraphDataPoints.Add("standpipePressureListYL", standpipePressureListYL.ToArray());
            ChartNGraphDataPoints.Add("standpipePressureListG", standpipePressureListG.ToArray());
            ChartNGraphDataPoints.Add("standpipePressureListYH", standpipePressureListYH.ToArray());
            ChartNGraphDataPoints.Add("standpipePressureListRH", standpipePressureListRH.ToArray());

        }

        private double? CalculateStandPipePressure(double? flowRateBaseValue, double? depthBaseValue, out double? annularVelocityMin)
        {

            double? totalPressureDrop = 0.00;
            annularVelocityMin = 0.00;
            if (flowRateBaseValue > 0.00 && _bhaInput != null && _annulusInput != null)
            {
                double flowRateInGPMInput = flowRateBaseValue.Value;
                double toolDepthInFeetInput = depthBaseValue ?? double.MinValue;
                Console.WriteLine(toolDepthInFeetInput);
                Console.WriteLine(flowRateInGPMInput);
                int torqueInFeetPound;
                double blockPostionInFeet;
                HydraulicAnalysisOutput hydraulicOutput = Main.CompleteHydraulicAnalysis(_fluidInput, flowRateInGPMInput, _cuttingsInput, _bhaInput, _annulusInput, _surfaceEquipmentInput, torqueInFeetPound = 0, toolDepthInFeetInput, blockPostionInFeet = double.MinValue);
                
               // ChartNGraphDataPoints.TryAdd("hydraulicOutput" + flowRateBaseValue, hydraulicOutput);
                double totalPressureDrop1 = CalculateTotalPressureDropFROMHydraulicAnalysisOutput(hydraulicOutput);
                Console.WriteLine("totalPressureDrop1       " + totalPressureDrop1);
                if (hydraulicOutput.Segment != null && hydraulicOutput.Segment.Count() > 0)
                {
                    foreach (var segment in hydraulicOutput.Segment)
                    {
                        var segmentHydraulicsOutput = segment as ISegmentHydraulicsOutput;
                        if (segmentHydraulicsOutput != null && !double.IsNaN(segmentHydraulicsOutput.PressureDropInPSI))
                        {
                            Console.WriteLine(segment.ToolDescription + "       " + segmentHydraulicsOutput.PressureDropInPSI);
                            totalPressureDrop += segmentHydraulicsOutput.PressureDropInPSI;
                        }

                        if (segmentHydraulicsOutput != null && segmentHydraulicsOutput.FlowType != "None" && !double.IsNaN(segmentHydraulicsOutput.AverageVelocityInFeetPerMinute))
                        {
                            if (annularVelocityMin == 0.00 || segmentHydraulicsOutput.AverageVelocityInFeetPerMinute < annularVelocityMin)
                            {
                                annularVelocityMin = segmentHydraulicsOutput.AverageVelocityInFeetPerMinute;
                            }
                        }
                    }
                }

                if (hydraulicOutput.BHATool != null && hydraulicOutput.BHATool.Count() > 0)
                {
                    foreach (var tool in hydraulicOutput.BHATool)
                    {
                        if (tool.BHAHydraulicsOutput != null && !double.IsNaN(tool.BHAHydraulicsOutput.PressureDropInPSI))
                        {
                            Console.WriteLine(tool.toolDescription + "       " + tool.BHAHydraulicsOutput.PressureDropInPSI);
                            totalPressureDrop += tool.BHAHydraulicsOutput.PressureDropInPSI;
                        }
                    }
                }
            }
            Console.WriteLine("totalPressureDrop       " + totalPressureDrop);

            return totalPressureDrop;

        }
        public double CalculateTotalPressureDropFROMHydraulicAnalysisOutput(HydraulicAnalysisOutput hydraulicOutput)
        {
            double totalPressureDrop = 0.00;
            double annularVelocityMin = 0.00;  // If this value is used outside this method, it should be returned or handled differently.

            if (hydraulicOutput != null)
            {
                if (hydraulicOutput.Segment != null && hydraulicOutput.Segment.Count() > 0)
                {
                    foreach (var segment in hydraulicOutput.Segment)
                    {
                        var segmentHydraulicsOutput = segment as ISegmentHydraulicsOutput;
                        if (segmentHydraulicsOutput != null && !double.IsNaN(segmentHydraulicsOutput.PressureDropInPSI))
                        {
                            totalPressureDrop += segmentHydraulicsOutput.PressureDropInPSI;

                        }

                        if (segmentHydraulicsOutput != null && segmentHydraulicsOutput.FlowType != "None" && !double.IsNaN(segmentHydraulicsOutput.AverageVelocityInFeetPerMinute))
                        {
                            if (annularVelocityMin == 0.00 || segmentHydraulicsOutput.AverageVelocityInFeetPerMinute < annularVelocityMin)
                            {
                                annularVelocityMin = segmentHydraulicsOutput.AverageVelocityInFeetPerMinute;
                            }
                        }
                    }
                }

                if (hydraulicOutput.BHATool != null && hydraulicOutput.BHATool.Count() > 0)
                {
                    for (int j = 0; j < hydraulicOutput.BHATool.Count(); j++)
                    {
                        var tool = hydraulicOutput.BHATool.ElementAt(j);
                        if (tool.BHAHydraulicsOutput != null && !double.IsNaN(tool.BHAHydraulicsOutput.PressureDropInPSI))
                        {
                            totalPressureDrop += tool.BHAHydraulicsOutput.PressureDropInPSI;
                        }
                    }
                }
            }

            return totalPressureDrop;
        }


    }

}
