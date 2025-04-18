using Heating_Optimization.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Heating_Optimization.Views;

public partial class UnitList : UserControl
{
    public UnitList()
    {
        InitializeComponent();
        DataContext = new UnitListViewModel();


    }
}