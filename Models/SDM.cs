using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;



namespace Heating_Optimization.Models
{
    public class SDM
    {
        public Dictionary<DateTime, HourlyData> WinterPeriod { get; set; } = new Dictionary<DateTime, HourlyData>();
        public Dictionary<DateTime, HourlyData> SummerPeriod { get; set; } = new Dictionary<DateTime, HourlyData>();

        public void LoadData(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                IgnoreBlankLines = true
            };
            using (var reader = new StreamReader(path))
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
                        WinterPeriod.Add(winterData.TimeFrom, winterData);
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
                        SummerPeriod.Add(summerData.TimeFrom, summerData);

                    }
                }
            }

        }

        public SDM()
        {
            // Console.WriteLine(Directory.GetCurrentDirectory());
            // LoadData(Directory.GetCurrentDirectory() + "/Assets/Data.csv");

            var data = DataLoader.LoadData<SDM>(Directory.GetCurrentDirectory() + "/Assets/SDMData.csv", ",");
            SummerPeriod = data.Item1;
            WinterPeriod = data.Item2;
        }
    }
}