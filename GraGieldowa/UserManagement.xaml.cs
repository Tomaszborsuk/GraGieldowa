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
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserManagement : Page
    {
        public List<User> ViewModel { get; }
        public User CurrentUser { get; set; }

        public UserManagement()
        {
            this.InitializeComponent();
            ViewModel = new List<User>();
            CurrentUser = new User();
            using (var db = new ApplicationDbContext())
            {
                var usersList = db.Users.ToList();
                ViewModel = usersList;
                var configUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
                if (configUserId != null)
                {
                    CurrentUser = db.Users.Where(x => x.Id.ToString() == configUserId).FirstOrDefault();
                }
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            LocalFrame.Navigate(typeof(AddUser));
        }

        private void UserComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new ApplicationDbContext();
                var currentUser = (User)ListViewUsers.SelectedItem;
                var currentConfigUser = db.Configs.Where(x => x.Key == "UserId").FirstOrDefault();
                if (currentConfigUser != null)
                {
                    currentConfigUser.Value = currentUser.Id.ToString();
                    db.Update(currentConfigUser);
                }
                else
                {
                    var config = new Config();
                    config.Key = "UserId";
                    config.Value = currentUser.Id.ToString();
                    db.Add(config);
                }
                db.SaveChanges();
                CurrentUser = db.Users.Where(x => x.Id == currentUser.Id).FirstOrDefault();
                UsersPage.UpdateLayout();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
