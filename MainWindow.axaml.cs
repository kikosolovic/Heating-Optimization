using System;
using Avalonia.Controls;
using Heating_Optimization.Models;
using System.Globalization;

namespace Heating_Optimization;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        OPT opt = new OPT();
        InitializeComponent();
       // opt.GenerateCSVForAllCases();
        
    }
}  

// 01/03/2024 09:00   - ejemplo de fecha que sirve
