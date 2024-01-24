using System;
using System.Linq;
using System.Collections.Generic;

using WFT.UI.Common.Base;
using WFT.UI.Common.Charts;
using WFT.UI.Common.Commands;

using HydraulicEngine;
using HydraulicCalAPI.Service;

namespace HydraulicCalAPI.ViewModel
{
    public class HydraulicOutputBHAViewModel
    {
        #region public Variables
        public Guid? _toolID;
        public string _workistring;
        public double _lengthBHA;
        public double _outerDiameter;
        public double _inputFlowRate;
        public double _averageVelocity;
        public double _criticalVelocity;
        public string _flowType;
        public double _bhaPressureDrop;
        public ChartAndGraphService.ColorStrength _averageVelocityColor;
        public string _bhaColor;
        public bool _isToolDetailsVisible;
        public string _actualToolDescription;
        public string _isPlusMinus;
        public double _maxPressure;
        public double _maxFlowRate;
        public ChartViewModel<double> _standpipeVsFlowRateChart = new ChartViewModel<double>(new List<string>() { "OperatingPoint" });
        public BHATool _bhaToolObjectFromHydraulicEngine = null;
        public Fluid _fluidDataFromHydraulicEngine;

        public bool _showStandpipeVsFlowRateScaling;
        public double _pressureLowerDisplayValue;
        public double _pressureUpperDisplayValue;
        public double _flowRateLowerDisplayValue;
        public double _flowRateUpperDisplayValue;
        public double? _pressureRangeMinAppliedValue;
        public double? _pressureRangeMaxAppliedValue;
        public double? _flowRateRangeMinAppliedValue;
        public double? _flowRateRangeMaxAppliedValue;
        public DelegateCommand _setSPVsFRScalingCommand;
        public DelegateCommand _resetSPVsFRScalingCommand;
        public DelegateCommand _cancelSPVsFRScalingCommand;
        public List<BHATool> _bhaTools = new List<BHATool>();
        public DelegateCommand _changeToolDetailVisibilityCommand;
        public bool _isToolInputDetailsVisible;
        public string _toolType;
        public string _toolSourcePath;
        #endregion
        readonly ViewModelBase objvmb;
        #region Constants

        public const string isPlusMinusField = "IsPlusMinus";
        public const string workstringField = "Workstring";
        public const string lengthBHAField = "LengthBHA";
        public const string outerDiameterField = "OuterDiameter";
        public const string inputFlowRateField = "Input Flow Rate";
        public const string averageVelocityField = "AverageVelocity";
        public const string criticalVelocityField = "CriticalVelocity";
        public const string flowTypeField = "FlowType";
        public const string bHAPressureDropField = "BHAPressureDrop";
        public const string averageVelocityColorField = "AverageVelocityColor";
        public const string bhaColorField = "BHAColor";
        public const string IsToolDetailsVisibleField = "IsToolDetailsVisible";
        public const string actualToolDescriptionField = "ActualToolDescription";
        public const string standpipeVsFlowRateChartField = "StandpipeVsFlowRateChart";

        public const string showStandpipeVsFlowRateScalingField = "ShowStandpipeVsFlowRateScaling";
        public const string pressureLowerDisplayValueField = "PressureLowerDisplayValue";
        public const string pressureUpperDisplayValueField = "PressureUpperDisplayValue";
        public const string flowRateLowerDisplayValueField = "FlowRateLowerDisplayValue";
        public const string flowRateUpperDisplayValueField = "FlowRateUpperDisplayValue";
        public const string pressureRangeMinAppliedValueField = "PressureRangeMinAppliedValue";
        public const string pressureRangeMaxAppliedValueField = "PressureRangeMaxAppliedValue";
        public const string flowRateRangeMinAppliedValueField = "FlowRateRangeMinAppliedValue";
        public const string flowRateRangeMaxAppliedValueField = "FlowRateRangeMaxAppliedValue";
        public const string toolTypeField = "ToolType";
        public const string isToolInputDetailsVisibleField = "IsToolInputDetailsVisible";
        public const string toolSourcePathField = "ToolSourcePath";
        #endregion

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

