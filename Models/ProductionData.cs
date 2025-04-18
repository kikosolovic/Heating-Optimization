using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Heating_Optimization.Models
{
    public class ProductionData
    {
        public DateTime Date { get; set; }
        public string Period { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public double TotalHeat { get; set; }
        public double PercentageUsed { get; set; }
        public double TotalCO2 { get; set; }
        public double TotalCost { get; set; }
        public string Hour => Date.ToString("HH:mm");
        public string Day => Date.ToString("dd-MM-yyyy");
    
    }
}
