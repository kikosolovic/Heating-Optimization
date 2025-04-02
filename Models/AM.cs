using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Xml.Linq;

namespace Heating_Optimization.Models
{
    public class AM
    {
        private int _idCount = 1;
        public string GridName { get; set; }
        public string ImageUrl { get; set; }

        public List<PU> ProductionUnits { get; set; } = new List<PU>();

        public void RemovePU(int id)
        {
            this.ProductionUnits.RemoveAt(id);
        }

        public AM()
        {
            // LoadData("Assets/PUData.csv");
            ProductionUnits = DataLoader.LoadData<AM>(Directory.GetCurrentDirectory() + "/Assets/PUData.csv", ";");

        }
    }
}
