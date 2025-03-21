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

        // Example: Perform calculation for a specific DateTime
        public void CalculateElectricityCosts(DateTime targetTime)
        {
            HourlyData? hourlyData = null;

            // Try to find ElectricityPrice for the given time from Winter or Summer data
            if (_sdm.WinterPeriod.ContainsKey(targetTime))
            {
                hourlyData = _sdm.WinterPeriod[targetTime];
                Console.WriteLine($"[Winter] Found data for {targetTime}, Electricity Price: {hourlyData.ElectricityPrice}");
            }
            else if (_sdm.SummerPeriod.ContainsKey(targetTime))
            {
                hourlyData = _sdm.SummerPeriod[targetTime];
                Console.WriteLine($"[Summer] Found data for {targetTime}, Electricity Price: {hourlyData.ElectricityPrice}");
            }
            else
            {
                Console.WriteLine($"No data found for {targetTime}");
                return;
            }


            // Create a temporary list to hold PU name and result
            List<(string Name, double Result)> puResults = new List<(string, double)>();

            foreach (var pu in _am.ProductionUnits)
            {
                Console.WriteLine($"PU Name: {pu.Name}");
                Console.WriteLine($"Production Cost: {pu.ProductionCost}");
                Console.WriteLine($"Electricity Production per MW: {pu.ElectricityProductionPerMW}");

                // Perform calculation (example formula):
                double result = pu.ProductionCost - (pu.ElectricityProductionPerMW * hourlyData.ElectricityPrice);
                Console.WriteLine($"Calculated Value: {result}");
                Console.WriteLine("----------------------------------");

                // Add to list
                puResults.Add((pu.Name, result));
            }

            // Sort the list by result (ascending order: best to worst)
            var sortedResults = puResults.OrderBy(r => r.Result).ToList();

            // Print ranking
            Console.WriteLine("\n=== Production Unit Ranking (Best to Worst) ===");
            for (int i = 0; i < sortedResults.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedResults[i].Name} - Result: {sortedResults[i].Result}");
            }
            Console.WriteLine("===============================================");
        }
    }
}