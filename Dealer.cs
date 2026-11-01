namespace Tjugoett;

public class Dealer : Participant
{
    private const int StandValue = 17;

    public bool IsHoleCardRevealed { get; private set; }

    // Första kortet är det öppna, andra kortet är hålkortet.
    public Card? UpCard => Hand.Cards.Count >= 1 ? Hand.Cards[0] : null;
    public Card? HoleCard => Hand.Cards.Count >= 2 ? Hand.Cards[1] : null;

    // Givarens fasta regel: köp på 16 eller lägre, stanna på 17 eller högre (även mjuk 17).
    public bool MustDraw => Hand.Value < StandValue;

    public Dealer() : base("Givaren")
    {
    }

    public void RevealHoleCard()
    {
        IsHoleCardRevealed = true;
    }
}