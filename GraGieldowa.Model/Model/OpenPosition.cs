using System;

namespace GraGieldowa.Model
{
    public class OpenPosition
    {
        public int Id { get; set; }

        public string Symbol { get; set; }

        public string StockName { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public DateTime OpenDate { get; set; }

        public int UserId { get; set; }
    }
}
