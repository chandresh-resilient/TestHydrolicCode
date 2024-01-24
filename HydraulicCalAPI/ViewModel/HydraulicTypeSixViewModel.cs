using HydraulicEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFT.UI.Common.Charts;
using WFT.UnitConversion.UI;
using WFT.UI.Common.Base;
using HydraulicCalAPI.Service;
using HydraulicCalAPI.ViewModel;

namespace HydraulicCalAPI.ViewModel
{
    public class HydraulicTypeSixViewModel : HydraulicOutputBHAViewModel
    {
        #region Private
        private DoubleWithUnitConversionViewModel _internalPressureDrop;
        private DoubleWithUnitConversionViewModel _regionOnePressureDrop;
        private DoubleWithUnitConversionViewModel _regionOneFlowRate;
        private DoubleWithUnitConversionViewModel _regionTwoPressureDrop;
        private DoubleWithUnitConversionViewModel _regionTwoFlowRate;
        private DoubleWithUnitConversionViewModel _regionThreePressureDrop;
        private DoubleWithUnitConversionViewModel _regionThreeFlowRate;
        private ChartViewModel<double> _standpipeVsFlowRateChartRegionOne = new ChartViewModel<double>();
        private ChartViewModel<double> _standpipeVsFlowRateChartRegionTwo = new ChartViewModel<double>();
        private ChartViewModel<double> _standpipeVsFlowRateChartRegionThree = new ChartViewModel<double>();
        private DoubleWithUnitConversionViewModel _annulusNozzleVelocity;
        private DoubleWithUnitConversionViewModel _bhaNozzleVelocity;
        private ControlCutConstants.ColorStrength _annulusNozzleVelocityColor;
        private ControlCutConstants.ColorStrength _bhaNozzleVelocityColor;
        private string _displayImage;
        List<XYValueModel<double>> standpipePressureListRegion1 = new List<XYValueModel<double>>();
        List<XYValueModel<double>> standpipePressureListRegion2 = new List<XYValueModel<double>>();
        List<XYValueModel<double>> standpipePressureListRegion3 = new List<XYValueModel<double>>();
        #endregion

        #region Constant
        private const string displayImageField = "DisplayImage";
        private const string internalPressureDropField = "InternalPressureDrop";
        private const string regionOnePressureDropField = "RegionOnePressureDrop";
        private const string regionOneFlowRateField = "RegionOneFlowRate";
        private const string regionTwoPressureDropField = "RegionTwoPressureDrop";
        private const string regionTwoFlowRateField = "RegionTwoFlowRate";
        private const string regionThreePressureDropField = "RegionThreePressureDrop";
        private const string regionThreeFlowRateField = "RegionThreeFlowRate";
        private const string standpipeVsFlowRateChartRegionOneField = "StandpipeVsFlowRateChartRegionOne";
        private const string standpipeVsFlowRateChartRegionTwoField = "StandpipeVsFlowRateChartRegionTwo";
        private const string standpipeVsFlowRateChartRegionThreeField = "StandpipeVsFlowRateChartRegionThree";
        private const string annulusNozzleVelocityField = "AnnulusNozzleVelocity";
        private const string bhaNozzleVelocityField = "BHANozzleVelocity";
        private const string annulusNozzleVelocityColorField = "AnnulusNozzleVelocityColor";
        private const string bhaNozzleVelocityColorField = "BHANozzleVelocityColor";
        #endregion
        readonly ViewModelBase objvmb;
       
