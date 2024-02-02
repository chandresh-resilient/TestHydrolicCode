using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydraulicEngine
{
   
    internal static class AccusetSystem
    {
        internal static Accuset GetAccusetDetails(string accusetSystemName, double nozzleSizeInInches, double mudDensityInPoundsPerGallons)
        {
            List<Accuset> allAccuset = GetAllAccusetSystems();
            Accuset returnValue;
            string fluid = GetFluidName(mudDensityInPoundsPerGallons);
            double standardNozzleSize = GetStandardNozzleSize(nozzleSizeInInches);

            returnValue = allAccuset.FirstOrDefault(x => x.AccusetSystemName == accusetSystemName && x.Fluid == fluid && x.StandardNozzleSize == standardNozzleSize);
            if (returnValue == null)
            {
                returnValue = allAccuset.FirstOrDefault(x => x.AccusetSystemName == accusetSystemName);
                returnValue.Fluid = "";
                returnValue.StandardNozzleSize = standardNozzleSize;
                returnValue.CorrectionFactor = 1;
            }
            return returnValue;
        }

        private static string GetFluidName(double mudDensityInPoundsPerGallons)
        {
            if (mudDensityInPoundsPerGallons >= 6.9 && mudDensityInPoundsPerGallons <= 8.6)
                return "Water";
            else if (mudDensityInPoundsPerGallons >= 8.7 && mudDensityInPoundsPerGallons <= 18)
                return "Mud";
            else
                return "";
        }


        private static double GetStandardNozzleSize(double nozzleSizeInInches)
        {
            if (nozzleSizeInInches >= 0.733 && nozzleSizeInInches <= 0.765)
                return 0.75;
            else if (nozzleSizeInInches >= 0.857 && nozzleSizeInInches <= 0.89)
                return 0.875;
            else if (nozzleSizeInInches >= 0.98 && nozzleSizeInInches <= 1.017)
                return 1;
            else if (nozzleSizeInInches >= 1.104 && nozzleSizeInInches <= 1.144)
                return 1.125;
            else if (nozzleSizeInInches >= 1.228 && nozzleSizeInInches <= 1.27)
                return 1.25;
            else if (nozzleSizeInInches >= 0.485 && nozzleSizeInInches <= 0.555)
                return 0.545;
            else if (nozzleSizeInInches >= 0.565 && nozzleSizeInInches <= 0.585)
                return 0.575;
            else
                return 0.605;
        }
        private static List<Accuset> GetAllAccusetSystems()
        {
            List<Accuset> returnValue = new List<Accuset>();
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Water", 0.75, 0.72));
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Water", 0.545, 1.26));
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Water", 0.605, 0.48));
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Mud", 0.75, 0.46));
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Mud", 0.545, 1.26));
            returnValue.Add(new Accuset("4-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.37256, "Mud", 0.605, 1.40));

            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Water", 0.75, 0.39));
            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Water", 0.545, 1.64));
            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Water", 0.605, 0.17));
            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Mud", 0.75, 0.2));
            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Mud", 0.545, 1.64));
            returnValue.Add(new Accuset("5-1/2\" MultiCatch (RHN)", 0.875, 0.98, 0.82, 0.57677, "Mud", 0.605, -0.22));

            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.75, -0.25));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.875, 0.063));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 1, 1));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.545, -4));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.575, -3));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.605, -2.26));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.75, -0.25));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.875, -0.125));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 1, 1));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.545, -4));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.575, -3));
            returnValue.Add(new Accuset("6-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.605, -2.26));

            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.75, -0.25));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.875, 0.063));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 1, 1));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.545, -4));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.575, -3));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.605, -2.26));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.75, -0.25));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.875, -0.125));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 1, 1));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.545, -4));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.575, -3));
            returnValue.Add(new Accuset("7\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.605, -2.26));

            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.75, -0.25));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.875, 0.063));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 1, 1));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.545, -4));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.575, -3));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Water", 0.605, -2.26));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.75, -0.25));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.875, -0.125));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 1, 1));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.545, -4));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.575, -3));
            returnValue.Add(new Accuset("7-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.605, -2.26));


            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Water", 0.75, 1));
            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Water", 0.875, 1));
            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Water", 1, 1));
            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.75, 1));
            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Mud", 0.875, 1));
            returnValue.Add(new Accuset("7\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 1.227, "Mud", 1, 1));

           
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.75, -1));
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.875, -3));
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.75, -3));
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.75, -5));
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.875, -4));
            returnValue.Add(new Accuset("9-5/8\" QuickPack Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.75, -4));

            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.75, 1.8));
            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.875, -3));
            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Water", 1, -3));
            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.75, -5));
            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.875, -4));
            returnValue.Add(new Accuset("9-5/8\" Starburst Accuset Packer (RHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 1, -4));

            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Water", 0.75, -6.9));
            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Water", 0.875, -4.7));
            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Water", 1, -2.8));
            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.75, -8));
            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.875, -5.8));
            returnValue.Add(new Accuset("9-5/8\" MultiCatch (RHN)", 2.2, 0.98, 0.82, 2.764, "Mud", 1, -3.2));

            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.75, -1));
            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Water", 0.875, -3));
            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Water", 1, -3));
            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.75, -5));
            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 0.875, -4));
            returnValue.Add(new Accuset("9-5/8\" Permanent Packer (PHS)", 2.2, 0.98, 0.82, 2.764, "Mud", 1, -4));

            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Water", 0.75, -1.9));
            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Water", 0.875, 1.3));
            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Water", 1, 1.3));
            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Mud", 0.75, -1.9));
            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Mud", 0.875, 1.3));
            returnValue.Add(new Accuset("10-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.142, "Mud", 1, 1.3));

            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Water", 0.75, -6.1));
            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Water", 0.875, -5.5));
            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Water", 1, -3.1));
            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Mud", 0.75, -6.1));
            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Mud", 0.875, -5.5));
            returnValue.Add(new Accuset("11-3/4\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 3.608, "Mud", 1, -3.1));

            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Water", 0.75, -13.7));
            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Water", 0.875, -11));
            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Water", 1, -5.1));
            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Mud", 0.75, -13.7));
            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Mud", 0.875, -11));
            returnValue.Add(new Accuset("13-3/8\" MultiCatch (RHN)", 2.95, 0.98, 0.82, 4.712, "Mud", 1, -5.1));
            
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Water", 0.75, -1));
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Water", 0.875, -3));
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Water", 1, -3));
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Mud", 0.75, -5));
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Mud", 0.875, -4));
            returnValue.Add(new Accuset("13-3/8\" Permanent Packer (PHS)", 2.95, 0.98, 0.82, 4.712, "Mud", 1, -4));

            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Water", 1, -36));
            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Water", 1.125, -20));
            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Water", 1.25, -13));
            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Mud", 1, -36));
            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Mud", 1.125, -20));
            returnValue.Add(new Accuset("16\"-20\" MultiCatch (RHN)", 4, 0.98, 0.82, 9.817, "Mud", 1.25, -13));

            return returnValue;
        }
    }
}
