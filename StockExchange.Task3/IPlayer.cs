using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task3
{
    public interface IPlayer
    {
        public Mediator Mediator { get; set; }

        public void AddSoldCount(int count);

        public void AddBoughtCount(int count);
    }
}
