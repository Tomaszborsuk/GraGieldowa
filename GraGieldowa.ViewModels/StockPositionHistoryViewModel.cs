using MvvmGen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraGieldowa.ViewModels
{
    [ViewModel]
    public partial class StockPositionHistoryViewModel
    {
        [Property]
        private UserViewModel _currentUser;
        partial void OnInitialize()
        {

        }
        public ObservableCollection<StockHistoryViewModel> HistoryStocks { get; } = new();
    }

    [ViewModel]
    public partial class StockHistoryViewModel
    {
        [Property] private string _stockName;
        [Property] private string _symbol;
        [Property] private string _buyPrice;
        [Property] private string _closePrice;
        [Property] private string _ROIPercent;
        [Property] private string _netProfit;
        [Property] private string _openDateTime;
        [Property] private string _closeDateTime;
        [Property] private int _amount;
    }
}
