using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFT.UnitConversion.UI;
using HydraulicEngine;
using WFT.UI.Common.Charts;
using WFT.UI.Common.Base;
using WFT.UI.Common.Commands;
using HydraulicCalAPI.Service;
using HydraulicCalAPI.ViewModel;

namespace HydraulicCalAPI.ViewModel
{
    public class HydraulicTypeThreeViewModel : HydraulicOutputBHAViewModel
    {
        #region Private
        private double _hydraulicHorsePower;
        private double _nozzleVelocityInFeetPerSecond;
        private double _impactForceInPounds;
        private double _nozzlePressureDropInPSI;
        private ControlCutConstants.ColorStrength _nozzleVelocityColor;
        #endregion

        #region Constant
        private const string hydraulicHorsePowerField = "HydraulicHorsePower";
        private const string nozzleVelocityInFeetPerSecondField = "NozzleVelocityInFeetPerSecond";
        private const string impactForceInPoundsField = "ImpactForceInPounds";
        private const string nozzlePressureDropInPSIField = "NozzlePressureDropInPSI";
        private const string nozzleVelocityColorField = "NozzleVelocityColor";
        #endregion
        readonly ViewModelBase objvmb;

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
        public double HydraulicHorsePower
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
        public ControlCutConstants.ColorStrength NozzleVelocityColor
        {
            get { return _nozzleVelocityColor; }
            set
            {
                SetProperty<ControlCutConstants.ColorStrength>(nozzleVelocityColorField, ref _nozzleVelocityColor, ref value);
            }
        }
        #endregion
        public HydraulicTypeThreeViewModel(BHATool bha, System.Drawing.Color? bhaToolColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRateBase, double? maxPressureBase, List<BHATool> bhaTools, double? inputFlowRate)
            : base(bha, bhaToolColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase,bhaTools, inputFlowRate)
        {
            InitializeProperties();
            SetTypeSpecificInfo(bha);
        }


        private void InitializeProperties()
        {
            HydraulicHorsePower = new (double)Power;
            NozzleVelocityInFeetPerSecond = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.TubularVelocity);
            ImpactForceInPounds = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Force);
            NozzlePressureDropInPSI = new DoubleWithUnitConversionViewModel(ControlCutConstants.UnitSystemAttributes.Pressure);
        }

        public override void SetTypeSpecificInfo(BHATool bha)
        {
            HydraulicHorsePower.BaseValue = (bha as BHAToolType3).BHAHydraulicsOutput.HydraulicHorsePower;
            NozzleVelocityInFeetPerSecond.BaseValue = (bha as BHAToolType3).BHAHydraulicsOutput.NozzleVelocityInFeetPerSecond;
            ImpactForceInPounds.BaseValue = (bha as BHAToolType3).BHAHydraulicsOutput.ImpactForceInPounds;
            NozzlePressureDropInPSI.BaseValue = (bha as BHAToolType3).BHAHydraulicsOutput.NozzlePressureDropInPSI;
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