        #region Properties
        public ChartViewModel<double> StandpipeVsFlowRateChartRegionOne
        {
            get { return _standpipeVsFlowRateChartRegionOne; }
            set
            {
                SetProperty<ChartViewModel<double>>(standpipeVsFlowRateChartRegionOneField, ref _standpipeVsFlowRateChartRegionOne, ref value);
            }
        }
        public ChartViewModel<double> StandpipeVsFlowRateChartRegionTwo
        {
            get { return _standpipeVsFlowRateChartRegionTwo; }
            set
            {
                SetProperty<ChartViewModel<double>>(standpipeVsFlowRateChartRegionTwoField, ref _standpipeVsFlowRateChartRegionTwo, ref value);
            }
        }
        public ChartViewModel<double> StandpipeVsFlowRateChartRegionThree
        {
            get { return _standpipeVsFlowRateChartRegionThree; }
            set
            {
                SetProperty<ChartViewModel<double>>(standpipeVsFlowRateChartRegionOneField, ref _standpipeVsFlowRateChartRegionThree, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel RegionOnePressureDrop
        {
            get { return _regionOnePressureDrop; }
            set
            {
                SetProperty(regionOnePressureDropField, ref _regionOnePressureDrop, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel InternalPressureDrop
        {
            get { return _internalPressureDrop; }
            set
            {
                SetProperty(internalPressureDropField, ref _internalPressureDrop, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel RegionOneFlowRate
        {
            get { return _regionOneFlowRate; }
            set
            {
                SetProperty(regionOneFlowRateField, ref _regionOneFlowRate, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel RegionTwoPressureDrop
        {
            get { return _regionTwoPressureDrop; }
            set
            {
                SetProperty(regionTwoPressureDropField, ref _regionTwoPressureDrop, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel RegionTwoFlowRate
        {
            get { return _regionTwoFlowRate; }
            set
            {
                SetProperty(regionTwoFlowRateField, ref _regionTwoFlowRate, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel RegionThreePressureDrop
        {
            get { return _regionThreePressureDrop; }
            set
            {
                SetProperty(regionThreePressureDropField, ref _regionThreePressureDrop, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel RegionThreeFlowRate
        {
            get { return _regionThreeFlowRate; }
            set
            {
                SetProperty(regionThreeFlowRateField, ref _regionThreeFlowRate, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel AnnulusNozzleVelocity
        {
            get { return _annulusNozzleVelocity; }
            set
            {
                SetProperty(annulusNozzleVelocityField, ref _annulusNozzleVelocity, ref value);
            }
        }

        public ControlCutConstants.ColorStrength AnnulusNozzleVelocityColor
        {
            get { return _annulusNozzleVelocityColor; }
            set
            {
                SetProperty<ControlCutConstants.ColorStrength>(annulusNozzleVelocityColorField, ref _annulusNozzleVelocityColor, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel BHANozzleVelocity
        {
            get { return _bhaNozzleVelocity; }
            set
            {
                SetProperty(bhaNozzleVelocityField, ref _bhaNozzleVelocity, ref value);
            }
        }

        public ControlCutConstants.ColorStrength BHANozzleVelocityColor
        {
            get { return _bhaNozzleVelocityColor; }
            set
            {
                SetProperty<ControlCutConstants.ColorStrength>(bhaNozzleVelocityColorField, ref _bhaNozzleVelocityColor, ref value);
            }
        }

        public string DisplayImage
        {
            get { return _displayImage; }
            set { SetProperty<string>(displayImageField, ref _displayImage, ref value); }
        }

        public double Pressure { get; private set; }
        public double FlowRate { get; private set; }
        #endregion
        public HydraulicTypeSixViewModel(BHATool bha, System.Drawing.Color? bhaToolColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRateBase, double? maxPressureBase, List<BHATool> bhaTools, double? inputFlowRate)
            : base(bha, bhaToolColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase, bhaTools, inputFlowRate)
        {
            InitializeProperties();
            SetTypeSpecificInfo(bha);            
        }


        private void InitializeProperties()
        {
            InternalPressureDrop = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
            RegionOnePressureDrop = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
            RegionOneFlowRate = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.FlowRate);
            RegionTwoPressureDrop = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
            RegionTwoFlowRate = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.FlowRate);
            RegionThreePressureDrop = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
            RegionThreeFlowRate = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.FlowRate);
            AnnulusNozzleVelocity = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.TubularVelocity);
            BHANozzleVelocity = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.TubularVelocity);
        }

        public override void SetTypeSpecificInfo(BHATool bha)
        {
            InternalPressureDrop.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.TotalPressureDropInPSI;
            RegionOnePressureDrop.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.ToolPressureDropInPSI;
            RegionOneFlowRate.BaseValue = InputFlowRate;
            RegionTwoPressureDrop.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.AnnulusOpeningPressureDropInPSI;
            RegionTwoFlowRate.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.AnnulusOpeningFlowrateInGPM;
            RegionThreePressureDrop.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.BHAOpeningPressureDropInPSI;
            RegionThreeFlowRate.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.BHAOpeningFlowrateInGPM;
            AnnulusNozzleVelocity.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.AnnulusNozzleVelocityInFeetPerSecond;
            BHANozzleVelocity.BaseValue = (bha as BHAToolType6).BHAHydraulicsOutput.BHANozzleVelocityInFeetPerSecond;
            SetNozzleVelocityColor();
            //SetImagePath(bha);
        }
        //private void SetImagePath(BHATool bha)
        //{
        //    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
        //        DisplayImage = System.Windows.Forms.Application.StartupPath + @"\Resources\FishingTypeSixOutputBHATool.jpg";
        //    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.OpenToAnnulus)
        //        DisplayImage = System.Windows.Forms.Application.StartupPath + @"\Resources\TypeSixOutputOpenToAnnulus.jpg";
        //    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.OpenToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
        //        DisplayImage = System.Windows.Forms.Application.StartupPath + @"\Resources\TypeSixOutputOpenToBHA.jpg";
        //    if ((bha as BHAToolType6).BHAOpeningState == HydraulicEngine.Common.ToolState.CloseToAnnulus && (bha as BHAToolType6).CurrentState == HydraulicEngine.Common.ToolState.CloseToAnnulus)
        //        DisplayImage = System.Windows.Forms.Application.StartupPath + @"\Resources\TypeSixOutputCloseToBoth.jpg";
        //}

        public override Dictionary<string, List<XYValueModelForLineData<double>>> GetSPvsFRChartData()
        {
            Dictionary<string, List<XYValueModelForLineData<double>>> result = new Dictionary<string, List<XYValueModelForLineData<double>>>();

            var observedSeriesRegionOne = StandpipeVsFlowRateChart.SeriesValues.Where(p => p.SeriesName == "HydraproRegionOneLineSeries").FirstOrDefault();            
            if (observedSeriesRegionOne != null)
            {
                List<XYValueModel<double>> tempStandpipePressureListRegion1 = new List<XYValueModel<double>>();
                foreach (var item in standpipePressureListRegion1)
                {
                    var tempItem1 = tempStandpipePressureListRegion1.Where(o => o.PrimaryAxisValue == item.PrimaryAxisValue).FirstOrDefault();
                    if (tempItem1 == null)
                        tempStandpipePressureListRegion1.Add(item);
                }
                result.Add("HydraproRegionOneLineSeries", tempStandpipePressureListRegion1.Cast<XYValueModelForLineData<double>>().ToList());
            }
            var observedSeriesRegionTwo = StandpipeVsFlowRateChart.SeriesValues.Where(p => p.SeriesName == "HydraproRegionTwoLineSeries").FirstOrDefault();
            if (observedSeriesRegionTwo != null)
            {
                List<XYValueModel<double>> tempStandpipePressureListRegion2 = new List<XYValueModel<double>>();
                foreach (var item in standpipePressureListRegion2)
                {
                    var tempItem2 = tempStandpipePressureListRegion2.Where(o => o.PrimaryAxisValue == item.PrimaryAxisValue).FirstOrDefault();
                    if (tempItem2 == null)
                        tempStandpipePressureListRegion2.Add(item);
                }
                result.Add("HydraproRegionTwoLineSeries", tempStandpipePressureListRegion2.Cast<XYValueModelForLineData<double>>().ToList());
            }
            var observedSeriesRegionThree = StandpipeVsFlowRateChart.SeriesValues.Where(p => p.SeriesName == "HydraproRegionThreeLineSeries").FirstOrDefault();
            if (observedSeriesRegionThree != null)
            {
                List<XYValueModel<double>> tempStandpipePressureListRegion3 = new List<XYValueModel<double>>();
                foreach (var item in standpipePressureListRegion3)
                {
                    var tempItem3 = tempStandpipePressureListRegion3.Where(o => o.PrimaryAxisValue == item.PrimaryAxisValue).FirstOrDefault();
                    if (tempItem3 == null)
                        tempStandpipePressureListRegion3.Add(item);
                }
                result.Add("HydraproRegionThreeLineSeries", tempStandpipePressureListRegion3.Cast<XYValueModelForLineData<double>>().ToList());
            }
            return result;
        }
        public override Dictionary<string, AnnotationModel<double>> GetAnnotationMarkers()
        {
            Dictionary<string, AnnotationModel<double>> result = new Dictionary<string, AnnotationModel<double>>();

            if (StandpipeVsFlowRateChart.AnnotationValues.ContainsKey("OperatingPoint") && StandpipeVsFlowRateChart.AnnotationValues["OperatingPoint"].Count > 0)
            {
                int count = 0;
                foreach (var item in StandpipeVsFlowRateChart.AnnotationValues["OperatingPoint"])
                {
                    count++;
                    result.Add("Operating" + count.ToString() + " Point", item);
                    
                }                
            }
            return result;
        }
        private void SetNozzleVelocityColor()
        {
            if (AnnulusNozzleVelocity.BaseValue.HasValue)
            {
                if (AnnulusNozzleVelocity.BaseValue >= 230)
                {
                    AnnulusNozzleVelocityColor = ControlCutConstants.ColorStrength.Red;
                }
                else if (AnnulusNozzleVelocity.BaseValue >= 190 && AnnulusNozzleVelocity.BaseValue < 230)
                {
                    AnnulusNozzleVelocityColor = ControlCutConstants.ColorStrength.Yellow;
                }
                else if (AnnulusNozzleVelocity.BaseValue < 190)
                {
                    AnnulusNozzleVelocityColor = ControlCutConstants.ColorStrength.Green;
                }
                else
                {
                    AnnulusNozzleVelocityColor = ControlCutConstants.ColorStrength.Transparent;
                }
            }
            if (BHANozzleVelocity.BaseValue.HasValue)
            {
                if (BHANozzleVelocity.BaseValue >= 230)
                {
                    BHANozzleVelocityColor = ControlCutConstants.ColorStrength.Red;
                }
                else if (BHANozzleVelocity.BaseValue >= 190 && BHANozzleVelocity.BaseValue < 230)
                {
                    BHANozzleVelocityColor = ControlCutConstants.ColorStrength.Yellow;
                }
                else if (BHANozzleVelocity.BaseValue < 190)
                {
                    BHANozzleVelocityColor = ControlCutConstants.ColorStrength.Green;
                }
                else
                {
                    BHANozzleVelocityColor = ControlCutConstants.ColorStrength.Transparent;
                }
            }
        }

        public override List<Array> PlotChart()
        {
            List<Array> objLstTypeSixdatapoints = new List<Array>();
            
            
            double flowrate = (double)FlowRate;
            double lastRecordedStandpipePressure = (double)Pressure;

            standpipePressureListRegion1 = new List<XYValueModel<double>>();
            standpipePressureListRegion2 = new List<XYValueModel<double>>();
            standpipePressureListRegion3 = new List<XYValueModel<double>>();

            int exitLoopCounter = 0;
            double currentSecondaryAxisValue = 0;

            StandpipeVsFlowRateChart.RemoveSeries("HydraproRegionOneLineSeries");

            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Blue",
                SeriesName = "HydraproRegionOneLineSeries",

            });
            StandpipeVsFlowRateChart.RemoveSeries("HydraproRegionTwoLineSeries");

            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Black",
                SeriesName = "HydraproRegionTwoLineSeries",

            });
            StandpipeVsFlowRateChart.RemoveSeries("HydraproRegionThreeLineSeries");

            StandpipeVsFlowRateChart.AddSeries(new SeriesModel<XYValueModel<double>, double>()
            {
                SeriesType = "Line",
                Color = "Purple",
                SeriesName = "HydraproRegionThreeLineSeries",

            });

            flowrate = 0;
            do
            {
                XYValueModelForLineData<double> valuemodelForRegion1LineData = new XYValueModelForLineData<double>();
                XYValueModelForLineData<double> valuemodelForRegion2LineData = new XYValueModelForLineData<double>();
                XYValueModelForLineData<double> valuemodelForRegion3LineData = new XYValueModelForLineData<double>();

                IBHAToolType6HydraulicsOutput BHAHydraulicsOutput = CalculateStandPipePressureForTool(Convert.ToDouble(flowrate));
                lastRecordedStandpipePressure = (double)BHAHydraulicsOutput.TotalPressureDropInPSI;


                valuemodelForRegion1LineData.PrimaryAxisValue = Convert.ToDouble(flowrate);
                valuemodelForRegion1LineData.SecondaryAxisValue = currentSecondaryAxisValue = Convert.ToDouble(lastRecordedStandpipePressure);
                standpipePressureListRegion1.Add(valuemodelForRegion1LineData);

                valuemodelForRegion2LineData.PrimaryAxisValue = Convert.ToDouble(BHAHydraulicsOutput.AnnulusOpeningFlowrateInGPM);
                valuemodelForRegion2LineData.SecondaryAxisValue = Convert.ToDouble(BHAHydraulicsOutput.AnnulusOpeningPressureDropInPSI);
                standpipePressureListRegion2.Add(valuemodelForRegion2LineData);

                valuemodelForRegion3LineData.PrimaryAxisValue = Convert.ToDouble(BHAHydraulicsOutput.BHAOpeningFlowrateInGPM);
                valuemodelForRegion3LineData.SecondaryAxisValue = Convert.ToDouble(BHAHydraulicsOutput.BHAOpeningPressureDropInPSI);
                standpipePressureListRegion3.Add(valuemodelForRegion3LineData);

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
            this.StandpipeVsFlowRateChart.ClearAnnotation("OperatingPoint");
            if (standpipePressureListRegion1.Count > 0 && !string.IsNullOrWhiteSpace(RegionOneFlowRate.DisplayValue))
            {
                var operatingPointFlowRate1 = Convert.ToDouble(RegionOneFlowRate.DisplayValue);
                var operatingPoint1 = standpipePressureListRegion1.Where(o => o.PrimaryAxisValue < operatingPointFlowRate1).LastOrDefault();
                if (operatingPoint1 != null)
                {
                    this.PlotOperatingPoint(operatingPoint1.PrimaryAxisValue, ((XYValueModelForLineData<double>)operatingPoint1).SecondaryAxisValue);
                }
            }
            if (standpipePressureListRegion2.Count > 0 && !string.IsNullOrWhiteSpace(RegionTwoFlowRate.DisplayValue))
            {
                var operatingPointFlowRate2 = Convert.ToDouble(RegionTwoFlowRate.DisplayValue);
                var operatingPoint2 = standpipePressureListRegion2.Where(o => o.PrimaryAxisValue < operatingPointFlowRate2).LastOrDefault();
                if (operatingPoint2 != null)
                {
                    this.PlotOperatingPoint(operatingPoint2.PrimaryAxisValue, ((XYValueModelForLineData<double>)operatingPoint2).SecondaryAxisValue);
                }
            }
            if (standpipePressureListRegion3.Count > 0 && !string.IsNullOrWhiteSpace(RegionThreeFlowRate.DisplayValue))
            {
                var operatingPointFlowRate3 = Convert.ToDouble(RegionThreeFlowRate.DisplayValue);
                var operatingPoint3 = standpipePressureListRegion3.Where(o => o.PrimaryAxisValue < operatingPointFlowRate3).LastOrDefault();
                if (operatingPoint3 != null)
                {
                    this.PlotOperatingPoint(operatingPoint3.PrimaryAxisValue, ((XYValueModelForLineData<double>)operatingPoint3).SecondaryAxisValue);
                }
            }
            StandpipeVsFlowRateChart.AddBulkValue("HydraproRegionOneLineSeries", standpipePressureListRegion1);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproRegionTwoLineSeries", standpipePressureListRegion2);
            StandpipeVsFlowRateChart.AddBulkValue("HydraproRegionThreeLineSeries", standpipePressureListRegion3);

            objLstTypeSixdatapoints.Add(standpipePressureListRegion1.ToArray());
            objLstTypeSixdatapoints.Add(standpipePressureListRegion2.ToArray());
            objLstTypeSixdatapoints.Add(standpipePressureListRegion3.ToArray());

            return objLstTypeSixdatapoints;
        }

        private void PlotOperatingPoint(double flowRate, double pressureDrop)
        {
            AnnotationModel<double> annotationForOperatingPoint = new AnnotationModel<double>();
            annotationForOperatingPoint.AnnotationText = "+";
            annotationForOperatingPoint.PrimaryAxisValue = flowRate;
            annotationForOperatingPoint.SecondaryAxisValue = pressureDrop;
            this.StandpipeVsFlowRateChart.AddAnnoation("OperatingPoint", annotationForOperatingPoint);
        }
        private IBHAToolType6HydraulicsOutput CalculateStandPipePressureForTool(double? flowRateBaseValue)
        {
            if (flowRateBaseValue.HasValue)
            {
                _bhaToolObjectFromHydraulicEngine.CalculateHydraulics(_fluidDataFromHydraulicEngine, flowRateBaseValue.Value, bhaTools: _bhaTools);
                return (_bhaToolObjectFromHydraulicEngine.BHAHydraulicsOutput as IBHAToolType6HydraulicsOutput);
            }
            return null;
        }
    }
}
