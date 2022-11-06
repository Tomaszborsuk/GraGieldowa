using GraGieldowa.Data;
using GraGieldowa.Model;
using GraGieldowa.ViewModels;
using Microsoft.UI;
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
using Windows.Services.Store;
using xAPI.Commands;
using xAPI.Responses;
using xAPI.Sync;
using static GraGieldowa.ViewModels.StockPossessionViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StockPossession : Page
    {
        private static Server serverData = Servers.DEMO;
        private static string userId = "11763324";

        public StockPossession()
        {
            try
            {
                this.InitializeComponent();
                ViewModel = new StockPossessionViewModel();

                using var db = new ApplicationDbContext();
                var currentUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
                var currentUser = db.Users.Where(x => x.Id.ToString() == currentUserId).FirstOrDefault();
                string password = db.Configs.Where(x => x.Key == "Password").Select(x => x.Value).FirstOrDefault();

                SyncAPIConnector connector = new SyncAPIConnector(serverData);
                Credentials credentials = new Credentials(userId, password, "", "YOUR APP NAME");
                LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
                AllSymbolsResponse allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(connector, true);
                var polishSymbols = allSymbolsResponse.SymbolRecords.Where(x => x.CurrencyProfit == "PLN" && x.CategoryName == "STC").ToList();

                var possessedStocks = db.OpenPositions.Where(x => x.UserId.ToString() == currentUserId).ToList();
                foreach (var stock in possessedStocks)
                {
                    var stockPossesionModel = new OpenStockViewModel
                    {
                        StockName = stock.StockName,
                        Amount = stock.Amount,
                        Symbol = stock.Symbol,
                        BuyPrice = stock.Price.ToString(),
                        PercentChange = ((Math.Round(polishSymbols.Where(x => x.CurrencyProfit == "PLN" && x.CategoryName == "STC" && x.Symbol == stock.Symbol && x.Description == stock.StockName).Select(x => (decimal)x.Bid).FirstOrDefault() / stock.Price * 100, 2)) - 100).ToString() + "%",
                        Id = stock.Id,
                        OpenDateTime = stock.OpenDate.ToString(),
                        UserId = stock.UserId,
                        CurrentPrice = polishSymbols.Where(x => x.CurrencyProfit == "PLN" && x.CategoryName == "STC" && x.Symbol == stock.Symbol && x.Description == stock.StockName).Select(x => x.Bid.ToString()).FirstOrDefault()
                    };
                    ViewModel.OpenStocks.Add(stockPossesionModel);
                }

                var userModel = new UserViewModel
                {
                    UserName = currentUser.UserName,
                    Id = currentUser.Id,
                    AccountBalance = currentUser.AccountBalance.ToString()
                };
                ViewModel.CurrentUser = userModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public StockPossessionViewModel ViewModel { get; }

        /// <summary>
        /// Metoda do sprzedaży wybranych akcji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SellStockButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorText.Text = "";
                ErrorText.Foreground = new SolidColorBrush(Colors.Red);
                if (ViewModel.SelectedOpenStock != null)
                {
                    var closedPositionModel = new ClosedPosition();

                    var db = new ApplicationDbContext();
                    var currentUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
                    var currentUser = db.Users.Where(x => x.Id.ToString() == currentUserId).FirstOrDefault();
                    int userId;
                    if (int.TryParse(currentUserId, out userId))
                    {
                        closedPositionModel.UserId = userId;
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z wczytaniem użytkownika";
                    }
                    var openPosition = db.OpenPositions.Where(x => x.Id == ViewModel.SelectedOpenStock.Id).FirstOrDefault();

                    closedPositionModel.StockName = ViewModel.SelectedOpenStock.StockName;
                    closedPositionModel.OpenDate = openPosition.OpenDate;
                    closedPositionModel.OpenPrice = openPosition.Price;
                    closedPositionModel.CloseDate = DateTime.Now;
                    closedPositionModel.Symbol = ViewModel.SelectedOpenStock.Symbol;
                    decimal currentPrice;
                    if (Decimal.TryParse(ViewModel.SelectedOpenStock.CurrentPrice, out currentPrice))
                    {
                        closedPositionModel.ClosePrice = currentPrice;
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z ceną zamknięcia akcji";
                    }

                    int noOfStocks;
                    if (int.TryParse(SelectedNumberOfStocksToSell.Text, out noOfStocks))
                    {
                        if(noOfStocks > openPosition.Amount)
                        {
                            ErrorText.Text = "Chcesz sprzedać więcej akcji niż masz w portfelu";
                        }
                        else if (noOfStocks <= 0)
                        {
                            ErrorText.Text = "Liczba akcji do sprzedaży musi być większa od 0";
                        }
                        else
                        {
                            closedPositionModel.Amount = noOfStocks;
                            openPosition.Amount -= noOfStocks;
                        }
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z liczbą akcji do sprzedania";
                        noOfStocks = 0;
                    }

                    closedPositionModel.NetProfit = (currentPrice - openPosition.Price) * noOfStocks;
                    closedPositionModel.ROIPercent = Math.Round((currentPrice / openPosition.Price) * 100, 2) - 100;


                    if (ErrorText.Text == "")
                    {
                        currentUser.AccountBalance += Math.Round(currentPrice * noOfStocks, 2);
                        if (openPosition.Amount == 0)
                        {
                            db.Remove(openPosition);
                            ViewModel.SelectedOpenStock.Amount = 0;
                            var openStockViewModel = ViewModel.OpenStocks.Where(x => x.Id == ViewModel.SelectedOpenStock.Id).FirstOrDefault();
                            ViewModel.OpenStocks.Remove(openStockViewModel);
                        }
                        else
                        {
                            db.Update(openPosition);
                            ViewModel.SelectedOpenStock.Amount = openPosition.Amount;
                            var openStockViewModel = ViewModel.OpenStocks.Where(x => x.Id == ViewModel.SelectedOpenStock.Id).FirstOrDefault();
                            openStockViewModel.Amount = openPosition.Amount;
                        }
                        db.Add(closedPositionModel);
                        db.Update(currentUser);
                        db.SaveChanges();
                        ErrorText.Text = "Poprawnie sprzedano akcje akcje";
                        ErrorText.Foreground = new SolidColorBrush(Colors.Green);
                        ViewModel.CurrentUser.AccountBalance = currentUser.AccountBalance.ToString();
                    }
                }
                else
                {
                    ErrorText.Text = "Musisz wybrać akcję do sprzedania";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
