using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{

    public class HydraulicAnalysisOutput
    {
        private List<Segment> seg;
        private List<BHATool> bha;
        private SurfaceEquipment se;
        private double toolDepth;
        private double blockPosition;

        public List<Segment> Segment
        {
            get { return seg; }
            set { seg = value; }
        }

        public List<BHATool> BHATool
        {
            get { return bha; }
            set { bha = value; }
        }

        public SurfaceEquipment surfaceEquipment
        {
            get { return se; }
            set { se = value; }
        }
        public double ToolDepthInFeet
        {
            get { return toolDepth; }
            set { toolDepth = value; }
        }

        public double BlockPositionInFeet
        {
            get { return blockPosition; }
            set { blockPosition = value; }
        }
    }

    public static class Main
    {

        public static HydraulicAnalysisOutput CompleteHydraulicAnalysis(Fluid fluid, double flowRateInGPM, Cuttings cuttings, List<BHATool> BHATopBottomInput, List<Annulus> annulusTopBottom, SurfaceEquipment surfaceEquipment, double torqueInFeetPound = 0, double toolDepthInFeet = double.MinValue, double blockPostionInFeet = double.MinValue)
        {
            double toolDepth;
            double totalBHALength;
            List<BHATool> BHATopBottom = new List<BHATool>();
            if (BHATopBottomInput != null)
            {
                foreach (var item in BHATopBottomInput)
                {
                    BHATopBottom.Add(item.GetDeepCopy());
                }
            }
            List<Segment> segments = SegmentHydraulicCalculations(fluid, flowRateInGPM, cuttings, BHATopBottom, annulusTopBottom, toolDepthInFeet);
            List<BHATool> bhaTools = BHAHydraulicCalculations(fluid, flowRateInGPM, BHATopBottom, out totalBHALength, torqueInFeetPound, segments, toolDepthInFeet);
            surfaceEquipment.CalculateHydraulics(fluid, flowRateInGPM);
            if (segments != null && segments.Count > 0)
                toolDepth = segments[segments.Count - 1].SegmentBottomInFeet;
            else
                toolDepth = 0;
            HydraulicAnalysisOutput output = new HydraulicAnalysisOutput();
            output.Segment = segments;
            output.BHATool = bhaTools;
            output.BlockPositionInFeet = totalBHALength - toolDepth;
            output.ToolDepthInFeet = toolDepth;
            output.surfaceEquipment = surfaceEquipment;
            return output;
        }

        public static List<BHATool> BHAHydraulicAnalysis(Fluid fluid, double flowRateInGPM, List<BHATool> BHATopBottom, double torqueInFeetPound = 0, double toolDepthInFeet = double.MinValue)
        {
            double bhalength;
            return BHAHydraulicCalculations(fluid, flowRateInGPM, BHATopBottom, out bhalength, torqueInFeetPound, null, toolDepthInFeet);
        }

        private static List<Segment> GetSegments(List<BHATool> BHATopBottom, List<Annulus> annulusTopBottom, double toolDepth = double.MinValue)
        {
            List<Annulus> sortedAnnulus;

            List<Segment> segments = new List<Segment>();
            Annulus firstAnnulus;
            int annulusCounter;
            int bHACounter;
            int position = 1;
            double segmentBottom;
            double bhaLengthUsed = 0;

            if (annulusTopBottom == null || annulusTopBottom.Count == 0 || BHATopBottom == null && BHATopBottom.Count == 0)
                return null;
            sortedAnnulus = GetSortedAnnulus(annulusTopBottom);

            if (toolDepth == double.MinValue)
                toolDepth = sortedAnnulus[sortedAnnulus.Count - 1].AnnulusBottomInFeet;

            firstAnnulus = sortedAnnulus.Where(o => o.AnnulusTopInFeet < toolDepth && o.AnnulusBottomInFeet >= toolDepth).FirstOrDefault();
            annulusCounter = sortedAnnulus.IndexOf(firstAnnulus);


            bHACounter = BHATopBottom.Count - 1;


            segmentBottom = toolDepth;

            while ((annulusCounter >= 0) && (bHACounter >= 0))
            {
                Annulus currAnnulus = new Annulus();
                BHATool currBHA;
                Segment segment = new Segment();

                currAnnulus = sortedAnnulus[annulusCounter];
                currBHA = BHATopBottom[bHACounter];

                segment.PositionNumber = position;
                segment.WellboreSectionName = currAnnulus.WellboreSectionName;
                segment.ToolDescription = currBHA.toolDescription;
                segment.ToolPositionNumber = currBHA.PositionNumber;
                segment.AnnulusIDInInch = currAnnulus.AnnulusIDInInch;
                segment.ToolODInInch = currBHA.OutsideDiameterInInch;
                segment.SegmentBottomInFeet = segmentBottom;
                segment.Depth = toolDepth;

                if ((currBHA.LengthInFeet - bhaLengthUsed) <= (segmentBottom - currAnnulus.AnnulusTopInFeet))
                {
                    segment.SegmentTopInFeet = segmentBottom - (currBHA.LengthInFeet - bhaLengthUsed);
                    bHACounter -= 1;
                    segmentBottom = segmentBottom - (currBHA.LengthInFeet - bhaLengthUsed);
                    bhaLengthUsed = 0;
                }
                else
                {
                    segment.SegmentTopInFeet = currAnnulus.AnnulusTopInFeet;
                    annulusCounter -= 1;
                    segmentBottom = currAnnulus.AnnulusTopInFeet;
                    bhaLengthUsed = segment.SegmentLengthInFeet;
                }
                if (segment.SegmentBottomInFeet - segment.SegmentTopInFeet > 0)
                    segments.Add(segment);
                position += 1;
            }
            int segmentCount = segments.Count;
            for (int counter = 0; counter < segments.Count; counter++)
            {
                segments[counter].PositionNumber = segmentCount - counter;
            }
            return segments.OrderBy(o => o.PositionNumber).ToList();
        }



        private static List<Annulus> GetSortedAnnulus(List<Annulus> annulusTopBottom)
        {
            List<Annulus> returnAnnulus = new List<Annulus>();
            List<Annulus> sortedAnnulus;

            sortedAnnulus = annulusTopBottom.OrderBy(o => o.AnnulusIDInInch).ToList();
            foreach (Annulus currAnnulus in sortedAnnulus)
            {
                if (returnAnnulus.Count == 0)
                    returnAnnulus.Add(currAnnulus);
                else
                {
                    Annulus newAnnulus = new Annulus();
                    newAnnulus.WellboreSectionName = currAnnulus.WellboreSectionName;
                    newAnnulus.AnnulusIDInInch = currAnnulus.AnnulusIDInInch;
                    newAnnulus.AnnulusODInInch = currAnnulus.AnnulusODInInch;
                    newAnnulus.AnnulusTopInFeet = currAnnulus.AnnulusTopInFeet;
                    newAnnulus.AnnulusBottomInFeet = currAnnulus.AnnulusBottomInFeet;

                    foreach (Annulus adddedAnnulus in returnAnnulus)
                    {
                        if ((adddedAnnulus.AnnulusTopInFeet <= newAnnulus.AnnulusTopInFeet) && (adddedAnnulus.AnnulusBottomInFeet > newAnnulus.AnnulusTopInFeet))
                            newAnnulus.AnnulusTopInFeet = adddedAnnulus.AnnulusBottomInFeet;
                        if ((adddedAnnulus.AnnulusTopInFeet < newAnnulus.AnnulusBottomInFeet) && (adddedAnnulus.AnnulusBottomInFeet >= newAnnulus.AnnulusBottomInFeet))
                            newAnnulus.AnnulusBottomInFeet = adddedAnnulus.AnnulusTopInFeet;
                    }
                    if (newAnnulus.AnnulusBottomInFeet - newAnnulus.AnnulusTopInFeet > 0)
                        returnAnnulus.Add(newAnnulus);
                }
            }
            return returnAnnulus.OrderBy(o => o.AnnulusTopInFeet).ToList();
        }

        private static List<BHATool> BHAHydraulicCalculations(Fluid fluid, double flowRateInGPM, List<BHATool> BHATopBottom, out double totalBHALength, double torqueInFeetPound = 0, List<Segment> segments = null, double toolDepthInFeet = double.MinValue)
        {
            totalBHALength = 0;
            List<BHATool> outputBHATopBottom ;
            if (toolDepthInFeet != double.MinValue)
            {
                List<BHATool> outputBHA = new List<BHATool>();
                for (int counter = BHATopBottom.Count - 1; counter >= 0; counter--)
                {
                    totalBHALength += BHATopBottom[counter].LengthInFeet;
                    if (totalBHALength > toolDepthInFeet)
                    {
                        totalBHALength = totalBHALength - BHATopBottom[counter].LengthInFeet ;
                        //totalBHALength = (toolDepthInFeet - totalBHALength);
                        outputBHA.Add(BHATopBottom[counter]);
                        outputBHA[outputBHA.Count - 1].LengthInFeet = (toolDepthInFeet - totalBHALength);
                        totalBHALength += outputBHA[outputBHA.Count - 1].LengthInFeet;
                        break;
                    }
                    else if (totalBHALength == toolDepthInFeet)
                    {
                        outputBHA.Add(BHATopBottom[counter]);
                        break;
                    }
                    else
                        outputBHA.Add(BHATopBottom[counter]);
                }
                outputBHATopBottom = outputBHA.OrderBy(o => o.PositionNumber).ToList();
            }
            else
                outputBHATopBottom = BHATopBottom;


            double flowRate = flowRateInGPM;
            
            if (BHATopBottom != null)
                foreach (BHATool bhatool in outputBHATopBottom)
                {
                    bhatool.CalculateHydraulics(fluid, flowRate, torqueInFeetPound, BHATopBottom, segments);
                    if (bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute != double.MinValue)
                        flowRate = bhatool.BHAHydraulicsOutput.OutputFlowInGallonsPerMinute;

                    totalBHALength += bhatool.LengthInFeet;
                }
            return outputBHATopBottom;
        }
        private static List<Segment> SegmentHydraulicCalculations(Fluid fluid, double flowRateInGPM, Cuttings cuttings, List<BHATool> BHATopBottom, List<Annulus> annulusTopBottom, double toolDepthInFeet = double.MinValue)
        {
            List<Segment> segments = GetSegments(BHATopBottom, annulusTopBottom, toolDepthInFeet);
            if (segments != null)
            {
                foreach (Segment seg in segments)
                {
                    seg.CalculateHydraulics(fluid, flowRateInGPM, cuttings, toolDepthInFeet);
                }
            }
            return segments;
        }
    }
}
