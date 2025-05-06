using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heating_Optimization.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Heating_Optimization;

public partial class MainViewModel : ObservableObject
{

    // All what is from the new windows
    [ObservableProperty]
    private UserControl currentView;

    [RelayCommand]
    private void ShowMainScreen() => CurrentView = new MainScreenView();

    [RelayCommand]
    private void ShowProductionUnits() => CurrentView = new ProductionUnitsView();

    [RelayCommand]
    private void ShowDemandAndPrice() => CurrentView = new DemandAndPriceView();

    [RelayCommand]
    private void ShowAbout() => CurrentView = new About();
    //
    [ObservableProperty]
    private string selectedScenario;

    partial void OnSelectedScenarioChanged(string value)
    {
        switch (value)
        {
            case "Scenario 1":
                IsChecked1 = true;
                IsChecked2 = true;
                IsChecked3 = true;
                IsChecked4 = false;
                IsChecked5 = false;
                break;

            case "Scenario 2":
                IsChecked1 = true;
                IsChecked2 = false;
                IsChecked3 = true;
                IsChecked4 = true;
                IsChecked5 = true;
                break;

            case "Manual":
                break;
        }
    }
    private void UpdateScenario()
    {
        // Si coincide con Scenario 1
        if (IsChecked1 && IsChecked2 && IsChecked3 && !IsChecked4 && !IsChecked5)
        {
            SelectedScenario = "Scenario 1";
        }
        // Si coincide con Scenario 2
        else if (IsChecked1 && !IsChecked2 && IsChecked3 && IsChecked4 && IsChecked5)
        {
            SelectedScenario = "Scenario 2";
        }
        // Si no coincide con ninguno, Manual
        else
        {
            SelectedScenario = "Manual";
        }
    }
    public ObservableCollection<string> ScenarioOptions { get; } = new()
{
    "Scenario 1",
    "Scenario 2",
    "Manual"
};

    [ObservableProperty]
    private HashSet<int> selectedMachines = new();

    [ObservableProperty]
    private bool isChecked1;
    partial void OnIsChecked1Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((0, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked2;
    partial void OnIsChecked2Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((1, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked3;
    partial void OnIsChecked3Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((2, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked4;
    partial void OnIsChecked4Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((3, value));
        UpdateScenario();
        UpdateChart();
    }

    [ObservableProperty]
    private bool isChecked5;
    partial void OnIsChecked5Changed(bool value)
    {
        ToggleMachineSelectionCommand.Execute((4, value));
        UpdateScenario();
        UpdateChart();
    }

    [RelayCommand]
    private void ToggleMachineSelection((int id, bool isSelected) param)
    {
        if (param.isSelected)
            SelectedMachines.Add(param.id);
        else
            SelectedMachines.Remove(param.id);

        // Notificar cambio del HashSet (por si estás bindeando a otra cosa)
        OnPropertyChanged(nameof(SelectedMachines));
    }
    private readonly OPT _optimizer;

    public MainViewModel(OPT optimizer)
    {

        _optimizer = optimizer;
        SelectedScenario = "Scenario 1";
        AllData = LoadCsv("ProductionCost_Selected.csv");

        var uniqueDates = AllData.Select(d => d.Day).Distinct();
        foreach (var date in uniqueDates)
            AvailableDates.Add(date);

        SelectedDate = AvailableDates.FirstOrDefault();
        SelectedMetric = AvailableMetrics.FirstOrDefault();

        UpdateChart();
        ShowMainScreen();
    }


    [RelayCommand]
    private void GenerateCsvOnChange()
    {
        string fileName = $"ProductionCost_Selected.csv";

        if (SelectedMachines.Count == 0)
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


        _optimizer.GenerateCSVForSelectedMachines(SelectedMachines, fileName);

        Console.WriteLine($"CSV successfully generated: {fileName}");
    }



    [ObservableProperty]
    private List<Axis> yAxis = new()
{

    new Axis {Name = "Values"}
};

    [ObservableProperty]
    private List<Axis> xAxis = new();
    public ObservableCollection<string> AvailableDates { get; } = new();
    public ObservableCollection<string> AvailableMetrics { get; } =
        new() { "Total CO²", "Total Cost", "Percentage of PU Used" };

    [ObservableProperty]
    private string selectedDate;

    [ObservableProperty]
    private string selectedMetric;

    [ObservableProperty]
    private ObservableCollection<ISeries> seriesCollection = new();

    [ObservableProperty]
    private List<string> labels;

    public List<ProductionData> AllData { get; set; }




    partial void OnSelectedDateChanged(string value) => UpdateChart();
    partial void OnSelectedMetricChanged(string value) => UpdateChart();

    private void UpdateChart()
    {
        GenerateCsvOnChange();
        AllData = LoadCsv("ProductionCost_Selected.csv");
        if (SelectedDate == null || SelectedMetric == null) return;

        var dataForDate = AllData.Where(d => d.Day == SelectedDate).ToList();
        var names = dataForDate.Select(d => d.Name).Distinct();

        SeriesCollection.Clear();

        var hours = dataForDate.OrderBy(d => d.Date)
                           .Select(d => d.Hour)
                           .Distinct()
                           .ToList();

        var totalByHour = hours.ToDictionary(h => h, h => 0.0);

        foreach (var name in names)
        {
            var dataPoints = dataForDate.Where(d => d.Name == name)
                                        .OrderBy(d => d.Date)
                                        .Select(d => SelectedMetric switch
                                        {
                                            "Total CO²" => d.TotalCO2,
                                            "Total Cost" => d.TotalCost,
                                            "Percentage of PU Used" => d.PercentageUsed,
                                            _ => 0
                                        }).ToList();
            for (int i = 0; i < dataPoints.Count; i++)
            {
                var hour = hours[i];
                totalByHour[hour] += dataPoints[i];
            }

            SeriesCollection.Add(new LineSeries<double>
            {
                Name = name,
                Values = new ObservableCollection<double>(dataPoints)
            });
        }
        var totalValues = hours.Select(h => totalByHour[h]).ToList();

        SeriesCollection.Add(new LineSeries<double>
        {
            Name = "Total",
            Values = new ObservableCollection<double>(totalValues),
            Stroke = new SolidColorPaint(new SKColor(220, 20, 60)) { StrokeThickness = 2 },
            Fill = null,
            GeometrySize = 0
        });

        YAxis.Clear();
        YAxis.Add(new Axis
        {
            Name = SelectedMetric
        });

        XAxis.Clear();
        XAxis.Add(new Axis
        {
            Name = "Time",
            Labels = dataForDate.OrderBy(d => d.Date)
                                .Select(d => d.Hour)
                                .Distinct()
                                .ToList()
        });
    }

    private List<ProductionData> LoadCsv(string path)
    {
        if (!File.Exists(path)) return new List<ProductionData>();

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
        return data;
    }

}