using GraGieldowa.Data;
using GraGieldowa.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddUser : Page
    {
        public AddUser()
        {
            this.InitializeComponent();
        }

        private void AddUserDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new User();
                ErrorText.Text = "";
                if (!String.IsNullOrEmpty(UserName.Text))
                    newUser.UserName = UserName.Text;
                else
                {
                    ErrorText.Text = "Podaj poprawną nazwę użytkownika";
                }

                if (!String.IsNullOrEmpty(CurrentBalance.Text))
                {
                    decimal balance;
                    if (Decimal.TryParse(CurrentBalance.Text, out balance))
                    {
                        newUser.AccountBalance = balance;
                    }
                    else
                    {
                        ErrorText.Text = "Podaj poprawny format salda początkowego";
                    }
                }
                else
                {
                    ErrorText.Text = "Podaj poprawne saldo początkowe";
                }

                if (ErrorText.Text == "")
                {
                    var db = new ApplicationDbContext();
                    db.Add(newUser);
                    db.SaveChanges();
                    AddUserButton.Content = "Poprawnie Dodano nowego użytkownika";
                    AddUserButton.IsEnabled = false;
                    AddUserPage.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
