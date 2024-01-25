using System.Collections.Generic;
using HydraulicEngine;
using WFT.UI.Common.Base;

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
        public double NozzleVelocityInFeetPerSecond
        {
            get { return _nozzleVelocityInFeetPerSecond; }
            set
            {
                SetProperty(nozzleVelocityInFeetPerSecondField, ref _nozzleVelocityInFeetPerSecond, ref value);
            }
        }

        public double ImpactForceInPounds
        {
            get { return _impactForceInPounds; }
            set
            {
                SetProperty(impactForceInPoundsField, ref _impactForceInPounds, ref value);
            }
        }
        public double NozzlePressureDropInPSI
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
            HydraulicHorsePower = 0;
            NozzleVelocityInFeetPerSecond = 0;
            ImpactForceInPounds = 0;
            NozzlePressureDropInPSI = 0;
        }

        public override void SetTypeSpecificInfo(BHATool bha)
        {
            HydraulicHorsePower = (bha as BHAToolType3).BHAHydraulicsOutput.HydraulicHorsePower;
            NozzleVelocityInFeetPerSecond = (bha as BHAToolType3).BHAHydraulicsOutput.NozzleVelocityInFeetPerSecond;
            ImpactForceInPounds = (bha as BHAToolType3).BHAHydraulicsOutput.ImpactForceInPounds;
            NozzlePressureDropInPSI = (bha as BHAToolType3).BHAHydraulicsOutput.NozzlePressureDropInPSI;
            SetNozzleVelocityColor();
        }
        private void SetNozzleVelocityColor()
        {
            if (double.IsNaN(NozzleVelocityInFeetPerSecond))
            {
                if (NozzleVelocityInFeetPerSecond >= 230)
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Red;
                }
                else if (NozzleVelocityInFeetPerSecond >= 190 && NozzleVelocityInFeetPerSecond < 230)
                {
                    NozzleVelocityColor = ControlCutConstants.ColorStrength.Yellow;
                }
                else if (NozzleVelocityInFeetPerSecond < 190)
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
