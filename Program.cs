using Tjugoett;

static void Show(params Card[] cards)
{
    var hand = new Hand();
    foreach (var card in cards)
    {
        hand.Add(card);
    }
    Console.WriteLine($"{string.Join(", ", cards.Select(c => c.Rank))} => {hand.Value}, soft={hand.IsSoft}, bust={hand.IsBust}, blackjack={hand.IsBlackjack}");
}

Show(new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.King));
Show(new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.Ace), new Card(Suit.Clubs, Rank.Nine));
Show(new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.Six), new Card(Suit.Clubs, Rank.King));
Show(new Card(Suit.Hearts, Rank.King), new Card(Suit.Spades, Rank.Queen), new Card(Suit.Clubs, Rank.Five));
Show(new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.Ace));