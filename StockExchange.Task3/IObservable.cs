using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task3
{
    public interface IObservable
    {
        void AddObserver(IPlayer o);
        void RemoveObserver(IPlayer o);
        void NotifyBuyer(string name, int count);

        void NotifySeller(string name, int count);
    }
}
