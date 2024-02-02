using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HydraulicEngine
{
    // A seprate interface is made to define output properties. This is done because each type of tool and different hydraulic outputs.
    // So, each type can define it's output properties in this interface. It becomes easy for the 
    public interface IBHAHydraulicsOutput
    {
        string FlowType { get; set; }
         double PressureDropInPSI { get; set; }
         double AverageVelocityInFeetPerSecond { get; set; }

         // This property by default will return double.MinValue. 
        //But the tool types which have either split flow or various states need to implement this property accordingly
         double OutputFlowInGallonsPerMinute { get; set; }

         double CriticalVelocityInFeetPerSecond { get; set; }
         double EquivalentCirculatingDensity {get; set; }
    }

    
    // Standard BHA class. All tool types need to derive from this class. This class in itself does not do any hydraulic calculation
    // Hydraulic calculations need to be implemented in the derived class
    public abstract class BHATool : IBHAHydraulicsOutput
    {
        // type cast the current class as Hydraulic output class so that user can browse through output properties
        public IBHAHydraulicsOutput BHAHydraulicsOutput
        {
            get { return (IBHAHydraulicsOutput)this; }
        }

          
        
        
        #region Private Variables
       
        private int pN;
        private string toolDesc;
        private double mOD;
        private double iD;
        private double len;
        protected double averageVelocity = double.MinValue;
        protected double criticalVelocity = double.MinValue;
        protected double equivalentCirculatingDensity = double.MinValue;
        protected double pressureDrop = double.MinValue;
        protected string flowType;
        protected double outputFlow = double.MinValue;
        #endregion

        #region Properties
        private Guid? _toolIdentifier;

        public Guid? ToolIdentifier
        {
            get { return _toolIdentifier; }
            set { _toolIdentifier = value; }
        }
       
        public int PositionNumber
        {
            get{return pN;}
            set{pN = value;}
        }

        public string toolDescription
        {
            get{return toolDesc;}
            set{toolDesc = value;}
        }

        public double OutsideDiameterInInch
        {
            get{return mOD;}
            set{mOD = value;}
        }

        
        public double LengthInFeet
        {
            get{return len;}
            set{len = value;}
        }

        //this property is by default set to false.
        // for tool types with split flow or different states. This need to be set to true
        public virtual bool HasVariableFlow
        {
            get { return false; }
        }

        //this property is by default return double.MinValue.
        // for tool types with split flow or different states. This need to be set to appropriate value
        double IBHAHydraulicsOutput.OutputFlowInGallonsPerMinute
        {
            get { return outputFlow; }
            set { outputFlow = value; }
           
        }
       
        double IBHAHydraulicsOutput.AverageVelocityInFeetPerSecond    
        {
            get { return averageVelocity; }
            set { averageVelocity = value; }
        }

        double IBHAHydraulicsOutput.PressureDropInPSI
        {
            get { return pressureDrop; }
            set { pressureDrop = value; }
        }

        string IBHAHydraulicsOutput.FlowType
        {
            get { return flowType; }
            set { flowType = value; }
        }

        double IBHAHydraulicsOutput.CriticalVelocityInFeetPerSecond
        {
            get { return criticalVelocity; }
            set { criticalVelocity = value; }
        }
        double IBHAHydraulicsOutput.EquivalentCirculatingDensity
        {
            get { return equivalentCirculatingDensity; }
            set { equivalentCirculatingDensity = value; }
        }


        #endregion
        public BHATool() { }
        public BHATool( int positionNumber, string toolDescription, double outsideDiameterInInch, double lengthInFeet)
        {
            pN = positionNumber;
            toolDesc = toolDescription;
            mOD =  outsideDiameterInInch;
            len = lengthInFeet;
        }

        public virtual void CalculateHydraulics(Fluid fluid, double flowRate = double.MinValue, double torqueInFeetPound = 0, List<BHATool> bhaTools = null, List<Segment> segments = null)
        {
            this.BHAHydraulicsOutput.AverageVelocityInFeetPerSecond = double.MinValue;
            this.BHAHydraulicsOutput.FlowType = Common.TurbulentFlowType;
            this.BHAHydraulicsOutput.PressureDropInPSI = double.MinValue;
        }

        public abstract BHATool GetDeepCopy();

        protected T DeepCopyHelper<T>(T inputObject)
        {
            T newCopy;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(ms, this);
                ms.Position = 0;
                newCopy = (T)xmlSerializer.Deserialize(ms);
            }
            return newCopy;
        }
       
    }
}