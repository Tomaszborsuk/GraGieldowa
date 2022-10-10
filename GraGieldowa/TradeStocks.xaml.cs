using GraGieldowa.Data;
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

            //Console.Read();
        }
    }
}
