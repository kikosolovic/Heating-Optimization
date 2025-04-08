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
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Metadata;

namespace Heating_Optimization.Models
{
    public class OPT
    {
        private double ActualHeat = 0;


        private HashSet<int> GetSelectedPUIds(int caseNumber)
        {
            return caseNumber switch
            {
                1 => new HashSet<int> { 1, 2, 3 },       // Gas Boiler 1, Gas Boiler 2, Oil Boiler
                2 => new HashSet<int> { 1, 3, 4, 5 },   // Gas Boiler 1, Oil Boiler, Gas Motor, Heat Pump
                3 => GetUserInputHashSet(),
                _ => new HashSet<int>() // Return an empty HashSet instead of null
            };
        }

        // Function to calculate production costs and automatically manage unit states
        public void SortByProductionCost(DateTime targetTime, int caseNumber)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }


            HashSet<int> selectedPUIds = GetSelectedPUIds(caseNumber);
            List<(int Id, string Name, double Result, double Co2, double MaxHeat)> puResults = new();
            List<(int Id, string Name, double TotalHeat, double PercentageUsed, double TotalCo2, double TotalCost)> puResults2 = new();

            foreach (var pu in AM.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
            {
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                double co2 = pu.Co2Emissions;
                puResults.Add((pu.Id, pu.Name, result, co2, pu.MaxHeat));
            }

            var sorted = puResults.OrderBy(r => r.Result).ToList();
            double TotalPrice = 0;

            Console.WriteLine($"\n=== Sorted by Production Cost (Case {caseNumber}) ===");
            foreach (var item in sorted)
            {
                Console.WriteLine($"> {item.Name} - Cost Result: {item.Result:F2}, CO2: {item.Co2}");
            }
            ActualHeat = hourlyData.HeatDemand;
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].MaxHeat <= ActualHeat & i != sorted.Count)
                {
                    double usedHeat = Math.Min(ActualHeat, sorted[i].MaxHeat);
                    ActualHeat -= usedHeat;
                    double percentageUsed = (usedHeat / sorted[i].MaxHeat) * 100;
                    double totalCost = usedHeat * sorted[i].Result;
                    double totalCo2 = sorted[i].Co2 * (percentageUsed / 100);

                    pu.IsOn = true;
                    puResults2.Add((sorted[i].Id, sorted[i].Name, usedHeat, percentageUsed, totalCo2, totalCost));
                }
                else
                {
                    double TotalHeat = ActualHeat;
                    double TotalCost = TotalHeat * sorted[i].Result;
                    double PercentageUsed = (100 * ActualHeat) / sorted[i].MaxHeat;
                    double TotalCo2 = sorted[i].Co2 * (PercentageUsed / 100);
                    ActualHeat = 0;
                    puResults2.Add((sorted[i].Id, sorted[i].Name, TotalHeat, PercentageUsed, TotalCo2, TotalCost));
                }
            }
            if (ActualHeat != 0)
            {
                Console.WriteLine($"\n=== A TOTAL OF {ActualHeat:F2}MW CANNOT BE SATISFIED ===");
            }

            foreach (var cost in puResults2)
            {
                TotalPrice += cost.TotalCost;
            }

            Console.WriteLine($"\n=== Sorted by Production Cost (Case {caseNumber}) ===");
            foreach (var item in puResults2)
            {
                Console.WriteLine($"> Id: {item.Id} - Name: {item.Name} - Percentage Of Use: {item.PercentageUsed:F2}%, TotalCO2: {item.TotalCo2:F2}kg/MWh, TotalHeat: {item.TotalHeat:F2}MW");
            }
            Console.WriteLine($"\n=== The Heat Demand for {hourlyData.TimeFrom} is {hourlyData.HeatDemand}MW  ===");
            Console.WriteLine($"\n=== The Electricity Price for {hourlyData.TimeFrom} is {hourlyData.ElectricityPrice} kr  ===");
            Console.WriteLine($"\n=== Total Spent (Case {caseNumber})= {TotalPrice} kr===");
        }

        public void RankByCO2Emissions(DateTime targetTime, int caseNumber)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }

            HashSet<int> selectedPUIds = GetSelectedPUIds(caseNumber);
            List<(string Name, double Result, double Co2)> puResults = new();

            foreach (var pu in AM.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
            {
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                double co2 = pu.Co2Emissions;
                puResults.Add((pu.Name, result, co2));
            }

            var sortedByCo2 = puResults.OrderBy(r => r.Co2).ToList();

            Console.WriteLine("\n=== Ranked by CO2 Emissions ===");
            foreach (var item in sortedByCo2)
            {
                Console.WriteLine($"> {item.Name} - CO2: {item.Co2}");
            }


        }

        public void CalculateAverageRanking(DateTime targetTime, int caseNumber)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }

            HashSet<int> selectedPUIds = GetSelectedPUIds(caseNumber);
            List<(string Name, double Result, double Co2)> puResults = new();

            foreach (var pu in AM.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
            {
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                double co2 = pu.Co2Emissions;
                puResults.Add((pu.Name, result, co2));
            }

            var sortedByCost = puResults.OrderBy(r => r.Result).ToList();
            var sortedByCO2 = puResults.OrderBy(r => r.Co2).ToList();

            Dictionary<string, int> costPositions = new();
            Dictionary<string, int> co2Positions = new();

            for (int i = 0; i < sortedByCost.Count; i++)
            {
                costPositions[sortedByCost[i].Name] = i + 1;
            }
            for (int i = 0; i < sortedByCO2.Count; i++)
            {
                co2Positions[sortedByCO2[i].Name] = i + 1;
            }

            List<(string Name, double AverageRank)> averageRanks = new();
            foreach (var pu in puResults)
            {
                int costRank = costPositions[pu.Name];
                int co2Rank = co2Positions[pu.Name];
                double avgRank = (costRank + co2Rank) / 2.0;
                averageRanks.Add((pu.Name, avgRank));
            }

            var sortedAverage = averageRanks.OrderBy(r => r.AverageRank).ToList();

            Console.WriteLine("\n=== Average Ranking ===");
            foreach (var item in sortedAverage)
            {
                Console.WriteLine($"> {item.Name} - Average Rank: {item.AverageRank:F2}");
            }
        }

        private HourlyData? GetHourlyData(DateTime targetTime)
        {
            if (SDM.WinterPeriod.ContainsKey(targetTime))
            {
                return SDM.WinterPeriod[targetTime];
            }
            else if (SDM.SummerPeriod.ContainsKey(targetTime))
            {
                return SDM.SummerPeriod[targetTime];
            }
            return null;
        }

        private HashSet<int> GetUserInputHashSet()
        {
            Console.WriteLine("Insert the ID of the machines separated by ',' (Example3: 1,3,4):");
            string input = Console.ReadLine(); // Leer la entrada del usuario

            // Convertir la entrada en un HashSet de enteros
            var inputArray = input.Split(',') // Separar la entrada por comas
                                   .Select(str => str.Trim()) // Eliminar espacios en blanco
                                   .Where(str => int.TryParse(str, out _)) // Filtrar solo números válidos
                                   .Select(int.Parse) // Convertir los strings a enteros
                                   .ToHashSet(); // Convertir a HashSet

            return inputArray;
        }
            return inputArray;
        }
}
}