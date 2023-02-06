using GraGieldowa.Data;
using GraGieldowa.ViewModels;
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
    public sealed partial class StockPositionHistory : Page
    {
        public StockPositionHistory()
        {
            try
            {
                this.InitializeComponent();
                ViewModel = new StockPositionHistoryViewModel();

                using var db = new ApplicationDbContext();
                var currentUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
                if (currentUserId != null)
                {
                    var currentUser = db.Users.Where(x => x.Id.ToString() == currentUserId).FirstOrDefault();

                    var historyPositions = db.ClosedPositions.Where(x => x.UserId.ToString() == currentUserId).ToList();
                    foreach (var postion in historyPositions)
                    {
                        var stockHistoryModel = new StockHistoryViewModel
                        {
                            StockName = postion.StockName,
                            Symbol = postion.Symbol,
                            BuyPrice = postion.OpenPrice.ToString(),
                            ClosePrice = postion.ClosePrice.ToString(),
                            ROIPercent = postion.ROIPercent.ToString() + "%",
                            NetProfit = postion.NetProfit.ToString(),
                            OpenDateTime = postion.OpenDate.ToString(),
                            CloseDateTime = postion.CloseDate.ToString(),
                            //Id = postion.Id,
                            Amount = postion.Amount,
                            //UserId = postion.UserId
                        };
                        ViewModel.HistoryStocks.Add(stockHistoryModel);
                    }

                    var userModel = new UserViewModel
                    {
                        UserName = currentUser.UserName,
                        Id = currentUser.Id,
                        AccountBalance = currentUser.AccountBalance.ToString()
                    };
                    ViewModel.CurrentUser = userModel;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public StockPositionHistoryViewModel ViewModel { get; }
    }
}
