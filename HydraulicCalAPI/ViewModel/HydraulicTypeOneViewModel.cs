using HydraulicEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicCalAPI.ViewModel
{
    public class HydraulicTypeOneViewModel : HydraulicOutputBHAViewModel
    {
        public HydraulicTypeOneViewModel(BHATool bha, System.Drawing.Color? bhaToolColor, Fluid fluidDataFromHydraulicEngine, double? maxFlowRateBase, double? maxPressureBase,List<BHATool> bhaTools, double? inputFlowRate)
            : base(bha, bhaToolColor, fluidDataFromHydraulicEngine, maxFlowRateBase, maxPressureBase,bhaTools, inputFlowRate)
        {
        }
    }
}
