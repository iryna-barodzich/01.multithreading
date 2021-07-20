﻿using System;

namespace StockExchange.Task3
{
    public class Blossomers: Player
    {
        public Blossomers() 
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
