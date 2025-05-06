using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using Heating_Optimization.ViewModels;
using Heating_Optimization.Models;

namespace Heating_Optimization.Views;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }
    private async void ShowCustomMessageBox()
    {
        var customMessageBox = new CustomMessageBox();
        await customMessageBox.ShowDialog(this);
    }
    public void verify(object sender, RoutedEventArgs args)
    {
        var username = this.username.Text;
        var password = this.password.Text;

        LoginViewModel log = new LoginViewModel();
        if (log.checkCredentials(username, password))
        {
            var optimizer = new OPT();
            var MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(optimizer),
            };
            MainWindow.Show();

            this.Close();
        }
        else
        {
            ShowCustomMessageBox();
        }


    }
}