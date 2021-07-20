namespace StockExchange.Task3
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
            mediator.AddObserver(players.RedSocks);
            mediator.AddObserver(players.Blossomers);
            mediator.AddObserver(players.RossSocks);

            return players;
        }
    }
}
