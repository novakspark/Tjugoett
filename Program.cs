using Tjugoett;

var deck = new Deck();
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(deck.Draw());
}
Console.WriteLine($"Kvar i leken: {deck.Count}");
