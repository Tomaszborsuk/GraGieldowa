using MvvmGen;
using System.Collections.ObjectModel;

namespace GraGieldowa.ViewModels
{
    [ViewModel]
    public partial class StockPossessionViewModel
    {
        [Property]
        private UserViewModel _currentUser;
        [Property]
        private OpenStockViewModel _selectedOpenStock;
        partial void OnInitialize()
        {

        }
        public ObservableCollection<OpenStockViewModel> OpenStocks { get; } = new();
    }

    [ViewModel]
    public partial class OpenStockViewModel
    {
        [Property] private string _stockName;
        [Property] private string _symbol;
        [Property] private string _buyPrice;
        [Property] private string _currentPrice;
        [Property] private string _percentChange;
        [Property] private string _openDateTime;
        [Property] private int _id;
        [Property] private int _amount;
        [Property] private int _userId;
    }
}
