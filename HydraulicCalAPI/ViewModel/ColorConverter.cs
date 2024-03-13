using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace HydraulicCalAPI.ViewModel
{
    public class ColorConverter
    {
        public static string ColorNameToHexString(string colorName)
        {
            Color color = Color.FromName(colorName);
            string hexString = ColorToHexString(color);

            return hexString;
        }

        private static string ColorToHexString(Color objcolor)
        {
            return "#" + objcolor.R.ToString("X2") + objcolor.G.ToString("X2") + objcolor.B.ToString("X2");
        }
    }
}
