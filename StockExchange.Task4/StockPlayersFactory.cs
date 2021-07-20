namespace StockExchange.Task4
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var mediator = new Mediator();
            var players = new Players
            {
                RedSocks = new RedSocks { Mediator = mediator },
                Blossomers = new Blossomers { Mediator = mediator },
                RossSocks = new RossSocks { Mediator = mediator },
            };

            players.RedSocks.Subscribe();
            players.Blossomers.Subscribe();
            players.RossSocks.Subscribe();

            return players;
        }
    }
}
