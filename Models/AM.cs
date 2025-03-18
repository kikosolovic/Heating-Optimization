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
        public string GridName { get; set; }
        public string ImageUrl { get; set; }

        public List<PU> ProductionUnits { get; set; } = new List<PU>();
        public void LoadData(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                IgnoreBlankLines = true
            };
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))

            {

                for (int i = 0; i < 1; i++) reader.ReadLine();

                while (csv.Read())
                {

                    if (!string.IsNullOrWhiteSpace(csv.GetField(0)))
                    {
                        var productionData = new PU
                        {
                            Name = csv.GetField(0).Trim(),
                            MaxHeat = double.Parse(csv.GetField(1).Trim()),
                            Co2Emissions = double.Parse(csv.GetField(2).Trim()),
                            FuelConsumption = double.Parse(csv.GetField(4).Trim()),
                            TypeOfFuel = csv.GetField(3).Trim(),
                            ElectricityProduction = double.Parse(csv.GetField(5).Trim()),
                            ProductionCost = double.Parse(csv.GetField(6).Trim())
                        };
                        ProductionUnits.Add(productionData);
                    }
                }
            }

        }
        public void RemovePU(int id)
        {
            this.ProductionUnits.RemoveAt(id);
        }

        public AM()
        {
            LoadData("Assets/PUData.csv");

        }
    }
}
