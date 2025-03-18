using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heating_Optimization.Models
{
    public class HourlyData
    {
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public double ElectricityPrice { get; set; }
        public double HeatDemand { get; set; }
    }
}