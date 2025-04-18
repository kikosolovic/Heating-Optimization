using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Heating_Optimization.Views;

public partial class CustomMessageBox : Window
{
    public CustomMessageBox()
    {
        InitializeComponent();
    }

    private void OnOkClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
