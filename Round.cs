namespace Tjugoett;

public class Round
{
    private const int MinPlayers = 1;
    private const int MaxPlayers = 5;

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

    // Två kort till var och en, ett i taget, spelarna först och givaren sist.
    public void Deal()
    {
        for (int i = 0; i < 2; i++)
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
            throw new InvalidOperationException("Spelaren är redan tjock och kan inte ta fler kort.");
        }

        player.Receive(_deck.Draw());
    }

    public void PlayDealer()
    {
        while (Dealer.MustDraw)
        {
            Dealer.Receive(_deck.Draw());
        }
    }

    public RoundResult GetResult(Player player)
    {
        Hand playerHand = player.Hand;
        Hand dealerHand = Dealer.Hand;

        if (playerHand.IsBust)
        {
            return RoundResult.Loss;
        }
        if (playerHand.IsBlackjack && !dealerHand.IsBlackjack)
        {
            return RoundResult.Blackjack;
        }
        if (dealerHand.IsBlackjack && !playerHand.IsBlackjack)
        {
            return RoundResult.Loss;
        }
        if (dealerHand.IsBust)
        {
            return RoundResult.Win;
        }

        if (playerHand.Value > dealerHand.Value)
        {
            return RoundResult.Win;
        }
        if (playerHand.Value < dealerHand.Value)
        {
            return RoundResult.Loss;
        }
        return RoundResult.Push;
    }
}