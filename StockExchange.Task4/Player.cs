using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task4
{
    public class Player : IPlayer
    {
        public Mediator Mediator { get; set; }

        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public void AddCount(object sender, Tuple<string, int, bool> tuple)
        {
            if (this.GetType().Name == tuple.Item1)
            {
                if (tuple.Item3)
                {
                    this.SoldShares += tuple.Item2;
                } else
                {
                    this.BoughtShares += tuple.Item2;
                }

            }
        }

        public void Subscribe()
        {
            Mediator.eventHandler += AddCount;
        }

        public void UnSubscribe()
        {
            Mediator.eventHandler -= AddCount;
        }
    }
}
