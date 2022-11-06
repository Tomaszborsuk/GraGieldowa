using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //ViewModel = new List<User>();
            //using (var db = new ApplicationDbContext())
            //{
            //    var usersList = db.Users.ToList();
            //    //UsersComboBox.DataContext = usersList;
            //    ViewModel = usersList;
            //}
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // you can also add items in code behind
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            //NavView.MenuItems.Add(new NavigationViewItem()
            //{ Content = "My content", Icon = new SymbolIcon(Symbol.Folder), Tag = "content" });

            // set the initial SelectedItem 
            //foreach (NavigationViewItemBase item in NavView.MenuItems)
            //{
            //    if (item is NavigationViewItem && item.Tag.ToString() == "HistoricalOperations")
            //    {
            //        NavView.SelectedItem = item;
            //        break;
            //    }
            //}
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(UserManagement));
            }
            else
            {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Trade":
                    ContentFrame.Navigate(typeof(TradeStocks));
                    break;

                case "Possession":
                    ContentFrame.Navigate(typeof(StockPossession));
                    break;

                case "HistoricalOperations":
                    ContentFrame.Navigate(typeof(StockPositionHistory));
                    break;

                case "UserManagement":
                    ContentFrame.Navigate(typeof(UserManagement));
                    break;
            }
        }

    }
}
