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
    public static class AM
    {
        private static int _idCount = 1;
        public static string GridName { get; set; }
        public static string ImageUrl { get; set; }

        public static List<PU> ProductionUnits { get; set; } = new List<PU>();

        public static void RemovePU(int id)
        {
            ProductionUnits.RemoveAt(id);
        }

        // public AM()
        // {
        //     // LoadData("Assets/PUData.csv");
        //     ProductionUnits = DataLoader.LoadData<AM>(Directory.GetCurrentDirectory() + "/Assets/PUData.csv", ";");

        // }
    }
}
