using System;
using System.Text;

namespace ComputerTracker
{
    class Computer
    {
        public readonly string Id;
        public bool? HasAntenna { get; set; }
        private double? storageCap;
        private double ram;
        public readonly int?[] licenses = new int?[5];

        public double? StorageCap
        {
            get
            {
                return storageCap;
            }
            set
            {
                if (value >= 0)
                {
                    this.storageCap = value;
                }
            }
        }

        public int GetNumLicenses()
        {
            int totalLicenses = 0;
            if (licenses != null)
            {
                foreach (int? numLicenses in licenses)
                {
                    totalLicenses += numLicenses ?? 0;
                }
            }
            else
            {
                totalLicenses += 0;
            }
            return totalLicenses;
        }

        public double Ram
        {
            get
            {
                int softwareDeduction = 10 * GetNumLicenses();
                int antennaDeduction = !(HasAntenna ?? true) ? 50 : 100;
                return ram - (softwareDeduction + antennaDeduction);
            }
            set
            {
                if (value >= 1000)
                {
                    this.ram = value;
                }
            }
        }

        public Computer(string id, double ram, bool? hasAntenna, double? storageCap, int?[] licenses)
        {
            this.Id = id;
            this.Ram = ram;
            this.HasAntenna = hasAntenna;
            this.StorageCap = storageCap;
            this.licenses = licenses;
        }


        public override string ToString()
        {
            StringBuilder compInfo = new StringBuilder();
            compInfo.Append($"Computer ID: {Id}\n");
            compInfo.Append($"RAM: {Ram}\n");
            compInfo.Append($"Has Antenna: {HasAntenna.ToString() ?? "N/A"}\n");
            compInfo.Append($"Storage Capacity: {StorageCap.ToString() ?? "N/A"}\n");
            compInfo.Append("Licenses: ");
            if (licenses == null)
            {
                compInfo.Append("N/A\n");
            }
            else
            {
                foreach (int? license in licenses)
                {
                    compInfo.Append($"{license}  ");
                }
            }
            
            return compInfo.ToString() + "\n";
        }
    }
}

