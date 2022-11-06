using GraGieldowa.Data;
using GraGieldowa.Model;
using GraGieldowa.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserManagement : Page
    {
        public UserManagement()
        {
            this.InitializeComponent();
            ViewModel = new UserManagementViewModel();
            using var db = new ApplicationDbContext();
            var usersList = db.Users.ToList();
            var configUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
            if (configUserId != null)
            {
                var currentUser = db.Users.Where(x => x.Id.ToString() == configUserId).FirstOrDefault();
                var userModel = new UserViewModel
                {
                    AccountBalance = currentUser.AccountBalance.ToString(),
                    Id = currentUser.Id,
                    UserName = currentUser.UserName
                };
                ViewModel.CurrentUser = userModel;
            }

            foreach (var user in usersList)
            {
                var userModel = new UserViewModel
                {
                    AccountBalance = user.AccountBalance.ToString(),
                    Id = user.Id,
                    UserName = user.UserName
                };
                ViewModel.Users.Add(userModel);
            }
        }

        public UserManagementViewModel ViewModel { get; }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorText.Text = "";
                ErrorText.Foreground = new SolidColorBrush(Colors.Red);
                if (ViewModel.SelectedUser != null)
                {
                    var db = new ApplicationDbContext();
                    var editUserModel = db.Users.Where(x => x.Id == ViewModel.SelectedUser.Id).FirstOrDefault();
                    if (!String.IsNullOrEmpty(SelectedAccountBalance.Text))
                    {
                        decimal balance;
                        if (Decimal.TryParse(SelectedAccountBalance.Text, out balance))
                        {
                            editUserModel.AccountBalance = balance;
                        }
                        else
                        {
                            ErrorText.Text = "Podaj poprawny format salda użytkownika";
                        }
                    }
                    else
                    {
                        ErrorText.Text = "Podaj poprawne saldo użytkownika";
                    }

                    if (!String.IsNullOrEmpty(SelectedUserName.Text))
                    {
                        editUserModel.UserName = SelectedUserName.Text;
                    }
                    else
                    {
                        ErrorText.Text = "Podaj poprawną nazwę użytkownika";
                    }

                    var currentConfigUser = db.Configs.Where(x => x.Key == "UserId").FirstOrDefault();
                    if (currentConfigUser != null)
                    {
                        currentConfigUser.Value = editUserModel.Id.ToString();
                        db.Update(currentConfigUser);
                    }
                    else
                    {
                        var config = new Config();
                        config.Key = "UserId";
                        config.Value = editUserModel.Id.ToString();
                        db.Add(config);
                    }

                    if (ErrorText.Text == "")
                    {
                        ErrorText.Foreground = new SolidColorBrush(Colors.Green);
                        db.Update(editUserModel);
                        db.SaveChanges();
                        ErrorText.Text = "Poprawnie zmienione zalogowanego użytkownika";
                        var userModel = new UserViewModel
                        {
                            AccountBalance = editUserModel.AccountBalance.ToString(),
                            Id = editUserModel.Id,
                            UserName = editUserModel.UserName
                        };
                        ViewModel.CurrentUser = userModel;
                    }
                }
                else
                {
                    ErrorText.Text = "Musisz wybrać użytkownika do edycji";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AddUserDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new ApplicationDbContext();
                var newUser = new User();
                ErrorTextNewUser.Text = "";
                ErrorTextNewUser.Foreground = new SolidColorBrush(Colors.Red);
                if (!String.IsNullOrEmpty(UserName.Text))
                    newUser.UserName = UserName.Text;
                else
                {
                    ErrorTextNewUser.Text = "Podaj poprawną nazwę użytkownika";
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
                        ErrorTextNewUser.Text = "Podaj poprawny format salda początkowego";
                    }
                }
                else
                {
                    ErrorTextNewUser.Text = "Podaj poprawne saldo początkowe";
                }

                if (ErrorTextNewUser.Text == "")
                {
                    db.Add(newUser);
                    db.SaveChanges();

                    var currentConfigUser = db.Configs.Where(x => x.Key == "UserId").FirstOrDefault();
                    if (currentConfigUser != null)
                    {
                        currentConfigUser.Value = newUser.Id.ToString();
                        db.Update(currentConfigUser);
                    }
                    else
                    {
                        var config = new Config();
                        config.Key = "UserId";
                        config.Value = newUser.Id.ToString();
                        db.Add(config);
                    }
                    db.SaveChanges();

                    ErrorTextNewUser.Foreground = new SolidColorBrush(Colors.Green);
                    ErrorTextNewUser.Text = "Poprawnie Dodano nowego użytkownika";
                    var userModel = new UserViewModel
                    {
                        AccountBalance = newUser.AccountBalance.ToString(),
                        Id = newUser.Id,
                        UserName = newUser.UserName
                    };
                    ViewModel.CurrentUser = userModel;
                    ViewModel.Users.Add(userModel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorText.Text = "";
                ErrorText.Foreground = new SolidColorBrush(Colors.Red);
                if (ViewModel.SelectedUser != null)
                {
                    var db = new ApplicationDbContext();
                    var openPositions = db.OpenPositions.Where(x => x.UserId == ViewModel.SelectedUser.Id).ToList();
                    db.RemoveRange(openPositions);

                    var closedPositions = db.ClosedPositions.Where(x => x.UserId == ViewModel.SelectedUser.Id).ToList();
                    db.RemoveRange(closedPositions);

                    var currentConfigUser = db.Configs.Where(x => x.Key == "UserId").FirstOrDefault();
                    if (currentConfigUser.Value == ViewModel.SelectedUser.Id.ToString())
                    {
                        currentConfigUser.Value = db.Users.Select(x => x.Id.ToString()).FirstOrDefault();
                        db.Update(currentConfigUser);
                    }

                    var user = db.Users.Where(x => x.Id == ViewModel.SelectedUser.Id).FirstOrDefault();
                    db.Remove(user);

                    if (ErrorText.Text == "")
                    {
                        db.SaveChanges();
                        ErrorText.Foreground = new SolidColorBrush(Colors.Green);
                        ErrorText.Text = "Poprawnie usunięto użytkownika";
                        ViewModel.Users.Remove(ViewModel.SelectedUser);

                        var currentUser = db.Users.Where(x => x.Id.ToString() == currentConfigUser.Value).FirstOrDefault();
                        var userModel = new UserViewModel
                        {
                            AccountBalance = currentUser.AccountBalance.ToString(),
                            Id = currentUser.Id,
                            UserName = currentUser.UserName
                        };
                        ViewModel.CurrentUser = userModel;
                    }
                }
                else
                {
                    ErrorText.Text = "Musisz wybrać użytkownika do usunięcia";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
