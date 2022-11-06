using MvvmGen;
using System.Collections.ObjectModel;

namespace GraGieldowa.ViewModels
{
    [ViewModel]
    public partial class TradeStockViewModel
    {
        [Property]
        private StockViewModel _selectedStock;
        [Property]
        private UserViewModel _currentUser;
        partial void OnInitialize()
        {
            
        }
        public ObservableCollection<StockViewModel> Stocks { get; } = new();
    }

    [ViewModel]
    public partial class StockViewModel
    {
        [Property] private string _name;
        [Property] private string _symbol;
        [Property] private string _buyPrice;
    }

    [ViewModel]
    public partial class UserViewModel
    {
        [Property] private string _userName;
        [Property] private string _accountBalance;
        [Property] private int _id;
    }

}