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
        private AM _am;
        private SDM _sdm;

        // Constructor gets instances of AM and SDM
        public OPT(AM am, SDM sdm)
        {
            _am = am;
            _sdm = sdm;
        }

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

        // Function to calculate production costs and print sorted list by cost
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

            foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
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
                    ActualHeat -= sorted[i].MaxHeat;
                    double TotalCost = sorted[i].MaxHeat * sorted[i].Result;
                    double PercentageUsed = 100;
                    double TotalCo2 = sorted[i].Co2;
                    double TotalHeat = sorted[i].MaxHeat;
                    puResults2.Add((sorted[i].Id, sorted[i].Name, TotalHeat, PercentageUsed, TotalCo2, TotalCost));
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
            if (ActualHeat!=0)
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

        // Function to rank units by CO2 emissions and print
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

            foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
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

        // Function to calculate average ranking between cost and CO2 emissions and print
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

            foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
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

        // Helper function to get HourlyData
        private HourlyData? GetHourlyData(DateTime targetTime)
        {
            if (_sdm.WinterPeriod.ContainsKey(targetTime))
            {
                return _sdm.WinterPeriod[targetTime];
            }
            else if (_sdm.SummerPeriod.ContainsKey(targetTime))
            {
                return _sdm.SummerPeriod[targetTime];
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
        public void GenerateCSVForAllCases()
{
    for (int caseNumber = 1; caseNumber <= 3; caseNumber++)
    {
        HashSet<int> selectedPUIds = caseNumber == 3 ? GetUserInputHashSet() : GetSelectedPUIds(caseNumber);
        string fileName = $"ProductionCost_Case{caseNumber}.csv";
        
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("Date;Period;Id;Name;TotalHeat;PercentageUsed;TotalCO2;TotalCost");

            // Collect all unique timestamps from both Winter and Summer periods
            HashSet<DateTime> allTimestamps = new HashSet<DateTime>(_sdm.WinterPeriod.Keys.Concat(_sdm.SummerPeriod.Keys));

            foreach (var timestamp in allTimestamps)
            {
                HourlyData? hourlyData = GetHourlyData(timestamp);
                if (hourlyData == null) continue;

                string period = _sdm.WinterPeriod.ContainsKey(timestamp) ? "Winter" : "Summer";
                
                List<(int Id, string Name, double TotalHeat, double PercentageUsed, double TotalCo2, double TotalCost)> results =
                    SortByProductionCostForCSV(timestamp, caseNumber, selectedPUIds);
                
                foreach (var item in results)
                {
                    writer.WriteLine($"{timestamp:dd-MM-yyyy HH:mm};{period};{item.Id};{item.Name};{item.TotalHeat:F2};{item.PercentageUsed:F2};{item.TotalCo2:F2};{item.TotalCost:F2}");
                }
            }
        }

        Console.WriteLine($"CSV file generated: {fileName}");
    }
}
private List<(int Id, string Name, double TotalHeat, double PercentageUsed, double TotalCo2, double TotalCost)> 
    SortByProductionCostForCSV(DateTime targetTime, int caseNumber, HashSet<int> selectedPUIds)
{
    HourlyData? hourlyData = GetHourlyData(targetTime);
    if (hourlyData == null)
    {
        Console.WriteLine($"No data found for {targetTime}");
        return new List<(int, string, double, double, double, double)>();
    }

    List<(int Id, string Name, double Result, double Co2, double MaxHeat)> puResults = new();
    List<(int Id, string Name, double TotalHeat, double PercentageUsed, double TotalCo2, double TotalCost)> puResults2 = new();

    foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id)))
    {
        double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
        double co2 = pu.Co2Emissions;
        puResults.Add((pu.Id, pu.Name, result, co2, pu.MaxHeat));
    }

    var sorted = puResults.OrderBy(r => r.Result).ToList();
    double ActualHeat = hourlyData.HeatDemand;

    for (int i = 0; i < sorted.Count; i++)
    {
        if (sorted[i].MaxHeat <= ActualHeat)
        {
            ActualHeat -= sorted[i].MaxHeat;
            double TotalCost = sorted[i].MaxHeat * sorted[i].Result;
            double PercentageUsed = 100;
            double TotalCo2 = sorted[i].Co2;
            double TotalHeat = sorted[i].MaxHeat;
            puResults2.Add((sorted[i].Id, sorted[i].Name, TotalHeat, PercentageUsed, TotalCo2, TotalCost));
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

    return puResults2;
}

    }
}