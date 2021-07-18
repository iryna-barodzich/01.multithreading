using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task3
{
    public class DealRequest
    {
        public string Name { get; set; }

        public int NumberOfShares { get; set; }

        public string Dealer { get; set; }
    }
}
