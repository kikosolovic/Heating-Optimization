using System;
using System.Linq;
using Heating_Optimization.Data;


namespace Heating_Optimization.ViewModels;


public class LoginViewModel
{
    public bool checkCredentials(string username, string password)
    {

        var db = new AppDbContext();
        var user = db.Users.FirstOrDefault(u => u.Name == username && u.Password == password);
        if (user == null) return false;
        return true;
    }


}
