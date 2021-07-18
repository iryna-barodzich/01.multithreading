using System;

namespace StockExchange.Task1
{
    public class RedSocks : Player
    {
        public RedSocks()
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
