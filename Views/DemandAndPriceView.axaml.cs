using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Heating_Optimization.Views;
using Heating_Optimization.ViewModels;

namespace Heating_Optimization;

public partial class DemandAndPriceView : UserControl
{
    public DemandAndPriceView()
    {
        InitializeComponent();
        DataContext = new DemandAndPriceViewModel();
    }
}