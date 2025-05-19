using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heating_Optimization.Models;
namespace Heating_Optimization.Models
{
    public static class RDM
    {
        public static List<ProductionData> productionData = new List<ProductionData>();
        public static void LoadCsv(string path)
        {
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).Skip(1);
            var data = new List<ProductionData>();

            foreach (var line in lines)
            {
                var parts = line.Split(';');
                data.Add(new ProductionData
                {
                    Date = DateTime.ParseExact(parts[0], "dd-MM-yyyy HH:mm", null),
                    Period = parts[1],
                    Id = int.Parse(parts[2]),
                    Name = parts[3],
                    TotalHeat = double.Parse(parts[4]),
                    PercentageUsed = double.Parse(parts[5]),
                    TotalCO2 = double.Parse(parts[6]),
                    TotalCost = double.Parse(parts[7])
                });
            }
            productionData = data;
        }


        public static void GenerateCsv(int MachineCount, OPT optimizer, HashSet<int> SelectedMachines)
        {
            string fileName = $"ProductionCost_Selected.csv";

            if (MachineCount == 0)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Console.WriteLine($"CSV file {fileName} was deleted because no machines were selected.");
                }
                else
                {
                    Console.WriteLine("No production units selected.");
                }
                return;
            }


            optimizer.GenerateCSVForSelectedMachines(SelectedMachines, fileName);

            Console.WriteLine($"CSV successfully generated: {fileName}");

            LoadCsv(fileName);
        }
    }
}