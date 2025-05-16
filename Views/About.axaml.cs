using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

using Avalonia.Interactivity;

namespace Heating_Optimization;

public partial class About : UserControl
{
    public About()
    {
        InitializeComponent();
    }

    public void showData(object sender, RoutedEventArgs args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "ProductionCost_Selected.csv",
            UseShellExecute = true
        });
    }
}