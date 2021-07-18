namespace StockExchange.Task1
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var mediator = new Mediator();
            return new Players
            {
                RedSocks = new RedSocks { Mediator = mediator },
                Blossomers = new Blossomers() { Mediator = mediator }
            };
        }
    }
}
