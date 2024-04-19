using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SkiaSharp;

using HydraulicCalAPI.Service;

namespace HydraulicCalAPI.ViewModel
{
    public class PieData
    {
        public string Label { get; set; }
        public float Value { get; set; }
        public string Color { get; set; }
    }
    public class PgPieChart
    {
        public byte[] GetPieChart(ChartAndGraphService objChartAndGraphService)
        {
            try
            {
                int increment = 0;
                Dictionary<string, string> pdfPieChart = new Dictionary<string, string>();
                List<PieData> pieDataPoints = new List<PieData>();
                foreach (var pieitem in objChartAndGraphService.PressureDistributionChartCollection)
                {
                    pieDataPoints.Add(new PieData
                    {
                        Label = pieitem.Name,
                        Value = (float)pieitem.Value,
                        Color = pieitem.Color
                    });
                }

                foreach (var itempiedata in objChartAndGraphService.PressureDistributionChartCollection)
                {
                    increment++;
                    pdfPieChart.Add("Name" + increment, itempiedata.Name != null ? itempiedata.Name.ToString() : "");
                    pdfPieChart.Add("Value" + increment, itempiedata.Value > 0 ? itempiedata.Value.ToString() : "");
                    pdfPieChart.Add("Color" + increment, itempiedata.Color != null ? itempiedata.Color.ToString() : "");
                }
                increment = 0;
                byte[] pieBytes = GeneratePieChart(pdfPieChart, pieDataPoints);
                return pieBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] GeneratePieChart(Dictionary<string, string> objPrsDrop, List<PieData> pieData)
        {
            try
            {
                float width = 200;
                float height = 200;
                float margin = 15;
                using (var surfcae = SKSurface.Create(new SKImageInfo(400, 350)))
                {
                    var canvas = surfcae.Canvas;
                    canvas.Clear(SKColors.White);
                    var center = new SKPoint(width / 2, height / 2);
                    var radius = Math.Min(width, height) / 2;
                    float labelPercentage = 0;
                    float totalValue = 0;

                    foreach (var itemval in objPrsDrop.Keys)
                    {
                        if (itemval.Substring(0, 5) == "Value")
                        {
                            string fltVale = !string.IsNullOrEmpty(objPrsDrop[itemval]) ? objPrsDrop[itemval].ToString() : "0";
                            float itmValue = (float)Math.Round(float.Parse(fltVale));
                            totalValue += itmValue;
                        }
                    }
                    var startAngle = 0.0f;
                    for (int i = 0; i < pieData.Count; i++)
                    {
                        string colourName = pieData[i].Color;
                        string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                        SKColor _color = SKColor.Parse(hexString);

                        labelPercentage = (float)Math.Round((pieData[i].Value / totalValue) * 100);
                        var sweepAngle = (float)(360 * pieData[i].Value / totalValue);
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = _color
                        })
                        {
                            var rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);
                            canvas.DrawArc(rect, startAngle, sweepAngle, true, paint);

                            //Draw Label
                            var labelAngle = startAngle + sweepAngle / 2;
                            var labelRadius = radius * 0.8f;
                            var labelX = center.X + labelRadius * (float)Math.Cos(labelAngle * Math.PI / 180);
                            var labelY = center.Y + labelRadius * (float)Math.Sin(labelAngle * Math.PI / 180);
                            using (var labelpaint = new SKPaint
                            {
                                Color = SKColors.Black,
                                TextSize = 10
                            })
                            {
                                if (labelPercentage > 0)
                                {
                                    canvas.DrawText($"{labelPercentage}%", labelX, labelY - 5, labelpaint);
                                }
                            }
                            // Update start angle for the next segment 
                            startAngle += sweepAngle;
                        }
                    }

                    foreach (var lstitem in objPrsDrop.Keys)
                    {
                        float legendX = 400 - margin - 170;
                        float legendY = margin;
                        float legendItemHeight = 8;
                        foreach (var data in pieData)
                        {
                            labelPercentage = (float)Math.Round((data.Value / totalValue) * 100);
                            using (var paint = new SKPaint())
                            {
                                string colourName = data.Color;
                                string hexString = ViewModel.ColorConverter.ColorNameToHexString(colourName);
                                paint.Color = SKColor.Parse(hexString);
                                canvas.DrawRect(legendX, legendY, legendItemHeight, legendItemHeight, paint);
                            }
                            using (var lblpaint = new SKPaint())
                            {
                                lblpaint.TextSize = 9;
                                lblpaint.Color = SKColors.Black;
                                lblpaint.TextAlign = SKTextAlign.Left;
                                canvas.DrawText($"{labelPercentage}%" + "=>" + $"{data.Label}", legendX + legendItemHeight + 5, legendY + 8, lblpaint);
                            }
                            legendY += legendItemHeight + 5;
                        }
                    }
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
    }
}