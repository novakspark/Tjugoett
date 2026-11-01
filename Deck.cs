namespace Tjugoett;

public class Deck
{
    public const int ReshuffleThreshold = 15;

    private readonly List<Card> _cards = new();

    public int Count => _cards.Count;
    public bool NeedsReshuffle => _cards.Count < ReshuffleThreshold;

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

    // Fisher-Yates: går bakifrån och byter varje kort med ett slumpat kort bland dem som ligger före (eller på samma plats).
    // Random.Shared är en enda Random som delas av hela programmet.
    private void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);
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