namespace Tjugoett;

public class Round
{
    private const int MinPlayers = 1;
    private const int MaxPlayers = 5;
    private const int StartingCards = 2;

    private readonly Deck _deck;

    public Dealer Dealer { get; } = new();
    public IReadOnlyList<Player> Players { get; }

    public Round(IEnumerable<Player> players, Deck deck)
    {
        Players = players.ToList();

        if (Players.Count < MinPlayers || Players.Count > MaxPlayers)
        {
            throw new ArgumentException($"En runda kräver {MinPlayers}-{MaxPlayers} spelare.", nameof(players));
        }

        _deck = deck;
    }

    // Två kort till var och en, ett i taget. Givarens andra kort är hålkortet.
    public void Deal()
    {
        if (_deck.NeedsReshuffle)
        {
            _deck.Reset();
        }

        foreach (Player player in Players)
        {
            player.Hand.Clear();
        }

        for (int i = 0; i < StartingCards; i++)
        {
            foreach (Player player in Players)
            {
                player.Receive(_deck.Draw());
            }
            Dealer.Receive(_deck.Draw());
        }
    }

    public void Hit(Player player)
    {
        if (player.Hand.IsBust)
        {
            throw new InvalidOperationException("Spelaren har redan spräckt och kan inte köpa fler kort.");
        }

        player.Receive(_deck.Draw());
    }

    // Rundan spelas alltid färdigt, även om alla spelare har spräckt.
    public void PlayDealer()
    {
        Dealer.RevealHoleCard();

        while (Dealer.MustDraw)
        {
            Dealer.Receive(_deck.Draw());
        }
    }

    public Outcome GetOutcome(Player player)
    {
        Hand playerHand = player.Hand;
        Hand dealerHand = Dealer.Hand;

        if (playerHand.IsBust)
        {
            return Outcome.Lose;
        }
        if (playerHand.IsBlackjack && !dealerHand.IsBlackjack)
        {
            return Outcome.Blackjack;
        }
        if (dealerHand.IsBlackjack && !playerHand.IsBlackjack)
        {
            return Outcome.Lose;
        }
        if (dealerHand.IsBust)
        {
            return Outcome.Win;
        }

        if (playerHand.Value > dealerHand.Value)
        {
            return Outcome.Win;
        }
        if (playerHand.Value < dealerHand.Value)
        {
            return Outcome.Lose;
        }
        return Outcome.Push;
    }

    // Avgör alla spelare mot givaren och bokför resultatet på varje spelare.
    public IReadOnlyDictionary<Player, Outcome> Settle()
    {
        var outcomes = new Dictionary<Player, Outcome>();

        foreach (Player player in Players)
        {
            Outcome outcome = GetOutcome(player);
            player.Record(outcome);
            outcomes[player] = outcome;
        }

        return outcomes;
    }
}