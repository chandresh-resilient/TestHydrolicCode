using HydraulicCalAPI.ViewModel;
using HydraulicEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFT.UnitConversion.UI;
using WFT.UI.Common.Base;

namespace HydraulicCalAPI.ViewModel
{
    public class HydraulicTypeTenViewModel : HydraulicOutputBHAViewModel
    {
        #region Private
        private DoubleWithUnitConversionViewModel _hydraulicHorsePower;
        private DoubleWithUnitConversionViewModel _nozzleVelocityInFeetPerSecond;
        private DoubleWithUnitConversionViewModel _impactForceInPounds;
        private DoubleWithUnitConversionViewModel _nozzlePressureDropInPSI;
        private DoubleWithUnitConversionViewModel _accusetPressureDropInPSI;
        private bool _hasAccuSet = false;
        private bool _hasNozzles = false;
        private bool _hasNothing = false;
        private ControlCutConstants.ColorStrength _nozzleVelocityColor;
        #endregion

        #region Constant
        private const string hydraulicHorsePowerField = "HydraulicHorsePower";
        private const string nozzleVelocityInFeetPerSecondField = "NozzleVelocityInFeetPerSecond";
        private const string impactForceInPoundsField = "ImpactForceInPounds";
        private const string nozzlePressureDropInPSIField = "NozzlePressureDropInPSI";
        private const string accusetPressureDropInPSIField = "AccusetPressureDropInPSI";
        private const string hasAccuSetField = "HasAccuSet";
        private const string hasNozzlesField = "HasNozzles";
        private const string hasNothingField = "HasNothing";
        private const string nozzleVelocityColorField = "NozzleVelocityColor";
        #endregion

        #region Properties
        public DoubleWithUnitConversionViewModel HydraulicHorsePower
        {
            get { return _hydraulicHorsePower; }
            set
            {
                SetProperty(hydraulicHorsePowerField, ref _hydraulicHorsePower, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel NozzleVelocityInFeetPerSecond
        {
            get { return _nozzleVelocityInFeetPerSecond; }
            set
            {
                SetProperty(nozzleVelocityInFeetPerSecondField, ref _nozzleVelocityInFeetPerSecond, ref value);
            }
        }

        public DoubleWithUnitConversionViewModel ImpactForceInPounds
        {
            get { return _impactForceInPounds; }
            set
            {
                SetProperty(impactForceInPoundsField, ref _impactForceInPounds, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel NozzlePressureDropInPSI
        {
            get { return _nozzlePressureDropInPSI; }
            set
            {
                SetProperty(nozzlePressureDropInPSIField, ref _nozzlePressureDropInPSI, ref value);
            }
        }
        public DoubleWithUnitConversionViewModel AccusetPressureDropInPSI
        {
            get { return _accusetPressureDropInPSI; }
            set
            {
                SetProperty(accusetPressureDropInPSIField, ref _accusetPressureDropInPSI, ref value);
            }
        }

        public bool HasAccuSet
        {
            get { return _hasAccuSet; }
            set
            {
                SetProperty<bool>(hasAccuSetField, ref _hasAccuSet, ref value);
            }
        }

        public bool HasNozzles
        {
            get { return _hasNozzles; }
            set
            {
                SetProperty<bool>(hasNozzlesField, ref _hasNozzles, ref value);
            }
        }

        public bool HasNothing
        {
            get { return _hasNothing; }
            set
            {
                SetProperty<bool>(hasNothingField, ref _hasNothing, ref value);
            }
        }
        public ControlCutConstants.ColorStrength NozzleVelocityColor
        {
            get { return _nozzleVelocityColor; }
            set
            {
                SetProperty<ControlCutConstants.ColorStrength>(nozzleVelocityColorField, ref _nozzleVelocityColor, ref value);
            }
        }
        #endregion
        public HydraulicTypeTenViewModel(BHATool bha, System.Drawing.Color? bhaToolColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRateBase, double? maxPressureBase, List<BHATool> bhaTools, double? inputFlowRate)
            : base(bha, bhaToolColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase,bhaTools,inputFlowRate)
        {
            InitializeProperties();
            SetTypeSpecificInfo(bha);
        }


        private void InitializeProperties()
        {
            HydraulicHorsePower = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Power);
            NozzleVelocityInFeetPerSecond = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.TubularVelocity);
            ImpactForceInPounds = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Force);
            NozzlePressureDropInPSI = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
            AccusetPressureDropInPSI = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
        }

        public override void SetTypeSpecificInfo(BHATool bha)
        {
            HydraulicHorsePower.BaseValue = (bha as BHAToolType10).BHAHydraulicsOutput.HydraulicHorsePower;
            NozzleVelocityInFeetPerSecond.BaseValue = (bha as BHAToolType10).BHAHydraulicsOutput.NozzleVelocityInFeetPerSecond;
            ImpactForceInPounds.BaseValue = (bha as BHAToolType10).BHAHydraulicsOutput.ImpactForceInPounds;
            NozzlePressureDropInPSI.BaseValue = (bha as BHAToolType10).BHAHydraulicsOutput.NozzlePressureDropInPSI;
            AccusetPressureDropInPSI.BaseValue = (bha as BHAToolType10).BHAHydraulicsOutput.AccusetPressureDropInPSI;
            HasAccuSet = (bha as BHAToolType10).ToolAccuset != null ? true : false;
            HasNozzles = (bha as BHAToolType10).NozzlesInfomation != null && (bha as BHAToolType10).NozzlesInfomation.Count > 0 ? true : false;
            HasNothing = HasAccuSet == false && HasNozzles == false ? true : false;
            SetNozzleVelocityColor();
        }
        private void SetNozzleVelocityColor()
        {
            if (NozzleVelocityInFeetPerSecond.BaseValue.HasValue)
            {
                if (NozzleVelocityInFeetPerSecond.BaseValue >= 230)
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Red;
                }
                else if (NozzleVelocityInFeetPerSecond.BaseValue >= 190 && NozzleVelocityInFeetPerSecond.BaseValue < 230)
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Yellow;
                }
                else if (NozzleVelocityInFeetPerSecond.BaseValue < 190)
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Green;
                }
                else
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Transparent;
                }
            }
        }
    }
}
