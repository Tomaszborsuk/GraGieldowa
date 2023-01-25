using System;

namespace GraGieldowa.Model
{
    public class ClosedPosition
    {
        public int Id { get; set; }

        public string Symbol { get; set; }

        public string StockName { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal NetProfit { get; set; }

        public decimal ROIPercent { get; set; }

        public int Amount { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public int UserId { get; set; }
    }
}
