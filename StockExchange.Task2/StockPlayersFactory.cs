﻿namespace StockExchange.Task2
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var mediator = new Mediator();
            return new Players
            {
                RedSocks = new RedSocks { Mediator = mediator },
                Blossomers = new Blossomers { Mediator = mediator },
                RossSocks = new RossSocks { Mediator = mediator },
            };
        }
    }
}
