namespace Tjugoett;

public class Deck
{
    private readonly List<Card> _cards = new();
    private readonly Random _random = new();

    public int Count => _cards.Count;

    public Deck()
    {
        Reset();
    }

    public void Reset()
    {
        _cards.Clear();
        foreach (Suit suit in Enum.GetValues<Suit>())
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                _cards.Add(new Card(suit, rank));
            }
        }
        Shuffle();
    }

    // Fisher-Yates: går bakifrån och byter varje kort med ett slumpat kort före (eller på) samma plats
    private void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            Reset();
        }

        Card card = _cards[^1];
        _cards.RemoveAt(_cards.Count - 1);
        return card;
    }
}