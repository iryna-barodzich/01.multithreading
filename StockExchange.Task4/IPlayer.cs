using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task4
{
    public interface IPlayer
    {
        public void AddCount(object sender, Tuple<string, int, bool> tuple);
    }
}
