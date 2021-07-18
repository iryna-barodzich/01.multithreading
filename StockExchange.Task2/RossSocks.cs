using System;

namespace StockExchange.Task2
{
    public class RossSocks : Player
    {
        public RossSocks()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return this.Mediator.SellOffer(stockName, numberOfShares, this);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return this.Mediator.BuyOffer(stockName, numberOfShares, this);
        }
    }
}
