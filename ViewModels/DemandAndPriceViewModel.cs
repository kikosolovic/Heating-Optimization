using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Heating_Optimization.Models;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Heating_Optimization.ViewModels;

public partial class DemandAndPriceViewModel : ObservableObject
{
    public ObservableCollection<string> PeriodOptions { get; } = new ObservableCollection<string> { "Winter Period", "Summer Period" };

        // Fechas disponibles según el periodo
        [ObservableProperty]
        private ObservableCollection<DateTime> dateOptions = new ObservableCollection<DateTime>();

        [ObservableProperty]
        private string selectedPeriod;

        [ObservableProperty]
        private DateTime selectedDate;

        // Series de gráficas
        [ObservableProperty]
        private IEnumerable<ISeries> priceSeries;

        [ObservableProperty]
        private IEnumerable<ISeries> demandSeries;
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            LabelsRotation = 30,
            Labels = Enumerable.Range(0, 24).Select(h => $"{h:00}:00").ToArray()
        }
    };
    public Axis[] ElectricityYAxis { get; set; } =
    {
        new Axis { Name = "Electricity Price in DKK" }
    };
    public Axis[] HeatDemandYAxis { get; set; } =
    {
        new Axis { Name = "Heat Demand in MW" }
    };
    public DemandAndPriceViewModel()
        {
            SelectedPeriod = "Winter Period";
            UpdateDateOptions();
            UpdateCharts();
        }

        partial void OnSelectedPeriodChanged(string value)
        {
            UpdateDateOptions();
            UpdateCharts();
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            UpdateCharts();
        }

        private void UpdateDateOptions()
        {
            DateOptions.Clear();

            var periodDict = SelectedPeriod == "Winter Period" ? SDM.WinterPeriod : SDM.SummerPeriod;
            foreach (var date in periodDict.Keys.Select(k => k.Date).Distinct())
            {
                DateOptions.Add(date);
            }

            if (DateOptions.Count > 0)
                SelectedDate = DateOptions.First();
        }

        private void UpdateCharts()
        {
            var periodDict = SelectedPeriod == "Winter Period" ? SDM.WinterPeriod : SDM.SummerPeriod;
            var dataOfDay = periodDict.Values.Where(d => d.TimeFrom.Date == SelectedDate).OrderBy(d => d.TimeFrom);

            var electricityPrices = new double[24];
            var heatDemands = new double[24];
            foreach (var d in dataOfDay)
            {
                int hour = d.TimeFrom.Hour;
                electricityPrices[hour] = d.ElectricityPrice;
                heatDemands[hour] = d.HeatDemand;
            }
        // Precio electricidad
        PriceSeries = new[]
            {
                new LineSeries<double>
                {
                    Values = dataOfDay.Select(d => d.ElectricityPrice).ToArray(),
                    Name = "Electricity Price",
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Blue, 3)
                }
            };

            // Demanda calor
            DemandSeries = new[]
            {
                new LineSeries<double>
                {
                    Values = dataOfDay.Select(d => d.HeatDemand).ToArray(),
                    Name = "Heat Demand",
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Orange, 3)   
                }
            };
        }
}

