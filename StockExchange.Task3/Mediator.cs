using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchange.Task3
{
    public class Mediator : IObservable
    {
        private List<IPlayer> observers;
        public Mediator()
        {
            this.SellRequests = new List<DealRequest>();
            this.BuyRequests = new List<DealRequest>();
            this.observers = new List<IPlayer>();
        }

        public void AddObserver(IPlayer o)
        {
            observers.Add(o);
        }

        public List<DealRequest> SellRequests { get; set; }

        public List<DealRequest> BuyRequests { get; set; }

        public void RemoveObserver(IPlayer o)
        {
            observers.Remove(o);
        }

        public void NotifyBuyer(string name, int count)
        {
            foreach (IPlayer observer in observers)
            {
                observer.AddBoughtCount(name, count);
            }
        }

        public void NotifySeller(string name, int count)
        {
            foreach (IPlayer observer in observers)
            {
                observer.AddSoldCount(name, count);
            }
        }

        public bool SellOffer(string stockName, int numberOfShares, Player player)
        {
            var seller = player.GetType().Name;
            var buyRequests = BuyRequests.Where(b => b.Name == stockName && b.NumberOfShares == numberOfShares && b.Dealer != seller);
            if (buyRequests.Any())
            {
                NotifyBuyer(buyRequests.First().Dealer, numberOfShares);
                NotifySeller(seller, numberOfShares);
                BuyRequests.Remove(buyRequests.First());
                return true;
            } else
            {
                SellRequests.Add(new DealRequest { Name = stockName, NumberOfShares = numberOfShares, Dealer = seller });
                return false;
            }
        }

        public bool BuyOffer(string stockName, int numberOfShares, Player player)
        {
            var buyer = player.GetType().Name;
            var sellRequests = SellRequests.Where(b => b.Name == stockName && b.NumberOfShares == numberOfShares && b.Dealer != buyer);
            if (sellRequests.Any())
            {
                NotifyBuyer(buyer, numberOfShares);
                NotifySeller(sellRequests.First().Dealer, numberOfShares);
                SellRequests.Remove(sellRequests.First());
                return true;
            }
            else
            {
                BuyRequests.Add(new DealRequest { Name = stockName, NumberOfShares = numberOfShares, Dealer = buyer });
                return false;
            }
        }
    }
}
