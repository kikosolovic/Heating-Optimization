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

namespace Heating_Optimization.Models
{
    public class OPT
    {
        private AM _am;
        private SDM _sdm;

        // Constructor gets instances of AM and SDM
        public OPT(AM am, SDM sdm)
        {
            _am = am;
            _sdm = sdm;
        }

        // Function to calculate production costs and print sorted list by cost
        public void SortByProductionCost(DateTime targetTime)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }

            List<(string Name, double Result, double Co2)> puResults = new List<(string, double, double)>();

            foreach (var pu in _am.ProductionUnits)
            {
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                double co2 = pu.Co2Emissions;
                puResults.Add((pu.Name, result, co2));
            }

            var sorted = puResults.OrderBy(r => r.Result).ToList();

            Console.WriteLine("\n=== Sorted by Production Cost ===");
            foreach (var item in sorted)
            {
                Console.WriteLine($"> {item.Name} - Cost Result: {item.Result:F2}, CO2: {item.Co2}");
            }
        }

        // Function to rank units by CO2 emissions and print
        public void RankByCO2Emissions(DateTime targetTime)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }

            List<(string Name, double Result, double Co2)> puResults = new List<(string, double, double)>();

            foreach (var pu in _am.ProductionUnits)
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
        public void CalculateAverageRanking(DateTime targetTime)
        {
            HourlyData? hourlyData = GetHourlyData(targetTime);
            if (hourlyData == null)
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }

            List<(string Name, double Result, double Co2)> puResults = new List<(string, double, double)>();

            foreach (var pu in _am.ProductionUnits)
            {
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                double co2 = pu.Co2Emissions;
                puResults.Add((pu.Name, result, co2));
            }

            var sortedByCost = puResults.OrderBy(r => r.Result).ToList();
            var sortedByCO2 = puResults.OrderBy(r => r.Co2).ToList();

            Dictionary<string, int> costPositions = new Dictionary<string, int>();
            Dictionary<string, int> co2Positions = new Dictionary<string, int>();

            for (int i = 0; i < sortedByCost.Count; i++)
            {
                costPositions[sortedByCost[i].Name] = i + 1;
            }
            for (int i = 0; i < sortedByCO2.Count; i++)
            {
                co2Positions[sortedByCO2[i].Name] = i + 1;
            }

            List<(string Name, double AverageRank)> averageRanks = new List<(string, double)>();
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
    }
}
