using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using Heating_Optimization.ViewModels;

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
            var mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
        else
        {
            ShowCustomMessageBox();
        }


    }
}