        #region Properties
        public string ToolSourcePath
        {
            get { return _toolSourcePath; }
            set
            {
                SetProperty<string>(toolSourcePathField, ref _toolSourcePath, ref value);
            }
        }
        public bool IsToolInputDetailsVisible
        {
            get { return _isToolInputDetailsVisible; }
            set
            {
                SetProperty<bool>(IsToolDetailsVisibleField, ref _isToolInputDetailsVisible, ref value);
            }
        }
        public string ToolType
        {
            get { return _toolType; }
            set
            {
                SetProperty<string>(toolTypeField, ref _toolType, ref value);
            }
        }
        public ChartViewModel<double> StandpipeVsFlowRateChart
        {
            get { return _standpipeVsFlowRateChart; }
            set
            {
                SetProperty<ChartViewModel<double>>(standpipeVsFlowRateChartField, ref _standpipeVsFlowRateChart, ref value);
            }
        }
        public Guid? ToolID
        {
            get { return _toolID; }
            set
            {
                _toolID = value;
            }
        }
        public string Workstring
        {
            get { return _workistring; }
            set
            {
                SetProperty<string>(workstringField, ref _workistring, ref value);
            }
        }
        public double LengthBHA
        {
            get { return _lengthBHA; }
            set
            {
                SetProperty(lengthBHAField, ref _lengthBHA, ref value);
            }
        }
        public double OuterDiameter
        {
            get { return _outerDiameter; }
            set
            {
                SetProperty(outerDiameterField, ref _outerDiameter, ref value);
            }
        }
        public double InputFlowRate
        {
            get { return _inputFlowRate; }
            set
            {
                SetProperty(inputFlowRateField, ref _inputFlowRate, ref value);
            }
        }
        public double AverageVelocity
        {
            get { return _averageVelocity; }
            set
            {
                SetProperty(averageVelocityField, ref _averageVelocity, ref value);
            }
        }
        public double CriticalVelocity
        {
            get { return _criticalVelocity; }
            set
            {
                SetProperty(criticalVelocityField, ref _criticalVelocity, ref value);
            }
        }
        public string FlowType
        {
            get { return _flowType; }
            set
            {
                SetProperty<string>(flowTypeField, ref _flowType, ref value);
            }
        }
        public double BHAPressureDrop
        {
            get { return _bhaPressureDrop; }
            set
            {
                SetProperty(bHAPressureDropField, ref _bhaPressureDrop, ref value);
            }
        }
        public ChartAndGraphService.ColorStrength AverageVelocityColor
        {
            get { return _averageVelocityColor; }
            set
            {
                SetProperty<ChartAndGraphService.ColorStrength>(averageVelocityColorField, ref _averageVelocityColor, ref value);
            }
        }
        public string IsPlusMinus
        {
            get { return _isPlusMinus; }
            set
            {
                SetProperty<string>(isPlusMinusField, ref _isPlusMinus, ref value);
            }
        }
        public string BHAColor
        {
            get { return _bhaColor; }
            set
            {
                SetProperty<string>(bhaColorField, ref _bhaColor, ref value);
            }
        }
        public bool IsToolDetailsVisible
        {
            get { return _isToolDetailsVisible; }
            set
            {
                SetProperty<bool>(IsToolDetailsVisibleField, ref _isToolDetailsVisible, ref value);
            }
        }
        public string ActualToolDescription
        {
            get { return _actualToolDescription; }
            set
            {
                SetProperty<string>(actualToolDescriptionField, ref _actualToolDescription, ref value);
            }
        }

