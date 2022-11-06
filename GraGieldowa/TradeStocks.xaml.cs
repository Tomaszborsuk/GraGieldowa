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
using Windows.ApplicationModel.Contacts.DataProvider;
using Windows.Foundation;
using Windows.Foundation.Collections;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GraGieldowa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TradeStocks : Page
    {
        private static Server serverData = Servers.DEMO;
        private static string userId = "11763324";

        public TradeStocks()
        {
            this.InitializeComponent();
            ViewModel = new TradeStockViewModel();

            using var db = new ApplicationDbContext();
            string password = db.Configs.Where(x => x.Key == "Password").Select(x => x.Value).FirstOrDefault();

            Console.WriteLine("Server address: " + serverData.Address + " port: " + serverData.MainPort + " streaming port: " + serverData.StreamingPort);

            // Connect to server
            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            //Console.WriteLine("Connected to the server");

            // Login to server
            Credentials credentials = new Credentials(userId, password, "", "YOUR APP NAME");

            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
            Console.WriteLine("Logged in as: " + userId);

            // Execute GetServerTime command
            ServerTimeResponse serverTimeResponse = APICommandFactory.ExecuteServerTimeCommand(connector, true);
            //Console.WriteLine("Server time: " + serverTimeResponse.TimeString);

            // Execute GetAllSymbols command
            AllSymbolsResponse allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(connector, true);
            Console.WriteLine("All symbols count: " + allSymbolsResponse.SymbolRecords.Count);

            var polishSymbols = allSymbolsResponse.SymbolRecords.Where(x => x.CurrencyProfit == "PLN" && x.CategoryName == "STC").ToList();
            foreach (var symbol in polishSymbols)
            {
                var stockModel = new StockViewModel
                {
                    Symbol = symbol.Symbol,
                    BuyPrice = symbol.Bid.ToString(),
                    Name = symbol.Description
                };
                ViewModel.Stocks.Add(stockModel);
            }

            var currentUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
            var currentUser = db.Users.Where(x => x.Id.ToString() == currentUserId).FirstOrDefault();

            var userModel = new UserViewModel
            {
                UserName = currentUser.UserName,
                Id = currentUser.Id,
                AccountBalance = currentUser.AccountBalance.ToString()
            };
            ViewModel.CurrentUser = userModel;
        }

        public TradeStockViewModel ViewModel { get; }

        private void BuyStockButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorText.Text = "";
                ErrorText.Foreground = new SolidColorBrush(Colors.Red);
                if (ViewModel.SelectedStock != null)
                {
                    var openPositionModel = new OpenPosition();
                    var db = new ApplicationDbContext();

                    var currentUserId = db.Configs.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
                    var currentUser = db.Users.Where(x => x.Id.ToString() == currentUserId).FirstOrDefault();
                    int userId;
                    if (int.TryParse(currentUserId, out userId))
                    {
                        openPositionModel.UserId = userId;
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z wczytaniem użytkownika";
                    }

                    openPositionModel.StockName = ViewModel.SelectedStock.Name;
                    openPositionModel.Symbol = ViewModel.SelectedStock.Symbol;

                    decimal stockPrice;
                    if (Decimal.TryParse(SelectedStockPrice.Text, out stockPrice))
                    {
                        openPositionModel.Price = stockPrice;
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z ceną akcji";
                    }

                    int noOfStocks;
                    if (int.TryParse(SelectedNumberOfStocks.Text, out noOfStocks))
                    {
                        openPositionModel.Amount = noOfStocks;
                    }
                    else
                    {
                        ErrorText.Text = "Wystąpił błąd z liczbą akcji do kupienia";
                    }

                    openPositionModel.OpenDate = DateTime.Now;
                    var buyPrice = noOfStocks * stockPrice;

                    if (buyPrice > currentUser.AccountBalance)
                    {
                        ErrorText.Text = "Nie masz wystarczających środków na zakup takiej ilości akcji";
                    }

                    bool ifAlreadyOwnedStock = db.OpenPositions.Any(x => x.Symbol == ViewModel.SelectedStock.Symbol && x.UserId == userId);
                    if(ifAlreadyOwnedStock)
                    {
                        var currentOpenPosition = db.OpenPositions.Where(x => x.Symbol == ViewModel.SelectedStock.Symbol && x.UserId == userId).FirstOrDefault();
                        var totalCost = buyPrice + (currentOpenPosition.Price * currentOpenPosition.Amount);
                        currentOpenPosition.Amount += noOfStocks;
                        currentOpenPosition.Price = Math.Round(totalCost / currentOpenPosition.Amount, 2);
                        db.Update(currentOpenPosition);
                    }

                    if (ErrorText.Text == "")
                    {
                        currentUser.AccountBalance -= buyPrice;

                        if (!ifAlreadyOwnedStock)
                            db.Add(openPositionModel);
                        db.Update(currentUser);
                        db.SaveChanges();
                        ErrorText.Foreground = new SolidColorBrush(Colors.Green);
                        ErrorText.Text = "Poprawnie zakupiono akcje";
                        ViewModel.CurrentUser.AccountBalance = currentUser.AccountBalance.ToString();
                    }
                }
                else
                {
                    ErrorText.Text = "Musisz wybrać akcję do kupienia";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
