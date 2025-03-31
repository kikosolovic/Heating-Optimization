using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Heating_Optimization.Models
{
    public class RDM
    {
        private readonly string _outputFolder = "Assets/Results";

        public RDM()
        {
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }
        }

        public void SavePlan(string planName, Dictionary<DateTime, List<(string UnitName, double HeatProduced)>> plan)
        {
            var filePath = Path.Combine(_outputFolder, $"{planName}.csv");
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteField("DateTime");
            csv.WriteField("UnitName");
            csv.WriteField("HeatProduced");
            csv.NextRecord();

            foreach (var hour in plan)
            {
                foreach (var entry in hour.Value)
                {
                    csv.WriteField(hour.Key);
                    csv.WriteField(entry.UnitName);
                    csv.WriteField(entry.HeatProduced);
                    csv.NextRecord();
                }
            }
        }

        public Dictionary<DateTime, List<(string UnitName, double HeatProduced)>> LoadPlan(string planName)
        {
            var filePath = Path.Combine(_outputFolder, $"{planName}.csv");
            var result = new Dictionary<DateTime, List<(string, double)>>();

            if (!File.Exists(filePath)) return result;

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var time = csv.GetField<DateTime>("DateTime");
                var unit = csv.GetField<string>("UnitName");
                var heat = csv.GetField<double>("HeatProduced");

                if (!result.ContainsKey(time))
                    result[time] = new List<(string, double)>();

                result[time].Add((unit, heat));
            }

            return result;
        }

        public List<string> ListStoredPlans()
        {
            var list = new List<string>();

            if (!Directory.Exists(_outputFolder)) return list;

            var files = Directory.GetFiles(_outputFolder, "*.csv");
            foreach (var file in files)
            {
                list.Add(Path.GetFileNameWithoutExtension(file));
            }

            return list;
        }

        public void ExportPlan(string planName, string exportPath)
        {
            var originalPath = Path.Combine(_outputFolder, $"{planName}.csv");
            if (!File.Exists(originalPath))
                throw new FileNotFoundException($"Plan '{planName}' not found.");

            File.Copy(originalPath, exportPath, overwrite: true);
        }
    }
}