        /// <summary>
        /// Gets or sets ShowStandpipeVsFlowRateScaling
        /// </summary>
        /// <value>
        /// ShowStandpipeVsFlowRateScaling
        /// </value>
        public bool ShowStandpipeVsFlowRateScaling
        {
            get { return _showStandpipeVsFlowRateScaling; }
            set
            {
                SetProperty<bool>(showStandpipeVsFlowRateScalingField, ref _showStandpipeVsFlowRateScaling, ref value);
            }
        }

        /// <summary>
        /// Gets or sets PressureLowerDisplayValue
        /// </summary>
        /// <value>
        /// PressureLowerDisplayValue
        /// </value>
        public double PressureLowerDisplayValue
        {
            get { return _pressureLowerDisplayValue; }
            set
            {
                SetProperty(pressureLowerDisplayValueField, ref _pressureLowerDisplayValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets PressureUpperDisplayValue
        /// </summary>
        /// <value>
        /// PressureUpperDisplayValue
        /// </value>
        public double PressureUpperDisplayValue
        {
            get { return _pressureUpperDisplayValue; }
            set
            {
                SetProperty(pressureUpperDisplayValueField, ref _pressureUpperDisplayValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets FlowRateLowerDisplayValue
        /// </summary>
        /// <value>
        /// FlowRateLowerDisplayValue
        /// </value>
        public double FlowRateLowerDisplayValue
        {
            get { return _flowRateLowerDisplayValue; }
            set
            {
                SetProperty(flowRateLowerDisplayValueField, ref _flowRateLowerDisplayValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets FlowRateUpperDisplayValue
        /// </summary>
        /// <value>
        /// FlowRateUpperDisplayValue
        /// </value>
        public double FlowRateUpperDisplayValue
        {
            get { return _flowRateUpperDisplayValue; }
            set
            {
                SetProperty(flowRateUpperDisplayValueField, ref _flowRateUpperDisplayValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets PressureRangeMinAppliedValue
        /// </summary>
        /// <value>
        /// PressureRangeMinAppliedValue
        /// </value>
        public double? PressureRangeMinAppliedValue
        {
            get { return _pressureRangeMinAppliedValue; }
            set
            {
                SetProperty<double?>(pressureRangeMinAppliedValueField, ref _pressureRangeMinAppliedValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets PressureRangeMaxAppliedValue
        /// </summary>
        /// <value>
        /// PressureRangeMaxAppliedValue
        /// </value>
        public double? PressureRangeMaxAppliedValue
        {
            get { return _pressureRangeMaxAppliedValue; }
            set
            {
                SetProperty<double?>(pressureRangeMaxAppliedValueField, ref _pressureRangeMaxAppliedValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets FlowRateRangeMinAppliedValue
        /// </summary>
        /// <value>
        /// FlowRateRangeMinAppliedValue
        /// </value>
        public double? FlowRateRangeMinAppliedValue
        {
            get { return _flowRateRangeMinAppliedValue; }
            set
            {
                SetProperty<double?>(flowRateRangeMinAppliedValueField, ref _flowRateRangeMinAppliedValue, ref value);
            }
        }

        /// <summary>
        /// Gets or sets FlowRateRangeMaxAppliedValue
        /// </summary>
        /// <value>
        /// FlowRateRangeMaxAppliedValue
        /// </value>
        public double? FlowRateRangeMaxAppliedValue
        {
            get { return _flowRateRangeMaxAppliedValue; }
            set
            {
                SetProperty<double?>(flowRateRangeMaxAppliedValueField, ref _flowRateRangeMaxAppliedValue, ref value);
            }
        }

        #endregion

        #region Constructor
        public HydraulicOutputBHAViewModel()
        {
            InitializeProperties();
        }
        public HydraulicOutputBHAViewModel(BHATool bha, System.Drawing.Color? bhaToolColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRate, double? maxStandpipePressure, List<BHATool> bhaTools, double? inputFlowRate)
        {
            InitializeProperties();
            SetHydraulicBHAOutput(bha, bhaToolColor, inputFlowRate);
            _bhaToolObjectFromHydraulicEngine = bha;
            _fluidDataFromHydraulicEngine = fluidDataFromHydraulicEngine;
            _maxFlowRate = (double)maxFlowRate;
            _maxPressure = (double)maxStandpipePressure;
            _bhaTools = bhaTools;
        }
        private void InitializeProperties()
        {
            LengthBHA = 0.00;
            OuterDiameter = 0.00;
            InputFlowRate = 0.00;
            AverageVelocity = 0.00;
            CriticalVelocity = 0.00;
            BHAPressureDrop = 0.00;

            PressureLowerDisplayValue = 0.00;
            PressureUpperDisplayValue = 0.00;
            FlowRateLowerDisplayValue = 0.00;
            FlowRateUpperDisplayValue = 0.00;
            IsPlusMinus = "+";
            _maxPressure = 0.00;
            _maxFlowRate = 0.00;
        }
        #endregion

        #region Methods
        public void ChangeToolDetailVisibility()
        {
            IsToolDetailsVisible = !_isToolDetailsVisible;
        }

        /// <summary>
        /// Method for DisplaySPVsFRScalingOptionsCommand
        /// </summary>
        private void DisplaySPVsFRScalingOptions()
        {
            this.ShowStandpipeVsFlowRateScaling = true;
        }
        /// <summary>
        /// Method for SetSPVsFRScalingCommand
        /// </summary>
        private void SetSPVsFRScaling()
        {
            PressureRangeMinAppliedValue = double.IsNaN(PressureLowerDisplayValue) ? Convert.ToDouble(PressureLowerDisplayValue) : 0.00;
            PressureRangeMaxAppliedValue = double.IsNaN(PressureUpperDisplayValue) ? Convert.ToDouble(PressureUpperDisplayValue) : 0.00;
            FlowRateRangeMinAppliedValue = double.IsNaN(FlowRateLowerDisplayValue) ? Convert.ToDouble(FlowRateLowerDisplayValue) : 0.00;
            FlowRateRangeMaxAppliedValue = double.IsNaN(FlowRateUpperDisplayValue) ? Convert.ToDouble(FlowRateUpperDisplayValue) : 0.00;

            this.ShowStandpipeVsFlowRateScaling = false;
        }
        /// <summary>
        /// Method for ResetSPVsFRScalingCommand
        /// </summary>
        private void ResetSPVsFRScaling()
        {
            this.PressureLowerDisplayValue = 0;
            this.PressureUpperDisplayValue = 0;
            this.FlowRateLowerDisplayValue = 0;
            this.FlowRateUpperDisplayValue = 0;
            SetSPVsFRScaling();
        }
        /// <summary>
        /// Method for CancelSPVsFRScalingCommand
        /// </summary>
        private void CancelSPVsFRScaling()
        {
            this.PressureLowerDisplayValue = (double)PressureRangeMinAppliedValue;
            this.PressureUpperDisplayValue = (double)PressureRangeMaxAppliedValue;
            this.FlowRateLowerDisplayValue = (double)FlowRateRangeMinAppliedValue;
            this.FlowRateUpperDisplayValue = (double)FlowRateRangeMaxAppliedValue;
            this.ShowStandpipeVsFlowRateScaling = false;
        }


        public double? CalculateStandPipePressureForTool(double? flowRateBaseValue)
        {
            if (flowRateBaseValue.HasValue)
            {
                _bhaToolObjectFromHydraulicEngine.CalculateHydraulics(_fluidDataFromHydraulicEngine, (double)flowRateBaseValue, bhaTools: _bhaTools);
                return _bhaToolObjectFromHydraulicEngine.BHAHydraulicsOutput.PressureDropInPSI;
            }
            return null;
        }


        public virtual Dictionary<string, List<XYValueModelForLineData<double>>> GetSPvsFRChartData()
        {
            Dictionary<string, List<XYValueModelForLineData<double>>> result = new Dictionary<string, List<XYValueModelForLineData<double>>>();

            var observedSeries = StandpipeVsFlowRateChart.SeriesValues.Where(p => p.SeriesName == "HydraproLineSeries").FirstOrDefault();

            if (observedSeries != null)
            {
                result.Add("HydraproLineSeries", observedSeries.SeriesDataSource.Cast<XYValueModelForLineData<double>>().ToList());
            }
            return result;
        }
        public virtual Dictionary<string, AnnotationModel<double>> GetAnnotationMarkers()
        {
            Dictionary<string, AnnotationModel<double>> result = new Dictionary<string, AnnotationModel<double>>();

            if (StandpipeVsFlowRateChart.AnnotationValues.ContainsKey("OperatingPoint"))
            {
                result.Add("Operating Point", StandpipeVsFlowRateChart.AnnotationValues["OperatingPoint"].FirstOrDefault());
            }

            return result;
        }
        public void PlotOperatingPoint(double flowRate, double pressureDrop)
        {
            this.StandpipeVsFlowRateChart.ClearAnnotation("OperatingPoint");
            AnnotationModel<double> annotationForOperatingPoint = new AnnotationModel<double>();
            annotationForOperatingPoint.AnnotationText = "+";
            annotationForOperatingPoint.PrimaryAxisValue = flowRate;
            annotationForOperatingPoint.SecondaryAxisValue = pressureDrop;
            this.StandpipeVsFlowRateChart.AddAnnoation("OperatingPoint", annotationForOperatingPoint);
        }

        public virtual List<Array> PlotChart()
        {
            List<Array> innerBHADataPoints = new List<Array>();
            double flowrate = 0;
            double lastRecordedStandpipePressure = 0;

            List<XYValueModel<double>> standpipePressureList = new List<XYValueModel<double>>();

            int exitLoopCounter = 0;
            double currentSecondaryAxisValue = 0;

            StandpipeVsFlowRateChart.RemoveSeries("HydraproLineSeries");

            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "DarkBlue",
                SeriesName = "HydraproLineSeries",

            });

            flowrate = 0;
            do
            {
                XYValueModelForLineData<double> valuemodelForLineData = new XYValueModelForLineData<double>();

                lastRecordedStandpipePressure = (double)CalculateStandPipePressureForTool(flowrate);


                valuemodelForLineData.PrimaryAxisValue = Convert.ToDouble(flowrate);
                valuemodelForLineData.SecondaryAxisValue = currentSecondaryAxisValue = Convert.ToDouble(lastRecordedStandpipePressure);

                standpipePressureList.Add(valuemodelForLineData);

                if (currentSecondaryAxisValue == 0)
                {
                    exitLoopCounter++;
                }
                flowrate += 1;
                if (exitLoopCounter == 1000)
                {
                    exitLoopCounter = 0;
                    break;
                }
            }
            while (lastRecordedStandpipePressure < 1.2 * this._maxPressure && flowrate < 1.2 * this._maxFlowRate);
            

            //TODO: check plotting operating point logic...
            if (standpipePressureList.Count > 0 && InputFlowRate > 0)
            {
                var operatingPointFlowRate = Convert.ToDouble(InputFlowRate);
                var operatingPoint = standpipePressureList.Where(o => o.PrimaryAxisValue < operatingPointFlowRate).LastOrDefault();
                if (operatingPoint != null)
                {
                    this.PlotOperatingPoint(operatingPoint.PrimaryAxisValue, ((XYValueModelForLineData<double>)operatingPoint).SecondaryAxisValue);
                }
            }

            StandpipeVsFlowRateChart.AddBulkValue("HydraproLineSeries", standpipePressureList);
            innerBHADataPoints.Add(standpipePressureList.ToArray());
            return innerBHADataPoints;
        }

        public virtual void SetTypeSpecificInfo(BHATool bha)
        {

        }
        public void SetHydraulicBHAOutput(BHATool bha, System.Drawing.Color? bhaColor, double? inputFlowRate)
        {
            ToolID = bha.ToolIdentifier;
            Workstring = bha.toolDescription;
            LengthBHA = bha.LengthInFeet;
            OuterDiameter = bha.OutsideDiameterInInch;
            InputFlowRate = (double)inputFlowRate;
            AverageVelocity = bha.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond;
            CriticalVelocity = bha.BHAHydraulicsOutput.CriticalVelocityInFeetPerSecond;
            FlowType = bha.BHAHydraulicsOutput.FlowType;
            BHAPressureDrop = bha.BHAHydraulicsOutput.PressureDropInPSI;
            //ActualToolDescription = bha.toolDescription;
            SetAverageVelocityColor();
            if (bhaColor.HasValue)
            {
                BHAColor = bhaColor.Value.Name.ToString();
            }
            SetToolImageSource(bha);
        }

        public void SetToolImageSource(BHATool bha)
        {
            if (bha is BHAToolType6)
            {
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Red)
                {
                    /*
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedLeftDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                        //ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedLeftTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedCloseTool.png";
                    */
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Green)
                {
                    /*
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightGreenLeftDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightGreenLeftTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightGreenDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedCloseTool.png";
                     */
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Yellow)
                {
                    /*
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightYellowLeftDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightYellowLeftTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightYellowDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightYellowCloseTool.png";
                     */
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Transparent)
                {
                    /*
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightTransparentLeftDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
                       // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightTransparentLeftTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                      //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightTransparentDownTool.png";
                    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
                        //  ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightTransparentCloseTool.png";
                    */
                }
            }
            else
            {
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Red)
                {
                   // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightRedDownTool.png";
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Green)
                {
                   // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightGreenDownTool.png";
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Yellow)
                {
                   // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightYellowDownTool.png";
                }
                if (AverageVelocityColor == ChartAndGraphService.ColorStrength.Transparent)
                {
                   // ToolSourcePath = System.Windows.Forms.Application.StartupPath + @"\Resources\LightTransparentDownTool.png";
                }
            }

        }

        public void SetAverageVelocityColor()
        {
            if (AverageVelocity > 0)
            {
                if (AverageVelocity > 0 && CriticalVelocity> 0)
                {
                    if (AverageVelocity >= 120)
                    {
                        AverageVelocityColor = ChartAndGraphService.ColorStrength.Red;
                    }
                    else if (AverageVelocity >= 70 && AverageVelocity < 120)
                    {
                        AverageVelocityColor = ChartAndGraphService.ColorStrength.Yellow;
                    }
                    else if (AverageVelocity < 70)
                    {
                        AverageVelocityColor = ChartAndGraphService.ColorStrength.Green;
                    }
                    else
                    {
                        AverageVelocityColor = ChartAndGraphService.ColorStrength.Transparent;
                    }
                }
            }
        }
       #endregion
    }
    
    public static class HydraulicToolTypeObjectManager
    {
        public static HydraulicOutputBHAViewModel GetToolTypeObject(BHATool bha, System.Drawing.Color? bhaColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRateBase, double? maxPressureBase, List<BHATool> bhaTools, double? inputFlowRate)
        {
            HydraulicOutputBHAViewModel HydraulicBHA = null;
            if (bha is BHAToolType1)
            {
                return HydraulicBHA = new HydraulicTypeOneViewModel(bha, bhaColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate);
            }
            else if (bha is BHAToolType3)
            {
                return HydraulicBHA = new HydraulicTypeThreeViewModel(bha, bhaColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate);
            }
            else if (bha is BHAToolType6)
            {
                return HydraulicBHA = new HydraulicTypeSixViewModel(bha, bhaColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate);
            }
            else if (bha is BHAToolType10)
            {
                return HydraulicBHA = new HydraulicTypeTenViewModel(bha, bhaColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate);
            }
            else
                return HydraulicBHA = new HydraulicTypeOneViewModel(bha, bhaColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate);
        }
    }
}