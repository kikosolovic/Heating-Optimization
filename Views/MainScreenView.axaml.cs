using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Heating_Optimization.Models;
using System;
using System.Data;
using Avalonia.Controls.ApplicationLifetimes;
using Heating_Optimization.Views;
using Heating_Optimization.ViewModels;

namespace Heating_Optimization;

public partial class MainScreenView : UserControl
{
    public MainScreenView()
    {
        var optimizer = new OPT();
        InitializeComponent();
    }
}