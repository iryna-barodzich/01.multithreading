using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task3
{
    public class Player : IPlayer
    {
        public Mediator Mediator { get; set; }

        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public void AddSoldCount(int count)
        {
            this.SoldShares += count;
        }

        public void AddBoughtCount(int count)
        {
            this.BoughtShares += count;
        }
    }
}
