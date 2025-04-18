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
using Avalonia.Metadata;


namespace Heating_Optimization.Models
{
    public static class DataLoader
    {

        public static void LoadPU()
        {
            int _idCount = 0;
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
                            Id = _idCount++,
                            Name = csv.GetField(0).Trim(),
                            MaxHeat = double.Parse(csv.GetField(1).Trim().Replace(",", "."), CultureInfo.InvariantCulture),
                            Co2Emissions = double.Parse(csv.GetField(2).Trim().Replace(",", "."), CultureInfo.InvariantCulture),
                            FuelConsumption = double.Parse(csv.GetField(4).Trim().Replace(",", "."), CultureInfo.InvariantCulture),
                            TypeOfFuel = csv.GetField(3).Trim(),
                            ElectricityProductionPerMW = double.Parse(csv.GetField(5).Trim().Replace(",", "."), CultureInfo.InvariantCulture) 
                            / double.Parse(csv.GetField(1).Trim().Replace(",", "."), CultureInfo.InvariantCulture),
                            ProductionCost = double.Parse(csv.GetField(6).Trim().Replace(",", "."), CultureInfo.InvariantCulture),
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
                            TimeFrom = DateTime.ParseExact(csv.GetField(0).Trim(), "M/d/yyyy H:mm", CultureInfo.InvariantCulture),
                            TimeTo = DateTime.ParseExact(csv.GetField(1).Trim(), "M/d/yyyy H:mm", CultureInfo.InvariantCulture),
                            HeatDemand = double.Parse(csv.GetField(2).Trim(), CultureInfo.InvariantCulture),
                            ElectricityPrice = double.Parse(csv.GetField(3).Trim(), CultureInfo.InvariantCulture)
                        };
                        SDM.WinterPeriod.Add(winterData.TimeFrom, winterData);
                    }

                    if (!string.IsNullOrWhiteSpace(csv.GetField(5)))
                    {
                        var summerData = new HourlyData
                        {
                            TimeFrom = DateTime.ParseExact(csv.GetField(5).Trim(), "M/d/yyyy H:mm", CultureInfo.InvariantCulture),
                            TimeTo = DateTime.ParseExact(csv.GetField(6).Trim(), "M/d/yyyy H:mm", CultureInfo.InvariantCulture),
                            HeatDemand = double.Parse(csv.GetField(7).Trim(), CultureInfo.InvariantCulture),
                            ElectricityPrice = double.Parse(csv.GetField(8).Trim(), CultureInfo.InvariantCulture)
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