using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SkiaSharp;
using iText.IO.Image;
using iText.Layout.Element;
using HydraulicCalAPI.Service;

namespace HydraulicCalAPI.ViewModel
{
    public class DataSeries
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string LineClr { get; set; }
    }
    public class PgHydraulicToolsLineChart
    {

        public List<Image> GetHydraulicToolsLineChart(ChartAndGraphService objChartService, PdfReportService objInputData)
        {
            Dictionary<string, Array> dicBhaChart = new Dictionary<string, Array>();
            List<Image> lstBHAToolsLineChart = new List<Image>();
            string _lineColor = "Blue";
            try
            {
                for (int i = 0; i < objChartService.HydraulicOutputBHAList.Count; i++)
                {
                    List<List<DataSeries>> list = new List<List<DataSeries>>();
                    Dictionary<string, List<WFT.UI.Common.Charts.XYValueModelForLineData<double>>> bHAchart = objChartService.HydraulicOutputBHAList[i].BHAchart;
                    foreach (var bHA in bHAchart.Keys)
                    {
                        switch (bHA.ToUpper())
                        {
                            case "HYDRAPROREGIONTWOLINESERIES":
                                {
                                    _lineColor = "Red";
                                    break;
                                }
                            case "HYDRAPROREGIONTHREELINESERIES":
                                {
                                    _lineColor = "Yellow";
                                    break;
                                }
                            default:
                                _lineColor = "Blue";
                                break;
                        }
                        List<DataSeries> dataSeries = new List<DataSeries>();
                        foreach (var item in bHAchart[bHA].ToArray())
                        {
                            dataSeries.Add(new DataSeries
                            {
                                X = (float)Math.Round(item.PrimaryAxisValue, 3),
                                Y = (float)Math.Round(item.SecondaryAxisValue, 3),
                                LineClr = _lineColor
                            });
                        }
                        list.Add(dataSeries);
                    }
                    byte[] hydraprograph = DrawHydraulicToolsGraph(list, objChartService.HydraulicOutputBHAList[i], objInputData);
                    lstBHAToolsLineChart.Add(new Image(ImageDataFactory.Create(hydraprograph)));
                }
                return lstBHAToolsLineChart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private (float MaxX, float MaxY) FindMaxCoordinates(List<List<DataSeries>> hyprobhadataPoints)
        {
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            // Iterate through each sublist
            foreach (var dataList in hyprobhadataPoints)
            {
                // Iterate through each DataSeries in the sublist
                foreach (var data in dataList)
                {
                    // Update maxX if current X is larger
                    if (data.X > maxX)
                        maxX = data.X;

                    // Update maxY if current Y is larger
                    if (data.Y > maxY)
                        maxY = data.Y;
                }
            }

            return (maxX, maxY);
        }
        private (float MinX, float MinY) FindMinCoordinates(List<List<DataSeries>> hyprobhadataPoints)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;

            // Iterate through each sublist
            foreach (var dataList in hyprobhadataPoints)
            {
                // Iterate through each DataSeries in the sublist
                foreach (var data in dataList)
                {
                    // Update minX if current X is smaller
                    if (data.X < minX)
                        minX = data.X;

                    // Update minY if current Y is smaller
                    if (data.Y < minY)
                        minY = data.Y;
                }
            }

            return (minX, minY);
        }
        private byte[] DrawHydraulicToolsGraph(List<List<DataSeries>> hyprobhadataPointss, ViewModel.HydraulicOutputBHAViewModel service, PdfReportService objUOM)
        {
            try
            {
                // Add labels and values to PDF document
                string gXValue = objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN" ? objUOM.UOM.FlowRateName.ToString() : "gal/min";
                string gYValue = objUOM.UOM.PressureName.ToUpper() != "PSI" ? objUOM.UOM.PressureName.ToString() : "psi";

                using (var surfcae = SKSurface.Create(new SKImageInfo(250, 200)))
                {
                    var canvas = surfcae.Canvas;
                    canvas.Clear(SKColors.White);
                    float width = 250;
                    float height = 200;
                    float margin = 30;

                    // Define the scaling factors
                    var max = FindMaxCoordinates(hyprobhadataPointss);
                    var min = FindMinCoordinates(hyprobhadataPointss);
                    float minX = min.MinX;
                    float maxX = max.MaxX;
                    float minY = min.MinY;
                    float maxY = max.MaxY;
                    // Calculate the scale for X and Y axis
                    float scaleX = (width - 100) / (maxX - minX);
                    float scaleY = (height - 100) / (maxY - minY);

                    float opPointX = (float)Math.Round(service.InputFlowRate, 3);
                    float opPointY = (float)Math.Round(service.BHAPressureDrop, 3);

                    float anx1 = margin + opPointX * scaleX;
                    float any1 = height - margin - opPointY * scaleY;

                    using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1, TextSize = 10, TextAlign = SKTextAlign.Right })
                    {
                        // Draw X and Y axis
                        canvas.DrawLine(margin, margin, margin, height - margin, paint);
                        canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint);
                        //canvas.DrawText("X", anx1, any1, paint);

                        // Draw Scale Mark and Scale to X-axis
                        var loopX = Math.Ceiling(maxX);
                        int extraX = GetExtraGap(loopX);
                        for (int i = 0; i <= (loopX + extraX); i += extraX)
                        {
                            float xpoint = 30 + i * scaleX;
                            float cordsY = height - 30;
                            canvas.DrawLine(xpoint, cordsY + 5, xpoint, cordsY - 5, paint);
                            canvas.DrawText(i.ToString(), xpoint + 3, cordsY + 13, paint);
                        }
                        // Draw Scale Mark and Scale to Y-axis
                        var loopY = Math.Ceiling(maxY);
                        int extraY = GetExtraGap(loopY);
                        for (int i = 0; i <= (loopY + extraY); i += extraY)
                        {
                            float cordsX = 30;
                            float ypoint = height - 30 - i * scaleY;
                            if (i > 0)
                            {
                                canvas.DrawLine(cordsX, ypoint, width - margin, ypoint, new SKPaint { Color = SKColors.LightGray });
                                canvas.DrawText(i.ToString(), cordsX - 2, ypoint + 5, paint);
                            }
                        }
                    }


                    foreach (var hyprobhadataPoints in hyprobhadataPointss)
                    {
                        using (var paint = new SKPaint {StrokeWidth = 1, IsAntialias = true })
                        {
                            string colourName = hyprobhadataPoints[0].LineClr;
                            string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                            paint.Color = SKColor.Parse(hexString);
                            for (int i = 0; i < hyprobhadataPoints.Count - 1; i++)
                            {
                                float x1 = margin + hyprobhadataPoints[i].X * scaleX;
                                float y1 = height - margin - hyprobhadataPoints[i].Y * scaleY;
                                float x2 = margin + hyprobhadataPoints[i + 1].X * scaleX;
                                float y2 = height - margin - hyprobhadataPoints[i + 1].Y * scaleY;
                                canvas.DrawLine(x1, y1, x2, y2, paint);
                            }
                        }
                    }

                    // Add X-axis label
                    using (var xLabelPaint = new SKPaint())
                    {
                        xLabelPaint.Color = SKColors.Black;
                        xLabelPaint.TextAlign = SKTextAlign.Center;
                        xLabelPaint.TextSize = 9;
                        canvas.DrawText("Flow Rate (" + gXValue + ")", width / 2, margin / 2 + 180, xLabelPaint);
                    }

                    // Add Y-axis label
                    using (var yLabelPaint = new SKPaint())
                    {
                        yLabelPaint.Color = SKColors.Black;
                        yLabelPaint.TextAlign = SKTextAlign.Center;
                        yLabelPaint.TextSize = 9;
                        yLabelPaint.IsAntialias = true;
                        canvas.RotateDegrees(-90);
                        canvas.DrawText("Standpipe Pressure (" + gYValue + ")", -height / 2 - 5, (margin / 2) - 8, yLabelPaint);
                        canvas.RotateDegrees(90);
                    }

                    // Convert bitmap to byte array
                    using (var image = surfcae.Snapshot())
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            data.SaveTo(stream);
                            return stream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetExtraGap(double loopValue)
        {
            int gap = 0;
            if (loopValue > 1500)
            {
                gap = 1000;
            }
            else if (loopValue > 1000 && loopValue <= 1500)
            {
                gap = 500;
            }
            else if (loopValue > 500 && loopValue <= 1000)
            {
                gap = 100;
            }
            else if (loopValue > 100 && loopValue <= 500)
            {
                gap = 50;
            }
            else
            {
                gap = 10;
            }
            return gap;
        }
    }
}