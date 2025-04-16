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
    public static class DataLoader
    {

        public static void LoadPU()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                IgnoreBlankLines = true
            };

            using (var reader = new StreamReader("Assets/PUData.csv"))
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
                            ElectricityProductionPerMW = double.Parse(csv.GetField(5).Trim()),
                            ProductionCost = double.Parse(csv.GetField(6).Trim()),
                            IsON = true
                        }
                        ;
                        AM.ProductionUnits.Add(productionData);
                    }
                }
            }
        }
        public static void loadSDM()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                IgnoreBlankLines = true
            };

            using (var reader = new StreamReader("Assets/SDMData.csv"))
            using (var csv = new CsvReader(reader, config))

            {
                for (int i = 0; i < 3; i++) reader.ReadLine();

                while (csv.Read())
                {

                    if (!string.IsNullOrWhiteSpace(csv.GetField(0)))
                    {
                        var winterData = new HourlyData
                        {
                            TimeFrom = DateTime.Parse(csv.GetField(0).Trim()),
                            TimeTo = DateTime.Parse(csv.GetField(1).Trim()),
                            HeatDemand = double.Parse(csv.GetField(2).Trim()),
                            ElectricityPrice = double.Parse(csv.GetField(3).Trim())
                        };
                        SDM.WinterPeriod.Add(winterData.TimeFrom, winterData);
                    }

                    if (!string.IsNullOrWhiteSpace(csv.GetField(5)))
                    {
                        var summerData = new HourlyData
                        {
                            TimeFrom = DateTime.Parse(csv.GetField(5).Trim()),
                            TimeTo = DateTime.Parse(csv.GetField(6).Trim()),
                            HeatDemand = double.Parse(csv.GetField(7).Trim()),
                            ElectricityPrice = double.Parse(csv.GetField(8).Trim())
                        };
                        SDM.SummerPeriod.Add(summerData.TimeFrom, summerData);

                    }
                }
            }
        }

        public static void LoadData()
        {
            LoadPU();
            loadSDM();
        }



    }

}