LicraM
licram
Invisible



Groovy está aquí. — 13/05/2021 12:18
Groovy
APP
 — 13/05/2021 12:18
Thanks for adding me to your server! 😊
To get started, join a voice channel and type /play to play a song! You can use song names, video links, and playlist links.

If you have any questions or need help with Groovy, click here to talk to our support team!

For exclusive features like volume control, 24/7 mode, audio effects, and saved queues, check out Groovy Premium.
LicraM — 08/10/2021 15:05
https://www.crunchyroll.com/es-es/hunter-x-hunter/episode-9-beware-x-of-x-prisoners-585056
LicraM — 19/12/2021 12:05
5651Mariio2
LicraM — 16/01/2022 19:19
mariot4680@gmail.com
LicraM — 15/03/2022 18:18
2385391802
333
LicraM — 14/12/2022 15:51
🤣
LicraM — 15/06/2023 9:58
Tema 2 (Álgebra): Factorizar y sistemas de ecuaciones e inecuaciones. Puede que nos lo pida en forma de problema.

Tema 3 (triángulos): seno, coseno y tangente. Los teoremas NO entran. Nos lo pedirá en forma de problema en plan el de saber la altura de un edificio o por el estilo.

Tema 7 (geometría analítica: Rectas

Tema 11 (Derivadas)
LicraM — 17/06/2023 3:05
https://fb.watch/lcM7xy4e9z/
LicraM — 10/11/2024 15:47
+---------------------------------------------------+
 |                       Program                      |
 +---------------------------------------------------+
 | + Main(): void                                     |
 +---------------------------------------------------+
                 |
Expandir
message.txt
4 KB
LicraM — 28/11/2024 14:45
using System;
using System.Collections.Generic;

namespace WasteHunters
{
    public class SortingGuideBooklet
Expandir
message.txt
7 KB
LicraM — 03/12/2024 22:34
IMPLEMENTARLO
public class Booklet
{
    private readonly Dictionary<string, string> sections;

    public Booklet()
    {
        sections = new Dictionary<string, string>
        {
            { "Introduction", "Welcome to Waste Hunters! Your mission is to manage waste responsibly and save the environment." },
            { "Commands", "Available commands: north, east, look, take, inventory, compost, booklet [section]." },
            { "Tips", "Tip: Recycling correctly helps reduce landfill waste. Compost organic material when possible." },
            { "SDG Goal", "This game promotes the 12th Sustainable Development Goal: Responsible consumption and production." }
        };
    }

    public string GetSection(string sectionName)
    {
        if (sections.ContainsKey(sectionName))
        {
            return sections[sectionName];
        }
        else
        {
            return $"Section '{sectionName}' not found in the booklet.";
        }
    }

    public string GetAllSections()
    {
        return string.Join("\n\n", sections.Select(s => $"### {s.Key} ###\n{s.Value}"));
    }
}
..............................................
public class Game
{
    private readonly Booklet booklet;

    public Game()
    {
        booklet = new Booklet();
        // Otras inicializaciones...
    }

    public void ProcessCommand(Command command)
    {
        string commandWord = command.Name.ToLower();
        switch (commandWord)
        {
            case "booklet":
                string section = command.SecondWord;
                if (string.IsNullOrEmpty(section))
                {
                    Console.WriteLine(booklet.GetAllSections());
                }
                else
                {
                    Console.WriteLine(booklet.GetSection(section));
                }
                break;

            // Otros comandos...

            default:
                Console.WriteLine("I don't know what you mean...");
                break;
        }
    }
}
................................................... LO DE ARRIBA ES PARA PONERLO EN GAME.CS
public class CommandWords
{
    public List<string> ValidCommands { get; private set; }

    public CommandWords()
    {
        ValidCommands = new List<string>
        {
            "north", "east", "look", "take", "inventory", "compost", "booklet"
        };
    }

    public bool IsValidCommand(string command)
    {
        return ValidCommands.Contains(command.ToLower());
    }
}
LicraM — 12/03/2025 18:13
Prueba
LicraM — 24/03/2025 13:26
TAREA JIRA
// File: Models/PU.cs
using System;

namespace Heating_Optimization.Models
{
    public class PU
Expandir
message.txt
5 KB
LicraM — ayer a las 13:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using System.Globalization;
Expandir
message.txt
9 KB
Optimizer automatico
LicraM — ayer a las 22:07
AHORA SI
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
                var pu = _am.ProductionUnits.First(p => p.Id == sorted[i].Id);

                if (ActualHeat > 0)
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
                    pu.IsOn = false;
                }
            }

            foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id) && !sorted.Any(s => s.Id == pu.Id)))
            {
                pu.IsOn = false;
            }

            if (ActualHeat != 0)
            {
                Console.WriteLine($"\n=== A TOTAL OF {ActualHeat:F2}MW CANNOT BE SATISFIED ===");
... (125 líneas restantes)
Contraer
message.txt
9 KB
﻿
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
                var pu = _am.ProductionUnits.First(p => p.Id == sorted[i].Id);

                if (ActualHeat > 0)
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
                    pu.IsOn = false;
                }
            }

            foreach (var pu in _am.ProductionUnits.Where(pu => selectedPUIds.Contains(pu.Id) && !sorted.Any(s => s.Id == pu.Id)))
            {
                pu.IsOn = false;
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
            string input = Console.ReadLine();
            var inputArray = input.Split(',')
                                   .Select(str => str.Trim())
                                   .Where(str => int.TryParse(str, out _))
                                   .Select(int.Parse)
                                   .ToHashSet();

            return inputArray;
        }
    }
}