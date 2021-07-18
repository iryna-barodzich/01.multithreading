using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchange.Task1
{
    public class Mediator
    {
        public Mediator()
        {
            this.SellRequests = new List<DealRequest>();
            this.BuyRequests = new List<DealRequest>();
        }
        public List<DealRequest> SellRequests { get; set; }

        public List<DealRequest> BuyRequests { get; set; }

        public bool SellOffer(string stockName, int numberOfShares, Player player)
        {
            var seller = player.GetType().Name;
            var buyRequests = BuyRequests.Where(b => b.Name == stockName && b.Dealer != seller);
            if (buyRequests.Any())
            {
                MakeSellDeal(buyRequests.First(), stockName, numberOfShares, seller);
                return true;
            } else
            {
                CreateSellOffer(stockName, numberOfShares, seller);
                return false;
            }
        }

        public bool BuyOffer(string stockName, int numberOfShares, Player player)
        {
            var buyer = player.GetType().Name;
            var sellRequests = SellRequests.Where(b => b.Name == stockName && b.Dealer != buyer);
            if (sellRequests.Any())
            {
                MakeSellDeal(sellRequests.First(), stockName, numberOfShares, buyer);
                return true;
            }
            else
            {
                CreateBuyOffer(stockName, numberOfShares, buyer);
                return false;
            }
        }

        private void CreateSellOffer(string stockName, int numberOfShares, string seller)
        {
            var request = this.SellRequests.Where(s => s.Name == stockName && s.Dealer == seller);
            if(request.Any())
            {
                request.First().NumberOfShares += numberOfShares;
            } else
            {
                SellRequests.Add(new DealRequest { Name = stockName, NumberOfShares = numberOfShares, Dealer = seller });
            }
        }

        private void CreateBuyOffer(string stockName, int numberOfShares, string buyer)
        {
            var request = this.BuyRequests.Where(s => s.Name == stockName && s.Dealer == buyer);
            if (request.Any())
            {
                request.First().NumberOfShares += numberOfShares;
            }
            else
            {
                BuyRequests.Add(new DealRequest { Name = stockName, NumberOfShares = numberOfShares, Dealer = buyer });
            }
        }

        private void MakeSellDeal(DealRequest buyRequest, string stockName, int numberOfShares, string seller)
        {
            var numberToBuy = Math.Min(buyRequest.NumberOfShares, numberOfShares);

            CutOffer(buyRequest, numberToBuy);

            if (numberToBuy < numberOfShares)
            {
                CreateSellOffer(stockName, numberOfShares - numberToBuy, seller);
            }
        }

        private void MakeBuyDeal(DealRequest sellRequest, string stockName, int numberOfShares, string seller)
        {
            var numberToSell = Math.Min(sellRequest.NumberOfShares, numberOfShares);

            CutOffer(sellRequest, numberToSell);

            if (numberToSell < numberOfShares)
            {
                CreateBuyOffer(stockName, numberOfShares - numberToSell, seller);
            }
        }

        private void CutOffer(DealRequest buyRequest, int numberOfShares)
        {
            buyRequest.NumberOfShares -= numberOfShares;
        }
    }
}
