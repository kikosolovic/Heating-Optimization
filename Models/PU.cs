using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heating_Optimization.Models
{
    public class PU
    {
        public string Name { get; set; }
        public double MaxHeat { get; set; }
        public double Co2Emissions { get; set; }
        public double FuelConsumption { get; set; }
        public string TypeOfFuel { get; set; }
        public double ElectricityProduction { get; set; }
        public double ProductionCost { get; set; }
    }
}
