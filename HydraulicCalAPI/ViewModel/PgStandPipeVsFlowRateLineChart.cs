using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SkiaSharp;

using HydraulicCalAPI.Service;

namespace HydraulicCalAPI.ViewModel
{
    public class DataPoints
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string LineClr { get; set; }
    }
    public class PgStandPipeVsFlowRateLineChart
    {
        List<DataPoints> dataPoints = new List<DataPoints>();
        public byte[] GetLineChart(ChartAndGraphService objChartService, PdfReportService objInputData)
        {
            try
            {
                if (objChartService.standpipePressureListRL.Count > 0)
                {
                    foreach (var item in objChartService.standpipePressureListRL)
                    {
                        dataPoints.Add(new DataPoints
                        {
                            X = (float)item.PrimaryAxisValue,
                            Y = (float)item.SecondaryAxisValue,
                            LineClr = "Red"
                        });
                    }
                }
                if (objChartService.standpipePressureListYL.Count > 0)
                {
                    foreach (var item in objChartService.standpipePressureListYL)
                    {
                        dataPoints.Add(new DataPoints
                        {
                            X = (float)item.PrimaryAxisValue,
                            Y = (float)item.SecondaryAxisValue,
                            LineClr = "Yellow"
                        });
                    }
                }
                if (objChartService.standpipePressureListG.Count > 0)
                {
                    foreach (var item in objChartService.standpipePressureListG)
                    {
                        dataPoints.Add(new DataPoints
                        {
                            X = (float)item.PrimaryAxisValue,
                            Y = (float)item.SecondaryAxisValue,
                            LineClr = "Green"
                        });
                    }
                }
                if (objChartService.standpipePressureListYH.Count > 0)
                {
                    foreach (var item in objChartService.standpipePressureListYH)
                    {
                        dataPoints.Add(new DataPoints
                        {
                            X = (float)item.PrimaryAxisValue,
                            Y = (float)item.SecondaryAxisValue,
                            LineClr = "Yellow"
                        });
                    }
                }
                if (objChartService.standpipePressureListRH.Count > 0)
                {
                    foreach (var item in objChartService.standpipePressureListRH)
                    {
                        dataPoints.Add(new DataPoints
                        {
                            X = (float)item.PrimaryAxisValue,
                            Y = (float)item.SecondaryAxisValue,
                            LineClr = "Red"
                        });
                    }
                }

                byte[] linechart = DrawLineGraph(objChartService,dataPoints,objInputData);
                return linechart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] DrawLineGraph(ChartAndGraphService objCags, List<DataPoints> dataPoints, PdfReportService objUOM)
        {
            try
            {
                // Add labels and values to PDF document
                string gXValue = "gal/min";
                string gYValue = "psi";
                if (objUOM.UOM.FlowRateName.ToUpper() != "GAL/MIN")
                {
                    gXValue = objUOM.UOM.FlowRateName.ToString();
                }
                else if (objUOM.UOM.PressureName.ToUpper() != "PSI")
                {
                    gYValue = objUOM.UOM.PressureName.ToString();
                }
                else { }
                // Sample data (replace with your actual data)
                List<float> flowrate = new List<float>();
                List<float> pressure = new List<float>();

                for (int i = 0; i < dataPoints.Count; i++)
                {
                    flowrate.Add(dataPoints[i].X);
                    pressure.Add(dataPoints[i].Y);
                }

                using (var surfcae = SKSurface.Create(new SKImageInfo(500, 400)))
                {
                    var canvas = surfcae.Canvas;
                    canvas.Clear(SKColors.White);
                    float width = 500;
                    float height = 400;
                    float margin = 40;

                    // Define the scaling factors
                    float minX = dataPoints.Min(p => p.X);
                    float maxX = dataPoints.Max(p => p.X);
                    float minY = dataPoints.Min(p => p.Y);
                    float maxY = dataPoints.Max(p => p.Y);

                    // Calculate the scale for X and Y axis
                    float scaleX = (width - 150) / (maxX - minX);
                    float scaleY = (height - 150) / (maxY - minY);

                    float opPointX = (float)objCags.HydraulicOutputBHAList[0].InputFlowRate;
                    float opPointY = (float)objCags.TotalPressureDrop;

                    float anx1 = margin + opPointX * scaleX;
                    float any1 = height - margin - opPointY * scaleY;

                    using (var paint = new SKPaint { Color = SKColors.Black, StrokeWidth = 1, TextSize = 10 })
                    {
                        // Draw X and Y axis
                        canvas.DrawLine(margin, margin, margin, height - margin, paint);
                        canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint);
                        //canvas.DrawText("X", anx1 - 50, any1 + 15, paint);

                        // Draw Scale Mark and Scale to X-axis
                        var loopX = Math.Ceiling(maxX);
                        int extraX = GetExtraGap(loopX);
                        for (int i = 0; i <= (loopX + extraX); i += extraX)
                        {
                            float xpoint = 40 + i * scaleX;
                            float cordsY = height - 40;
                            canvas.DrawLine(xpoint, cordsY + 5, xpoint, cordsY -5, paint);
                            canvas.DrawText(i.ToString(), xpoint-7, cordsY + 17, paint);
                        }
                        // Draw Scale Mark and Scale to Y-axis
                        var loopY = Math.Ceiling(maxY);
                        int extraY = GetExtraGap(loopY); 
                        for (int i = 0; i <= (loopY + extraY); i += extraY)
                        {
                            float cordsX = 40;
                            float ypoint = height - 40 - i * scaleY;
                            if(i > 0)
                            {
                                canvas.DrawLine(cordsX, ypoint, cordsX + width, ypoint, new SKPaint { Color = SKColors.LightGray });
                                canvas.DrawText(i.ToString(), cordsX - 25, ypoint + 5, paint);
                            }
                        }
                    }

                    using (var paint = new SKPaint { Color = SKColors.Red, StrokeWidth = 1, IsAntialias = true })
                    {
                        // Draw data points and lines
                        for (int i = 0; i < dataPoints.Count - 1; i++)
                        {
                            float x1 = margin + dataPoints[i].X * scaleX;
                            float y1 = height - margin - dataPoints[i].Y * scaleY;
                            float x2 = margin + dataPoints[i + 1].X * scaleX;
                            float y2 = height - margin - dataPoints[i + 1].Y * scaleY;

                            if (dataPoints[i].LineClr == "Yellow")
                            {
                                string colourName = dataPoints[i].LineClr;
                                string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                                paint.Color = SKColor.Parse(hexString);
                                canvas.DrawLine(x1, y1, x2, y2, paint);
                            }
                            else if (dataPoints[i].LineClr == "Green")
                            {
                                string colourName = dataPoints[i].LineClr;
                                string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                                paint.Color = SKColor.Parse(hexString);
                                canvas.DrawLine(x1, y1, x2, y2, paint);
                            }
                            else
                            {
                                canvas.DrawLine(x1, y1, x2, y2, paint);
                            }
                        }
                    }

                    // Add X-axis label
                    using (var xLabelPaint = new SKPaint())
                    {
                        xLabelPaint.Color = SKColors.Black;
                        xLabelPaint.TextAlign = SKTextAlign.Center;
                        xLabelPaint.TextSize = 12;
                        canvas.DrawText("Flow Rate (" + gXValue + ")", width / 2, margin / 2 + 370, xLabelPaint);
                    }

                    // Add Y-axis label
                    using (var yLabelPaint = new SKPaint())
                    {
                        yLabelPaint.Color = SKColors.Black;
                        yLabelPaint.TextAlign = SKTextAlign.Center;
                        yLabelPaint.TextSize = 12;
                        yLabelPaint.IsAntialias = true;
                        canvas.RotateDegrees(-90);
                        canvas.DrawText("Standpipe Pressure (" + gYValue + ")", -height / 2 - 5, (margin / 2) - 10, yLabelPaint);
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
            else if (loopValue > 100 && loopValue <= 1500)
            {
                gap = 100;
            }
            else
            {
                gap = 10;
            }
            return gap;
        }
    }
}
