namespace Tjugoett;

public class Hand
{
    private readonly List<Card> _cards = new();

    public IReadOnlyList<Card> Cards => _cards;
    public int Value => Calculate().Total;
    public bool IsSoft => Calculate().IsSoft;
    public bool IsBust => Value > 21;
    public bool IsBlackjack => _cards.Count == 2 && Value == 21;

    public void Add(Card card)
    {
        _cards.Add(card);
    }

    public void Clear()
    {
        _cards.Clear();
    }

    private (int Total, bool IsSoft) Calculate()
    {
        int total = 0;
        int aces = 0;

        foreach (Card card in _cards)
        {
            total += CardPoints(card.Rank);
            if (card.Rank == Rank.Ace)
            {
                aces++;
            }
        }

        // Alla ess räknas först som 11. Är summan över 21 görs ett ess till 1 (minus 10).
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        // Finns ett ess kvar som räknas som 11 är handen "mjuk".
        return (total, aces > 0);
    }

    private static int CardPoints(Rank rank) => rank switch
    {
        Rank.Ace => 11,
        Rank.Jack or Rank.Queen or Rank.King => 10,
        _ => (int)rank
    };
}