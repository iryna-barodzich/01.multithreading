using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task4
{
    public interface IObservable
    {
        void Notify(string name, int count, bool isSeller);

    }
}
